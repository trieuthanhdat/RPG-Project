using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        // [SerializeField] float chaseDistance = 5f;

        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float arrgevateCoolDownTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float sphereCastRadius = 4f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        LazyValue<Vector3> guardPosition;
        FieldOfView targetLock;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeGetArrgevated = Mathf.Infinity;
        int currentWaypointIndex = 0;
        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindGameObjectWithTag("Player").gameObject;
            targetLock = GetComponent<FieldOfView>();

            guardPosition = new LazyValue<Vector3>(InitGuardPosition);
            guardPosition.ForceInit();
        }
        private Vector3 InitGuardPosition()
        {
            return transform.position;
        }
        

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeGetArrgevated += Time.deltaTime;
        }
        //=====AI Patrol Behaviour=====
        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }
        //=====AI Suspicion Behaviour=====
        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        //=====AI attack behaviour=====
        public void Reset()
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            targetLock.canSeeTarget = false;
            agent.Warp(guardPosition.value);
            timeSinceLastSawPlayer = Mathf.Infinity;
            timeSinceArrivedAtWaypoint = Mathf.Infinity;
            timeGetArrgevated = Mathf.Infinity;
            currentWaypointIndex = 0;
        }
        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
            ArrgevateSphereEffect();
        }
        public void ArrgevateSphereEffect()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereCastRadius, Vector3.up, 0); 
            foreach(RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if(ai == null) continue;
                ai.Arrgevate();
            }
        }
        public void Arrgevate()
        {
            timeGetArrgevated = 0;
        }

        private bool InAttackRangeOfPlayer()
        {
            // float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            // return distanceToPlayer < chaseDistance;
            return targetLock.GetCanSeeTarget || timeGetArrgevated < arrgevateCoolDownTime;
        }

        // Called by Unity
        // private void OnDrawGizmosSelected() {
        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawWireSphere(transform.position, chaseDistance);
        // }
    }
}