using UnityEngine;

namespace SimpleMedievalPlatformer.Systems
{
    [DisallowMultipleComponent]
    public sealed class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

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
            ApplySavedVolume();
        }

        public void ApplySavedVolume()
        {
            AudioListener.volume = SaveManager.GetMasterVolume();
        }

        public void SetVolume(float value)
        {
            SaveManager.SetMasterVolume(value);
            AudioListener.volume = SaveManager.GetMasterVolume();
        }
    }
}
