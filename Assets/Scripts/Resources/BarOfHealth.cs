using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources
{
    public class BarOfHealth : MonoBehaviour
    {
        [SerializeField] Health health = null ;
        [SerializeField] RectTransform foreGround = null;
        [SerializeField] Canvas barOfHealthCanvas = null;
       /// <summary>
       /// Update is called every frame, if the MonoBehaviour is enabled.
       /// </summary>
       private void Update()
       {
           if(Mathf.Approximately(health.CalculateFraction(), 1)|| Mathf.Approximately(health.CalculateFraction(), 0))
           {
               barOfHealthCanvas.enabled = false;
               return;
           }
           
           barOfHealthCanvas.enabled = true;
           foreGround.localScale = new Vector3(health.CalculateFraction(), foreGround.localScale.y);
       }

    }

}
