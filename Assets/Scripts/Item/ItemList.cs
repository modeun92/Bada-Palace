using Assets.Scripts.Item;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ItemList : MonoBehaviour
{
    [SerializeField] private Item[] Items;

    private static ItemList instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static ItemList Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public static Item GetItem(ItemType aItemType)
    { 
        Item lItem = null;
        if (instance != null)
        {
            foreach (Item eItem in instance.Items)
            {
                if (eItem.Type == aItemType)
                {

                    lItem = (Item)((ICloneable)eItem).Clone();
                    break;
                }
            }
        }
        return lItem;
    }
    public static Item GetItem(ItemType aItemType, int aAmount)
    {
        Item lItem = GetItem(aItemType);
        if (lItem != null)
        {
            //lItem.Amount = aAmount;
        }
        return lItem;
    }
}
