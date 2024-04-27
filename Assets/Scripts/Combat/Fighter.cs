using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        private Mover mover;

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandPosition = null;
        [SerializeField] private Transform leftHandPosition = null;
        [SerializeField] private Weapon defaultWeapon = null;
        [SerializeField] private bool shouldApplyModifiers = false;

        private Weapon currentWeapon = null;
        private GameObject currentWeaponGameObject = null;

        private Health target;
        private float timeSinceLastAttack = 0f;

        private void Awake()
        {
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (Vector3.Distance(transform.position, target.transform.position) > currentWeapon.GetRange())
            {
                mover.MoveTo(target.transform.position, 1.0f);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        public Health GetTarget()
        {
            return target;
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

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (!shouldApplyModifiers) yield return 0.0f;

            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (!shouldApplyModifiers) yield return 0.0f;

            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageModifier();
            }
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Mover>().Cancel();
            target = null;
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            if (currentWeaponGameObject != null) Destroy(currentWeaponGameObject);
            currentWeapon = newWeapon;
            currentWeaponGameObject = currentWeapon.Equip(rightHandPosition, leftHandPosition, GetComponent<Animator>());
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string savedWeaponName = state as string;
            Weapon weapon = Resources.Load<Weapon>(savedWeaponName);
            EquipWeapon(weapon);
        }

        //Animation event
        public void Hit()
        {
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (target == null) return;
            currentWeapon.Attack(gameObject, leftHandPosition, rightHandPosition, target, damage);
        }

        public void Shoot()
        {
            Hit();
        }
    }
}