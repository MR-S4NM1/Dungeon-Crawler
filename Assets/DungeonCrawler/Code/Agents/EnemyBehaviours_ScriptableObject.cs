using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MrSanmi.DungeonCrawler
{
    #region Enum

    public enum EnemyBehaviourType
    {
        STOP, // STOP
        MOVE_TO_RANDOM_DIRECTION, // MOVE
        PERSECUTE_THE_AVATAR, // MOVE // Persecution
        SHOOT_THE_AVATAR
    }

    #endregion

    #region Structs

    [System.Serializable]
    public struct EnemyBehaviour
    {
        public EnemyBehaviourType type;
        public float speed;
        public float time;
    }

    #endregion

    [CreateAssetMenu(menuName = "Dungeon Crawler/New Enemy Behaviour")]
    public class EnemyBehaviours_ScriptableObject : ScriptableObject
    {
        [SerializeField] public EnemyBehaviour[] patrolBehaviours;

        [SerializeField] public EnemyBehaviour[] persecutionBehaviours;

        [SerializeField] public EnemyBehaviour[] shootingBehaviours;

        [SerializeField] public float visionRadius;
    }
}
