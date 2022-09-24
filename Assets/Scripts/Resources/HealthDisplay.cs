using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] Image healthBar = null;   
        Health health = null;
        void Start()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }
        void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoint(), health.GetMaxHealthPoint());
            if(healthBar != null)
            {
                healthBar.transform.localScale = new Vector2(health.CalculateFraction(), healthBar.transform.localScale.y);
            }
        }
    }
    
}
