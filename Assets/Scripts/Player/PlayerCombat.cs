using System.Collections.Generic;
using SimpleMedievalPlatformer.Enemies;
using SimpleMedievalPlatformer.Systems;
using SimpleMedievalPlatformer.Weapons;
using UnityEngine;

namespace SimpleMedievalPlatformer.Player
{
    [DisallowMultipleComponent]
    public sealed class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private WeaponStatsSO swordStats;
        [SerializeField] private WeaponStatsSO bowStats;

        private WeaponType currentWeapon = WeaponType.Sword;
        private float lastAttackTime = -999f;
        private PlayerController controller;

        private void Awake()
        {
            controller = GetComponent<PlayerController>();

            if (swordStats == null)
            {
                swordStats = Resources.Load<WeaponStatsSO>("ScriptObjects/Weapon_Sword");
            }

            if (bowStats == null)
            {
                bowStats = Resources.Load<WeaponStatsSO>("ScriptObjects/Weapon_Bow");
            }

            GameEvents.RaiseWeaponChanged(GetCurrentStats().displayName);
        }

        public void TryAttack(Vector2 facingDirection)
        {
            WeaponStatsSO stats = GetCurrentStats();
            if (stats == null)
            {
                return;
            }

            if (Time.time < lastAttackTime + stats.cooldown)
            {
                return;
            }

            lastAttackTime = Time.time;

            if (currentWeapon == WeaponType.Sword)
            {
                DoMeleeAttack(facingDirection, stats);
            }
            else
            {
                FireProjectile(facingDirection, stats);
            }
        }

        public void SwitchWeapon()
        {
            currentWeapon = currentWeapon == WeaponType.Sword ? WeaponType.Bow : WeaponType.Sword;
            GameEvents.RaiseWeaponChanged(GetCurrentStats().displayName);
        }

        private WeaponStatsSO GetCurrentStats()
        {
            return currentWeapon == WeaponType.Sword ? swordStats : bowStats;
        }

        private void DoMeleeAttack(Vector2 facingDirection, WeaponStatsSO stats)
        {
            Vector2 center = (Vector2)transform.position + facingDirection.normalized * (stats.range * 0.55f);
            Vector2 size = new Vector2(stats.range, 0.9f);
            Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f);

            HashSet<EnemyHealth> uniqueHits = new HashSet<EnemyHealth>();
            foreach (Collider2D hit in hits)
            {
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                if (enemy == null || uniqueHits.Contains(enemy))
                {
                    continue;
                }

                uniqueHits.Add(enemy);
                enemy.TakeDamage(stats.damage, transform.position);
            }
        }

        private void FireProjectile(Vector2 facingDirection, WeaponStatsSO stats)
        {
            GameObject projectilePrefab = Resources.Load<GameObject>("Prefabs/ArrowShell");
            if (projectilePrefab == null)
            {
                return;
            }

            Vector3 spawnPosition = transform.position + new Vector3(facingDirection.normalized.x * 0.9f, 0.2f, 0f);
            GameObject projectileObject = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Launch(facingDirection.normalized, stats.projectileSpeed, stats.damage, true, controller.BodyCollider);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (swordStats == null)
            {
                return;
            }

            Vector2 facing = controller != null && !controller.FacingRight ? Vector2.left : Vector2.right;
            Vector2 center = (Vector2)transform.position + facing * (swordStats.range * 0.55f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(center, new Vector3(swordStats.range, 0.9f, 0f));
        }
#endif
    }
}
