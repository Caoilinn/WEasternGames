using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyAction : MonoBehaviour
{
    public enum EnemyActionType
    {
        Idle,
        LightAttack,
        HeavyAttack,
        Block,
        PerfectBlockOnly,
        EnterInjured,
        Injured
    }

    public EnemyActionType action;

    public bool isPerfectBlock = false;
    public bool isKeepBlocking = false;
    public bool isInPerfectBlockOnly = false;
}
