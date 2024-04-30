using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private LazyValue<float> health;

        private bool isDead = false;

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
        }

        private void Start()
        {
            health.Evaluate();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth()
        {
            health.val = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject attacker, float damage)
        {
            health.val = Math.Max(0f, health.val - damage);

            if (health.val == 0)
            {
                Die();
                AwardExperience(attacker);
            }
        }

        public float GetHealth()
        {
            return health.val;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
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
            return health.val;
        }

        public void RestoreState(object state)
        {
            health.val = (float)state;
            TakeDamage(null, 0);
        }
    }
}