using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] classes = null;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookup = null;

        public float GetStat(CharacterClass characterClass, Stat stat, int level)
        {
            float[] levels = GetStats(characterClass, stat);
            if (level <= 0 || level > levels.Length)
            {
                return 0;
            }
            return levels[level - 1];
        }

        public float[] GetStats(CharacterClass characterClass, Stat stat)
        {
            BuildLookup();

            float[] levels = lookup[characterClass][stat];
            return levels;
        }

        private void BuildLookup()
        {
            if (lookup != null) return;

            lookup = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass pcl in classes)
            {
                if (!lookup.ContainsKey(pcl.characterClass))
                {
                    lookup[pcl.characterClass] = new Dictionary<Stat, float[]>();
                }

                Dictionary<Stat, float[]> statLookup = lookup[pcl.characterClass];

                foreach (ProgressionStat ps in pcl.stats)
                {
                    statLookup[ps.stat] = ps.levels;
                }
            }
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