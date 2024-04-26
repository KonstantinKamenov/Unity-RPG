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
        private float health = -1f;

        private bool isDead = false;

        private void Start()
        {
            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void RegenerateHealth()
        {
            print("regen");
            health = GetComponent<BaseStats>().GetStat(Stat.Health);
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
            return health / GetComponent<BaseStats>().GetStat(Stat.Health) * 100.0f;
        }

        private void AwardExperience(GameObject attacker)
        {
            if (attacker == null) return;

            Experience attackerExperience = attacker.GetComponent<Experience>();
            if (attacker != null) attackerExperience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.EXPReward));
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