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
            string path = GetPathFromFile(saveFile);
            Debug.Log("Saving to " + path);
            using (FileStream fileStream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, CaptureState());
            }
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromFile(saveFile);
            Debug.Log("Loading from " + path);
            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                object state = binaryFormatter.Deserialize(fileStream);
                RestoreState(state);
            }
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (SaveableEntity e in GameObject.FindObjectsOfType<SaveableEntity>())
            {
                state[e.GetUniqueIdentifier()] = e.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> dict = state as Dictionary<string, object>;
            foreach (SaveableEntity e in GameObject.FindObjectsOfType<SaveableEntity>())
            {
                e.RestoreState(dict[e.GetUniqueIdentifier()]);
            }
        }

        private string GetPathFromFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}