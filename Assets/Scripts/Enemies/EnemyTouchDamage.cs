using SimpleMedievalPlatformer.Player;
using UnityEngine;

namespace SimpleMedievalPlatformer.Enemies
{
    [DisallowMultipleComponent]
    public sealed class EnemyTouchDamage : MonoBehaviour
    {
        [SerializeField] private float hitCooldown = 0.75f;

        private EnemyHealth enemyHealth;
        private float lastHitTime = -999f;

        private void Awake()
        {
            enemyHealth = GetComponent<EnemyHealth>();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            TryDamage(collision.collider);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryDamage(collision.collider);
        }

        private void TryDamage(Collider2D other)
        {
            if (Time.time < lastHitTime + hitCooldown)
            {
                return;
            }

            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player == null)
            {
                return;
            }

            lastHitTime = Time.time;

            int damage = enemyHealth != null && enemyHealth.Stats != null ? enemyHealth.Stats.contactDamage : 1;
            player.TakeDamage(damage, transform.position);
        }
    }
}
