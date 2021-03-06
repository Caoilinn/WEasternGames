using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using AI.States;
using UnityEngine;
using UnityEngine.Playables;

public class AIController : MonoBehaviour
{
    private StateMachine _sm;
    public PlayableAsset trashTalkDialogue;
    public PlayableDirector playableDirector;


    private void Awake()
    {
        _sm = new StateMachine();
        _sm.SetTrashTalkDialogue(trashTalkDialogue);
        _sm.SetPlayableDirector(playableDirector);
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
