using System;
using UnityEngine;

public class CollectingPageController : CharacterPageRoot
{
	public UISprite handGunValLine;

	public UISprite machineGunValLine;

	public UISprite carValLine;

	public UILabel handGunPercentLabel;

	public UILabel machineGunPercentLabel;

	public UILabel carPercentLabel;

	public UISprite handGunBackLine;

	public UISprite machineGunBackLine;

	public UISprite carBackLine;

	public UIEventListener handGunBtn;

	public UIEventListener machineGunBtn;

	public UIEventListener carBtn;

	public UILabel handGunNumLabel;

	public UILabel machineGunNumLabel;

	public UILabel carNumLabel;

	public float handGunPercent;

	public float machineGunPercent;

	public float carPercent;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Init()
	{
		handGunPercent = (float)GlobalInf.collectHandGunNum / (float)GlobalDefine.COLLECT_HAND_GUN_MAXNUM;
		machineGunPercent = (float)GlobalInf.collectMachineGunNum / (float)GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM;
		carPercent = (float)GlobalInf.collectCarNum / (float)GlobalDefine.COLLECT_CAR_MAXNUM;
		handGunValLine.fillAmount = handGunPercent;
		machineGunValLine.fillAmount = machineGunPercent;
		carValLine.fillAmount = carPercent;
		handGunNumLabel.text = string.Empty + GlobalInf.collectHandGunNum + "/" + GlobalDefine.COLLECT_HAND_GUN_MAXNUM;
		machineGunNumLabel.text = string.Empty + GlobalInf.collectMachineGunNum + "/" + GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM;
		carNumLabel.text = string.Empty + GlobalInf.collectCarNum + "/" + GlobalDefine.COLLECT_CAR_MAXNUM;
		UIEventListener uIEventListener = handGunBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickHandGunBtn));
		UIEventListener uIEventListener2 = machineGunBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickMachineGunBtn));
		UIEventListener uIEventListener3 = carBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickCarBtn));
		handGunBtn.gameObject.SetActiveRecursively(false);
		machineGunBtn.gameObject.SetActiveRecursively(false);
		carBtn.gameObject.SetActiveRecursively(false);
		if (!GlobalInf.collectHandGunDoneFlag)
		{
			if (GlobalInf.collectHandGunNum >= GlobalDefine.COLLECT_HAND_GUN_MAXNUM)
			{
				handGunBtn.gameObject.SetActiveRecursively(true);
				handGunPercentLabel.gameObject.SetActiveRecursively(false);
			}
			else
			{
				handGunPercentLabel.text = string.Empty + (int)(handGunPercent * 100f) + "%";
			}
		}
		else
		{
			handGunPercentLabel.text = "100%";
		}
		if (!GlobalInf.collectMachineGunDoneFlag)
		{
			if (GlobalInf.collectMachineGunNum >= GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM)
			{
				machineGunBtn.gameObject.SetActiveRecursively(true);
				machineGunPercentLabel.gameObject.SetActiveRecursively(false);
			}
			else
			{
				machineGunPercentLabel.text = string.Empty + (int)(machineGunPercent * 100f) + "%";
			}
		}
		else
		{
			machineGunPercentLabel.text = "100%";
		}
		if (!GlobalInf.collectCarDoneFlag)
		{
			if (GlobalInf.collectCarNum >= GlobalDefine.COLLECT_CAR_MAXNUM)
			{
				carBtn.gameObject.SetActiveRecursively(true);
				carPercentLabel.gameObject.SetActiveRecursively(false);
			}
			else
			{
				carPercentLabel.text = string.Empty + (int)(carPercent * 100f) + "%";
			}
		}
		else
		{
			carPercentLabel.text = "100%";
		}
	}

	private new void OnEnable()
	{
		base.OnEnable();
		Init();
	}

	public void OnClickHandGunBtn(GameObject btn)
	{
		if (!GlobalInf.collectHandGunDoneFlag && GlobalInf.collectHandGunNum >= GlobalDefine.COLLECT_HAND_GUN_MAXNUM)
		{
			GlobalInf.collectHandGunDoneFlag = true;
			StoreDateController.SetCollectHandGunDoneFlag(GlobalInf.collectHandGunDoneFlag);
			((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).itemHandGunList[5].unlockFlag = true;
			((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).itemHandGunList[5].gunNum = 1;
			((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).ResetEquip();
			((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).itemHandGunList[5].equipedFlag = true;
			((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).itemHandGunList[5].buyFlag = true;
			((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).CopyHandGunInfo(((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).itemHandGunList[5].gunInfo);
			GlobalInf.handgunIndex = 5;
			StoreDateController.SetHandGunNum(5, 1);
			StoreDateController.SetHandGunIndex(5);
			MenuSenceController.instance.getItemPage.gameObject.SetActiveRecursively(true);
			MenuSenceController.instance.getItemPage.InitPage(ITEMTYPE.HANDGUN);
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.CLAIMPAGE);
		}
		MenuSenceController.instance.getItemPage.gameObject.SetActiveRecursively(true);
		MenuSenceController.instance.getItemPage.InitPage(ITEMTYPE.HANDGUN);
		MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.CLAIMPAGE);
	}

	public void OnClickMachineGunBtn(GameObject btn)
	{
		if (!GlobalInf.collectMachineGunDoneFlag && GlobalInf.collectMachineGunNum >= GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM)
		{
			GlobalInf.collectMachineGunDoneFlag = true;
			StoreDateController.SetCollectMachineGunDoneFlag(GlobalInf.collectMachineGunDoneFlag);
			((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).itemMachineGunList[4].unlockFlag = true;
			((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).itemMachineGunList[4].gunNum = 1;
			((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).ResetEquip();
			((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).itemMachineGunList[4].equipedFlag = true;
			((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).itemMachineGunList[4].buyFlag = true;
			((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).CopyMachineGunInfo(((MachineGunPageController)MenuSenceController.instance.storeMenu.machineGunPageController).itemMachineGunList[4].gunInfo);
			GlobalInf.machineGunIndex = 4;
			StoreDateController.SetMachineGunNum(4, 1);
			StoreDateController.SetMachineGunIndex(4);
			MenuSenceController.instance.getItemPage.gameObject.SetActiveRecursively(true);
			MenuSenceController.instance.getItemPage.InitPage(ITEMTYPE.MACHINEGUN);
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.CLAIMPAGE);
		}
	}

	public void OnClickCarBtn(GameObject btn)
	{
		if (!GlobalInf.collectCarDoneFlag && GlobalInf.collectCarNum >= GlobalDefine.COLLECT_CAR_MAXNUM)
		{
			GlobalInf.collectCarDoneFlag = true;
			StoreDateController.SetCollectCarDoneFlag(GlobalInf.collectCarDoneFlag);
			((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).itemCarList[4].unlockFlag = true;
			((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).itemCarList[4].carNum = 1;
			((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).ResetEquip();
			((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).itemCarList[4].equipedFlag = true;
			((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).itemCarList[4].buyFlag = true;
			((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).CopyCarInfo(((CarPageControllor)MenuSenceController.instance.storeMenu.carPageController).itemCarList[4].carInfo);
			GlobalInf.playerCarIndex = 4;
			StoreDateController.SetCarNum(4, 1);
			StoreDateController.SetCarIndex(4);
			MenuSenceController.instance.getItemPage.gameObject.SetActiveRecursively(true);
			MenuSenceController.instance.getItemPage.InitPage(ITEMTYPE.CAR);
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.CLAIMPAGE);
		}
	}
}
