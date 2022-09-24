using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FieldOfView : MonoBehaviour
    {
        public float radius;
        [Range(0, 360)]
        public float angle;

        public GameObject playerRef;
        public bool canSeeTarget;
        [SerializeField] LayerMask targetMask;
        [SerializeField] LayerMask obstructionMask;
        Collider[] rangeTargets;

        void Start()
        {
            playerRef = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(FOVRoutine());
        }

        private IEnumerator FOVRoutine()
        {
            WaitForSeconds time = new WaitForSeconds(0.1f);
            while(true)
            {
                yield return  time;
                FieldOfVisionCheck();
            }
        }
        private void FieldOfVisionCheck()
        {
            rangeTargets = Physics.OverlapSphere(transform.position, radius, targetMask);
            if(rangeTargets.Length != 0)
            {
                Transform target = rangeTargets[0].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if(Vector3.Angle(transform.forward, directionToTarget) < angle/2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        canSeeTarget = true;
                    else
                        canSeeTarget = false;
                }else
                    canSeeTarget = false;
            }else if(canSeeTarget)
                canSeeTarget = false;
        }
        public void ReconfigFOVRadius(float newRadius)
        {
            radius = newRadius;
        }
        public bool GetCanSeeTarget
        {
            get{return canSeeTarget;}
        }
        public Collider[] GetRangeTargets
        {
            get{return rangeTargets;}
        }
        // void OnDrawGizmos()
        // {
        //     Physics.OverlapSphere(transform.position, radius, targetMask);
        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawWireSphere(transform.position, radius);
        // }
    }

}
