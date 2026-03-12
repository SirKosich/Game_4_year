using UnityEngine;

namespace SimpleMedievalPlatformer.Systems
{
    public static class SceneNames
    {
        public const string MainMenu = "MainMenu";
        public const string Level1Forest = "Level1_Forest";
        public const string Level2Castle = "Level2_Castle";

        public static readonly string[] Levels =
        {
            Level1Forest,
            Level2Castle
        };

        public static int GetLevelNumber(string sceneName)
        {
            if (sceneName == Level1Forest) return 1;
            if (sceneName == Level2Castle) return 2;
            return 0;
        }

        public static string GetSceneByLevelNumber(int levelNumber)
        {
            return levelNumber switch
            {
                1 => Level1Forest,
                2 => Level2Castle,
                _ => Level1Forest
            };
        }

        public static string GetNextLevel(string sceneName)
        {
            return sceneName switch
            {
                Level1Forest => Level2Castle,
                _ => MainMenu
            };
        }

        public static string GetHighestUnlockedScene()
        {
            return GetSceneByLevelNumber(SaveManager.GetUnlockedLevel());
        }
    }
}
