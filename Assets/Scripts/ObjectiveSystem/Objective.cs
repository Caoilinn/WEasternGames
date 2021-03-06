using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Objective
{   
    public Objective nextObjective;
    protected ObjectiveSystem objSys; // handle to objective system

    // test
    protected bool isActive = false; // this will be used to check if it responds to game events

    public Objective(ObjectiveSystem objSys) {
        this.objSys = objSys;
    }

    // method is used to start the objective
    public void ObjectiveStart() {
        // activate the objective
        this.isActive = true;
        
        // run the defined operations when an objective is finished
        OnObjectiveStart();
    }

    // method should only be called when the objective is completed
    protected void ObjectiveCompleted() {
        // run whatever operations need to be carried out when objective is completed
        OnObjectiveCompleted();

        // deactivate this objective and run the next objective if it exists
        this.isActive = false;
        this.nextObjective?.ObjectiveStart();
    }

    public virtual void OnObjectiveStart() {
        // this method is meant to be overwritten, contains operations relating to the game scene objects when the objective is started
    }

    public virtual void OnObjectiveCompleted() {
        // this method is meant to be overwritten, contains operations relating to the game scene objects when the objective is completed
    }
    public void SetNextObjective(Objective nextObjective) {
        this.nextObjective = nextObjective;
    }

    protected IEnumerator CompleteObjectiveInSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        this.ObjectiveCompleted();
    }
}
