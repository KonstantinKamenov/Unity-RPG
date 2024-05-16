using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Control;
using RPG.Attributes;
using RPG.Utils;

namespace RPG.Combat
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionDuration = 2.0f;
        [SerializeField] private float dwellDuration = 1.0f;
        [SerializeField] private PatrolPath path;
        [SerializeField] private float waypointTolerance = 0.5f;
        [SerializeField] private float patrolSpeedFraction = 0.5f;
        [SerializeField] private float aggressionDuration = 2.0f;

        private LazyValue<Vector3> guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int currentWaypoint = 0;
        private float timeSinceAggrevated = Mathf.Infinity;

        private Fighter fighter;
        private Mover mover;
        private GameObject player;
        private Health health;

        private Vector3 GetInitialGuardPosition()
        {
            return transform.position;
        }

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            guardPosition = new LazyValue<Vector3>(GetInitialGuardPosition);
        }

        private void Start()
        {
            guardPosition.Evaluate();
        }

        private void Update()
        {
            if (health.IsDead()) return;

            timeSinceLastSawPlayer += Time.deltaTime;

            if (IsAggrevated() && GetComponent<Fighter>().CanAttack(player.GetComponent<CombatTarget>()))
            {
                timeSinceLastSawPlayer = 0;
                fighter.Attack(player.GetComponent<CombatTarget>());
            }
            else if (timeSinceLastSawPlayer <= suspicionDuration)
            {
                GetComponent<ActionScheduler>().CancelAction();
            }
            else
            {
                if (path == null)
                {
                    mover.StartMoveAction(guardPosition.val, 1.0f);
                    return;
                }

                timeSinceArrivedAtWaypoint += Time.deltaTime;
                if (IsAtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0.0f;
                    currentWaypoint = path.NextWaypoint(currentWaypoint);
                }
                if (timeSinceArrivedAtWaypoint >= dwellDuration)
                {
                    mover.StartMoveAction(path.GetWaypointPosition(currentWaypoint), patrolSpeedFraction);
                }
            }
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0.0f;
        }

        private bool IsAggrevated()
        {
            Vector3 playerPostition = player.transform.position;
            float distanceToPlayer = Vector3.Distance(transform.position, playerPostition);
            return distanceToPlayer <= chaseDistance || timeSinceAggrevated < aggressionDuration;
        }

        private bool IsAtWaypoint()
        {
            return Vector3.Distance(transform.position, path.GetWaypointPosition(currentWaypoint)) <= waypointTolerance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}