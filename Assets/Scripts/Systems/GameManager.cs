using UnityEngine;
using UnityEngine.SceneManagement;

namespace SimpleMedievalPlatformer.Systems
{
    [DisallowMultipleComponent]
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Player.PlayerController Player { get; private set; }
        public int CurrentScore { get; private set; }
        public bool IsPaused { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            SaveManager.InitializeDefaults();
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Player = null;
            SetPaused(false, false);

            if (scene.name == SceneNames.MainMenu)
            {
                ResetScore();
            }
            else
            {
                ResetScore();
            }
        }

        public void RegisterPlayer(Player.PlayerController player)
        {
            Player = player;
        }

        public void ResetScore()
        {
            CurrentScore = 0;
            GameEvents.RaiseScoreChanged(CurrentScore);
        }

        public void AddScore(int amount)
        {
            CurrentScore += Mathf.Max(0, amount);
            GameEvents.RaiseScoreChanged(CurrentScore);
        }

        public void SetPaused(bool paused, bool broadcast = true)
        {
            IsPaused = paused;
            Time.timeScale = paused ? 0f : 1f;

            if (broadcast)
            {
                GameEvents.RaisePauseChanged(paused);
            }
        }

        public void TogglePause()
        {
            if (SceneManager.GetActiveScene().name == SceneNames.MainMenu)
            {
                return;
            }

            SetPaused(!IsPaused, true);
        }

        public void LoadScene(string sceneName)
        {
            SetPaused(false, false);
            SceneManager.LoadScene(sceneName);
        }

        public void LoadHighestUnlockedLevel()
        {
            LoadScene(SceneNames.GetHighestUnlockedScene());
        }

        public void LoadNextLevel()
        {
            LoadScene(SceneNames.GetNextLevel(SceneManager.GetActiveScene().name));
        }

        public int CompleteCurrentLevel()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SaveManager.UnlockNextLevel(sceneName);
            return SaveManager.SaveBestScore(sceneName, CurrentScore);
        }
    }
}
