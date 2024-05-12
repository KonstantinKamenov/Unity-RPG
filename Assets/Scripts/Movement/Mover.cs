using RPG.Combat;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float maxSpeed = 6.0f;
        [SerializeField] private float maxPathLength = 40.0f;

        private NavMeshAgent navMeshAgent;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = speedFraction * maxSpeed;
            navMeshAgent.isStopped = false;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            GetComponent<Fighter>().Cancel();
            MoveTo(destination, speedFraction);
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath || path.status != NavMeshPathStatus.PathComplete || GetPathLength(path) > maxPathLength) return false;
            return true;
        }
        
        private float GetPathLength(NavMeshPath path)
        {
            float distance = 0.0f;
            Vector3[] points = path.corners;
            for (int i = 0; i < points.Length - 1; i++)
            {
                distance += Vector3.Distance(points[i], points[i + 1]);
            }
            return distance;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float forwardSpeed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", forwardSpeed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = state as SerializableVector3;
            GetComponent<NavMeshAgent>().Warp(position.ToVector3());
        }
    }
}
