using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AI;

public class LockOnTutorialObjective : Objective
{
    private TextMeshProUGUI tutorialTextComponent;
    private FieldOfView tutorialEnemyFOV;
    public LockOnTutorialObjective(ObjectiveSystem objSys) : base(objSys) {
        tutorialTextComponent = objSys.tutorialText.GetComponent<TextMeshProUGUI>();
        tutorialEnemyFOV = objSys.tutorialEnemy.GetComponent<FieldOfView>();
    }

    public override void OnObjectiveCompleted()
    {
        // switch On the AI
        tutorialEnemyFOV.enabled = true;

        //resume time
        objSys.timeManager.ChangeTimescale(1);

        tutorialTextComponent.text = "";

        // unregister events
        objSys.playerInput.OnLockOnButtonPressed -= OnLockOnPressed;
    }

    public override void OnObjectiveStart()
    {
        // register to input event
        objSys.playerInput.OnLockOnButtonPressed += OnLockOnPressed;
        
        // stop time
        objSys.timeManager.ChangeTimescale(0);

        tutorialTextComponent.text = "Press F to lock on to an enemy";
    }

    public void OnLockOnPressed() {
        this.ObjectiveCompleted();
    }
}
