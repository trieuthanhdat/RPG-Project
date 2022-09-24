using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 10f;
        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HidePickupForATime(respawnTime));
            }

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

    }
}