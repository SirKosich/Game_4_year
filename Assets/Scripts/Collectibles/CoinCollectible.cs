using SimpleMedievalPlatformer.Player;
using SimpleMedievalPlatformer.Systems;
using UnityEngine;

namespace SimpleMedievalPlatformer.Collectibles
{
    [DisallowMultipleComponent]
    public sealed class CoinCollectible : MonoBehaviour
    {
        private Vector3 startPosition;

        private void Awake()
        {
            CircleCollider2D trigger = GetComponent<CircleCollider2D>();
            if (trigger == null)
            {
                trigger = gameObject.AddComponent<CircleCollider2D>();
            }

            trigger.isTrigger = true;
            trigger.radius = 0.5f;

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer == null)
            {
                renderer = gameObject.AddComponent<SpriteRenderer>();
            }

            renderer.sprite = RuntimeSpriteLibrary.GetSprite(RuntimeShape.Circle);
            renderer.color = new Color(1f, 0.9f, 0.2f);

            transform.localScale = new Vector3(0.45f, 0.45f, 1f);
            startPosition = transform.position;
        }

        private void Update()
        {
            transform.position = startPosition + Vector3.up * (Mathf.Sin(Time.time * 4f + transform.position.x) * 0.08f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<PlayerController>() == null)
            {
                return;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
            }

            Destroy(gameObject);
        }
    }
}
