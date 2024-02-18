using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {

            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            CombatTarget finalTarget = null;
            float minDist = Mathf.Infinity;

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null || !target.IsValidTarget()) continue;

                if (hit.distance < minDist)
                {
                    minDist = hit.distance;
                    finalTarget = target;
                }
            }
            if (finalTarget == null) return false;

            if (Input.GetMouseButton(0))
            {
                GetComponent<Fighter>().Attack(finalTarget);
            }
            return true;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
