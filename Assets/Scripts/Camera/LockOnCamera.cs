using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnCamera : MonoBehaviour
{
    public GameObject player;
    public CameraManager cameraManager;
    public LayerMask ignoreMask;
    public bool isOnHit;

    private void Start()
    {
        isOnHit = false;
        player = GameObject.FindGameObjectWithTag("Player");
        cameraManager = GameObject.FindGameObjectWithTag("GameSetting").GetComponent<CameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag != "Ground")
        //{
        //    Material mat = other.gameObject.GetComponent<MeshRenderer>().material;
        //    Color oldColor = mat.color;
        //    Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
        //    mat.SetColor("_Color", newColor);
        //}
    }

    //https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
    void FixedUpdate()
    {
        // if(cameraManager.EnemyLockOnList.Count != 0)
        // {
        //     RaycastHit hit;
        //     // Does the ray intersect any objects excluding the player layer
        //     Vector3 direction = (this.transform.position - cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center).normalized;

        //     if (Physics.Raycast(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center, direction, out hit, 100, ignoreMask))
        //     {
        //         Debug.DrawRay(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center, direction * hit.distance, Color.yellow);
        //         Debug.Log("Did Hit  " + hit.collider.name);
        //         cameraManager.lockOnCamera.transform.position = hit.point + 2f * direction;
        //         isOnHit = true;
        //     }
        //     else
        //     {
        //         Debug.DrawRay(cameraManager.EnemyLockOnList[cameraManager.enemyCursor].GetComponent<Collider>().bounds.center, direction * 1000, Color.red);
        //         isOnHit = false;
        //         Debug.Log("Did not Hit");
        //     }
        // }

    }
}
