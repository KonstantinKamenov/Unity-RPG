using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController playerController)
        {
            if (tag == "Player" || !IsValidTarget()) return false;

            if (Input.GetMouseButton(0))
            {
                playerController.GetComponent<Fighter>().Attack(this);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Attack;
        }

        public bool IsValidTarget()
        {
            return !GetComponent<Health>().IsDead();
        }
    }
}