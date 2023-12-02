using GameObjects.Enemies;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "StaticData/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        public EnemyTypeId EnemyTypeId;
        [Range(1f, 10f)] public float Health = 1;
        [Range(1f, 10f)] public float Speed = 1;
        [Range(1f, 10f)] public float Damage = 1;
        [Range(0f, 1)] public float Armor = 0;
    }
}