using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shop
{
    public class Shopper : MonoBehaviour
    {
        Shop currentShop = null;
        public event Action activeShopChange;
        public void SetActiveShop(Shop shop)
        {
            currentShop = shop;
            if(currentShop != null)
            {
                activeShopChange();
            }
        }

        public Shop GetActiveShop()
        {
            return currentShop;
        }
    }

}
