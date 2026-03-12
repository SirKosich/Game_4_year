using SimpleMedievalPlatformer.Player;
using SimpleMedievalPlatformer.Systems;
using SimpleMedievalPlatformer.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleMedievalPlatformer.Levels
{
    [DisallowMultipleComponent]
    public sealed class LevelPortal : MonoBehaviour
    {
        private bool activated;

        private void Awake()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer == null)
            {
                renderer = gameObject.AddComponent<SpriteRenderer>();
            }

            RuntimeVisual visual = GetComponent<RuntimeVisual>();
            if (visual == null)
            {
                visual = gameObject.AddComponent<RuntimeVisual>();
            }

            BoxCollider2D trigger = GetComponent<BoxCollider2D>();
            if (trigger == null)
            {
                trigger = gameObject.AddComponent<BoxCollider2D>();
            }

            trigger.isTrigger = true;
            visual.SetCustom(new Color(0.66f, 0.3f, 0.9f), new Vector2(1.2f, 1.8f), 1);
            renderer.sprite = RuntimeSpriteLibrary.GetSprite(RuntimeShape.Diamond);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (activated || other.GetComponent<PlayerController>() == null)
            {
                return;
            }

            activated = true;
            string currentScene = SceneManager.GetActiveScene().name;
            string nextScene = SceneNames.GetNextLevel(currentScene);
            bool hasNextLevel = nextScene != SceneNames.MainMenu;

            int bestScore = GameManager.Instance.CompleteCurrentLevel();
            GameManager.Instance.SetPaused(true, false);

            HUDController hud = Object.FindFirstObjectByType<HUDController>();
            if (hud != null)
            {
                hud.ShowWin(GameManager.Instance.CurrentScore, bestScore, hasNextLevel, nextScene);
            }
        }
    }
}
