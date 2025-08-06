using System;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
	public GameObject startMenuObj;

	public GameObject startBackGroundObj;

	public UIEventListener playBtn;

	public UIEventListener moreGameBtn;

	public UIEventListener storeBtn;

	public UIEventListener backBtn;

	public UIEventListener characterBtn;

	public UIEventListener settingBtn;

	public UIEventListener dailyBounsBtn;

	public UIEventListener saleBtn;

	public UISprite storeBtnSignal;

	public UISprite storeBtnNewSignal;

	public UISprite characterBtnSignal;

	public UISprite moreGamesBtnSignal;

	public GameObject saleRoot;

	public UILabel remainTimeLabel;

	public TweenColor dailyBounseDisableObj;

	public UIEventListener dailyTaskBtn;

	public DailyTaskController dailyTaskPage;

	public GameObject dailyTaskSignal;

	public GameObject dailyBonuseSignal;

	public UILabel hartCountLabel;

	public UILabel hartCountTimeLabel;

	public int remainHartTime;

	private int min;

	private int sec;

	private string temp;

	private float counttt;

	private float countHartTime;

	private void OnEnable()
	{
		startMenuObj.SetActiveRecursively(true);
		startBackGroundObj.SetActiveRecursively(true);
		if (!MenuSenceController.instance.loadEnterFlag)
		{
			startMenuObj.GetComponent<Animation>().Play("StatMenuEnter");
			startBackGroundObj.GetComponent<Animation>().Play("StartMenuBackEnter");
		}
		else
		{
			MenuSenceController.instance.loadEnterFlag = false;
			startMenuObj.GetComponent<Animation>().Play("StartMenuLoadEnter");
		}
		MenuSenceController.instance.characterMenu.CheckSignal();
		CheckSignal();
		dailyBounseDisableObj.gameObject.SetActiveRecursively(false);
		if (Platform.limitedTimeOfferFlag)
		{
			saleRoot.gameObject.SetActiveRecursively(true);
		}
		else
		{
			saleRoot.gameObject.SetActiveRecursively(false);
		}
		Platform.getHarts();
		ResetHartLabel();
	}

	public void ResetHartLabel()
	{
		Platform.CountHarts();
		hartCountLabel.text = string.Empty + GlobalInf.hartCount + "/" + (3 + GlobalInf.gameLevel);
		if (GlobalInf.hartCount < 3 + GlobalInf.gameLevel)
		{
			remainHartTime = Platform.GetRemainHartCountTime();
			hartCountTimeLabel.text = ChangeSecToMin(remainHartTime);
		}
		else
		{
			hartCountTimeLabel.text = "MAX";
		}
	}

	public string ChangeSecToMin(int val)
	{
		temp = string.Empty;
		min = val / 60;
		sec = val % 60;
		string text = temp;
		temp = text + "0" + min + ":";
		if (sec < 10)
		{
			temp = temp + "0" + sec;
		}
		else
		{
			temp = temp + string.Empty + sec;
		}
		return temp;
	}

	public void CheckSignal()
	{
		if (MenuSenceController.instance.handGunBtnSignal || MenuSenceController.instance.machineGunBtnSignal || MenuSenceController.instance.packBtnSignal || MenuSenceController.instance.carBtnSignal)
		{
			storeBtnSignal.gameObject.SetActiveRecursively(true);
			storeBtnNewSignal.gameObject.SetActiveRecursively(false);
		}
		else
		{
			storeBtnSignal.gameObject.SetActiveRecursively(false);
			if (MenuSenceController.instance.handGunBtnNewSignal || MenuSenceController.instance.machineGunBtnNewSignal || MenuSenceController.instance.carBtnNewSignal)
			{
				storeBtnNewSignal.gameObject.SetActiveRecursively(true);
			}
			else
			{
				storeBtnNewSignal.gameObject.SetActiveRecursively(false);
			}
		}
		if (MenuSenceController.instance.characterBtnSignal)
		{
			characterBtnSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			characterBtnSignal.gameObject.SetActiveRecursively(false);
		}
		if (PlayerPrefs.GetInt("ClickMoreGames", 0) == 0)
		{
			moreGamesBtnSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			moreGamesBtnSignal.gameObject.SetActiveRecursively(false);
		}
		if (dailyTaskPage.CheckDailySignal())
		{
			dailyTaskSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			dailyTaskSignal.gameObject.SetActiveRecursively(false);
		}
		if (PlayerPrefs.GetInt("ClickDailyBonuseFlag", 0) == 0)
		{
			dailyBonuseSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			dailyBonuseSignal.gameObject.SetActiveRecursively(false);
		}
	}

	public void CheckDailyBounseSignal()
	{
		if (PlayerPrefs.GetInt("ClickDailyBonuseFlag", 0) == 0)
		{
			dailyBonuseSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			dailyBonuseSignal.gameObject.SetActiveRecursively(false);
		}
	}

	private void OnDisable()
	{
		if (startMenuObj != null)
		{
			startMenuObj.SetActiveRecursively(false);
		}
		if (startBackGroundObj != null)
		{
			startBackGroundObj.SetActiveRecursively(false);
		}
	}

	private void Awake()
	{
		UIEventListener uIEventListener = playBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickPlayBtn));
		UIEventListener uIEventListener2 = characterBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickCharacterBtn));
		UIEventListener uIEventListener3 = storeBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickStoreBtn));
		UIEventListener uIEventListener4 = moreGameBtn;
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickMoreGameBtn));
		UIEventListener uIEventListener5 = settingBtn;
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnClickSettingBtn));
		UIEventListener uIEventListener6 = dailyBounsBtn;
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnClickDailyBonusBtn));
		UIEventListener uIEventListener7 = saleBtn;
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener7.onClick, new UIEventListener.VoidDelegate(OnClickSaleBtn));
		UIEventListener uIEventListener8 = dailyTaskBtn;
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener8.onClick, new UIEventListener.VoidDelegate(OnClickDailyTaskBtn));
		counttt = 1f;
	}

	private void Update()
	{
		if (Platform.limitedTimeOfferFlag)
		{
			counttt += Time.deltaTime;
			if (counttt >= 1f)
			{
				counttt = 0f;
				remainTimeLabel.text = Platform.GetRemainTime();
				if (!Platform.limitedTimeOfferFlag)
				{
					saleRoot.gameObject.SetActiveRecursively(false);
				}
			}
		}
		countHartTime += Time.deltaTime;
		if (countHartTime >= 1f)
		{
			countHartTime -= 1f;
			ResetHartLabel();
		}
	}

	public void OnClickSaleBtn(GameObject btn)
	{
		MenuSenceController.instance.storeMenu.packPageController.myPackPageScrollView.curIndex = 1;
		MenuSenceController.instance.storeMenu.prePage = MENUUISTATE.SALEPAGE;
		MenuSenceController.instance.startMenu.OnClickStoreBtn(null);
		MenuSenceController.instance.storeMenu.packPageController.OnClickLeftBtn(null);
	}

	public void OnClickPlayBtn(GameObject btn)
	{
		if (GlobalInf.hartCount > 0)
		{
			GlobalInf.nextSence = "Ganstars";
			Application.LoadLevel(0);
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/BuyHartUI")) as GameObject;
		gameObject.transform.parent = startMenuObj.transform.parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		MenuSenceController.instance.topLineUI.cashBtn.transform.localPosition = new Vector3(0.15f * GlobalDefine.screenRatioWidth, 0f, 0f);
		MenuSenceController.instance.topLineUI.goldBtn.transform.localPosition = new Vector3(0.36f * GlobalDefine.screenRatioWidth, 0f, 0f);
		MenuSenceController.instance.topLineUI.topLineObj.GetComponent<Animation>().Play("topLineEnter");
		MenuSenceController.instance.storeMenu.backBtn.gameObject.SetActiveRecursively(false);
		MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.BUYHARTPAGE);
	}

	public void OnClickCharacterBtn(GameObject btn)
	{
		MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MenuSenceController.instance.characterMenu.prePage);
		MenuSenceController.instance.OnChangeCharacterMenu();
		startMenuObj.GetComponent<Animation>().Play("StatMenuExit");
		startBackGroundObj.GetComponent<Animation>().Play("StartMenuBackExit");
	}

	public void OnClickSettingBtn(GameObject btn)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/Setting")) as GameObject;
		gameObject.transform.parent = startMenuObj.transform.parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.SETTINGPAGE);
		startMenuObj.gameObject.SetActiveRecursively(false);
		startBackGroundObj.gameObject.SetActiveRecursively(false);
	}

	public void OnClickStoreBtn(GameObject btn)
	{
		MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MenuSenceController.instance.storeMenu.prePage);
		MenuSenceController.instance.OnChangeStore();
		startMenuObj.GetComponent<Animation>().Play("StatMenuExit");
		startBackGroundObj.GetComponent<Animation>().Play("StartMenuBackExit");
	}

	public void OnClickMoreGameBtn(GameObject btn)
	{
		Platform.showMoreGames();
		PlayerPrefs.SetInt("ClickMoreGames", 1);
		moreGamesBtnSignal.gameObject.SetActiveRecursively(false);
		Platform.flurryEvent_onMoreGameClick();
	}

	public void OnClickDailyBonusBtn(GameObject btn)
	{
		if (Platform.getServerTime() != 0L)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/DailyBounseUI")) as GameObject;
			gameObject.transform.parent = startMenuObj.transform.parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			startMenuObj.gameObject.SetActiveRecursively(false);
			startBackGroundObj.gameObject.SetActiveRecursively(false);
		}
		else
		{
			dailyBounseDisableObj.ResetToBeginning();
			dailyBounseDisableObj.PlayForward();
			dailyBounseDisableObj.gameObject.SetActiveRecursively(true);
			CancelInvoke("DelayDisableDailyBounseLabel");
			Invoke("DelayDisableDailyBounseLabel", 2f);
		}
	}

	public void OnClickDailyTaskBtn(GameObject btn)
	{
		Platform.showFeatureView(FeatureViewPosType.MIDDLE);
		if (!GlobalInf.dailyTaskInitFlag)
		{
			dailyTaskPage.ResetDailyTask();
		}
		dailyTaskPage.gameObject.SetActiveRecursively(true);
		dailyTaskPage.tweenScale.from = Vector3.zero;
		dailyTaskPage.tweenScale.PlayForward();
		dailyTaskPage.InitDailyTask();
		MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.DAILYTASKPAGE);
	}

	public void DelayDisableDailyBounseLabel()
	{
		dailyBounseDisableObj.gameObject.SetActiveRecursively(false);
	}

	public void OnClickRateBtn(GameObject btn)
	{
	}
}
