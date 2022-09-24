using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Control;
using RPG.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace  RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] 
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject lifeUpEffect;
        [SerializeField] bool canUseModifier = false;
        public event Action onLevelUp;
        public UnityEvent OnLevelUp;
        Transform player;
        LazyValue<int> currentLevel;

        Experience experience;
        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            currentLevel.ForceInit();
            
        }
        private void OnEnable()
        {
            if(experience != null)
                experience.onGainLevel += UpdateLevel;
        }
        private void OnDisable()
        {
            if(experience != null)
                experience.onGainLevel -= UpdateLevel;
        }

        private void UpdateLevel()
        {
            int nextLevel = CalculateLevel();
            if(nextLevel > currentLevel.value)
            {
                currentLevel.value  = nextLevel;
             
                experience.SetXPoint = 0;
                
                levelUpEffect();
                onLevelUp();
                OnLevelUp.Invoke();
            }
        }
        private void levelUpEffect()
        {
            Instantiate<GameObject>(lifeUpEffect, player);
            lifeUpEffect.GetComponentInChildren<Animator>().Play("LevelupTextAnimation");
        }
        public float GetStats(Stats stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat) )* (1 + GetPercentageModifier(stat)/100);
        }

        private float GetBaseStat(Stats stat)
        {
           return progression.GetStats(stat, characterClass, GetLevels());
        }

        private float GetPercentageModifier(Stats stat)
        {
            if(canUseModifier == false) return 0;
            float sum = 0;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetPercentageModifier(stat))
                {
                    sum += modifier;
                }
            }
            return sum;
        }

        private int GetAdditiveModifier(Stats stat)
        {
            if(canUseModifier == false) return 0;
            int sum = 0;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(int modifier in provider.GetAdditiveModifier(stat))
                {
                    sum += modifier;
                }
            }
            return sum;
        }

        public float GetXPReward(Stats stat)
        {
            // print(gameObject.name+ " you gain "+ GetStats(stat));
            return GetStats(stat);
        }
        public int GetLevels()
        {
            return currentLevel.value;
        }
        public int CalculateLevel()
        {
            if(experience == null) return startingLevel;

            float currentXP = experience.GetXPPoint();
            int advanceLevels = progression.GetLevels(characterClass, Stats.xpRequiredToLevelUp);
            for(int level = 1; level <= advanceLevels; level ++)
            {
                float xpToNextLevel = progression.GetStats(Stats.xpRequiredToLevelUp, characterClass, level);
                if(xpToNextLevel >= currentXP)
                {
                    return level;
                }
                
            }
            
            return advanceLevels + 1;
        }
    }
}

