using UnityEngine;

namespace RPG.Core
{
    public class PersistnetObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentObjects;

        private bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObjects();
            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject spawnedObjects = Instantiate(persistentObjects);
            DontDestroyOnLoad(spawnedObjects);
        }
    }
}