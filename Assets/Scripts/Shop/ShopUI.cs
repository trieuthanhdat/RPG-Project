using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Shop
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI shopName;

        Shopper shopper = null;
        Shop currentShop = null;
        private void Start()
        {
            shopper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();

            if(shopper == null) return;

            shopper.activeShopChange += ShopChange;

            ShopChange();
            
        }

        private void ShopChange()
        {
            currentShop = shopper.GetActiveShop();
            gameObject.SetActive(currentShop != null);
            HookShopName();
        }

        public void HookShopName()
        {
            if(currentShop != null)
            shopName.text = currentShop.GetShopName();
        }
        public void CloseShop()
        {
            gameObject.SetActive(false);
            currentShop = null;
        }
    }

}
