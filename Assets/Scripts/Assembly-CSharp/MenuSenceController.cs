using UnityEngine;

public class MenuSenceController : MonoBehaviour
{
	public static MenuSenceController instance;

	public GameObject menuSenceObj;

	public StartMenuController startMenu;

	public StoreMenuController storeMenu;

	public TopLineController topLineUI;

	public CharacterMenuController characterMenu;

	public ExitUIController exitUI;

	public MenuSenceTutorialController menuTutorialController;

	public bool storeFlag;

	public bool loadEnterFlag;

	public bool storeBtnSignal;

	public bool characterBtnSignal;

	public bool achievementBtnSignal;

	public bool collectingBtnSignal;

	public bool packBtnSignal;

	public bool handGunBtnSignal;

	public bool handGunBtnNewSignal;

	public bool machineGunBtnSignal;

	public bool machineGunBtnNewSignal;

	public bool carBtnSignal;

	public bool carBtnNewSignal;

	public GetItemPageController getItemPage;

	public ParticleSystem mainScreenParticle;

	private void OnEnable()
	{
		menuSenceObj.active = true;
	}

	private void OnDisable()
	{
		if (menuSenceObj != null)
		{
			menuSenceObj.active = false;
		}
	}

	private void OnApplicationFocus(bool isFocuse)
	{
		if (!isFocuse)
		{
			Platform.SetNotification();
		}
		else
		{
			Platform.internalCancelNotification();
		}
	}

	public void InitMenuSence()
	{
		StoreDateController.InitDate();
		loadEnterFlag = true;
		((CharacterPageController)characterMenu.characterPageController).SetCloth(GlobalInf.clothIndex);
		topLineUI.RefreshCash();
		topLineUI.RefreshGold();
	}

	public void InitSignal()
	{
		((AchievementPageController)characterMenu.achievementPageController).GetAchievementLevel();
		for (int i = 0; i < ((AchievementPageController)characterMenu.achievementPageController).achievementLevel.Length; i++)
		{
			if (((AchievementPageController)characterMenu.achievementPageController).CheckAchievementState(i))
			{
				characterBtnSignal = true;
				achievementBtnSignal = true;
				break;
			}
		}
		if ((!GlobalInf.collectHandGunDoneFlag && GlobalInf.collectHandGunNum >= GlobalDefine.COLLECT_HAND_GUN_MAXNUM) || (!GlobalInf.collectMachineGunDoneFlag && GlobalInf.collectMachineGunNum >= GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM) || (!GlobalInf.collectCarDoneFlag && GlobalInf.collectCarNum >= GlobalDefine.COLLECT_CAR_MAXNUM))
		{
			characterBtnSignal = true;
			collectingBtnSignal = true;
		}
	}

	private void Start()
	{
		if (AudioController.instance != null)
		{
			AudioController.instance.play(AudioType.MENU);
		}
		if (GlobalInf.backToHandGunPageFlag)
		{
			GlobalInf.backToHandGunPageFlag = false;
			instance.storeMenu.prePage = MENUUISTATE.HANDGUNPAGE;
			instance.loadEnterFlag = false;
			startMenu.gameObject.active = false;
			storeMenu.gameObject.active = false;
			characterMenu.gameObject.active = false;
			exitUI.gameObject.active = false;
			startMenu.startBackGroundObj.SetActiveRecursively(false);
			topLineUI.gameObject.active = true;
			OnChangeStore();
		}
		else if (GlobalInf.backToCarPageFlag)
		{
			GlobalInf.backToCarPageFlag = false;
			instance.storeMenu.prePage = MENUUISTATE.VECHICLESPAGE;
			instance.loadEnterFlag = false;
			startMenu.gameObject.active = false;
			storeMenu.gameObject.active = false;
			characterMenu.gameObject.active = false;
			exitUI.gameObject.active = false;
			startMenu.startBackGroundObj.SetActiveRecursively(false);
			topLineUI.gameObject.active = true;
			OnChangeStore();
		}
		else
		{
			startMenu.gameObject.active = true;
			storeMenu.gameObject.active = false;
			topLineUI.gameObject.active = true;
			characterMenu.gameObject.active = false;
			exitUI.gameObject.active = false;
		}
		Platform.internalCancelNotification();
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		if (GlobalInf.upgradeTutorialFlag)
		{
			menuTutorialController.gameObject.SetActiveRecursively(true);
		}
		else
		{
			menuTutorialController.gameObject.SetActiveRecursively(false);
		}
		if (!Platform.hasWaitFakeLoadingOver)
		{
			Invoke("DelayDisableFakeLoading", Platform.lanuchWaitTime);
		}
		else
		{
			CheckLastDay();
		}
		InitMenuSence();
		InitGame();
		storeMenu.handGunPageController.Init();
		storeMenu.machineGunPageController.Init();
		storeMenu.carPageController.Init();
		((CharacterPageController)characterMenu.characterPageController).InitClothData();
		if (Platform.limitedTimeOfferFlag)
		{
			packBtnSignal = true;
		}
		else
		{
			packBtnSignal = false;
		}
		if (GlobalInf.handgunIndex != -1)
		{
			((HandGunPageController)storeMenu.handGunPageController).CopyHandGunInfo(((HandGunPageController)storeMenu.handGunPageController).itemHandGunList[GlobalInf.handgunIndex].gunInfo);
		}
		if (GlobalInf.machineGunIndex != -1)
		{
			((MachineGunPageController)storeMenu.machineGunPageController).CopyMachineGunInfo(((MachineGunPageController)storeMenu.machineGunPageController).itemMachineGunList[GlobalInf.machineGunIndex].gunInfo);
		}
		if (GlobalInf.playerCarIndex != -1)
		{
			((CarPageControllor)storeMenu.carPageController).CopyCarInfo(((CarPageControllor)storeMenu.carPageController).itemCarList[GlobalInf.playerCarIndex].carInfo);
			if (GlobalInf.playerCarIndex == ((CarPageControllor)storeMenu.carPageController).MOTORINDEX)
			{
			}
			((CarPageControllor)storeMenu.carPageController).CopyPreCarInfo(((CarPageControllor)storeMenu.carPageController).itemCarList[GlobalInf.preCarIndex].carInfo);
		}
	}

	public void DelayDisableFakeLoading()
	{
		Platform.disableFakeLoading();
		Platform.hasWaitFakeLoadingOver = true;
		if (!Platform.hasMainMenuShowFullScreen)
		{
			Platform.hasMainMenuShowFullScreen = true;
			Platform.showFullScreenSmall();
		}
		long serverTime = Platform.getServerTime();
		if (serverTime != 0L)
		{
			int @int = PlayerPrefs.GetInt("LastAutoClickDay", 0);
			int num = (int)(serverTime / 86400);
			if (num - @int >= 1)
			{
				PlayerPrefs.SetInt("LastAutoClickDay", num);
				startMenu.dailyTaskPage.ResetDailyTask();
				PlayerPrefs.SetInt("ClickDailyBonuseFlag", 0);
				startMenu.CheckDailyBounseSignal();
				PlayerPrefs.SetInt("AdsHandGunBullet", 0);
				PlayerPrefs.SetInt("AdsMachineGunBullet", 0);
				PlayerPrefs.SetInt("AdsHealthKit", 0);
				PlayerPrefs.SetInt("AdsToolKit", 0);
				GlobalInf.adsHandGunBulletFlag = true;
				GlobalInf.adsMachineGunBulletFlag = true;
				GlobalInf.adsHealthKitFlag = true;
				GlobalInf.adsToolKitFlag = true;
				GlobalInf.goldVideoCount = 0;
				StoreDateController.SetGoldVideoCount();
			}
			else
			{
				startMenu.dailyTaskPage.ResumeDailyTask();
			}
		}
		else
		{
			startMenu.dailyTaskPage.ResumeDailyTask();
		}
	}

	public void CheckLastDay()
	{
		long serverTime = Platform.getServerTime();
		if (serverTime != 0L)
		{
			int @int = PlayerPrefs.GetInt("LastAutoClickDay", 0);
			int num = (int)serverTime / 86400;
			if (num - @int >= 1)
			{
				PlayerPrefs.SetInt("LastAutoClickDay", num);
				startMenu.dailyTaskPage.ResetDailyTask();
				PlayerPrefs.SetInt("ClickDailyBonuseFlag", 0);
				startMenu.CheckDailyBounseSignal();
			}
			else
			{
				startMenu.dailyTaskPage.ResumeDailyTask();
			}
		}
		else
		{
			startMenu.dailyTaskPage.ResumeDailyTask();
		}
	}

	private void InitGame()
	{
		GlobalInf.handGunInfo = new GunsInfo();
		GlobalInf.machineGunInfo = new GunsInfo();
		GlobalInf.carInfo = new CarInfo();
		GlobalInf.preCarInfo = new CarInfo();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.K))
		{
			PlayerPrefs.DeleteAll();
		}
	}

	public void OnChangeStore()
	{
		CancelInvoke("DisactiveStoreMenu");
		Invoke("DisactiveStartMenu", 1.1f);
		storeMenu.gameObject.active = true;
		topLineUI.cashBtn.transform.localPosition = new Vector3(0.15f * GlobalDefine.screenRatioWidth, 0f, 0f);
		topLineUI.goldBtn.transform.localPosition = new Vector3(0.36f * GlobalDefine.screenRatioWidth, 0f, 0f);
		storeMenu.backBtn.transform.localPosition = new Vector3(-0.43f * GlobalDefine.screenRatioWidth, 0f, 0f);
		topLineUI.topLineObj.GetComponent<Animation>().Play("topLineEnter");
		storeFlag = true;
		storeMenu.backBtn.gameObject.SetActiveRecursively(true);
		characterMenu.backBtn.gameObject.SetActiveRecursively(false);
		mainScreenParticle.Stop();
	}

	private void DisactiveStartMenu()
	{
		startMenu.gameObject.SetActiveRecursively(false);
	}

	public void OnChangeStartMenu()
	{
		startMenu.gameObject.SetActiveRecursively(true);
		Invoke("DisactiveStoreMenu", 1.1f);
		Invoke("DisactiveChacterMenu", 1.1f);
		mainScreenParticle.Play();
		if (storeFlag)
		{
			topLineUI.topLineObj.GetComponent<Animation>().Play("topLineExit");
		}
	}

	private void DisactiveStoreMenu()
	{
		storeMenu.gameObject.SetActiveRecursively(false);
	}

	public void OnChangeCharacterMenu()
	{
		characterMenu.gameObject.active = true;
		Invoke("DisactiveStartMenu", 1.1f);
		topLineUI.cashBtn.transform.localPosition = new Vector3(0.15f * GlobalDefine.screenRatioWidth, 0f, 0f);
		topLineUI.goldBtn.transform.localPosition = new Vector3(0.36f * GlobalDefine.screenRatioWidth, 0f, 0f);
		characterMenu.backBtn.transform.localPosition = new Vector3(-0.43f * GlobalDefine.screenRatioWidth, 0f, 0f);
		topLineUI.topLineObj.GetComponent<Animation>().Play("topLineEnter");
		storeMenu.backBtn.gameObject.SetActiveRecursively(false);
		characterMenu.backBtn.gameObject.SetActiveRecursively(true);
		storeFlag = true;
	}

	private void DisactiveChacterMenu()
	{
		characterMenu.gameObject.SetActiveRecursively(false);
	}

	public void OnEnableExitUI()
	{
		exitUI.gameObject.active = true;
		if (Platform.isFullScreenSmallReady())
		{
			exitUI.Reset(true);
		}
		else
		{
			exitUI.Reset(false);
		}
		MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.EXITPAGE);
	}

	public void OnDisableExitUI()
	{
		exitUI.gameObject.active = false;
		Platform.hideFullScreenSmall();
		MenuSenceBackBtnCtl.instance.PopMenuUIState();
	}
}
