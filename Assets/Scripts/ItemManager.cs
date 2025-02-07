using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemManager : MonoBehaviour
{
    public Image ItemImage;
    public Text AmountText;

    public OnItemRunOut OnItemRunOut = () => { };
    private Item mItem;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void CheckDisappear()
    {
        if (mItem.Amount <= 0)
        {
            Destroy(gameObject);
            OnItemRunOut();
        }
    }
    public void SetItem(Item aItem)
    {
        mItem = aItem;
        Debug.Log(string.Format("SetItem({0})", mItem.Amount));
        ItemImage.sprite = mItem.Image;
        ItemImage.preserveAspect = true;
        AmountText.text = mItem.Amount.ToString();
        CheckDisappear();
    }
    public void AddItem(int aAmount)
    {
        Debug.Log(string.Format("AddItem({0}+{1})", mItem.Amount, aAmount));
        mItem.Amount += aAmount;
        AmountText.text = mItem.Amount.ToString();
        CheckDisappear();
    }
    public void UseItem()
    {
        mItem.Amount--;
        AmountText.text = mItem.Amount.ToString();
        CheckDisappear();
    }
}
public delegate void OnItemRunOut();
