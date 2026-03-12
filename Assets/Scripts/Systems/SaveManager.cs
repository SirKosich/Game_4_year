using UnityEngine;

namespace SimpleMedievalPlatformer.Systems
{
    public static class SaveManager
    {
        private const string UnlockedLevelKey = "UnlockedLevel";
        private const string VolumeKey = "MasterVolume";
        private const string BestScorePrefix = "BestScore_";

        public static void InitializeDefaults()
        {
            if (!PlayerPrefs.HasKey(UnlockedLevelKey))
            {
                PlayerPrefs.SetInt(UnlockedLevelKey, 1);
            }

            if (!PlayerPrefs.HasKey(VolumeKey))
            {
                PlayerPrefs.SetFloat(VolumeKey, 0.8f);
            }

            PlayerPrefs.Save();
        }

        public static int GetUnlockedLevel()
        {
            InitializeDefaults();
            return Mathf.Clamp(PlayerPrefs.GetInt(UnlockedLevelKey, 1), 1, 2);
        }

        public static void UnlockNextLevel(string currentScene)
        {
            int currentLevel = SceneNames.GetLevelNumber(currentScene);
            int nextLevel = Mathf.Clamp(currentLevel + 1, 1, 2);
            int unlockedLevel = GetUnlockedLevel();

            if (nextLevel > unlockedLevel)
            {
                PlayerPrefs.SetInt(UnlockedLevelKey, nextLevel);
                PlayerPrefs.Save();
            }
        }

        public static int GetBestScore(string sceneName)
        {
            return PlayerPrefs.GetInt(BestScorePrefix + sceneName, 0);
        }

        public static int SaveBestScore(string sceneName, int score)
        {
            int best = GetBestScore(sceneName);
            if (score > best)
            {
                best = score;
                PlayerPrefs.SetInt(BestScorePrefix + sceneName, best);
                PlayerPrefs.Save();
            }

            return best;
        }

        public static float GetMasterVolume()
        {
            InitializeDefaults();
            return PlayerPrefs.GetFloat(VolumeKey, 0.8f);
        }

        public static void SetMasterVolume(float value)
        {
            PlayerPrefs.SetFloat(VolumeKey, Mathf.Clamp01(value));
            PlayerPrefs.Save();
        }

        public static void ResetProgress()
        {
            PlayerPrefs.DeleteKey(UnlockedLevelKey);
            PlayerPrefs.DeleteKey(VolumeKey);
            PlayerPrefs.DeleteKey(BestScorePrefix + SceneNames.Level1Forest);
            PlayerPrefs.DeleteKey(BestScorePrefix + SceneNames.Level2Castle);
            PlayerPrefs.Save();
            InitializeDefaults();
        }
    }
}
