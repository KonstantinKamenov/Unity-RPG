using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float health = 100f;
        private float maxHealth;

        private bool isDead = false;

        private void Start()
        {
            maxHealth = GetComponent<BaseStats>().GetHealth();
            health = GetComponent<BaseStats>().GetHealth();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject attacker, float damage)
        {
            health = Math.Max(0f, health - damage);

            if (health == 0)
            {
                Die();
                AwardExperience(attacker);
            }
        }

        public float GetPercentage()
        {
            return health / maxHealth * 100.0f;
        }

        private void AwardExperience(GameObject attacker)
        {
            Experience attackerExperience = attacker.GetComponent<Experience>();
            if (attacker != null) attackerExperience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
        }

        public void Die()
        {
            if (isDead) return;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelAction();
            NavMeshAgent navMesh = GetComponent<NavMeshAgent>();
            if (navMesh != null) navMesh.enabled = false;

            isDead = true;
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            TakeDamage(null, 0);
        }
    }
}