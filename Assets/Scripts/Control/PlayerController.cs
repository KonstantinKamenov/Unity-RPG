using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System.Collections.Generic;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CursorMapping[] cursorMappings;

        private Health health;
        private Dictionary<CursorType, CursorMapping> mappingsDict = null;

        private enum CursorType
        {
            None,
            Move,
            Attack
        }

        [System.Serializable]
        private struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        private void Awake()
        {
            health = GetComponent<Health>();
            mappingsDict = new Dictionary<CursorType, CursorMapping>();
            foreach (CursorMapping mapping in cursorMappings)
            {
                mappingsDict.Add(mapping.type, mapping);
            }
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat())
            {
                SetCursor(CursorType.Attack);
                return;
            }
            if (InteractWithMovement())
            {
                SetCursor(CursorType.Move);
                return;
            }
            SetCursor(CursorType.None);
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            CombatTarget finalTarget = null;
            float minDist = Mathf.Infinity;

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (target.tag == "Player") continue;
                if (!target.IsValidTarget()) continue;

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
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit, Mathf.Infinity, LayerMask.GetMask("Ground"));
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1.0f);
                }
                return true;
            }
            return false;
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = mappingsDict[type];
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }
    }
}
