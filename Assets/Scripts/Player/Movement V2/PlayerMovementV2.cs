using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    public Transform playerCameraTransform;
    public float playerSpeed;
    public float runningSpeed = 8;
    public float walkingSpeed = 4;
    public float hitSpeed = 1;
    public float rotationSpeed = 500;
    public float gravity = 9.81f;
    public float groundedThreshold = 10;
    public LayerMask groundLayerMask;

    private bool isGoingForward;
    private bool isGoingBackward;
    private bool isGoingLeft;
    private bool isGoingRight;
    public bool isRunning;
    public bool isDodging;
    public bool isIdle;
    public bool isGrounded = false;
    private float VerticalSpeed = 0f;
    private float currentSpeed = 0f;
    private float currentRotationSpeed = 0f;
    private List<KeyCode> inputList;
    private Vector3 direction = new Vector3(0,0,0);
    private Vector3 velocity = new Vector3(0,0,0);
    private Vector3 gravityVelocity = new Vector3(0,0,0);
    private Vector3 oldPosition;
    private Vector3 forward;
    private Rigidbody playerRigidbody;
    private Animator animator;
    PlayerStats playerStats;
    public float consumeStaminaSpeedTime = 0;
    public float DodgeTime = 0f;
    public float speedDebuffTime = 0f;
    public bool moveKeyPressed = false;

    // Initialize variables and register input events
    void Start()
    {
        playerSpeed = 4;
        inputList = new List<KeyCode>();
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        oldPosition = playerRigidbody.position;
        forward = gameObject.transform.forward;
        RegisterInputEvents();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update() {
        // check what inputs are in the buffer and in which order
        CheckInputs();
    }

    void FixedUpdate() {
        // update the direction based on current input state
        UpdateDirection();

        // multiply by the speed and set it relative to the camera forward direction
        UpdateVelocity();

        // apply force of gravity
        ApplyGravity();

        // update player model rotation
        UpdatePlayerModelRotation();

        // Update animation 
        UpdateAnimations();
        
        // apply the final velocity to rigidbody, in fixed update
        UpdatePosition();

        // check if ground is close enough, if it is then player is falling
        CheckIfFalling();

        //slow down player's speed when get hit
        slowDownSpeed();

        dodgeAndRunningStats();

    }

    private void dodgeAndRunningStats()
    {
        #region Dodge
        if (DodgeTime > 0)
        {
            DodgeTime -= Time.deltaTime;
            playerRigidbody.AddRelativeForce(Vector3.forward * 150);

        }
        if (DodgeTime <= 0)
        {
            isDodging = false;
            animator.ResetTrigger("Dodge");
        }
        #endregion
        if (playerStats.stamina > 0 && isRunning && !animator.GetCurrentAnimatorStateInfo(0).IsTag("A"))
        {
            playerStats.speed = 8f;
            playerStats.readyToRestoreStaminaTime = playerStats.setReadyToRestoreStaminaTime(3.0f);

            if (consumeStaminaSpeedTime <= 0)
            {
                playerStats.stamina -= 1;
                consumeStaminaSpeedTime = setConsumeStaminaTime(0.1f);
            }
            if (consumeStaminaSpeedTime > 0 && GameObject.Find("Player").transform.hasChanged == true)
            {
                consumeStaminaSpeedTime -= Time.fixedDeltaTime;
            }
        }

        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool backPressed = Input.GetKey(KeyCode.S);
        if (forwardPressed || rightPressed || leftPressed || backPressed)
        {
            moveKeyPressed = true;
        }
        else if (!forwardPressed || !rightPressed || !leftPressed || !backPressed)
        {
            moveKeyPressed = false;
        }
    }

    private void slowDownSpeed()
    {
        if(speedDebuffTime > 0)
        {
            speedDebuffTime -= Time.fixedDeltaTime;
        }
    }

    private void CheckInputs() {
        // reset all booleans
        this.isGoingBackward = false;
        this.isGoingForward = false;
        this.isGoingLeft = false;
        this.isGoingRight = false;

        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("BI") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("PB")
            && !animator.GetCurrentAnimatorStateInfo(0).IsTag("LT") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("HT") && !playerStats.isHitStun)
        {
            if (inputList.Count == 0)
            {
                animator.SetBool("isIdle", true);
                isIdle = true;
                return;
            }
            else
            {
                animator.SetBool("isIdle", false);
                isIdle = false;
            }

            // check from the input list, which is pressed first back or front
            foreach (KeyCode keycode in inputList)
            {
                if (keycode == KeyCode.W)
                {
                    isGoingForward = true;
                    isGoingBackward = false;
                }
                else if (keycode == KeyCode.S)
                {
                    isGoingBackward = true;
                    isGoingForward = false;
                }
            }

            // check from the input list, which is pressed first left or right
            foreach (KeyCode keycode in inputList)
            {
                if (keycode == KeyCode.A)
                {
                    isGoingLeft = true;
                    isGoingRight = false;
                }
                else if (keycode == KeyCode.D)
                {
                    isGoingRight = true;
                    isGoingLeft = false;
                }
            }
        }
    }

    private void UpdateDirection() {
        // reset to 0
        this.direction.x = 0;
        this.direction.z = 0;

        if (isGoingForward) {
            this.direction.z = 1; 
        }

        if (isGoingBackward) {
            this.direction.z = -1;
        }

        if (isGoingLeft) {
            this.direction.x = -1;
        }

        if (isGoingRight) {
            this.direction.x = 1;
        }

        this.direction.Normalize();
    }

    private void UpdateVelocity() {

        if(!isIdle && speedDebuffTime <= 0)
        {
            // check whether running or walking, assign speed accordingly
            playerSpeed = (isRunning) ? runningSpeed : walkingSpeed;
        }
        else if(!isIdle && speedDebuffTime > 0)
        {
            playerSpeed = hitSpeed;
        }
        if(isIdle)
        {
            playerSpeed = 0;
        }
        #region change player speed when on block action
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("B"))
        {
            playerSpeed = 2f;
            isRunning = false;
        }
        #endregion

        // scale the current direction by the speed
        this.velocity = this.direction * playerSpeed;

        // set the velocity relative to camera rotation.
        this.velocity = Quaternion.AngleAxis(playerCameraTransform.rotation.eulerAngles.y, Vector3.up) * this.velocity;
    }

    private void ApplyGravity() {
        if (isGrounded) {
            VerticalSpeed = 0;
        } else {
            VerticalSpeed += gravity * Time.deltaTime;
        }

        this.gravityVelocity = Vector3.down * VerticalSpeed;
    }

    private void UpdatePlayerModelRotation() {
        if (!isIdle) {
            Vector3 velocityXZonly = new Vector3(this.velocity.x, 0, this.velocity.z);
            float crossProduct = Vector3.Cross(this.transform.forward, velocityXZonly).normalized.y;
            float angleInBetween = Vector3.Angle(this.transform.forward, velocityXZonly);
            float rotateBy = rotationSpeed * Time.deltaTime;
            

            if (angleInBetween > rotateBy) {
                gameObject.transform.Rotate(new Vector3(0, rotateBy * crossProduct, 0), Space.World);
            } else {
                gameObject.transform.Rotate(new Vector3(0, angleInBetween * crossProduct, 0), Space.World);
            }
        }

        // if (!isIdle) {
        //     Vector3 velocityXZOnly = new Vector3(this.velocity.x, 0, this.velocity.z);
        //     forward = velocityXZOnly;
        // }
        // gameObject.transform.forward = forward;
    }

    private void UpdatePosition() {
        playerRigidbody.velocity = this.velocity + this.gravityVelocity;
    }

    private void UpdateAnimations() {
        // get difference between old and current position, calculate the speed
        currentSpeed = (playerRigidbody.position - oldPosition).magnitude;

        // send the speed to the animator to control animation speed
        this.animator.SetFloat("movementSpeed", currentSpeed);

        // record current position as old position for the next update
        oldPosition = playerRigidbody.position;
    }

    private bool CheckIfFalling() {

        if (isGrounded) {return true;}
        RaycastHit groundHit;

        bool hit = Physics.Raycast(playerRigidbody.position, Vector3.down, out groundHit, groundedThreshold, groundLayerMask);

        if (hit) {

            float distanceFromGround = (playerRigidbody.position - groundHit.point).magnitude;
            Debug.Log("Distance From Ground: " + distanceFromGround);

            return true;
        } else {
            animator.SetBool("isFalling", true);
        }

        return false;
    }
    
    #region "Event Handling"
    private void RegisterInputEvents() {
        PlayerInput playerInput = GetComponent<PlayerInput>();

        playerInput.OnForwardKeyPressed += OnForwardKeyPressed;
        playerInput.OnForwardKeyReleased += OnForwardKeyReleased;
        playerInput.OnBackwardKeyPressed += OnBackwardKeyPressed;
        playerInput.OnBackwardKeyReleased += OnBackwardKeyReleased;
        playerInput.OnLeftKeyPressed += OnLeftKeyPressed;
        playerInput.OnLeftKeyReleased += OnLeftKeyReleased;
        playerInput.OnRightKeyPressed += OnRightKeyPressed;
        playerInput.OnRightKeyReleased += OnRightKeyReleased;
        playerInput.OnRunningKeyPressed += OnRunningKeyPressed;
        playerInput.OnRunningKeyReleased += OnRunningKeyReleased;
    }
    #endregion

    #region "Input Methods"
    public void OnForwardKeyPressed(){
        this.inputList.Add(KeyCode.W);
    }
    public void OnForwardKeyReleased() {
        this.inputList.Remove(KeyCode.W);
    }
    public void OnBackwardKeyPressed(){
        this.inputList.Add(KeyCode.S);
    }
    public void OnBackwardKeyReleased() {
        this.inputList.Remove(KeyCode.S);
    }
    public void OnLeftKeyPressed(){
        this.inputList.Add(KeyCode.A);
    }
    public void OnLeftKeyReleased() {
        this.inputList.Remove(KeyCode.A);
    }
    public void OnRightKeyPressed(){
        this.inputList.Add(KeyCode.D);
    }
    public void OnRightKeyReleased() {
        this.inputList.Remove(KeyCode.D);
    }

    public void OnRunningKeyPressed(){
        this.isRunning = true;
    }
    public void OnRunningKeyReleased() {
        this.isRunning = false;
        consumeStaminaSpeedTime = setConsumeStaminaTime(0.1f);
    }
    #endregion

    #region "Collision Methods"
    public void OnCollisionEnter(Collision collidee){
        if (collidee.collider.tag == "Ground") {
            animator.SetBool("isFalling", false);
            isGrounded = true;
        }
    }

    public void OnCollisionExit(Collision collidee){
        if (collidee.collider.tag == "Ground") {
            isGrounded = false;
        }
    }
    #endregion

    #region "Getters & Setters"
    public Vector3 GetCurrentDirection() {
        return this.direction;
    }
    #endregion

    float setConsumeStaminaTime(float num)
    {
        return num;
    }

    public void setSpeedDebuffTime(float num)
    {
        speedDebuffTime = num;
    }
    
}
