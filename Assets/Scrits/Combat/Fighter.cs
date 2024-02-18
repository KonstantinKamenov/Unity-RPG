using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private Mover mover;

        [SerializeField]
        private float weaponRange = 2.0f;
        [SerializeField]
        private float timeBetweenAttacks = 1f;
        [SerializeField]
        private float damage = 10f;

        private Transform target;
        private float timeSinceLastAttack = 0f;

        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (Vector3.Distance(transform.position, target.position) > weaponRange)
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();

                if (timeSinceLastAttack >= timeBetweenAttacks)
                {
                    GetComponent<Animator>().SetTrigger("attack");
                    timeSinceLastAttack = 0f;
                }
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }

        //Animation event
        public void Hit()
        {
            Health enemyHealth = target.GetComponent<Health>();
            enemyHealth.TakeDamage(damage);
        }
    }
}