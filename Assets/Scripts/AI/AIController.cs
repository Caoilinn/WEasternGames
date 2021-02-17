using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using AI.States;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private StateMachine _sm;

    private void Awake()
    {
        _sm = new StateMachine();
        _sm._CurState = new IdleState(gameObject, _sm);
    }

    // Update is called once per frame
    void Update()
    {
     _sm._CurState.Update();   
    }

    private void FixedUpdate()
    {
        _sm._CurState.FixedUpdate();
    }
}
