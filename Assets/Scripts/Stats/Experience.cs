using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experience = 0.0f;

        public event Action onExperienceGained;

        public float GetExperience()
        {
            return experience;
        }

        public void GainExperience(float experience)
        {
            this.experience += experience;
            onExperienceGained();
        }

        public object CaptureState()
        {
            return experience;
        }

        public void RestoreState(object state)
        {
            experience = (float)state;
        }
    }
}