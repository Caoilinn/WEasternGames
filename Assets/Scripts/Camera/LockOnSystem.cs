using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public CameraManager cameraManager;
    public PlayerMovementV2 playerMovement;

    private void Awake()
    {
        cameraManager = GameObject.FindGameObjectWithTag("GameSetting").GetComponent<CameraManager>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementV2>();
    }

    private void Update()
    {
        lockOnCameraFixed();
        lockOnEnemy();
        
    }

    private void lockOnCameraFixed()
    {
        this.transform.position = cameraManager.player.transform.position;
        if (cameraManager.EnemyLockOnList.Count == 1)
        {
            cameraManager.enemyCursor = 0;
        }
    }

    private void lockOnEnemy()
    {
        if(cameraManager.isLockOnMode && cameraManager.EnemyLockOnList.Count != 0)
        {
            Vector3 forwardToTargetEnemy = cameraManager.EnemyLockOnList[cameraManager.enemyCursor].transform.position - cameraManager.player.transform.position;
            forwardToTargetEnemy.y = 0;
            cameraManager.player.transform.forward = forwardToTargetEnemy;
            cameraManager.lockOnCamera.transform.position = cameraManager.player.transform.position -  3.5f * cameraManager.player.transform.forward + 2f * Vector3.up;
            cameraManager.lockOnCamera.transform.LookAt(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].transform);
            cameraManager.lockDot.transform.position = cameraManager.lockOnCamera.WorldToScreenPoint(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            cameraManager.EnemyLockOnList.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (cameraManager.EnemyLockOnList.Count > 0)
            {
                if(cameraManager.enemyCursor > 0)
                {
                    cameraManager.enemyCursor--;
                }
                else if(cameraManager.enemyCursor == 0 &&
                    cameraManager.EnemyLockOnList.Count > 0 &&
                    cameraManager.enemyCursor < cameraManager.EnemyLockOnList.Count - 1)
                {
                    cameraManager.enemyCursor++;
                    if(cameraManager.EnemyLockOnList.Count == 1)
                    {
                        cameraManager.enemyCursor = 0;
                    }
                }
                cameraManager.EnemyLockOnList.Remove(enemy);
            }

        }
    }
}
