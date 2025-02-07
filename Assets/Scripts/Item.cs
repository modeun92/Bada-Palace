using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { FISH, STARFISH, SHELL, INVISIBLE_POTION, SPEED_POTION, KEY };

[Serializable]
public class Item: ICloneable
{
    public ItemType Type;
    public int Price;
    public int Amount;
    public Sprite Image;

    object ICloneable.Clone()
    {
        Item lItem = new Item();
        lItem.Type = Type;
        lItem.Price = Price;
        lItem.Amount = Amount;
        lItem.Image = Image;
        return lItem;
    }
}
