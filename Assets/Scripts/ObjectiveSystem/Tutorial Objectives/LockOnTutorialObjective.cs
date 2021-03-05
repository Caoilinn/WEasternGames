using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using AI;

public class LockOnTutorialObjective : Objective
{
    private TextMeshProUGUI tutorialTextComponent;
    public LockOnTutorialObjective(ObjectiveSystem objSys) : base(objSys) {
        tutorialTextComponent = objSys.tutorialText.GetComponent<TextMeshProUGUI>();
    }

    public override void OnObjectiveStart()
    {

        objSys.StartCoroutine(PlayCutscene(objSys.lockOnTutorialDialogue));
    }

    public override void OnObjectiveCompleted()
    {
        //resume time
        objSys.timeManager.ChangeTimescale(1);

        tutorialTextComponent.text = "";

        // unregister events
        objSys.playerInput.OnLockOnButtonPressed -= OnLockOnPressed;
    }

    public void OnLockOnPressed() {
        this.ObjectiveCompleted();
    }

    public void OnDialogueFinishedPlaying() {
        // prompt the tutorial
        
        // set black background

        // register to input event
        objSys.playerInput.OnLockOnButtonPressed += OnLockOnPressed;
        
        // stop time
        objSys.timeManager.ChangeTimescale(0);

        // set text
        tutorialTextComponent.color = new Color(1,1,1,1);
        tutorialTextComponent.text = "Press F to lock on to an enemy";
    }
    
    private IEnumerator PlayCutscene(PlayableAsset cutscene) {
        float duration = (float)cutscene.duration;
        objSys.playableDirector.playableAsset = cutscene;
        objSys.playableDirector.Play();
        yield return new WaitForSeconds(duration);
        objSys.playableDirector.Stop();
        this.OnDialogueFinishedPlaying();
    }
}
