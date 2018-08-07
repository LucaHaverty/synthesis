#include "Exporter.h"
#include <vector>
#include <Fusion/Fusion/Design.h>
#include <Fusion/Components/Component.h>
#include <Fusion/Components/JointMotion.h>
#include <Core/Application/Attributes.h>
#include <Core/Application/Attribute.h>
#include "Data/Filesystem.h"
#include "Data/BXDA/Mesh.h"
#include "Data/BXDA/SubMesh.h"
#include "Data/BXDA/Vertex.h"
#include "Data/BXDA/Surface.h"
#include "Data/BXDA/Triangle.h"
#include "Data/BXDJ/RigidNode.h"
#include "Data/BXDJ/Version.h"

using namespace Synthesis;

std::vector<Ptr<Joint>> Exporter::collectJoints(Ptr<FusionDocument> document)
{
	std::string stringifiedJoints;

	std::vector<Ptr<Joint>> allJoints = document->design()->rootComponent()->allJoints();
	std::vector<Ptr<Joint>> joints;

	for (int j = 0; j < allJoints.size(); j++)
	{
		Ptr<Joint> joint = allJoints[j];

		JointTypes type = joint->jointMotion()->jointType();

		// Check if joint is supported to have drivers
		if (type == JointTypes::RevoluteJointType ||
			type == JointTypes::SliderJointType ||
			type == JointTypes::CylindricalJointType)
			joints.push_back(joint);
	}

	return joints;
}

BXDJ::ConfigData Exporter::loadConfiguration(Ptr<FusionDocument> document)
{
	Ptr<Attribute> attr = document->attributes()->itemByName("Synthesis", "RobotConfigData.json");

	BXDJ::ConfigData config;
	if (attr != nullptr)
		config.fromJSONString(attr->value());

	config.filterJoints(collectJoints(document));

	return config;
}

void Exporter::saveConfiguration(BXDJ::ConfigData config, Ptr<FusionDocument> document)
{
	document->attributes()->add("Synthesis", "RobotConfigData.json", config.toJSONString());
}

void Exporter::exportExample()
{
	BXDA::Mesh mesh = BXDA::Mesh(Guid());
	BXDA::SubMesh subMesh = BXDA::SubMesh();

	// Face
	std::vector<BXDA::Vertex> vertices;
	vertices.push_back(BXDA::Vertex(Vector3<>(1, 2, 3), Vector3<>(1, 0, 0)));
	vertices.push_back(BXDA::Vertex(Vector3<>(4, 5, 6), Vector3<>(1, 0, 0)));
	vertices.push_back(BXDA::Vertex(Vector3<>(7, 8, 9), Vector3<>(1, 0, 0)));
	
	subMesh.addVertices(vertices);

	// Surface
	BXDA::Surface surface;
	std::vector<BXDA::Triangle> triangles;
	triangles.push_back(BXDA::Triangle(0, 1, 2));
	surface.addTriangles(triangles);
	surface.setColor(255, 16, 0);

	subMesh.addSurface(surface);
	mesh.addSubMesh(subMesh);

	//Generates timestamp and attaches to file name
	std::string filename = Filesystem::getCurrentRobotDirectory("example") + "exampleFusion.bxda";
	BXDA::BinaryWriter binary(filename);
	binary.write(mesh);
}

void Exporter::exportExampleXml()
{
	std::string filename = Filesystem::getCurrentRobotDirectory("example") + "exampleFusionXml.bxdj";
	BXDJ::XmlWriter xml(filename, false);
	xml.startElement("BXDJ");
	xml.writeAttribute("Version", "4.0.0");
	xml.startElement("Node");
	xml.writeAttribute("GUID", "0ba8e1ce-1004-4523-b844-9bfa69efada9");
	xml.writeElement("ParentID", "-1");
	xml.writeElement("ModelFileName", "node_0.bxda");
	xml.writeElement("ModelID", "Part2:1");
}

void Exporter::exportMeshes(BXDJ::ConfigData config, Ptr<FusionDocument> document, std::function<void(double)> progressCallback, bool * cancel)
{	
	if (progressCallback)
		progressCallback(0);

	// Generate tree
	Guid::resetAutomaticSeed();
	std::shared_ptr<BXDJ::RigidNode> rootNode = std::make_shared<BXDJ::RigidNode>(document->design()->rootComponent(), config);

	// List all rigid-nodes in tree
	std::vector<std::shared_ptr<BXDJ::RigidNode>> allNodes;
	allNodes.push_back(rootNode);
	rootNode->getChildren(allNodes, true);

	int totalOccurrences = 0; // Used for scaling the progress bar contribution of each node
	for (std::shared_ptr<BXDJ::RigidNode> node : allNodes)
		totalOccurrences += node->getOccurrenceCount();

	// Write robot to file
	Filesystem::createDirectory(Filesystem::getCurrentRobotDirectory(config.robotName));

	// Write BXDJ file
	std::string filenameBXDJ = Filesystem::getCurrentRobotDirectory(config.robotName) + "skeleton.bxdj";
	BXDJ::XmlWriter xml(filenameBXDJ, false);

	xml.startElement("BXDJ");

#ifdef BXDJ_4
	xml.writeAttribute("Version", "4.0.0");
#else
	xml.writeAttribute("Version", "3.0.0");
#endif

	xml.write(*rootNode);

	xml.startElement("DriveTrainType");
	xml.writeElement("DriveTrainTypeNumber", std::to_string((int)config.driveTrainType));
	xml.endElement();

	xml.endElement();

	// Write BXDA files
	int completedOccurrences = 0;
	for (int i = 0; i < allNodes.size(); i++)
	{
		// Prepare binary writer for writing mesh
		std::string filenameBXDA = "node_" + std::to_string(allNodes[i]->getGUID().getSeed()) + ".bxda";
		BXDA::BinaryWriter binary(Filesystem::getCurrentRobotDirectory(config.robotName) + filenameBXDA);

		// Generate mesh
		BXDA::Mesh mesh(allNodes[i]->getGUID());
		int occurrencesInNode = allNodes[i]->getOccurrenceCount();
		allNodes[i]->getMesh(mesh, false, [progressCallback, completedOccurrences, occurrencesInNode, totalOccurrences](double percent)
		{
			if (progressCallback)
				progressCallback((completedOccurrences + percent * occurrencesInNode) / totalOccurrences); // Scale percent of each node by the number of occurrences in that node
		}, cancel);
		completedOccurrences += occurrencesInNode;

		// Check if thread is being canceled
		if (cancel != nullptr)
			if (*cancel)
				break;

		// Write mesh
		binary.write(mesh);
	}

	// If canceled, delete the created robot folder
	if (cancel != nullptr)
		if (*cancel)
			return; // TODO: Delete the create robot directory to prevent corrupted robots from being available

	if (progressCallback)
		progressCallback(1);
}
