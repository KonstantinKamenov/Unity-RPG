using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using System.Linq;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CursorMapping[] cursorMappings;

        private Health health;
        private Dictionary<CursorType, CursorMapping> mappingsDict = null;

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
                mappingsDict[mapping.type] = mapping;
            }
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.Dead);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay()).OrderBy(hit => hit.distance).ToArray();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
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
                SetCursor(CursorType.Move);
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
