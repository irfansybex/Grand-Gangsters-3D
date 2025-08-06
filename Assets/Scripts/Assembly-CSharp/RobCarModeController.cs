using System;
using UnityEngine;

public class RobCarModeController : GameModeController
{
	public bool countTimeFlag;

	public float restTime;

	public float[] setTime;

	public GameObject finishLabel;

	public Vector3[] startPos;

	public Vector3[] endPos;

	public bool failFlag;

	public bool restartFlag;

	public bool failUIFlag;

	public MiniMapController minimapController;

	public bool beforeGameFlag;

	public float beforeGameCount;

	public CARTYPE targetCarType;

	public int[] targetCarMaxNum;

	public RobCarUIRoot robCarUI;

	public bool robCarTutorialFlag;

	public int taskIndex;

	public int robCarTutorialState;

	public bool leftFlag;

	private void Start()
	{
		PlayerController instance = PlayerController.instance;
		instance.onPlayerDie = (PlayerController.OnPlayerDie)Delegate.Combine(instance.onPlayerDie, new PlayerController.OnPlayerDie(OnPlayerDie));
		PlayerController instance2 = PlayerController.instance;
		instance2.resetSence = (PlayerController.ResetSence)Delegate.Combine(instance2.resetSence, new PlayerController.ResetSence(FailGame));
	}

	public override void Reset(int index)
	{
		taskIndex = index;
		restTime = setTime[index];
		if (restartFlag)
		{
			CitySenceController.instance.ClearAI();
			robCarUI.gameObject.SetActiveRecursively(true);
		}
		else
		{
			targetCarType = (CARTYPE)UnityEngine.Random.Range(0, 5);
			robCarUI = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/robCarUI"))).GetComponent<RobCarUIRoot>();
			robCarUI.targetCarPic.spriteName = "Car" + (int)targetCarType;
			robCarUI.transform.parent = GameUIController.instance.rootContainner.transform;
			robCarUI.transform.localPosition = Vector3.zero;
			robCarUI.transform.localRotation = Quaternion.identity;
			robCarUI.transform.localScale = Vector3.one;
		}
		PlayerController.instance.transform.position = TaskLabelController.instance.robbingCarTaskInfo[index].startPos;
		PlayerController.instance.transform.eulerAngles = TaskLabelController.instance.robbingCarTaskInfo[index].startAngle;
		PlayerController.instance.ResetPlayer();
		PoliceLevelCtl.ResetPoliceLevel();
		finishLabel.SetActiveRecursively(true);
		finishLabel.transform.position = endPos[index];
		minimapController.mapDrawPath.ClearLine();
		Invoke("LateInvoke", 0.5f);
		failFlag = false;
		beforeGameFlag = true;
		beforeGameCount = 4.5f;
		GameUIController.instance.timeCountFlag = true;
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(true);
		GameUIController.instance.taskBoardController.DisableNum();
		GameUIController.instance.taskBoardController.beforeGameFlag = true;
		PlayerController.instance.moveCtl.disableMoveFlag = true;
		GameUIController.instance.taskBoardController.bottomLabel.text = "Rob the vehicle that shown in the picture";
		GameUIController.instance.taskBoardController.topBottom.width = (int)(470f * GlobalDefine.screenWidthFit);
		failUIFlag = false;
		CarManage.instance.ResetPlayerCarPos();
		restartFlag = false;
		GameUIController.instance.taskBoardController.SetTime(1f);
		GameUIController.instance.taskBoardController.SetTime(0f);
		UIEventListener ctl_GetOnCarBtn = GameUIController.instance.ctl_GetOnCarBtn;
		ctl_GetOnCarBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(ctl_GetOnCarBtn.onClick, new UIEventListener.VoidDelegate(CheckTitle));
		if (GlobalInf.newUserFlag)
		{
			if (PlayerPrefs.GetInt("RobCarTutorial", 0) == 0)
			{
				robCarTutorialFlag = true;
			}
			else
			{
				robCarTutorialFlag = false;
			}
		}
		if (robCarTutorialFlag)
		{
			robCarTutorialState = 0;
			robCarUI.tutorialWord.text = "In this task you're asked to rob a car.\nPlease move to the street first.";
			robCarUI.tutorialBack.width = (int)(GlobalDefine.screenWidthFit * 480f);
			robCarUI.tutorialBack.height = 80;
			GameUIController.instance.taskBoardController.beforeGameFlag = false;
			PlayerController.instance.moveCtl.disableMoveFlag = false;
			targetCarType = CARTYPE.SPORTSCAR;
			robCarUI.targetCarPic.spriteName = "Car" + (int)targetCarType;
			CarManage.instance.playerCar.gameObject.SetActiveRecursively(false);
			CarManage.instance.playerCar.transform.position = new Vector3(500f, 0f, 500f);
		}
		GameUIController.instance.minimapController.DisablePlayerCarPos();
	}

	public void CheckTitle(GameObject btn)
	{
		Invoke("DelayCheckTitle", 1.2f);
		if (robCarTutorialFlag)
		{
			robCarUI.pressTips.gameObject.SetActiveRecursively(false);
		}
	}

	public void DelayCheckTitle()
	{
		if (PlayerController.instance.curState == PLAYERSTATE.CAR)
		{
			if (PlayerController.instance.car.carType == targetCarType)
			{
				GameUIController.instance.taskBoardController.bottomLabel.text = "Drive the vehicle to the  destination";
				minimapController.mapDrawPath.targetFlag = false;
				minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(endPos[taskIndex]) + new Vector3(1000f, 480f, 0f));
				GameUIController.instance.EnableLocateLabel(finishLabel);
				robCarUI.tweenScale.enabled = false;
				robCarUI.rightPic.gameObject.SetActiveRecursively(true);
				robCarUI.wrongPic.gameObject.SetActiveRecursively(false);
			}
			else
			{
				robCarUI.tweenScale.enabled = true;
				GameUIController.instance.taskBoardController.bottomLabel.text = "Rob the vehicle that shown in the picture";
				robCarUI.rightPic.gameObject.SetActiveRecursively(false);
				robCarUI.wrongPic.gameObject.SetActiveRecursively(true);
				minimapController.DisableTargetPos();
				GameUIController.instance.DisableLocateLabel();
			}
		}
		else
		{
			robCarUI.tweenScale.enabled = true;
			GameUIController.instance.taskBoardController.bottomLabel.text = "Rob the vehicle that shown in the picture";
			robCarUI.rightPic.gameObject.SetActiveRecursively(false);
			robCarUI.wrongPic.gameObject.SetActiveRecursively(false);
			minimapController.DisableTargetPos();
			GameUIController.instance.DisableLocateLabel();
		}
		if (!robCarTutorialFlag)
		{
			return;
		}
		if (PlayerController.instance.curState == PLAYERSTATE.CAR)
		{
			if (PlayerController.instance.car.carType == targetCarType)
			{
				robCarUI.tutorialWord.text = "Good, it's the car!\nNow drive it to the destination";
				robCarUI.tutorialBack.width = (int)(GlobalDefine.screenWidthFit * 400f);
				robCarUI.tutorialBack.height = 80;
				robCarTutorialState = 2;
				robCarUI.pressTips.gameObject.SetActiveRecursively(false);
				robCarUI.rightPic.gameObject.SetActiveRecursively(true);
				robCarUI.wrongPic.gameObject.SetActiveRecursively(false);
				robCarUI.uiMask.gameObject.SetActiveRecursively(false);
			}
			else
			{
				robCarUI.tutorialWord.text = "Sorry, this is not\n\"[00F4FF]\"THE CAR SHOWN ON THE PICTURE\"[FFFFFF]\".\nPlease try another one.";
				robCarUI.tutorialBack.width = (int)(GlobalDefine.screenWidthFit * 480f);
				robCarUI.tutorialBack.height = 120;
				robCarTutorialState = 2;
				robCarUI.pressTips.gameObject.SetActiveRecursively(true);
				robCarUI.rightPic.gameObject.SetActiveRecursively(false);
				robCarUI.wrongPic.gameObject.SetActiveRecursively(true);
				robCarUI.uiMask.gameObject.SetActiveRecursively(true);
				GameUIController.instance.ctl_GetOnCarBtn.gameObject.GetComponent<UISprite>().depth = 6;
			}
		}
		else
		{
			robCarTutorialState = 1;
			robCarUI.tutorialWord.text = "Now look at the \"[00F4FF]\"PICTURE\"[FFFFFF]\" on the left,\nand rob the car that shown.";
			robCarUI.tutorialBack.width = (int)(GlobalDefine.screenWidthFit * 480f);
			robCarUI.tutorialBack.height = 80;
			robCarUI.rightPic.gameObject.SetActiveRecursively(false);
			robCarUI.wrongPic.gameObject.SetActiveRecursively(false);
			robCarUI.uiMask.gameObject.SetActiveRecursively(false);
			GameUIController.instance.ctl_GetOnCarBtn.gameObject.GetComponent<UISprite>().depth = 0;
		}
	}

	public void ResetUI()
	{
		if (robCarTutorialFlag)
		{
			GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
			robCarUI.tutorialRoot.SetActiveRecursively(false);
			robCarUI.tutorialRoot.SetActive(true);
			robCarUI.tutorialWord.transform.parent.gameObject.SetActive(true);
			robCarUI.tutorialWord.gameObject.SetActiveRecursively(true);
			robCarUI.tutorialBack.gameObject.SetActiveRecursively(true);
			if (robCarTutorialState == 0)
			{
				robCarUI.blockCube.gameObject.SetActiveRecursively(true);
				robCarUI.tutorialCollider.gameObject.SetActiveRecursively(true);
				robCarUI.pressTips.SetActiveRecursively(false);
				robCarUI.uiMask.SetActiveRecursively(false);
				robCarUI.handTip.gameObject.SetActiveRecursively(false);
				robCarUI.rightPic.gameObject.SetActiveRecursively(false);
				robCarUI.wrongPic.gameObject.SetActiveRecursively(false);
			}
			else
			{
				DelayCheckTitle();
				robCarUI.blockCube.gameObject.SetActiveRecursively(true);
				robCarUI.tutorialCollider.gameObject.SetActiveRecursively(false);
			}
		}
		else
		{
			robCarUI.tutorialRoot.SetActiveRecursively(false);
			robCarUI.uiMask.gameObject.SetActiveRecursively(false);
			robCarUI.blockCube.gameObject.SetActiveRecursively(false);
			DelayCheckTitle();
		}
	}

	public void LateInvoke()
	{
		BlackScreen.instance.TurnOnScreen();
		GameUIController.instance.InitUI();
		if (robCarTutorialFlag)
		{
			GameUIController.instance.EnableLocateLabel(robCarUI.tutorialCollider.gameObject);
			robCarUI.pressTips.gameObject.SetActiveRecursively(false);
			robCarUI.handTip.gameObject.SetActiveRecursively(false);
			GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
		}
		else
		{
			robCarUI.tutorialRoot.SetActiveRecursively(false);
			robCarUI.tutorialCollider.gameObject.SetActiveRecursively(false);
			robCarUI.blockCube.gameObject.SetActiveRecursively(false);
		}
		robCarUI.rightPic.gameObject.SetActiveRecursively(false);
		robCarUI.wrongPic.gameObject.SetActiveRecursively(false);
		robCarUI.uiMask.gameObject.SetActiveRecursively(false);
	}

	public void OnEnterCar()
	{
		robCarUI.tutorialWord.text = "Now look at the \"[00F4FF]\"PICTURE\"[FFFFFF]\" on the left,\nand rob the car that shown.";
		robCarUI.tutorialBack.width = (int)(GlobalDefine.screenWidthFit * 480f);
		robCarUI.tutorialBack.height = 80;
		AICarPoolController.instance.ForceGetRightCar(new Vector3(-515f, 0f, -254.3f), roadInfoListNew.instance.roadList[203].roadPointList[2], roadInfoListNew.instance.roadList[203].roadPointList[3], 0, AICarPoolController.instance.car0DisableList, 0);
		AICarPoolController.instance.ForceGetRightCar(new Vector3(-530f, 0f, -254.3f), roadInfoListNew.instance.roadList[203].roadPointList[2], roadInfoListNew.instance.roadList[203].roadPointList[3], 0, AICarPoolController.instance.car3DisableList, 3);
		GameUIController.instance.EnableLocateLabel(AICarPoolController.instance.enableList[0].gameObject);
		robCarTutorialState = 1;
		robCarUI.pressTips.transform.position = GameUIController.instance.ctl_GetOnCarBtn.transform.position;
	}

	public void CheckTargetPos()
	{
		if (GameUIController.instance.locateLabelList[0].transform.localPosition.x <= 50f)
		{
			if (!robCarUI.handTip.gameObject.active)
			{
				robCarUI.handTip.gameObject.SetActiveRecursively(true);
				robCarUI.handTip.from = new Vector3(-50f * GlobalDefine.screenWidthFit, -50f, 0f);
				robCarUI.handTip.to = new Vector3(-200f * GlobalDefine.screenWidthFit, -50f, 0f);
				robCarUI.handTip.ResetToBeginning();
				robCarUI.handTipTweenAlph.ResetToBeginning();
				robCarUI.handTip.PlayForward();
				robCarUI.handTipTweenAlph.PlayForward();
			}
			if (!leftFlag)
			{
				leftFlag = true;
				robCarUI.handTip.gameObject.SetActiveRecursively(true);
				robCarUI.handTip.from = new Vector3(-50f * GlobalDefine.screenWidthFit, -50f, 0f);
				robCarUI.handTip.to = new Vector3(-200f * GlobalDefine.screenWidthFit, -50f, 0f);
				robCarUI.handTip.ResetToBeginning();
				robCarUI.handTipTweenAlph.ResetToBeginning();
				robCarUI.handTip.PlayForward();
				robCarUI.handTipTweenAlph.PlayForward();
			}
		}
		else if (GameUIController.instance.locateLabelList[0].transform.localPosition.x >= GlobalDefine.screenRatioWidth - 50f)
		{
			if (!robCarUI.handTip.gameObject.active)
			{
				robCarUI.handTip.gameObject.SetActiveRecursively(true);
				robCarUI.handTip.from = new Vector3(50f * GlobalDefine.screenWidthFit, -50f, 0f);
				robCarUI.handTip.to = new Vector3(200f * GlobalDefine.screenWidthFit, -50f, 0f);
				robCarUI.handTip.ResetToBeginning();
				robCarUI.handTipTweenAlph.ResetToBeginning();
				robCarUI.handTip.PlayForward();
				robCarUI.handTipTweenAlph.PlayForward();
			}
			if (leftFlag)
			{
				leftFlag = false;
				robCarUI.handTip.gameObject.SetActiveRecursively(true);
				robCarUI.handTip.from = new Vector3(50f * GlobalDefine.screenWidthFit, -50f, 0f);
				robCarUI.handTip.to = new Vector3(200f * GlobalDefine.screenWidthFit, -50f, 0f);
				robCarUI.handTip.ResetToBeginning();
				robCarUI.handTipTweenAlph.ResetToBeginning();
				robCarUI.handTip.PlayForward();
				robCarUI.handTipTweenAlph.PlayForward();
			}
		}
		else if (robCarUI.handTip.gameObject.active)
		{
			robCarUI.handTip.gameObject.SetActiveRecursively(false);
		}
	}

	public override void MyUpdate()
	{
		if (robCarTutorialFlag)
		{
			if (robCarTutorialState == 0)
			{
				CheckTargetPos();
			}
			else if (robCarTutorialState == 1)
			{
				if (GameUIController.instance.ctl_GetOnCarBtn.gameObject.active)
				{
					if (!robCarUI.pressTips.gameObject.active && CarManage.instance.nearestCar.carType == targetCarType)
					{
						robCarUI.pressTips.gameObject.SetActiveRecursively(true);
					}
				}
				else if (robCarUI.pressTips.gameObject.active)
				{
					robCarUI.pressTips.gameObject.SetActiveRecursively(false);
				}
			}
			else if (robCarTutorialState == 2)
			{
				GameController.instance.normalMode.MyUpdate();
			}
			return;
		}
		base.MyUpdate();
		GameController.instance.normalMode.MyUpdate();
		if (beforeGameFlag)
		{
			beforeGameCount -= Time.deltaTime;
			GameUIController.instance.taskBoardController.SetBeforeGameCount((int)(beforeGameCount - 0.1f));
			if (beforeGameCount <= 0f)
			{
				beforeGameFlag = false;
				GameUIController.instance.taskBoardController.beforeGameFlag = false;
				PlayerController.instance.moveCtl.disableMoveFlag = false;
				GameUIController.instance.taskBoardController.baskMask.SetActiveRecursively(false);
				GameUIController.instance.taskBoardController.tweenAlph[0].gameObject.SetActiveRecursively(false);
			}
		}
		else if (restTime > 0f)
		{
			restTime -= Time.deltaTime;
			GameUIController.instance.taskBoardController.SetTime(restTime);
			if (restTime < 0f && !failUIFlag)
			{
				failUIFlag = true;
				failFlag = true;
				FailGame();
			}
		}
	}

	public void OnPlayerDie()
	{
		failFlag = true;
	}

	public void FailGame()
	{
		if (GameController.instance.curGameMode == GAMEMODE.ROBCAR)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/TaskEndUI")) as GameObject;
			TaskEndUIController component = gameObject.GetComponent<TaskEndUIController>();
			component.Reset(failFlag, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, 0, GameUIController.instance.rewardIndex);
			gameObject.transform.parent = GameUIController.instance.uiRoot.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localRotation = Quaternion.identity;
			Time.timeScale = 0f;
			GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
			robCarUI.gameObject.SetActiveRecursively(false);
		}
	}

	public override void Exit()
	{
		base.Exit();
		CarManage.instance.playerCar.gameObject.SetActiveRecursively(true);
		finishLabel.SetActiveRecursively(false);
		minimapController.mapDrawPath.ClearLine();
		PlayerController.instance.moveCtl.disableMoveFlag = false;
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
		GameUIController.instance.timeCountFlag = false;
		if (PlayerController.instance.curState == PLAYERSTATE.CAR)
		{
			PlayerController.instance.car.OnCloseDoor();
			PlayerController.instance.car.OnDisableCar();
			PlayerController.instance.cam.OnChangeTarget(false);
			PlayerController.instance.GetOffCarDone();
		}
		if (!restartFlag)
		{
			PlayerController.instance.transform.position = GameUIController.instance.curTaskInfo.exitPlayerPos;
			PlayerController.instance.transform.eulerAngles = GameUIController.instance.curTaskInfo.exitPlayerAngle;
			PlayerController.instance.moveCtl.playerFaceAngle = PlayerController.instance.transform.eulerAngles.y;
			CarManage.instance.playerCar.ResetPlayerCar();
			CarManage.instance.ResetPlayerCarPos();
			GameUIController.instance.minimapController.EnablePlayerCarPos(CarManage.instance.playerCar.transform.position);
			UnityEngine.Object.Destroy(robCarUI.gameObject);
			robCarUI = null;
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}
		UIEventListener ctl_GetOnCarBtn = GameUIController.instance.ctl_GetOnCarBtn;
		ctl_GetOnCarBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(ctl_GetOnCarBtn.onClick, new UIEventListener.VoidDelegate(CheckTitle));
	}
}
