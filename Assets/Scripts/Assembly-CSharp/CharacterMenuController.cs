using System;
using UnityEngine;

public class CharacterMenuController : MonoBehaviour
{
	public GameObject characterMenuObj;

	public UIEventListener backBtn;

	public UIEventListener characterBtn;

	public UIEventListener achievementBtn;

	public UIEventListener collectingBtn;

	public UIToggle[] bottomLineUIBtn;

	public GameObject bottomLine;

	public GameObject topLine;

	public GameObject characterPageRoot;

	public GameObject defaultPage;

	public MENUUISTATE prePage = MENUUISTATE.CHARACTERPAGE;

	public CharacterPageRoot prePageController;

	public CharacterPageRoot curPageController;

	public CharacterPageRoot characterPageController;

	public CharacterPageRoot achievementPageController;

	public CharacterPageRoot collectingPageController;

	public bool enableBtnFlag;

	public UISprite achievementBtnSignal;

	public UISprite collectingBtnSignal;

	public UIEventListener dragPlayerBtn;

	public GameObject player;

	public GameObject downPlane;

	public float rotateSpeed;

	private bool backEnableFlag;

	private bool tempDisableFlag;

	public void OnDragDragPlayerBtn(GameObject btn, Vector2 delta)
	{
		player.transform.localEulerAngles = new Vector3(0f, player.transform.localEulerAngles.y - delta.x / 2f, 0f);
	}

	public void CheckSignal()
	{
		MenuSenceController.instance.characterBtnSignal = false;
		MenuSenceController.instance.achievementBtnSignal = false;
		MenuSenceController.instance.collectingBtnSignal = false;
		((AchievementPageController)achievementPageController).GetAchievementLevel();
		for (int i = 0; i < ((AchievementPageController)achievementPageController).achievementLevel.Length; i++)
		{
			if (((AchievementPageController)achievementPageController).CheckAchievementState(i))
			{
				MenuSenceController.instance.characterBtnSignal = true;
				MenuSenceController.instance.achievementBtnSignal = true;
				break;
			}
		}
		if ((!GlobalInf.collectHandGunDoneFlag && GlobalInf.collectHandGunNum >= GlobalDefine.COLLECT_HAND_GUN_MAXNUM) || (!GlobalInf.collectMachineGunDoneFlag && GlobalInf.collectMachineGunNum >= GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM) || (!GlobalInf.collectCarDoneFlag && GlobalInf.collectCarNum >= GlobalDefine.COLLECT_CAR_MAXNUM))
		{
			MenuSenceController.instance.characterBtnSignal = true;
			MenuSenceController.instance.collectingBtnSignal = true;
		}
	}

	public void CheckUISignal()
	{
		if (MenuSenceController.instance.achievementBtnSignal)
		{
			achievementBtnSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			achievementBtnSignal.gameObject.SetActiveRecursively(false);
		}
		if (MenuSenceController.instance.collectingBtnSignal)
		{
			collectingBtnSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			collectingBtnSignal.gameObject.SetActiveRecursively(false);
		}
	}

	private void OnEnable()
	{
		enableBtnFlag = true;
		characterMenuObj.active = true;
		backBtn.gameObject.SetActiveRecursively(true);
		bottomLine.gameObject.SetActiveRecursively(true);
		topLine.gameObject.SetActiveRecursively(true);
		defaultPage.gameObject.SetActiveRecursively(true);
		characterPageRoot.SetActive(true);
		player.transform.localEulerAngles = new Vector3(0f, 170f, 0f);
		if (!GlobalDefine.smallPhoneFlag)
		{
			player.transform.localPosition = new Vector3(-270.9133f, -240f, 436.5507f);
			downPlane.transform.localPosition = new Vector3(-270.9449f, -236f, 436.5547f);
		}
		else
		{
			player.transform.localPosition = new Vector3(-215.5829f, -240f, 436.5507f);
			downPlane.transform.localPosition = new Vector3(-215.6534f, -236f, 436.5547f);
		}
		backEnableFlag = false;
		switch (prePage)
		{
		case MENUUISTATE.CHARACTERPAGE:
			OnClickCharacterBtn(null);
			break;
		case MENUUISTATE.ACHIEVEMENTPAGE:
			OnClickAchievementBtn(null);
			break;
		case MENUUISTATE.COLLECTINGPAGE:
			OnClickCollectingBtn(null);
			break;
		default:
			OnClickCharacterBtn(null);
			break;
		}
		characterMenuObj.GetComponent<Animation>().Play("CharacterUIEnter");
		Invoke("EnableBack", 1.1f);
		CheckUISignal();
	}

	private void EnableBack()
	{
		backEnableFlag = true;
	}

	private void OnDisable()
	{
		if (characterMenuObj != null)
		{
			characterMenuObj.gameObject.SetActiveRecursively(false);
		}
		if (backBtn != null)
		{
			backBtn.gameObject.SetActiveRecursively(false);
		}
		if (bottomLine != null)
		{
			bottomLine.gameObject.SetActiveRecursively(false);
		}
		if (topLine != null)
		{
			topLine.gameObject.SetActiveRecursively(false);
		}
	}

	public void Init()
	{
		UIEventListener uIEventListener = backBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickBackBtn));
		UIEventListener uIEventListener2 = characterBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickCharacterBtn));
		UIEventListener uIEventListener3 = achievementBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickAchievementBtn));
		UIEventListener uIEventListener4 = collectingBtn;
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickCollectingBtn));
		UIEventListener uIEventListener5 = dragPlayerBtn;
		uIEventListener5.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener5.onDrag, new UIEventListener.VectorDelegate(OnDragDragPlayerBtn));
	}

	private void Start()
	{
		Init();
	}

	public void OnClickBackBtn(GameObject btn)
	{
		if (backEnableFlag && enableBtnFlag)
		{
			backEnableFlag = false;
			enableBtnFlag = false;
			MenuSenceController.instance.OnChangeStartMenu();
			characterMenuObj.GetComponent<Animation>().Play("CharacterUIExit");
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			curPageController = null;
		}
	}

	public void OnClickCharacterBtn(GameObject btn)
	{
		if (enableBtnFlag && (!(curPageController != null) || curPageController.uiType != MENUUISTATE.CHARACTERPAGE))
		{
			prePageController = curPageController;
			curPageController = characterPageController;
			curPageController.gameObject.active = true;
			prePage = MENUUISTATE.CHARACTERPAGE;
			DisablePrePage();
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.CHARACTERPAGE);
		}
	}

	private void DisablePrePage()
	{
		if (prePageController != null)
		{
			prePageController.gameObject.active = false;
		}
	}

	public void OnClickAchievementBtn(GameObject btn)
	{
		if (enableBtnFlag && (!(curPageController != null) || curPageController.uiType != MENUUISTATE.ACHIEVEMENTPAGE))
		{
			prePageController = curPageController;
			curPageController = achievementPageController;
			curPageController.gameObject.active = true;
			prePage = MENUUISTATE.ACHIEVEMENTPAGE;
			DisablePrePage();
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.ACHIEVEMENTPAGE);
		}
	}

	public void OnClickCollectingBtn(GameObject btn)
	{
		if (enableBtnFlag && (!(curPageController != null) || curPageController.uiType != MENUUISTATE.COLLECTINGPAGE))
		{
			prePageController = curPageController;
			curPageController = collectingPageController;
			curPageController.gameObject.active = true;
			prePage = MENUUISTATE.COLLECTINGPAGE;
			DisablePrePage();
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.COLLECTINGPAGE);
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
		achievementPageController.gameObject.active = false;
		characterPageController.gameObject.active = false;
		collectingPageController.gameObject.active = false;
	}
}
