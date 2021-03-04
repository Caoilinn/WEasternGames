using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public CameraManager cameraManager;
    public PlayerMovementV2 playerMovement;
    public float CameraYaxisMultiplier;
    public float playerToEnemyDistance;

    private void Awake()
    {
        CameraYaxisMultiplier = 2;
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
            playerToEnemyDistance = Vector3.Distance(cameraManager.player.transform.position, cameraManager.EnemyLockOnList[cameraManager.enemyCursor].transform.position);
            CameraYaxisMultiplier = adjustCameraYaxisMultiplier(CameraYaxisMultiplier, playerToEnemyDistance);
            Vector3 forwardToTargetEnemy = cameraManager.EnemyLockOnList[cameraManager.enemyCursor].transform.position - cameraManager.player.transform.position;
            forwardToTargetEnemy.y = 0;
            cameraManager.player.transform.forward = forwardToTargetEnemy;
            if(!cameraManager.lockOnCamera.GetComponent<LockOnCamera>().isOnHit)
            {
                cameraManager.lockOnCamera.transform.position = cameraManager.player.transform.position - 3.5f * cameraManager.player.transform.forward + CameraYaxisMultiplier * Vector3.up;
            }
            else
            {

            }
            cameraManager.lockOnCamera.transform.LookAt(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].transform);
            cameraManager.lockDot.transform.position = cameraManager.lockOnCamera.WorldToScreenPoint(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center);

        }
    }

    private float adjustCameraYaxisMultiplier(float cameraYaxisMultiplier, float distance)
    {
        float multiplier = 2;
        if(distance > 4)
        {
            multiplier = 2;
        }
        else if(distance <= 4 && distance >= 3)
        {
            multiplier = 2;
            multiplier += distance / 2;
        }
        else if(distance <= 3 && distance >= 1.8)
        {
            multiplier = 5;
        }
        else if(distance < 1.8)
        {
            multiplier = 5.3f;
        }

        return multiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            int tempIndex = cameraManager.EnemyLockOnList.FindIndex(a => enemy == a);
            if(tempIndex == -1)
            {
                cameraManager.EnemyLockOnList.Add(enemy);
            }
            else
            {
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (cameraManager.EnemyLockOnList.Count > 0)
            {
                Enemy currentEnemy = cameraManager.EnemyLockOnList[cameraManager.enemyCursor];
                cameraManager.EnemyLockOnList.Remove(enemy);
                int tempIndex = cameraManager.EnemyLockOnList.FindIndex(a => currentEnemy == a);
                if(tempIndex != -1)
                {
                    cameraManager.enemyCursor = tempIndex;
                }
                else
                {
                    cameraManager.enemyCursor = 0;
                }
            }

        }
    }
}
