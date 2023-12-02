using GameObjects.Tank;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "StaticData/Weapon")]
    public class WeaponStaticData : ScriptableObject
    {
        public WeaponTypeId WeaponTypeId;
        [Range(0f, 10f)] public float Damage;
        [Range(0f, 10f)] public float Cooldown;
    }
}