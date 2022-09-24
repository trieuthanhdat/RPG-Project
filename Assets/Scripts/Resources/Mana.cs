using RPG.Saving;
using UnityEngine;
using RPG.Stats;
using GameDevTV.Utils;
using System.Collections;
using System.Collections.Generic;
using System;

namespace RPG.Resources
{
    public class Mana : MonoBehaviour, ISaveable
    {
        [SerializeField] float mpRegenPercentage = 70;
        [SerializeField] float manaRecoverAfterTime = 10f;
        [SerializeField] float manaRecoverTime = 3f;
        LazyValue<float> manaPoints;
        List<ManaRegenerate> regenList;

        private void Awake()
        {
            manaPoints = new LazyValue<float>(GetInitialMP);   
        }
        private float GetInitialMP()
        {
            return GetComponent<BaseStats>().GetStats(Stats.Stats.mana);
        }
        void Start()
        {
            manaPoints.ForceInit();
            regenList = new List<ManaRegenerate>();
            StartCoroutine(RegenManaTimely());
        }
        private void OnEnable()
        {
            //subcribe Regen mana action
            GetComponent<BaseStats>().onLevelUp += ManaRegenAfterLeveledUp;
        }
        private void OnDisable()
        {
            //Unsubcribe mana action
            GetComponent<BaseStats>().onLevelUp -= ManaRegenAfterLeveledUp;
        }
        
        //Regen buff effect
        private void Update()
        {
            foreach(var mpReg in regenList.ToArray())
            {
                if(GetManaPoint() < GetMaxManaPoint() && mpReg.fDuration > 0)
                {
                    manaPoints.value += mpReg.fHealthPerSecond * Time.deltaTime;
                }
                mpReg.fDuration -= 1f * Time.deltaTime;
                if(mpReg.fDuration < 0)
                {
                    regenList.RemoveAt(regenList.IndexOf(mpReg));
                }
            }
            if(manaPoints.value < GetMaxManaPoint())
                StartCoroutine(RegenManaTimely());
            else 
                StopCoroutine(RegenManaTimely());
                
        }
        public void AddManaRegen(ManaRegenerate mp)
        {
            regenList.Add(mp);
        }
        private IEnumerator RegenManaTimely()
        {
            yield return new WaitForSeconds(manaRecoverTime);
            while(manaPoints.value < GetMaxManaPoint())
            {
            print("recovering mana");
                float currentMana = manaPoints.value;
                currentMana += manaRecoverAfterTime * Time.deltaTime / 500;

                if(currentMana >= GetMaxManaPoint())
                    currentMana = GetMaxManaPoint();

                manaPoints.value = currentMana;
                yield return new WaitForSeconds(2f);
            }
        }

        private bool CompareCurrentMana()
        {
            return manaPoints.value < GetMaxManaPoint();
        }

        public void ConsumeMana(float amount)
        {
            // debug
            print(gameObject.name +" consume mana "+ amount); 

            if(GetManaPoint() == 0 || GetManaPoint() < amount)
               return;
            manaPoints.value = Mathf.Max(manaPoints.value - amount, 0);
            
        }
        //======MP Calculation======
        public float GetManaPoint()
        {
            return manaPoints.value;
        }
        public float GetMaxManaPoint()
        {
            return GetComponent<BaseStats>().GetStats(Stats.Stats.mana);
        }
        public float CalculateFraction()
        {
            return GetManaPoint() / GetComponent<BaseStats>().GetStats(Stats.Stats.mana);
        }
        public float GetManaRate()
        {
            return  CalculateFraction() * 100;
        }
        private void ManaRegenAfterLeveledUp()
        {
            //Regenerate by an amount of mana
            float manaRegen = GetComponent<BaseStats>().GetStats(Stats.Stats.mana) * (mpRegenPercentage/100);
            manaPoints.value += manaRegen;
            if(GetManaRate() > 100)
            manaPoints.value = Mathf.Max(GetMaxManaPoint(), manaRegen);

            manaPoints.value = Mathf.Max(manaPoints.value, manaRegen);
        }
        internal void RestoreMana(float value)
        {
            manaPoints.value = Mathf.Min(manaPoints.value + value, GetManaPoint());
        }
        //======Saving stuff======
        public object CaptureState()
        {
            return manaPoints.value;
        }

        public void RestoreState(object state)
        {
            manaPoints.value = (float) state;
        }
    }
    public class ManaRegenerate
    {
        public float fDuration;
        public float fHealthPerSecond;

        public ManaRegenerate()
        {
            
        }
        public ManaRegenerate(float duration, float healthPoint)
        {
            fDuration = duration;
            fHealthPerSecond = healthPoint;
        }
    }
}