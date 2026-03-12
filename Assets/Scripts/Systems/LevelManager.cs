using System.Collections;
using UnityEngine;

namespace SimpleMedievalPlatformer.Systems
{
    [DisallowMultipleComponent]
    public sealed class LevelManager : MonoBehaviour
    {
        public static LevelManager Current { get; private set; }

        private Vector3 spawnPoint;
        private Vector3 checkpointPoint;
        private Player.PlayerHealth playerHealth;

        private void Awake()
        {
            Current = this;
        }

        private void OnDestroy()
        {
            if (Current == this)
            {
                Current = null;
            }
        }

        public void RegisterPlayer(Player.PlayerHealth targetHealth)
        {
            playerHealth = targetHealth;
            spawnPoint = targetHealth.transform.position;
            checkpointPoint = spawnPoint;
        }

        public void SetCheckpoint(Vector3 point)
        {
            checkpointPoint = point;
        }

        public void HandlePlayerDeath(Player.PlayerHealth deadPlayer)
        {
            if (playerHealth == null)
            {
                playerHealth = deadPlayer;
            }

            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            GameEvents.RaisePlayerDeadChanged(true);
            yield return new WaitForSeconds(1.1f);

            if (playerHealth != null)
            {
                playerHealth.RespawnAt(checkpointPoint);
            }

            GameEvents.RaisePlayerDeadChanged(false);
        }
    }
}
