using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHand : MonoBehaviour
{
    public static List<GameObject> starfishList;
    private GameObject itemTemp;
    private Item item;
    public GameObject fishItem;
    public GameObject starfishItem;
    public AudioClip fishBubble;
    public AudioClip starfishSound;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (starfishList == null)
        {
            starfishList = new List<GameObject>();
        }
    }
    void Update()
    {
    }
    private void HandItem()
    {
    }
    public void UseItem()
    {
        if (TempSaving.HasItem())
        {
            TempSaving.GetItem(out item);
            if (item.Type == ItemType.FISH)
            {
                audioSource.clip = fishBubble;
                itemTemp = Instantiate(fishItem, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
            }
            else if (item.Type == ItemType.STARFISH)
            {
                audioSource.clip = starfishSound;
                itemTemp = Instantiate(starfishItem, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity);
                starfishList.Add(itemTemp);
            }
            audioSource.Play();
            if (itemTemp == null)
            {
                Debug.Log("itemTemp is null");
            }
            else
            {
                itemTemp.GetComponent<ItemController>().BeUsed(item);
            }
            item = null;
        }
    }
}
