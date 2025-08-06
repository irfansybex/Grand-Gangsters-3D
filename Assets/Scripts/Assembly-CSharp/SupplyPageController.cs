using System;
using UnityEngine;

public class SupplyPageController : WeaponPageController
{
	public int healthKitMoney;

	public int toolKitMoney;

	public int packKitMoney;

	public UIEventListener healthKitBtn;

	public UIEventListener toolKitBtn;

	public UIEventListener packKitBtn;

	public UILabel healthKitNumLabel;

	public UILabel toolKitNumLabel;

	public TweenColor healthKitBackLight;

	public TweenColor toolKitBackLight;

	public TweenColor packKitBackLight;

	private void Start()
	{
		UIEventListener uIEventListener = healthKitBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickHealthKitBtn));
		UIEventListener uIEventListener2 = toolKitBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickToolKitBtn));
		UIEventListener uIEventListener3 = packKitBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickPackKitBtn));
	}

	private void OnEnable()
	{
		weaponPageObj.gameObject.SetActiveRecursively(true);
		if (playEnterAnimaFlag)
		{
			weaponPageObj.GetComponent<Animation>().Play("StorePageEnter");
		}
		healthKitNumLabel.text = "Owned : " + GlobalInf.healthKitNum;
		toolKitNumLabel.text = "Owned : " + GlobalInf.toolKitNum;
	}

	public void OnClickHealthKitBtn(GameObject btn)
	{
		if (GlobalInf.gold - healthKitMoney >= 0)
		{
			GlobalInf.gold -= healthKitMoney;
			StoreDateController.SetGold();
			GlobalInf.healthKitNum += 3;
			StoreDateController.SetHealthKitNum(GlobalInf.healthKitNum);
			healthKitNumLabel.text = "Owned : " + GlobalInf.healthKitNum;
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.GET_ITEM);
			}
			healthKitBackLight.ResetToBeginning();
			healthKitBackLight.PlayForward();
			Platform.flurryEvent_onEquipmentKitPurchase(0);
		}
		else
		{
			TopLineController.instance.OnClickGoldBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.MENUKIT;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void OnClickToolKitBtn(GameObject btn)
	{
		if (GlobalInf.gold - toolKitMoney >= 0)
		{
			GlobalInf.gold -= toolKitMoney;
			StoreDateController.SetGold();
			GlobalInf.toolKitNum += 3;
			StoreDateController.SetToolKitNum(GlobalInf.toolKitNum);
			toolKitNumLabel.text = "Owned : " + GlobalInf.toolKitNum;
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.GET_ITEM);
			}
			toolKitBackLight.ResetToBeginning();
			toolKitBackLight.PlayForward();
			Platform.flurryEvent_onEquipmentKitPurchase(1);
		}
		else
		{
			TopLineController.instance.OnClickGoldBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.MENUKIT;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void OnClickPackKitBtn(GameObject btn)
	{
		if (GlobalInf.gold - packKitMoney >= 0)
		{
			GlobalInf.gold -= packKitMoney;
			StoreDateController.SetGold();
			GlobalInf.toolKitNum += 3;
			StoreDateController.SetToolKitNum(GlobalInf.toolKitNum);
			toolKitNumLabel.text = "Owned : " + GlobalInf.toolKitNum;
			GlobalInf.healthKitNum += 3;
			StoreDateController.SetHealthKitNum(GlobalInf.healthKitNum);
			healthKitNumLabel.text = "Owned : " + GlobalInf.healthKitNum;
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.GET_ITEM);
			}
			packKitBackLight.ResetToBeginning();
			packKitBackLight.PlayForward();
			Platform.flurryEvent_onEquipmentKitPurchase(2);
		}
		else
		{
			TopLineController.instance.OnClickGoldBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.MENUKIT;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}
}
