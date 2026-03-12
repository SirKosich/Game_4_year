using UnityEngine;

namespace SimpleMedievalPlatformer.Systems
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Transform))]
    public sealed class RuntimeVisual : MonoBehaviour
    {
        public enum Preset
        {
            Custom = 0,
            Player = 1,
            Skeleton = 2,
            Archer = 3,
            Coin = 4,
            Checkpoint = 5,
            Portal = 6,
            Sword = 7,
            Bow = 8,
            Projectile = 9,
            Platform = 10,
            Background = 11
        }

        [SerializeField] private Preset preset = Preset.Custom;
        [SerializeField] private Vector2 size = Vector2.zero;
        [SerializeField] private Color customColor = Color.white;
        [SerializeField] private int sortingOrder;

        private void Awake()
        {
            Apply();
        }

        public void Apply()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            }

            RuntimeShape shape = RuntimeShape.Square;
            Color color = customColor;
            Vector2 defaultSize = Vector2.one;
            int defaultOrder = sortingOrder;

            switch (preset)
            {
                case Preset.Player:
                    shape = RuntimeShape.Square;
                    color = new Color(0.23f, 0.54f, 0.96f);
                    defaultSize = new Vector2(0.8f, 1.2f);
                    break;

                case Preset.Skeleton:
                    shape = RuntimeShape.Square;
                    color = new Color(0.82f, 0.84f, 0.86f);
                    defaultSize = new Vector2(0.9f, 1.1f);
                    break;

                case Preset.Archer:
                    shape = RuntimeShape.Square;
                    color = new Color(0.31f, 0.65f, 0.38f);
                    defaultSize = new Vector2(0.9f, 1.1f);
                    break;

                case Preset.Coin:
                    shape = RuntimeShape.Circle;
                    color = new Color(1f, 0.9f, 0.2f);
                    defaultSize = new Vector2(0.45f, 0.45f);
                    break;

                case Preset.Checkpoint:
                    shape = RuntimeShape.Diamond;
                    color = new Color(1f, 0.58f, 0.15f);
                    defaultSize = new Vector2(0.8f, 1.1f);
                    break;

                case Preset.Portal:
                    shape = RuntimeShape.Diamond;
                    color = new Color(0.67f, 0.35f, 0.88f);
                    defaultSize = new Vector2(1.2f, 1.8f);
                    break;

                case Preset.Sword:
                    shape = RuntimeShape.Square;
                    color = new Color(0.85f, 0.88f, 0.92f);
                    defaultSize = new Vector2(0.9f, 0.18f);
                    break;

                case Preset.Bow:
                    shape = RuntimeShape.Diamond;
                    color = new Color(0.52f, 0.32f, 0.16f);
                    defaultSize = new Vector2(0.35f, 0.75f);
                    break;

                case Preset.Projectile:
                    shape = RuntimeShape.Arrow;
                    color = new Color(0.95f, 0.95f, 0.95f);
                    defaultSize = new Vector2(0.8f, 0.24f);
                    break;

                case Preset.Platform:
                    shape = RuntimeShape.Square;
                    color = new Color(0.38f, 0.26f, 0.14f);
                    defaultSize = Vector2.one;
                    break;

                case Preset.Background:
                    shape = RuntimeShape.Square;
                    color = new Color(0.55f, 0.7f, 0.42f);
                    defaultSize = Vector2.one;
                    defaultOrder = -10;
                    break;
            }

            spriteRenderer.sprite = RuntimeSpriteLibrary.GetSprite(shape);
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = defaultOrder;
            Vector2 appliedSize = size == Vector2.zero ? defaultSize : size;
            transform.localScale = new Vector3(appliedSize.x, appliedSize.y, 1f);
        }

        public void SetCustom(Color color, Vector2 newSize, int newSortingOrder = 0)
        {
            preset = Preset.Custom;
            customColor = color;
            size = newSize;
            sortingOrder = newSortingOrder;
            Apply();
        }
    }
}
