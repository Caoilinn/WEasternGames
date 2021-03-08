using UnityEngine;

namespace UnityTemplateProjects.Utilities
{
    public class EnemyAction : MonoBehaviour
    {
        public enum EnemyActionType
        {
            Idle,
            LightAttack,
            HeavyAttack,
            Block,
            PerfectBlockOnly,
            EnterInjured,
            Injured
        }

        public EnemyActionType action;

        public bool isPerfectBlock = false;
        public bool isKeepBlocking = false;
        public bool isInPerfectBlockOnly = false;
    }
}