using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfiguration weapon = null;
        [SerializeField] float respawnTime = 10f;
        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.tag == "Player")
            {
               PickUp(other.GetComponent<Fighter>());
            }

        }

        public void PickUp(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HidePickupForATime(respawnTime));
        }

        private IEnumerator HidePickupForATime(float time)
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(time);
            ShowPickUp(true);
        }
        private void ShowPickUp(bool canShow)
        {
            GetComponent<Collider>().enabled = canShow;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(canShow);
            }
        }

        public bool HandleRaycast(PlayerController controller)
        {
            // if(Input.GetMouseButtonDown(1))
            // {
            //     PickUp(controller.GetComponent<Fighter>());
            // }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.loot;
        }
    }
}