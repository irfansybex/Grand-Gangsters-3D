using System;
using UnityEngine;

public class PackPageController : WeaponPageController
{
	public bool saleFalg;

	public PackPage[] page;

	public TweenColor[] lightLabel;

	private float counttt = 1f;

	private string countTime;

	private void Awake()
	{
		Init();
		UIEventListener buyBtn = page[0].buyBtn;
		buyBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(buyBtn.onClick, new UIEventListener.VoidDelegate(OnClickPack0));
		UIEventListener buyBtn2 = page[1].buyBtn;
		buyBtn2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(buyBtn2.onClick, new UIEventListener.VoidDelegate(OnClickPack1));
		UIEventListener buyBtn3 = page[2].buyBtn;
		buyBtn3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(buyBtn3.onClick, new UIEventListener.VoidDelegate(OnCLickPack2));
	}

	public override void Init()
	{
		base.Init();
		myPackPageScrollView.page.Clear();
		saleFalg = Platform.limitedTimeOfferFlag;
		for (int i = 0; i < page.Length; i++)
		{
			myPackPageScrollView.page.Add(page[i]);
		}
		myPackPageScrollView.Init();
	}

	private void OnEnable()
	{
		weaponPageObj.gameObject.SetActiveRecursively(true);
		if (saleFalg)
		{
			for (int i = 0; i < page.Length; i++)
			{
				page[i].salePic.gameObject.SetActiveRecursively(true);
				page[i].normalPic.gameObject.SetActiveRecursively(false);
				page[i].prise = page[i].salePrise;
				page[i].priseLabel.text = string.Empty + page[i].prise;
				page[i].saleLimiteTime.gameObject.SetActiveRecursively(true);
			}
		}
		else
		{
			for (int j = 0; j < page.Length; j++)
			{
				page[j].salePic.gameObject.SetActiveRecursively(false);
				page[j].normalPic.gameObject.SetActiveRecursively(true);
				page[j].prise = page[j].normalPrise;
				page[j].priseLabel.text = string.Empty + page[j].prise;
				page[j].saleLimiteTime.gameObject.SetActiveRecursively(false);
			}
		}
		if (playEnterAnimaFlag)
		{
			weaponPageObj.GetComponent<Animation>().Play("StorePageEnter");
		}
		if (myPackPageScrollView.curIndex == 0)
		{
			leftBtn.gameObject.SetActiveRecursively(false);
			rightBtnAnima.PlayForward();
		}
		else
		{
			rightBtnAnima.enabled = false;
			rightBtnSprite.alpha = 1f;
		}
		if (myPackPageScrollView.curIndex == 2)
		{
			rightBtn.gameObject.SetActiveRecursively(false);
			leftBtnAnima.PlayForward();
		}
		else
		{
			leftBtnAnima.enabled = false;
			leftBtnSprite.alpha = 1f;
		}
	}

	public void OnClickPack1(GameObject btn)
	{
		BuyPack(page[1], 1);
	}

	public void OnCLickPack2(GameObject btn)
	{
		BuyPack(page[2], 2);
	}

	public void OnClickPack0(GameObject btn)
	{
		BuyPack(page[0], 0);
	}

	public void BuyPack(PackPage p, int index)
	{
		if (p.isGoldFlag)
		{
			if (GlobalInf.gold < p.prise)
			{
				TopLineController.instance.OnClickGoldBtn(null);
				if (saleFalg)
				{
					GlobalInf.chargeShowType = CHARGESHOWTYPE.PACKLTO;
					Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
				}
				else
				{
					GlobalInf.chargeShowType = CHARGESHOWTYPE.PACKREGULAR;
					Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
				}
				return;
			}
			GlobalInf.gold -= p.prise;
			StoreDateController.SetGold();
		}
		else
		{
			if (GlobalInf.cash < p.prise)
			{
				TopLineController.instance.OnClickCashBtn(null);
				if (saleFalg)
				{
					GlobalInf.chargeShowType = CHARGESHOWTYPE.PACKLTO;
					Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
				}
				else
				{
					GlobalInf.chargeShowType = CHARGESHOWTYPE.PACKREGULAR;
					Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
				}
				return;
			}
			GlobalInf.cash -= p.prise;
			StoreDateController.SetCash();
			GlobalInf.totalCashSpent += p.prise;
			StoreDateController.SetTotalCashSpent();
		}
		if (saleFalg)
		{
			Platform.flurryEvent_onPackBuyRegular(index);
		}
		else
		{
			Platform.flurryEvent_onPackBuyLTO(index);
		}
		Platform.flurryEvent_onLTOLevel(GlobalInf.gameLevel);
		for (int i = 0; i < p.type.Length; i++)
		{
			switch (p.type[i])
			{
			case ITEMTYPE.CAR:
			{
				int healthKitNum = StoreDateController.GetCarNum(p.itemIndex[i]) + p.itemNum[i];
				StoreDateController.SetCarNum(p.itemIndex[i], healthKitNum);
				if (!((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).itemCarList[p.itemIndex[i]].unlockFlag)
				{
					((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).itemCarList[p.itemIndex[i]].unlockFlag = true;
				}
				break;
			}
			case ITEMTYPE.HANDGUN:
			{
				int healthKitNum = StoreDateController.GetHandGunNum(p.itemIndex[i]) + p.itemNum[i];
				StoreDateController.SetHandGunNum(p.itemIndex[i], healthKitNum);
				if (!((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).itemHandGunList[p.itemIndex[i]].unlockFlag)
				{
					((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).itemHandGunList[p.itemIndex[i]].unlockFlag = true;
				}
				StoreDateController.SetHandGunBulletNum(p.itemIndex[i], 200);
				break;
			}
			case ITEMTYPE.MACHINEGUN:
			{
				int healthKitNum = StoreDateController.GetMachineGunNum(p.itemIndex[i]) + p.itemNum[i];
				StoreDateController.SetMachineGunNum(p.itemIndex[i], healthKitNum);
				if (!((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).itemMachineGunList[p.itemIndex[i]].unlockFlag)
				{
					((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).itemMachineGunList[p.itemIndex[i]].unlockFlag = true;
				}
				StoreDateController.SetMachineGunBulletNum(p.itemIndex[i], 500);
				break;
			}
			case ITEMTYPE.HEALTHKIT:
			{
				int healthKitNum = StoreDateController.GetHealthKitNum() + p.itemNum[i];
				StoreDateController.SetHealthKitNum(healthKitNum);
				GlobalInf.healthKitNum = healthKitNum;
				healthKitNum = StoreDateController.GetToolKitNum() + p.itemNum[i];
				StoreDateController.SetToolKitNum(healthKitNum);
				GlobalInf.toolKitNum = healthKitNum;
				break;
			}
			}
		}
		StoreDateController.GetHandGunInfoList(((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).itemHandGunList);
		StoreDateController.GetMachineGunInfoList(((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).itemMachineGunList);
		StoreDateController.GetCarInfoList(((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).itemCarList);
		if (AudioController.instance != null)
		{
			AudioController.instance.play(AudioType.GET_ITEM);
		}
		for (int j = 0; j < lightLabel.Length; j++)
		{
			lightLabel[j].ResetToBeginning();
			lightLabel[j].PlayForward();
		}
		((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).CheckHandGunSignal();
		((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).CheckMachineGunSignal();
		((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).CheckCarSignal();
		MenuSenceController.instance.storeMenu.CheckSignal();
	}

	private void Update()
	{
		if (!Platform.limitedTimeOfferFlag)
		{
			return;
		}
		counttt += Time.deltaTime;
		if (!(counttt >= 1f))
		{
			return;
		}
		counttt = 0f;
		countTime = Platform.GetRemainTimeFull();
		for (int i = 0; i < page.Length; i++)
		{
			page[i].saleLimiteTime.text = countTime;
		}
		if (!Platform.limitedTimeOfferFlag)
		{
			for (int j = 0; j < page.Length; j++)
			{
				page[j].salePic.gameObject.SetActiveRecursively(false);
				page[j].normalPic.gameObject.SetActiveRecursively(true);
				page[j].prise = page[j].normalPrise;
				page[j].priseLabel.text = string.Empty + page[j].prise;
				page[j].saleLimiteTime.gameObject.SetActiveRecursively(false);
			}
		}
	}
}
