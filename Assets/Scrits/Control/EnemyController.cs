using UnityEngine;
using RPG.Core;
using RPG.Movement;

namespace RPG.Combat
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private float chaseDistance = 5f;

        private Vector3 guardPosition;

        private Fighter fighter;
        private Mover mover;
        private GameObject player;
        private Health health;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
        }

        private void Start()
        {
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            Vector3 playerPostition = player.transform.position;
            float distanceToPlayer = Vector3.Distance(transform.position, playerPostition);

            if (distanceToPlayer <= chaseDistance && player.GetComponent<CombatTarget>().IsValidTarget())
            {
                fighter.Attack(player.GetComponent<CombatTarget>());
            }
            else
            {
                mover.StartMoveAction(guardPosition);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}