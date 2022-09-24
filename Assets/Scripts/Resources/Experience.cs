using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;
using RPG.Stats;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour, ISaveable
    {
       [SerializeField] float xpPoint = 0;
        public event Action onGainLevel;

        public object CaptureState()
        {
            return xpPoint;
        }

        public void RestoreState(object state)
        {
            xpPoint = (float)state;
        }
        public float GetXPRequiredToLevelUp()
        {
            return GetComponent<BaseStats>().GetStats(Stats.Stats.xpRequiredToLevelUp);
        }
        public float CalculateFraction()
        {
            float xp = GetXPRequiredToLevelUp();
            return xpPoint/xp;
        }
        public void GainExp(float point)
       {
           xpPoint += point;
           onGainLevel();
       }
       public float SetXPoint
       {
            set{xpPoint = value;}
       }
        
        public float GetXPPoint()
        {
            return xpPoint;
        }
    }
    
}
