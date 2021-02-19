using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRadius : MonoBehaviour
{
    public float maxAngle;

    /*
     *https://www.youtube.com/watch?v=M9zwnHFPvy4
     *give me the idea to use a very simple way to work out the field of view relative to the enemy
     */
    public bool EnemyInFOV(Enemy enemy)
    {
        Vector3 targetDirection = enemy.transform.position - this.transform.position;  //to get the direction from the player to the enemy

        // 0 degree will be the player's look at direction is the target direction. 
        // angle = player's look at direction - the direction from the player to the enemy
        float angle = Vector3.Angle(targetDirection, this.transform.forward);  

        if (angle <= maxAngle) // if the angle is lower or equal to the given MaxAngle by player, for example 45 degrees, this mean the enemy is in the player's fov
        {
            return true;
        }
        else  // or out of fov means cant see enemy
        {
            return false;
        }
    }

}
