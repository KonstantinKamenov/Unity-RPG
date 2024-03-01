using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private Mover mover;

        [SerializeField] private float weaponRange = 2.0f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private float damage = 10f;

        private Health target;
        private float timeSinceLastAttack = 0f;

        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (Vector3.Distance(transform.position, target.transform.position) > weaponRange)
            {
                mover.MoveTo(target.transform.position, 1.0f);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                GetComponent<Animator>().SetTrigger("attack");
                GetComponent<Animator>().ResetTrigger("stopAttack");
                timeSinceLastAttack = 0f;
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
            target = null;
        }

        //Animation event
        public void Hit()
        {
            if (target == null) return;
            target.TakeDamage(damage);
        }
    }
}