using System;
using UnityEngine;

public class RechargeController : MonoBehaviour
{
	public bool cashFlag;

	public UIEventListener[] btn;

	public UIEventListener backBtn;

	public int[] cashMoney;

	public int[] goldMoney;

	private void Start()
	{
		UIEventListener uIEventListener = backBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickBackBtn));
		for (int i = 0; i < btn.Length; i++)
		{
			UIEventListener obj = btn[i];
			obj.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(obj.onClick, new UIEventListener.VoidDelegate(OnClickBuyBtn));
		}
		Platform.hideFeatureView();
	}

	private void Destroy()
	{
		if (cashFlag)
		{
			if (TopLineController.instance.rechargeCashObj == this)
			{
				TopLineController.instance.rechargeCashObj = null;
			}
		}
		else if (TopLineController.instance.rechargeGoldObj == this)
		{
			TopLineController.instance.rechargeGoldObj = null;
		}
	}

	private void Update()
	{
	}

	public void OnClickBackBtn(GameObject obj)
	{
		TopLineController.instance.showFlag = false;
		if (cashFlag)
		{
			TopLineController.instance.rechargeCashObj = null;
		}
		else
		{
			TopLineController.instance.rechargeGoldObj = null;
		}
		if (GameSenceBackBtnCtl.instance != null)
		{
			GameSenceBackBtnCtl.instance.PopGameUIState();
		}
		if (MenuSenceBackBtnCtl.instance != null)
		{
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			TopLineController.instance.CheckAdsBtn();
		}
		if (TopLineController.instance.checkPageFlag)
		{
			Platform.showFeatureView(FeatureViewPosType.MIDDLE);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnClickBuyBtn(GameObject obj)
	{
		for (int i = 0; i < btn.Length; i++)
		{
			if (!(obj == btn[i].gameObject))
			{
				continue;
			}
			if (cashFlag)
			{
				if (!Platform.callBilling(i))
				{
					OnBuySuccess(i, !cashFlag);
				}
			}
			else if (!Platform.callBilling(i + 6))
			{
				OnBuySuccess(i + 6, !cashFlag);
			}
		}
		Platform.OnChargeEvent(GlobalInf.chargeShowType, 1);
	}

	public void OnBuySuccess(int id, bool goldFlag)
	{
		if (AudioController.instance != null)
		{
			AudioController.instance.play(AudioType.PICK_CASH);
		}
		if (id > 0 && id != 6)
		{
			Platform.setAdFree();
		}
		if (goldFlag)
		{
			GlobalInf.gold += Platform.COINS_ADD[id];
			StoreDateController.SetGold();
			GlobalInf.totalGoldEarned += Platform.COINS_ADD[id];
			StoreDateController.SetTotalGoldEarned();
		}
		else
		{
			GlobalInf.cash += Platform.COINS_ADD[id];
			StoreDateController.SetCash();
			GlobalInf.totalCashEarned += Platform.COINS_ADD[id];
			StoreDateController.SetTotalCashEarned(GlobalInf.totalCashEarned);
		}
	}
}
