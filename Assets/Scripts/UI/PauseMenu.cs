using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using System;
using RPG.SceneManagement;
using RPG.Core;

namespace RPG.UI
{
    public class PauseMenu : MonoBehaviour
    {
        private bool isPaused = false;
        [SerializeField] PlayerController playerController ;
        [SerializeField] GameObject notification;
        // /// <summary>
        // /// Awake is called when the script instance is being loaded.
        // /// </summary>
        // private void Awake()
        // {
        //     playerController = GameObject.FindObjectOfType<PlayerController>();
        // }
        private void OnEnable()
        {
           GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }
        private void OnDisable()
        {
            GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState newState)
        {
            playerController.enabled = newState == GameState.Gameplay;
            Time.timeScale = newState == GameState.Gameplay ? 1f : 0f;
        }

        
        public void Save()
        {
            var savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            gameObject.SetActive(false);
            if(notification != null)
                notification.SetActive(true);
        }
        public void SaveAndQuit()
        {
            var savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            savingWrapper.LoadMenu();
        }
        public void QuitToDesktop()
        {
            var savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }

}
