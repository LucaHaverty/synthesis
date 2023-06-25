using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Synthesis.PreferenceManager;
using Synthesis.UI;
using Synthesis.UI.Dynamic;
using UnityEngine;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode

public class ChangeDrivetrainModal : ModalDynamic {

    public const float MODAL_WIDTH = 400f;

    public static float MODAL_HEIGHT
    {
        get
        {
            if (RobotSimObject.GetCurrentlyPossessedRobot() != null)
                return 100f;
            else
                return 70f;
        }
    }

    private RobotSimObject.DrivetrainType _selectedType;
    
    public Func<Button, Button> DisableButton = b =>
        b.StepIntoImage(i => i.SetColor(ColorManager.SYNTHESIS_BLACK_ACCENT))
            .StepIntoLabel(l => l.SetColor(ColorManager.SYNTHESIS_ORANGE_CONTRAST_TEXT))
            .DisableEvents<Button>();
        
    public Func<UIComponent, UIComponent> VerticalLayout = (u) => {
        var offset = (-u.Parent!.RectOfChildren(u).yMin) + 7.5f;
        u.SetTopStretch<UIComponent>(anchoredY: offset);
        return u;
    };

    public ChangeDrivetrainModal(): base(new Vector2(MODAL_WIDTH, MODAL_HEIGHT)) { }
    public override void Create()
    {
        bool robotExists = RobotSimObject.GetCurrentlyPossessedRobot() != null;
        
        Title.SetText("Change Drivetrain");

        if (robotExists)
        {
            Description.SetText("Select a drivetrain for this robot");

            AcceptButton.AddOnClickedEvent(b =>
            {
                RobotSimObject.GetCurrentlyPossessedRobot().ConfiguredDrivetrainType = _selectedType;
                DynamicUIManager.CloseActiveModal();
            });

            _selectedType = RobotSimObject.GetCurrentlyPossessedRobot().ConfiguredDrivetrainType;

            MainContent.CreateLabeledDropdown().SetTopStretch<LabeledDropdown>().StepIntoLabel(l => l.SetText("Type"))
                .StepIntoDropdown(d => d.SetOptions(RobotSimObject.DRIVETRAIN_TYPES.Select(x => x.Name).ToArray())
                    .AddOnValueChangedEvent(
                        (d, i, o) => _selectedType = RobotSimObject.DRIVETRAIN_TYPES[i])
                    .SetValue(_selectedType.Value));
        }
        else
        {
            AcceptButton.ApplyTemplate(DisableButton);
            Description.SetText("Spawn a robot to change it's drivetrain");
            
            var spacing = 15f;
            var button = MainContent.CreateButton("Spawn Robot")
                .ApplyTemplate<Button>(VerticalLayout)
                .AddOnClickedEvent(b => DynamicUIManager.CreateModal<AddRobotModal>())
                .StepIntoLabel(l => l.SetText("Spawn Robot"));
        }
    }
    public override void Update() { }
    public override void Delete() { }
}
