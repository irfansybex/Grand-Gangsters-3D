using System;
using UnityEngine;

public class BuyHartPageController : MonoBehaviour
{
	public UISprite hartValPic;

	public UILabel hartValLabel;

	public UILabel hartTimeCountLabel;

	public UIEventListener buyOneBtn;

	public UIEventListener buyMaxBtn;

	public UIEventListener backBtn;

	public int buyOneMoney;

	public int buyMaxMoney;

	private int remainHartTime;

	public static BuyHartPageController instance;

	private int min;

	private int sec;

	private string temp;

	private float countT;

	private float preTime;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		UIEventListener uIEventListener = buyOneBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickBuyOneBtn));
		UIEventListener uIEventListener2 = buyMaxBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickBuyMaxBtn));
		UIEventListener uIEventListener3 = backBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickBackBtn));
		preTime = Time.realtimeSinceStartup;
		ResetPage();
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	public void ResetPage()
	{
		Platform.CountHarts();
		hartValPic.fillAmount = (float)GlobalInf.hartCount / (float)(GlobalInf.gameLevel + 3);
		hartValLabel.text = string.Empty + GlobalInf.hartCount + "/" + (GlobalInf.gameLevel + 3);
		if (GlobalInf.hartCount < 3 + GlobalInf.gameLevel)
		{
			remainHartTime = Platform.GetRemainHartCountTime();
			hartTimeCountLabel.text = ChangeSecToMin(remainHartTime);
		}
		else
		{
			hartTimeCountLabel.text = "MAX";
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

	private void Start()
	{
	}

	private void Update()
	{
		countT += Time.realtimeSinceStartup - preTime;
		if (countT >= 1f)
		{
			countT -= 1f;
			ResetPage();
		}
		preTime = Time.realtimeSinceStartup;
	}

	public void OnClickBuyOneBtn(GameObject btn)
	{
		if (GlobalInf.gold - buyOneMoney > 0)
		{
			GlobalInf.gold -= buyOneMoney;
			StoreDateController.SetGold();
			GlobalInf.hartCount++;
			if (GlobalInf.hartCount > GlobalInf.gameLevel + 3)
			{
				GlobalInf.hartCount = GlobalInf.gameLevel + 3;
			}
			StoreDateController.SetHartCount(GlobalInf.hartCount);
			ResetPage();
		}
		else
		{
			TopLineController.instance.OnClickGoldBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.BUYHART;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void OnClickBuyMaxBtn(GameObject btn)
	{
		if (GlobalInf.gold - buyMaxMoney > 0)
		{
			GlobalInf.gold -= buyMaxMoney;
			StoreDateController.SetGold();
			GlobalInf.hartCount = GlobalInf.gameLevel + 3;
			StoreDateController.SetHartCount(GlobalInf.hartCount);
			ResetPage();
		}
		else
		{
			TopLineController.instance.OnClickGoldBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.BUYHART;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void OnClickBackBtn(GameObject btn)
	{
		if (MenuSenceBackBtnCtl.instance != null)
		{
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
		}
		if (GameSenceBackBtnCtl.instance != null)
		{
			GameSenceBackBtnCtl.instance.PopGameUIState();
		}
		UnityEngine.Object.Destroy(base.gameObject);
		if (MenuSenceController.instance != null)
		{
			MenuSenceController.instance.topLineUI.topLineObj.GetComponent<Animation>().Play("topLineExit");
		}
		else
		{
			TopLineController.instance.gameObject.SetActiveRecursively(false);
		}
		Resources.UnloadUnusedAssets();
	}
}
