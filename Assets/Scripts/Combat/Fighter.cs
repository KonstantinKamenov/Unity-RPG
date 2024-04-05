using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private Mover mover;

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform handPosition = null;
        [SerializeField] private Weapon weapon = null;

        private Health target;
        private float timeSinceLastAttack = 0f;

        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            SpawnWeapon();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (Vector3.Distance(transform.position, target.transform.position) > weapon.GetRange())
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
            GetComponent<Mover>().Cancel();
            target = null;
        }

        private void SpawnWeapon()
        {
            if (weapon == null) return;
            weapon.Spawn(handPosition, GetComponent<Animator>());
        }

        //Animation event
        public void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weapon.GetDamage());
        }
    }
}