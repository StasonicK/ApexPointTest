using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public SceneId SceneId;
        public bool GameLoopLevel;
        public Vector3 InitialTankPosition;

        [Range(1f, 10f)] public float SecondsBetweenSpawns = 1.0f;
        [Range(1, 10)] public int KilledEnemiesVictoryCount = 1;
        [Range(1, 10)] public int Enemy1Count = 1;
        [Range(1, 10)] public int Enemy2Count = 1;
        [Range(1, 10)] public int MaxActiveEnemies = 1;
        [Range(1, 10)] public int Reward;

        public List<Vector3> EnemySpawners;
    }
}