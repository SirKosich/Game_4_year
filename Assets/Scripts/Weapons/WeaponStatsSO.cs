using UnityEngine;

namespace SimpleMedievalPlatformer.Weapons
{
    [CreateAssetMenu(fileName = "WeaponStats", menuName = "Simple Medieval Platformer/Weapon Stats")]
    public sealed class WeaponStatsSO : ScriptableObject
    {
        public string displayName = "Sword";
        public WeaponType type = WeaponType.Sword;
        public int damage = 1;
        public float cooldown = 0.35f;
        public float range = 1.25f;
        public float projectileSpeed = 8f;
    }
}
