using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdmobScreenAd : MonoBehaviour
{
    private readonly string unitId = "ca-app-pub-9567472410674125/8022242515";
    private readonly string test_unitId = "ca-app-pub-3940256099942544/1033173712";
    public bool IsLoaded = false;
    private bool IsInitialized = false;
    private InterstitialAd screenAd;
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
        screenAd = new InterstitialAd(id);
        screenAd.OnAdClosed += (sender, e) => { Debug.Log("ad closed."); };
        screenAd.OnAdLoaded += (sender, e) => { Debug.Log("ad loaded."); };
        Debug.Log("InitAd done.");
        IsInitialized = true;
    }
    public void Show()
    {
        AdRequest request = new AdRequest.Builder().Build();
        screenAd.LoadAd(request);
        StartCoroutine("ShowScreenAd");
    }
    public void Load()
    {
        try
        {
            AdRequest request = new AdRequest.Builder().Build();
            screenAd.LoadAd(request);
            IsLoaded = true;
        }
        catch(Exception e)
        {
            Debug.Log("Error occured during loading ads.");
        }
    }
    public void ShowIfCan()
    {
        if (IsLoaded)
        {
            StartCoroutine("ShowScreenAd");
        }
        IsLoaded = false;
    }
    private IEnumerator ShowScreenAd()
    {
        while (!screenAd.IsLoaded())
        {
            yield return null;
        }
        screenAd.Show();
    }
}
