using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using AI;

public class MovementTutorialObjective : Objective
{
    TextMeshProUGUI tutorialTextComponent;
    EnemyWeaponCollision tutorialEnemyWeaponCollision;
    PlayerCollision playerCollision;
    PlayerMovementV2 playerMovement;
    FieldOfView tutorialEnemyFOV;
    public MovementTutorialObjective(ObjectiveSystem objSys) : base(objSys)
    {
        tutorialTextComponent = objSys.tutorialText.GetComponent<TextMeshProUGUI>();
        tutorialEnemyWeaponCollision = objSys.tutorialEnemyWeapon.GetComponent<EnemyWeaponCollision>();
        playerCollision = objSys.playerObject.GetComponent<PlayerCollision>();
        playerMovement = objSys.playerObject.GetComponent<PlayerMovementV2>();
        tutorialEnemyFOV = objSys.tutorialEnemy.GetComponent<FieldOfView>();
    }

    public override void OnObjectiveStart()
    {  
        objSys.StartCoroutine(PlayCutscene(objSys.bringItOnDialogue));
    }

    public override void OnObjectiveCompleted()
    {
        // remove from the event list to stop getting called next time
        playerCollision.OnHitPlayer -= OnEnemyHitPlayer;

        tutorialTextComponent.text = "";
    }

    public void OnEnemyHitPlayer() {
        // switch off damage collision detection
        objSys.tutorialEnemyWeapon.GetComponent<Collider>().isTrigger = true;

        // complete the objective and move on to the perfect block tutorial
        this.ObjectiveCompleted();
    }

    public void OnDialogueFinishedPlaying() {

        // register to collision event
        playerCollision.OnHitPlayer += OnEnemyHitPlayer;

        // enable movement input
        playerMovement.enableMovement = true;

        // switch On the AI
        tutorialEnemyFOV.enabled = true;

        // set text
        tutorialTextComponent.color = new Color(1,1,1,1);
        tutorialTextComponent.text = "Use W, A, S, D to move around";

        // enemy Says Hraa!! Diee!!
        PlaySecondCutscene(objSys.enemyShoutingDialogue);
    }
    
    private IEnumerator PlayCutscene(PlayableAsset cutscene) {
        float duration = (float)cutscene.duration;
        objSys.playableDirector.playableAsset = cutscene;
        objSys.playableDirector.Play();
        yield return new WaitForSeconds(duration);
        objSys.playableDirector.Stop();
        this.OnDialogueFinishedPlaying();
    }

    private void PlaySecondCutscene(PlayableAsset cutscene) {
        float duration = (float)cutscene.duration;
        objSys.playableDirector.playableAsset = cutscene;
        objSys.playableDirector.Play();
    }
}
