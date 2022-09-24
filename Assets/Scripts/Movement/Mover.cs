using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 45f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        private void Start() {
            
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        //=====Movement=====
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }
        public bool CanMoveTo(Vector3 destination)
        {
             //calculate navmesh paths
            NavMeshPath navPath = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navPath);
            if(!hasPath) return false;
            if(navPath.status != NavMeshPathStatus.PathComplete) return false;
            //compare the distance of (from player to mouse point) to maxNavPathLength
            if(CalculatePathLength(navPath) > maxNavPathLength) return false;

            return true;
        }
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }
        public float GetMovementSpeed(float speedFraction)
        {
            return speedFraction * maxSpeed;
        }
        private float CalculatePathLength(NavMeshPath navPath)
        {
            float sum = 0;
            if(navPath.corners.Length < 2f ) return 0;
            for(int i = 0; i< navPath.corners.Length - 1; i++)
            {
                sum += Vector3.Distance(navPath.corners[i], navPath.corners[i+1]);
            }
            return sum;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        //=====Animator=====
        public void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
        //=====Save & load implementation=====
        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            navMeshAgent.enabled = false;
            transform.position = position.ToVector();
            navMeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}