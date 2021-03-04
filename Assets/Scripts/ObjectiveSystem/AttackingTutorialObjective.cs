using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackingTutorialObjective : Objective
{
    private TextMeshProUGUI tutorialTextComponent;
    private PlayerControl playerControlScript;
    public AttackingTutorialObjective(ObjectiveSystem objSys) : base(objSys) {
        tutorialTextComponent = objSys.tutorialText.GetComponent<TextMeshProUGUI>();
        playerControlScript = objSys.playerObject.GetComponent<PlayerControl>();
    }

    public override void OnObjectiveCompleted()
    {
        tutorialTextComponent.text = "";

        //resume time
        objSys.timeManager.ChangeTimescale(1);

        // unregister events
        objSys.playerInput.OnLeftClickPressed -= OnAttackButtonPressed;
    }

    public override void OnObjectiveStart()
    {
        // enable combat 
        playerControlScript.attackEnabled = true;

        // register to input event
        objSys.playerInput.OnLeftClickPressed += OnAttackButtonPressed;

        //stop time
        objSys.timeManager.ChangeTimescale(0);

        tutorialTextComponent.text = "Press Left Click to Attack";
    }

    public void OnAttackButtonPressed() {
        this.ObjectiveCompleted();
    }
}
