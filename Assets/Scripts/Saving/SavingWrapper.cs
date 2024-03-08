using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string SAVE_FILE = "save";

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