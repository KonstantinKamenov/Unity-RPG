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

        private bool isDead = false;

        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            health = Math.Max(0f, health - damage);

            if (health == 0)
            {
                Die();
            }
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
            TakeDamage(0);
        }
    }
}