using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using Assets.Scripts.Item;
public class AdmobReward : MonoBehaviour
{
    private RewardedAd rewarded;
    private readonly string unitId = "ca-app-pub-9567472410674125/9685504518";
    private readonly string test_unitId = "ca-app-pub-3940256099942544/5224354917";
    private string id;
    private ItemType type;
    private bool IsLoaded = false;
    private bool IsInitialized = false;
    // Start is called before the first frame update
    void Start()
    {
        InitAd();
    }
    void Update()
    {
        if (IsInitialized)
        {
            if (!IsLoaded)
            {
                Load();
            }
        }
    }
    private void InitAd()
    {
        string id = Debug.isDebugBuild ? test_unitId : unitId;
        rewarded = new RewardedAd(id);
        rewarded.OnAdClosed += (sender, e) => { };
        rewarded.OnAdLoaded += (sender, e) => { };
        rewarded.OnUserEarnedReward += (sender, e) =>
        {
            //var item = new Item();
            //item.Type = type;
            //TempSaving.RewardItem(item);
        };
        IsInitialized = true;
        Debug.Log("Rewarded ad Initialized.");
    }
    public void Load()
    {
        try
        {
            AdRequest request = new AdRequest.Builder().Build();
            rewarded.LoadAd(request);
            IsLoaded = true;
            Debug.Log("Rewarded ad loaded.");
        }
        catch (Exception e)
        {
            Debug.Log("Error occured during loading ads.");
        }
    }
    public void ShowIfCan(string itemName)
    {
        if (itemName == "starfish")
        {
            type = ItemType.STARFISH;
        }
        else
        {
            type = ItemType.FISH;
        }
        if (IsLoaded)
        {
            StartCoroutine("ShowScreenAd");
        }
        IsLoaded = false;
    }
    public void Show(string itemName)
    {
        if (itemName == "starfish")
        {
            type = ItemType.STARFISH;
        }
        else
        {
            type = ItemType.FISH;
        }
        AdRequest request = new AdRequest.Builder().Build();
        rewarded.LoadAd(request);
        StartCoroutine("ShowScreenAd");
    }
    private IEnumerator ShowScreenAd()
    {
        while (!rewarded.IsLoaded())
        {
            yield return null;
        }
        rewarded.Show();
        IsLoaded = false;
        GameObject rewardPanel;
        while ((rewardPanel = GameObject.Find("1024x768(Clone)")) == null)
        {
            yield return null;
        }
        rewardPanel.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
    }
}
