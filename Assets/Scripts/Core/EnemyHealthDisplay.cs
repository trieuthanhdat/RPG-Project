using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;
using RPG.Combat;
using TMPro;

namespace RPG.Core
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter = null;

        void Start()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }
        void Update()
        {
            if(fighter.GetTarget() == null)
            {
                GetComponent<TextMeshProUGUI>().text = "N/A";
                return;
            }
            
            Health health = fighter.GetTarget();
            // GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}%", health.GetHealthRate());
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoint(), health.GetMaxHealthPoint());
        }
    }
    
}
