using System;
using UnityEngine;

public class TopLineController : MonoBehaviour
{
	public static TopLineController instance;

	public GameObject topLineObj;

	public UILabel cashLabel;

	public UILabel goldLabel;

	public UIEventListener cashBtn;

	public UIEventListener goldBtn;

	public RechargeController rechargeCashObj;

	public RechargeController rechargeGoldObj;

	public bool showFlag;

	public bool cashFlag;

	public bool checkPageFlag;

	public GameObject rechargeRoot;

	public GameObject backLine;

	public UIEventListener adsEnableBtn;

	public UISprite adsEnableBtnSprite;

	public UISprite adsEnableBtnPicSprite;

	public UISprite adsDisableBtnPicSprite;

	public GameObject adsDisableBtn;

	private void OnEnable()
	{
		topLineObj.SetActiveRecursively(true);
		RefreshCash();
		RefreshGold();
		if (MenuSenceController.instance != null)
		{
			CheckAdsBtn();
		}
	}

	private void OnDisable()
	{
		if (topLineObj != null && topLineObj.activeInHierarchy)
		{
			topLineObj.SetActive(false);
		}
	}

	private void Destroy()
	{
		if (instance != null)
		{
			instance = null;
		}
	}

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		UIEventListener uIEventListener = cashBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickCashBtn));
		UIEventListener uIEventListener2 = goldBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickGoldBtn));
		UIEventListener uIEventListener3 = cashBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnPlayerClick));
		UIEventListener uIEventListener4 = goldBtn;
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnPlayerClick));
		if (MenuSenceController.instance != null)
		{
			UIEventListener uIEventListener5 = adsEnableBtn;
			uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnClickAdsEnableBtn));
		}
	}

	public void OnClickAdsEnableBtn(GameObject btn)
	{
		GlobalInf.onFetchCompletedFlag = false;
		if (!GlobalInf.cashVideoFlag)
		{
			CheckAdsBtn();
		}
		else
		{
			CheckCashAdsBtn();
		}
		Platform.internalShowUnityAds();
		Platform.flurryEvent_onClickUnityAddMoneyAds();
	}

	public void CheckAdsBtn()
	{
		if (!Platform.isLowAPILevel)
		{
			GlobalInf.cashVideoFlag = false;
			if (GlobalInf.goldVideoCount < 5)
			{
				adsEnableBtnSprite.spriteName = "upgrate";
				adsEnableBtnPicSprite.spriteName = "free-jinbi";
				adsDisableBtnPicSprite.spriteName = "free-jinbi";
			}
			else
			{
				adsEnableBtnSprite.spriteName = "play";
				adsEnableBtnPicSprite.spriteName = "vedio";
				adsDisableBtnPicSprite.spriteName = "vedio";
			}
			if (Platform.CheckAdsCanShow())
			{
				adsEnableBtn.gameObject.SetActiveRecursively(true);
				adsDisableBtn.SetActiveRecursively(false);
			}
			else
			{
				adsDisableBtn.SetActiveRecursively(true);
				adsEnableBtn.gameObject.SetActiveRecursively(false);
			}
		}
		else
		{
			adsEnableBtn.gameObject.SetActiveRecursively(false);
			adsDisableBtn.SetActiveRecursively(false);
		}
	}

	public void OnPlayerClick(GameObject btn)
	{
		GlobalInf.chargeShowType = CHARGESHOWTYPE.DIRECT;
		Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
	}

	public void RefreshCash()
	{
		cashLabel.text = string.Empty + GlobalInf.cash;
	}

	public void RefreshGold()
	{
		goldLabel.text = string.Empty + GlobalInf.gold;
	}

	public void CheckCashAdsBtn()
	{
		if (!Platform.isLowAPILevel)
		{
			GlobalInf.cashVideoFlag = true;
			adsEnableBtnSprite.spriteName = "play";
			adsEnableBtnPicSprite.spriteName = "vedio";
			adsDisableBtnPicSprite.spriteName = "vedio";
			if (Platform.CheckAdsCanShow())
			{
				adsEnableBtn.gameObject.SetActiveRecursively(true);
				adsDisableBtn.SetActiveRecursively(false);
			}
			else
			{
				adsDisableBtn.SetActiveRecursively(true);
				adsEnableBtn.gameObject.SetActiveRecursively(false);
			}
		}
		else
		{
			adsDisableBtn.SetActiveRecursively(false);
			adsEnableBtn.gameObject.SetActiveRecursively(false);
		}
	}

	public void OnClickCashBtn(GameObject obj)
	{
		if (MenuSenceController.instance != null)
		{
			CheckCashAdsBtn();
		}
		if (!showFlag)
		{
			showFlag = true;
			cashFlag = true;
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/RechargeUI/CashPage")) as GameObject;
			gameObject.transform.parent = rechargeRoot.transform;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			rechargeCashObj = gameObject.GetComponent<RechargeController>();
			if (GameSenceBackBtnCtl.instance != null)
			{
				GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.BUYCASH);
			}
			if (MenuSenceBackBtnCtl.instance != null)
			{
				MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.BUYCASHPAGE);
			}
		}
		else if (!cashFlag && rechargeGoldObj != null)
		{
			cashFlag = true;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("UI/RechargeUI/CashPage")) as GameObject;
			gameObject2.transform.parent = rechargeRoot.transform;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.localPosition = Vector3.zero;
			rechargeCashObj = gameObject2.GetComponent<RechargeController>();
			UnityEngine.Object.Destroy(rechargeGoldObj.gameObject);
			if (GameSenceBackBtnCtl.instance != null)
			{
				GameSenceBackBtnCtl.instance.PopGameUIState();
				GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.BUYCASH);
			}
			if (MenuSenceBackBtnCtl.instance != null)
			{
				MenuSenceBackBtnCtl.instance.PopMenuUIState();
				MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.BUYCASHPAGE);
			}
		}
	}

	public void OnClickGoldBtn(GameObject obj)
	{
		if (MenuSenceController.instance != null)
		{
			CheckAdsBtn();
		}
		if (!showFlag)
		{
			showFlag = true;
			cashFlag = false;
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/RechargeUI/GoldPage")) as GameObject;
			gameObject.transform.parent = rechargeRoot.transform;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			rechargeGoldObj = gameObject.GetComponent<RechargeController>();
			if (GameSenceBackBtnCtl.instance != null)
			{
				GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.BUYGOLD);
			}
			if (MenuSenceBackBtnCtl.instance != null)
			{
				MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.BUYGOLDPAGE);
			}
		}
		else if (cashFlag)
		{
			cashFlag = false;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("UI/RechargeUI/GoldPage")) as GameObject;
			gameObject2.transform.parent = rechargeRoot.transform;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.localPosition = Vector3.zero;
			rechargeGoldObj = gameObject2.GetComponent<RechargeController>();
			UnityEngine.Object.Destroy(rechargeCashObj.gameObject);
			if (GameSenceBackBtnCtl.instance != null)
			{
				GameSenceBackBtnCtl.instance.PopGameUIState();
				GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.BUYGOLD);
			}
			if (MenuSenceBackBtnCtl.instance != null)
			{
				MenuSenceBackBtnCtl.instance.PopMenuUIState();
				MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.BUYGOLDPAGE);
			}
		}
	}
}
