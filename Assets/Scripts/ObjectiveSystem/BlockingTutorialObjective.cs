using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AI;

public class BlockingTutorialObjective : Objective
{
    private TextMeshProUGUI tutorialTextComponent;
    private PlayerControl playerControlScript;
    private PlayerMovementV2 playerMovement;
    private FieldOfView tutorialEnemyFOV;
    public BlockingTutorialObjective(ObjectiveSystem objSys) : base(objSys)
    {
        tutorialTextComponent = objSys.tutorialText.GetComponent<TextMeshProUGUI>();
        playerControlScript = objSys.playerObject.GetComponent<PlayerControl>();
        playerMovement = objSys.playerObject.GetComponent<PlayerMovementV2>();
        tutorialEnemyFOV = objSys.tutorialEnemy.GetComponent<FieldOfView>();
    }

    public override void OnObjectiveStart()
    {
        // switch off the AI
        tutorialEnemyFOV.enabled = false;

        // play dialogue

        // track block button input
        objSys.playerInput.OnRightClickPressed += OnBlockButtonPressed;

        // disable attack input
        playerControlScript.attackEnabled = false;

        // disable movement input
        playerMovement.enableMovement = false;

        // stop time
        objSys.timeManager.ChangeTimescale(0);

        // set text
        tutorialTextComponent.text = "Hold the Right Mouse Button to Block";

        // set black background
    }

    public override void OnObjectiveCompleted()
    {
        // enable movement input
        playerMovement.enableMovement = true;
        
        // reset black background
    }

    public void OnBlockButtonPressed() {
        // reset text
        tutorialTextComponent.text = "";

        // untrack block button input
        objSys.playerInput.OnRightClickPressed -= OnBlockButtonPressed;

        // resume time
        objSys.timeManager.ChangeTimescale(1);

        // complete objective after a time testing
        objSys.StartCoroutine(this.CompleteObjectiveInSeconds(2));
    }
}
