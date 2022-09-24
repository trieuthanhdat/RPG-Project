using UnityEngine;
using TMPro;
using System;

namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void SetPoint(float damage)
        {
            text.text = String.Format("{0:0}", damage);
        }
    }
}