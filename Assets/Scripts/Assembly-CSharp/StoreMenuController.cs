using System;
using UnityEngine;

public class StoreMenuController : MonoBehaviour
{
	public GameObject storeMenuObj;

	public UIEventListener backBtn;

	public UIEventListener machineGunBtn;

	public UIEventListener carBtn;

	public UIEventListener handGunBtn;

	public UIEventListener saleBtn;

	public UIEventListener suppliesBtn;

	public UIToggle[] bottomLineUIBtn;

	public GameObject bottomLine;

	public GameObject shopPageRoot;

	public MENUUISTATE prePage = MENUUISTATE.HANDGUNPAGE;

	public WeaponPageController prePageController;

	public WeaponPageController curPageController;

	public WeaponPageController machineGunPageController;

	public WeaponPageController handGunPageController;

	public WeaponPageController carPageController;

	public WeaponPageController packPageController;

	public WeaponPageController supplyPageController;

	public GameObject bootomLineLight;

	public bool enableBtnFlag;

	public UISprite packBtnSignal;

	public UISprite handGunBtnSignal;

	public UISprite machineGunBtnSignal;

	public UISprite carBtnSignal;

	public UISprite handGunBtnNewSignal;

	public UISprite machineGunBtnNewSignal;

	public UISprite carBtnNewSignal;

	public ParticleSystem gunBackParticle;

	public UIAnchor[] bottomLineAnchor;

	private bool backEnableFlag;

	private bool tempDisableFlag;

	public void CheckSignal()
	{
		if (MenuSenceController.instance.packBtnSignal)
		{
			packBtnSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			packBtnSignal.gameObject.SetActiveRecursively(false);
		}
		if (MenuSenceController.instance.handGunBtnSignal)
		{
			handGunBtnSignal.gameObject.SetActiveRecursively(true);
			handGunBtnNewSignal.gameObject.SetActiveRecursively(false);
		}
		else
		{
			handGunBtnSignal.gameObject.SetActiveRecursively(false);
			if (MenuSenceController.instance.handGunBtnNewSignal)
			{
				handGunBtnNewSignal.gameObject.SetActiveRecursively(true);
			}
			else
			{
				handGunBtnNewSignal.gameObject.SetActiveRecursively(false);
			}
		}
		if (MenuSenceController.instance.machineGunBtnSignal)
		{
			machineGunBtnSignal.gameObject.SetActiveRecursively(true);
			machineGunBtnNewSignal.gameObject.SetActiveRecursively(false);
		}
		else
		{
			machineGunBtnSignal.gameObject.SetActiveRecursively(false);
			if (MenuSenceController.instance.machineGunBtnNewSignal)
			{
				machineGunBtnNewSignal.gameObject.SetActiveRecursively(true);
			}
			else
			{
				machineGunBtnNewSignal.gameObject.SetActiveRecursively(false);
			}
		}
		if (MenuSenceController.instance.carBtnSignal)
		{
			carBtnSignal.gameObject.SetActiveRecursively(true);
			carBtnNewSignal.gameObject.SetActiveRecursively(false);
			return;
		}
		carBtnSignal.gameObject.SetActiveRecursively(false);
		if (MenuSenceController.instance.carBtnNewSignal)
		{
			carBtnNewSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			carBtnNewSignal.gameObject.SetActiveRecursively(false);
		}
	}

	private void OnEnable()
	{
		enableBtnFlag = true;
		storeMenuObj.active = true;
		backBtn.gameObject.SetActiveRecursively(true);
		bottomLine.gameObject.SetActiveRecursively(true);
		shopPageRoot.SetActive(true);
		backEnableFlag = false;
		switch (prePage)
		{
		case MENUUISTATE.MACHINEGUNPAGE:
			OnClickMachineGunBtn(null);
			machineGunBtn.gameObject.GetComponent<UIToggle>().value = true;
			break;
		case MENUUISTATE.HANDGUNPAGE:
			OnClickHandGunBtn(null);
			handGunBtn.gameObject.GetComponent<UIToggle>().value = true;
			break;
		case MENUUISTATE.VECHICLESPAGE:
			OnClickCarBtn(null);
			carBtn.gameObject.GetComponent<UIToggle>().value = true;
			break;
		case MENUUISTATE.PACKSPAGE:
			OnClickSaleBtn(null);
			saleBtn.gameObject.GetComponent<UIToggle>().value = true;
			break;
		case MENUUISTATE.ITEMSPAGE:
			OnClickSuppliesBtn(null);
			suppliesBtn.gameObject.GetComponent<UIToggle>().value = true;
			break;
		default:
			OnClickSaleBtn(null);
			saleBtn.gameObject.GetComponent<UIToggle>().value = true;
			break;
		}
		storeMenuObj.GetComponent<Animation>().Play("StoreMenuEnter");
		Invoke("EnableBack", 1.1f);
		CheckSignal();
	}

	private void EnableBack()
	{
		backEnableFlag = true;
	}

	private void OnDisable()
	{
		if (storeMenuObj != null)
		{
			storeMenuObj.active = false;
		}
		if (backBtn != null)
		{
			backBtn.gameObject.SetActiveRecursively(false);
		}
		if (bottomLine != null)
		{
			bottomLine.gameObject.SetActiveRecursively(false);
		}
		if (bootomLineLight != null)
		{
			bootomLineLight.transform.localPosition = new Vector3(2000f, 0f, 0f);
		}
	}

	public void Init()
	{
		UIEventListener uIEventListener = backBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickBackBtn));
		UIEventListener uIEventListener2 = machineGunBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickMachineGunBtn));
		UIEventListener uIEventListener3 = handGunBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickHandGunBtn));
		UIEventListener uIEventListener4 = saleBtn;
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickSaleBtn));
		UIEventListener uIEventListener5 = suppliesBtn;
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnClickSuppliesBtn));
		UIEventListener uIEventListener6 = carBtn;
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnClickCarBtn));
	}

	private void Start()
	{
		Init();
	}

	private void Update()
	{
	}

	public void OnClickBackBtn(GameObject btn)
	{
		if (backEnableFlag && enableBtnFlag)
		{
			backEnableFlag = false;
			enableBtnFlag = false;
			Invoke("EnableBtn", 1.1f);
			MenuSenceController.instance.OnChangeStartMenu();
			storeMenuObj.GetComponent<Animation>().Play("StoreMenuExit");
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			curPageController = null;
		}
	}

	public void OnClickMachineGunBtn(GameObject btn)
	{
		if (enableBtnFlag && (!(curPageController != null) || curPageController.uiType != MENUUISTATE.MACHINEGUNPAGE))
		{
			enableBtnFlag = false;
			tempDisableFlag = true;
			Invoke("EnableBtn", 1.1f);
			prePageController = curPageController;
			curPageController = machineGunPageController;
			if (btn == null)
			{
				curPageController.playEnterAnimaFlag = false;
			}
			else
			{
				curPageController.playEnterAnimaFlag = true;
			}
			curPageController.gameObject.active = true;
			prePage = MENUUISTATE.MACHINEGUNPAGE;
			if (prePageController != null && prePageController != curPageController)
			{
				prePageController.weaponPageObj.GetComponent<Animation>().Play("StorePageExit");
				Invoke("DisablePrePage", 1f);
			}
			machineGunPageController.weaponPageObj.transform.localPosition = Vector3.zero;
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.MACHINEGUNPAGE);
		}
	}

	private void DisablePrePage()
	{
		prePageController.gameObject.active = false;
	}

	public void OnClickCarBtn(GameObject btn)
	{
		if (enableBtnFlag && (!(curPageController != null) || curPageController.uiType != MENUUISTATE.VECHICLESPAGE))
		{
			enableBtnFlag = false;
			tempDisableFlag = true;
			Invoke("EnableBtn", 1.1f);
			prePageController = curPageController;
			curPageController = carPageController;
			if (btn == null)
			{
				curPageController.playEnterAnimaFlag = false;
			}
			else
			{
				curPageController.playEnterAnimaFlag = true;
			}
			curPageController.gameObject.active = true;
			prePage = MENUUISTATE.VECHICLESPAGE;
			if (prePageController != null && prePageController != curPageController)
			{
				prePageController.weaponPageObj.GetComponent<Animation>().Play("StorePageExit");
				Invoke("DisablePrePage", 1f);
			}
			carPageController.weaponPageObj.transform.localPosition = Vector3.zero;
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.VECHICLESPAGE);
		}
	}

	public void OnClickHandGunBtn(GameObject btn)
	{
		if (enableBtnFlag && (!(curPageController != null) || curPageController.uiType != MENUUISTATE.HANDGUNPAGE))
		{
			enableBtnFlag = false;
			tempDisableFlag = true;
			Invoke("EnableBtn", 1.1f);
			prePageController = curPageController;
			curPageController = handGunPageController;
			if (btn == null)
			{
				curPageController.playEnterAnimaFlag = false;
			}
			else
			{
				curPageController.playEnterAnimaFlag = true;
			}
			curPageController.gameObject.SetActive(true);
			prePage = MENUUISTATE.HANDGUNPAGE;
			if (prePageController != null && prePageController != curPageController)
			{
				prePageController.weaponPageObj.GetComponent<Animation>().Play("StorePageExit");
				Invoke("DisablePrePage", 1f);
			}
			handGunPageController.weaponPageObj.transform.localPosition = Vector3.zero;
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.HANDGUNPAGE);
		}
	}

	public void OnClickSaleBtn(GameObject btn)
	{
		if (enableBtnFlag && (!(curPageController != null) || curPageController.uiType != MENUUISTATE.PACKSPAGE))
		{
			enableBtnFlag = false;
			tempDisableFlag = true;
			Invoke("EnableBtn", 1.1f);
			prePageController = curPageController;
			curPageController = packPageController;
			if (btn == null)
			{
				curPageController.playEnterAnimaFlag = false;
			}
			else
			{
				curPageController.playEnterAnimaFlag = true;
			}
			packPageController.gameObject.active = true;
			prePage = MENUUISTATE.PACKSPAGE;
			if (prePageController != null && prePageController != curPageController)
			{
				prePageController.weaponPageObj.GetComponent<Animation>().Play("StorePageExit");
				Invoke("DisablePrePage", 1f);
			}
			packPageController.weaponPageObj.transform.localPosition = Vector3.zero;
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.PACKSPAGE);
		}
	}

	public void OnClickSuppliesBtn(GameObject btn)
	{
		if (enableBtnFlag && (!(curPageController != null) || curPageController.uiType != MENUUISTATE.ITEMSPAGE))
		{
			enableBtnFlag = false;
			tempDisableFlag = true;
			Invoke("EnableBtn", 1.1f);
			prePageController = curPageController;
			curPageController = supplyPageController;
			if (btn == null)
			{
				curPageController.playEnterAnimaFlag = false;
			}
			else
			{
				curPageController.playEnterAnimaFlag = true;
			}
			supplyPageController.gameObject.active = true;
			prePage = MENUUISTATE.ITEMSPAGE;
			if (prePageController != null && prePageController != curPageController)
			{
				prePageController.weaponPageObj.GetComponent<Animation>().Play("StorePageExit");
				Invoke("DisablePrePage", 1f);
			}
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.ITEMSPAGE);
		}
	}

	private void LateUpdate()
	{
		if (tempDisableFlag)
		{
			tempDisableFlag = false;
			TempDisableBtn();
		}
	}

	private void TempDisableBtn()
	{
		for (int i = 0; i < bottomLineUIBtn.Length; i++)
		{
			bottomLineUIBtn[i].tempDisableFlag = true;
		}
	}

	private void TempEnableBtn()
	{
		for (int i = 0; i < bottomLineUIBtn.Length; i++)
		{
			bottomLineUIBtn[i].tempDisableFlag = false;
		}
	}

	private void EnableBtn()
	{
		enableBtnFlag = true;
		TempEnableBtn();
	}

	public void DisActiveController()
	{
		machineGunPageController.gameObject.active = false;
		handGunPageController.gameObject.active = false;
		carPageController.gameObject.active = false;
		packPageController.gameObject.active = false;
		supplyPageController.gameObject.active = false;
	}
}
