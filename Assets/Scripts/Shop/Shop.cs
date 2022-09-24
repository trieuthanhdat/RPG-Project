using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Control;
using TMPro;
using UnityEngine;

namespace RPG.Shop
{
    public class Shop : MonoBehaviour, IRaycastable
    {

        [SerializeField] string merchantName;
        public class ShopItem
        {
            InventoryItem item;
            int availability;
            float price;
            int quantity;
            
        }
    
        public string GetShopName()
        {
            return merchantName;
        }

       public event Action onChange;
       public IEnumerable<ShopItem> GetFilteredItems() {return null;}
       public void SelectFilter(ItemCategory category){}
       public ItemCategory GetFilter() {return ItemCategory.none;}

       public CursorType GetCursorType()
       {
            return CursorType.purchase;
       }

       public bool HandleRaycast(PlayerController callingController)
       {
            if(Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<Shopper>().SetActiveShop(this);
            }

            return true;
       }

    

    }

}
