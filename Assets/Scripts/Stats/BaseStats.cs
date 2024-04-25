using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;

        private int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();

            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                print("level up");
                currentLevel = newLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(characterClass, stat, GetLevel());
        }

        public int GetLevel()
        {
            if (currentLevel == 0)
            {
                CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel()
        {
            if (GetComponent<Experience>() == null) return startingLevel;

            float currentEXP = GetComponent<Experience>().GetExperience();
            float[] levels = progression.GetStats(characterClass, Stat.TotalEXPToLevel);
            for (int i = 0; i < levels.Length; i++)
            {
                if (currentEXP < levels[i]) return i;
            }

            return 1;
        }
    }
}