using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagment
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defualtSavingFile = "save";
        [SerializeField] float fadeInTime = 0.3f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadLastScene()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defualtSavingFile);
            Fader fader = FindAnyObjectByType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defualtSavingFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defualtSavingFile);
        }
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defualtSavingFile);
        }
    }

}