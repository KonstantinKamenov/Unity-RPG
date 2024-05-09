using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        private Mover mover;

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandPosition = null;
        [SerializeField] private Transform leftHandPosition = null;
        [SerializeField] private WeaponConfig defaultWeapon = null;
        [SerializeField] private bool shouldApplyModifiers = false;

        private WeaponConfig currentWeaponConfig;
        private LazyValue<Weapon> currentWeapon;

        private Health target;
        private float timeSinceLastAttack = 0f;

        private Weapon InitializeCurrentWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Awake()
        {
            mover = GetComponent<Mover>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(InitializeCurrentWeapon);
        }

        private void Start()
        {
            currentWeapon.Evaluate();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (Vector3.Distance(transform.position, target.transform.position) > currentWeaponConfig.GetRange())
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
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (!shouldApplyModifiers) yield return 0.0f;

            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetPercentageModifier();
            }
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Mover>().Cancel();
            target = null;
        }

        public void EquipWeapon(WeaponConfig newWeapon)
        {
            if (currentWeapon != null) Destroy(currentWeapon.val);
            currentWeaponConfig = newWeapon;
            AttachWeapon(currentWeaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            currentWeapon.val = weapon.Equip(rightHandPosition, leftHandPosition, GetComponent<Animator>());
            return currentWeapon.val;
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string savedWeaponName = state as string;
            WeaponConfig weapon = Resources.Load<WeaponConfig>(savedWeaponName);
            EquipWeapon(weapon);
        }

        //Animation event
        public void Hit()
        {
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (target == null) return;

            if (currentWeapon.val != null)
            {
                currentWeapon.val.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(gameObject, leftHandPosition, rightHandPosition, target, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        public void Shoot()
        {
            Hit();
        }
    }
}