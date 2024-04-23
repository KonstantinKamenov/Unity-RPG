using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] classes = null;

        public float GetStat(CharacterClass characterClass, Stat stat, int level)
        {
            foreach (ProgressionCharacterClass pcl in classes)
            {
                if (pcl.characterClass != characterClass) continue;

                foreach (ProgressionStat ps in pcl.stats)
                {
                    if (ps.stat != stat || level <= 0 || level > ps.levels.Length) continue;

                    return ps.levels[level - 1];
                }
            }
            return 0.0f;
        }

        [System.Serializable]
        private class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        private class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}