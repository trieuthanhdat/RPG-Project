using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class Power : MonoBehaviour
    {
        [SerializeField] PowerTypes powerTypes;
        [SerializeField] Image fillImage;   
        [SerializeField] Skill skill;
        [SerializeField] KeyCode keyBound;
        SkillManager skillManager; 
        public Image GetFillImage()
        {
            return fillImage;
        }
        public Skill GetSkills()
        {
             return skill;
        }
        private void Start()
        {
            skillManager = GameObject.FindGameObjectWithTag("Player").GetComponent<SkillManager>();
        }
       
        private void Update()
        {
            if(skill != null && skill.GetWaitTimeFromSkill() != 0)
            {
                if(skill.GetCoolDownStatus() == true)
                {
                   CoolDown(skill.GetWaitTimeFromSkill());
                }
            }
        }
        public void SetCoolDown()
        {
            if(fillImage == null || skill ==null) return;

            if(!fillImage.gameObject.activeInHierarchy)
            {
                fillImage.gameObject.SetActive(true);
                fillImage.fillAmount = 1;
            }
            skill.SetCoolDownStatus(true);
            

        }
        public void CoolDown(float coolDownTime)
        {
            fillImage.fillAmount -= coolDownTime * Time.deltaTime;
            if(fillImage.fillAmount <= 0)
            {
                fillImage.gameObject.SetActive(false);
                skill.SetCoolDownStatus(false);
            }
        }


    }

}
