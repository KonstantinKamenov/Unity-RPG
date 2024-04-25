using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string SAVE_FILE = "save";

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutCompete();
            yield return GetComponent<SavingSystem>().LoadLastScene(SAVE_FILE);
            yield return fader.FadeIn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Delete();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(SAVE_FILE);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(SAVE_FILE);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(SAVE_FILE);
        }
    }
}