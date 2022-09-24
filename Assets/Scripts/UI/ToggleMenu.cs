using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class ToggleMenu : MonoBehaviour
    {
        [SerializeField] GameObject entryUI;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            Toggle(entryUI);
        }
        public void Toggle(GameObject toToggle)
        {
            //Check root menus
            if(toToggle.transform.parent != transform) return;
            foreach(Transform child in transform)
            {
                if(child.gameObject == toToggle)
                    child.gameObject.SetActive(!child.gameObject.activeInHierarchy);
            }
        }
    }

}
