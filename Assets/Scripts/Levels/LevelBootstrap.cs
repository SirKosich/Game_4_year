using SimpleMedievalPlatformer.Player;
using SimpleMedievalPlatformer.Systems;
using SimpleMedievalPlatformer.UI;
using UnityEngine;

namespace SimpleMedievalPlatformer.Levels
{
    [DisallowMultipleComponent]
    public sealed class LevelBootstrap : MonoBehaviour
    {
        private Transform worldRoot;

        private void Start()
        {
            BootstrapUtility.EnsurePersistentSystems();
            BootstrapUtility.EnsureEventSystem();

            if (GetComponent<LevelManager>() == null)
            {
                gameObject.AddComponent<LevelManager>();
            }

            BuildLevel();
        }

        private void BuildLevel()
        {
            worldRoot = new GameObject("World").transform;

            Camera camera = BootstrapUtility.EnsureCamera();
            CameraFollow cameraFollow = camera.GetComponent<CameraFollow>();
            if (cameraFollow == null)
            {
                cameraFollow = camera.gameObject.AddComponent<CameraFollow>();
            }

            Canvas canvas = BootstrapUtility.EnsureCanvas("GameCanvas");
            GameObject hudRoot = new GameObject("HUD", typeof(RectTransform), typeof(HUDController));
            hudRoot.transform.SetParent(canvas.transform, false);

            GameObject mobileControlsPrefab = Resources.Load<GameObject>("Prefabs/MobileControlPanel");
            if (mobileControlsPrefab != null)
            {
                GameObject controls = Instantiate(mobileControlsPrefab, canvas.transform);
                controls.name = "MobileControlPanel";
            }

            BuildBackground();
            SpawnPlayer(cameraFollow);

            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (sceneName == SceneNames.Level1Forest)
            {
                BuildForestLevel(cameraFollow);
            }
            else
            {
                BuildCastleLevel(cameraFollow);
            }
        }

        private void BuildBackground()
        {
            CreateDecoration("SkyBand", new Vector2(0f, 4f), new Vector2(80f, 12f), new Color(0.66f, 0.82f, 0.98f), -20);
            CreateDecoration("Horizon", new Vector2(8f, 0.5f), new Vector2(90f, 8f), new Color(0.48f, 0.72f, 0.44f), -15);
        }

        private void SpawnPlayer(CameraFollow cameraFollow)
        {
            GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
            GameObject player = Instantiate(playerPrefab, new Vector3(-11f, 2f, 0f), Quaternion.identity);
            player.name = "Player";
            cameraFollow.SetTarget(player.transform);
            LevelManager.Current.RegisterPlayer(player.GetComponent<PlayerHealth>());
            GameManager.Instance.RegisterPlayer(player.GetComponent<PlayerController>());
        }

        private void BuildForestLevel(CameraFollow cameraFollow)
        {
            cameraFollow.SetBounds(new Vector2(-11f, -2f), new Vector2(25f, 8f));

            CreateGround(new Vector2(-8f, -1.2f), new Vector2(12f, 1.5f));
            CreateGround(new Vector2(5f, -1.2f), new Vector2(11f, 1.5f));
            CreateGround(new Vector2(18f, -1.2f), new Vector2(13f, 1.5f));
            CreateGround(new Vector2(-1f, 1.3f), new Vector2(3f, 0.6f));
            CreateGround(new Vector2(9.5f, 1.8f), new Vector2(3f, 0.6f));
            CreateGround(new Vector2(15f, 3.2f), new Vector2(3f, 0.6f));
            CreateGround(new Vector2(23f, 2.5f), new Vector2(4f, 0.6f));

            SpawnEnemy("SkeletonPatrol", new Vector2(-3f, 0.2f));
            SpawnEnemy("SkeletonPatrol", new Vector2(6f, 0.2f));
            SpawnEnemy("Archer", new Vector2(15f, 4.2f));

            SpawnCheckpoint(new Vector2(10f, 2.8f));

            SpawnCoinRow(-6f, 2.1f, 4);
            SpawnCoinRow(2f, 2.4f, 5);
            SpawnCoinRow(13.8f, 4.3f, 3);
            SpawnCoinRow(22f, 3.6f, 4);

            SpawnPortal(new Vector2(26f, 3.4f));
        }

        private void BuildCastleLevel(CameraFollow cameraFollow)
        {
            cameraFollow.SetBounds(new Vector2(-11f, -2f), new Vector2(32f, 12f));

            CreateGround(new Vector2(-8f, -1.2f), new Vector2(12f, 1.5f));
            CreateGround(new Vector2(6f, -1.2f), new Vector2(10f, 1.5f));
            CreateGround(new Vector2(19f, -1.2f), new Vector2(8f, 1.5f));
            CreateGround(new Vector2(29f, -1.2f), new Vector2(8f, 1.5f));
            CreateGround(new Vector2(-1f, 1.6f), new Vector2(3.5f, 0.6f));
            CreateGround(new Vector2(6f, 3.1f), new Vector2(3.5f, 0.6f));
            CreateGround(new Vector2(12f, 4.7f), new Vector2(3.5f, 0.6f));
            CreateGround(new Vector2(19f, 6.2f), new Vector2(4f, 0.6f));
            CreateGround(new Vector2(26.5f, 4f), new Vector2(3.5f, 0.6f));

            SpawnEnemy("SkeletonPatrol", new Vector2(-2f, 0.2f));
            SpawnEnemy("SkeletonPatrol", new Vector2(9f, 0.2f));
            SpawnEnemy("Archer", new Vector2(12f, 5.7f));
            SpawnEnemy("Archer", new Vector2(20f, 7.2f));
            SpawnEnemy("SkeletonPatrol", new Vector2(28f, 0.2f));

            SpawnCheckpoint(new Vector2(7f, 4.1f));
            SpawnCheckpoint(new Vector2(20f, 7.2f));

            SpawnCoinRow(-5.5f, 2f, 5);
            SpawnCoinRow(6f, 4.3f, 4);
            SpawnCoinRow(12f, 5.9f, 4);
            SpawnCoinRow(19f, 7.7f, 5);
            SpawnCoinRow(27f, 5.1f, 4);

            SpawnPortal(new Vector2(32f, 0.7f));
        }

        private void CreateGround(Vector2 position, Vector2 size)
        {
            GameObject ground = CreateDecoration("Ground", position, size, new Color(0.38f, 0.26f, 0.14f), 0);
            ground.layer = 0;
            BoxCollider2D collider = ground.AddComponent<BoxCollider2D>();
            collider.size = Vector2.one;
        }

        private GameObject CreateDecoration(string name, Vector2 position, Vector2 size, Color color, int sortingOrder)
        {
            GameObject block = new GameObject(name, typeof(RuntimeVisual));
            block.transform.SetParent(worldRoot, false);
            block.transform.position = new Vector3(position.x, position.y, 0f);

            RuntimeVisual visual = block.GetComponent<RuntimeVisual>();
            visual.SetCustom(color, size, sortingOrder);

            return block;
        }

        private void SpawnEnemy(string resourceName, Vector2 position)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + resourceName);
            if (prefab == null)
            {
                return;
            }

            GameObject instance = Instantiate(prefab, new Vector3(position.x, position.y, 0f), Quaternion.identity);
            instance.name = resourceName;
        }

        private void SpawnCheckpoint(Vector2 position)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Checkpoint");
            if (prefab == null)
            {
                return;
            }

            GameObject checkpoint = Instantiate(prefab, new Vector3(position.x, position.y, 0f), Quaternion.identity);
            checkpoint.name = "Checkpoint";
        }

        private void SpawnPortal(Vector2 position)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/LevelPortal");
            if (prefab == null)
            {
                return;
            }

            GameObject portal = Instantiate(prefab, new Vector3(position.x, position.y, 0f), Quaternion.identity);
            portal.name = "LevelPortal";
        }

        private void SpawnCoinRow(float startX, float y, int count)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Coin");
            if (prefab == null)
            {
                return;
            }

            for (int index = 0; index < count; index++)
            {
                Vector3 position = new Vector3(startX + (index * 0.9f), y, 0f);
                GameObject coin = Instantiate(prefab, position, Quaternion.identity);
                coin.name = "Coin";
            }
        }
    }
}
