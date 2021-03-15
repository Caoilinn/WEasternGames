using System.Collections.Generic;
using AI;
using AI.States;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

//https://answers.unity.com/questions/433791/rotate-object-around-moving-object.html
//Resource used to calculate new circular motion that did not lock the Y 
public class EvasiveState : State
{
    private Transform _player;
    private float _moveSpeed = 1f;
    private Vector3 _centre;
    private float _angle;
    private float _radius;
    private float _timer;
    private float _rotationalSpeed;
    private bool _flipped;
    private float _flippedTime;
    private float _maxDistance;
    private Vector3 _flipPosition; 
    
    
    
    #region Animations
    private float _xVel;
    private int _xVelHash = Animator.StringToHash("EnemyX");
    private int _zVelHash = Animator.StringToHash("enemyVelZ");
    private static readonly int BackFlip = Animator.StringToHash("CombatFlip");
    #endregion
    
    public EvasiveState(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator) : base(go, sm, attributes, animator)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _timer = 5f;
        _rotationalSpeed = 5f;
        _flipped = false;
        _flipPosition = Position();
        
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Contains("backflip"))
            {
                _flippedTime = clip.length;
            }
        }

        // for triggering trash talk dialogue
        if (_sm.playableDirector.state == PlayState.Playing) { return; }
        _sm.playableDirector.playableAsset = _sm.trashTalkDialogue;
        _sm.playableDirector.Play();
    }

    public override void FixedUpdate()
    {

        if(!_flipped)
        {
            DoBackFlip();
        }

        _flippedTime -= Time.fixedDeltaTime;
        
        if (_flippedTime > 0)
        {
            float step = 4 * Time.fixedDeltaTime;
            Vector3 position = _go.transform.position;
            position = Vector3.MoveTowards(position, Position(), step);
            _go.transform.position = position;
            
            //_centre = _player.transform.position;
            //This is the distance the AI will reach when the finish flipping away from the player
            _maxDistance = Vector3.Distance(_go.transform.position, _player.position);
        }
        else
        {
            _timer -= Time.fixedDeltaTime;          
            _go.transform.LookAt(_player);
            
            float distanceToPlayer = Vector3.Distance(_go.transform.position, _player.position);
            if (distanceToPlayer < _maxDistance)
            {
                _animator.SetFloat(_zVelHash, -1f);
                _go.transform.position -= _go.transform.forward * (4 * Time.fixedDeltaTime);

            }
            else
            {
                _animator.SetFloat(_zVelHash, 0f);
            }
        }

        if (!(_timer <= 0)) return;
        //Return to a follow state to get back to the player's position to start combat again
        _xVel = 0;
        _animator.SetFloat(_xVelHash, _xVel);
        _sm._CurState = new CombatWalk(_go, _sm, _attributes, _animator, true);
    }

    private void DoBackFlip()
    { 
        _animator.SetTrigger(BackFlip);
        _flipped = true;
    }

    private Vector3 Position()
    {
        Vector3 position = _go.transform.position;
        return  new Vector3(position.x, position.y, position.z - 10f); 
    }
}