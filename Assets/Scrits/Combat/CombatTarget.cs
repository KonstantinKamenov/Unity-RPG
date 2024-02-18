using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
        public bool IsValidTarget()
        {
            return !GetComponent<Health>().IsDead();
        }
    }
}