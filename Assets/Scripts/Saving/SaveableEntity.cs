using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";
        private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            foreach (ISaveable e in GetComponents<ISaveable>())
            {
                state[e.GetType().ToString()] = e.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = state as Dictionary<string, object>;

            foreach (ISaveable e in GetComponents<ISaveable>())
            {
                string type = e.GetType().ToString();
                if (!stateDict.ContainsKey(type)) continue;

                e.RestoreState(stateDict[type]);
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[uniqueIdentifier] = this;
        }
#endif
        private bool IsUnique(string uuid)
        {
            if (!globalLookup.ContainsKey(uuid)) return true;

            if (globalLookup[uuid] == this) return true;

            if (globalLookup[uuid] == null)
            {
                globalLookup.Remove(uuid);
                return true;
            }

            if (globalLookup[uuid].GetUniqueIdentifier() != uuid)
            {
                globalLookup.Remove(uuid);
                return true;
            }

            return false;
        }
    }
}