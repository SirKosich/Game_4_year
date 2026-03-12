using SimpleMedievalPlatformer.Player;
using SimpleMedievalPlatformer.Systems;
using SimpleMedievalPlatformer.Weapons;
using UnityEngine;

namespace SimpleMedievalPlatformer.Enemies
{
    [DisallowMultipleComponent]
    public sealed class ArcherEnemy : MonoBehaviour
    {
        private EnemyHealth enemyHealth;
        private BoxCollider2D bodyCollider;
        private SpriteRenderer spriteRenderer;
        private float lastShotTime = -999f;
        private PlayerController player;

        private void Awake()
        {
            enemyHealth = GetComponent<EnemyHealth>();
            if (enemyHealth == null)
            {
                enemyHealth = gameObject.AddComponent<EnemyHealth>();
            }

            bodyCollider = GetComponent<BoxCollider2D>();
            if (bodyCollider == null)
            {
                bodyCollider = gameObject.AddComponent<BoxCollider2D>();
            }

            bodyCollider.size = new Vector2(0.9f, 1.1f);

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }

            rb.freezeRotation = true;
            rb.gravityScale = 2.5f;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }

            spriteRenderer.sprite = RuntimeSpriteLibrary.GetSprite(RuntimeShape.Square);
            spriteRenderer.color = new Color(0.31f, 0.65f, 0.38f);
            transform.localScale = new Vector3(0.9f, 1.1f, 1f);
        }

        private void Update()
        {
            if (enemyHealth == null || enemyHealth.IsDead || GameManager.Instance == null || GameManager.Instance.IsPaused)
            {
                return;
            }

            if (player == null)
            {
                player = Object.FindFirstObjectByType<PlayerController>();
                if (player == null)
                {
                    return;
                }
            }

            Vector2 toPlayer = player.transform.position - transform.position;
            float detectionRange = enemyHealth.Stats != null ? enemyHealth.Stats.detectionRange : 8f;
            float attackCooldown = enemyHealth.Stats != null ? enemyHealth.Stats.attackCooldown : 1.5f;

            if (Mathf.Abs(toPlayer.x) <= detectionRange && Mathf.Abs(toPlayer.y) <= 2.5f)
            {
                spriteRenderer.flipX = toPlayer.x < 0f;

                if (Time.time >= lastShotTime + attackCooldown)
                {
                    FireProjectile(toPlayer.normalized);
                    lastShotTime = Time.time;
                }
            }
        }

        private void FireProjectile(Vector2 direction)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/ArrowShell");
            if (prefab == null)
            {
                return;
            }

            Vector3 spawnPosition = transform.position + new Vector3(direction.x * 0.85f, 0.2f, 0f);
            GameObject projectileObject = Instantiate(prefab, spawnPosition, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            if (projectile == null)
            {
                return;
            }

            int damage = enemyHealth.Stats != null ? enemyHealth.Stats.attackDamage : 1;
            float speed = enemyHealth.Stats != null ? enemyHealth.Stats.projectileSpeed : 7f;
            projectile.Launch(direction, speed, damage, false, bodyCollider);
        }
    }
}
