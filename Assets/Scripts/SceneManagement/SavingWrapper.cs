using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string currentSaveKey = "currentSaveFile";
        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] int firstSceneBuildIndex = 1;
        [SerializeField] int menuSceneBuildIndex = 0;

        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }
        public void ResumeGame()
        {
            if(!PlayerPrefs.HasKey(currentSaveKey)) return;
            if(!GetComponent<SavingSystem>().SaveFileExist(GetCurrentSave())) return;
            StartCoroutine(LoadLastScene());
        }
        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ResumeGame();
        }
        public IEnumerable<String> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSave();
        }
        public void NewGame(string newNameFile)
        {
            if(String.IsNullOrEmpty(newNameFile)) return;

            SetCurrentSave(newNameFile);
            StartCoroutine(LoadFirstScene());
        }

        private void SetCurrentSave(string newNameFile)
        {
            PlayerPrefs.SetString(currentSaveKey, newNameFile);
        }
        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadLastScene() {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);
        }
        private IEnumerator LoadFirstScene() {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstSceneBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }
        private IEnumerator LoadMenuScene() {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(menuSceneBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }
        ///=====Debug Section=====///
        
        // private void Update() {
        //     //Save 
        //     if (Input.GetKeyDown(KeyCode.S))
        //     {
        //         Save();
        //     }
        //     //load save file
        //     if (Input.GetKeyDown(KeyCode.L))
        //     {
        //         Load();
        //     }
        //     //Delete save file
        //     if(Input.GetKeyDown(KeyCode.Delete))
        //     {
        //         Delete();
        //     }
        // }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }
    }
}