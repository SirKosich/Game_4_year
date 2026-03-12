using SimpleMedievalPlatformer.Systems;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMedievalPlatformer.UI
{
    [DisallowMultipleComponent]
    public sealed class HUDController : MonoBehaviour
    {
        private Text hpText;
        private Text scoreText;
        private Text weaponText;
        private GameObject pausePanel;
        private GameObject gameOverPanel;
        private GameObject winPanel;

        private void Awake()
        {
            Build();
        }

        private void OnEnable()
        {
            GameEvents.PlayerHealthChanged += OnPlayerHealthChanged;
            GameEvents.ScoreChanged += OnScoreChanged;
            GameEvents.WeaponChanged += OnWeaponChanged;
            GameEvents.PauseChanged += OnPauseChanged;
            GameEvents.PlayerDeadChanged += OnPlayerDeadChanged;
        }

        private void OnDisable()
        {
            GameEvents.PlayerHealthChanged -= OnPlayerHealthChanged;
            GameEvents.ScoreChanged -= OnScoreChanged;
            GameEvents.WeaponChanged -= OnWeaponChanged;
            GameEvents.PauseChanged -= OnPauseChanged;
            GameEvents.PlayerDeadChanged -= OnPlayerDeadChanged;
        }

        private void Build()
        {
            RectTransform root = GetComponent<RectTransform>();
            root.anchorMin = Vector2.zero;
            root.anchorMax = Vector2.one;
            root.offsetMin = Vector2.zero;
            root.offsetMax = Vector2.zero;

            hpText = RuntimeFactory.CreateText("HP", transform, "HP: ♥♥♥♥♥", 42, TextAnchor.UpperLeft, Color.white);
            RectTransform hpRect = hpText.rectTransform;
            hpRect.anchorMin = new Vector2(0f, 1f);
            hpRect.anchorMax = new Vector2(0f, 1f);
            hpRect.anchoredPosition = new Vector2(150f, -60f);
            hpRect.sizeDelta = new Vector2(440f, 50f);

            scoreText = RuntimeFactory.CreateText("Score", transform, "Score: 0", 42, TextAnchor.UpperCenter, Color.white);
            RectTransform scoreRect = scoreText.rectTransform;
            scoreRect.anchorMin = new Vector2(0.5f, 1f);
            scoreRect.anchorMax = new Vector2(0.5f, 1f);
            scoreRect.anchoredPosition = new Vector2(0f, -60f);
            scoreRect.sizeDelta = new Vector2(340f, 50f);

            weaponText = RuntimeFactory.CreateText("Weapon", transform, "Weapon: Sword", 42, TextAnchor.UpperRight, Color.white);
            RectTransform weaponRect = weaponText.rectTransform;
            weaponRect.anchorMin = new Vector2(1f, 1f);
            weaponRect.anchorMax = new Vector2(1f, 1f);
            weaponRect.anchoredPosition = new Vector2(-180f, -60f);
            weaponRect.sizeDelta = new Vector2(440f, 50f);

            BuildPausePanel();
            BuildGameOverPanel();
        }

        private void BuildPausePanel()
        {
            pausePanel = RuntimeFactory.CreatePanel("PausePanel", transform, new Color(0f, 0f, 0f, 0.72f)).gameObject;
            RectTransform rect = pausePanel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(760f, 500f);

            Text title = RuntimeFactory.CreateText("Title", pausePanel.transform, "Paused", 58, TextAnchor.MiddleCenter, Color.white);
            RectTransform titleRect = title.rectTransform;
            titleRect.anchorMin = new Vector2(0.5f, 1f);
            titleRect.anchorMax = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0f, -60f);
            titleRect.sizeDelta = new Vector2(400f, 80f);

            Text volumeLabel = RuntimeFactory.CreateText("VolumeLabel", pausePanel.transform, "Volume", 34, TextAnchor.MiddleCenter, Color.white);
            RectTransform volumeLabelRect = volumeLabel.rectTransform;
            volumeLabelRect.anchorMin = new Vector2(0.5f, 0.5f);
            volumeLabelRect.anchorMax = new Vector2(0.5f, 0.5f);
            volumeLabelRect.anchoredPosition = new Vector2(0f, 70f);
            volumeLabelRect.sizeDelta = new Vector2(300f, 50f);

            Slider slider = RuntimeFactory.CreateSlider("VolumeSlider", pausePanel.transform, new Vector2(420f, 50f), new Color(0.2f, 0.2f, 0.2f), new Color(0.95f, 0.82f, 0.28f), Color.white);
            ((RectTransform)slider.transform).anchoredPosition = new Vector2(0f, 10f);
            slider.value = SaveManager.GetMasterVolume();
            slider.onValueChanged.AddListener(value =>
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetVolume(value);
                }
            });

            Button resumeButton = RuntimeFactory.CreateButton("ResumeButton", pausePanel.transform, "Resume", new Vector2(260f, 76f), new Color(0.2f, 0.4f, 0.7f), () => GameManager.Instance.SetPaused(false));
            ((RectTransform)resumeButton.transform).anchoredPosition = new Vector2(0f, -90f);

            Button mainMenuButton = RuntimeFactory.CreateButton("MainMenuButton", pausePanel.transform, "Main Menu", new Vector2(260f, 76f), new Color(0.65f, 0.2f, 0.2f), () => GameManager.Instance.LoadScene(SceneNames.MainMenu));
            ((RectTransform)mainMenuButton.transform).anchoredPosition = new Vector2(0f, -185f);

            pausePanel.SetActive(false);
        }

        private void BuildGameOverPanel()
        {
            gameOverPanel = RuntimeFactory.CreatePanel("GameOverPanel", transform, new Color(0f, 0f, 0f, 0.55f)).gameObject;
            RectTransform rect = gameOverPanel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(600f, 220f);

            Text label = RuntimeFactory.CreateText("Text", gameOverPanel.transform, "Respawning...", 58, TextAnchor.MiddleCenter, Color.white);
            RectTransform labelRect = label.rectTransform;
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            gameOverPanel.SetActive(false);
        }

        public void ShowWin(int currentScore, int bestScore, bool hasNextLevel, string nextScene)
        {
            if (winPanel != null)
            {
                Destroy(winPanel);
            }

            winPanel = RuntimeFactory.CreatePanel("WinPanel", transform, new Color(0f, 0f, 0f, 0.82f)).gameObject;
            RectTransform rect = winPanel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(820f, 460f);

            Text title = RuntimeFactory.CreateText("Title", winPanel.transform, "Level Complete", 60, TextAnchor.MiddleCenter, Color.white);
            RectTransform titleRect = title.rectTransform;
            titleRect.anchorMin = new Vector2(0.5f, 1f);
            titleRect.anchorMax = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0f, -65f);
            titleRect.sizeDelta = new Vector2(460f, 70f);

            Text score = RuntimeFactory.CreateText("Score", winPanel.transform, $"Score: {currentScore}\nBest: {bestScore}", 42, TextAnchor.MiddleCenter, Color.white);
            RectTransform scoreRect = score.rectTransform;
            scoreRect.anchorMin = new Vector2(0.5f, 0.62f);
            scoreRect.anchorMax = new Vector2(0.5f, 0.62f);
            scoreRect.anchoredPosition = Vector2.zero;
            scoreRect.sizeDelta = new Vector2(300f, 120f);

            Button replayButton = RuntimeFactory.CreateButton("ReplayButton", winPanel.transform, "Replay", new Vector2(240f, 72f), new Color(0.18f, 0.42f, 0.7f), () =>
            {
                GameManager.Instance.SetPaused(false, false);
                GameManager.Instance.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            });

            ((RectTransform)replayButton.transform).anchoredPosition = new Vector2(0f, -70f);

            Button menuButton = RuntimeFactory.CreateButton("MenuButton", winPanel.transform, "Main Menu", new Vector2(240f, 72f), new Color(0.65f, 0.2f, 0.2f), () =>
            {
                GameManager.Instance.SetPaused(false, false);
                GameManager.Instance.LoadScene(SceneNames.MainMenu);
            });

            ((RectTransform)menuButton.transform).anchoredPosition = new Vector2(-135f, -165f);

            Button nextButton = RuntimeFactory.CreateButton("NextButton", winPanel.transform, hasNextLevel ? "Next Level" : "Finish", new Vector2(240f, 72f), new Color(0.2f, 0.55f, 0.2f), () =>
            {
                GameManager.Instance.SetPaused(false, false);
                GameManager.Instance.LoadScene(hasNextLevel ? nextScene : SceneNames.MainMenu);
            });

            ((RectTransform)nextButton.transform).anchoredPosition = new Vector2(135f, -165f);
        }

        private void OnPlayerHealthChanged(int current, int max)
        {
            string hearts = string.Empty;
            for (int index = 0; index < max; index++)
            {
                hearts += index < current ? "♥" : "♡";
            }

            hpText.text = $"HP: {hearts}";
        }

        private void OnScoreChanged(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        private void OnWeaponChanged(string weaponName)
        {
            weaponText.text = $"Weapon: {weaponName}";
        }

        private void OnPauseChanged(bool paused)
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(paused);
            }
        }

        private void OnPlayerDeadChanged(bool dead)
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(dead);
            }
        }
    }
}
