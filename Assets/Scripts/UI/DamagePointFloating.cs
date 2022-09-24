using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class DamagePointFloating : MonoBehaviour
    {
        [SerializeField] DamageText damageText = null;
      
        public void SpawnDamgeFloatingPoint(float damage)
        {
            DamageText damagePoint = Instantiate<DamageText>(damageText, transform);
            damagePoint.SetPoint(damage);
        }
    }
    
}
