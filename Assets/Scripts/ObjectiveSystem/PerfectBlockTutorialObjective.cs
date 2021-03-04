using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PerfectBlockTutorialObjective : Objective
{
    private TextMeshProUGUI tutorialTextComponent;
    public PerfectBlockTutorialObjective(ObjectiveSystem objSys) : base(objSys) {
        tutorialTextComponent = objSys.tutorialText.GetComponent<TextMeshProUGUI>();
    }

    public override void OnObjectiveStart()
    {
        // Register to Input Event
        objSys.playerInput.OnRightClickPressed += OnBlockingButtonPressed;

        // stop time 
        objSys.timeManager.ChangeTimescale(0);

        tutorialTextComponent.text = "Press Right Click at the right time to do a Perfect Block";
    }

    public override void OnObjectiveCompleted()
    {
        
    }


    public void OnBlockingButtonPressed() {
        // trigger a perfect block
        objSys.tutorialEnemyWeapon.GetComponent<EnemyWeaponCollision>().TriggerPerfectBlock(objSys.playerObject);

        // unregister events
        objSys.playerInput.OnRightClickPressed -= OnBlockingButtonPressed;

        // resume time
        objSys.timeManager.ChangeTimescale(1);

        tutorialTextComponent.text = "";

        // complete objective after a time testing
        objSys.StartCoroutine(this.CompleteObjectiveInSeconds(0.25f));
    }
}
