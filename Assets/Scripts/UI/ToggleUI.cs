using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.UI
{
    public class ToggleUI : MonoBehaviour
    {
       [SerializeField] KeyCode toggleKey = KeyCode.Escape;
       [SerializeField] GameObject uiContainer = null;

       /// <summary>
       /// Start is called on the frame when a script is enabled just before
       /// any of the Update methods is called the first time.
       /// </summary>
       private void Start()
       {
            uiContainer.SetActive(false);
       }
       /// <summary>
       /// Update is called every frame, if the MonoBehaviour is enabled.
       /// </summary>
       private void Update()
       {
            if(Input.GetKeyDown(toggleKey))
            {
                Toggle();
            }
       }
       public void Toggle()
        {
            if(uiContainer.GetComponent<PauseMenu>())
            {
                GameState currentGameState = GameStateManager.Instance.CurrentGameState;
                GameState newGameState = currentGameState == GameState.Gameplay ? GameState.Paused : GameState.Gameplay;

                GameStateManager.Instance.SetState(newGameState);
                print(newGameState);
            }
            uiContainer.SetActive(!uiContainer.gameObject.activeInHierarchy);
        }
    }

}
