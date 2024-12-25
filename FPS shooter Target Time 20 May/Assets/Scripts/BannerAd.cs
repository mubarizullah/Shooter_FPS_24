using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAd : MonoBehaviour
{
    string adUnitId = "ca-app-pub-3940256099942544/6300978111";  // Test ad unit id

    BannerView bannerView;                   // BannerView object to display the banner ad

    private void Start()
    {
        MobileAds.Initialize(initStatus => { }); // Initialize the Google Mobile Ads SDK
        CreateBannerView();              // Create the banner view
        LoadBannerAd();               // Load the banner ad
    }

    public void LoadBannerAd()
    {
        if (bannerView == null)
        {
            CreateBannerView();
        }
        AdRequest request = new AdRequest();        // Create an empty ad request
        Debug.Log("Loading Banner Ad");
        bannerView.LoadAd(request);         // Load the banner with the request
    }

    public void CreateBannerView()
    {
        bannerView = new BannerView(adUnitId, AdSize.MediumRectangle, AdPosition.Bottom); // Create a banner ad with the ad unit id, ad size and position
    }


}
