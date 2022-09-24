using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;


namespace RPG.Combat
{
    public class Skill : MonoBehaviour
    {
        [SerializeField] GameObject skillPrefab;
        private string skillName;
        bool isWaiting = false;
        float coolDownTime = 0;
        //=====Parent virtual method=====//
        public virtual float GetWaitTimeFromSkill()
        {
            return coolDownTime;
        }
        public virtual bool CanUseSkillWithCurrentMana(Mana mana)
        {
            return false;
        }

        public virtual void UseSkill(GameObject skillPrefab, Mana mana)
        {
            print("Using Skill");
        }

        //=====Set and Get Skill's Variable=====//
        public void SetCoolDownStatus(bool status)
        {
            isWaiting = status;
        }
        
        public bool GetCoolDownStatus()
        {
            return isWaiting;
        }
       
        public string GetSkillName()
        {
            skillName = gameObject.name;
            return skillName;
        }
        
       
        //=====Check SKill type and Initialize skill=====//
        public void InitializeSkill(Image fillImage, Mana mana)
        {
            Skill skill = null;
            if(GetSkillName().Equals("Lightning"))
            {
                if(isWaiting == false)
                {
                    skill = skillPrefab.GetComponent<Lightning>();
                }
            }
            else if(GetSkillName().Equals("Meteor"))
            {
                if(isWaiting == false)
                {
                    skill = skillPrefab.GetComponent<Meteor>();
                }
            }
            else if(GetSkillName().Equals("Buff"))
            {
                if(isWaiting == false)
                {
                    skill = skillPrefab.GetComponent<Buff>();
                }
            }

            if(skill != null)
            {
                skill.UseSkill(skillPrefab, mana);
                coolDownTime = skill.GetWaitTimeFromSkill();
            }

        }
    }

}
