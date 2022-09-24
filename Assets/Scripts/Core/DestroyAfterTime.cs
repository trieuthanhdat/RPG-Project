using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] float timeToDestroy = 0;
        private void Update()
        {
            Destroy(gameObject, timeToDestroy);
        }

        public void SetTimeToDestroy(float time)
        {
            timeToDestroy = time;
        }
    }

}
