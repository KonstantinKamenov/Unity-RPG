using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        private Mover mover;

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandPosition = null;
        [SerializeField] private Transform leftHandPosition = null;
        [SerializeField] private Weapon defaultWeapon = null;

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

        public void EquipWeapon(Weapon newWeapon)
        {
            if (currentWeaponGameObject != null) Destroy(currentWeaponGameObject);
            currentWeapon = newWeapon;
            currentWeaponGameObject = currentWeapon.Equip(rightHandPosition, leftHandPosition, GetComponent<Animator>());
        }

        //Animation event
        public void Hit()
        {
            if (target == null) return;
            currentWeapon.Attack(leftHandPosition, rightHandPosition, target);
        }

        public void Shoot()
        {
            Hit();
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
    }
}