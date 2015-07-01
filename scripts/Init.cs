using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Init : MonoBehaviour
{
    // We will need these
    public const float PHYSICS_MASS_MULTIPLIER = 0.001f;

    private GUIController gui;

    private RigidNode_Base skeleton;
    private GameObject activeRobot;
	private GameObject cameraObject;
	private Camera camera;
	private Field field;

    private unityPacket udp = new unityPacket();
	private string filePath = BXDSettings.Instance.LastSkeletonDirectory + "\\";

    /// <summary>
    /// Frames before the robot gets reloaded, or -1 if no reload is queued.
    /// </summary>
    /// <remarks>
    /// This allows reloading the robot to be delayed until a "Loading" dialog can be drawn.
    /// </remarks>
    private volatile int reloadInFrames = -1;

    public Init()
    {
    }

    [STAThread]
    void OnGUI()
    {
        if (gui == null)
        {
            gui = new GUIController();

			gui.AddWindow ("Exit", new DialogWindow ("Exit?", "Yes", "No"), (object o) =>
				{
					if ((int) o == 1) {
						Application.Quit();
					}
				});

            gui.AddWindow("Load Model", new FileBrowser(), (object o) =>
            {
                string fileLocation = (string) o;
                // If dir was selected...
                if (File.Exists(fileLocation + "\\skeleton.bxdj"))
				{
                    fileLocation += "\\skeleton.bxdj";
				}
                DirectoryInfo parent = Directory.GetParent(fileLocation);
                if (parent != null && parent.Exists && File.Exists(parent.FullName + "\\skeleton.bxdj"))
                {
                    this.filePath = parent.FullName + "\\";
                    reloadInFrames = 2;
                }
                else
                {
                    UserMessageManager.Dispatch("Invalid selection!");
                }
            });

            gui.AddAction("Orient Robot", () =>
            {
                OrientRobot();
            });

            if (!File.Exists(filePath + "\\skeleton.bxdj"))
            {
                gui.DoAction("Load Model");
            }

			gui.AddWindow ("Switch View", new DialogWindow("Switch View",
			    "Driver Station [D]", "Orbit Robot [R]", "First Person [F]"), (object o) =>
			    {
					switch ((int) o) {
					case 0:
						camera.SwitchCameraState(new Camera.DriverStationState(camera));
						break;
					case 1:
						camera.SwitchCameraState(new Camera.OrbitState(camera));
						break;
					case 2:
						camera.SwitchCameraState(new Camera.FPVState(camera));
						break;
					default:
						Debug.Log("Camera state not found: " + (string) o);
						break;
					}
				});
        }
        gui.Render();

        if (reloadInFrames >= 0)
        {
            GUI.backgroundColor = new Color(1, 1, 1, 0.5f);
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Loading... Please Wait", gui.BlackBoxStyle);
        }
    }

    /// <summary>
    /// Repositions the robot so it is aligned at the center of the field, and resets all the
    /// joints, velocities, etc..
    /// </summary>
    private void OrientRobot()
    {
        if (activeRobot != null && skeleton != null)
        {
            var unityWheelData = new List<GameObject>();
            // Invert the position of the root object
            activeRobot.transform.localPosition = new Vector3(2.5f, 0f, -2.25f);
            activeRobot.transform.localRotation = Quaternion.identity;
            var nodes = skeleton.ListAllNodes();
            foreach (RigidNode_Base node in nodes)
            {
                UnityRigidNode uNode = (UnityRigidNode) node;
                uNode.unityObject.transform.localPosition = Vector3.zero;
                uNode.unityObject.transform.localRotation = Quaternion.identity;
                if (uNode.unityObject.rigidbody != null)
                {
                    uNode.unityObject.rigidbody.velocity = Vector3.zero;
                    uNode.unityObject.rigidbody.angularVelocity = Vector3.zero;
                }
                if (uNode.HasDriverMeta<WheelDriverMeta>() && uNode.wheelCollider != null)
                {
                    unityWheelData.Add(uNode.wheelCollider);
                }
            }
            if (unityWheelData.Count > 0)
            {
               auxFunctions.OrientRobot(unityWheelData, activeRobot.transform);
            }
        }
		camera.SwitchCameraState (new Camera.DriverStationState(camera));
    }

    private void TryLoad()
    {
        if (activeRobot != null)
        {
            skeleton = null;
            UnityEngine.Object.Destroy(activeRobot);
        }
        if (filePath != null && skeleton == null)
        {
            List<Collider> meshColliders = new List<Collider>();
            activeRobot = new GameObject("Robot");
            activeRobot.transform.parent = transform;

            List<RigidNode_Base> names = new List<RigidNode_Base>();
            RigidNode_Base.NODE_FACTORY = delegate()
            {
                return new UnityRigidNode();
            };
			//filePath = "C:\\Users\\t_buckm\\Documents\\Unity 4\\Synthesis\\Assets\\resources\\";
            skeleton = BXDJSkeleton.ReadSkeleton(filePath + "skeleton.bxdj");
			Debug.Log(filePath + "skeleton.bxdj");
            skeleton.ListAllNodes(names);
            foreach (RigidNode_Base node in names)
            {
                UnityRigidNode uNode = (UnityRigidNode) node;

                uNode.CreateTransform(activeRobot.transform);
                uNode.CreateMesh(filePath + uNode.modelFileName);
                uNode.CreateJoint();

                meshColliders.AddRange(uNode.unityObject.GetComponentsInChildren<Collider>());
            }

            {   // Add some mass to the base object
                UnityRigidNode uNode = (UnityRigidNode) skeleton;
                uNode.unityObject.transform.rigidbody.mass += 20f * PHYSICS_MASS_MULTIPLIER; // Battery'
                Vector3 vec = uNode.unityObject.rigidbody.centerOfMass;
                vec.y *= 0.9f;
                uNode.unityObject.rigidbody.centerOfMass = vec;
            }

            auxFunctions.IgnoreCollisionDetection(meshColliders);
            OrientRobot();
        }
        else
        {
            Debug.Log("unityWheelData is null...");
        }
        gui.guiVisible = false;
    }

    void Start()
    {
        Physics.gravity = new Vector3(0, -9.8f, 0);
        Physics.solverIterationCount = 15;
        Physics.minPenetrationForPenalty = 0.001f;

		cameraObject = GameObject.Find ("Camera");
		camera = cameraObject.GetComponent<Camera> ();

		/** /
		UnityRigidNode nodeThing = new UnityRigidNode();
		nodeThing.modelFileName = "field.bxda";
		nodeThing.CreateTransform(transform);
		nodeThing.CreateMesh(UnityEngine.Application.dataPath + "\\Resources\\field.bxda");
		nodeThing.unityObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		/**/

		/**/
		field = new Field ("field2015", new Vector3(0f, 0.58861f, 0f), new Vector3(0.2558918f, 0.2558918f, 0.2558918f));

		field.AddCollisionObjects (
			"GE-15025_0", "GE-15025_1", "GE-15025_2", "GE-15025_A",
			"GE-15000_A", "GE-15001_A", "GE-15000_0", "GE-15001_0"
		);

		BoxCollider floor = field.AddComponent<BoxCollider> ("floor");
		floor.center = new Vector3 (0f, -6.5f, 0f);
		floor.size = new Vector3 (36.00475f, 8.343518f, 86.43035f);

		BoxCollider blueDS = field.AddComponent<BoxCollider> ("blueDS");
		blueDS.center = new Vector3 (0f, 1.529588f, 33.50338f);
		blueDS.size = new Vector3 (21.74336f, 7.846722f, 2.357369f);

		BoxCollider redDS = field.AddComponent<BoxCollider> ("redDS");
		redDS.center = new Vector3 (0f, 1.529588f, -33.50338f);
		redDS.size = new Vector3 (21.74336f, 7.846722f, 2.357369f);

		BoxCollider step = field.AddComponent<BoxCollider> ("step");
		step.center = new Vector3 (0f, -2.022223f, 0f);
		step.size = new Vector3 (32.41857f, 0.7251982f, 2.498229f);

		BoxCollider leftSidePanels = field.AddComponent<BoxCollider> ("leftSidePanels");
		leftSidePanels.center = new Vector3 (-17.08714f, -1.359237f, 0f);
		leftSidePanels.size = new Vector3 (1.80665f, 2.039491f, 50.91413f);

		BoxCollider rightSidePanels = field.AddComponent<BoxCollider> ("rightSidePanels");
		rightSidePanels.center = new Vector3 (17.08714f, -1.359237f, 0f);
		rightSidePanels.size = new Vector3 (1.80665f, 2.039491f, 50.91413f);
		/**/

        reloadInFrames = 2;
    }

    void OnEnable()
    {
        udp.Start();
    }

    void OnDisable()
    {
        udp.Stop();
    }

    void Update()
    {
        if (reloadInFrames >= 0 && reloadInFrames-- == 0)
        {
            reloadInFrames = -1;
            TryLoad();
        }
    }

    void FixedUpdate()
    {
        if (skeleton != null)
        {
            unityPacket.OutputStatePacket packet = udp.GetLastPacket();
            DriveJoints.UpdateAllMotors(skeleton, packet.dio);
            DriveJoints.UpdateSolenoids(skeleton, packet.solenoid);
            List<RigidNode_Base> nodes = skeleton.ListAllNodes();
            InputStatePacket sensorPacket = new InputStatePacket();
            foreach (RigidNode_Base node in nodes)
            {
                if (node.GetSkeletalJoint() == null)
                    continue;
                foreach (RobotSensor sensor in node.GetSkeletalJoint().attachedSensors)
                {
                    if (sensor.type == RobotSensorType.POTENTIOMETER && node.GetSkeletalJoint() is RotationalJoint_Base)
                    {
                        UnityRigidNode uNode = (UnityRigidNode) node;
                        float angle = DriveJoints.GetAngleBetweenChildAndParent(uNode) + ((RotationalJoint_Base) uNode.GetSkeletalJoint()).currentAngularPosition;
                        sensorPacket.ai[sensor.module - 1].analogValues[sensor.port - 1] = (int) sensor.equation.Evaluate(angle);
                    }
                }
            }
            udp.WritePacket(sensorPacket);
        }
    }
}
