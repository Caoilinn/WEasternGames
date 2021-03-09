using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EnemyBehaviour : MonoBehaviour
{
    private Animator _anim;

    private GameObject testPlayer;
    public float playerDistance;
    EnemyAction enemyAction;

    void Start()
    {
        _anim = GetComponent<Animator>();
        testPlayer = GameObject.FindGameObjectWithTag("Player");
        enemyAction = this.GetComponent<EnemyAction>();
    }

    void FixedUpdate()
    {
        playerDistance = Vector3.Distance(this.transform.position, testPlayer.transform.position);
        if (playerDistance < 5)
        {
           // this.transform.LookAt(new Vector3(testPlayer.transform.position.x, this.transform.position.y, testPlayer.transform.position.z)); // only rotate y axis
            
        }
    } 
}
