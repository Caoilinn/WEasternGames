using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponCollisionCopy : MonoBehaviour
{
    public GameObject enemy;
    public EnemyAction2.EnemyActionType enemyActionType;
    EnemyAction2 enemyAction;

    void Start()
    {
        enemyAction = enemy.GetComponent<EnemyAction2>();
        //enemy = this.transform.root.Find("EnemyHolder/Enemy").gameObject;
    }

    void FixedUpdate()
    {
        enemyActionType = enemyAction.action;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            if (collision.transform.root.gameObject.GetComponent<PlayerAction>().isPerfectBlock == true && this.GetComponent<Collider>().isTrigger == false) //get player perfect block
            {
                enemy.GetComponent<EnemyAnimationCopy>()._anim.SetTrigger("getPlayerPerfectBlockImpact");
                
                // spawn sword clash effect
                collision.gameObject.GetComponentInParent<SwordEffectSpawner>().SpawnBigSwordClash();
            }
            this.GetComponent<Collider>().isTrigger = true;
        }
        if (collision.gameObject.tag == "Player")
        {
            //get player perfect block
            if (collision.gameObject.GetComponent<PlayerAction>().isPerfectBlock == true && this.GetComponent<Collider>().isTrigger == false) 
            {
                enemy.GetComponent<EnemyAnimationCopy>()._anim.SetTrigger("getPlayerPerfectBlockImpact");

                // spawn sword clash effect
                collision.gameObject.GetComponent<SwordEffectSpawner>().SpawnBigSwordClash();
            }

            this.GetComponent<Collider>().isTrigger = true;
        }
    }

}
