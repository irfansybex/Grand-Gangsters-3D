using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuSenceTutorialController : MonoBehaviour
{
	public GameObject blackBack;

	public UIPanel rootPannel;

	public UIPanel hightLightRootPanel;

	public List<UIPanel> prePannel;

	public List<Transform> preTransfrom;

	public List<UIWidget> highLightObjList;

	public MENUTUTORIALSTATE curState;

	public MENUTUTORIALSTATE nextState;

	public UILabel disWorld;

	public UIEventListener preBtn;

	public GameObject fingerRootObj;

	public UILabel playBtnLabel;

	public UISprite playBtnPic;

	private void Start()
	{
		if (((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).itemHandGunList[0].gunNum != 1)
		{
			curState = MENUTUTORIALSTATE.PRE_UPGRADE;
			nextState = MENUTUTORIALSTATE.UPGRADE;
		}
		else
		{
			curState = MENUTUTORIALSTATE.PRE_BUY;
			nextState = MENUTUTORIALSTATE.BUY;
		}
		blackBack.SetActiveRecursively(true);
		rootPannel.gameObject.SetActiveRecursively(true);
		disWorld.gameObject.SetActiveRecursively(false);
		fingerRootObj.gameObject.SetActiveRecursively(false);
	}

	public void OnEnterUpgradeState()
	{
		curState = MENUTUTORIALSTATE.UPGRADE;
		nextState = MENUTUTORIALSTATE.PRE_BACKTOMAIN;
		((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).cashBuyBtn.gameObject.SetActiveRecursively(false);
		disWorld.gameObject.SetActiveRecursively(true);
		OnAddHighLightObj(((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).gunLevel);
		OnAddHighLightObj(((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).upgradeLineRoot);
		OnAddHighLightObj(((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).upgradeBtn.gameObject.GetComponent<UISprite>());
		OnEnableHightLightObj();
		preBtn = ((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).upgradeBtn.gameObject.GetComponent<UIEventListener>();
		UIEventListener uIEventListener = preBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnChangeState));
		fingerRootObj.SetActiveRecursively(true);
		fingerRootObj.transform.position = ((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).upgradeBtn.transform.position;
		disWorld.text = "You can upgrade it when it's enough.";
	}

	public void OnEnterBuyState()
	{
		curState = MENUTUTORIALSTATE.BUY;
		nextState = MENUTUTORIALSTATE.PRE_UPGRADE;
		disWorld.gameObject.SetActiveRecursively(true);
		OnAddHighLightObj(((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).gunLevel);
		OnAddHighLightObj(((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).upgradeLineRoot);
		OnAddHighLightObj(((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).cashBuyBtn.gameObject.GetComponent<UISprite>());
		OnEnableHightLightObj();
		preBtn = ((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).cashBuyBtn.gameObject.GetComponent<UIEventListener>();
		UIEventListener uIEventListener = preBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnChangeState));
		fingerRootObj.SetActiveRecursively(true);
		fingerRootObj.transform.position = ((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).cashBuyBtn.transform.position;
		((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).buyBtnSignal.gameObject.SetActiveRecursively(false);
		disWorld.text = "Guns and cars could be accumulated.";
	}

	private void Update()
	{
		CheckState();
	}

	public void CheckState()
	{
		switch (curState)
		{
		case MENUTUTORIALSTATE.PRE_BUY:
			CheckPreBuyState();
			break;
		case MENUTUTORIALSTATE.PRE_UPGRADE:
			CheckPreUpgradeState();
			break;
		case MENUTUTORIALSTATE.PRE_BACKTOMAIN:
			OnEnterBackToMainState();
			break;
		case MENUTUTORIALSTATE.PRE_BACKTOGAME:
			PreBackToGameStateCheck();
			break;
		case MENUTUTORIALSTATE.UPGRADE:
		case MENUTUTORIALSTATE.BACKTOMAIN:
		case MENUTUTORIALSTATE.BACKTOGAME:
		case MENUTUTORIALSTATE.DONE:
			break;
		}
	}

	public void PreBackToGameStateCheck()
	{
		if (!MenuSenceController.instance.startMenu.startMenuObj.GetComponent<Animation>().isPlaying)
		{
			OnEnterBackToGameState();
		}
	}

	public void OnEnterBackToGameState()
	{
		curState = MENUTUTORIALSTATE.BACKTOGAME;
		nextState = MENUTUTORIALSTATE.DONE;
		OnAddHighLightObj(MenuSenceController.instance.startMenu.playBtn.gameObject.GetComponent<UISprite>());
		OnAddHighLightObj(playBtnPic);
		OnAddHighLightObj(playBtnLabel);
		OnEnableHightLightObj();
		preBtn = MenuSenceController.instance.startMenu.playBtn;
		UIEventListener uIEventListener = preBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickDoneBtn));
		fingerRootObj.SetActiveRecursively(true);
		fingerRootObj.transform.position = MenuSenceController.instance.startMenu.playBtn.transform.position;
		disWorld.text = "Press START and get back to the city.";
	}

	public void OnClickDoneBtn(GameObject btn)
	{
		UIEventListener uIEventListener = preBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickDoneBtn));
		curState = MENUTUTORIALSTATE.DONE;
		GlobalInf.upgradeTutorialFlag = false;
	}

	public void OnEnterBackToMainState()
	{
		curState = MENUTUTORIALSTATE.BACKTOMAIN;
		nextState = MENUTUTORIALSTATE.PRE_BACKTOGAME;
		((HandGunPageController)MenuSenceController.instance.storeMenu.handGunPageController).upgradeBtn.gameObject.SetActiveRecursively(false);
		OnAddHighLightObj(MenuSenceController.instance.storeMenu.backBtn.gameObject.GetComponent<UISprite>());
		OnEnableHightLightObj();
		preBtn = MenuSenceController.instance.storeMenu.backBtn;
		UIEventListener uIEventListener = preBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnChangeState));
		disWorld.text = "Upgrade successful!\nNow let's get back.";
		fingerRootObj.SetActiveRecursively(true);
		fingerRootObj.transform.position = MenuSenceController.instance.storeMenu.backBtn.transform.position;
		PlayerPrefs.SetInt("UpgradeTutorialFlag", 1);
	}

	public void CheckPreUpgradeState()
	{
		if (!MenuSenceController.instance.storeMenu.storeMenuObj.GetComponent<Animation>().isPlaying)
		{
			OnEnterUpgradeState();
		}
	}

	public void CheckPreBuyState()
	{
		if (!MenuSenceController.instance.storeMenu.storeMenuObj.GetComponent<Animation>().isPlaying)
		{
			OnEnterBuyState();
		}
	}

	public void PreStateCheck()
	{
		if (!MenuSenceController.instance.startMenu.startMenuObj.GetComponent<Animation>().isPlaying)
		{
			OnEnterStartPageState();
		}
	}

	public void PreStartPageToHandGunPageCheck()
	{
		if (!MenuSenceController.instance.storeMenu.storeMenuObj.GetComponent<Animation>().isPlaying)
		{
			OnEnterStartPageToHandGunPageState();
		}
	}

	public void OnChangeState(GameObject obj)
	{
		fingerRootObj.gameObject.SetActiveRecursively(false);
		OnDisableHighLightObj();
		curState = nextState;
		UIEventListener uIEventListener = preBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnChangeState));
		preBtn = null;
	}

	public void OnEnterStartPageState()
	{
		OnAddHighLightObj(MenuSenceController.instance.startMenu.storeBtn.gameObject.GetComponent<UISprite>());
		OnEnableHightLightObj();
		UIEventListener component = MenuSenceController.instance.startMenu.storeBtn.gameObject.GetComponent<UIEventListener>();
		component.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(component.onClick, new UIEventListener.VoidDelegate(OnChangeState));
	}

	public void OnEnterStartPageToHandGunPageState()
	{
		OnAddHighLightObj(MenuSenceController.instance.storeMenu.bottomLineUIBtn[1].gameObject.GetComponent<UISprite>());
		OnEnableHightLightObj();
		UIEventListener component = MenuSenceController.instance.storeMenu.bottomLineUIBtn[1].gameObject.GetComponent<UIEventListener>();
		component.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(component.onClick, new UIEventListener.VoidDelegate(OnChangeState));
	}

	public void OnDisableHighLightObj()
	{
		for (int i = 0; i < highLightObjList.Count; i++)
		{
			highLightObjList[i].panel = prePannel[i];
			highLightObjList[i].transform.parent = preTransfrom[i];
			highLightObjList[i].gameObject.SetActiveRecursively(false);
			highLightObjList[i].gameObject.SetActiveRecursively(true);
		}
		hightLightRootPanel.widgets.Clear();
		highLightObjList.Clear();
		prePannel.Clear();
		preTransfrom.Clear();
	}

	public void OnEnableHightLightObj()
	{
		for (int i = 0; i < highLightObjList.Count; i++)
		{
			highLightObjList[i].transform.parent = hightLightRootPanel.transform;
			highLightObjList[i].panel = hightLightRootPanel;
			highLightObjList[i].gameObject.SetActiveRecursively(false);
			highLightObjList[i].gameObject.SetActiveRecursively(true);
		}
	}

	public void OnAddHighLightObj(UIWidget obj)
	{
		prePannel.Add(obj.panel);
		preTransfrom.Add(obj.transform.parent);
		highLightObjList.Add(obj);
	}
}
