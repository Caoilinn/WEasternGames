using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveSystem : MonoBehaviour
{
    // any unity scene objects that needs to bee referenced is got from here
    public PlayerInput playerInput;
    public TimeManager timeManager;
    public GameObject playerObject;
    public GameObject tutorialText;
    public GameObject tutorialEnemyWeapon;
    public GameObject tutorialEnemy;

    // objectives
    private Objective[] objectives;
    void Start() {
        // initialize number of objectives
        this.objectives = new Objective[5];

        // initialize all objectives, assigning the references to unity scene objects
        this.objectives[0] = new BlockingTutorialObjective(this);
        this.objectives[1] = new LockOnTutorialObjective(this);
        this.objectives[2] = new MovementTutorialObjective(this);
        this.objectives[3] = new PerfectBlockTutorialObjective(this);
        this.objectives[4] = new AttackingTutorialObjective(this);

        // set the order of the objectives
        this.objectives[0].SetNextObjective(this.objectives[1]);
        this.objectives[1].SetNextObjective(this.objectives[2]);
        this.objectives[2].SetNextObjective(this.objectives[3]);
        this.objectives[3].SetNextObjective(this.objectives[4]);

        // start the first objective
        this.objectives[0]?.ObjectiveStart();
    }
}
