using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class MainLevelObjective : Objective
{
    PlayerCollision playerCollision;
    PlayerStats playerStats;
    float gettingHitDialogueChance = 0.25f;
    public MainLevelObjective(ObjectiveSystem objSys) : base(objSys) {
        playerCollision = objSys.playerObject.GetComponent<PlayerCollision>();
        playerStats = objSys.playerObject.GetComponent<PlayerStats>();
    }   

    public override void OnObjectiveCompleted()
    {
        // disable all gameplay


        // unregister enemy killed events


        // unregister to sachi getting hit events
        playerCollision.OnPlayerHurt -= OnSachiGetHit;

        // play ending cutscene


    }

    public override void OnObjectiveStart()
    {   
        // display objective for a brief moment

        // register to enemy killed events

        // register to sachi getting hit events
        playerCollision.OnPlayerHurt += OnSachiGetHit;
    }

    private void OnSachiGetHit() {
        // check if sachi is dead, if dead then play the death dialogue
        if (playerStats.isDeath) { return; }

        // random chance of triggering the dialogues
        if (Random.Range(0f, 1f) > this.gettingHitDialogueChance) { return; }

        // check if playable is still playing
        if (objSys.playableDirector.state == PlayState.Playing) { return; }

        // choose a random hurt dialogue
        if (objSys.SachiHurtDialogues.Length == 0) { return; }
        int random = Random.Range(0,objSys.SachiHurtDialogues.Length);
        objSys.StartCoroutine(PlayCutscene(objSys.SachiHurtDialogues[random]));
    }

    private void OnEnemyKilled() {
        // show updated objective display 

        // increment number of defeated enemies

        // check is it equals to number of enemies

        // if it is then complete the objective
    }

    private IEnumerator PlayCutscene(PlayableAsset cutscene) {
        float duration = (float)cutscene.duration;
        objSys.playableDirector.playableAsset = cutscene;
        objSys.playableDirector.Play();
        yield return new WaitForSeconds(duration);
        objSys.playableDirector.Stop();
    }
}
