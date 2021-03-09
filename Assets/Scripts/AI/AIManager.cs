using System;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class AIManager : MonoBehaviour
    {
        public static AIManager current;
        
        #region Events
        public event Action<AIController> OnAttackStateChangeReq;
        
        public void AttackStateChangeReq(AIController controller)
        {
            OnAttackStateChangeReq?.Invoke(controller);
        }
        #endregion
        
        
        private GameObject[] _enemies;
        private IDictionary<GameObject, AIController> _enemyControllers;


        private void Awake()
        {
            current = this;
        }

        
        private void Start()
        {
            _enemies = GameObject.FindGameObjectsWithTag("Enemy");
            _enemyControllers = new Dictionary<GameObject, AIController>();
            
            foreach (GameObject enemy in _enemies)
            {
                _enemyControllers.Add(enemy, enemy.GetComponent<AIController>());
            }
        }

        private void FixedUpdate()
        {
            //Check if enemy has the attack flag
            //CheckForAttack();
        }

        public void RequestsAttackState(GameObject reqEnemy)
        {
            AIController enemyController = _enemyControllers[reqEnemy];

            if (enemyController != null && !enemyController.HasAttackFlag)
            {
                enemyController.HasAttackFlag = true;
            }
        }


        public void CheckForAttack()
        {
            bool oneAttacking = false;
            int low = 100;
            int high = 0 ;
            
            foreach (var kvp in _enemyControllers)
            {
                if (kvp.Value.id > high)
                    high = kvp.Value.id;

                if (kvp.Value.id < low)
                    low = kvp.Value.id;
                
                if (kvp.Value.HasAttackFlag)
                    oneAttacking = true;
            }

            if (!oneAttacking)
            {
                int random = Random.Range(low, high);

                AIController controllerToChange = null;

                foreach (var kvp in _enemyControllers)
                {
                    if (kvp.Value.id == random)
                    {
                        controllerToChange = kvp.Value;
                    }
                }
                
                if(controllerToChange != null)
                    controllerToChange.AttackStateChange(controllerToChange);
            }
        }
    }
}