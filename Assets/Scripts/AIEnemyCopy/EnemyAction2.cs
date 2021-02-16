using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction2 : MonoBehaviour
{
    public enum EnemyActionType
    {
        Idle,
        LightAttack,
        HeavyAttack,
        Block,
        PerfectBlockOnly,
    }

    
    private bool jkdefh;
    
    public EnemyActionType action;
        
    public bool isPerfectBlock = false;
    public bool isKeepBlocking = false;
    public bool isInPerfectBlockOnly = false;
}
