using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;

        public float GetStat(Stat stat)
        {
            return progression.GetStat(characterClass, stat, startingLevel);
        }

        public int GetLevel()
        {
            if (GetComponent<Experience>() == null) return startingLevel;

            float currentEXP = GetComponent<Experience>().GetExperience();
            float[] levels = progression.GetStats(characterClass, Stat.TotalEXPToLevel);
            for (int i = 0; i < levels.Length; i++)
            {
                if (currentEXP < levels[i]) return i;
            }

            return 0;
        }
    }
}