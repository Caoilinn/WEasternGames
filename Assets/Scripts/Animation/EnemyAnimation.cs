﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Animator _anim;
    private EnemyAction enemyAction;
    public GameObject player;
    public GameObject enemy;
    private Collider collider;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimationController/EnemyAnimator"); //Load controller at runtime https://answers.unity.com/questions/1243273/runtimeanimatorcontroller-not-loading-from-script.html
        enemyAction = GetComponent<EnemyAction>();
        collider = this.enemy.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/" +
            "katana").gameObject.GetComponent<BoxCollider>(); // to find a child game object by name   //https://docs.unity3d.com/ScriptReference/Transform.Find.html
    }

    void FixedUpdate()
    {
        initialiseAnimatorBool();
    }

    void initialiseAnimatorBool()
    {
        _anim.SetBool("isAttacking", collider.isTrigger);
        _anim.SetBool("isPerfectBlock", enemyAction.isPerfectBlock);
        _anim.SetBool("isKeepBlocking", enemyAction.isKeepBlocking);
    }

    #region Enemy Attack Logic
    public void OnAnimation_IsHeavyAttackActive()
    {
        collider.isTrigger = false;
    }

    public void OnAnimation_IsHeavyAttackDeactive()
    {
        collider.isTrigger = true;
    }

    public void OnAnimation_IsLightAttackActive()
    {
        collider.isTrigger = false;
    }

    public void OnAnimation_IsLightAttackDeactive()
    {
        collider.isTrigger = true;
    }

    public void OnAnimation_StopAttackCollision()
    {
        collider.isTrigger = true;
    }
    #endregion

    #region Enemy Block Logic
    public void OnAnimation_BlockStart()
    {
        enemyAction.isKeepBlocking = true;
    }

    public void OnAnimation_isPerfectBlock()
    {
        enemyAction.isPerfectBlock = true;
    }

    public void OnAnimation_isPerfectBlockEnd()
    {
        enemyAction.isPerfectBlock = false;
    }
    #endregion

    #region Enemy Get Hurt Logic
    public void OnAnimation_isGetCriticalHit()
    {
    }
    #endregion
}