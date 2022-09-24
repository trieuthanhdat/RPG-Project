using RPG.Core;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    public class Buff : Skill
    {
        [SerializeField] float manaConsume = 0;
        [SerializeField] float waitTime = 0;
        [SerializeField] float lifeTime = 4;
        [SerializeField] float manaRecoverOverTime = 10;
        [SerializeField] float healthRecoverOverTime = 10;
        Health health;
        Mana mana;
        
        [SerializeField] Sprite imageSource;
        GameObject buffEffect = null;
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void FixedUpdate()
        {
            if(buffEffect != null)
                buffEffect.transform.parent = GetPlayer().transform;
        }
       
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

        public override void UseSkill(GameObject buffPrefab, Mana mana)
        {
            GameObject player = GetPlayer();
            if(player != null)
            {
                Vector3 pos = new Vector3(player.transform.position.x, 
                                          player.transform.position.y + 1f, 
                                          player.transform.position.z);
                buffEffect = Instantiate(buffPrefab, pos, Quaternion.identity);
                buffEffect.transform.parent = player.transform; 
                //Buff health effects
                health = player.GetComponent<Health>(); 
                HealthRegenerate hpRegen = new HealthRegenerate(lifeTime, healthRecoverOverTime);
                health.AddHPRegen(hpRegen);
                //buff mana effect
                mana = player.GetComponent<Mana>();
                ManaRegenerate mpRegen = new ManaRegenerate(lifeTime, manaRecoverOverTime);
                mana.AddManaRegen(mpRegen);

            }
            buffEffect.GetComponent<DestroyAfterTime>().SetTimeToDestroy(lifeTime);
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
