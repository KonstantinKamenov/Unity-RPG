using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;
        [SerializeField] private GameObject levelUpParticleEffect;

        public event Action onLevelUp;

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
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            if (levelUpParticleEffect == null) return;
            Instantiate(levelUpParticleEffect, transform);
        }

        private float GetAdditiveModiviers(Stat stat)
        {
            float statValue = 0.0f;
            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            foreach (IModifierProvider provider in providers)
            {
                foreach (float additiveModifier in provider.GetAdditiveModifier(stat))
                {
                    statValue += additiveModifier;
                }
            }
            return statValue;
        }

        private float GetPercentageModiviers(Stat stat)
        {
            float statValue = 0.0f;
            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            foreach (IModifierProvider provider in providers)
            {
                foreach (float additiveModifier in provider.GetPercentageModifiers(stat))
                {
                    statValue += additiveModifier;
                }
            }
            return statValue;
        }

        public float GetStat(Stat stat)
        {
            return (progression.GetStat(characterClass, stat, GetLevel()) + GetAdditiveModiviers(stat)) * (1 + GetPercentageModiviers(stat) / 100.0f);
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