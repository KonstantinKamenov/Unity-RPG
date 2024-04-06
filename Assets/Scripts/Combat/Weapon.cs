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

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            Transform handPosition = rightHand;
            if (isLeftHanded) handPosition = leftHand;
            if (weaponPrefab != null) Instantiate(weaponPrefab, handPosition);
            if (animatorOverride != null) animator.runtimeAnimatorController = animatorOverride;
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetRange()
        {
            return range;
        }
    }
}