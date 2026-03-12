using System;

namespace SimpleMedievalPlatformer.Systems
{
    public static class GameEvents
    {
        public static event Action<int, int> PlayerHealthChanged;
        public static event Action<int> ScoreChanged;
        public static event Action<string> WeaponChanged;
        public static event Action<bool> PauseChanged;
        public static event Action<bool> PlayerDeadChanged;

        public static void RaisePlayerHealthChanged(int current, int max)
        {
            PlayerHealthChanged?.Invoke(current, max);
        }

        public static void RaiseScoreChanged(int score)
        {
            ScoreChanged?.Invoke(score);
        }

        public static void RaiseWeaponChanged(string weaponName)
        {
            WeaponChanged?.Invoke(weaponName);
        }

        public static void RaisePauseChanged(bool paused)
        {
            PauseChanged?.Invoke(paused);
        }

        public static void RaisePlayerDeadChanged(bool dead)
        {
            PlayerDeadChanged?.Invoke(dead);
        }
    }
}
