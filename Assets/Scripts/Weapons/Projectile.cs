using SimpleMedievalPlatformer.Enemies;
using SimpleMedievalPlatformer.Player;
using SimpleMedievalPlatformer.Systems;
using UnityEngine;

namespace SimpleMedievalPlatformer.Weapons
{
    [DisallowMultipleComponent]
    public sealed class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifetime = 3f;

        private Rigidbody2D rb;
        private BoxCollider2D trigger;
        private SpriteRenderer spriteRenderer;
        private bool firedByPlayer;
        private int damage;
        private Collider2D ownerCollider;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }

            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            trigger = GetComponent<BoxCollider2D>();
            if (trigger == null)
            {
                trigger = gameObject.AddComponent<BoxCollider2D>();
            }

            trigger.isTrigger = true;
            trigger.size = new Vector2(0.85f, 0.2f);

            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }

            spriteRenderer.sprite = RuntimeSpriteLibrary.GetSprite(RuntimeShape.Arrow);

            transform.localScale = new Vector3(0.8f, 0.24f, 1f);
        }

        public void Launch(Vector2 direction, float speed, int projectileDamage, bool fromPlayer, Collider2D owner)
        {
            firedByPlayer = fromPlayer;
            damage = projectileDamage;
            ownerCollider = owner;

            if (ownerCollider != null)
            {
                Physics2D.IgnoreCollision(trigger, ownerCollider, true);
            }

            spriteRenderer.color = firedByPlayer ? new Color(0.92f, 0.92f, 0.92f) : new Color(0.2f, 0.85f, 0.5f);
            rb.linearVelocity = direction.normalized * speed;
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == ownerCollider)
            {
                return;
            }

            if (firedByPlayer)
            {
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage, transform.position);
                    Destroy(gameObject);
                    return;
                }
            }
            else
            {
                PlayerHealth player = other.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    player.TakeDamage(damage, transform.position);
                    Destroy(gameObject);
                    return;
                }
            }

            if (!other.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}
