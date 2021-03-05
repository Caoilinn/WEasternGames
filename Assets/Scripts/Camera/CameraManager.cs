using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public Camera playerCamera;
    public Camera lockOnCamera;
    public GameObject player;
    public bool isLockOnMode;
    public bool canLockOn;
    public List<Enemy> EnemyLockOnList;
    public int enemyCursor;
    public Image lockDot;

    private void Start()
    {
        enemyCursor = 0;
        lockOnCamera.enabled = false;
        lockOnCamera.GetComponent<AudioListener>().enabled = false;
        lockDot = GameObject.FindGameObjectWithTag("LockCursor").GetComponent<Image>();
    }

    private void Awake()
    {
        isLockOnMode = false;
    }

    void Update()
    {
        checkIfLockOnListEmpty();
        autoTurnOffLockOn();
        lockDotTrigger();
        checkIfEnemyIsDead();
    }

    private void checkIfEnemyIsDead()
    {
        foreach(Enemy enemies in EnemyLockOnList)
        {
            if(enemies.HP <= 0)
            {
                EnemyLockOnList.Remove(enemies);
                sortEnemyListFromNearToFar();
                enemyCursor = 0;
            }
        }
    }

    private void lockDotTrigger()
    {
        if (isLockOnMode)
            lockDot.enabled = true;
        else
            lockDot.enabled = false;
    }

    public void sortEnemyListFromNearToFar()
    {
        if(EnemyLockOnList.Count != 0)
        {
            EnemyLockOnList.Sort(SortByDistanceToPlayer);
        }
    }

    /*https://gamedev.stackexchange.com/questions/166811/sort-a-list-of-objects-by-distance
     * To sort the List in the class from near to far.
     * sqrMagnitude compute the program more faster.
     */
    int SortByDistanceToPlayer(Enemy a, Enemy b)
    {
        float squaredRangeA = (a.transform.position - player.transform.position).sqrMagnitude;
        float squaredRangeB = (b.transform.position - player.transform.position).sqrMagnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }

    private void checkIfLockOnListEmpty()
    {
        if (EnemyLockOnList.Count == 0)
            canLockOn = false;
        else
            canLockOn = true;
    }

    private void autoTurnOffLockOn()
    {
        if(!canLockOn)
        {
            isLockOnMode = false;
            playerCamera.enabled  = true;
            lockOnCamera.enabled = false;
            lockOnCamera.GetComponent<AudioListener>().enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = true;
            player.GetComponent<PlayerMovementV2>().playerCameraTransform = playerCamera.transform;
            enemyCursor = 0;
        }
    }

}
