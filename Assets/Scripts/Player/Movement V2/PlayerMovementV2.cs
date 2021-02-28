using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementV2 : MonoBehaviour
{
    public bool enableMovement = true;
    public Transform playerCameraTransform;
    public float playerSpeed;
    public float runningSpeed = 8;
    public float walkingSpeed = 4;
    public float hitSpeed = 1;
    public float airAcceleration = 1;
    public float maxAirSpeed = 0.25f;
    public float rotationSpeed = 500;
    public float dodgeAcceleration = 2;
    public float dodgeDeceleration = 5;
    public float jumpForce = 10;
    public float doubleJumpBoost = 0;
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
    private bool isFalling = false;
    private bool isCollidingWithBuilding = false;
    private bool alreadyDoubleJumped = false;
    private float fallingSpeed = 0f;
    private float dodgeBoostSpeed = 0f;
    private float currentSpeed = 0f;
    private float currentRotationSpeed = 0f;
    private float currentAirControlSpeed = 0f;
    private List<KeyCode> inputList;
    private Vector3 direction = new Vector3(0,0,0);
    private Vector3 moveVelocity = new Vector3(0,0,0);
    private Vector3 gravityVelocity = new Vector3(0,0,0);
    private Vector3 jumpVelocity = new Vector3(0,0,0);
    private Vector3 midAirVelocity = new Vector3(0,0,0);
    private Vector3 dodgeBoostVelocity = new Vector3(0,0,0);
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
        UpdateMovementVelocity();

        // apply force of gravity
        ApplyGravity();

        // update player model rotation
        UpdatePlayerModelRotation();

        // Update animation 
        UpdateAnimations();
        
        //slow down player's speed when get hit
        slowDownSpeed();

        // update dodge and running stats
        dodgeAndRunningStats();

        // apply the final velocity to rigidbody, in fixed update
        UpdateTotalVelocity();

        // check if ground is close enough, if it is then player is falling
        CheckIfFalling();
    }

    private void dodgeAndRunningStats()
    {
        #region Dodge
        if (DodgeTime > 0)
        {
            DodgeTime -= Time.deltaTime;

            // accelerate while dodging is active
            this.dodgeBoostSpeed += this.dodgeAcceleration * Time.deltaTime;
            //playerRigidbody.AddRelativeForce(Vector3.forward * 150);

        }
        if (DodgeTime <= 0)
        {
            // decelerate when dodging is deactivated
            this.dodgeBoostSpeed -= this.dodgeDeceleration * Time.deltaTime;
            // limit to 0 minimum
            this.dodgeBoostSpeed = Mathf.Clamp(this.dodgeBoostSpeed, 0, Mathf.Infinity);

            isDodging = false;
            animator.ResetTrigger("Dodge");
        }

        this.dodgeBoostVelocity = this.transform.forward * this.dodgeBoostSpeed;

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

        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("BI") && 
            !animator.GetCurrentAnimatorStateInfo(0).IsTag("PB") && 
            !animator.GetCurrentAnimatorStateInfo(0).IsTag("LT") && 
            !animator.GetCurrentAnimatorStateInfo(0).IsTag("HT") && 
            !playerStats.isHitStun &&
            this.enableMovement)
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

    private void UpdateMovementVelocity() {
        if (isFalling) { AirControl(); return; }

        if(!isIdle && speedDebuffTime <= 0)
        {
            // check whether running, walking or falling, assign speed accordingly
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
        this.moveVelocity = this.direction * playerSpeed;

        // set the velocity relative to camera rotation.
        this.moveVelocity = Quaternion.AngleAxis(playerCameraTransform.rotation.eulerAngles.y, Vector3.up) * this.moveVelocity;
    }
    
    private void AirControl() {
        if (!isIdle && !isCollidingWithBuilding) {
            this.currentAirControlSpeed += this.airAcceleration * Time.deltaTime;
            this.currentAirControlSpeed = Mathf.Clamp(this.currentAirControlSpeed, 0, maxAirSpeed);
            this.direction = Quaternion.AngleAxis(playerCameraTransform.rotation.eulerAngles.y, Vector3.up) * this.direction;
            this.midAirVelocity = this.direction * this.currentAirControlSpeed;

            return;
        }
            
        //Vector3 airVelocity = this.direction * this.currentAirControlSpeed;
        // if ((this.moveVelocity + airVelocity).magnitude <= this.runningSpeed) {
        //     this.moveVelocity += this.direction * this.currentAirControlSpeed;
        // } else {
        //     this.moveVelocity = this.direction * this.runningSpeed;
        // }

        //this.moveVelocity += airVelocity;
    }

    private void UpdateJumpVelocity() {
        if (isGrounded) { jumpVelocity = Vector3.zero; }
        jumpVelocity = Vector3.up * jumpForce;
    }

    private void Jump() {
        // can't jump if already falling in the air, only double jump 
        if (isFalling) { 
            DoubleJump();
            return; 
        }

        this.isFalling = true;
        this.moveVelocity /= 2;
        this.fallingSpeed = 0f;
        jumpVelocity = Vector3.up * jumpForce; 
    }

    private void DoubleJump() {
        if (alreadyDoubleJumped) { return; }

        // if (!isIdle) {
        //     this.moveVelocity = this.moveVelocity.magnitude * this.direction;
        // }
        this.fallingSpeed = 0f;
        this.jumpVelocity += Vector3.up * this.doubleJumpBoost;
        this.alreadyDoubleJumped = true;
    }
    private void StopJumping() {
        jumpVelocity = Vector3.zero;
        alreadyDoubleJumped = false;
    }

    private void ApplyGravity() {
        if (isGrounded) {
            fallingSpeed = 0;
        } else {
            fallingSpeed += gravity * Time.deltaTime;
        }

        this.gravityVelocity = Vector3.down * fallingSpeed;
    }

    private void UpdatePlayerModelRotation() {
        if (!isIdle) {
            Vector3 velocityXZonly = new Vector3(this.moveVelocity.x + this.midAirVelocity.x, 0, this.moveVelocity.z + this.midAirVelocity.z);
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

    private void UpdateTotalVelocity() {
        playerRigidbody.velocity = this.moveVelocity + 
                                   this.gravityVelocity + 
                                   this.dodgeBoostVelocity + 
                                   this.jumpVelocity +
                                   this.midAirVelocity;
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
        // if the player is already grounded then no point in checking, just return
        if (isGrounded) {return false;}

        // raycast downwards to check if there is ground
        RaycastHit groundHit;
        bool hit = Physics.Raycast(playerRigidbody.position + (Vector3.up * 0.5f), Vector3.down, out groundHit, groundedThreshold, groundLayerMask);

        // if the raycast hits, then the ground is close enough, hence the player is not in a falling state
        if (hit) {
            float distanceFromGround = (playerRigidbody.position - groundHit.point).magnitude;
            return false;
        } else {
            this.isFalling = true;
            animator.SetBool("isFalling", true);
        }

        return true;
    }
    
    private void OnHitTheGround() {
        // set animator properties
        animator.SetBool("isFalling", false);

        // set booleans
        isFalling = false;
        isGrounded = true;

        // Stop Jumping
        StopJumping();

        // reset air control speed and velocity
        this.currentAirControlSpeed = 0f;
        this.midAirVelocity = Vector3.zero;
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
        playerInput.OnJumpButtonPressed += OnJumpButtonPressed;
    }
    #endregion

    #region "Input Methods"
    public void OnForwardKeyPressed(){
        this.inputList.Add(KeyCode.W);
    }
    public void OnForwardKeyReleased() {
        this.inputList.RemoveAll((code) => code == KeyCode.W);
    }
    public void OnBackwardKeyPressed(){
        this.inputList.Add(KeyCode.S);
    }
    public void OnBackwardKeyReleased() {
        this.inputList.RemoveAll((code) => code == KeyCode.S);
    }
    public void OnLeftKeyPressed(){
        this.inputList.Add(KeyCode.A);
    }
    public void OnLeftKeyReleased() {
        this.inputList.RemoveAll((code) => code == KeyCode.A);
    }
    public void OnRightKeyPressed(){
        this.inputList.Add(KeyCode.D);
    }
    public void OnRightKeyReleased() {
        this.inputList.RemoveAll((code) => code == KeyCode.D);
    }

    public void OnRunningKeyPressed(){
        animator.SetBool("isRunning", true);
        this.isRunning = true;
    }
    public void OnRunningKeyReleased() {
        animator.SetBool("isRunning", false);
        this.isRunning = false;
        consumeStaminaSpeedTime = setConsumeStaminaTime(0.1f);
    }

    public void OnJumpButtonPressed() {
        Jump();
    }
    #endregion

    #region "Collision Methods"
    public void OnCollisionEnter(Collision collidee){
        if (collidee.collider.tag == "Ground") {
            OnHitTheGround();
        }
        if (collidee.collider.tag == "Building") {
            // this fixes the player sticking to walls
            this.moveVelocity = Vector3.zero;
            this.midAirVelocity = Vector3.zero;
            isCollidingWithBuilding = true;
        }
    }

    public void OnCollisionStay(Collision collidee) {
        if (collidee.collider.tag == "Building") {
            // this fixes the player sticking to walls
            this.moveVelocity = Vector3.zero;
        }
    }

    public void OnCollisionExit(Collision collidee){
        if (collidee.collider.tag == "Ground") {
            isGrounded = false;
        }
        if (collidee.collider.tag == "Building") {
            isCollidingWithBuilding = false;
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