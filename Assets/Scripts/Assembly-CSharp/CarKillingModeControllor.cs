using System;
using UnityEngine;

public class CarKillingModeControllor : GameModeController
{
	public bool countTimeFlag;

	public float restTime;

	public float setTime;

	public int killNum;

	public float[] carKillingMissionTime;

	public bool failFlag;

	public bool restartFlag;

	public CarController playerCar;

	public bool beforeGameFlag;

	public float beforeGameCount;

	public float enterCarHealthVal;

	public bool endGameFlag;

	private void Start()
	{
		PlayerController instance = PlayerController.instance;
		instance.resetSence = (PlayerController.ResetSence)Delegate.Combine(instance.resetSence, new PlayerController.ResetSence(FailGame));
		PlayerController instance2 = PlayerController.instance;
		instance2.onPlayerDie = (PlayerController.OnPlayerDie)Delegate.Combine(instance2.onPlayerDie, new PlayerController.OnPlayerDie(OnPlayerDie));
	}

	public override void Reset(int index)
	{
		endGameFlag = false;
		restTime = carKillingMissionTime[index];
		PoliceLevelCtl.ResetPoliceLevel();
		Invoke("LateInvoke", 0.5f);
		GameController.instance.normalMode.npcProduceTime = 2f;
		PlayerController.instance.ResetPlayer();
		ResetPlayerCar(index);
		enterCarHealthVal = CarManage.instance.playerCar.carHealth.healthVal;
		if (restartFlag)
		{
			if (!failFlag)
			{
			}
			restartFlag = false;
		}
		failFlag = false;
		killNum = 0;
		beforeGameFlag = true;
		beforeGameCount = 4.5f;
		GameUIController.instance.disableCarBtnFlag = true;
		GameUIController.instance.taskBoardController.beforeGameFlag = true;
		GameUIController.instance.taskBoardController.bottomLabel.text = "Kill as many citizens as possible!";
		GameUIController.instance.taskBoardController.topBottom.width = (int)(380f * GlobalDefine.screenWidthFit);
		GameUIController.instance.timeCountFlag = true;
		CarManage.instance.playerCar.carHealth.healthVal = enterCarHealthVal;
		if (enterCarHealthVal > 50f)
		{
			TempObjControllor.instance.RecycleSmoke();
		}
		else
		{
			CarManage.instance.playerCar.Damage(0f);
		}
		GameUIController.instance.minimapController.mapDrawPath.ClearLine();
		GameUIController.instance.taskBoardController.SetTime(1f);
		GameUIController.instance.taskBoardController.SetTime(0f);
	}

	public void LateInvoke()
	{
		BlackScreen.instance.TurnOnScreen();
		GameUIController.instance.InitUI();
	}

	public override void MyUpdate()
	{
		if (endGameFlag || failFlag)
		{
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
				GameUIController.instance.disableCarBtnFlag = false;
				GameUIController.instance.taskBoardController.baskMask.SetActiveRecursively(false);
				GameUIController.instance.taskBoardController.tweenAlph[0].gameObject.SetActiveRecursively(false);
			}
		}
		else if (restTime > 0f)
		{
			restTime -= Time.deltaTime;
			GameUIController.instance.taskBoardController.SetTime(restTime);
			if (restTime <= 0f)
			{
				EndMission(false);
			}
		}
	}

	public void ResetPlayerCar(int index)
	{
		CarManage.instance.playerCar.transform.position = TaskLabelController.instance.carKillingTaskInfo[index].startPos;
		CarManage.instance.playerCar.transform.eulerAngles = TaskLabelController.instance.carKillingTaskInfo[index].startAngle;
		CarManage.instance.playerCar.gameObject.SetActiveRecursively(true);
		CarManage.instance.playerCar.AICarCtl.moveFlag = false;
		if (CarManage.instance.playerCar == TempObjControllor.instance.curBrokenCar)
		{
			TempObjControllor.instance.brokenCar.SetActiveRecursively(false);
		}
		PlayerController.instance.transform.position = CarManage.instance.playerCar.getOnPoint.transform.position;
		PlayerController.instance.transform.forward = CarManage.instance.playerCar.getOnPoint.transform.forward;
		PlayerController.instance.GetOnCar(CarManage.instance.playerCar.getOnPoint.transform, CarManage.instance.playerCar);
		PlayerController.instance.cam.transform.forward = CarManage.instance.playerCar.transform.forward;
	}

	private void OnPlayerDie()
	{
		if (GameController.instance.curGameMode == GAMEMODE.CARKILLING)
		{
			failFlag = true;
		}
	}

	private void FailGame()
	{
		if (GameController.instance.curGameMode == GAMEMODE.CARKILLING)
		{
			EndMission(true);
		}
	}

	public void EndMission(bool isFail)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/TaskEndUI")) as GameObject;
		TaskEndUIController component = gameObject.GetComponent<TaskEndUIController>();
		component.Reset(isFail, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, killNum, GameUIController.instance.rewardIndex);
		gameObject.transform.parent = GameUIController.instance.uiRoot.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localRotation = Quaternion.identity;
		GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
		Time.timeScale = 0f;
		endGameFlag = true;
	}

	public override void Exit()
	{
		GameController.instance.normalMode.npcProduceTime = 5f;
		if (!restartFlag)
		{
			if (PlayerController.instance.car != null)
			{
				PlayerController.instance.car.OnCloseDoor();
				PlayerController.instance.car.OnDisableCar();
				PlayerController.instance.cam.OnChangeTarget(false);
				PlayerController.instance.GetOffCarDone();
				PlayerController.instance.transform.eulerAngles = new Vector3(0f, PlayerController.instance.transform.eulerAngles.y, 0f);
			}
			if (!failFlag)
			{
				CarManage.instance.ResetPlayerCarPos();
			}
			CarManage.instance.playerCar.ResetPlayerCar();
			PlayerController.instance.transform.position = GameUIController.instance.curTaskInfo.exitPlayerPos;
			PlayerController.instance.transform.eulerAngles = GameUIController.instance.curTaskInfo.exitPlayerAngle;
			PlayerController.instance.moveCtl.playerFaceAngle = PlayerController.instance.transform.eulerAngles.y;
		}
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
		GameUIController.instance.timeCountFlag = false;
		base.Exit();
	}
}
