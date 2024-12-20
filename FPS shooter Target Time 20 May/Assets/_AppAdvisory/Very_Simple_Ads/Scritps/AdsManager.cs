
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/


#pragma warning disable 0162 // code unreached.
#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used

//#define IAD
//#define ENABLE_ADMOB
//#define CHARTBOOST
//#define ENABLE_ADCOLONY
//#define ADCOLONY_INTERSTITIAL
//#define ADCOLONY_REWARDED_VIDEO

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif


namespace AppAdvisory.Ads
{
	/// <summary>
	/// This is a static class that references a singleton object in order to provide a simple interface for easely configure Ads in the game
	///
	/// Class in charge to display ads in the game (banners, interstitials and rewarded videos) - please refere to the ADS_INTEGRATION_DOCUMENTATION.PDF
	/// </summary>
	public class AdsManager : AppAdvisory.Ads.Singleton<AdsManager>
	{
		protected AdsManager () {} // guarantee this will be always a singleton only - can't use the constructor!

		[HideInInspector]
		public InterstitialNetwork interstitialNetworkToShowAtRun = InterstitialNetwork.NULL;

		[HideInInspector]
		public bool showInterstitialFirstRun = true;

		[HideInInspector]
		public bool showBannerAtRun = true;


		#if ENABLE_ADMOB
		[HideInInspector]
		public GoogleMobileAds.Api.AdSize AdmobBannerSize = GoogleMobileAds.Api.AdSize.SmartBanner;
		[HideInInspector]
		public GoogleMobileAds.Api.AdPosition AdmobBannerPosition = GoogleMobileAds.Api.AdPosition.Bottom;
		#endif


		#if ENABLE_FACEBOOK
		[HideInInspector]
		public AudienceNetwork.AdSize FacebookBannerSize =  AudienceNetwork.AdSize.BANNER_HEIGHT_50;
		[HideInInspector]
		public FacebookBannerPosition FacebookBannerPosition = FacebookBannerPosition.Bottom;
		#endif

		public delegate void OnBannerShow();
		public static event OnBannerShow OnBannerShowed;
		public static void DOBannerShowed()
		{
			if(OnBannerShowed != null)
				OnBannerShowed();
		}

		public delegate void OnInterstitialOpen();
		public static event OnInterstitialOpen OnInterstitialOpened;
		public static void DOInterstitialOpened()
		{
			if(OnInterstitialOpened != null)
				OnInterstitialOpened();
		}

		public delegate void OnInterstitialClose();
		public static event OnInterstitialClose OnInterstitialClosed;
		public static void DOInterstitialClosed()
		{
			if(OnInterstitialClosed != null)
				OnInterstitialClosed();
		}


		public delegate void OnVideoInterstitialOpen();
		public static event OnVideoInterstitialOpen OnVideoInterstitialOpened;
		public static void DOVideoInterstitialOpened()
		{
			if(OnVideoInterstitialOpened != null)
				OnVideoInterstitialOpened();
		}

		public delegate void OnVideoInterstitialClose();
		public static event OnVideoInterstitialClose OnVideoInterstitialClosed;
		public static void DOVideoInterstitialClosed()
		{
			if(OnVideoInterstitialClosed != null)
				OnVideoInterstitialClosed();
		}

		[HideInInspector]
		public bool randomize = false;


		[SerializeField, HideInInspector] public List<BannerNetwork> bannerNetworks = new List<BannerNetwork> ();

		[SerializeField, HideInInspector] public BannerNetwork bannerNetwork;
		[SerializeField, HideInInspector] public List<InterstitialNetwork> interstitialNetworks = new List<InterstitialNetwork>();
		[SerializeField, HideInInspector] public List<VideoNetwork> videoNetworks = new List<VideoNetwork>();
		[SerializeField, HideInInspector] public List<RewardedVideoNetwork> rewardedVideoNetworks = new List<RewardedVideoNetwork>();

		IBanner currentBanner;
		List<IBanner> listBanners = new List<IBanner> ();
		//IBanner banner;
		List<IInterstitial> listInterstitials = new List<IInterstitial>();
		List<IVideoAds> listVideos = new List<IVideoAds>();
		List<IRewardedVideo> listRewardedVideos = new List<IRewardedVideo>();




		/// <summary>
		/// To store the time and know when we have to show an interstitial at game over if, and only if, basedTimeInterstitialAtGameOver = true
		/// </summary>
		float realTimeSinceStartup;

		public ADIDS m_adIds;
		[SerializeField] public ADIDS adIds
		{
			get
			{
				if(m_adIds == null)
				{
					Debug.LogWarning("ADIDS not in the scene!, please add it by clicking right on the hierarchy view -> app advisory > Very Simple Ads > Create Adids");

					GameObject gameObject = new GameObject("AdsInit");
					AdsInit a = gameObject.AddComponent<AdsInit>();
				}

				return m_adIds;
			}
			set
			{
				m_adIds = value;
			}
		}


		void Randomize()
		{
			if(randomize)
			{
				if(interstitialNetworks != null && interstitialNetworks.Count > 0)
					interstitialNetworks.Shuffle();

				if(videoNetworks != null && videoNetworks.Count > 0)
					videoNetworks.Shuffle();

				if(rewardedVideoNetworks != null && rewardedVideoNetworks.Count > 0)
					rewardedVideoNetworks.Shuffle();

				if (bannerNetworks != null && bannerNetworks.Count > 0)
					bannerNetworks.Shuffle ();


				if(listInterstitials != null && listInterstitials.Count > 0)
					listInterstitials.Shuffle();

				if(listVideos != null && listVideos.Count > 0)
					listVideos.Shuffle();

				if(listRewardedVideos != null && listRewardedVideos.Count > 0)
					listRewardedVideos.Shuffle();

				if (listBanners != null && listBanners.Count > 0)
					listBanners.Shuffle ();
			}
		}

		#if ENABLE_ADMOB
		void AddAdmob()
		{
		if(gameObject.GetComponent<AAAdmob>() == null)
		gameObject.AddComponent<AAAdmob>();

		GetComponent<AAAdmob>().SetBannerSizeAndPosition(AdmobBannerSize,AdmobBannerPosition);
		GetComponent<AAAdmob>().Init();
		}
		#endif

		#if ENABLE_UNITY_ADS
		//#if UNITY_ADS
		void AddUnityAds()
		{
		if(gameObject.GetComponent<AAUnityAds>() == null)
		gameObject.AddComponent<AAUnityAds>();

			GetComponent<AAUnityAds>().Init();

		}
		#endif

		#if ENABLE_ADCOLONY
		void AddADColony()
		{
		if(gameObject.GetComponent<AAADColony>() == null)
		gameObject.AddComponent<AAADColony>();

		GetComponent<AAADColony>().Init();
		}
		#endif

		#if CHARTBOOST
		void AddChartboost()
		{
		if(gameObject.GetComponent<AAChartboost>() == null)
		gameObject.AddComponent<AAChartboost>();

		GetComponent<AAChartboost>().Init();
		}
		#endif

		#if ENABLE_FACEBOOK
		void AddFacebook()
		{
		if (gameObject.GetComponent<AAFacebook> () == null)
		gameObject.AddComponent<AAFacebook> ();

		GetComponent<AAFacebook> ().SetBannerSizeAndPosition (FacebookBannerSize, FacebookBannerPosition);
		GetComponent<AAFacebook> ().Init ();
		}
		#endif

		IInterstitial FIRST_interstitialNetwork;

		public void DOAwake()
		{
			var a = FindObjectsOfType<AdsManager>();
			if(a!= null && a.Length > 1)
			{
				foreach(var ad in a)
				{
					if(ad != this)
					{
						Destroy(ad.gameObject);
					}
				}

				return;
			}


			#if ENABLE_ADMOB
			AddAdmob();
			//banner = GetComponent<AAAdmob>();
			#endif

			#if ENABLE_UNITY_ADS
			//#if UNITY_ADS
			AddUnityAds();
			#endif

			#if ENABLE_ADCOLONY
			AddADColony();
			#endif

			#if CHARTBOOST
			AddChartboost();
			#endif

			#if ENABLE_FACEBOOK
			AddFacebook();
			//banner = GetComponent<AAFacebook>();
			#endif

			if (bannerNetworks != null) 
			{
				if (listBanners == null)
					listBanners = new List<IBanner> ();

				foreach (BannerNetwork bannerNetworkType in bannerNetworks) 
				{
					#if ENABLE_ADMOB
					if(bannerNetworkType == BannerNetwork.Admob)
					{
					if(!listBanners.Contains(GetComponent<AAAdmob>()))
					{
					listBanners.Add(GetComponent<AAAdmob>());
					}
					}
					#endif

					#if ENABLE_FACEBOOK
					if(bannerNetworkType == BannerNetwork.Facebook) 
					{
					if(!listBanners.Contains(GetComponent<AAFacebook>()))
					{
					listBanners.Add(GetComponent<AAFacebook>());
					}
					}
					#endif
				}

			}

			if(interstitialNetworks != null)
			{
				if(listInterstitials == null)
					listInterstitials = new List<IInterstitial>();



				foreach(var m in interstitialNetworks)
				{
					#if ENABLE_ADMOB
					if(m == InterstitialNetwork.Admob)
					{
					if(!listInterstitials.Contains(GetComponent<AAAdmob>()))
					{
					listInterstitials.Add(GetComponent<AAAdmob>());
					if(m == interstitialNetworkToShowAtRun)
					FIRST_interstitialNetwork = GetComponent<AAAdmob>();
					}
					}
					#endif

					#if CHARTBOOST
					if(m == InterstitialNetwork.Chartboost)
					{
					if(!listInterstitials.Contains(GetComponent<AAChartboost>()))
					{
					listInterstitials.Add(GetComponent<AAChartboost>());
					if(m == interstitialNetworkToShowAtRun)
					FIRST_interstitialNetwork = GetComponent<AAChartboost>();
					}
					}
					#endif

					#if ENABLE_FACEBOOK
					if(m == InterstitialNetwork.Facebook)
					{
					if(!listInterstitials.Contains(GetComponent<AAFacebook>()))
					{
					listInterstitials.Add(GetComponent<AAFacebook>());
					if(m == interstitialNetworkToShowAtRun)
					FIRST_interstitialNetwork = GetComponent<AAFacebook>();
					}
					}
					#endif
				}


				//				if(listInterstitials != null && listInterstitials.Count > 0)
				//					FIRST_interstitialNetwork = listInterstitials[0];
			}

			if(videoNetworks != null)
			{
				if(listVideos == null)
					listVideos = new List<IVideoAds>();

				foreach(var m in videoNetworks)
				{

					#if ENABLE_ADCOLONY
					if(m == VideoNetwork.ADColony)
					{
					if(!listVideos.Contains(GetComponent<AAADColony>()))
					{
					listVideos.Add(GetComponent<AAADColony>());
					}
					}
					#endif

					#if ENABLE_UNITY_ADS
					//#if UNITY_ADS
					if(m == VideoNetwork.UnityAds)
					{
					if(!listVideos.Contains(GetComponent<AAUnityAds>()))
					{
					listVideos.Add(GetComponent<AAUnityAds>());
					}
					}
					#endif
				}
			}

			if(rewardedVideoNetworks != null)
			{
				if(listRewardedVideos == null)
					listRewardedVideos = new List<IRewardedVideo>();

				foreach(var m in rewardedVideoNetworks)
				{
					#if ENABLE_ADCOLONY
					if(m == RewardedVideoNetwork.ADColony)
					{
					if(!listRewardedVideos.Contains(GetComponent<AAADColony>()))
					{
					listRewardedVideos.Add(GetComponent<AAADColony>());
					}
					}
					#endif

					#if ENABLE_UNITY_ADS
					//#if UNITY_ADS
					if(m == RewardedVideoNetwork.UnityAds)
					{
					if(!listRewardedVideos.Contains(GetComponent<AAUnityAds>()))
					{
					listRewardedVideos.Add(GetComponent<AAUnityAds>());
					}
					}
					#endif

					#if CHARTBOOST
					if(m == RewardedVideoNetwork.Chartboost)
					{
					if(!listRewardedVideos.Contains(GetComponent<AAChartboost>()))
					{
					listRewardedVideos.Add(GetComponent<AAChartboost>());
					}
					}
					#endif

					#if ENABLE_ADMOB
					if(m == RewardedVideoNetwork.Admob)
					{
					if(!listRewardedVideos.Contains(GetComponent<AAAdmob>()))
					{
					listRewardedVideos.Add(GetComponent<AAAdmob>());
					}
					}
					#endif
				}
			}

			DontDestroyOnLoad(gameObject);
		}

		bool nextSceneIsLoaded = false;

		void LoadNextScene()
		{
			if(nextSceneIsLoaded)
				return;

			Debug.Log("@@@@@@ LOAD NEXT SCENE");


			nextSceneIsLoaded = true;
			#if UNITY_5_3_OR_NEWER
			//			SceneManager.LoadSceneAsync(1);
			SceneManager.LoadScene(1);
			#else
			Application.LoadLevel(1);
			#endif
		}


		bool waitForInternet = true;

		public void DOStart()
		{
			StopAllCoroutines();
			StartCoroutine(_DOStart());
		}

		IEnumerator _DOStart()
		{
			yield return new WaitForSeconds(0.3f);

			nextSceneIsLoaded = true;
			waitForInternet = true;

			if(adIds.LoadNextSceneWhenAdLoaded)
			{
				nextSceneIsLoaded = false;
			}
			else
			{
				nextSceneIsLoaded = true;
			}

			float timeSinceStartup = Time.realtimeSinceStartup;


			while(waitForInternet)
			{
				yield return new WaitForSeconds(0.1f);

				WWW www = new WWW("http://google.com");

				yield return www;

				if (www.error != null) 
				{
					print ("----------> WWW waitForInternet = true");

					waitForInternet = true;

					if((Time.realtimeSinceStartup - timeSinceStartup)  > 4)
					{
						waitForInternet = false;
						break;
					}
				}
				else 
				{
					print ("----------> WWW waitForInternet = false");

					waitForInternet = false;
					break;
				}

				yield return new WaitForSeconds(0.1f);

				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					print ("----------> Application.internetReachability == NetworkReachability.NotReachable");

					waitForInternet = true;

					if((Time.realtimeSinceStartup - timeSinceStartup)  > 4)
					{
						waitForInternet = false;
						break;
					}
				} else {
					print ("----------> Application.internetReachability != NetworkReachability.NotReachable");

					waitForInternet = false;
					break;
				}
			}


			if(this.showBannerAtRun)
				StartCoroutine(CoroutShowBanner());

			if (adIds.ShowIntertitialAtStart && !nextSceneIsLoaded) 
			{
				yield return new WaitForSeconds (0.3f);

				StartCoroutine (CoroutShowInterstitialStartup ());
			}


			if(!adIds.ShowIntertitialAtStart && bannerNetwork == BannerNetwork.NULL)
			{
				yield return new WaitForSeconds(0.3f);

				LoadNextScene();
			}

			float timeToWaitToShowNextSceneBackup = 3f;

			yield return new WaitForSeconds(timeToWaitToShowNextSceneBackup);

			if(!nextSceneIsLoaded)
			{
				LoadNextScene();
			}
		}

		bool bannerIsShowed = false;
		bool interstitialStartIsShowed = false;
	
		IEnumerator CoroutShowBanner()
		{
			while (true) 
			{
				yield return new WaitForSecondsRealtime (1f);

				WWW www = new WWW("http://google.com");

				yield return www;

				if (www.error != null) 
				{
					print ("----------> WWW wait to show banner = true");
				}
				else 
				{
					print ("----------> WWW wait to show banner = BREAK!");
					break;
				}

				yield return new WaitForSeconds(0.1f);

				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					print ("----------> wait to show banner Application.internetReachability == NetworkReachability.NotReachable");

				} else {
					print ("----------> wait to show banner Application.internetReachability == BREAK");
					break;
				}
			}

			bannerIsShowed = true;

			ShowBanner();
			OnBannerShowed += AdsManager_OnBannerShowed;
			yield return null;
		}

		IEnumerator CoroutShowInterstitialStartup()
		{
			while (true) 
			{
				yield return new WaitForSecondsRealtime (1f);

				WWW www = new WWW("http://google.com");

				yield return www;

				if (www.error != null) 
				{
					print ("----------> WWW wait to show Interstitial Startup = true");
				}
				else 
				{
					print ("----------> WWW wait to show Interstitial Startup = BREAK!");
					break;
				}

				yield return new WaitForSeconds(0.1f);

				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					print ("----------> wait to show Interstitial Startup Application.internetReachability == NetworkReachability.NotReachable");

				} else {
					print ("----------> wait to show Interstitial Startup Application.internetReachability == BREAK");
					break;
				}
			}

			interstitialStartIsShowed = true;

			this.ShowInterstitialStartup();

			OnInterstitialOpened += AdsManager_OnInterstitialSTARTUPOpened;
		}

		void AdsManager_OnInterstitialSTARTUPOpened()
		{

			OnInterstitialOpened -= AdsManager_OnInterstitialSTARTUPOpened;

			interstitialStartIsShowed = true;

			if(!nextSceneIsLoaded)
			{
				StopCoroutine("CoroutLoadNextScene");
				StartCoroutine("CoroutLoadNextScene");
			}
		}

		public void ShowInterstitialStartup()
		{

			if(isNoAds)
				return;

			if(!this.showInterstitialFirstRun)
			{
				bool isFirstRun = PlayerPrefs.GetInt("IS_IT_FIRST_RUN", 0) == 0;

				PlayerPrefs.SetInt("IS_IT_FIRST_RUN", 1);
				PlayerPrefs.Save();

				if(isFirstRun)
					return;
			}

			PlayerPrefs.SetInt("IS_IT_FIRST_RUN", 1);
			PlayerPrefs.Save();

			if(FIRST_interstitialNetwork != null)
			{
				FIRST_interstitialNetwork.ShowInterstitialStartup(null);

				return;
			}
		}


		void AdsManager_OnBannerShowed ()
		{
			OnBannerShowed -= AdsManager_OnBannerShowed;

			bannerIsShowed = true;

			if(!nextSceneIsLoaded)
			{
				StopCoroutine("CoroutLoadNextScene");
				StartCoroutine("CoroutLoadNextScene");
			}
		}

		void AdsManager_OnInterstitialOpened ()
		{
			OnInterstitialOpened -= AdsManager_OnInterstitialOpened;

			interstitialStartIsShowed = true;

			if(!nextSceneIsLoaded)
			{
				StopCoroutine("CoroutLoadNextScene");
				StartCoroutine("CoroutLoadNextScene");
			}
		}

		IEnumerator CoroutLoadNextScene() 
		{
			while(!bannerIsShowed && !interstitialStartIsShowed)
			{
				yield return new WaitForSeconds(1f);
			}

			LoadNextScene();
		}
			
		public void SetNoAdsPuschased()
		{

			PlayerPrefs.SetInt("AA NO ADS", 1);
			PlayerPrefs.Save();
			HideBanner();
			DestroyBanner();
		}

		public bool isNoAds
		{
			get
			{
				return PlayerPrefs.GetInt("AA NO ADS", 0) == 1;
			}
		}

		public void ShowBanner()
		{
			if(isNoAds)
				return;

			#if UNITY_EDITOR
			Debug.LogWarning("There are no Banners on the Unity Editor ! Please build your project on a mobile device !");
			#endif

			if(listBanners != null && listBanners.Count > 0)
			{
				if(randomize)
					listBanners.Shuffle();

				List<IBanner> listTemp = new List<IBanner>();

				foreach(var it in listBanners)
				{
					listTemp.Add(it);
				}

				if(randomize)
				{
					listTemp.Shuffle();
				}

				_ShowBanner (listTemp);
			}
		}

		public void DestroyBanner()
		{
			#if UNITY_EDITOR
			Debug.LogWarning("There are no Banners on the Unity Editor ! Please build your project on a mobile device !");
			#endif

			if (currentBanner == null)
				return;

			if (listBanners != null && listBanners.Count > 0) 
			{
				if(listBanners.Contains(currentBanner))
				{
					listBanners.Remove (currentBanner);
				}
			}

			currentBanner.DestroyBanner ();
			currentBanner = null;
		}

		public void HideBanner()
		{
			#if UNITY_EDITOR
			Debug.LogWarning("There are no Banners on the Unity Editor ! Please build your project on a mobile device !");
			#endif

			if (currentBanner == null)
				return;

			currentBanner.HideBanner ();

		}

		public void ShowInterstitial()
		{
			if(isNoAds)
				return;

			#if UNITY_EDITOR
			Debug.LogWarning("There are no Interstitials on the Unity Editor ! Please build your project on a mobile device !");
			#endif

			if(listInterstitials != null && listInterstitials.Count > 0)
			{
				if(randomize)
					listInterstitials.Shuffle();

				List<IInterstitial> listTemp = new List<IInterstitial>();

				foreach(var it in listInterstitials)
				{
					listTemp.Add(it);
				}

				if(randomize)
				{
					listTemp.Shuffle();
				}

				_ShowInterstitial(listTemp);
			}
		}



		void _ShowInterstitial(List<IInterstitial> listTemp)
		{
			if(isNoAds)
				return;

			print("###### _ShowInterstitial : " + listTemp.GetType().ToString());

			var i = listTemp[0];
			listTemp.RemoveAt(0);

			print("trying ShowInterstitial for : " + i.GetType().ToString());

			i.ShowInterstitial((bool success) => {
				if(success)
				{
					print("success ShowInterstitial for : " + i.GetType().ToString());
				}
				else
				{
					print("fail ShowInterstitial for : " + i.GetType().ToString());

					if(listTemp != null && listTemp.Count > 0)
					{
						_ShowInterstitial(listTemp);
					}
				}
			});
		}

		void _ShowBanner(List<IBanner> listTemp) 
		{
			if (isNoAds)
				return;


			print("###### _ShowBanner : " + listTemp.GetType().ToString());

			var i = listTemp[0];
			listTemp.RemoveAt(0);

			print("trying ShowBanner for : " + i.GetType().ToString());

			i.ShowBanner ();
			currentBanner = i;

			// il faudrait savoir si on a réussi à afficher la banière.
			// Si non essayer d'afficher une autre banière

		}

		void CacheAllInterstitial()
		{
			if(isNoAds)
				return;

			if(listInterstitials != null && listInterstitials.Count > 0)
			{
				foreach(var itt in listInterstitials)
				{
					itt.CacheInterstitial();
				}
			}
		}

		void CacheAllInterstitialStartup()
		{
			if(isNoAds)
				return;

			if(listInterstitials != null && listInterstitials.Count > 0)
			{
				foreach(var itt in listInterstitials)
				{
					itt.CacheInterstitialStartup();
				}
			}
		}

		public bool IsReadyInterstitial()
		{
			if(isNoAds)
				return false;

			bool isReady = false;

			if(listInterstitials != null && listInterstitials.Count > 0)
			{
				foreach(var itt in listInterstitials)
				{
					if(itt.IsReadyInterstitial())
					{
						isReady = true;
					}
					//					else
					//					{
					//						itt.CacheInterstitial();
					//					}
				}
			}

			return isReady;
		}

		public void ShowVideoAds()
		{
			if(isNoAds)
				return;

			#if UNITY_EDITOR
			Debug.LogWarning("There are no Video Ads on the Unity Editor ! Please build your project on a mobile device !");
			#endif

			if(listVideos != null && listVideos.Count > 0)
			{
				Randomize();

				List<IVideoAds> listTemp = new List<IVideoAds>();

				foreach(var it in listVideos)
				{
					listTemp.Add(it);
				}

				if(randomize)
				{
					listTemp.Shuffle();
				}

				int rand = UnityEngine.Random.Range(0,listTemp.Count);
				var i = listTemp[rand];

				if(!i.IsReadyVideoAds())
				{
					i.CacheVideoAds();

					listTemp.RemoveAt(rand);

					foreach(var itt in listTemp)
					{
						if(itt.IsReadyVideoAds())
						{
							itt.ShowVideoAds();
							return;
						}
						else
						{
							itt.CacheVideoAds();
						}
					}
				}
				else
				{
					i.ShowVideoAds();
					return;
				}
			}
		}

		void CacheAllVideoAds()
		{
			if(isNoAds)
				return;

			if(listVideos != null && listVideos.Count > 0)
			{
				foreach(var itt in listVideos)
				{

					itt.CacheVideoAds();
				}
			}
		}

		public bool IsReadyVideoAds()
		{
			if(isNoAds)
				return false;

			bool isReady = false;

			if(listVideos != null && listVideos.Count > 0)
			{
				foreach(var itt in listVideos)
				{
					if(itt.IsReadyVideoAds())
					{
						isReady = true;
					}
					//					else
					//					{
					//						itt.CacheVideoAds();
					//					}
				}
			}

			return isReady;
		}

		public void ShowRewardedVideo(Action<bool> success)
		{
			#if UNITY_EDITOR
			Debug.LogWarning("There are no Rewarded Videos on the Unity Editor ! Please build your project on a mobile device !");
			#endif

			Debug.Log (listRewardedVideos.Count);
			if(listRewardedVideos != null && listRewardedVideos.Count > 0)
			{
				Randomize();


				List<IRewardedVideo> listTemp = new List<IRewardedVideo>();

				foreach(var it in listRewardedVideos)
				{
					listTemp.Add(it);
				}

				listTemp.Shuffle();

				int rand = UnityEngine.Random.Range(0,listTemp.Count);
				var i = listTemp[rand];

				if(!i.IsReadyRewardedVideo())
				{

					print("AdsManager - rewarded video not ready for " + i.GetType().ToString() + " ==> let's cache it");

					i.CacheRewardedVideo();

					listTemp.RemoveAt(rand);

					foreach(var itt in listTemp)
					{
						if(itt.IsReadyRewardedVideo())
						{
							print("AdsManager - (2nd chance) rewarded video is ready for " + itt.GetType().ToString() + " ==> let's show it");


							itt.ShowRewardedVideo(success);
							return;
						}
						else
						{
							print("AdsManager - (2nd chance) rewarded video not ready for " + itt.GetType().ToString() + " ==> let's cache it");


							itt.CacheRewardedVideo();
						}
					}
				}
				else
				{
					print("AdsManager - rewarded video is ready for " + i.GetType().ToString() + " ==> let's show it");

					i.ShowRewardedVideo(success);
					return;
				}
			}

			if(success != null)
			{
				success(false);
			}
		}

		void CacheAllRewardedVideo()
		{
			if(listRewardedVideos != null && listRewardedVideos.Count > 0)
			{
				foreach(var itt in listRewardedVideos)
				{
					itt.CacheRewardedVideo();
				}
			}
		}

		public bool IsReadyRewardedVideo()
		{
			bool isReady = false;

			if(listRewardedVideos != null && listRewardedVideos.Count > 0)
			{
				foreach(var itt in listRewardedVideos)
				{
					if(itt.IsReadyRewardedVideo())
					{
						isReady = true;
					}
					else
					{
						itt.CacheRewardedVideo();
					}
				}
			}

			return isReady;
		}

		public int numberOfPlayToShowInterstitial = 3;
		public bool randomizeInterstitialAndVideo = true;

		private static readonly string VerySimpleAdsURL = "http://u3d.as/oWD";

		public void ShowAds()
		{
			int count = PlayerPrefs.GetInt("GAMEOVER_COUNT",0);
			count++;

			#if APPADVISORY_ADS
			if(count > numberOfPlayToShowInterstitial)
			{
				string adsTypeShown = "";

				if(!randomizeInterstitialAndVideo)
				{
					if(AppAdvisory.Ads.AdsManager.instance.IsReadyInterstitial())
					{
						adsTypeShown = "SHOW ADS - Interstitial";

						PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
						AppAdvisory.Ads.AdsManager.instance.ShowInterstitial();
					}
				}
				else
				{
					if(UnityEngine.Random.Range(0,100) < 50)
					{
						if(AppAdvisory.Ads.AdsManager.instance.IsReadyInterstitial())
						{
							adsTypeShown = "SHOW ADS - Interstitial";

							PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
							AppAdvisory.Ads.AdsManager.instance.ShowInterstitial();
						}
						else if(AppAdvisory.Ads.AdsManager.instance.IsReadyVideoAds())
						{
							adsTypeShown = "SHOW ADS - Video Ads";

							PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
							AppAdvisory.Ads.AdsManager.instance.ShowVideoAds();
						}    
					}
					else
					{
						if(AppAdvisory.Ads.AdsManager.instance.IsReadyVideoAds())
						{
							adsTypeShown = "SHOW ADS - Video Ads";

							PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
							AppAdvisory.Ads.AdsManager.instance.ShowVideoAds();
						}
						else if(AppAdvisory.Ads.AdsManager.instance.IsReadyInterstitial())
						{
							adsTypeShown = "SHOW ADS - Interstitial";

							PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
							AppAdvisory.Ads.AdsManager.instance.ShowInterstitial();
						}    
					}
				}

//				if(adsTypeShown.Length > 0)
//					GameAnalyticsSDK.GameAnalytics.NewDesignEvent (adsTypeShown);
			}
			else
			{
				PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
			}

			PlayerPrefs.Save();

			#else
			if(count >= numberOfPlayToShowInterstitial)
			{
			Debug.LogWarning("To show ads, please have a look at Very Simple Ad on the Asset Store, or go to this link: " + VerySimpleAdsURL);
			PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
			}
			else
			{
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
			}
			PlayerPrefs.Save();
			#endif
		}
	}
}