using System;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private float health = 100f;

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
            isDead = true;
        }
    }
}