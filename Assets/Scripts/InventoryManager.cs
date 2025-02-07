using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject ItemForm;
    public GameObject Content;
    public Text GoldAmount;

    private RectTransform mContentTransform;
    private GridLayoutGroup mContentGridLayout;
    private Vector2 mCellSize;
    private Vector2 mSpacing;
    private float mContentDefaultWidth;
    private int mFitCount;
    private float mCellUnitWidth;
    private Dictionary<ItemType, GameObject> mItemLibrary;

    private int mGoldAmount = 0;
    private int mItemCount = 0;
    private bool mIsInitialized = false;
    private object locker = new object();

    // Start is called before the first frame update
    void Start()
    {
        CheckInit();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void CheckInit()
    {
        lock (locker)
        {
            if (!mIsInitialized)
            {
                Initiate();
            }
        }
    }
    private void Initiate()
    {
        mItemLibrary = new Dictionary<ItemType, GameObject>();
        mContentTransform = Content.GetComponent<RectTransform>();
        mContentGridLayout = Content.GetComponent<GridLayoutGroup>();
        mCellSize = mContentGridLayout.cellSize;
        mSpacing = mContentGridLayout.spacing;
        mContentDefaultWidth = GetComponent<RectTransform>().rect.width;
        mContentTransform.sizeDelta = new Vector2(mContentDefaultWidth, mContentTransform.sizeDelta.y);
        mCellUnitWidth = mCellSize.x + mSpacing.x;
        mFitCount = (int)(mContentDefaultWidth / mCellUnitWidth);
        mIsInitialized = true;
        LoadInventory();
    }
    private void LoadInventory()
    {
        Item[] lItems = null;
        GameMainSettingManager.GetValue(Assets.Scripts.ConfigParameter.ITEMS, out lItems);
        GameMainSettingManager.GetValue(Assets.Scripts.ConfigParameter.GOLD_AMOUNT, out mGoldAmount);
        if (lItems != null)
        {
            foreach (var lItem in lItems)
            {
                AddItem(lItem);
            }
        }
        GoldAmount.text = mGoldAmount.ToString();
    }
    private void ChangeCount(int aChange)
    {
        lock (locker)
        {
            mItemCount += aChange;
        }
        ChangeContentSize();
    }
    private void ChangeContentSize()
    {
        CheckInit();
        if (mItemCount > mFitCount)
        {
            var lWidth = (mCellUnitWidth * mItemCount) + mSpacing.x;

            mContentTransform.sizeDelta = new Vector2(lWidth, mContentTransform.sizeDelta.y);
        }
        else
        {
            mContentTransform.sizeDelta = new Vector2(mContentDefaultWidth, mContentTransform.sizeDelta.y);
        }
    }
    public void BuyItem(Item aItem)
    {
        lock (locker)
        {
            AddItem(aItem);
            AddGold(-aItem.Price);
            GoldAmount.text = mGoldAmount.ToString();
        }
    }
    public void AddGold(int aGoldAmount)
    {
        lock (locker)
        {
            mGoldAmount += aGoldAmount;
            GoldAmount.text = mGoldAmount.ToString();
        }
    }
    public void AddItem(Item aItem)
    {
        lock (locker)
        {
            if (mItemLibrary.ContainsKey(aItem.Type))
            {
                GameObject itemCell;
                if(mItemLibrary.TryGetValue(aItem.Type, out itemCell))
                {
                    var itemCellManager = itemCell.GetComponent<ItemManager>();
                    itemCellManager.AddItem(aItem.Amount);
                }
            }
            else
            {
                var itemCell = Instantiate(ItemForm, new Vector3(), Quaternion.identity, mContentTransform);
                mItemLibrary.Add(aItem.Type, itemCell);
                var itemCellManager = itemCell.GetComponent<ItemManager>();
                itemCellManager.SetItem(aItem);
                itemCellManager.OnItemRunOut += () => { ChangeCount(-1); };
                ChangeCount(1);
            }
        }
    }
}
