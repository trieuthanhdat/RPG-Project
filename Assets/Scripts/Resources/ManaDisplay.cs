using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Resources
{
    public class ManaDisplay : MonoBehaviour
    {
        [SerializeField] Image manaBar = null;   
        Mana mana = null;
        void Start()
        {
            mana = GameObject.FindWithTag("Player").GetComponent<Mana>();
        }
        void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}/{1:0}", mana.GetManaPoint(), mana.GetMaxManaPoint());
            if(manaBar != null)
            {
                manaBar.transform.localScale = new Vector2(mana.CalculateFraction(), manaBar.transform.localScale.y);
            }
        }
    }
    
}
