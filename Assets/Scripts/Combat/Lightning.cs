using RPG.Core;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    public class Lightning : Skill
    {
        [SerializeField] float manaConsume = 0;
        [SerializeField] float waitTime = 0;
        [SerializeField] float damage = 25;
        [SerializeField] Sprite imageSource;
        [SerializeField] GameObject damagePoint;
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
        public override void UseSkill(GameObject LightningPrefab, Mana mana)
        {
            FieldOfView fov  = GetPlayer().GetComponent<FieldOfView>();

            if(fov.GetRangeTargets.Length <= 0 || fov == null) return;

                foreach(Collider target in fov.GetRangeTargets)
                {
                    Vector3 pos = new Vector3(target.transform.position.x, 
                                            target.transform.position.y, 
                                            target.transform.position.z);
                    Instantiate(LightningPrefab ,pos ,Quaternion.identity);
                }
            mana.ConsumeMana(manaConsume);
        }

        //=====Awake=====//
        private void Awake()
        {
            SetSkillDamage();
        }
       
     
        //=====Initialise damage=====//
        public void SetSkillDamage()
        {
            damagePoint.GetComponent<SkillDamage>().SetDamageAmount(damage);
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
