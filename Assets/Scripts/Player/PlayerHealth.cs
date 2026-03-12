using System.Collections;
using SimpleMedievalPlatformer.Systems;
using UnityEngine;

namespace SimpleMedievalPlatformer.Player
{
    [DisallowMultipleComponent]
    public sealed class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 5;
        [SerializeField] private float invulnerabilityDuration = 0.75f;
        [SerializeField] private float knockbackForce = 5f;
        [SerializeField] private float fatalFallY = -8.5f;

        private int currentHealth;
        private bool invulnerable;
        private bool dead;
        private Rigidbody2D rb;
        private PlayerController controller;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            currentHealth = maxHealth;
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 3f;
                rb.freezeRotation = true;
            }

            controller = GetComponent<PlayerController>();
            if (controller == null)
            {
                controller = gameObject.AddComponent<PlayerController>();
            }

            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = RuntimeSpriteLibrary.GetSprite(RuntimeShape.Square);
                spriteRenderer.color = new Color(0.23f, 0.54f, 0.96f);
            }
            GameEvents.RaisePlayerHealthChanged(currentHealth, maxHealth);
        }

        private void Update()
        {
            if (!dead && transform.position.y < fatalFallY)
            {
                TakeDamage(maxHealth, transform.position + Vector3.down);
            }
        }

        public void TakeDamage(int damage, Vector2 sourcePosition)
        {
            if (dead || invulnerable)
            {
                return;
            }

            currentHealth -= Mathf.Max(1, damage);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            GameEvents.RaisePlayerHealthChanged(currentHealth, maxHealth);

            Vector2 pushDirection = ((Vector2)transform.position - sourcePosition).normalized;
            if (pushDirection == Vector2.zero)
            {
                pushDirection = Vector2.up;
            }

            rb.linearVelocity = new Vector2(pushDirection.x * knockbackForce, knockbackForce * 0.6f);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvulnerabilityRoutine());
            }
        }

        private IEnumerator InvulnerabilityRoutine()
        {
            invulnerable = true;
            float elapsed = 0f;

            while (elapsed < invulnerabilityDuration)
            {
                elapsed += Time.deltaTime;
                if (spriteRenderer != null)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                }

                yield return new WaitForSeconds(0.1f);
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }

            invulnerable = false;
        }

        private void Die()
        {
            dead = true;
            controller.SetControlsEnabled(false);
            rb.linearVelocity = Vector2.zero;

            if (LevelManager.Current != null)
            {
                LevelManager.Current.HandlePlayerDeath(this);
            }
        }

        public void RespawnAt(Vector3 position)
        {
            StopAllCoroutines();
            transform.position = position;
            currentHealth = maxHealth;
            dead = false;
            invulnerable = false;
            rb.linearVelocity = Vector2.zero;
            controller.SetControlsEnabled(true);

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
            }

            GameEvents.RaisePlayerHealthChanged(currentHealth, maxHealth);
            StartCoroutine(RespawnInvulnerability());
        }

        private IEnumerator RespawnInvulnerability()
        {
            invulnerable = true;
            yield return new WaitForSeconds(0.75f);
            invulnerable = false;
        }
    }
}
