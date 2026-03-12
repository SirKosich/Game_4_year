using UnityEngine;

namespace SimpleMedievalPlatformer.Enemies
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "Simple Medieval Platformer/Enemy Stats")]
    public sealed class EnemyStatsSO : ScriptableObject
    {
        public string displayName = "Enemy";
        public int maxHealth = 2;
        public float moveSpeed = 2f;
        public int contactDamage = 1;
        public int attackDamage = 1;
        public float attackCooldown = 1.5f;
        public float projectileSpeed = 7f;
        public float detectionRange = 8f;
        public int minCoins = 1;
        public int maxCoins = 3;
    }
}
