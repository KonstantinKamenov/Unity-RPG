using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Create New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private float range = 2.0f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private bool isLeftHanded = false;
        [SerializeField] private Projectile projectile = null;

        public GameObject Equip(Transform rightHand, Transform leftHand, Animator animator)
        {
            GameObject equippedWeapon = null;
            if (weaponPrefab != null) equippedWeapon = Instantiate(weaponPrefab, GetHandPosition(rightHand, leftHand));
            if (animatorOverride != null) animator.runtimeAnimatorController = animatorOverride;
            return equippedWeapon;
        }

        private void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile newProjectile = Instantiate(projectile, GetHandPosition(rightHand, leftHand).position, Quaternion.identity) as Projectile;
            newProjectile.SetTarget(target, damage);
        }

        private Transform GetHandPosition(Transform rightHand, Transform leftHand)
        {
            Transform handPosition = rightHand;
            if (isLeftHanded) handPosition = leftHand;

            return handPosition;
        }

        public void Attack(Transform rightHand, Transform leftHand, Health target)
        {
            if (projectile == null)
            {
                target.TakeDamage(damage);
            }
            else
            {
                LaunchProjectile(leftHand, rightHand, target);
            }
        }

        public float GetRange()
        {
            return range;
        }
    }
}