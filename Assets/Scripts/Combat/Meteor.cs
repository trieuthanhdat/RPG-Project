using RPG.Core;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    public class Meteor : Skill
    {
        [SerializeField] float manaConsume = 0;
        [SerializeField] float waitTime = 0;
        [SerializeField] float damage = 25;
        [SerializeField] Sprite imageSource;
        [SerializeField] GameObject targetLockEffect = null;
       
        //=====Override methods=====//
        public override float GetWaitTimeFromSkill()
        {
            return waitTime;
        }
        
        public override bool CanUseSkillWithCurrentMana(Mana mana)
        {
            if(mana.GetManaPoint() < manaConsume)
                return false;
            return true;
        }

        public override void UseSkill(GameObject meteorPrefab, Mana mana)
        {
            FieldOfView fov  = GetPlayer().GetComponent<FieldOfView>();
            if(fov.GetRangeTargets.Length <= 0 || fov == null) return;

                foreach(Collider target in fov.GetRangeTargets)
                {
                    if(target !=null) 
                    {
                        Vector3 pos = new Vector3(GetPlayer().transform.position.x, 
                                                GetPlayer().transform.position.y + 10f, 
                                                GetPlayer().transform.position.z);
                        Projectile meteor = Instantiate(meteorPrefab ,pos ,Quaternion.identity).
                                                        gameObject.GetComponent<Projectile>();
                                                        
                        //lock target effect and strike
                        GameObject meteorInstance = Instantiate(targetLockEffect, target.transform.position, Quaternion.identity);
                        meteorInstance.transform.parent = target.transform;
                        
                        meteor.SetTarget(target.GetComponent<Health>(), GetPlayer(), damage);
                    }
                }
            mana.ConsumeMana(manaConsume);
        }

         //=====Getter=====//
        public float GetManaConsume()
        {
            return manaConsume;
        }
        private GameObject GetPlayer()
        {
            return GameObject.FindGameObjectWithTag("Player").gameObject;
        }
        
    }
    
}
