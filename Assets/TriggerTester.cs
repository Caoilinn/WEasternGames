using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class TriggerTester : MonoBehaviour
{

    public AIController aiController;

    private void Awake()
    {
        aiController = GameObject.Find("Enemy").GetComponent<AIController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //aiController = GameObject.Find("Enemy3").GetComponent<AIController>();
        //Debug.Log(AIManager.current);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        AIManager.current.AttackStateChangeReq(aiController);
    }
}
