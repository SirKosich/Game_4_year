using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleMedievalPlatformer.Systems
{
    public static class BootstrapUtility
    {
        public static void EnsurePersistentSystems()
        {
            if (Object.FindFirstObjectByType<GameManager>() == null)
            {
                new GameObject("GameManager").AddComponent<GameManager>();
            }

            if (Object.FindFirstObjectByType<AudioManager>() == null)
            {
                new GameObject("AudioManager").AddComponent<AudioManager>();
            }
        }

        public static Camera EnsureCamera()
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                return camera;
            }

            GameObject cameraRoot = new GameObject("Main Camera");
            cameraRoot.tag = "MainCamera";

            Camera cam = cameraRoot.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5.6f;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.58f, 0.79f, 0.94f, 1f);

            cameraRoot.AddComponent<AudioListener>();
            return cam;
        }

        public static void EnsureEventSystem()
        {
            if (Object.FindFirstObjectByType<EventSystem>() != null)
            {
                return;
            }

            GameObject root = new GameObject("EventSystem");
            root.AddComponent<EventSystem>();
            root.AddComponent<StandaloneInputModule>();
        }

        public static Canvas EnsureCanvas(string canvasName = "Canvas")
        {
            Canvas existing = Object.FindFirstObjectByType<Canvas>();
            if (existing != null)
            {
                return existing;
            }

            return UI.RuntimeFactory.CreateCanvasRoot(canvasName);
        }
    }
}
