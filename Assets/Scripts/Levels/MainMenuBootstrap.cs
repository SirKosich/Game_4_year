using SimpleMedievalPlatformer.Systems;
using SimpleMedievalPlatformer.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleMedievalPlatformer.Levels
{
    [DisallowMultipleComponent]
    public sealed class MainMenuBootstrap : MonoBehaviour
    {
        private GameObject settingsPanel;

        private void Start()
        {
            BootstrapUtility.EnsurePersistentSystems();
            BootstrapUtility.EnsureEventSystem();
            BootstrapUtility.EnsureCamera();

            BuildMenu();
        }

        private void BuildMenu()
        {
            Canvas canvas = BootstrapUtility.EnsureCanvas("MainMenuCanvas");

            Image background = RuntimeFactory.CreatePanel("Background", canvas.transform, new Color(0.1f, 0.14f, 0.2f, 1f));
            RectTransform backgroundRect = background.rectTransform;
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.anchorMax = Vector2.one;
            backgroundRect.offsetMin = Vector2.zero;
            backgroundRect.offsetMax = Vector2.zero;

            Text title = RuntimeFactory.CreateText("Title", canvas.transform, "Simple Medieval Platformer", 72, TextAnchor.MiddleCenter, new Color(1f, 0.92f, 0.72f));
            RectTransform titleRect = title.rectTransform;
            titleRect.anchorMin = new Vector2(0.5f, 1f);
            titleRect.anchorMax = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0f, -120f);
            titleRect.sizeDelta = new Vector2(1200f, 100f);

            Text subtitle = RuntimeFactory.CreateText("Subtitle", canvas.transform, "2D • Android • Touch + Keyboard", 34, TextAnchor.MiddleCenter, Color.white);
            RectTransform subtitleRect = subtitle.rectTransform;
            subtitleRect.anchorMin = new Vector2(0.5f, 1f);
            subtitleRect.anchorMax = new Vector2(0.5f, 1f);
            subtitleRect.anchoredPosition = new Vector2(0f, -190f);
            subtitleRect.sizeDelta = new Vector2(900f, 50f);

            Button playButton = RuntimeFactory.CreateButton("PlayButton", canvas.transform, "Play", new Vector2(360f, 90f), new Color(0.2f, 0.55f, 0.2f), OnPlayPressed);
            ((RectTransform)playButton.transform).anchoredPosition = new Vector2(0f, 130f);

            Button forestButton = RuntimeFactory.CreateButton("ForestButton", canvas.transform, "Level 1 - Forest", new Vector2(360f, 90f), new Color(0.18f, 0.42f, 0.7f), () => GameManager.Instance.LoadScene(SceneNames.Level1Forest));
            ((RectTransform)forestButton.transform).anchoredPosition = new Vector2(0f, 20f);

            Button castleButton = RuntimeFactory.CreateButton("CastleButton", canvas.transform, "Level 2 - Castle", new Vector2(360f, 90f), new Color(0.5f, 0.25f, 0.65f), () => GameManager.Instance.LoadScene(SceneNames.Level2Castle));
            ((RectTransform)castleButton.transform).anchoredPosition = new Vector2(0f, -90f);
            castleButton.interactable = SaveManager.GetUnlockedLevel() >= 2;

            Button settingsButton = RuntimeFactory.CreateButton("SettingsButton", canvas.transform, "Settings", new Vector2(360f, 90f), new Color(0.45f, 0.36f, 0.16f), ToggleSettings);
            ((RectTransform)settingsButton.transform).anchoredPosition = new Vector2(0f, -200f);

            Button exitButton = RuntimeFactory.CreateButton("ExitButton", canvas.transform, "Exit", new Vector2(360f, 90f), new Color(0.65f, 0.2f, 0.2f), ExitGame);
            ((RectTransform)exitButton.transform).anchoredPosition = new Vector2(0f, -310f);

            Text bestScores = RuntimeFactory.CreateText(
                "BestScores",
                canvas.transform,
                $"Best Forest: {SaveManager.GetBestScore(SceneNames.Level1Forest)}\nBest Castle: {SaveManager.GetBestScore(SceneNames.Level2Castle)}",
                30,
                TextAnchor.UpperLeft,
                Color.white);

            RectTransform bestScoresRect = bestScores.rectTransform;
            bestScoresRect.anchorMin = new Vector2(0f, 1f);
            bestScoresRect.anchorMax = new Vector2(0f, 1f);
            bestScoresRect.anchoredPosition = new Vector2(180f, -140f);
            bestScoresRect.sizeDelta = new Vector2(420f, 120f);

            Text controls = RuntimeFactory.CreateText(
                "Controls",
                canvas.transform,
                "Editor Keyboard\nA / D = Move\nSpace = Jump\nJ = Attack\nK = Switch\nEsc = Pause",
                28,
                TextAnchor.UpperRight,
                Color.white);

            RectTransform controlsRect = controls.rectTransform;
            controlsRect.anchorMin = new Vector2(1f, 1f);
            controlsRect.anchorMax = new Vector2(1f, 1f);
            controlsRect.anchoredPosition = new Vector2(-170f, -140f);
            controlsRect.sizeDelta = new Vector2(360f, 220f);

            BuildSettingsPanel(canvas.transform);
        }

        private void BuildSettingsPanel(Transform parent)
        {
            settingsPanel = RuntimeFactory.CreatePanel("SettingsPanel", parent, new Color(0f, 0f, 0f, 0.78f)).gameObject;
            RectTransform rect = settingsPanel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(720f, 340f);
            rect.anchoredPosition = new Vector2(0f, -20f);

            Text title = RuntimeFactory.CreateText("Title", settingsPanel.transform, "Settings", 52, TextAnchor.MiddleCenter, Color.white);
            RectTransform titleRect = title.rectTransform;
            titleRect.anchorMin = new Vector2(0.5f, 1f);
            titleRect.anchorMax = new Vector2(0.5f, 1f);
            titleRect.anchoredPosition = new Vector2(0f, -50f);
            titleRect.sizeDelta = new Vector2(500f, 70f);

            Text volumeLabel = RuntimeFactory.CreateText("VolumeLabel", settingsPanel.transform, "Master Volume", 36, TextAnchor.MiddleCenter, Color.white);
            RectTransform volumeLabelRect = volumeLabel.rectTransform;
            volumeLabelRect.anchorMin = new Vector2(0.5f, 0.5f);
            volumeLabelRect.anchorMax = new Vector2(0.5f, 0.5f);
            volumeLabelRect.anchoredPosition = new Vector2(0f, 40f);
            volumeLabelRect.sizeDelta = new Vector2(500f, 50f);

            Slider slider = RuntimeFactory.CreateSlider("VolumeSlider", settingsPanel.transform, new Vector2(480f, 54f), new Color(0.2f, 0.2f, 0.2f), new Color(0.95f, 0.82f, 0.28f), Color.white);
            RectTransform sliderRect = (RectTransform)slider.transform;
            sliderRect.anchoredPosition = new Vector2(0f, -25f);
            slider.value = SaveManager.GetMasterVolume();
            slider.onValueChanged.AddListener(value =>
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetVolume(value);
                }
            });

            Button closeButton = RuntimeFactory.CreateButton("CloseButton", settingsPanel.transform, "Close", new Vector2(260f, 76f), new Color(0.2f, 0.4f, 0.7f), ToggleSettings);
            ((RectTransform)closeButton.transform).anchoredPosition = new Vector2(0f, -115f);

            settingsPanel.SetActive(false);
        }

        private void OnPlayPressed()
        {
            GameManager.Instance.LoadHighestUnlockedLevel();
        }

        private void ToggleSettings()
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
