using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovementTutorialObjective : Objective
{
    TextMeshProUGUI tutorialTextComponent;
    EnemyWeaponCollision tutorialEnemyWeaponCollision;
    PlayerCollision playerCollision;
    public MovementTutorialObjective(ObjectiveSystem objSys) : base(objSys)
    {
        tutorialTextComponent = objSys.tutorialText.GetComponent<TextMeshProUGUI>();
        tutorialEnemyWeaponCollision = objSys.tutorialEnemyWeapon.GetComponent<EnemyWeaponCollision>();
        playerCollision = objSys.playerObject.GetComponent<PlayerCollision>();
    }

    public override void OnObjectiveStart()
    {  
        // register to collision event
        playerCollision.OnHitPlayer += OnEnemyHitPlayer;

        tutorialTextComponent.text = "Use W, A, S, D to move around"; 
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

        // complete the objective and move on to the blocking tutorial
        this.ObjectiveCompleted();
    }
}
