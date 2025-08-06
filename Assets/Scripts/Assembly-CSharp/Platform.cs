using UnityEngine;

public class Platform : MonoBehaviour
{
	private const float FAKE_LOADING_TIME = 5f;

	private const int LIMITED_TIME_OFFER_TIME = 172800;

	public const int GOODS_COUNT = 12;

	public static AndroidJavaClass jc;

	public static AndroidJavaObject currentActivity;

	public static float lanuchWaitTime;

	public static bool hasWaitFakeLoadingOver;

	public static int HART_COUNT_TIME = 300;

	public static bool limitedTimeOfferFlag;

	private static int remainLimitedTime;

	private static float remainLimitedStartTime;

	private static float limitedStartTime;

	public static bool hasMainMenuShowFullScreen;

	private static bool isInit;

	public static bool isLowAPILevel;

	private static Platform instance;

	public static string[] GOODS_ID = new string[12]
	{
		"product_1", "product_2", "product_3", "product_4", "product_5", "product_6", "product_7", "product_8", "product_9", "product_10",
		"product_11", "product_12"
	};

	public static float[] GOODS_PRICE = new float[12]
	{
		1.99f, 4.99f, 9.99f, 19.99f, 49.99f, 99.99f, 1.99f, 4.99f, 9.99f, 19.99f,
		49.99f, 99.99f
	};

	public static int[] COINS_ADD = new int[12]
	{
		5000, 13000, 30000, 65000, 170000, 350000, 20, 52, 120, 260,
		680, 1400
	};

	public static int testCount = 20;

	public static void init()
	{
		if (!isInit)
		{
			isInit = true;
			InitAndroid();
			if (Debug.isDebugBuild)
			{
				currentActivity = null;
			}
			if (currentActivity != null && !currentActivity.Call<bool>("isUnityAndroidConnectSuccess", new object[0]))
			{
				currentActivity = null;
			}
			if (currentActivity != null)
			{
				lanuchWaitTime = 5f - currentActivity.Call<float>("internalGetLeftTime", new object[0]) / 1000f;
			}
			else
			{
				lanuchWaitTime = 0f;
			}
			if (lanuchWaitTime < 0.1f)
			{
				lanuchWaitTime = 0.1f;
			}
			limitedStartTime = Time.realtimeSinceStartup;
			remainLimitedTime = internaleGetRemainLimitedTime();
			remainLimitedStartTime = remainLimitedTime;
			if (remainLimitedTime <= 0)
			{
				limitedTimeOfferFlag = false;
			}
			else
			{
				limitedTimeOfferFlag = true;
			}
			internalCancelNotification();
			StoreDateController.GetCash();
			StoreDateController.GetGold();
			GlobalInf.onFetchCompletedFlag = false;
			GlobalInf.onPlayCompletedFlag = false;
			if (PlayerPrefs.GetInt("AdsHandGunBullet", 0) == 0)
			{
				GlobalInf.adsHandGunBulletFlag = true;
			}
			else
			{
				GlobalInf.adsHandGunBulletFlag = false;
			}
			if (PlayerPrefs.GetInt("AdsMachineGunBullet", 0) == 0)
			{
				GlobalInf.adsMachineGunBulletFlag = true;
			}
			else
			{
				GlobalInf.adsMachineGunBulletFlag = false;
			}
			if (PlayerPrefs.GetInt("AdsHealthKit", 0) == 0)
			{
				GlobalInf.adsHealthKitFlag = true;
			}
			else
			{
				GlobalInf.adsHealthKitFlag = false;
			}
			if (PlayerPrefs.GetInt("AdsToolKit", 0) == 0)
			{
				GlobalInf.adsToolKitFlag = true;
			}
			else
			{
				GlobalInf.adsToolKitFlag = false;
			}
			GetAPILevel();
		}
	}

	private static void InitAndroid()
	{
		Debug.Log("**************Platform Unity Init:" + Debug.isDebugBuild);
		if (Debug.isDebugBuild)
		{
			currentActivity = null;
			return;
		}
		if (currentActivity == null)
		{
			jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
		}
		Debug.Log("**************Platform Unity InitX:" + currentActivity);
	}

	public static void GetAPILevel()
	{
		if (currentActivity != null)
		{
			isLowAPILevel = currentActivity.Call<bool>("checkAPILevel", new object[0]);
		}
		else
		{
			isLowAPILevel = false;
		}
	}

	public static void disableFeatureScreen()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalDisableFeatureScreen");
		}
	}

	public static void disableFakeLoading()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalHideFakeLoading");
		}
	}

	public static void showFullScreenSmall()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalShowFullScreenSmall");
			flurryEvent_onAdsShow();
		}
	}

	public static void showFullScreenSmallExit()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalShowFullScreenExitSmall");
			flurryEvent_onAdsShow();
		}
	}

	public static void hideFullScreenSmall()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalHideFullScreenSmall");
		}
	}

	public static bool isFullScreenSmallShowing()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<bool>("internalIsFullScreenSmallShowing", new object[0]);
		}
		return false;
	}

	public static bool willShowFullScreenSamll()
	{
		return isFullScreenSmallReady() && !getAdFree();
	}

	public static bool isFullScreenSmallReady()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<bool>("internalIsFulScreenSmallReady", new object[0]) && !getAdFree();
		}
		return false;
	}

	public static void showFeatureView()
	{
		FeatureViewPosType featureViewPosType = FeatureViewPosType.MIDDLE;
		if (Screen.width < 800)
		{
			featureViewPosType = FeatureViewPosType.MIDDLE_LONG;
		}
		if (currentActivity != null)
		{
			currentActivity.Call("internalShowFeatureView", (int)featureViewPosType);
		}
	}

	public static void showFeatureView(FeatureViewPosType type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalShowFeatureView", (int)type);
		}
	}

	public static void hideFeatureView()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalHideFeatureView");
		}
	}

	public static void showMoreGames()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalMoreGames");
		}
	}

	public static void rating()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalDirectToMarket");
		}
	}

	public static void setAdFree()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalSetAdFreeTrue");
		}
		hideFeatureView();
		hideFullScreenSmall();
	}

	public static bool getAdFree()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<bool>("internalGetAdFree", new object[0]);
		}
		return false;
	}

	public static int readGold()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<int>("internalGetGoldAmounts", new object[0]);
		}
		return -1;
	}

	public static bool saveGold(int coins)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalSaveGoldAmounts", coins);
			return true;
		}
		return false;
	}

	public static int readCash()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<int>("internalGetCashAmounts", new object[0]);
		}
		return -1;
	}

	public static bool saveCash(int coins)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalSaveCashAmounts", coins);
			return true;
		}
		return false;
	}

	public static void quitGame()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalQuitGame");
		}
	}

	public static bool canShowDailyBonus()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<bool>("internalCanShowBonus", new object[0]);
		}
		return true;
	}

	public static int getDailyBonusDay()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<int>("internalGetDailyBonusDay", new object[0]);
		}
		return -1;
	}

	public static void setDailyBounsDay()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalSetDailyBonusDay");
		}
	}

	private static void internalSaveTimeLimitedOffer(bool isCompleteCurrent)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalSaveTimeLimitedOffer", isCompleteCurrent ? 1 : 0);
		}
	}

	private static int internaleGetRemainLimitedTime()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<int>("internalGetRemainLimitedTime", new object[0]);
		}
		return 3608;
	}

	public static void internalSetNotification(int id, int hasGetReward, long delay, string title, string message)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalSetNotification", id, hasGetReward, delay, title, message);
		}
	}

	public static void internalCancelNotification()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("clearNotification");
		}
	}

	public static void startTimeLimitedOffer()
	{
		internalSaveTimeLimitedOffer(false);
		remainLimitedTime = 172800;
		remainLimitedStartTime = remainLimitedTime;
		limitedStartTime = Time.realtimeSinceStartup;
		limitedTimeOfferFlag = true;
	}

	public static int getRemainLimitedTime()
	{
		if (remainLimitedTime <= 0)
		{
			limitedTimeOfferFlag = false;
			return -1;
		}
		limitedTimeOfferFlag = true;
		remainLimitedTime = (int)(remainLimitedStartTime - (Time.realtimeSinceStartup - limitedStartTime));
		return remainLimitedTime;
	}

	public static string GetRemainTime()
	{
		int num = getRemainLimitedTime();
		if (num >= 3600)
		{
			return GetRemainTimeMinString();
		}
		return GetRemainTimeSecondString();
	}

	public static string GetRemainTimeFull()
	{
		int num = getRemainLimitedTime();
		string empty = string.Empty;
		int num2 = num / 3600;
		empty = ((num2 >= 10) ? (empty + string.Empty + num2) : (empty + "0" + num2));
		int num3 = num % 3600 / 60;
		empty = ((num3 < 10) ? (empty + ":0" + num3) : (empty + ":" + num3));
		int num4 = num % 3600 % 60;
		if (num4 >= 10)
		{
			return empty + ":" + num4;
		}
		return empty + ":0" + num4;
	}

	public static string GetRemainTimeMinString()
	{
		int num = getRemainLimitedTime();
		string empty = string.Empty;
		int num2 = num / 3600;
		empty = ((num2 >= 10) ? (empty + string.Empty + num2) : (empty + "0" + num2));
		int num3 = num % 3600 / 60;
		if (num3 >= 10)
		{
			return empty + ":" + num3;
		}
		return empty + ":0" + num3;
	}

	public static string GetRemainTimeSecondString()
	{
		int num = getRemainLimitedTime();
		string empty = string.Empty;
		int num2 = num % 3600 / 60;
		empty = ((num2 < 10) ? (empty + "0" + num2) : (empty + string.Empty + num2));
		int num3 = num % 3600 % 60;
		if (num3 >= 10)
		{
			return empty + ":" + num3;
		}
		return empty + ":0" + num3;
	}

	public static void completeCurrentTimeLimitedOffer()
	{
		internalSaveTimeLimitedOffer(true);
		remainLimitedTime = -1;
		limitedTimeOfferFlag = false;
	}

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		instance = this;
		base.gameObject.name = GetType().ToString();
		Object.DontDestroyOnLoad(this);
	}

	private void OnDestroy()
	{
		if (this == instance)
		{
			instance = null;
		}
	}

	public void onAndroidCreate(string message)
	{
		Debug.Log("onAndroidCreate!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		if (hasWaitFakeLoadingOver)
		{
			currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			createBilling();
			disableFakeLoading();
		}
	}

	public static void createBilling()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("creatBilling");
		}
	}

	public static bool callBilling(int id)
	{
		if (id < 0 || id >= GOODS_ID.Length)
		{
			Debug.LogError("callBilling error: wrong id");
			return false;
		}
		if (currentActivity != null)
		{
			currentActivity.Call("callBilling", id);
			return true;
		}
		return false;
	}

	public void onPurcaseSuccess(string purchaseSku)
	{
		for (int i = 0; i < GOODS_ID.Length; i++)
		{
			if (purchaseSku.Equals(GOODS_ID[i]))
			{
				if (i > 0 && i != 6)
				{
					setAdFree();
				}
				if (i < 6)
				{
					GlobalInf.cash += COINS_ADD[i];
					GlobalInf.totalCashEarned += COINS_ADD[i];
					StoreDateController.SetTotalCashEarned(GlobalInf.totalCashEarned);
					StoreDateController.SetCash();
				}
				else
				{
					GlobalInf.gold += COINS_ADD[i];
					GlobalInf.totalGoldEarned += COINS_ADD[i];
					StoreDateController.SetTotalGoldEarned();
					StoreDateController.SetGold();
				}
				return;
			}
		}
		OnChargeEvent(GlobalInf.chargeShowType, 2);
		Debug.LogError("onPurcaseSuccess, but goods id return error");
	}

	public static long getServerTime()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<long>("internalGetServerTime", new object[0]);
		}
		return 0L;
	}

	public void onFullScreenSmallClosed()
	{
		if (MenuSenceBackBtnCtl.instance != null && MenuSenceBackBtnCtl.instance.curState == MENUUISTATE.EXITPAGE)
		{
			MenuSenceController.instance.exitUI.Reset(false);
		}
	}

	public static int getHarts()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("CountHarts");
			return currentActivity.Call<int>("GetHarts", new object[0]);
		}
		return 0;
	}

	public static void CountHarts()
	{
		GlobalInf.hartCount += getHarts();
		if (GlobalInf.hartCount > 3 + GlobalInf.gameLevel)
		{
			GlobalInf.hartCount = 3 + GlobalInf.gameLevel;
		}
		if (GlobalInf.preHartCount != GlobalInf.hartCount)
		{
			GlobalInf.preHartCount = GlobalInf.hartCount;
			StoreDateController.SetHartCount(GlobalInf.hartCount);
		}
	}

	public static int getRemainHartTime()
	{
		if (currentActivity != null)
		{
			return currentActivity.Call<int>("GetRemainHartTime", new object[0]);
		}
		return 0;
	}

	public static void ResetHartTimeCount()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("ResetHartTimeCount");
		}
	}

	public static int GetRemainHartCountTime()
	{
		return HART_COUNT_TIME - getRemainHartTime();
	}

	public static void SetNotification()
	{
		if (GlobalInf.notificationFlag && currentActivity != null)
		{
			currentActivity.Call("setNotificationAlarm");
		}
	}

	public void onUnityAdsFetchCompleted(string str)
	{
		GlobalInf.onFetchCompletedFlag = true;
		if (TopLineController.instance != null)
		{
			if (!GlobalInf.cashVideoFlag)
			{
				TopLineController.instance.CheckAdsBtn();
			}
			else
			{
				TopLineController.instance.CheckCashAdsBtn();
			}
		}
	}

	public void onUnityAdsVideoCompleted(string str)
	{
		GlobalInf.onPlayCompletedFlag = true;
		if (MenuSenceController.instance != null)
		{
			GameObject original = Resources.Load("UI/MenuVideo") as GameObject;
			MenuSenceVideoCheckUI component = ((GameObject)Object.Instantiate(original)).GetComponent<MenuSenceVideoCheckUI>();
			component.Reset(GlobalInf.cashVideoFlag);
			component.transform.parent = TopLineController.instance.rechargeRoot.transform;
			component.transform.localPosition = Vector3.zero;
			component.transform.localScale = Vector3.one;
		}
		else
		{
			GameObject original2 = Resources.Load("UI/GameVideo") as GameObject;
			GameSenceVideoCheckUI component2 = ((GameObject)Object.Instantiate(original2)).GetComponent<GameSenceVideoCheckUI>();
			component2.transform.parent = GameUIController.instance.uiRoot.transform;
			component2.transform.localPosition = Vector3.zero;
			component2.transform.localScale = Vector3.one;
			component2.Reset(GlobalInf.videoType);
		}
	}

	public static bool CheckAdsCanShow()
	{
		if (currentActivity != null)
		{
			return GlobalInf.onFetchCompletedFlag = currentActivity.Call<bool>("checkAdsCanShow", new object[0]);
		}
		GlobalInf.onFetchCompletedFlag = false;
		return false;
	}

	public static void internalShowUnityAds()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("internalShowUnityAds");
		}
	}

	public static void flurryEvent_onClickUnityAddMoneyAds()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onClickUnityAddMoneyAds");
		}
	}

	public static void flurryEvent_onClickUnityAddHealthKitAds()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onClickUnityAddHealthKitAds");
		}
	}

	public static void flurryEvent_onClickUnityAddBulletAds()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onClickUnityAddBulletAds");
		}
	}

	public static void flurryEvent_onTaskStart(int type, int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onTaskStart", type, id);
		}
	}

	public static void flurryEvent_onTaskFirstWin(int type, int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onTaskFirstWin", type, id);
		}
	}

	public static void flurryEvent_onTaskWin(int type, int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onTaskWin", type, id);
		}
	}

	public static void flurryEvent_onTask3Stars(int type, int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onTask3Stars", type, id);
		}
	}

	public static void flurryEvent_onTaskFail(int type, int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onTaskFail", type, id);
		}
	}

	public static void flurryEvent_onTaskQuit(int type, int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onTaskQuit", type, id);
		}
	}

	public static void flurryEvent_onTaskPause(int type, int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onTaskPause", type, id);
		}
	}

	public static void flurryEvent_onEquipmentHandGunPurchase(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentHandGunPurchase", id);
		}
	}

	public static void flurryEvent_onEquipmentMachineGunPurchase(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentMachineGunPurchase", id);
		}
	}

	public static void flurryEvent_onEquipmentCarPurchase(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentCarPurchase", id);
		}
	}

	public static void flurryEvent_onEquipmentKitPurchase(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentKitPurchase", id);
		}
	}

	public static void flurryEvent_onEquipmentCarGetPurchase(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentCarGetPurchase", id);
		}
	}

	public static void flurryEvent_onEquipmentHandGunGetPurchase(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentHandGunGetPurchase", id);
		}
	}

	public static void flurryEvent_onEquipmentMachineGunGetPurchase(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentMachineGunGetPurchase", id);
		}
	}

	public static void flurryEvent_onEquipmentClothPurchase(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentClothPurchase", id);
		}
	}

	public static void flurryEvent_onEquipmentLevelPurchase(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentLevelPurchase", id);
		}
	}

	public static void flurryEvent_onEquipmentHandGunGetReward(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentHandGunGetReward", id);
		}
	}

	public static void flurryEvent_onEquipmentMachineGunGetReward(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentMachineGunGetReward", id);
		}
	}

	public static void flurryEvent_onEquipmentCarGetReward(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentCarGetReward", id);
		}
	}

	public static void flurryEvent_onEquipmentHandGunUpgrade(int id, int level)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentHandGunUpgrade", id, level);
		}
	}

	public static void flurryEvent_onEquipmentMachineGunUpgrade(int id, int level)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentMachineGunUpgrade", id, level);
		}
	}

	public static void flurryEvent_onEquipmentCarUpgrade(int id, int level)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onEquipmentCarUpgrade", id, level);
		}
	}

	public static void flurryEvent_onPackBuyRegular(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onPackBuyRegular", id);
		}
	}

	public static void flurryEvent_onPackBuyLTO(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onPackBuyLTO", id);
		}
	}

	public static void flurryEvent_onLTOLevel(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onLTOLevel", id);
		}
	}

	public static void flurryEvent_onMoreGameClick()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onMoreGameClick");
		}
	}

	public static void flurryEvent_onDailyBonus(int id)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onDailyBonus", id);
		}
	}

	public static void flurryEvent_onAdsShow()
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onAdsShow");
		}
	}

	public static void flurryEvent_onChargeDirect(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargeDirect", type);
		}
	}

	public static void flurryEvent_onChargeEquipment(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargeEquipment", type);
		}
	}

	public static void flurryEvent_onChargePackRegular(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargePackRegular", type);
		}
	}

	public static void flurryEvent_onChargePackLTO(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargePackLTO", type);
		}
	}

	public static void flurryEvent_onChargeBullet(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargeBullet", type);
		}
	}

	public static void flurryEvent_onChargeSlot(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargeSlot", type);
		}
	}

	public static void flurryEvent_onChargeBuyHart(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargeBuyHart", type);
		}
	}

	public static void flurryEvent_onChargeGameKit(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargeGameKit", type);
		}
	}

	public static void flurryEvent_onChargeMenuKit(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargeMenuKit", type);
		}
	}

	public static void flurryEvent_onChargeCloth(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargeCloth", type);
		}
	}

	public static void flurryEvent_onChargeLevel(int type)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onChargeLevel", type);
		}
	}

	public static void flurryEvent_onTutorialState(int index)
	{
		if (currentActivity != null)
		{
			currentActivity.Call("flurryEvent_onTutorialState", index);
		}
	}

	public static void OnChargeEvent(CHARGESHOWTYPE type, int style)
	{
		switch (type)
		{
		case CHARGESHOWTYPE.DIRECT:
			flurryEvent_onChargeDirect(style);
			break;
		case CHARGESHOWTYPE.EQUIPMENT:
			flurryEvent_onChargeEquipment(style);
			break;
		case CHARGESHOWTYPE.PACKLTO:
			flurryEvent_onChargePackLTO(style);
			break;
		case CHARGESHOWTYPE.PACKREGULAR:
			flurryEvent_onChargePackRegular(style);
			break;
		case CHARGESHOWTYPE.BUYHART:
			flurryEvent_onChargeBuyHart(style);
			break;
		case CHARGESHOWTYPE.GAMEKIT:
			flurryEvent_onChargeGameKit(style);
			break;
		case CHARGESHOWTYPE.MENUKIT:
			flurryEvent_onChargeMenuKit(style);
			break;
		case CHARGESHOWTYPE.SLOT:
			flurryEvent_onChargeSlot(style);
			break;
		case CHARGESHOWTYPE.BULLET:
			flurryEvent_onChargeBullet(style);
			break;
		case CHARGESHOWTYPE.CLOTH:
			flurryEvent_onChargeCloth(style);
			break;
		case CHARGESHOWTYPE.LEVEL:
			flurryEvent_onChargeLevel(style);
			break;
		}
	}
}
