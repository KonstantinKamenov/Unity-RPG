using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experience = 0.0f;

        public float GetExperience()
        {
            return experience;
        }

        public void GainExperience(float experience)
        {
            this.experience += experience;
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