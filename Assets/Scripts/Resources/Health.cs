using RPG.Saving;
using UnityEngine;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float hpRegenPercentage = 70;
        [SerializeField] ReceiveDamageEvent receiveDamage;
        public UnityEvent OnDie;

        [System.Serializable]
        public class ReceiveDamageEvent: UnityEvent<float>
        {

        }
        LazyValue<float> healthPoints;

        List<HealthRegenerate> regenList;
        bool wasDeadLastFrame = false;

        //=====Basics of Monobehaviour=====//
        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHP);   
        }
        void Start()
        {
            healthPoints.ForceInit();
            regenList = new List<HealthRegenerate>();
        }
        private void OnEnable()
        {
            //subcribe Regen health action
            GetComponent<BaseStats>().onLevelUp += HealthRegen;
        }
        private void OnDisable()
        {
            //Unsubcribe health action
            GetComponent<BaseStats>().onLevelUp -= HealthRegen;
        }

        //regen buff effect
        public void Update()
        {
            foreach(var hpReg in regenList.ToArray())
            {
                if(GetHealthPoint() < GetMaxHealthPoint() && hpReg.fDuration > 0)
                {
                    healthPoints.value += hpReg.fHealthPerSecond * Time.deltaTime;
                }
                hpReg.fDuration -= 1f * Time.deltaTime;
                if(hpReg.fDuration < 0)
                {
                    regenList.RemoveAt(regenList.IndexOf(hpReg));
                }
            }
        }
        

        private float GetInitialHP()
        {
            return GetComponent<BaseStats>().GetStats(Stats.Stats.health);
        }
       
        public void TakeDamage(GameObject instigator, float damage)
        {
            // debug

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if(IsDead())
            {
                OnDie.Invoke();
                AwardExperience(instigator);
            }else
            {
                receiveDamage.Invoke(damage);
            }
            UpdateState();
        }

        

        //======HP Calculation======
        public float GetHealthPoint()
        {
            return healthPoints.value;
        }
        public float GetMaxHealthPoint()
        {
            return GetComponent<BaseStats>().GetStats(Stats.Stats.health);
        }
        public float CalculateFraction()
        {
            return GetHealthPoint() / GetComponent<BaseStats>().GetStats(Stats.Stats.health);
        }
        public float GetHealthRate()
        {
            return  CalculateFraction() * 100;
        }
        internal void Heal(float value)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + value, GetMaxHealthPoint());
            UpdateState();
        }
        private void HealthRegen()
        {
            //Regenerate by an amount of health
            float healthRegen = GetComponent<BaseStats>().GetStats(Stats.Stats.health) * (hpRegenPercentage/100);
            healthPoints.value += healthRegen;
            if(GetHealthRate() > 100)
            healthPoints.value = Mathf.Max(GetMaxHealthPoint(), healthRegen);

            healthPoints.value = Mathf.Max(healthPoints.value, healthRegen);
        }
        public void AddHPRegen(HealthRegenerate hp)
        {
            regenList.Add(hp);
        }
        //======Experience stuff======
        private void AwardExperience(GameObject instigator)
        {
            Experience exp = instigator.GetComponent<Experience>();
            if(exp == null) return;

            exp.GainExp(GetComponent<BaseStats>().GetXPReward(Stats.Stats.xpReward));
        }
        //======Die state======
        public bool IsDead()
        {
            return healthPoints.value <= 0;
        }

        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();
            if (!wasDeadLastFrame && IsDead())
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
                animator.SetTrigger("die");
                GetComponent<CapsuleCollider>().enabled = false;
            }
            else if(wasDeadLastFrame && !IsDead())
            {
                //reset the animator entirely
                animator.Rebind();
            }
            wasDeadLastFrame = IsDead();
        }
        
        //======Saving stuff======
        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float) state;
            UpdateState();
        }
    }

    public class HealthRegenerate
    {
        public float fDuration;
        public float fHealthPerSecond;

        public HealthRegenerate(){}
        public HealthRegenerate(float duration, float healthPoint)
        {
            fDuration = duration;
            fHealthPerSecond = healthPoint;
        }
    }
}