using SimpleMedievalPlatformer.Systems;
using UnityEngine;

namespace SimpleMedievalPlatformer.Enemies
{
    [DisallowMultipleComponent]
    public sealed class SkeletonPatrol : MonoBehaviour
    {
        [SerializeField] private float patrolRange = 3f;
        [SerializeField] private LayerMask groundMask = ~0;

        private EnemyHealth enemyHealth;
        private Rigidbody2D rb;
        private BoxCollider2D bodyCollider;
        private SpriteRenderer spriteRenderer;
        private float startX;
        private int direction = 1;

        private void Awake()
        {
            enemyHealth = GetComponent<EnemyHealth>();
            if (enemyHealth == null)
            {
                enemyHealth = gameObject.AddComponent<EnemyHealth>();
            }

            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 2.5f;
                rb.freezeRotation = true;
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            }

            bodyCollider = GetComponent<BoxCollider2D>();
            if (bodyCollider == null)
            {
                bodyCollider = gameObject.AddComponent<BoxCollider2D>();
            }

            bodyCollider.size = new Vector2(0.9f, 1.1f);

            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }

            spriteRenderer.sprite = RuntimeSpriteLibrary.GetSprite(RuntimeShape.Square);
            spriteRenderer.color = new Color(0.82f, 0.84f, 0.86f);
            transform.localScale = new Vector3(0.9f, 1.1f, 1f);

            startX = transform.position.x;
        }

        private void FixedUpdate()
        {
            if (enemyHealth == null || enemyHealth.IsDead)
            {
                return;
            }

            float speed = enemyHealth.Stats != null ? enemyHealth.Stats.moveSpeed : 2f;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

            bool outOfRange = Mathf.Abs(transform.position.x - startX) >= patrolRange;
            bool edgeAhead = !Physics2D.Raycast((Vector2)transform.position + new Vector2(direction * 0.7f, -0.55f), Vector2.down, 0.4f, groundMask);

            if (outOfRange || edgeAhead)
            {
                direction *= -1;
            }

            spriteRenderer.flipX = direction < 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts.Length > 0 && Mathf.Abs(collision.contacts[0].normal.x) > 0.5f)
            {
                direction *= -1;
            }
        }
    }
}
