using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Item
{
    //This manages money and inventory(buying/selling)
    //Does it have to get InventoryCorrespondent as a member? I don't know
    //We need to check where I need to use this
    public class ShopCorrespondent
    {
        private Stack<ItemToOwn> m_CartToBuy;
        private Stack<ItemToOwn> m_CartToSell;
        //like this those following functions don't matter
        public void Sell(ItemType a_ItemType)
        {
            //check if the inventory has the item
        }
        public void Sell(ItemToOwn a_ItemType)
        {
            //check if the inventory has the item
        }
        public void Sell(params ItemToOwn[] a_Items)
        {
            //check if the inventory has the item
        }
        public void SellAll(ItemType a_ItemType)
        {
            //check if the inventory has the item
        }
        public void Buy(ItemType a_ItemType)
        {
            //check if the money is enough to buy it
        }
        public void Buy(ItemToOwn a_Item)
        {
            //check if the money is enough to buy it
        }
        public void Buy(params ItemToOwn[] a_Items)
        {
            //check if the money is enough to buy it
        }
        public void BuyAll(ItemType a_ItemType)
        {
            //check how many he can buy
        }
    }
}
