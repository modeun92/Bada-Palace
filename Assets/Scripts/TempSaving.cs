using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSaving
{
    private static TempSaving mTempSaving;
    private static object locking = new object();
    private Vector3 mermaidLocation;
    private int level;
    private int stage;
    private Item item;
    private bool isMermaidMovable;
    private bool isRightHanded;
    public static bool IsMermaidMovable 
    { 
        get
        {
            if (mTempSaving == null)
            {
                return true;
            }
            else
            {
                return mTempSaving.isMermaidMovable;
            }
        }
        set
        {
            if (mTempSaving != null)
            {
                mTempSaving.isMermaidMovable = value;
            }
        }
    }
    private TempSaving()
    {
        isMermaidMovable = true;
        mermaidLocation = new Vector3(0f, 0f, 0f);
        level = 1;
        stage = 1;
        item = null;
    }
    private static void Instantiate()
    {
        lock (locking)
        {
            if (mTempSaving == null)
            {
                mTempSaving = new TempSaving();
                Debug.Log("TempSaving.Instantiate");
            }
        }
    }
    public static bool HasItem()
    {
        Instantiate();
        return (mTempSaving != null && mTempSaving.item != null);
    }
    public static bool GetItem(out Item item)
    {
        Instantiate();
        bool isNull;
        lock (locking)
        {
            isNull = mTempSaving == null || mTempSaving.item == null;
            if (isNull)
            {
                item = null;
            }
            else
            {
                item = mTempSaving.item;
                mTempSaving.item = null;
            }
        }
        return !isNull;
    }
    public static void RewardItem(Item item)
    {
        Instantiate();
        mTempSaving.item = item;
    }
    public static void Reset()
    {
        Instantiate();
    }
    public static void ResetMermaid()
    {
        Instantiate();
        mTempSaving.mermaidLocation = new Vector3(0f, 0f, 0f);
    }
    public static void SetBackgroundInfo(int level, int stage)
    {
        Instantiate();
        mTempSaving.level = level;
        mTempSaving.stage = stage;
    }
    public static void SetMermaidLocation(Vector3 loc)
    {
        Instantiate();
        mTempSaving.mermaidLocation = loc;
    }
    public static void GetBackgroundInfo(out int level, out int stage)
    {
        if (mTempSaving != null)
        {
            level = mTempSaving.level;
            stage = mTempSaving.stage;
        }
        else
        {
            level = 1;
            stage = 1;
        }
    }
    public static Vector3 GetMermaidLocation()
    {
        if (mTempSaving != null)
        {
            return mTempSaving.mermaidLocation;
        }
        else
        {
            return new Vector3(0f, 0f, 0f);
        }
    }
}
