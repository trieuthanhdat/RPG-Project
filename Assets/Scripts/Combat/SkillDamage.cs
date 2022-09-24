using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    public class SkillDamage : MonoBehaviour
    {
        [SerializeField] float damageRadius = 2f;
        [SerializeField] LayerMask enemyLayer;
        float damageAmount = 0;
        Health enemyHealth = null;
        public void SetDamageAmount(float value)
        {
            damageAmount = value;
        }
        bool collided = false;

        // Update is called once per frame
        void Update()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius,enemyLayer);
            foreach(Collider collider in colliders)
            {
                enemyHealth = collider.gameObject.GetComponent<Health>();
                collided = true;
            }
            if(collided)
            {     
                enemyHealth.TakeDamage(GameObject.FindGameObjectWithTag("Player").gameObject, damageAmount);
                enabled = false;
            }
            
        }
    }

}
