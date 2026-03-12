using SimpleMedievalPlatformer.Player;
using UnityEngine;

namespace SimpleMedievalPlatformer.Systems
{
    [DisallowMultipleComponent]
    public sealed class Checkpoint : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private bool activated;

        private void Awake()
        {
            BoxCollider2D trigger = GetComponent<BoxCollider2D>();
            if (trigger == null)
            {
                trigger = gameObject.AddComponent<BoxCollider2D>();
            }

            trigger.isTrigger = true;
            trigger.size = new Vector2(1f, 1.5f);

            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }

            spriteRenderer.sprite = RuntimeSpriteLibrary.GetSprite(RuntimeShape.Diamond);
            spriteRenderer.color = new Color(1f, 0.58f, 0.15f);

            transform.localScale = new Vector3(0.8f, 1.1f, 1f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (activated || other.GetComponent<PlayerController>() == null || LevelManager.Current == null)
            {
                return;
            }

            activated = true;
            LevelManager.Current.SetCheckpoint(transform.position + new Vector3(0f, 1f, 0f));
            spriteRenderer.color = new Color(0.95f, 1f, 0.35f);
        }
    }
}
