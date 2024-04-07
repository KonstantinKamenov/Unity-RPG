using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string SAVE_FILE = "save";

        private IEnumerator Start()
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
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(SAVE_FILE);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(SAVE_FILE);
        }
    }
}