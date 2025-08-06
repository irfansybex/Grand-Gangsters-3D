using System;
using UnityEngine;

public class GameSenceTutorialController : MonoBehaviour
{
	public static GameSenceTutorialController instance;

	public GAMETUTORIALSTATE curState;

	public GAMETUTORIALSTATE nextState;

	public Vector3 playerPos;

	public Vector3 playerCarPos;

	public GameObject UIRoot;

	public GameObject turtialUIRoot;

	public UILabel label;

	public UIEventListener continueBtn;

	public GameObject noMaskUIRoot;

	public UILabel noMaskLabel;

	public UISprite noMaskBack;

	public GameObject moveTargetObj;

	public GameObject playerWall;

	public bool enableTouchFlag;

	public GunsInfo handGunInfo;

	public CarInfo carInfo;

	public UIEventListener handleBtn;

	public UIEventListener gravityBtn;

	public UISprite handleBtnSprite;

	public UISprite gravityBtnSprite;

	public UIEventListener okBtn;

	public TweenPosition handTipTweenPos;

	public TweenAlpha handTipTweenAlph;

	public GameObject pressTips;

	private Vector3 viewPos;

	public bool backFlag;

	public bool leftFlag;

	public bool delayChangFlag;

	private bool doneFlag;

	private RatePageController ratePageController;

	public GameObject driveModeChooseRoot;

	public GameObject handleRoot;

	public GameObject gravityRoot;

	private AIController lastAI;

	private Vector3 aiPos;

	private void Awake()
	{
		StoreDateController.InitDate();
		if (!GlobalInf.firstOpenGameFlag)
		{
			base.gameObject.SetActiveRecursively(false);
			return;
		}
		if (instance == null)
		{
			instance = this;
		}
		moveTargetObj.SetActive(false);
		playerWall.SetActive(true);
		GlobalInf.handgunIndex = -1;
		GlobalInf.machineGunIndex = -1;
		Platform.hasWaitFakeLoadingOver = true;
		Invoke("HideFakeLoading", Platform.lanuchWaitTime);
		Debug.Log("Awake!!!!!!!!!!!!!!!!!!!!!!!!! Platform.lanuchWaitTime :: " + Platform.lanuchWaitTime);
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("AI"), LayerMask.NameToLayer("PlayerWall"), false);
		PlayerPrefs.SetInt("NewUser", 1);
		GlobalInf.newUserFlag = true;
		StoreDateController.GetLastInfo();
	}

	public void HideFakeLoading()
	{
		Debug.Log("HideFakeLoading!!!!!!!!!!!!!!!!!!!!!!!!!!");
		Platform.disableFakeLoading();
		Platform.showFullScreenSmall();
	}

	private void Start()
	{
		PlayerController.instance.transform.position = playerPos;
		UIEventListener uIEventListener = continueBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickContinueBtn));
		UIEventListener joyStickAreaBtn = PlayerController.instance.moveCtl.joyStick.joyStickAreaBtn;
		joyStickAreaBtn.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(joyStickAreaBtn.onPress, new UIEventListener.BoolDelegate(OnPressJosyStickBtn));
		UIEventListener uIEventListener2 = handleBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickHandleBtn));
		UIEventListener uIEventListener3 = gravityBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickGravityBtn));
		UIEventListener uIEventListener4 = okBtn;
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickContinueBtn));
	}

	public void OnClickHandleBtn(GameObject obj)
	{
		GlobalInf.carCtrlType = CARCTRLTYPE.BUTTON;
		handleRoot.SetActive(true);
		gravityRoot.SetActive(false);
		StoreDateController.SetControlType(false);
	}

	public void OnClickGravityBtn(GameObject obj)
	{
		GlobalInf.carCtrlType = CARCTRLTYPE.GRAVITY;
		handleRoot.SetActive(false);
		gravityRoot.SetActive(true);
		StoreDateController.SetControlType(true);
	}

	private void Update()
	{
		PoliceLevelCtl.ResetPoliceLevel();
		CheckState();
		if (curState == GAMETUTORIALSTATE.CHOOSETARGET || curState == GAMETUTORIALSTATE.SHOOT_CLICK || curState == GAMETUTORIALSTATE.MOVE_ARRIVE || curState == GAMETUTORIALSTATE.GETONCAR)
		{
			CheckTargetPos();
		}
		if (curState == GAMETUTORIALSTATE.CHOOSETARGET || curState == GAMETUTORIALSTATE.SHOOT_CLICK)
		{
			if (GameUIController.instance.locateLabelList[0].transform.localPosition.x <= 50f)
			{
				if (pressTips.active)
				{
					pressTips.SetActive(false);
				}
			}
			else if (GameUIController.instance.locateLabelList[0].transform.localPosition.x >= GlobalDefine.screenRatioWidth - 50f)
			{
				if (pressTips.active)
				{
					pressTips.SetActive(false);
				}
			}
			else
			{
				if (!pressTips.active)
				{
					pressTips.SetActive(true);
				}
				viewPos = Camera.main.WorldToViewportPoint(lastAI.transform.position + Vector3.up);
				pressTips.transform.localPosition = new Vector3((viewPos.x - 0.5f) * GlobalDefine.screenRatioWidth, viewPos.y * 480f - 240f, 0f);
			}
		}
		if (curState != GAMETUTORIALSTATE.GETONCARBTN)
		{
			return;
		}
		if (GameUIController.instance.ctl_GetOnCarBtn.gameObject.active)
		{
			if (!pressTips.gameObject.active)
			{
				pressTips.gameObject.SetActive(true);
			}
		}
		else if (pressTips.gameObject.active)
		{
			pressTips.gameObject.SetActive(false);
		}
	}

	public void CheckTargetPos()
	{
		if (GameUIController.instance.locateLabelList[0].transform.localPosition.x <= 50f)
		{
			if (!handTipTweenPos.gameObject.active)
			{
				handTipTweenPos.gameObject.SetActive(true);
				handTipTweenPos.from = new Vector3(-50f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.to = new Vector3(-200f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.ResetToBeginning();
				handTipTweenAlph.ResetToBeginning();
				handTipTweenPos.PlayForward();
				handTipTweenAlph.PlayForward();
			}
			if (!leftFlag)
			{
				leftFlag = true;
				handTipTweenPos.gameObject.SetActive(true);
				handTipTweenPos.from = new Vector3(-50f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.to = new Vector3(-200f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.ResetToBeginning();
				handTipTweenAlph.ResetToBeginning();
				handTipTweenPos.PlayForward();
				handTipTweenAlph.PlayForward();
			}
		}
		else if (GameUIController.instance.locateLabelList[0].transform.localPosition.x >= GlobalDefine.screenRatioWidth - 50f)
		{
			if (!handTipTweenPos.gameObject.active)
			{
				handTipTweenPos.gameObject.SetActive(true);
				handTipTweenPos.from = new Vector3(50f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.to = new Vector3(200f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.ResetToBeginning();
				handTipTweenAlph.ResetToBeginning();
				handTipTweenPos.PlayForward();
				handTipTweenAlph.PlayForward();
			}
			if (leftFlag)
			{
				leftFlag = false;
				handTipTweenPos.gameObject.SetActive(true);
				handTipTweenPos.from = new Vector3(50f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.to = new Vector3(200f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.ResetToBeginning();
				handTipTweenAlph.ResetToBeginning();
				handTipTweenPos.PlayForward();
				handTipTweenAlph.PlayForward();
			}
		}
		else if (handTipTweenPos.gameObject.active)
		{
			handTipTweenPos.gameObject.SetActive(false);
		}
	}

	public void CheckParkLabelPos()
	{
		if (GameUIController.instance.playerCarParkLabel.transform.localPosition.x <= 50f)
		{
			if (!handTipTweenPos.gameObject.active)
			{
				handTipTweenPos.gameObject.SetActive(true);
				handTipTweenPos.from = new Vector3(-50f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.to = new Vector3(-200f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.ResetToBeginning();
				handTipTweenAlph.ResetToBeginning();
				handTipTweenPos.PlayForward();
				handTipTweenAlph.PlayForward();
			}
			if (!leftFlag)
			{
				leftFlag = true;
				handTipTweenPos.gameObject.SetActive(true);
				handTipTweenPos.from = new Vector3(-50f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.to = new Vector3(-200f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.ResetToBeginning();
				handTipTweenAlph.ResetToBeginning();
				handTipTweenPos.PlayForward();
				handTipTweenAlph.PlayForward();
			}
		}
		else if (GameUIController.instance.playerCarParkLabel.transform.localPosition.x >= GlobalDefine.screenRatioWidth - 50f)
		{
			if (!handTipTweenPos.gameObject.active)
			{
				handTipTweenPos.gameObject.SetActive(true);
				handTipTweenPos.from = new Vector3(50f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.to = new Vector3(200f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.ResetToBeginning();
				handTipTweenAlph.ResetToBeginning();
				handTipTweenPos.PlayForward();
				handTipTweenAlph.PlayForward();
			}
			if (leftFlag)
			{
				leftFlag = false;
				handTipTweenPos.gameObject.SetActive(true);
				handTipTweenPos.from = new Vector3(50f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.to = new Vector3(200f * GlobalDefine.screenWidthFit, -50f, 0f);
				handTipTweenPos.ResetToBeginning();
				handTipTweenAlph.ResetToBeginning();
				handTipTweenPos.PlayForward();
				handTipTweenAlph.PlayForward();
			}
		}
		else if (handTipTweenPos.gameObject.active)
		{
			handTipTweenPos.gameObject.SetActive(false);
		}
	}

	public void OnPressNoMaskBtn(GameObject obj, bool isPress)
	{
		if (isPress && !delayChangFlag)
		{
			UIEventListener rotateBtn = GameUIController.instance.rotateBtn;
			rotateBtn.onPress = (UIEventListener.BoolDelegate)Delegate.Remove(rotateBtn.onPress, new UIEventListener.BoolDelegate(OnPressNoMaskBtn));
			nextState = GAMETUTORIALSTATE.PRE_VIEW;
			Invoke("DelayDisableNoMaskObj", 1f);
			Invoke("DelayChangeState", 2f);
			delayChangFlag = true;
		}
	}

	public void OnPressJosyStickBtn(GameObject obj, bool isPress)
	{
		if (isPress && !delayChangFlag)
		{
			UIEventListener joyStickAreaBtn = PlayerController.instance.moveCtl.joyStick.joyStickAreaBtn;
			joyStickAreaBtn.onPress = (UIEventListener.BoolDelegate)Delegate.Remove(joyStickAreaBtn.onPress, new UIEventListener.BoolDelegate(OnPressJosyStickBtn));
			nextState = GAMETUTORIALSTATE.PRE_MOVE_ARRIVE;
			Invoke("DelayDisableNoMaskObj", 1f);
			Invoke("DelayChangeState", 2f);
			delayChangFlag = true;
		}
	}

	public void DelayDisableNoMaskObj()
	{
		noMaskUIRoot.gameObject.SetActive(false);
	}

	public void OnClickContinueBtn(GameObject obj)
	{
		if (!delayChangFlag)
		{
			switch (curState)
			{
			case GAMETUTORIALSTATE.WELCOME:
			{
				turtialUIRoot.SetActive(false);
				nextState = GAMETUTORIALSTATE.PRE_MOVE;
				Invoke("DelayChangeState", 1f);
				delayChangFlag = true;
				UIEventListener rotateBtn = GameUIController.instance.rotateBtn;
				rotateBtn.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(rotateBtn.onPress, new UIEventListener.BoolDelegate(OnPressNoMaskBtn));
				handTipTweenPos.gameObject.SetActive(false);
				break;
			}
			case GAMETUTORIALSTATE.MOVE_ARRIVE:
				noMaskLabel.gameObject.SetActive(false);
				noMaskBack.gameObject.SetActive(false);
				nextState = GAMETUTORIALSTATE.PRE_CHOOSETARGET;
				Invoke("DelayChangeState", 1f);
				GameUIController.instance.DisableLocateLabel();
				delayChangFlag = true;
				break;
			case GAMETUTORIALSTATE.PUNCH:
				noMaskLabel.gameObject.SetActive(false);
				noMaskBack.gameObject.SetActive(false);
				nextState = GAMETUTORIALSTATE.PRE_PICKUP;
				Invoke("DelayChangeState", 1f);
				aiPos = lastAI.transform.position;
				delayChangFlag = true;
				break;
			case GAMETUTORIALSTATE.CHANGE_WEAPON:
			{
				noMaskLabel.gameObject.SetActive(false);
				noMaskBack.gameObject.SetActive(false);
				nextState = GAMETUTORIALSTATE.PRE_SHOOT_CLICK;
				Invoke("DelayChangeState", 1f);
				delayChangFlag = true;
				UIEventListener ctl_LeftChangeBtn = GameUIController.instance.ctl_LeftChangeBtn;
				ctl_LeftChangeBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(ctl_LeftChangeBtn.onClick, new UIEventListener.VoidDelegate(OnClickContinueBtn));
				UIEventListener ctl_RightChangeBtn = GameUIController.instance.ctl_RightChangeBtn;
				ctl_RightChangeBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(ctl_RightChangeBtn.onClick, new UIEventListener.VoidDelegate(OnClickContinueBtn));
				GameUIController.instance.lockWeaponFlag = true;
				pressTips.gameObject.SetActive(false);
				break;
			}
			case GAMETUTORIALSTATE.SHOOT:
				noMaskLabel.gameObject.SetActive(false);
				noMaskBack.gameObject.SetActive(false);
				nextState = GAMETUTORIALSTATE.PRE_GETONCAR;
				Invoke("DelayChangeState", 1f);
				delayChangFlag = true;
				GameUIController.instance.lockWeaponFlag = false;
				break;
			case GAMETUTORIALSTATE.GETONCARBTN:
				curState = GAMETUTORIALSTATE.PRE_DRIVEMODE;
				noMaskLabel.gameObject.SetActive(false);
				noMaskBack.gameObject.SetActive(false);
				pressTips.gameObject.SetActive(false);
				break;
			case GAMETUTORIALSTATE.DRIVEMODE:
				Time.timeScale = 1f;
				driveModeChooseRoot.SetActive(false);
				curState = GAMETUTORIALSTATE.PRE_DRIVE;
				break;
			}
		}
	}

	public void CheckState()
	{
		switch (curState)
		{
		case GAMETUTORIALSTATE.PRE_WELCOME:
			OnEnterWelcome();
			break;
		case GAMETUTORIALSTATE.PRE_MOVE:
			OnEnterMove();
			break;
		case GAMETUTORIALSTATE.PRE_VIEW:
			OnEnterView();
			break;
		case GAMETUTORIALSTATE.PRE_MOVE_ARRIVE:
			OnEnterArrive();
			break;
		case GAMETUTORIALSTATE.PRE_CHOOSETARGET:
			OnEnterChooseTarget();
			break;
		case GAMETUTORIALSTATE.CHOOSETARGET:
			CheckChooseTarget();
			break;
		case GAMETUTORIALSTATE.PRE_PUNCH:
			OnEnterPunch();
			break;
		case GAMETUTORIALSTATE.PUNCH:
			PunchStateCheck();
			break;
		case GAMETUTORIALSTATE.PRE_PICKUP:
			OnEnterPickUp();
			break;
		case GAMETUTORIALSTATE.PRE_CHANGE_WEAPON:
			OnEnterPreChangeWeapon();
			break;
		case GAMETUTORIALSTATE.PRE_SHOOT_CLICK:
			OnEnterPreShootClick();
			break;
		case GAMETUTORIALSTATE.SHOOT_CLICK:
			CheckShootClick();
			break;
		case GAMETUTORIALSTATE.PRE_SHOOT:
			OnEnterShoot();
			break;
		case GAMETUTORIALSTATE.SHOOT:
			PunchStateCheck();
			break;
		case GAMETUTORIALSTATE.PRE_GETONCAR:
			OnEnterGetOnCar();
			break;
		case GAMETUTORIALSTATE.PRE_GETONCARBTN:
			OnEnterPreGetOnCarBtn();
			break;
		case GAMETUTORIALSTATE.PRE_DRIVEMODE:
			OnEnterDriveMode();
			break;
		case GAMETUTORIALSTATE.PRE_DRIVE:
			OnEnterDrive();
			break;
		case GAMETUTORIALSTATE.PRE_DONE:
			OnEnterDone();
			break;
		case GAMETUTORIALSTATE.GETONCAR:
			if (GameUIController.instance.ctl_GetOnCarBtn.gameObject.activeInHierarchy)
			{
				curState = GAMETUTORIALSTATE.PRE_GETONCARBTN;
			}
			break;
		case GAMETUTORIALSTATE.WELCOME:
		case GAMETUTORIALSTATE.MOVE:
		case GAMETUTORIALSTATE.MOVE_ARRIVE:
		case GAMETUTORIALSTATE.VIEW:
		case GAMETUTORIALSTATE.PICKUP:
		case GAMETUTORIALSTATE.CHANGE_WEAPON:
		case GAMETUTORIALSTATE.DRIVEMODE:
		case GAMETUTORIALSTATE.DRIVE:
		case GAMETUTORIALSTATE.DONE:
			break;
		}
	}

	public void OnEnterPreMoveTocar()
	{
		pressTips.gameObject.SetActive(false);
		noMaskLabel.text = "Now try to reach the designated point on the ground";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 680f);
		noMaskBack.height = 45;
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		moveTargetObj.transform.position = new Vector3(-449.2f, 0f, -253f);
		moveTargetObj.SetActive(true);
		GameUIController.instance.EnableLocateLabel(moveTargetObj);
		GameController.instance.driveMode.minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(moveTargetObj.transform.position) + new Vector3(1000f, 480f, 0f));
		playerWall.SetActive(false);
		PlayerController.instance.occlusion.gameObject.SetActive(false);
	}

	public void PunchStateCheck()
	{
		if (PlayerController.instance.fireTarget == null && PlayerController.instance.preFireTarget == null && lastAI.curState.stateName != AISTATE.DIE)
		{
			PlayerController.instance.preFireTarget = lastAI;
		}
	}

	public void OnEnterDone()
	{
		if (!doneFlag)
		{
			doneFlag = true;
			Invoke("RatePageShow", 1f);
			Invoke("DelayDone", 2f);
		}
	}

	public void RatePageShow()
	{
		ratePageController = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/RatePage"))).GetComponent<RatePageController>();
		ratePageController.transform.parent = GameUIController.instance.controlUIRoot.transform;
		ratePageController.transform.localPosition = Vector3.zero;
		ratePageController.transform.localScale = Vector3.one;
		ratePageController.enablaBtnFlag = false;
		noMaskLabel.gameObject.SetActive(false);
		noMaskBack.gameObject.SetActive(false);
	}

	private void DelayDone()
	{
		curState = GAMETUTORIALSTATE.DONE;
		GlobalInf.firstOpenGameFlag = false;
		PlayerPrefs.SetInt("firstOpenGameFlag", 1);
		StoreDateController.SetHandGunNum(0, 1);
		GameController.instance.curMode = GameController.instance.normalMode;
		GameController.instance.curGameMode = GAMEMODE.NORMAL;
		TaskLabelController.instance.gameObject.SetActive(true);
		UnityEngine.Object.Destroy(base.gameObject);
		Resources.UnloadUnusedAssets();
		MinimapLightLabelController.instance.EnableLightLabel();
		for (int i = 0; i < MinimapLightLabelController.instance.lightLabel2.Length; i++)
		{
			MinimapLightLabelController.instance.lightLabel2[i].DisableStars();
		}
		instance = null;
		GameStateController.instance.ChangeGameState();
		GameController.instance.normalMode.normalSenceDieFlag = false;
		Time.timeScale = 0f;
		ratePageController.enablaBtnFlag = true;
		ratePageController = null;
	}

	public void OnEnterDrive()
	{
		noMaskLabel.gameObject.SetActive(false);
		noMaskBack.gameObject.SetActive(false);
		curState = GAMETUTORIALSTATE.DRIVE;
		nextState = GAMETUTORIALSTATE.PRE_DONE;
		Invoke("DelayChangeState", 3f);
		Platform.flurryEvent_onTutorialState(13);
	}

	public void OnEnterDriveMode()
	{
		Time.timeScale = 0f;
		driveModeChooseRoot.gameObject.SetActive(true);
		gravityRoot.SetActive(false);
		GlobalInf.carCtrlType = CARCTRLTYPE.BUTTON;
		GameUIController.instance.DisableLocateLabel();
		UIEventListener ctl_GetOnCarBtn = GameUIController.instance.ctl_GetOnCarBtn;
		ctl_GetOnCarBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(ctl_GetOnCarBtn.onClick, new UIEventListener.VoidDelegate(OnClickContinueBtn));
		curState = GAMETUTORIALSTATE.DRIVEMODE;
		Platform.flurryEvent_onTutorialState(12);
	}

	public void OnEnterPreGetOnCarBtn()
	{
		noMaskLabel.text = "Tap the \"[00F4FF]\"DOOR\"[FFFFFF]\" button to get on";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 450f);
		noMaskBack.height = 45;
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		curState = GAMETUTORIALSTATE.GETONCARBTN;
		UIEventListener ctl_GetOnCarBtn = GameUIController.instance.ctl_GetOnCarBtn;
		ctl_GetOnCarBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(ctl_GetOnCarBtn.onClick, new UIEventListener.VoidDelegate(OnClickContinueBtn));
		Platform.flurryEvent_onTutorialState(11);
		pressTips.gameObject.SetActive(true);
		pressTips.transform.position = GameUIController.instance.ctl_GetOnCarBtn.gameObject.transform.position;
	}

	public void OnEnterGetOnCar()
	{
		OnEnablePlayerCar();
		noMaskLabel.text = "Well done! Now let's talk about driving.\nYou can only get into a car, whether you\nown it or not, from \"[00F4FF]\"THE LEFT SIDE\"[FFFFFF]\".";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 550f);
		noMaskBack.height = 120;
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		curState = GAMETUTORIALSTATE.GETONCAR;
		playerWall.SetActive(false);
		PlayerController.instance.occlusion.gameObject.SetActive(false);
		PlayerController.instance.preFireTarget = null;
		PlayerController.instance.fireTarget = null;
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("AI"), LayerMask.NameToLayer("PlayerWall"), true);
		GameController.instance.driveMode.minimapController.DisableTargetPos();
		GameController.instance.driveMode.minimapController.EnablePlayerCarPos(CarManage.instance.playerCar.transform.position);
		pressTips.gameObject.SetActive(false);
		Platform.flurryEvent_onTutorialState(10);
	}

	public void OnEnterPreShootClick()
	{
		noMaskLabel.text = "When a gun is equipped, an NPC will be aimed at\nwhen he's set as your target. Try it on that guy over there";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 700f);
		noMaskBack.height = 80;
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		curState = GAMETUTORIALSTATE.SHOOT_CLICK;
		AIController aIController = (AIController)UnityEngine.Object.Instantiate(NPCPoolController.instance.ganstarBlackPreferb);
		aIController.gameObject.SetActiveRecursively(true);
		aIController.type = NPCTYPE.GANSTARBLACK_PUNCH;
		aIController.standFlag = true;
		aIController.transform.position = PlayerController.instance.transform.position + Vector3.left * 9.5f;
		aIController.transform.forward = -PlayerController.instance.transform.forward;
		aIController.ChangeState(AISTATE.IDLE);
		HealthController healthCtl = aIController.healthCtl;
		healthCtl.OnDestroy = (HealthController.onDestroy)Delegate.Combine(healthCtl.OnDestroy, new HealthController.onDestroy(OnTempAIDie));
		aIController.healthCtl.healthVal = 50f;
		lastAI = aIController;
		GameUIController.instance.EnableLocateLabel(lastAI.gameObject);
		GameController.instance.driveMode.minimapController.DisableTargetPos();
		GameController.instance.driveMode.minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(aIController.transform.position) + new Vector3(1000f, 480f, 0f));
		GameUIController.instance.player_ShotBtn.gameObject.SetActive(false);
		GameUIController.instance.reloadBtn.gameObject.SetActive(false);
		handTipTweenPos.gameObject.SetActive(false);
		Platform.flurryEvent_onTutorialState(9);
	}

	public void OnEnterShoot()
	{
		noMaskLabel.text = "Press and hold Attack button till he dies!";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 500f);
		noMaskBack.height = 45;
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		curState = GAMETUTORIALSTATE.SHOOT;
		GameUIController.instance.player_ShotBtn.gameObject.SetActive(true);
		GameUIController.instance.reloadBtn.gameObject.SetActive(true);
		pressTips.gameObject.SetActive(true);
		pressTips.transform.localPosition = new Vector3(257f * GlobalDefine.screenWidthFit, -140.7f, 0f);
		Platform.flurryEvent_onTutorialState(8);
	}

	public void OnEnterPreChangeWeapon()
	{
		noMaskLabel.text = "Great, it's a handgun!\nYou can change between different weapons at any time";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 700f);
		noMaskBack.height = 80;
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		curState = GAMETUTORIALSTATE.CHANGE_WEAPON;
		UIEventListener ctl_LeftChangeBtn = GameUIController.instance.ctl_LeftChangeBtn;
		ctl_LeftChangeBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(ctl_LeftChangeBtn.onClick, new UIEventListener.VoidDelegate(OnClickContinueBtn));
		UIEventListener ctl_RightChangeBtn = GameUIController.instance.ctl_RightChangeBtn;
		ctl_RightChangeBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(ctl_RightChangeBtn.onClick, new UIEventListener.VoidDelegate(OnClickContinueBtn));
		GameUIController.instance.InitUI();
		GameUIController.instance.addHealthBtn.gameObject.SetActive(false);
		GameUIController.instance.player_ShotBtn.gameObject.SetActive(false);
		GameUIController.instance.reloadBtn.gameObject.SetActive(false);
		GameController.instance.driveMode.minimapController.DisableTargetPos();
		pressTips.gameObject.SetActive(true);
		pressTips.transform.localPosition = new Vector3(313.3f * GlobalDefine.screenWordFit, 197.3f, 0f);
		Platform.flurryEvent_onTutorialState(7);
	}

	public void OnEnterPickUp()
	{
		curState = GAMETUTORIALSTATE.PICKUP;
		FallingObjCtl handGunObj = FallingObjPool.instance.GetHandGunObj();
		handGunObj.transform.position = aiPos;
		handGunObj.recycleFlag = true;
		handGunObj.num = 0;
		noMaskLabel.text = "Look! Something is dropped on the ground,\nwhy not pick it up?";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 550f);
		noMaskBack.height = 80;
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		PlayerController.instance.preFireTarget = null;
		PlayerController.instance.fireTarget = null;
		pressTips.gameObject.SetActive(false);
		Platform.flurryEvent_onTutorialState(6);
	}

	public void OnEnterPunch()
	{
		curState = GAMETUTORIALSTATE.PUNCH;
		noMaskLabel.text = "Move toward him, then use \"[00F4FF]\"ATTACK button\"[FFFFFF]\"\nto give him a punch! Don't stop before he dies!";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 600f);
		noMaskBack.height = 80;
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		GameUIController.instance.player_BoxingBtn.gameObject.SetActive(true);
		pressTips.gameObject.SetActive(true);
		pressTips.transform.localPosition = new Vector3(257f * GlobalDefine.screenWidthFit, -140.7f, 0f);
		handTipTweenPos.gameObject.SetActive(false);
		Platform.flurryEvent_onTutorialState(5);
	}

	public void CheckChooseTarget()
	{
		if (PlayerController.instance.fireTarget != null)
		{
			noMaskLabel.gameObject.SetActive(false);
			noMaskBack.gameObject.SetActive(false);
			curState = GAMETUTORIALSTATE.PRE_PUNCH;
		}
	}

	public void CheckShootClick()
	{
		if (PlayerController.instance.fireTarget != null)
		{
			noMaskLabel.gameObject.SetActive(false);
			noMaskBack.gameObject.SetActive(false);
			curState = GAMETUTORIALSTATE.PRE_SHOOT;
		}
	}

	public void OnEnterChooseTarget()
	{
		OnEnableNormalAI();
		curState = GAMETUTORIALSTATE.CHOOSETARGET;
		noMaskLabel.text = "By \"[00F4FF]\"TAPPING\"[FFFFFF]\" on an NPC, you can set him as your target";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 680f);
		noMaskBack.height = 45;
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		handTipTweenPos.gameObject.SetActive(false);
		Platform.flurryEvent_onTutorialState(4);
	}

	public void OnEnterArrive()
	{
		noMaskLabel.text = "Now try to reach the designated point on the ground";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 680f);
		noMaskBack.height = 45;
		noMaskUIRoot.SetActive(true);
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		moveTargetObj.transform.position = playerPos + Vector3.forward * 5f;
		moveTargetObj.SetActive(true);
		GameUIController.instance.EnableLocateLabel(moveTargetObj);
		GameController.instance.driveMode.minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(moveTargetObj.transform.position) + new Vector3(1000f, 480f, 0f));
		curState = GAMETUTORIALSTATE.MOVE_ARRIVE;
		Platform.flurryEvent_onTutorialState(3);
	}

	public void OnEnterView()
	{
		noMaskUIRoot.SetActive(true);
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		noMaskLabel.text = "\"[00F4FF]\"PUSH\"[FFFFFF]\" the Stick to move";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 350f);
		noMaskBack.height = 45;
		curState = GAMETUTORIALSTATE.VIEW;
		GameUIController.instance.controlUIRoot.SetActive(true);
		GameUIController.instance.controlUI.SetActive(false);
		GameUIController.instance.playerUI.SetActiveRecursively(false);
		GameUIController.instance.playerUI.SetActive(true);
		GameUIController.instance.joyStick.gameObject.SetActiveRecursively(true);
		handTipTweenPos.gameObject.SetActive(true);
		handTipTweenPos.from = new Vector3(-230f, -200f, 0f);
		handTipTweenPos.to = new Vector3(-100f, -70f, 0f);
		handTipTweenPos.ResetToBeginning();
		handTipTweenPos.PlayForward();
		handTipTweenAlph.ResetToBeginning();
		handTipTweenAlph.PlayForward();
		Platform.flurryEvent_onTutorialState(2);
	}

	public void OnEnterMove()
	{
		noMaskUIRoot.SetActive(true);
		enableTouchFlag = true;
		noMaskLabel.gameObject.SetActive(true);
		noMaskBack.gameObject.SetActive(true);
		noMaskLabel.text = "\"[00F4FF]\"SWIPE\"[FFFFFF]\" on the screen to adjust the camera";
		noMaskBack.width = (int)(GlobalDefine.screenWidthFit * 550f);
		noMaskBack.height = 45;
		curState = GAMETUTORIALSTATE.MOVE;
		handTipTweenPos.gameObject.SetActive(true);
		handTipTweenPos.from = new Vector3(-75f, -50f, 0f);
		handTipTweenPos.to = new Vector3(75f, -50f, 0f);
		handTipTweenPos.ResetToBeginning();
		handTipTweenPos.PlayForward();
		handTipTweenAlph.ResetToBeginning();
		handTipTweenAlph.PlayForward();
		Platform.flurryEvent_onTutorialState(1);
	}

	public void OnEnterWelcome()
	{
		curState = GAMETUTORIALSTATE.WELCOME;
		turtialUIRoot.SetActive(true);
		label.text = "Welcome to the city of freedom.\nBut first of all, we'll show you\nhow to play this game.";
		GameUIController.instance.controlUIRoot.gameObject.SetActive(false);
		GameUIController.instance.carUI.gameObject.SetActive(false);
		GameUIController.instance.tempUI.SetActive(false);
		GameUIController.instance.taskBoardController.gameObject.SetActive(false);
		GameUIController.instance.stateUIController.gameObject.SetActive(false);
		GameUIController.instance.resetPlayerCarBtn.gameObject.SetActive(false);
		GameUIController.instance.outOfAmmoLabel.gameObject.SetActive(false);
		Platform.flurryEvent_onTutorialState(0);
	}

	public void OnEnableHandGun()
	{
		GlobalInf.handgunIndex = 0;
		PlayerController.instance.EnableHandGun();
		PlayerController.instance.animaCtl.HandGunAwake();
	}

	public void OnEnableMachineGun()
	{
		GlobalInf.machineGunIndex = 0;
		PlayerController.instance.EnableMachineGun();
		PlayerController.instance.animaCtl.MachineGunAwake();
	}

	public void OnEnablePunchAI()
	{
		AIController attackAI = NPCPoolController.instance.GetAttackAI(NPCTYPE.GANSTARNAKED_PUNCH);
		attackAI.transform.position = playerPos + Vector3.forward * 5f;
		attackAI.ChangeState(AISTATE.FIGHTREADY);
	}

	public void OnEnableNormalAI()
	{
		AIController aIController = (AIController)UnityEngine.Object.Instantiate(NPCPoolController.instance.ganstarNakedPreferb);
		aIController.gameObject.SetActiveRecursively(true);
		aIController.type = NPCTYPE.GANSTARNAKED_PUNCH;
		aIController.standFlag = true;
		aIController.transform.position = PlayerController.instance.transform.position + Vector3.forward * 4.5f;
		aIController.transform.forward = -PlayerController.instance.transform.forward;
		aIController.ChangeState(AISTATE.IDLE);
		HealthController healthCtl = aIController.healthCtl;
		healthCtl.OnDestroy = (HealthController.onDestroy)Delegate.Combine(healthCtl.OnDestroy, new HealthController.onDestroy(OnTempAIDie));
		aIController.healthCtl.healthVal = 50f;
		lastAI = aIController;
		GameController.instance.driveMode.minimapController.DisableTargetPos();
		GameUIController.instance.EnableLocateLabel(lastAI.gameObject);
		GameController.instance.driveMode.minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(aIController.transform.position) + new Vector3(1000f, 480f, 0f));
	}

	public void OnTempAIDie()
	{
		OnClickContinueBtn(null);
		HealthController healthCtl = lastAI.healthCtl;
		healthCtl.OnDestroy = (HealthController.onDestroy)Delegate.Remove(healthCtl.OnDestroy, new HealthController.onDestroy(OnTempAIDie));
		GameUIController.instance.DisableLocateLabel();
	}

	public void OnEnableMachineGunAI()
	{
		AIController attackAI = NPCPoolController.instance.GetAttackAI(NPCTYPE.GANSTARBLACK_MG);
		attackAI.transform.position = playerPos + Vector3.forward * 15f;
		attackAI.transform.forward = -Vector3.forward;
		attackAI.ChangeState(AISTATE.MACHINEGUN);
	}

	public void OnEnablePlayerCar()
	{
		GlobalInf.playerCarIndex = 0;
		StoreDateController.SetCarNum(GlobalInf.playerCarIndex, 1);
		StoreDateController.SetCarIndex(GlobalInf.playerCarIndex);
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("PlayerCar/Car0")) as GameObject;
		gameObject.name = "PlayerCar";
		gameObject.transform.parent = null;
		CarManage.instance.playerCar = gameObject.GetComponent<CarController>();
		CopyCarInfo();
		CarManage.instance.playerCar.AICarCtl.enabled = false;
		CarManage.instance.playerCar.AICarCtl.insideNPC = null;
		CarManage.instance.playerCar.enabled = false;
		CarManage.instance.playerCar.AICarCtl.motor.enabled = false;
		CarManage.instance.playerCar.transform.position = playerCarPos + Vector3.up;
		CarManage.instance.playerCar.transform.eulerAngles = new Vector3(0f, 90f, 0f);
		CarManage.instance.playerCar.ResetPlayerCar();
		GameUIController.instance.EnableLocateLabel(CarManage.instance.playerCar.gameObject);
	}

	public void DelayChangeState()
	{
		delayChangFlag = false;
		curState = nextState;
	}

	public void CopyHandGunInfo()
	{
		noMaskLabel.gameObject.SetActive(false);
		noMaskBack.gameObject.SetActive(false);
		GlobalInf.handgunIndex = 0;
		StoreDateController.SetHandGunIndex(GlobalInf.handgunIndex);
		GlobalInf.handGunInfo = new GunsInfo();
		GlobalInf.handGunInfo.accuracy = handGunInfo.accuracy;
		GlobalInf.handGunInfo.level = handGunInfo.level;
		GlobalInf.handGunInfo.bulletNum = handGunInfo.bulletNum;
		GlobalInf.handGunInfo.bulletSpeed = handGunInfo.bulletSpeed;
		GlobalInf.handGunInfo.damage = handGunInfo.damage;
		GlobalInf.handGunInfo.gunName = handGunInfo.gunName;
		GlobalInf.handGunInfo.maxBulletNum = handGunInfo.maxBulletNum;
		GlobalInf.handGunInfo.reloadTime = handGunInfo.reloadTime;
		GlobalInf.handGunInfo.restBulletNum = handGunInfo.restBulletNum;
		GlobalInf.handGunInfo.shotInterval = handGunInfo.shotInterval;
		GlobalInf.handGunInfo.bulletPrise = handGunInfo.bulletPrise;
		PlayerController.instance.EnableHandGun();
		PlayerController.instance.animaCtl.HandGunAwake();
		curState = GAMETUTORIALSTATE.PRE_CHANGE_WEAPON;
	}

	public void CopyCarInfo()
	{
		GlobalInf.carInfo = new CarInfo();
		GlobalInf.carInfo.brakeForce = carInfo.brakeForce;
		GlobalInf.carInfo.maxHealthVal = carInfo.maxHealthVal;
		GlobalInf.carInfo.maxSpeed = carInfo.maxSpeed;
		GlobalInf.carInfo.maxSpeedSteerAngle = carInfo.maxSpeedSteerAngle;
		GlobalInf.carInfo.maxSteerAngle = carInfo.maxSteerAngle;
		GlobalInf.carInfo.restHealthVal = carInfo.restHealthVal;
		CarManage.instance.playerCar.maxSpeed = GlobalInf.carInfo.maxSpeed;
		CarManage.instance.playerCar.brakeForce = GlobalInf.carInfo.brakeForce;
		CarManage.instance.playerCar.maxSteerAngle = GlobalInf.carInfo.maxSteerAngle;
		CarManage.instance.playerCar.maxSpeedSteerAngle = GlobalInf.carInfo.maxSpeedSteerAngle;
		CarManage.instance.playerCar.carHealth.maxHealthVal = GlobalInf.carInfo.maxHealthVal;
		CarManage.instance.playerCar.carHealth.healthVal = GlobalInf.carInfo.restHealthVal;
		CarManage.instance.playerCar.carHealth.playerCarFlag = true;
	}
}
