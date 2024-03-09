using System;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float health = 100f;

        private bool isDead = false;

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