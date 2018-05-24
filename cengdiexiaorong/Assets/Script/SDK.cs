using System.Collections;
using System.Collections.Generic;
using Umeng;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Advertisements;
using System;
using admob;
using System.Runtime.InteropServices;
using UnityEngine.Purchasing;

public class SDK : MonoBehaviour, IStoreListener
{

	public static SDK Instance;
	// unity广告ID
	public string unity_ads_game_id = "1798175";
	// iosgamecenter排行榜
	public string gamecenter_board_id = "challenge";
	// ios appid 
	public string APP_ID;
	// ios 去广告ID
	public string AppPurchaseNoAds;

	private void Awake()
	{
		#region 友盟
		GA.Start();
		//调试时开启日志 发布时设置为false
		GA.SetLogEnabled(true);
		#endregion
		InitGameCenter();
		InitPurchase();
		// 初始化谷歌广告
		initAdmob();
		// 初始化unity广告
		if (Advertisement.isSupported)
		{
			Advertisement.Initialize(unity_ads_game_id);
		}
		Instance = this;
	}
	
	//// Use this for initialization
	//void Start () {

	//	#region 友盟
	//	GA.Start();
	//	//调试时开启日志 发布时设置为false
	//	GA.SetLogEnabled(true);
	//	#endregion
	//}

	// 进入关卡
	public void StartLevel(string level_name)
	{
		GA.StartLevel(level_name);
		
	}

	public void FinishLevel(string level_name)
	{
		GA.FinishLevel(level_name);
	}


	private void InitGameCenter()
	{
		Social.localUser.Authenticate(HandleAuthenticated);
	}

	#region ios 评论

	public void SkipToAssetStoreURL()
	{
#if UNITY_IPHONE || UNITY_EDITOR || UNITY_IOS
		var url = string.Format(
			"itms-apps://itunes.apple.com/cn/app/id{0}?mt=8&action=write-review",
			APP_ID);
		Application.OpenURL(url);
#endif
	}


	//public void SkipToAssetStore()
	//{
	//	if (UnityEngine.iOS.Device.RequestStoreReview())
	//	{
	//		Debug.Log("Done");
	//	}
	//}

	[DllImport("__Internal")]
	private static extern void _goComment();
	public void GoToCommnet()
	{
#if UNITY_IPHONE
		setAppId(APP_ID);
        _goComment();
#endif
		Debug.LogError("挑到星级评价");
	}


	// 传数据给iOS
	[DllImport("__Internal")]
	// 给iOS传string参数,无返回值,返回值通过iOS的UnitySendMessage方法返回给Unity
	private static extern void setAppId(string appid);

	public void SetAppId(string appid)
	{
#if UNITY_IPHONE
        setAppId(appid);
#endif
	}


	#endregion
	/// <summary>  
	/// 初始化 GameCenter 结果回调函数  
	/// </summary>  
	/// <param name="success">If set to <c>true</c> success.</param>  
	private void HandleAuthenticated(bool success)
	{
		Debug.Log("*** HandleAuthenticated: success = " + success);
		///初始化成功  
		if (success)
		{
			string userInfo = "Username: " + Social.localUser.userName +
				"\nUser ID: " + Social.localUser.id +
				"\nIsUnderage: " + Social.localUser.underage;
			Debug.Log(userInfo);
		}
		else
		{
			///初始化失败  

		}

	}

	// 成就设置
	public void ReportProgress()
	{
		if (Social.localUser.authenticated)
		{
			Social.ReportProgress("XXXX", 15, HandleProgressReported);
		}
	}

	// 排行榜分数设置
	public void ReportScore(long score)
	{
		if (Social.localUser.authenticated)
		{
			Debug.Log("ReportScore" + score);
			Social.ReportScore(score, gamecenter_board_id, HandleScoreReported);
		}
	}

	// 打开排行榜
	public void ShowLeaderboardUI()
	{
		GA.Event("打开排行榜");
		if (Social.localUser.authenticated)
		{
			Social.ShowLeaderboardUI();
		}
	}
	//打开成就

	public void ShowAchievementsUI()
	{
		if (Social.localUser.authenticated)
		{
			Social.ShowAchievementsUI();
		}
	}

	//上传排行榜分数  
	public void HandleScoreReported(bool success)
	{
		Debug.Log("*** HandleScoreReported: success = " + success);
	}
	//设置 成就  
	private void HandleProgressReported(bool success)
	{
		Debug.Log("*** HandleProgressReported: success = " + success);
	}


	/// <summary>  
	/// 加载成就回调  
	/// </summary>  
	/// <param name="achievements">Achievements.</param>  
	private void HandleAchievementsLoaded(IAchievement[] achievements)
	{
		Debug.Log("* HandleAchievementsLoaded");
		foreach (IAchievement achievement in achievements)
		{
			Debug.Log("* achievement = " + achievement.ToString());
		}
	}

	/// <summary>  
	///   
	/// 成就回调描述  
	/// </summary>  
	/// <param name="achievementDescriptions">Achievement descriptions.</param>  
	private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions)
	{
		Debug.Log("*** HandleAchievementDescriptionsLoaded");
		foreach (IAchievementDescription achievementDescription in achievementDescriptions)
		{
			Debug.Log("* achievementDescription = " + achievementDescription.ToString());
		}
	}


	#region unity 广告

	public void ShowRewardedVideo()
	{
		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowResult;

		Advertisement.Show("rewardedVideo", options);
	}

	public static Action RewardUnityAds;
	void HandleShowResult(ShowResult result)
	{
		if (result == ShowResult.Finished)
		{
			Debug.Log("Video completed - Offer a reward to the player");
			// Reward your player here.
			if (RewardUnityAds != null)
			{
				RewardUnityAds();
			}

		}
		else if (result == ShowResult.Skipped)
		{
			Debug.LogWarning("Video was skipped - Do NOT reward the player");

		}
		else if (result == ShowResult.Failed)
		{
			Debug.LogError("Video failed to show");
		}
	}

	#endregion

	#region google 广告
	public string bannerID;
	public string fullID;
	Admob ad;
	void initAdmob()
	{

		ad = Admob.Instance();
		ad.bannerEventHandler += onBannerEvent;
		ad.interstitialEventHandler += onInterstitialEvent;
		ad.rewardedVideoEventHandler += onRewardedVideoEvent;
		ad.nativeBannerEventHandler += onNativeBannerEvent;
		//ad.initAdmob("ca-app-pub-3940256099942544/2934735716", "ca-app-pub-3940256099942544/4411468910");//all id are admob test id,change those to your
		//ad.setTesting(true);//show test ad
#if UNITY_EDITOR
		ad.initAdmob("ca-app-pub-3940256099942544/2934735716", "ca-app-pub-3940256099942544/4411468910");//all id are admob test id,change those to your
		ad.setTesting(true);//show test ad
#else
		ad.initAdmob(bannerID, fullID);//all id are admob test id,change those to your
#endif

		ad.setGender(AdmobGender.MALE);
		string[] keywords = { "game", "crash", "male game" };
		//  ad.setKeywords(keywords);//set keywords for ad
		Debug.Log("admob inited -------------");

	}

	public void showBannerRelative(AdSize size, int position, int marginY, string instanceName = "defaultBanner")
	{
		bool pursed = GameData.GetPursedRemoveAds();
		if (pursed)
		{
			return;
		}
		if (ad.isInterstitialReady())
		{
			ad.showInterstitial();
		}
		Admob.Instance().showBannerRelative(size, position, marginY);
	}

	public void ShowInterstitial()
	{
		bool pursed = GameData.GetPursedRemoveAds();
		if (pursed)
		{
			return;
		}
		if (ad.isInterstitialReady())
		{
			ad.showInterstitial();
		}
		else
		{
			ad.loadInterstitial();
		}
	}

	void onInterstitialEvent(string eventName, string msg)
	{
		Debug.Log("handler onAdmobEvent---" + eventName + "   " + msg);
		if (eventName == AdmobEvent.onAdLoaded)
		{
			Admob.Instance().showInterstitial();
		}
	}
	void onBannerEvent(string eventName, string msg)
	{
		Debug.Log("handler onAdmobBannerEvent---" + eventName + "   " + msg);
	}
	void onRewardedVideoEvent(string eventName, string msg)
	{
		Debug.Log("handler onRewardedVideoEvent---" + eventName + "  rewarded: " + msg);
	}
	void onNativeBannerEvent(string eventName, string msg)
	{
		Debug.Log("handler onAdmobNativeBannerEvent---" + eventName + "   " + msg);
	}
	#endregion

	#region 去广告
	private IStoreController controller;
	private static IExtensionProvider m_StoreExtensionProvider;
	private void InitPurchase()
	{
		var module = StandardPurchasingModule.Instance();
		ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
		builder.AddProduct(AppPurchaseNoAds, ProductType.NonConsumable);
		UnityPurchasing.Initialize(this, builder);
	}

	/// <summary>
	/// Called when Unity IAP is ready to make purchases.
	/// </summary>
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		Debug.Log("OnInitialized");
		this.controller = controller;
	}

	/// <summary>
	/// Called when Unity IAP encounters an unrecoverable initialization error.
	///
	/// Note that this will not be called if Internet is unavailable; Unity IAP
	/// will attempt initialization until it becomes available.
	/// </summary>
	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("OnInitializeFailed"+ error);
	}

	public static Action<bool> PurchaseRemoveAds;

	public static void HandlePurchaseRemoveAds(bool state)
	{
		if (PurchaseRemoveAds != null)
		{
			PurchaseRemoveAds(state);
		}
	}

	/// <summary>
	/// Called when a purchase completes.
	///
	/// May be called at any time after OnInitialized().
	/// </summary>
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
	{
		Debug.Log("ProcessPurchase-----" + e.purchasedProduct.definition.id);
		// 去广告
		if (string.Equals(e.purchasedProduct.definition.id, AppPurchaseNoAds, System.StringComparison.Ordinal))
		{
			GameData.SetPursedRemoveAds(true);
			HandlePurchaseRemoveAds(true);
		}
		
		return PurchaseProcessingResult.Complete;
	}

	/// <summary>
	/// Called when a purchase fails.
	/// </summary>
	public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
	{
	}

	public void OnPurchaseClicked(string productId)
	{
		if (!IsInitialized())
		{
			Debug.LogError("内购初始化失败");
			return;
		}
		controller.InitiatePurchase(productId);
	}

	private bool IsInitialized()
	{
		return controller != null && m_StoreExtensionProvider != null;
	}

	public void RestorePurchases()
	{
		if (!IsInitialized())
		{
			Debug.LogError("内购恢复初始化失败");
			return;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			// Debug.Log("RestorePurchases started ...");  
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			apple.RestoreTransactions(HandleRestored);
		}
		else
		{
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}
	//如果restore之后，会返回一个状态，如果状态为true，那边以前购买的非消耗物品都会回调一次 ProcessPurchase 然后在这里个回调里面进行处理  
	void HandleRestored(bool result)
	{
		//返回一个bool值，如果成功，则会多次调用支付回调，然后根据支付回调中的参数得到商品id，最后做处理(ProcessPurchase)  
		// Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");  
		//blnRestore = false;
		if (result)
		{
			Debug.Log("Restore success!");
		}
		else
		{
			Debug.Log("Restore Failed!");
		}
	}

	#endregion
}
