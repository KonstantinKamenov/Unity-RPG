using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AI;

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
            foreach (SaveableEntity e in GameObject.FindObjectsOfType<SaveableEntity>())
            {
                state[e.GetUniqueIdentifier()] = e.CaptureState();
            }
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
    }
}