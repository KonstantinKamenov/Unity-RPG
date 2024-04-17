using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] classes = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass pcl in classes)
            {
                if (pcl.GetCharacterClass() == characterClass)
                {
                    return pcl.GetHealth(level - 1);
                }
            }
            return 0.0f;
        }

        [System.Serializable]
        private class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass characterClass;
            [SerializeField] private float[] health;

            public CharacterClass GetCharacterClass()
            {
                return characterClass;
            }

            public float GetHealth(int i)
            {
                return health[i];
            }
        }
    }
}