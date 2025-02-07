using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public class AdmobBanner : MonoBehaviour
{
    private readonly string unitId = "ca-app-pub-3940256099942544/6300978111";
    private readonly string test_unitId = "ca-app-pub-3940256099942544/6300978111";

    private BannerView banner;
    public AdPosition position;
    private void Start()
    {
        //InitAd();
        //GoogleMobileAds.Api.MobileAds
        //GoogleMobileAds.Api.RewardedAd
    }
    private void InitAd()
    {
        string id = Debug.isDebugBuild ? test_unitId : unitId;
        banner = new BannerView(id, AdSize.SmartBanner, position);
        AdRequest request = new AdRequest.Builder().Build();
        banner.LoadAd(request);
    }
    public void ToggleAd(bool active)
    {
        if (active)
        {
            banner.Show();
        }
        else
        {
            banner.Hide();
        }
    }
    public void DestroyAd()
    {
        banner.Destroy();
    }
}
