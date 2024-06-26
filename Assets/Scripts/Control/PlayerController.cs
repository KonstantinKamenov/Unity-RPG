using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CursorMapping[] cursorMappings;
        [SerializeField] private float maxNavMeshDistance = 0.5f;
        [SerializeField] private float raycastRadius = 1.0f;

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
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius).OrderBy(hit => hit.distance).ToArray();
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
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1.0f);
                }
                SetCursor(CursorType.Move);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            RaycastHit hit;
            target = new Vector3();
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit, Mathf.Infinity, LayerMask.GetMask("Ground"));
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasHitNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshDistance, NavMesh.AllAreas);
            if (!hasHitNavMesh) return false;

            target = navMeshHit.position;

            return true;
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
