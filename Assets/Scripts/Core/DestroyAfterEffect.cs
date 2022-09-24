using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject toDestroy;
        void Update()
        {
            if(!GetComponent<ParticleSystem>().IsAlive())
            {
                if(toDestroy != null)
                    Destroy(toDestroy);
                else
                    Destroy(gameObject);
            }
        }
    }

}



