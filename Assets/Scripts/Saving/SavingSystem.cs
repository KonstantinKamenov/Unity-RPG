using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromFile(saveFile);
            Debug.Log("Saving to " + path);
            using (FileStream fileStream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, state);
            }
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int scene = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey("scene"))
            {
                scene = (int)state["scene"];
            }
            yield return SceneManager.LoadSceneAsync(scene);
            RestoreState(state);
        }

        public IEnumerator LoadLastScene1(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            if (state.ContainsKey("scene"))
            {
                int scene = (int)state["scene"];
                if (scene != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(scene);
                }
            }
            RestoreState(state);
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            Debug.Log("Loading from " + path);
            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(fileStream) as Dictionary<string, object>;
            }
        }

        public void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity e in FindObjectsOfType<SaveableEntity>())
            {
                state[e.GetUniqueIdentifier()] = e.CaptureState();
            }

            state["scene"] = SceneManager.GetActiveScene().buildIndex;
        }

        public void RestoreState(Dictionary<string, object> state)
        {
            Dictionary<string, object> dict = state;
            foreach (SaveableEntity e in GameObject.FindObjectsOfType<SaveableEntity>())
            {
                if (!dict.ContainsKey(e.GetUniqueIdentifier())) continue;
                e.RestoreState(dict[e.GetUniqueIdentifier()]);
            }
        }

        private string GetPathFromFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromFile(saveFile));
        }
    }
}