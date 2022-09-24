using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using UnityEngine.UI;
using RPG.Resources;

namespace RPG.Core
{
    public class SkillManager : MonoBehaviour
    {
        [SerializeField] Skill[] skills;
        [SerializeField] PowerManager powerManager;
        Mana mana;
        Animator animator;
        Power currentPower;
        private void Start()
        {
            powerManager = GameObject.FindObjectOfType<PowerManager>();
            animator = GetComponent<Animator>();
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
        }
        public void PlaySkillAnimation(int keycode)
        {
            if(CheckSkillExist(keycode) && !CheckSkillCoolDownStatus(keycode))
            {
                if(CheckManaAvailable(skills[keycode], mana))
                    animator.Play(skills[keycode].GetSkillName());
            }
        }

        public void SkillPrepare(int keycode)
        {
            if(!CheckSkillExist(keycode))
            {
                print("no such skill");
                return;
            }
            currentPower = powerManager.power[keycode]; 
            if(!CheckSkillCoolDownStatus( keycode) && currentPower != null)
            {
                currentPower.GetSkills().InitializeSkill(currentPower.GetFillImage(), mana);
                currentPower.SetCoolDown();
            }
        }
       
        private bool CheckManaAvailable(Skill currentSkill, Mana mana)
        {
            return currentSkill.CanUseSkillWithCurrentMana(mana);
        }
        private bool CheckSkillExist(int keycode)
        {
            return skills[keycode] != null;
        }
        private bool CheckSkillCoolDownStatus(int keycode)
        {
            return skills[keycode].GetCoolDownStatus() == true;
        }

        public bool CheckSkillIsInAnimationState(int keycode)
        {
            //Get power via keycode
            currentPower = powerManager.power[keycode]; 
            if(currentPower == null) return false;
            //debug//
            // print("skill name "+currentPower.GetSkills().GetSkillName());
            // print("animation state status "+animator.GetCurrentAnimatorStateInfo(0).IsName("Lightning"));

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            // int skillStateHash = Animator.StringToHash("Base Layer."+currentPower.GetSkills().GetSkillName());
            // print("state hash skill: " +  skillStateHash);
            // print(stateInfo.nameHash);

            //Start checking
            if(stateInfo.IsName(currentPower.GetSkills().GetSkillName()) &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
               return true;
            }

            return false;
        }   
        
    }

}
