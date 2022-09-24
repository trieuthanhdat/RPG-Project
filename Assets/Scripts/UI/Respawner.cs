using System;
using System.Collections;
using Cinemachine;
using RPG.Core;
using RPG.Resources;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour {
        [SerializeField] Transform respawnLocation;
        [SerializeField] float respawnDelay = 3;
        [SerializeField] float fadeTime = 0.2f;
        [SerializeField] float healthRegenPercentage = 50;
        [SerializeField] float manaRegenPercnetage = 40;
        [SerializeField] float enemyHealthRegenPercentage = 15;

        private void Awake()
        {
            GetComponent<Health>().OnDie.AddListener(Respawn);
        }

        private void Start() {
            if (GetComponent<Health>().IsDead())
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            // savingWrapper.Save();
            yield return new WaitForSeconds(respawnDelay);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);

            RespawnPlayer();
            ResetEnemies();
            // savingWrapper.Save();
            yield return fader.FadeIn(fadeTime);
        }

        private void ResetEnemies()
        {
            foreach (AIController enemyControllers in FindObjectsOfType<AIController>())
            {
                Health health = enemyControllers.GetComponent<Health>();
                if (health && !health.IsDead())
                {
                    enemyControllers.Reset();
                    health.Heal(health.GetMaxHealthPoint() * enemyHealthRegenPercentage / 100);
                }
            }
        }

        private void RespawnPlayer()
        {
            GetComponent<FieldOfView>().canSeeTarget = false;
            Vector3 postionDelta = respawnLocation.position - transform.position;
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            Health health = GetComponent<Health>();
            Mana mana = GetComponent<Mana>();
            health.Heal(health.GetMaxHealthPoint() * healthRegenPercentage / 100);
            mana.RestoreMana(mana.GetMaxManaPoint() * manaRegenPercnetage/100);
            
            ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if (activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, postionDelta);
            }
        }
    }
}