using Assets.Scripts.Global;
using Assets.Scripts.Item;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ItemForm;
    [SerializeField]
    private GameObject Content;
    [SerializeField]
    private TextMeshProUGUI GoldAmount;
    [SerializeField]
    private ItemDirector ItemDirector;

    private Dictionary<ItemType, GameObject> mItemLibrary;
    private RectTransform mContentTransform;
    private GridLayoutGroup mContentGridLayout;
    private Vector2 mCellSize;
    private Vector2 mSpacing;
    private float mContentDefaultWidth;
    private int mFitCount;
    private float mCellUnitWidth;

    private int mGoldAmount = 0;
    private int mItemCount = 0;
    private bool mIsInitialized = false;
    private object locker = new object();

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void Init()
    {
        lock (locker)
        {
            if (!mIsInitialized)
            {
                mItemLibrary = new Dictionary<ItemType, GameObject>();

                mContentTransform = Content.GetComponent<RectTransform>();
                mContentGridLayout = Content.GetComponent<GridLayoutGroup>();
                mContentDefaultWidth = GetComponent<RectTransform>().rect.width;

                mCellSize = mContentGridLayout.cellSize;
                mSpacing = mContentGridLayout.spacing;
                mContentTransform.sizeDelta = new Vector2(mContentDefaultWidth, mContentTransform.sizeDelta.y);
                mCellUnitWidth = mCellSize.x + mSpacing.x;
                mFitCount = (int)(mContentDefaultWidth / mCellUnitWidth);
                mIsInitialized = true;
                LoadInventory();
            }
        }
    }
    private void LoadInventory()
    {
        Item[] lItems = null;
        lItems = ConfigManager.Setting.Items;
        mGoldAmount = ConfigManager.Setting.GoldAmount;
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
        Init();
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
            //AddGold(-aItem.Price);
            GoldAmount.text = mGoldAmount.ToString();
        }
    }
    public void SellItem(Item aItem)
    {
        lock (locker)
        {
            AddItem(aItem);
            //AddGold(aItem.Price);
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
                    //itemCellManager.AddItem(aItem.Amount);
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
