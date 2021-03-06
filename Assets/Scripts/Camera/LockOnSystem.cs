using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSystem : MonoBehaviour
{
    public float cameraDistance = 3.5f;
    public float maxCameraHeight = 2f;
    public CameraManager cameraManager;
    public PlayerMovementV2 playerMovement;
    public float CameraYaxisMultiplier;
    public float playerToEnemyDistance;
    public LayerMask collisionMask;

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
            CameraYaxisMultiplier = adjustCameraYaxisMultiplier(playerToEnemyDistance);
            Vector3 forwardToTargetEnemy = cameraManager.EnemyLockOnList[cameraManager.enemyCursor].transform.position - cameraManager.player.transform.position;
            //prevent player rotating with the camera together in the y axis
            forwardToTargetEnemy.y = 0;
            cameraManager.player.transform.forward = forwardToTargetEnemy;
            cameraManager.lockOnCamera.transform.position = cameraManager.topOfHead.transform.position - cameraDistance * cameraManager.topOfHead.transform.forward + CameraYaxisMultiplier * Vector3.up;
            
            CheckIfCollidingWithObject();

            cameraManager.lockOnCamera.transform.LookAt(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].transform);
            cameraManager.lockDot.transform.position = cameraManager.lockOnCamera.WorldToScreenPoint(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center);

        }
    }
    private void CheckIfCollidingWithObject() {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        Vector3 direction = (cameraManager.lockOnCamera.transform.position - cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center).normalized;

        float distanceToCamera = Vector3.Distance(cameraManager.lockOnCamera.transform.position, cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center);
        if (Physics.Raycast(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center, direction, out hit, distanceToCamera, collisionMask))
        {
            Debug.DrawRay(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center, direction * hit.distance, Color.yellow);
            cameraManager.lockOnCamera.transform.position = hit.point;
        } else {
             Debug.DrawRay(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center, direction * distanceToCamera, Color.red);
        }
    }
    private float adjustCameraYaxisMultiplier(float distance)
    {
        float result;
        // the closer you get the higher the camera height
        result = Mathf.Clamp(distance, 0, maxCameraHeight);

        return result;
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
