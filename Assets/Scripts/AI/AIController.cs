using System;
using AI;
using AI.States;
using UnityEngine;using UnityTemplateProjects.Utilities;

public class AIController : MonoBehaviour
{
    private StateMachine _sm;
    private EnemyAction _enemyAction;
    private int _attacked;    
    public int id;
    
    public bool HasAttackFlag { get; set; } = false;

    public enum InitialAIState
    {
        Idle,
        Patrol,
    }

    public InitialAIState initialAIState;
    
    private void Awake()
    {
        _sm = new StateMachine();
        _sm._CurState = new EvasiveState(gameObject, _sm);
        _enemyAction = this.GetComponent<EnemyAction>();
        _attacked = 0;
        //AIManager.current.OnAttackStateChangeReq += OnAttackStateChange;
    }

    private void Start()
    {
        //Debug.Log("AIController " + id + " is printing " + AIManager.current);
    }

    // Update is called once per frame
    void Update()
    {
     _sm._CurState.Update();   
    }

    private void FixedUpdate()
    {
        _sm._CurState.FixedUpdate();

        if (_enemyAction.action == EnemyAction.EnemyActionType.EnterInjured)
            _sm._CurState = new InjuredState(gameObject, _sm);

        if (_attacked == 5)
            _sm._CurState = new BlockingState(gameObject, _sm);
    }
    
    public void AttackStateChange(AIController controller)
    {
        if (controller == this)
            _sm._CurState = new AttackingState(gameObject, _sm);
        //Debug.Log("AI Controller ID is: " + controller.id);
    }
}
