using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class ObjectiveSystem : MonoBehaviour
{
    // any unity scene objects that needs to bee referenced is got from here
    public PlayerInput playerInput;
    public TimeManager timeManager;
    public PlayableDirector playableDirector;
    public GameObject playerObject;
    public GameObject tutorialText;
    public GameObject tutorialEnemyWeapon;
    public GameObject tutorialEnemy;
    public PlayableAsset blockingTutorialDialogue;
    public PlayableAsset lockOnTutorialDialogue;
    public PlayableAsset bringItOnDialogue;
    public PlayableAsset enemyShoutingDialogue;
    public PlayableAsset AfterAttackTutorialDialogue;
    public PlayableAsset SachiDeathDialogue;

    public PlayableAsset[] SachiHurtDialogues;

    // objectives
    private Objective[] objectives;
    void Start() {
        // initialize number of objectives
        this.objectives = new Objective[6];

        // initialize all objectives, assigning the references to unity scene objects
        this.objectives[0] = new BlockingTutorialObjective(this);
        this.objectives[1] = new LockOnTutorialObjective(this);
        this.objectives[2] = new MovementTutorialObjective(this);
        this.objectives[3] = new PerfectBlockTutorialObjective(this);
        this.objectives[4] = new AttackingTutorialObjective(this);
        this.objectives[5] = new MainLevelObjective(this);

        // set the order of the objectives
        this.objectives[0].SetNextObjective(this.objectives[1]);
        this.objectives[1].SetNextObjective(this.objectives[2]);
        this.objectives[2].SetNextObjective(this.objectives[3]);
        this.objectives[3].SetNextObjective(this.objectives[4]);
        this.objectives[4].SetNextObjective(this.objectives[5]);

        // start the first objective
        //this.objectives[0]?.ObjectiveStart();
        this.objectives[5]?.ObjectiveStart();
    }
}
