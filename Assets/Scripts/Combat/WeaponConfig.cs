using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private Weapon weaponPrefab = null;
        [SerializeField] private float range = 2.0f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float percentageModifier = 0.0f;
        [SerializeField] private bool isLeftHanded = false;
        [SerializeField] private Projectile projectile = null;

        public Weapon Equip(Transform rightHand, Transform leftHand, Animator animator)
        {
            Weapon equippedWeapon = null;
            if (weaponPrefab != null) equippedWeapon = Instantiate(weaponPrefab, GetHandPosition(rightHand, leftHand));
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else
            {
                AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController != null) animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return equippedWeapon;
        }

        public void LaunchProjectile(GameObject attacker, Transform rightHand, Transform leftHand, Health target, float fighterDamage)
        {
            Projectile newProjectile = Instantiate(projectile, GetHandPosition(rightHand, leftHand).position, Quaternion.identity);
            newProjectile.SetTarget(attacker, target, fighterDamage);
        }

        private Transform GetHandPosition(Transform rightHand, Transform leftHand)
        {
            Transform handPosition = rightHand;
            if (isLeftHanded) handPosition = leftHand;

            return handPosition;
        }

        public float GetRange()
        {
            return range;
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetPercentageModifier()
        {
            return percentageModifier;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }
    }
}