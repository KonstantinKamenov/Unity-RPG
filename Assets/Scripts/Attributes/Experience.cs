using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float experience = 0.0f;

        public void GainExperience(float experience)
        {
            this.experience += experience;
        }
    }
}