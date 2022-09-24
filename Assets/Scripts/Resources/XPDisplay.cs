using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class XPDisplay : MonoBehaviour
    {
        [SerializeField] Image EXPBar;
        Experience xp = null;
        void Start()
        {
            xp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }
        void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}/{1:0}", xp.GetXPPoint(), xp.GetXPRequiredToLevelUp());
            
            if(xp != null)
                EXPBar.transform.localScale = new Vector2(xp.CalculateFraction(), EXPBar.transform.localScale.y);
        }
    }
    
}
