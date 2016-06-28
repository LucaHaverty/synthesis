using System;
using System.Runtime.InteropServices;
using Inventor;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Drawing;
using stdole;

namespace InventorAddInBrowserPaneAttempt5
{
    /// <summary>
    /// This is the primary AddIn Server class that implements the ApplicationAddInServer interface
    /// that all Inventor AddIns are required to implement. The communication between Inventor and
    /// the AddIn is via the methods on this interface.
    /// </summary>
    [GuidAttribute("55e5c0be-2fa4-4c95-a1f6-4782ea7a3258")]
    public class StandardAddInServer : Inventor.ApplicationAddInServer
    {

        // Inventor application object.
        private Inventor.Application m_inventorApplication;

      
        //button of adding tree view browserpublic 
        Inventor.ButtonDefinition m_TreeViewBrowser;
        //button of adding ActiveX browserpublic 
        Inventor.ButtonDefinition m_ActiveXBrowser;
        //button of starting or stopping BrowserEvents
        public Inventor.ButtonDefinition m_DoBrowserEvents;
        //BrowserEvents
        public Inventor.BrowserPanesEvents m_BrowserEvents;
        // no driver, motor, servo, bumper pneumatics, relay pneumatics, worm screw, dual motor
        Inventor.ButtonDefinition m_None;
        Inventor.ButtonDefinition m_Motor;
        Inventor.ButtonDefinition m_Servo;
        Inventor.ButtonDefinition m_BumperPneumatics;
        Inventor.ButtonDefinition m_RelayPneumatics;
        Inventor.ButtonDefinition m_WormScrew;
        Inventor.ButtonDefinition m_DualMotor;

        ComboBoxDefinition JointsComboBox;

        Inventor.ComboBoxDefinitionSink_OnSelectEventHandler SlotWidthComboBox_OnSelectEventDelegate;

        //HighlightSet
        public Inventor.HighlightSet oHighlight;
        public string m_ClientId;
        public UserControl1 m_ActiveX;
        public UserControl1 forthelolz;
        public int i;
        public StandardAddInServer()
        {
        }

        #region ApplicationAddInServer Members

        public void Activate(Inventor.ApplicationAddInSite addInSiteObject, bool firstTime)
        {
            m_ClientId = " ";
            i = 0;
            // This method is called by Inventor when it loads the addin.
            // The AddInSiteObject provides access to the Inventor Application object.
            // The FirstTime flag indicates if the addin is loaded for the first time.

            // Initialize AddIn members.
            m_inventorApplication = addInSiteObject.Application;
            forthelolz = new UserControl1();
            // TODO: Add ApplicationAddInServer.Activate implementation.
            // e.g. event initialization, command creation etc.

            int largeIconSize = 0;
            if (m_inventorApplication.UserInterfaceManager.InterfaceStyle == InterfaceStyleEnum.kRibbonInterface) {
             largeIconSize = 32;
            } else {
             largeIconSize = 24;
            }

           
            ControlDefinitions controlDefs = m_inventorApplication.CommandManager.ControlDefinitions;

            stdole.IPictureDisp smallPicture1 = AxHostConverter.ImageToPictureDisp(new Icon(@"C:\Users\t_gracj\Desktop\git\Exporter-Research\InventorAddInBrowserPaneAttempt5\InventorAddInBrowserPaneAttempt5\Resources\Icon1.ico").ToBitmap());
            stdole.IPictureDisp largePicture1 = AxHostConverter.ImageToPictureDisp(new Icon(@"C:\Users\t_gracj\Desktop\git\Exporter-Research\InventorAddInBrowserPaneAttempt5\InventorAddInBrowserPaneAttempt5\Resources\Icon1.ico").ToBitmap());
            m_TreeViewBrowser = controlDefs.AddButtonDefinition("HierarchyPane", "InventorAddInBrowserPaneAttempt5:HierarchyPane", CommandTypesEnum.kNonShapeEditCmdType, m_ClientId,null ,null, smallPicture1, largePicture1);
            m_TreeViewBrowser.OnExecute+= new ButtonDefinitionSink_OnExecuteEventHandler(m_TreeViewBrowser_OnExecute);

            stdole.IPictureDisp smallPicture2 = AxHostConverter.ImageToPictureDisp(new Icon(@"C:\Users\t_gracj\Desktop\git\Exporter-Research\InventorAddInBrowserPaneAttempt5\InventorAddInBrowserPaneAttempt5\Resources\Icon2.ico").ToBitmap());
            stdole.IPictureDisp largePicture2 = AxHostConverter.ImageToPictureDisp(new Icon(@"C:\Users\t_gracj\Desktop\git\Exporter-Research\InventorAddInBrowserPaneAttempt5\InventorAddInBrowserPaneAttempt5\Resources\Icon2.ico").ToBitmap());
            m_ActiveXBrowser = controlDefs.AddButtonDefinition("ActiveXPane", "InventorAddInBrowserPaneAttempt5:ActiveXPane", CommandTypesEnum.kNonShapeEditCmdType, m_ClientId, null ,null, smallPicture2, largePicture2);
            m_ActiveXBrowser.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(m_ActiveXBrowser_OnExecute);

            stdole.IPictureDisp smallPicture3 = AxHostConverter.ImageToPictureDisp(new Icon(@"C:\Users\t_gracj\Desktop\git\Exporter-Research\InventorAddInBrowserPaneAttempt5\InventorAddInBrowserPaneAttempt5\Resources\Icon3.ico").ToBitmap());
            stdole.IPictureDisp largePicture3 = AxHostConverter.ImageToPictureDisp(new Icon(@"C:\Users\t_gracj\Desktop\git\Exporter-Research\InventorAddInBrowserPaneAttempt5\InventorAddInBrowserPaneAttempt5\Resources\Icon3.ico").ToBitmap());
            m_DoBrowserEvents = controlDefs.AddButtonDefinition("DoBrowserEvents", "InventorAddInBrowserPaneAttempt5:DoBrowserEvents", CommandTypesEnum.kNonShapeEditCmdType, m_ClientId,null ,null, smallPicture3, largePicture3);
            m_DoBrowserEvents.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(m_DoBrowserEvents_OnExecute);

            m_None = controlDefs.AddButtonDefinition("No Driver", "InventorAddInBrowserPaneAttempt5:NoDriver", CommandTypesEnum.kNonShapeEditCmdType, m_ClientId, null, null);
            m_None.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(m_None_OnExecute);

            m_Motor = controlDefs.AddButtonDefinition("Motor", "InventorAddInBrowserPaneAttempt5:Motor", CommandTypesEnum.kNonShapeEditCmdType, m_ClientId, null, null);
            m_Motor.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(m_Motor_OnExecute);

            m_Servo = controlDefs.AddButtonDefinition("Servo", "InventorAddInBrowserPaneAttempt5:Servo", CommandTypesEnum.kNonShapeEditCmdType, m_ClientId, null, null);
            m_Servo.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(m_Servo_OnExecute);

            m_BumperPneumatics = controlDefs.AddButtonDefinition("Bumper Pneumatics", "InventorAddInBrowserPaneAttempt5:BumperPneumatics", CommandTypesEnum.kNonShapeEditCmdType, m_ClientId, null, null);
            m_BumperPneumatics.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(m_BumperPneumatics_OnExecute);

            m_RelayPneumatics = controlDefs.AddButtonDefinition("Relay Pneumatics", "InventorAddInBrowserPaneAttempt5:RelayPneumatics", CommandTypesEnum.kNonShapeEditCmdType, m_ClientId, null, null);
            m_RelayPneumatics.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(m_RelayPneumatics_OnExecute);

            m_WormScrew = controlDefs.AddButtonDefinition("Worm Screw", "InventorAddInBrowserPaneAttempt5:WormScrew", CommandTypesEnum.kNonShapeEditCmdType, m_ClientId, null, null);
            m_WormScrew.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(m_WormScrew_OnExecute);

            m_DualMotor = controlDefs.AddButtonDefinition("Dual Motor", "InventorAddInBrowserPaneAttempt5:DualMotor", CommandTypesEnum.kNonShapeEditCmdType, m_ClientId, null, null);
            m_DualMotor.OnExecute += new ButtonDefinitionSink_OnExecuteEventHandler(m_DualMotor_OnExecute);
            // Get the assembly ribbon.
            Inventor.Ribbon partRibbon = m_inventorApplication.UserInterfaceManager.Ribbons["Assembly"];
            // Get the "Part" tab.
            Inventor.RibbonTab partTab = partRibbon.RibbonTabs.Add("Robot Exporter", "BxD:RobotExporter", "{55e5c0be-2fa4-4c95-a1f6-4782ea7a3258}");
             Inventor.RibbonPanel partPanel = partTab.RibbonPanels.Add("Joints", "RobotExporter:Joints", "{55e5c0be-2fa4-4c95-a1f6-4782ea7a3258}");
            JointsComboBox = m_inventorApplication.CommandManager.ControlDefinitions.AddComboBoxDefinition("Joint choices", "Autodesk:SimpleAddIn:JointsChoiceBox", CommandTypesEnum.kShapeEditCmdType, 100, "{55e5c0be-2fa4-4c95-a1f6-4782ea7a3258}", "Slot width", "Slot width", Type.Missing, Type.Missing, ButtonDisplayEnum.kDisplayTextInLearningMode);
            JointsComboBox.AddItem("1 cm", 0);
            JointsComboBox.AddItem("2 cm", 0);
            JointsComboBox.AddItem("3 cm", 0);
            JointsComboBox.AddItem("4 cm", 0);
            JointsComboBox.AddItem("5 cm", 0);
            JointsComboBox.ListIndex = 1;
            JointsComboBox.ToolTipText = JointsComboBox.Text;
            JointsComboBox.DescriptionText = "Slot width: " + JointsComboBox.Text;
            partPanel.CommandControls.AddComboBox(JointsComboBox);
            partPanel.CommandControls.AddButton(m_TreeViewBrowser, true);
             partPanel.CommandControls.AddButton(m_DoBrowserEvents);
             partPanel.CommandControls.AddButton(m_ActiveXBrowser);
            partPanel.CommandControls.AddButton(m_None);
            partPanel.CommandControls.AddButton(m_Motor);
            partPanel.CommandControls.AddButton(m_Servo);
            partPanel.CommandControls.AddButton(m_BumperPneumatics);
            partPanel.CommandControls.AddButton(m_RelayPneumatics);
            partPanel.CommandControls.AddButton(m_WormScrew);
            partPanel.CommandControls.AddButton(m_DualMotor);
           
          

        }

        
        public void Deactivate()
        {
            // This method is called by Inventor when the AddIn is unloaded.
            // The AddIn will be unloaded either manually by the user or
            // when the Inventor session is terminated

            // TODO: Add ApplicationAddInServer.Deactivate implementation

            // Release objects.
            Marshal.ReleaseComObject(m_inventorApplication);
            m_inventorApplication = null;

            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void ExecuteCommand(int commandID)
        {
            // Note:this method is now obsolete, you should use the 
            // ControlDefinition functionality for implementing commands.
        }

        public object Automation
        {
            // This property is provided to allow the AddIn to expose an API 
            // of its own to other programs. Typically, this  would be done by
            // implementing the AddIn's API interface in a class and returning 
            // that class object through this property.

            get
            {
                // TODO: Add ApplicationAddInServer.Automation getter implementation
                return null;
            }
        }

        #endregion

        /// <summary>
        /// When [HierarchicalBrowser] button is clicked
        /// </summary>
        /// <param name="Context"></param>
        /// <remarks></remarks>

        private void m_TreeViewBrowser_OnExecute(Inventor.NameValueMap Context)
        {
            try {
                if (i == 0)
                {
                    Document oDoc = default(Document);
                    oDoc = m_inventorApplication.ActiveDocument;

                    BrowserPanes oPanes = default(BrowserPanes);
                    oPanes = oDoc.BrowserPanes;

                    //Create a standard Microsoft Windows IPictureDisp referencing an icon (.bmp) bitmap file.
                    //Change the file referenced here as appropriate - here the code references test.bmp.
                    //This is the icon that will be displayed at this node. Add the IPictureDisp to the client node resource.

                    ClientNodeResources oRscs = oPanes.ClientNodeResources;

                    stdole.IPictureDisp clientNodeIcon = AxHostConverter.ImageToPictureDisp(new Bitmap(@"C:\Users\t_gracj\Desktop\git\Exporter-Research\InventorAddInBrowserPaneAttempt5\InventorAddInBrowserPaneAttempt5\Resources\test.bmp"));

                    ClientNodeResource oRsc = oRscs.Add(m_ClientId, 1, clientNodeIcon);

                    BrowserNodeDefinition oDef = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("Top Node", 3, oRsc);
                 
                    //adding a new pane tab to the panes collection, define the top node the pane will contain.
                    Inventor.BrowserPane oPane = oPanes.AddTreeBrowserPane("My Pane", m_ClientId, oDef);

                    //Add two child nodes to the tree, labeled Node 2 and Node 3.
                    BrowserNodeDefinition oDef1 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("Node2", 5, oRsc);
                    BrowserNode oNode1 = oPane.TopNode.AddChild(oDef1);

                    BrowserNodeDefinition oDef2 = (BrowserNodeDefinition)oPanes.CreateBrowserNodeDefinition("Node3", 6, oRsc);
                    BrowserNode oNode2 = oPane.TopNode.AddChild(oDef2);

                    //Add the native node (from root)  of "Model" pane to the tree
                    BrowserNode oNativeRootNode = default(BrowserNode);
                    oNativeRootNode = oDoc.BrowserPanes["Model"].TopNode;

                    oPane.TopNode.AddChild(oNativeRootNode.BrowserNodeDefinition);
                    i++;
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
}

        /// <summary>
        /// when [ActiveXBrowser] button is clicked
        /// </summary>
        /// <param name="Context"></param>
        /// <remarks></remarks>


        public void m_None_OnExecute(Inventor.NameValueMap Context)
        {

        }
        public void m_Motor_OnExecute(Inventor.NameValueMap Context)
        {

        }
        public void m_Servo_OnExecute(Inventor.NameValueMap Context)
        {

        }
        public void m_BumperPneumatics_OnExecute(Inventor.NameValueMap Context)
        {

        }
        public void m_RelayPneumatics_OnExecute(Inventor.NameValueMap Context)
        {

        }
        public void m_WormScrew_OnExecute(Inventor.NameValueMap Context)
        {

        }
        public void m_DualMotor_OnExecute(Inventor.NameValueMap Context)
        {

        }
        private void m_ActiveXBrowser_OnExecute(Inventor.NameValueMap Context)
        {
            try
            {
                //get active document
                Document oDoc = m_inventorApplication.ActiveDocument;

                //get the BrowserPanes
                BrowserPanes oPanes = default(BrowserPanes);
                oPanes = oDoc.BrowserPanes;

                //add the BrowserPane with the control
                BrowserPane oPane = default(BrowserPane);
                //oPane = oPanes.Add("MyActiveXPane", "InventorAddInBrowserPaneAttempt5.UserControl1");
                oPane = oPanes.Add("MyActiveXPane", "InventorUtilities.Utilites.1");
                //get the control
                m_ActiveX = (UserControl1)oPane.Control;

                //call a method of the control
                m_ActiveX.DrawASketchRectangle(m_inventorApplication);

                //activate the BrowserPane
                oPane.Activate();
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        /// <summary>
        /// when [DoBrowserEvents] button is clicked.
        /// </summary>
        /// <param name="Context"></param>
        /// <remarks></remarks>
        private void m_DoBrowserEvents_OnExecute(Inventor.NameValueMap Context){
            try { 
                if (m_DoBrowserEvents.Pressed == false)
                {
                    MessageBox.Show("BrowserEvents Starts!");

                    m_DoBrowserEvents.Pressed = true;

                    m_BrowserEvents = m_inventorApplication.ActiveDocument.BrowserPanes.BrowserPanesEvents;

                }else{
                    MessageBox.Show("BrowserEvents Stops!");

                    m_DoBrowserEvents.Pressed = false;

                    m_BrowserEvents = null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// fire when custom node  is activated
        /// </summary>
        /// <param name="BrowserNodeDefinition"></param>
        /// <param name="Context"></param>
        /// <param name="HandlingCode"></param>
        /// <remarks></remarks>
        private void m_BrowserEvents_OnBrowserNodeActivate(object BrowserNodeDefinition, Inventor.NameValueMap Context, ref Inventor.HandlingCodeEnum HandlingCode)
        {
            MessageBox.Show ("OnBrowserNodeActivate");
        }

        /// <summary>
        /// delete custom nodes
        /// </summary>
        /// <param name="BrowserNodeDefinition"></param>
        /// <param name="BeforeOrAfter"></param>
        /// <param name="Context"></param>
        /// <param name="HandlingCode"></param>
        /// <remarks></remarks>
        private void m_BrowserEvents_OnBrowserNodeDeleteEntry(object BrowserNodeDefinition, Inventor.EventTimingEnum BeforeOrAfter, Inventor.NameValueMap Context, ref Inventor.HandlingCodeEnum HandlingCode)
        {
            MessageBox.Show("OnBrowserNodeDeleteEntry");
            //do deletion by the client

            if (BeforeOrAfter == EventTimingEnum.kAfter)
            {
                ClientBrowserNodeDefinition oBND = (ClientBrowserNodeDefinition)BrowserNodeDefinition;
                oBND.Delete();

            }
        }


        private void m_BrowserEvents_OnBrowserNodeGetDisplayObjects(object BrowserNodeDefinition, ref Inventor.ObjectCollection Objects, Inventor.NameValueMap Context, ref Inventor.HandlingCodeEnum HandlingCode)
        {
             PartDocument oPartDocument = m_inventorApplication.ActiveDocument as PartDocument;
             PartComponentDefinition oPartDef = oPartDocument.ComponentDefinition;

             if (oHighlight == null) {
              oHighlight = oPartDocument.CreateHighlightSet();
             } else {
              oHighlight.Clear();
             }

             Inventor.Color oColor = default(Inventor.Color);
             oColor = m_inventorApplication.TransientObjects.CreateColor(128, 22, 22);

             // Set the opacity
             oColor.Opacity = 0.8;
             oHighlight.Color = oColor;

             if (BrowserNodeDefinition is ClientBrowserNodeDefinition) {
              ClientBrowserNodeDefinition oClientB = (ClientBrowserNodeDefinition)BrowserNodeDefinition;
              //highlight all ExtrudeFeature
              if (oClientB.Label == "Node2") {
       
               foreach ( ExtrudeFeature oExtrudeF in oPartDef.Features.ExtrudeFeatures) {
                oHighlight.AddItem(oExtrudeF);
               }
              //highlight all RevolveFeature
              } else if (oClientB.Label == "Node3") {
         
               foreach ( RevolveFeature oRevolveF in oPartDef.Features.RevolveFeatures) {
                oHighlight.AddItem(oRevolveF);
               }
              }
             }

        }

        private void m_BrowserEvents_OnBrowserNodeLabelEdit(object BrowserNodeDefinition, string NewLabel, Inventor.EventTimingEnum BeforeOrAfter, Inventor.NameValueMap Context, ref Inventor.HandlingCodeEnum HandlingCode)
        {
            MessageBox.Show("OnBrowserNodeLabelEdit");
        }

    }

    //from http://blogs.msdn.com/b/andreww/archive/2007/07/30/converting-between-ipicturedisp-and-system-drawing-image.aspx

    internal class AxHostConverter : AxHost
    {
        private AxHostConverter()
            : base("")
        {
        }


        public static stdole.IPictureDisp ImageToPictureDisp(Image image)
        {
            return (stdole.IPictureDisp)GetIPictureDispFromPicture(image);
        }


        public static Image PictureDispToImage(stdole.IPictureDisp pictureDisp)
        {
            return GetPictureFromIPicture(pictureDisp);
        }
    }


}
