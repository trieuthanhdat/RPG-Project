using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.SceneManagement;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class MainMenu : MonoBehaviour
    {
        LazyValue<SavingWrapper> savingWrapper;

        [SerializeField] TMP_InputField newGameInput;

        private void Awake() {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        public void ResumeGame()
        {
            print("Resume");
            savingWrapper.value.ResumeGame();
        }

        public void NewGame()
        {
            savingWrapper.value.NewGame(newGameInput.text);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
    
}
