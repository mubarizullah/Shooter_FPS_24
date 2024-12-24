using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using Unity.VisualScripting;
public class AdTesting : MonoBehaviour
{
    BannerView bannerView;
    void Start()
    {
       MobileAds.Initialize(initStatus => { });
       RequestBanner();
    }

    void RequestBanner()
    {
        string adUnitID = "ca-app-pub-3940256099942544/6300978111";

        bannerView = new BannerView(adUnitID, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();

        bannerView.LoadAd(request);


    }
}
