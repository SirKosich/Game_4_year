using System.Collections.Generic;
using UnityEngine;

namespace SimpleMedievalPlatformer.Systems
{
    public enum RuntimeShape
    {
        Square,
        Circle,
        Diamond,
        Arrow
    }

    public static class RuntimeSpriteLibrary
    {
        private static readonly Dictionary<RuntimeShape, Sprite> CachedSprites = new Dictionary<RuntimeShape, Sprite>();

        public static Sprite GetSprite(RuntimeShape shape)
        {
            if (CachedSprites.TryGetValue(shape, out Sprite cached))
            {
                return cached;
            }

            Texture2D texture = new Texture2D(32, 32, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                name = shape.ToString()
            };

            Color clear = new Color(0f, 0f, 0f, 0f);
            Color solid = Color.white;
            int half = 16;

            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    bool draw = shape switch
                    {
                        RuntimeShape.Square => true,
                        RuntimeShape.Circle => Vector2.Distance(new Vector2(x, y), new Vector2(15.5f, 15.5f)) <= 14f,
                        RuntimeShape.Diamond => Mathf.Abs(x - half) + Mathf.Abs(y - half) <= 14,
                        RuntimeShape.Arrow => IsArrowPixel(x, y),
                        _ => true
                    };

                    texture.SetPixel(x, y, draw ? solid : clear);
                }
            }

            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 32f, 32f), new Vector2(0.5f, 0.5f), 32f);
            sprite.name = shape.ToString();

            CachedSprites.Add(shape, sprite);
            return sprite;
        }

        private static bool IsArrowPixel(int x, int y)
        {
            bool shaft = x >= 4 && x <= 20 && y >= 13 && y <= 18;
            bool head = x >= 20 && Mathf.Abs(y - 15) <= (x - 20);
            return shaft || head;
        }
    }
}
