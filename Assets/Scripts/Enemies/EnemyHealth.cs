using SimpleMedievalPlatformer.Systems;
using UnityEngine;

namespace SimpleMedievalPlatformer.Enemies
{
    [DisallowMultipleComponent]
    public sealed class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private EnemyStatsSO stats;

        private int currentHealth;
        private Rigidbody2D rb;
        private bool dead;

        public EnemyStatsSO Stats => stats;
        public bool IsDead => dead;

        private void Awake()
        {
            if (stats == null)
            {
                string assetName = gameObject.name.Contains("Archer") ? "Enemy_Archer" : "Enemy_Skeleton";
                stats = Resources.Load<EnemyStatsSO>("ScriptObjects/" + assetName);
            }

            currentHealth = stats != null ? stats.maxHealth : 2;

            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }

            rb.freezeRotation = true;
            rb.gravityScale = 2.5f;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        public void TakeDamage(int damage, Vector2 sourcePosition)
        {
            if (dead)
            {
                return;
            }

            currentHealth -= Mathf.Max(1, damage);

            Vector2 knockDirection = ((Vector2)transform.position - sourcePosition).normalized;
            if (knockDirection == Vector2.zero)
            {
                knockDirection = Vector2.up;
            }

            rb.linearVelocity = new Vector2(knockDirection.x * 2.5f, 3f);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            dead = true;
            DropCoins();
            Destroy(gameObject);
        }

        private void DropCoins()
        {
            GameObject coinPrefab = Resources.Load<GameObject>("Prefabs/Coin");
            if (coinPrefab == null)
            {
                return;
            }

            int min = stats != null ? stats.minCoins : 1;
            int max = stats != null ? stats.maxCoins : 3;
            int count = Random.Range(min, max + 1);

            for (int index = 0; index < count; index++)
            {
                Vector3 spawn = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.2f, 0.8f), 0f);
                Instantiate(coinPrefab, spawn, Quaternion.identity);
            }
        }
    }
}
