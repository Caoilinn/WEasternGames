    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
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

        // disable attack input
        playerControlScript.attackEnabled = false;

        // disable movement input
        playerMovement.enableMovement = false;

        // play dialogue timeline
        objSys.playableDirector.playableAsset = objSys.blockingTutorialDialogue;
        objSys.StartCoroutine(this.PlayCutscene(objSys.blockingTutorialDialogue.duration));
    }

    public override void OnObjectiveCompleted()
    {
        // nothing needed
    }

    public void OnBlockButtonPressed() {
        // complete the objective
        // this.ObjectiveCompleted();

        // testing no follow up cutscene

        // reset text
        tutorialTextComponent.text = "";

        // untrack block button input
        objSys.playerInput.OnRightClickPressed -= OnBlockButtonPressed;

        // resume time
        objSys.timeManager.ChangeTimescale(1);

        objSys.StartCoroutine(this.CompleteObjectiveInSeconds(2));
    }

    public void OnDialogueFinishedPlaying() {
        // prompt the tutorial
        
        // set black background

        // stop time
        objSys.timeManager.ChangeTimescale(0);
        
        // track block button input
        objSys.playerInput.OnRightClickPressed += OnBlockButtonPressed;

        // set text
        tutorialTextComponent.color = new Color(1,1,1,1);
        tutorialTextComponent.text = "Hold the Right Mouse Button to Block";
    }

    private IEnumerator PlayCutscene(double duration) {
        objSys.playableDirector.Play();
        yield return new WaitForSeconds((float)duration);
        objSys.playableDirector.Stop();
        this.OnDialogueFinishedPlaying();
    }
}
