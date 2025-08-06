using System;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
	public static GameUIController instance;

	public UIRoot uiRoot;

	public MyNGUIJoyStick joyStick;

	//public GUITexture targetPic;

	public PlayerController playerCtl;

	public Camera mainCam;

	public GameObject playerUI;

	public GameObject carUI;

	public GameObject controlUI;

	public GameObject controlUIRoot;

	public GameObject rootContainner;

	public UIEventListener player_BoxingBtn;

	public UIEventListener player_ShotBtn;

	public UIEventListener reloadBtn;

	public bool player_ShotPressFlag;

	public UIEventListener car_TurnLeftBtn;

	public UIEventListener car_TurnRightBtn;

	public UIEventListener car_AccBtn;

	public UIEventListener car_BrakeBtn;

	public bool car_TurnLeftPressFlag;

	public bool car_TurnRightPressFlag;

	public bool car_AccPressFlag;

	public bool car_BrakePressFlag;

	public UIEventListener ctl_LeftChangeBtn;

	public UIEventListener ctl_RightChangeBtn;

	public UIEventListener ctl_GetOnCarBtn;

	public GameObject topRightLabel;

	public GameObject topRightCircle;

	public GameObject topRightHand;

	public GameObject topRightHandGun;

	public GameObject topRightMachineGun;

	public GameObject policeLevelRoot;

	public GameObject[] policeLevelStar;

	public GameObject policeLevelBackRoot;

	public UISprite fireTarget;

	public UISprite preFireTarget;

	public UISprite playerCarParkLabel;

	public UILabel playerCarDistance;

	private Vector3 preFireTargetPos;

	public Vector3 playerCarParkLabelPos;

	public bool enableLocateLabelFlag;

	public GameObject[] locateLabelList;

	public UILabel[] locateDistanceList;

	public List<GameObject> locateTargetList;

	public UIEventListener addHealthBtn;

	public UIEventListener addToolKitBtn;

	public UILabel healthKitNumLabel;

	public UILabel toolKitNumLabel;

	public HeartUIControllor playerHealthUI;

	public HeartUIControllor carHealthUI;

	public GameObject redScreen;

	public TaskBoxUIController taskLabelUI;

	public TaskLabelUIController taskCheckUI;

	public DeadUIControllor deadUI;

	public GameObject tempUI;

	public GAMEMODE gameMode;

	public int taskLevel;

	public int taskIndex;

	public int rewardIndex;

	public TaskInfo curTaskInfo;

	public MiniMapController minimapController;

	public GameObject MiniMapEnterBtn;

	public OnClickMapEnter exitMapBtn;

	public UIEventListener pauseBtn;

	public PauseUIControllor pauseUIControllor;

	public UILabel bulletNumLabel;

	public TaskEndUIController taskEndUIControllor;

	public TopLineController topLine;

	public bool rolloverFlag;

	public UIEventListener rotateBtn;

	public bool timeCountFlag;

	public TaskBoardController taskBoardController;

	public bool disableCarBtnFlag;

	public PickLabelController pickLabel;

	public KillLabelController killLabel;

	public SurvivalWaveLabel survivalWaveLabel;

	public UIEventListener resetPlayerCarBtn;

	public LevelUpUIController levelUpUI;

	public UISprite miniMapFinger;

	public GameObject fingerObj;

	public StateUIController stateUIController;

	public DialogUIController dialogUIController;

	public bool healthKitTutorialFlag;

	public bool toolKitTutorialFlag;

	public TweenAlpha outOfAmmoLabel;

	public UILabel outOfAmmoLabelText;

	public BuyKitPageController buyKitPage;

	public UIEventListener bulletVideoBtn;

	public bool clickTaskLabelFlag;

	public bool taskLabelTimeCountFlag;

	private float taskLabelTimeCount;

	public bool clickTaskCheckFlag;

	public int lockIndex;

	public GAMEMODE lockMode;

	private int sumBulletNum;

	public bool lockWeaponFlag;

	private Vector2 pos;

	public bool getOnCarBtnFlag;

	public bool delayEnableGetOnCarBtn;

	private bool fingerFlag;

	private float xPos;

	private float yPos;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		InitBtn();
		PlayerController playerController = playerCtl;
		playerController.onGetOnCarDone = (PlayerController.OnGetOnCarDone)Delegate.Combine(playerController.onGetOnCarDone, new PlayerController.OnGetOnCarDone(OnChangCarUI));
		PlayerController playerController2 = playerCtl;
		playerController2.onGetOffCarDone = (PlayerController.OnGetOffCarDone)Delegate.Combine(playerController2.onGetOffCarDone, new PlayerController.OnGetOffCarDone(OnChangePlayerUI));
		policeLevelRoot.gameObject.SetActiveRecursively(true);
		policeLevelRoot.gameObject.SetActiveRecursively(false);
		healthKitTutorialFlag = false;
		toolKitTutorialFlag = false;
	}

	public void CheckPoliceLevel()
	{
		if (PoliceLevelCtl.level == 0)
		{
			if (AICarPoolController.instance.policeCarCount != 0)
			{
				policeLevelStar[0].gameObject.SetActiveRecursively(true);
				for (int i = 0; i < policeLevelStar.Length; i++)
				{
					if (i < 1)
					{
						policeLevelStar[i].gameObject.SetActiveRecursively(true);
					}
					else
					{
						policeLevelStar[i].gameObject.SetActiveRecursively(false);
					}
				}
				if (!policeLevelBackRoot.active)
				{
					policeLevelBackRoot.SetActiveRecursively(true);
				}
			}
			else
			{
				if (policeLevelBackRoot.active)
				{
					policeLevelBackRoot.SetActiveRecursively(false);
				}
				for (int j = 0; j < policeLevelStar.Length; j++)
				{
					policeLevelStar[j].gameObject.SetActiveRecursively(false);
				}
			}
			return;
		}
		if (!policeLevelBackRoot.active)
		{
			policeLevelBackRoot.SetActiveRecursively(true);
		}
		for (int k = 0; k < policeLevelStar.Length; k++)
		{
			if (k < PoliceLevelCtl.level)
			{
				policeLevelStar[k].gameObject.SetActiveRecursively(true);
			}
			else
			{
				policeLevelStar[k].gameObject.SetActiveRecursively(false);
			}
		}
	}

	public void InitUI()
	{
		rootContainner.SetActive(true);
		controlUIRoot.SetActive(true);
		if (PlayerController.instance != null)
		{
			if (PlayerController.instance.curState == PLAYERSTATE.CAR)
			{
				OnChangCarUI();
			}
			else
			{
				OnChangePlayerUI();
			}
		}
		else
		{
			OnChangePlayerUI();
		}
		controlUI.gameObject.SetActiveRecursively(true);
		bulletVideoBtn.gameObject.SetActiveRecursively(false);
		ctl_GetOnCarBtn.gameObject.SetActiveRecursively(false);
		SetTopRightLabel(playerCtl.curState);
		taskLabelUI.gameObject.SetActiveRecursively(false);
		taskCheckUI.gameObject.SetActiveRecursively(false);
		deadUI.gameObject.SetActiveRecursively(false);
		pauseUIControllor.gameObject.SetActiveRecursively(false);
		pickLabel.gameObject.SetActiveRecursively(false);
		killLabel.gameObject.SetActiveRecursively(false);
		survivalWaveLabel.gameObject.SetActiveRecursively(false);
		rotateBtn.gameObject.SetActive(true);
		tempUI.SetActive(true);
		MiniMapEnterBtn.gameObject.SetActive(true);
		if (GameController.instance.curGameMode == GAMEMODE.SKILLDRIVING)
		{
			resetPlayerCarBtn.gameObject.SetActiveRecursively(true);
		}
		else
		{
			resetPlayerCarBtn.gameObject.SetActiveRecursively(false);
		}
		outOfAmmoLabel.gameObject.SetActiveRecursively(false);
		miniMapFinger.gameObject.SetActiveRecursively(false);
		if (timeCountFlag)
		{
			taskBoardController.ResetTaskBoardUI();
		}
		else
		{
			taskBoardController.gameObject.SetActiveRecursively(false);
		}
		if (!GlobalInf.newUserFlag)
		{
			if (GlobalInf.gameState < GameStateController.MAXSTATENUM)
			{
				GameStateController.instance.CheckGameState();
			}
			else
			{
				stateUIController.gameObject.SetActiveRecursively(false);
			}
		}
		else if (GlobalInf.gameState < GameStateController.NEWMAXSTATENUM)
		{
			GameStateController.instance.CheckGameState();
		}
		else
		{
			stateUIController.gameObject.SetActiveRecursively(false);
		}
		dialogUIController.gameObject.SetActiveRecursively(false);
		topLine.gameObject.SetActiveRecursively(false);
		buyKitPage.gameObject.SetActiveRecursively(false);
		CheckPoliceLevel();
		if (GameController.instance.curGameMode == GAMEMODE.ROBCAR)
		{
			GameController.instance.robbingCarMode.ResetUI();
		}
	}

	public void SetTopRightLabel(PLAYERSTATE cState)
	{
		topRightCircle.SetActive(true);
		topRightHand.SetActive(false);
		topRightHandGun.SetActive(false);
		topRightMachineGun.SetActive(false);
		switch (cState)
		{
		case PLAYERSTATE.NORMAL:
			topRightHand.SetActive(true);
			bulletNumLabel.gameObject.SetActive(false);
			bulletVideoBtn.gameObject.SetActive(false);
			break;
		case PLAYERSTATE.HANDGUN:
			topRightHandGun.SetActive(true);
			bulletNumLabel.gameObject.SetActive(true);
			ReflashHandGunBulletNum();
			sumBulletNum = PlayerController.instance.gun.gunInfo.curBulletNum + PlayerController.instance.gun.gunInfo.restBulletNum;
			if (sumBulletNum == 0 && GlobalInf.onFetchCompletedFlag && GlobalInf.adsHandGunBulletFlag && !Platform.isLowAPILevel)
			{
				bulletVideoBtn.gameObject.SetActiveRecursively(true);
			}
			break;
		case PLAYERSTATE.MACHINEGUN:
			topRightMachineGun.SetActive(true);
			bulletNumLabel.gameObject.SetActive(true);
			ReflashMachineGunBulletNum();
			sumBulletNum = PlayerController.instance.machineGun.gunInfo.curBulletNum + PlayerController.instance.machineGun.gunInfo.restBulletNum;
			if (sumBulletNum == 0 && GlobalInf.onFetchCompletedFlag && GlobalInf.adsMachineGunBulletFlag && !Platform.isLowAPILevel)
			{
				bulletVideoBtn.gameObject.SetActiveRecursively(true);
			}
			break;
		default:
			topRightHand.SetActiveRecursively(true);
			break;
		}
	}

	public void ReflashHandGunBulletNum()
	{
		bulletNumLabel.text = string.Empty + (PlayerController.instance.gun.gunInfo.curBulletNum - PlayerController.instance.gun.bulletCount) + "/" + (PlayerController.instance.gun.gunInfo.restBulletNum + (PlayerController.instance.gun.gunInfo.curBulletNum - PlayerController.instance.gun.bulletCount));
	}

	public void ReflashMachineGunBulletNum()
	{
		bulletNumLabel.text = string.Empty + (PlayerController.instance.machineGun.gunInfo.curBulletNum - PlayerController.instance.machineGun.bulletCount) + "/" + (PlayerController.instance.machineGun.gunInfo.restBulletNum + (PlayerController.instance.machineGun.gunInfo.curBulletNum - PlayerController.instance.machineGun.bulletCount));
	}

	public void InitBtn()
	{
		UIEventListener uIEventListener = player_ShotBtn;
		uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressPlayer_ShotBtn));
		UIEventListener uIEventListener2 = reloadBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickReloadBtn));
		UIEventListener uIEventListener3 = car_TurnLeftBtn;
		uIEventListener3.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener3.onPress, new UIEventListener.BoolDelegate(OnPressCar_TurnLeftBtn));
		UIEventListener uIEventListener4 = car_TurnRightBtn;
		uIEventListener4.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener4.onPress, new UIEventListener.BoolDelegate(OnPressCar_TurnRightBtn));
		UIEventListener uIEventListener5 = car_AccBtn;
		uIEventListener5.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener5.onPress, new UIEventListener.BoolDelegate(OnPressCar_AccBtn));
		UIEventListener uIEventListener6 = car_BrakeBtn;
		uIEventListener6.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener6.onPress, new UIEventListener.BoolDelegate(OnPressCar_BrakeBtn));
		UIEventListener uIEventListener7 = player_BoxingBtn;
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener7.onClick, new UIEventListener.VoidDelegate(OnClickPlayer_BoxingBtn));
		UIEventListener uIEventListener8 = ctl_GetOnCarBtn;
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener8.onClick, new UIEventListener.VoidDelegate(OnClickCtl_GetOnCarBtn));
		UIEventListener uIEventListener9 = ctl_LeftChangeBtn;
		uIEventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener9.onClick, new UIEventListener.VoidDelegate(OnClickCtl_LeftChangeBtn));
		UIEventListener uIEventListener10 = ctl_RightChangeBtn;
		uIEventListener10.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener10.onClick, new UIEventListener.VoidDelegate(OnClickCtl_RightChangeBtn));
		UIEventListener okBtn = taskCheckUI.okBtn;
		okBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(okBtn.onClick, new UIEventListener.VoidDelegate(OnClickTaskCheckBtn));
		UIEventListener okBtn2 = taskLabelUI.okBtn;
		okBtn2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(okBtn2.onClick, new UIEventListener.VoidDelegate(OnClickTaskLabelBtn));
		UIEventListener cancelBtn = taskCheckUI.cancelBtn;
		cancelBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(cancelBtn.onClick, new UIEventListener.VoidDelegate(OnClickBackTaskBtn));
		UIEventListener buyHandGunBulletBtn = taskCheckUI.buyHandGunBulletBtn;
		buyHandGunBulletBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(buyHandGunBulletBtn.onClick, new UIEventListener.VoidDelegate(OnClickBuyHandGunBulletBtn));
		UIEventListener buyMachineGunBulletBtn = taskCheckUI.buyMachineGunBulletBtn;
		buyMachineGunBulletBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(buyMachineGunBulletBtn.onClick, new UIEventListener.VoidDelegate(OnClickBuyMachineGunBulletBtn));
		UIEventListener uIEventListener11 = bulletVideoBtn;
		uIEventListener11.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener11.onClick, new UIEventListener.VoidDelegate(OnClickBulletVideoBtn));
		UIEventListener okBtn3 = deadUI.okBtn;
		okBtn3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(okBtn3.onClick, new UIEventListener.VoidDelegate(OnClickDieCheckBtn));
		UIEventListener uIEventListener12 = pauseBtn;
		uIEventListener12.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener12.onClick, new UIEventListener.VoidDelegate(OnClickPauseBtn));
		UIEventListener uIEventListener13 = addHealthBtn;
		uIEventListener13.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener13.onClick, new UIEventListener.VoidDelegate(OnClickAddHealthBtn));
		UIEventListener uIEventListener14 = addToolKitBtn;
		uIEventListener14.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener14.onClick, new UIEventListener.VoidDelegate(OnClickToolKitBtn));
		UIEventListener uIEventListener15 = resetPlayerCarBtn;
		uIEventListener15.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener15.onClick, new UIEventListener.VoidDelegate(OnClickResetPlayerCarBtn));
		UIEventListener backMenuBtn = deadUI.backMenuBtn;
		backMenuBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(backMenuBtn.onClick, new UIEventListener.VoidDelegate(pauseUIControllor.OnClickMainMenuBtn));
	}

	public void OnClickBuyHandGunBulletBtn(GameObject btn)
	{
		if (GlobalInf.cash >= taskCheckUI.handGunBulletPrise)
		{
			if (taskCheckUI.handGunBulletPrise != 0)
			{
				GlobalInf.cash -= taskCheckUI.handGunBulletPrise;
				StoreDateController.SetCash();
				GlobalInf.totalCashSpent += taskCheckUI.handGunBulletPrise;
				StoreDateController.SetTotalCashSpent();
				PlayerController.instance.gun.gunInfo.restBulletNum = PlayerController.instance.gun.gunInfo.maxBulletNum - PlayerController.instance.gun.gunInfo.bulletNum;
				PlayerController.instance.gun.bulletCount = 0;
				PlayerController.instance.gun.gunInfo.curBulletNum = PlayerController.instance.gun.gunInfo.bulletNum;
				StoreDateController.SetHandGunBulletNum(GlobalInf.handgunIndex, PlayerController.instance.gun.gunInfo.maxBulletNum);
				taskCheckUI.handGunBulletPrise = 0;
				taskCheckUI.handGunBulletPriseLabel.text = string.Empty + 0;
				taskCheckUI.handGunBulletNumLabel.text = string.Empty + PlayerController.instance.gun.gunInfo.maxBulletNum + "/" + PlayerController.instance.gun.gunInfo.maxBulletNum;
				AudioController.instance.play(AudioType.RELOAD);
			}
		}
		else
		{
			topLine.checkPageFlag = true;
			topLine.gameObject.SetActiveRecursively(true);
			topLine.OnClickCashBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.BULLET;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
			Platform.hideFeatureView();
		}
	}

	public void OnClickBuyMachineGunBulletBtn(GameObject btn)
	{
		if (GlobalInf.cash >= taskCheckUI.machineGunBulletPrise)
		{
			if (taskCheckUI.machineGunBulletPrise != 0)
			{
				GlobalInf.cash -= taskCheckUI.machineGunBulletPrise;
				StoreDateController.SetCash();
				GlobalInf.totalCashSpent += taskCheckUI.machineGunBulletPrise;
				StoreDateController.SetTotalCashSpent();
				PlayerController.instance.machineGun.gunInfo.restBulletNum = PlayerController.instance.machineGun.gunInfo.maxBulletNum - PlayerController.instance.machineGun.gunInfo.bulletNum;
				PlayerController.instance.machineGun.bulletCount = 0;
				PlayerController.instance.machineGun.gunInfo.curBulletNum = PlayerController.instance.machineGun.gunInfo.bulletNum;
				StoreDateController.SetMachineGunBulletNum(GlobalInf.machineGunIndex, PlayerController.instance.machineGun.gunInfo.maxBulletNum);
				taskCheckUI.machineGunBulletPrise = 0;
				taskCheckUI.machineGunBulletPriseLabel.text = string.Empty + 0;
				taskCheckUI.machineGunBulletNumLabel.text = string.Empty + PlayerController.instance.machineGun.gunInfo.maxBulletNum + "/" + PlayerController.instance.machineGun.gunInfo.maxBulletNum;
				AudioController.instance.play(AudioType.RELOAD);
			}
		}
		else
		{
			topLine.checkPageFlag = true;
			topLine.gameObject.SetActiveRecursively(true);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.BULLET;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
			topLine.OnClickCashBtn(null);
			Platform.hideFeatureView();
		}
	}

	public void OnClickReloadBtn(GameObject obj)
	{
		if (PlayerController.instance.curState == PLAYERSTATE.HANDGUN)
		{
			PlayerController.instance.gun.ReloadPlayerGun();
		}
		else
		{
			PlayerController.instance.machineGun.ReloadPlayerGun();
		}
	}

	public void OnClickResetPlayerCarBtn(GameObject obj)
	{
		GameController.instance.skillDrivingMode.ResetPlayerCarPos();
	}

	public void OnClickAddHealthBtn(GameObject obj)
	{
		if (PlayerController.instance.healCtl.healthVal > PlayerController.instance.healCtl.maxHealthVal * 0.75f)
		{
			return;
		}
		if (GlobalInf.healthKitNum > 0)
		{
			PlayerPrefs.SetInt("healthTutorialFlag", 1);
			if (healthKitTutorialFlag)
			{
				healthKitTutorialFlag = false;
				addHealthBtn.gameObject.GetComponent<UISprite>().depth = 2;
				Time.timeScale = 1f;
				miniMapFinger.enabled = false;
				miniMapFinger.depth = 7;
				fingerObj.transform.localPosition = new Vector3(-297f * GlobalDefine.screenWidthFit, 162f, 0f);
			}
			playerHealthUI.ResetCureState();
			GlobalInf.playerHealthVal = 100;
			StoreDateController.SetPlayerHealthVal();
			PlayerController.instance.healCtl.Reset();
			GlobalInf.healthKitNum--;
			StoreDateController.SetHealthKitNum(GlobalInf.healthKitNum);
			TempObjControllor.instance.GetHealthObj(PlayerController.instance.transform);
		}
		else
		{
			Time.timeScale = 0f;
			buyKitPage.gameObject.SetActiveRecursively(true);
			buyKitPage.ResetPage(true);
			topLine.gameObject.SetActiveRecursively(true);
			GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.BUYKIT);
			pauseBtn.gameObject.SetActiveRecursively(false);
		}
		OnInitHealthKitNum();
	}

	public void OnClickToolKitBtn(GameObject btn)
	{
		if (PlayerController.instance.car.carHealth.healthVal >= PlayerController.instance.car.carHealth.maxHealthVal * 0.75f)
		{
			return;
		}
		if (GlobalInf.toolKitNum > 0)
		{
			PlayerPrefs.SetInt("toolKitTutorialFlag", 1);
			if (toolKitTutorialFlag)
			{
				toolKitTutorialFlag = false;
				addHealthBtn.gameObject.GetComponent<UISprite>().depth = 2;
				Time.timeScale = 1f;
				miniMapFinger.enabled = false;
				miniMapFinger.depth = 7;
				fingerObj.transform.localPosition = new Vector3(-297f * GlobalDefine.screenWidthFit, 162f, 0f);
			}
			carHealthUI.ResetCureState();
			PlayerController.instance.car.carHealth.RepaireCar();
			TempObjControllor.instance.RecycleSmoke();
			GlobalInf.toolKitNum--;
			StoreDateController.SetToolKitNum(GlobalInf.toolKitNum);
			TempObjControllor.instance.GetHealthObj(PlayerController.instance.car.transform);
		}
		else
		{
			Time.timeScale = 0f;
			buyKitPage.gameObject.SetActiveRecursively(true);
			buyKitPage.ResetPage(false);
			topLine.gameObject.SetActiveRecursively(true);
			GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.BUYKIT);
			pauseBtn.gameObject.SetActiveRecursively(false);
		}
		OnInitToolKitNum();
	}

	public void OnInitHealthKitNum()
	{
		healthKitNumLabel.text = string.Empty + GlobalInf.healthKitNum;
	}

	public void OnInitToolKitNum()
	{
		toolKitNumLabel.text = string.Empty + GlobalInf.toolKitNum;
	}

	public void OnClickPauseBtn(GameObject obj)
	{
		if (!(GameSenceTutorialController.instance != null))
		{
			if (taskLabelUI.gameObject.activeInHierarchy)
			{
				taskLabelUI.gameObject.SetActive(false);
			}
			pauseUIControllor.gameObject.SetActiveRecursively(true);
			pauseUIControllor.ResetPauseUI();
			GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.PAUSE);
			GC.Collect();
			Resources.UnloadUnusedAssets();
			AudioController.instance.pauseSounds();
		}
	}

	public void OnClickBackTaskBtn(GameObject btn)
	{
		if (!GlobalInf.mapMode)
		{
			Time.timeScale = 1f;
		}
		joyStick.enableFlag = true;
		taskCheckUI.gameObject.SetActiveRecursively(false);
		if (GlobalInf.mapMode)
		{
			minimapController.mapUIFlag = false;
		}
		if (!GlobalInf.mapMode)
		{
			InitUI();
		}
		GameSenceBackBtnCtl.instance.PopGameUIState();
		Platform.hideFeatureView();
		gameMode = GAMEMODE.NORMAL;
		clickTaskLabelFlag = false;
		topLine.checkPageFlag = false;
	}

	public void DisableTaskCheck()
	{
		taskCheckUI.gameObject.SetActiveRecursively(false);
	}

	public void ResetTaskCheckLabel()
	{
		taskCheckUI.ResetTaskCheckUI(gameMode, taskIndex);
	}

	public void OnClickTaskLabelBtn(GameObject btn)
	{
		if (taskLabelUI.okUIBtn.enabled)
		{
			taskLabelUI.transform.GetChild(0).GetComponent<Animation>().Play("TaskLabelOut");
			taskLabelUI.OnDisableOKBtn();
			taskLabelTimeCount = 0f;
			taskLabelTimeCountFlag = true;
			clickTaskLabelFlag = true;
			if (gameMode == GAMEMODE.SLOT)
			{
				curTaskInfo = TaskLabelController.instance.GetTaskInfo(gameMode, taskIndex);
				clickTaskLabelFlag = false;
				GameController.instance.ChangeMode(gameMode, taskIndex);
				GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.SLOT);
			}
			else
			{
				taskCheckUI.gameObject.SetActiveRecursively(true);
				ResetTaskCheckLabel();
				joyStick.enableFlag = false;
				GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.TASKCHECK);
				Platform.showFeatureView(FeatureViewPosType.MIDDLE);
			}
			controlUIRoot.SetActiveRecursively(false);
			resetPlayerCarBtn.gameObject.SetActiveRecursively(false);
			outOfAmmoLabel.gameObject.SetActiveRecursively(false);
			stateUIController.gameObject.SetActiveRecursively(false);
			dialogUIController.gameObject.SetActiveRecursively(false);
			buyKitPage.gameObject.SetActiveRecursively(false);
			topLine.gameObject.SetActiveRecursively(true);
		}
	}

	public void DisableTaskLabel()
	{
		taskLabelUI.gameObject.SetActiveRecursively(false);
		if (gameMode != GAMEMODE.SLOT && taskCheckUI.gameObject.active)
		{
			Time.timeScale = 0f;
		}
	}

	public void OnDieUIEnable()
	{
		GlobalInf.playerHealthVal = 100;
		StoreDateController.SetPlayerHealthVal();
		controlUIRoot.gameObject.SetActiveRecursively(false);
		resetPlayerCarBtn.gameObject.SetActiveRecursively(false);
		outOfAmmoLabel.gameObject.SetActiveRecursively(false);
		DisableTaskLabel();
		Time.timeScale = 0f;
		NGUITools.SetActiveRecursively(deadUI.gameObject, true);
		deadUI.disAppearHart.gameObject.SetActiveRecursively(false);
		ADShowController.instance.showFlag = false;
		ADShowController.instance.countTime = 0f;
		Platform.showFullScreenSmall();
		deadUI.driveLabel.text = string.Empty + string.Format("{0:F}", GlobalInf.curDistance) + "km";
		deadUI.timeLabel.text = string.Empty + pauseUIControllor.ChangeSecToMin((int)Time.time - GlobalInf.curStartTime);
		deadUI.killLabel.text = string.Empty + GlobalInf.curKill;
		joyStick.enableFlag = false;
		if (GlobalInf.hartCount == GlobalInf.gameLevel + 3)
		{
			Platform.ResetHartTimeCount();
		}
		else
		{
			Platform.CountHarts();
		}
		GlobalInf.hartCount--;
		Platform.CountHarts();
		GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.DEAD);
		GlobalInf.dailyDieNum++;
		Platform.showFeatureView(FeatureViewPosType.MIDDLE);
	}

	public void OnClickDieCheckBtn(GameObject btn)
	{
		Platform.hideFeatureView();
		if (GlobalInf.hartCount > 0)
		{
			Time.timeScale = 1f;
			GlobalInf.curDistance = 0f;
			GlobalInf.curStartTime = (int)Time.time;
			GlobalInf.curKill = 0;
			GameSenceBackBtnCtl.instance.PopGameUIState();
			CancelInvoke("DisableTaskLabel");
			joyStick.enableFlag = true;
			deadUI.gameObject.SetActiveRecursively(false);
			CitySenceController.instance.OnPlayerDie();
		}
		else
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/BuyHartUI")) as GameObject;
			gameObject.transform.parent = rootContainner.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			topLine.gameObject.SetActiveRecursively(true);
			GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.BUYHART);
		}
	}

	public void OnClickTaskCheckBtn(GameObject btn)
	{
		if (clickTaskCheckFlag)
		{
			return;
		}
		clickTaskCheckFlag = true;
		GameSenceBackBtnCtl.instance.PopGameUIState();
		Time.timeScale = 1f;
		joyStick.enableFlag = true;
		delayEnableGetOnCarBtn = true;
		Invoke("DelayEnableGetOnCarBtn", 0.5f);
		lockMode = gameMode;
		if (gameMode == GAMEMODE.DRIVING0)
		{
			MinimapLightLabelController.instance.DisableLightLabel();
			BlackScreen.instance.TurnOffScreen();
			lockIndex = taskIndex;
			Invoke("DelayScreen", 0.5f);
		}
		else if (gameMode == GAMEMODE.DELIVER)
		{
			if (GameController.instance.startLabel != null)
			{
				GameController.instance.startLabel.gameObject.SetActiveRecursively(false);
			}
			MinimapLightLabelController.instance.DisableLightLabel();
			BlackScreen.instance.TurnOffScreen();
			lockIndex = taskIndex;
			Invoke("DelayScreen", 0.5f);
		}
		else if (gameMode == GAMEMODE.SURVIVAL)
		{
			MinimapLightLabelController.instance.DisableLightLabel();
			BlackScreen.instance.TurnOffScreen();
			lockIndex = taskIndex;
			Invoke("DelayScreen", 0.5f);
		}
		else if (gameMode == GAMEMODE.SLOT)
		{
			GameController.instance.ChangeMode(gameMode, taskIndex);
			GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.SLOT);
		}
		else if (gameMode == GAMEMODE.GUNKILLING)
		{
			MinimapLightLabelController.instance.DisableLightLabel();
			BlackScreen.instance.TurnOffScreen();
			lockIndex = taskIndex;
			Invoke("DelayScreen", 0.5f);
		}
		else if (gameMode == GAMEMODE.CARKILLING)
		{
			MinimapLightLabelController.instance.DisableLightLabel();
			BlackScreen.instance.TurnOffScreen();
			lockIndex = taskIndex;
			Invoke("DelayScreen", 0.5f);
		}
		else if (gameMode == GAMEMODE.SKILLDRIVING)
		{
			MinimapLightLabelController.instance.DisableLightLabel();
			BlackScreen.instance.TurnOffScreen();
			lockIndex = taskIndex;
			Invoke("DelayScreen", 0.5f);
		}
		else if (gameMode == GAMEMODE.ROBCAR)
		{
			MinimapLightLabelController.instance.DisableLightLabel();
			BlackScreen.instance.TurnOffScreen();
			lockIndex = taskIndex;
			Invoke("DelayScreen", 0.5f);
		}
		else if (gameMode == GAMEMODE.FIGHTING)
		{
			MinimapLightLabelController.instance.DisableLightLabel();
			BlackScreen.instance.TurnOffScreen();
			lockIndex = taskIndex;
			Invoke("DelayScreen", 0.5f);
		}
		else if (gameMode == GAMEMODE.ROBMOTOR)
		{
			MinimapLightLabelController.instance.DisableLightLabel();
			BlackScreen.instance.TurnOffScreen();
			lockIndex = taskIndex;
			Invoke("DelayScreen", 0.5f);
		}
		Platform.hideFeatureView();
		curTaskInfo = TaskLabelController.instance.GetTaskInfo(gameMode, taskIndex);
		TaskLabelController.instance.lastInfo = curTaskInfo;
		GlobalInf.lastMode = curTaskInfo.taskMode;
		GlobalInf.lastIndex = curTaskInfo.taskIndex;
		StoreDateController.SetLastInfo((int)GlobalInf.lastMode, GlobalInf.lastIndex);
		topLine.checkPageFlag = false;
	}

	private void DelayEnableGetOnCarBtn()
	{
		delayEnableGetOnCarBtn = false;
		clickTaskCheckFlag = false;
	}

	private void DelayScreen()
	{
		if (GlobalInf.mapMode)
		{
			minimapController.mapUIFlag = false;
			exitMapBtn.entermissionFlag = true;
			exitMapBtn.Execute();
		}
		taskCheckUI.gameObject.SetActiveRecursively(false);
		GameController.instance.ChangeMode(lockMode, lockIndex);
		clickTaskLabelFlag = false;
	}

	private void OnPressPlayer_ShotBtn(GameObject btn, bool isPress)
	{
		player_ShotPressFlag = isPress;
		if (isPress)
		{
			if (PlayerController.instance.curState == PLAYERSTATE.HANDGUN)
			{
				sumBulletNum = PlayerController.instance.gun.gunInfo.curBulletNum + PlayerController.instance.gun.gunInfo.restBulletNum;
				if (sumBulletNum > 0)
				{
					DisableOutOffAmmo();
					if (bulletVideoBtn.gameObject.active)
					{
						bulletVideoBtn.gameObject.SetActiveRecursively(false);
					}
				}
				else
				{
					EnableOutOffAmmo("Out of Ammo");
					if (!bulletVideoBtn.gameObject.active && GlobalInf.onFetchCompletedFlag && GlobalInf.adsHandGunBulletFlag && !Platform.isLowAPILevel)
					{
						bulletVideoBtn.gameObject.SetActiveRecursively(true);
					}
				}
			}
			else
			{
				if (PlayerController.instance.curState != PLAYERSTATE.MACHINEGUN)
				{
					return;
				}
				sumBulletNum = PlayerController.instance.machineGun.gunInfo.curBulletNum + PlayerController.instance.machineGun.gunInfo.restBulletNum;
				if (sumBulletNum > 0)
				{
					DisableOutOffAmmo();
					if (bulletVideoBtn.gameObject.active)
					{
						bulletVideoBtn.gameObject.SetActiveRecursively(false);
					}
				}
				else
				{
					EnableOutOffAmmo("Out of Ammo");
					if (!bulletVideoBtn.gameObject.active && GlobalInf.onFetchCompletedFlag && GlobalInf.adsMachineGunBulletFlag && !Platform.isLowAPILevel)
					{
						bulletVideoBtn.gameObject.SetActiveRecursively(true);
					}
				}
			}
		}
		else
		{
			DisableOutOffAmmo();
		}
	}

	private void OnPressCar_TurnLeftBtn(GameObject btn, bool isPress)
	{
		car_TurnLeftPressFlag = isPress;
		playerCtl.car.leftArrowPressFlag = isPress;
	}

	private void OnPressCar_TurnRightBtn(GameObject btn, bool isPress)
	{
		car_TurnRightPressFlag = isPress;
		playerCtl.car.rightArrowPressFlag = isPress;
	}

	private void OnPressCar_AccBtn(GameObject btn, bool isPress)
	{
		if (!disableCarBtnFlag)
		{
			car_AccPressFlag = isPress;
			if (playerCtl.car != null)
			{
				playerCtl.car.accBtnPressFlag = isPress;
			}
		}
	}

	private void OnPressCar_BrakeBtn(GameObject btn, bool isPress)
	{
		if (!disableCarBtnFlag)
		{
			car_BrakePressFlag = isPress;
			playerCtl.car.brakeBtnPressFlag = isPress;
		}
	}

	public void OnClickPlayer_BoxingBtn(GameObject btn)
	{
		playerCtl.OnClickPlayer_BoxingBtn();
	}

	public void OnClickCtl_LeftChangeBtn(GameObject btn)
	{
		if (!lockWeaponFlag && playerCtl.curState != PLAYERSTATE.CAR && playerCtl.curState != PLAYERSTATE.RAGDOLL)
		{
			playerCtl.OnChangeState(true);
			SetTopRightLabel(playerCtl.curState);
			ChangePlayerFireBtn(playerCtl.curState);
		}
	}

	public void OnClickCtl_RightChangeBtn(GameObject btn)
	{
		if (!lockWeaponFlag && playerCtl.curState != PLAYERSTATE.CAR && playerCtl.curState != PLAYERSTATE.RAGDOLL)
		{
			playerCtl.OnChangeState(false);
			SetTopRightLabel(playerCtl.curState);
			ChangePlayerFireBtn(playerCtl.curState);
		}
	}

	public void OnClickCtl_GetOnCarBtn(GameObject btn)
	{
		if (!playerCtl.changingTheCarFlag)
		{
			playerCtl.changingTheCarFlag = true;
			if (playerCtl.curState == PLAYERSTATE.CAR)
			{
				playerCtl.GetOffCar();
			}
			else
			{
				playerCtl.GetOnCar(CarManage.instance.nearestCar.getOnPoint.transform, CarManage.instance.nearestCar);
			}
		}
	}

	private void Start()
	{
		InitUI();
		topRightLabel.GetComponent<UIAnchor>().Run();
	}

	private void Update()
	{
		if (!GlobalInf.mapMode)
		{
			if (playerCtl.curState == PLAYERSTATE.CAR)
			{
				if (playerCarParkLabel.gameObject.active)
				{
					playerCarParkLabel.gameObject.SetActiveRecursively(false);
				}
				if (GameController.instance.curMode.mode != GAMEMODE.DRIVING0)
				{
					if (playerCtl.car.currentSpeed > 20f)
					{
						getOnCarBtnFlag = false;
					}
					else if (!delayEnableGetOnCarBtn)
					{
						getOnCarBtnFlag = true;
					}
				}
				if (preFireTarget.gameObject.active)
				{
					preFireTarget.gameObject.SetActiveRecursively(false);
				}
				if (fireTarget.gameObject.active)
				{
					fireTarget.gameObject.SetActiveRecursively(false);
				}
			}
			else
			{
				if (player_ShotPressFlag)
				{
					playerCtl.OnPlayer_ShotBtnPress();
				}
				if (playerCtl.fireTarget != null)
				{
					if (!fireTarget.gameObject.active)
					{
						fireTarget.gameObject.SetActiveRecursively(true);
					}
					if (preFireTarget.gameObject.active)
					{
						preFireTarget.gameObject.SetActiveRecursively(false);
					}
					pos = mainCam.WorldToViewportPoint(playerCtl.fireTarget.fireTarget.transform.position);
					fireTarget.transform.localPosition = new Vector3(pos.x * GlobalDefine.screenRatioWidth, pos.y * 480f, 0f);
				}
				else
				{
					if (fireTarget.gameObject.active)
					{
						fireTarget.gameObject.SetActiveRecursively(false);
					}
					if (playerCtl.preFireTarget != null)
					{
						if (!preFireTarget.gameObject.active)
						{
							preFireTarget.gameObject.SetActiveRecursively(true);
						}
						preFireTargetPos = mainCam.WorldToViewportPoint(playerCtl.preFireTarget.fireTarget.transform.position);
						preFireTarget.transform.localPosition = new Vector3(preFireTargetPos.x * GlobalDefine.screenRatioWidth, preFireTargetPos.y * 480f, 0f);
						if (preFireTargetPos.x < 0f || preFireTargetPos.y < 0f)
						{
							preFireTarget.gameObject.SetActiveRecursively(false);
							PlayerController.instance.preFireTarget = null;
						}
					}
					else if (preFireTarget.gameObject.active)
					{
						preFireTarget.gameObject.SetActiveRecursively(false);
					}
				}
				if (GameController.instance.curMode.mode == GAMEMODE.NORMAL)
				{
					if (CarManage.instance.playerCar != null && PlayerController.instance.curState != PLAYERSTATE.DIE)
					{
						if (!playerCarParkLabel.gameObject.active)
						{
							playerCarParkLabel.gameObject.SetActiveRecursively(true);
						}
						playerCarParkLabel.transform.localPosition = ConvertObjToViewPos(CarManage.instance.playerCar.gameObject.transform.position);
						playerCarDistance.text = string.Empty + (int)Vector3.Distance(CarManage.instance.playerCar.transform.position, PlayerController.instance.transform.position) + "m";
					}
					else if (playerCarParkLabel.gameObject.active)
					{
						playerCarParkLabel.gameObject.SetActiveRecursively(false);
					}
				}
				else if (!GlobalInf.firstOpenGameFlag && playerCarParkLabel.gameObject.active)
				{
					playerCarParkLabel.gameObject.SetActiveRecursively(false);
				}
			}
			if (enableLocateLabelFlag)
			{
				for (int i = 0; i < locateTargetList.Count; i++)
				{
					locateLabelList[i].transform.localPosition = ConvertObjToViewPos(locateTargetList[i].transform.position + Vector3.up);
					locateDistanceList[i].text = string.Empty + (int)Vector3.Distance(locateTargetList[i].transform.position, PlayerController.instance.transform.position) + "m";
				}
			}
		}
		else
		{
			if (fireTarget.gameObject.active)
			{
				fireTarget.gameObject.SetActiveRecursively(false);
			}
			if (preFireTarget.gameObject.active)
			{
				preFireTarget.gameObject.SetActiveRecursively(false);
			}
			if (playerCarParkLabel.gameObject.active)
			{
				playerCarParkLabel.gameObject.SetActiveRecursively(false);
			}
		}
		if (Time.timeScale == 0f)
		{
			if (fireTarget.gameObject.active)
			{
				fireTarget.gameObject.SetActiveRecursively(false);
			}
			if (preFireTarget.gameObject.active)
			{
				preFireTarget.gameObject.SetActiveRecursively(false);
			}
			if (playerCarParkLabel.gameObject.active)
			{
				playerCarParkLabel.gameObject.SetActiveRecursively(false);
			}
			if (playerCarParkLabel.gameObject.active)
			{
				playerCarParkLabel.gameObject.SetActiveRecursively(false);
			}
		}
		if (GameController.instance.curGameMode == GAMEMODE.GUNKILLING || GameController.instance.curGameMode == GAMEMODE.SLOT || GameController.instance.curGameMode == GAMEMODE.SKILLDRIVING || rolloverFlag || Time.timeScale == 0f)
		{
			getOnCarBtnFlag = false;
		}
		if (GameController.instance.curGameMode == GAMEMODE.DRIVING0)
		{
			if (CarManage.instance.playerCar.carType == CARTYPE.MOTOR)
			{
				if (!(PlayerController.instance.car == null))
				{
					getOnCarBtnFlag = false;
				}
			}
			else
			{
				getOnCarBtnFlag = false;
			}
		}
		if (PlayerController.instance.curState == PLAYERSTATE.RAGDOLL)
		{
			getOnCarBtnFlag = false;
		}
		if (getOnCarBtnFlag)
		{
			if (!ctl_GetOnCarBtn.gameObject.active)
			{
				ctl_GetOnCarBtn.gameObject.SetActiveRecursively(true);
			}
		}
		else if (ctl_GetOnCarBtn.gameObject.active)
		{
			ctl_GetOnCarBtn.gameObject.SetActiveRecursively(false);
		}
		fingerFlag = false;
		if (Controller.instance.unLockLevelFlag && GameController.instance.curGameMode == GAMEMODE.NORMAL && minimapController.unLockStape < 2)
		{
			fingerFlag = true;
			if (!miniMapFinger.gameObject.active)
			{
				miniMapFinger.enabled = false;
			}
		}
		if (healthKitTutorialFlag || toolKitTutorialFlag)
		{
			fingerFlag = true;
			if (!miniMapFinger.gameObject.active)
			{
				miniMapFinger.enabled = true;
			}
		}
		if (fingerFlag)
		{
			if (!miniMapFinger.gameObject.active)
			{
				miniMapFinger.gameObject.SetActiveRecursively(true);
			}
		}
		else if (miniMapFinger.gameObject.active)
		{
			miniMapFinger.gameObject.SetActiveRecursively(false);
		}
		if (taskLabelTimeCountFlag)
		{
			taskLabelTimeCount += Time.deltaTime;
			if (taskLabelTimeCount >= 0.5f)
			{
				taskLabelTimeCountFlag = false;
				DisableTaskLabel();
			}
		}
	}

	public void EnableOutOffAmmo(string word)
	{
		if (!outOfAmmoLabel.gameObject.active)
		{
			outOfAmmoLabel.gameObject.SetActiveRecursively(true);
			outOfAmmoLabelText.text = word;
		}
	}

	public void DisableOutOffAmmo()
	{
		if (outOfAmmoLabel.gameObject.active)
		{
			outOfAmmoLabel.gameObject.SetActiveRecursively(false);
		}
	}

	public void EnableLocateLabel(GameObject[] target)
	{
		locateTargetList.Clear();
		for (int i = 0; i < target.Length; i++)
		{
			locateTargetList.Add(target[i]);
			locateLabelList[i].gameObject.SetActiveRecursively(true);
		}
		enableLocateLabelFlag = true;
	}

	public void EnableLocateLabel(GameObject target)
	{
		tempUI.SetActive(true);
		locateTargetList.Clear();
		locateTargetList.Add(target);
		locateLabelList[0].gameObject.SetActiveRecursively(true);
		enableLocateLabelFlag = true;
	}

	public void DisableLocateLabel()
	{
		locateTargetList.Clear();
		for (int i = 0; i < locateLabelList.Length; i++)
		{
			locateLabelList[i].gameObject.SetActiveRecursively(false);
		}
		enableLocateLabelFlag = false;
	}

	public Vector3 ConvertObjToViewPos(Vector3 obj)
	{
		Vector3 vector = mainCam.WorldToViewportPoint(obj);
		if (vector.z > 0f)
		{
			if (vector.x * GlobalDefine.screenRatioWidth < 25f)
			{
				xPos = 40f;
			}
			else if (GlobalDefine.screenRatioWidth - vector.x * GlobalDefine.screenRatioWidth < 25f)
			{
				xPos = GlobalDefine.screenRatioWidth - 40f;
			}
			else
			{
				xPos = vector.x * GlobalDefine.screenRatioWidth;
			}
			if (vector.y * 480f < 30f)
			{
				yPos = 30f;
			}
			else if (480f * (1f - vector.y) < 50f)
			{
				yPos = 430f;
			}
			else
			{
				yPos = vector.y * 480f;
			}
			if (yPos <= 30f)
			{
				if (xPos < GlobalDefine.screenRatioWidth / 2f)
				{
					xPos = 40f;
				}
				else
				{
					xPos = GlobalDefine.screenRatioWidth - 40f;
				}
			}
			return new Vector3(xPos, yPos, 0f);
		}
		if ((0f - vector.x) * GlobalDefine.screenRatioWidth < 25f)
		{
			xPos = 40f;
		}
		else if (GlobalDefine.screenRatioWidth + vector.x * GlobalDefine.screenRatioWidth < 25f)
		{
			xPos = GlobalDefine.screenRatioWidth - 40f;
		}
		else
		{
			xPos = (0f - vector.x) * GlobalDefine.screenRatioWidth;
		}
		yPos = 30f;
		if (yPos <= 30f)
		{
			if (xPos < GlobalDefine.screenRatioWidth / 2f)
			{
				xPos = 40f;
			}
			else
			{
				xPos = GlobalDefine.screenRatioWidth - 40f;
			}
		}
		return new Vector3(xPos, yPos, 0f);
	}

	public void OnChangCarUI()
	{
		if (!(PlayerController.instance.car == null))
		{
			PlayerController.instance.car.accBtnPressFlag = false;
			PlayerController.instance.car.brakeBtnPressFlag = false;
			PlayerController.instance.car.leftArrowPressFlag = false;
			PlayerController.instance.car.rightArrowPressFlag = false;
			carUI.gameObject.SetActiveRecursively(true);
			playerUI.gameObject.SetActiveRecursively(false);
			SetTopRightLabel(playerCtl.curState);
			if (GlobalInf.carCtrlType == CARCTRLTYPE.BUTTON)
			{
				car_AccBtn.transform.localPosition = new Vector3(0.4f * GlobalDefine.screenRatioWidth, -76.799995f, 0f);
				car_BrakeBtn.transform.localPosition = new Vector3(0.29f * GlobalDefine.screenRatioWidth, -168f, 0f);
				car_TurnLeftBtn.gameObject.SetActiveRecursively(true);
				car_TurnRightBtn.gameObject.SetActiveRecursively(true);
			}
			else
			{
				car_TurnLeftBtn.gameObject.SetActiveRecursively(false);
				car_TurnRightBtn.gameObject.SetActiveRecursively(false);
				car_AccBtn.transform.localPosition = new Vector3(0.32f * GlobalDefine.screenRatioWidth, -120f, 0f);
				car_BrakeBtn.transform.localPosition = new Vector3(-0.32f * GlobalDefine.screenRatioWidth, -120f, 0f);
			}
			if (PlayerController.instance.car.carType == CARTYPE.MOTOR)
			{
				playerUI.SetActive(true);
				playerHealthUI.gameObject.SetActiveRecursively(true);
				carHealthUI.gameObject.SetActiveRecursively(false);
				OnInitHealthKitNum();
			}
			else
			{
				OnInitToolKitNum();
			}
		}
	}

	private void OnApplicationFocus(bool isFocuse)
	{
		if (!isFocuse)
		{
			if (pauseBtn.gameObject.active)
			{
				OnClickPauseBtn(null);
			}
			Platform.SetNotification();
		}
		else
		{
			Platform.internalCancelNotification();
		}
	}

	public void OnChangePlayerUI()
	{
		if (GameController.instance.curGameMode == GAMEMODE.SLOT)
		{
			return;
		}
		if (PlayerController.instance != null)
		{
			if (PlayerController.instance.curState == PLAYERSTATE.DIE)
			{
				return;
			}
			PlayerController.instance.moveCtl.joyStick.position = new Vector2(0f, 0f);
			PlayerController.instance.moveCtl.movingDirection = Vector3.zero;
		}
		player_ShotPressFlag = false;
		carUI.gameObject.SetActiveRecursively(false);
		playerUI.gameObject.SetActiveRecursively(true);
		playerCtl.changingTheCarFlag = false;
		SetTopRightLabel(playerCtl.curState);
		ChangePlayerFireBtn(playerCtl.curState);
		OnInitHealthKitNum();
	}

	public void ChangePlayerFireBtn(PLAYERSTATE cState)
	{
		if (cState == PLAYERSTATE.NORMAL || cState == PLAYERSTATE.RAGDOLL)
		{
			player_ShotBtn.gameObject.SetActiveRecursively(false);
			reloadBtn.gameObject.SetActiveRecursively(false);
			player_BoxingBtn.gameObject.SetActiveRecursively(true);
		}
		else
		{
			player_BoxingBtn.gameObject.SetActiveRecursively(false);
			reloadBtn.gameObject.SetActiveRecursively(true);
			player_ShotBtn.gameObject.SetActiveRecursively(true);
		}
	}

	public void OnEnableGetOnCarBtn()
	{
		getOnCarBtnFlag = true;
	}

	public void OnDisableGetOnCarBtn()
	{
		getOnCarBtnFlag = false;
	}

	public void OnClickBulletVideoBtn(GameObject btn)
	{
		if (PlayerController.instance.curState == PLAYERSTATE.HANDGUN)
		{
			GlobalInf.videoType = VIDEOTYPE.HANDGUNBULLET;
		}
		else if (PlayerController.instance.curState == PLAYERSTATE.MACHINEGUN)
		{
			GlobalInf.videoType = VIDEOTYPE.MACHINEGUNBULLET;
		}
		Platform.internalShowUnityAds();
		Platform.flurryEvent_onClickUnityAddBulletAds();
	}

	private void OnDestroy()
	{
		instance = null;
	}
}
