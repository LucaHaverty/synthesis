﻿using Synthesis.Input;
using Synthesis.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Synthesis.GUI
{
    class EmulationDriverStation : MonoBehaviour
    {
        public static EmulationDriverStation Instance { get; private set; }
        private bool VMConnectionCoroutineRunning = false;

        GameObject canvas;
        InputField gameSpecificMessage;
        GameObject emuDriverStationPanel;
        GameObject javaEmulationNotSupportedPopUp; // TODO remove this once support is added
        GameObject runButton;

        Text VMConnectionStatusMessage;

        Image VMConnectionStatusImage;
        Image runRobotCodeImage;
        Image enableRobotImage;
        Image disableRobotImage;

        // Sprites for emulation coloring details
        // Tethered in Unity > Simulator > Attached to the EmulationDriverStation script
        public Sprite HighlightColor;
        public Sprite DefaultColor;
        public Sprite EnableColor;
        public Sprite DisableColor;
        public Sprite StartEmulator;
        public Sprite EmulatorConnection;
        public Sprite StartCode;
        public Sprite StopCode;

        public void Start()
        {
            canvas = GameObject.Find("Canvas");
            emuDriverStationPanel = Auxiliary.FindObject(canvas, "EmulationDriverStation");
            javaEmulationNotSupportedPopUp = Auxiliary.FindObject(canvas, "JavaEmulationNotSupportedPopUp");

            gameSpecificMessage = Auxiliary.FindObject(canvas, "InputField").GetComponent<InputField>();

            runButton = Auxiliary.FindObject(canvas, "StartRobotCodeButton");
            runRobotCodeImage = Auxiliary.FindObject(canvas, "CodeImage").GetComponentInChildren<Image>();

            VMConnectionStatusMessage = Auxiliary.FindObject(canvas, "VMConnectionStatus").GetComponentInChildren<Text>();
            VMConnectionStatusImage = Auxiliary.FindObject(canvas, "VMConnectionStatusImage").GetComponentInChildren<Image>();

            enableRobotImage = Auxiliary.FindObject(canvas, "Enable").GetComponentInChildren<Image>();
            disableRobotImage = Auxiliary.FindObject(canvas, "Disable").GetComponentInChildren<Image>();

            RobotDisabled();
            StopRobotCode();
            BeginTrackingVMConnectionStatus();
        }

        public void Awake()
        {
            Instance = this;
        }

        public void DisableDrag()
        {
            DynamicCamera.MovementEnabled = false;
        }

        public void EnableDrag()
        {
            DynamicCamera.MovementEnabled = true;
        }

        public void Update()
        {
            InputControl.freeze = gameSpecificMessage.isFocused;
        }

        /// <summary>
        /// Opens the emulation driver station
        /// </summary>
        public void ToggleDriverStation()
        {
            if (emuDriverStationPanel.activeSelf == true) // Close it
            {
                emuDriverStationPanel.SetActive(false);
            }
            else // Open it
            {
                emuDriverStationPanel.SetActive(true);
                RobotState("teleop");
            }
        }

        /// <summary>
        /// Displays dialogue that Java emulation is not currently supproted (WPILib v2019)
        /// </summary>
        public void ShowJavaNotSupportedPopUp()
        {
            javaEmulationNotSupportedPopUp.SetActive(true);
        }

        /// <summary>
        /// Close dialogue that displays Java emulation is not currently supproted
        /// </summary>
        public void CloseJavaNotSupportedPopUp()
        {
            javaEmulationNotSupportedPopUp.SetActive(false);
        }

        /// <summary>
        /// Indicator for VM connection status
        /// </summary>
        private System.Collections.IEnumerator UpdateVMConnectionStatus()
        {
            while (true)
            {
                if (EmulatorManager.IsVMConnected())
                {
                    VMConnectionStatusImage.sprite = EmulatorConnection;
                    if (EmulatorNetworkConnection.Instance.IsConnected())
                        VMConnectionStatusMessage.text = "Connected";
                    else
                        VMConnectionStatusMessage.text = "Ready";
                }
                else if(EmulatorManager.IsVMRunning())
                {
                    VMConnectionStatusImage.sprite = EmulatorConnection;
                    VMConnectionStatusMessage.text = "Starting";
                    StopRobotCode();
                } else
                {
                    VMConnectionStatusImage.sprite = StartEmulator;
                    VMConnectionStatusMessage.text = "Start Emulator";
                    StopRobotCode();
                }
                yield return new WaitForSeconds(1.0f); // s
            }
        }

        public void BeginTrackingVMConnectionStatus()
        {
            if (!VMConnectionCoroutineRunning){
                StartCoroutine(UpdateVMConnectionStatus());
                VMConnectionCoroutineRunning = true;
            }
        }

        public void RunRobotCode()
        {
            if (EmulatorManager.IsFRCUserProgramPresent() && !EmulatorManager.IsRunningRobotCode())
            {
                runButton.GetComponentInChildren<Text>().text = "Stop Code";
                runRobotCodeImage.sprite = StopCode;
                EmulatorManager.StartRobotCode();
            }
        }

        public void StopRobotCode()
        {
            runButton.GetComponentInChildren<Text>().text = "Run Code";
            runRobotCodeImage.sprite = StartCode;
            if (EmulatorManager.IsRunningRobotCode())
                EmulatorManager.StopRobotCode();
            RobotDisabled();
        }

        /// <summary>
        /// Toggle button for run/stop code toolbar button
        /// </summary>
        public void ToggleRobotCodeButton()
        {
            if (EmulatorManager.IsFRCUserProgramPresent() && !EmulatorManager.IsRunningRobotCode())
            {
                RunRobotCode();
            }
            else
            {
                StopRobotCode();
            }
        }

        /// <summary>
        /// Selected state for the driver station
        /// </summary>
        /// <param name="theState"></param>
        public void RobotState(string theState)
        {
            switch (theState)
            {
                case "auto":
                    InputManager.Instance.RobotMode.Mode = EmulationService.RobotInputs.Types.RobotMode.Types.Mode.Autonomous;
                    GameObject.Find("TeleOp").GetComponent<Image>().sprite = DefaultColor;
                    GameObject.Find("Auto").GetComponent<Image>().sprite = HighlightColor;
                    GameObject.Find("Test").GetComponent<Image>().sprite = DefaultColor;
                    break;
                case "test":
                    InputManager.Instance.RobotMode.Mode = EmulationService.RobotInputs.Types.RobotMode.Types.Mode.Test;
                    GameObject.Find("TeleOp").GetComponent<Image>().sprite = DefaultColor;
                    GameObject.Find("Auto").GetComponent<Image>().sprite = DefaultColor;
                    GameObject.Find("Test").GetComponent<Image>().sprite = HighlightColor;
                    break;
                case "teleop":
                default:
                    InputManager.Instance.RobotMode.Mode = EmulationService.RobotInputs.Types.RobotMode.Types.Mode.Teleop;
                    GameObject.Find("TeleOp").GetComponent<Image>().sprite = HighlightColor;
                    GameObject.Find("Auto").GetComponent<Image>().sprite = DefaultColor;
                    GameObject.Find("Test").GetComponent<Image>().sprite = DefaultColor;
                    break;
            }
        }

        public void RobotEnabled()
        {
            if (EmulatorNetworkConnection.Instance.IsConnected())
            {
                InputManager.Instance.RobotMode.Enabled = true;
                enableRobotImage.sprite = EnableColor;
                disableRobotImage.sprite = DefaultColor;
            }
        }

        public void RobotDisabled()
        {
            InputManager.Instance.RobotMode.Enabled = false;
            enableRobotImage.sprite = DefaultColor;
            disableRobotImage.sprite = DisableColor;
        }

        /// <summary>
        /// Selected team alliance station
        /// </summary>
        /// <param name="teamStation"></param>
        public void TeamStation(int teamStation)
        {
            switch (teamStation)
            {
                case 1:
                    InputManager.Instance.MatchInfo.AllianceStationId = EmulationService.RobotInputs.Types.MatchInfo.Types.AllianceStationID.Red2;
                    break;
                case 2:
                    InputManager.Instance.MatchInfo.AllianceStationId = EmulationService.RobotInputs.Types.MatchInfo.Types.AllianceStationID.Red3;
                    break;
                case 3:
                    InputManager.Instance.MatchInfo.AllianceStationId = EmulationService.RobotInputs.Types.MatchInfo.Types.AllianceStationID.Blue1;
                    break;
                case 4:
                    InputManager.Instance.MatchInfo.AllianceStationId = EmulationService.RobotInputs.Types.MatchInfo.Types.AllianceStationID.Blue2;
                    break;
                case 5:
                    InputManager.Instance.MatchInfo.AllianceStationId = EmulationService.RobotInputs.Types.MatchInfo.Types.AllianceStationID.Blue3;
                    break;
                case 0:
                default:
                    InputManager.Instance.MatchInfo.AllianceStationId = EmulationService.RobotInputs.Types.MatchInfo.Types.AllianceStationID.Red1;
                    break;
            }
        }

        public void FinishGameSpecificMessage()
        {
            InputManager.Instance.MatchInfo.GameSpecificMessage = gameSpecificMessage.text;
        }

        public string GetGameSpecificMessage()
        {
            return gameSpecificMessage.text;
        }
    }
}
