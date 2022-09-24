using System;
using UnityEngine;
using TMPro;
using RPG.Stats;

namespace RPG.Resources
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats = null;

        void Start()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }
        void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}", baseStats.GetLevels());
        }
    }
    
}
