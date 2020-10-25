﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;

    //Move Setting
    public float moveSpeed = 4f;
    public float horizontalVelocity;

    //Jump Setting
    public float verticalVelocity;
    private float gravity = 10f;
    private float jumpForce = 5f;

    private Transform CameraPivot; //empty point on player
    public CharacterController myController;

    private Vector3 camDirection;
    public bool isOnKnockBack = false;

    //Sprint
    //private bool isAccelerated = false;
    private bool isAcceleratedFinished = false;
    private bool isAcclerationCoolDownOn = false;
    private float acclerationTime = 0.2f;
    private float acclerationCoolDown = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        this.CameraPivot = this.playerCamera.transform.parent;
        myController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
    }

    private void Sprint()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            //isAccelerated = true;
            isAcclerationCoolDownOn = true;
        }
        if(isAcclerationCoolDownOn == true && acclerationCoolDown >= 0) // accleration cool down
        {
            acclerationCoolDown -= Time.deltaTime;
        }

        if (isAcclerationCoolDownOn == true && acclerationTime >= 0) // player instant accleration 
        {
            moveSpeed = 15f;
            acclerationTime -= Time.deltaTime;
        }
        if (acclerationCoolDown <= 0)
        {
            acclerationTime = 0.2f;
            acclerationCoolDown = 0.5f;
            isAcclerationCoolDownOn = false;
            isAcceleratedFinished = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) && isAcceleratedFinished == true)
        {
            moveSpeed = 8f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 4f;
            isAcceleratedFinished = false;
        }
    }

    public void Jump()
    {
        if(myController.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;//have a little pressure on player to stick to the floor
            if(Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 jumpVector = new Vector3(0, verticalVelocity, 0);
        myController.Move(jumpVector * Time.deltaTime);
    }

    public void Movement()
    {
        camDirection = (this.CameraPivot.transform.position - this.playerCamera.transform.position).normalized; // Get direction formula https://answers.unity.com/questions/697830/how-to-calculate-direction-between-2-objects.html
        //Debug.Log(camDirection.normalized);
        if (Input.GetKey(KeyCode.W) && isOnKnockBack == false)
        {
            Sprint();
            Vector3 moveVector = new Vector3(camDirection.x * moveSpeed, 0, camDirection.z * moveSpeed);
            myController.Move(moveVector * Time.deltaTime);
            //Debug.Log("pressing W");
            //this.transform.position += new Vector3(camDirection.x * moveSpeed, 0, camDirection.z * moveSpeed);
        }

        if (Input.GetKey(KeyCode.A) && isOnKnockBack == false)
        {
            Sprint();
            Vector3 moveVector = -this.playerCamera.transform.right.normalized * moveSpeed;
            myController.Move(moveVector * Time.deltaTime);
            //Debug.Log("pressing A");
            //this.transform.position += -this.playerCamera.transform.right * moveSpeed;
        }

        if (Input.GetKey(KeyCode.S) && isOnKnockBack == false)
        {
            Sprint();
            Vector3 moveVector = new Vector3(-camDirection.x * moveSpeed, 0, -camDirection.z * moveSpeed);
            myController.Move(moveVector * Time.deltaTime);
            //Debug.Log("pressing S");
            //this.transform.position += new Vector3(-camDirection.x * moveSpeed, 0, -camDirection.z * moveSpeed);
        }

        if (Input.GetKey(KeyCode.D) && isOnKnockBack == false)
        {
            Sprint();
            Vector3 moveVector = this.playerCamera.transform.right.normalized * moveSpeed;
            myController.Move(moveVector * Time.deltaTime);
            //Debug.Log("pressing D");
            //this.transform.position += this.playerCamera.transform.right * moveSpeed;
        }
    }
}
