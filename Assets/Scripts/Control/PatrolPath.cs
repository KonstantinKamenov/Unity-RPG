using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float WAYPOINT_GIZMO_RADIUS = 0.25f;

        [SerializeField] private Transform[] path;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.grey;
            for (int i = 0; i < path.Length; i++)
            {
                int nxt = NextWaypoint(i);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(NextWaypoint(i)));
                Gizmos.DrawSphere(path[i].transform.position, WAYPOINT_GIZMO_RADIUS);
            }
        }

        public int NextWaypoint(int i)
        {
            return (i + 1) % path.Length;
        }

        public Vector3 GetWaypointPosition(int i)
        {
            return path[i].position;
        }
    }
}
