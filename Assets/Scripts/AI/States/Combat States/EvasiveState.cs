using AI;
using AI.States;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

//https://answers.unity.com/questions/433791/rotate-object-around-moving-object.html
//Resource used to calculate new circular motion that did not lock the Y 
public class EvasiveState : State
{
    private Animator _anim;
    private Transform _player;
    private float _moveSpeed = 0.5f;
    private Vector3 _centre;
    private float _angle;
    private float _radius;
    private float _timer;
    private float _rotationalSpeed;
    
    #region Blend Tree Parameters
    
    private float _xVel;
    private int _xVelHash;
    
    #endregion
    
    public EvasiveState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _anim = _go.GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _radius = 5f;
        _timer = 6f;
        _rotationalSpeed = 30f;
        
        //For the blend tree animation
        _xVelHash = Animator.StringToHash("enemyVelX");

        // for triggering trash talk dialogue
        if (_sm.playableDirector.state == PlayState.Playing) { return; }
        _sm.playableDirector.playableAsset = _sm.trashTalkDialogue;
        _sm.playableDirector.Play();
    }

    public override void FixedUpdate()
    {
        _timer -= Time.fixedDeltaTime; 
        _centre = _player.transform.position;

        //Rotates the enemy around the player
        _go.transform.position =
            _centre + (_go.transform.position - _centre).normalized * _radius;
        _go.transform.RotateAround(_centre, Vector3.up, _rotationalSpeed * Time.fixedDeltaTime);
        
        //Makes sure that the enemy is still facing the player
        _go.transform.LookAt(_centre);
      
        //Updates the blend tree to perform a walk animation
        _xVel = -1f;
        _anim.SetFloat(_xVelHash, _xVel);

        if (_timer <= 0)
        {
            //Return to a follow state to get back to the player's position to start combat again
            _xVel = 0;
            _anim.SetFloat(_xVelHash, _xVel);
            _sm._CurState = new CombatWalk(_go, _sm, true);
        }
    }
}