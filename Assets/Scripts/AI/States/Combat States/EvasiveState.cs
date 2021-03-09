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
    private float _moveSpeed = 1f;
    private Vector3 _centre;
    private float _angle;
    private float _radius;
    private float _timer;
    private float _rotationalSpeed;
    private bool _flipped;
    private float _flippedTime;
    private Vector3 _flipPosition; 
    
    
    #region Animations
    private float _xVel;
    private int _xVelHash = Animator.StringToHash("EnemyX");
    private static readonly int BackFlip = Animator.StringToHash("CombatFlip");
    #endregion
    
    public EvasiveState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _anim = _go.GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _timer = 5f;
        _rotationalSpeed = 30f;
        _flipped = false;
        _flipPosition = Position();
        
        AnimationClip[] clips = _anim.runtimeAnimatorController.animationClips;
        
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
            float step = 3 * Time.fixedDeltaTime;
            Vector3 position = _go.transform.position;
            position = Vector3.MoveTowards(position, Position(), step);
            _go.transform.position = position;
            _centre = _player.transform.position;
            _radius = Vector3.Distance(position, _player.position);
        }
        else
        {
            _timer -= Time.fixedDeltaTime;
            
            //Rotates the enemy around the player
            _go.transform.position =
                _centre + (_go.transform.position - _centre).normalized * _radius;
            _go.transform.RotateAround(_centre, Vector3.up, _rotationalSpeed * Time.fixedDeltaTime);
   
            //Makes sure that the enemy is still facing the player
            _go.transform.LookAt(_centre);
        
            //Updates the blend tree to perform a walk animation
            _xVel = -2f;
            _anim.SetFloat(_xVelHash, _xVel);
        }

        if (!(_timer <= 0)) return;
        //Return to a follow state to get back to the player's position to start combat again
        _xVel = 0;
        _anim.SetFloat(_xVelHash, _xVel);
        _sm._CurState = new CombatWalk(_go, _sm, true);
    }

    private void DoBackFlip()
    { 
        _anim.SetTrigger(BackFlip);
        _flipped = true;
    }

    private Vector3 Position()
    {
        Vector3 position = _go.transform.position;
        return  new Vector3(position.x, position.y, position.z - 5f); 
    }
}