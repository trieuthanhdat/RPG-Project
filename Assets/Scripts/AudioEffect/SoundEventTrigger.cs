using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Control;

namespace RPG.Audio
{
    public class SoundEventTrigger : MonoBehaviour
    {
        public UnityEvent OnEventStart;
        

        private void OnTriggerEnter(Collider other)
        {
            
            if(other.CompareTag("Player"))
            {
                print("Triggering Sound");
                OnEventStart.Invoke();
                gameObject.GetComponent<SphereCollider>().enabled = false;
            }
        }
  
        
    }
    
}
