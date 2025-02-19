using System;
using UnityEngine;


namespace Assets.Scripts.Item
{
    public enum ItemType { FISH, STARFISH, SHELL, INVISIBLE_POTION, SPEED_POTION, KEY };
    [Serializable]
    public class Item : ICloneable
    {
        public ItemType Type;
        object ICloneable.Clone()
        {
            Item lItem = new Item();
            lItem.Type = Type;
            return lItem;
        }
    }
    [Serializable]
    public class ItemToShow : Item, ICloneable
    {
        public Sprite Image;
        object ICloneable.Clone()
        {
            ItemToShow lItem = new ItemToShow();
            lItem.Type = Type;
            lItem.Image = Image;
            return lItem;
        }
    }
    [Serializable]
    public class ItemForSale : ItemToShow, ICloneable
    {
        public int Price;
        object ICloneable.Clone()
        {
            ItemForSale lItem = new ItemForSale();
            lItem.Type = Type;
            lItem.Price = Price;
            lItem.Image = Image;
            return lItem;
        }
    }
    [Serializable]
    public class ItemForInventory : ItemToShow, ICloneable
    {
        public int Amount;
        object ICloneable.Clone()
        {
            ItemForInventory lItem = new ItemForInventory();
            lItem.Type = Type;
            lItem.Image = Image;
            lItem.Amount = Amount;
            return lItem;
        }
    }
    [Serializable]
    public class ItemToOwn : Item, ICloneable
    {
        public int Amount;
        object ICloneable.Clone()
        {
            ItemToOwn lItem = new ItemToOwn();
            lItem.Type = Type;
            lItem.Amount = Amount;
            return lItem;
        }
    }
}