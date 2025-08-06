using System;
using UnityEngine;

public class DeliverModeController : GameModeController
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

	private void Start()
	{
		PlayerController instance = PlayerController.instance;
		instance.onPlayerDie = (PlayerController.OnPlayerDie)Delegate.Combine(instance.onPlayerDie, new PlayerController.OnPlayerDie(OnPlayerDie));
		PlayerController instance2 = PlayerController.instance;
		instance2.resetSence = (PlayerController.ResetSence)Delegate.Combine(instance2.resetSence, new PlayerController.ResetSence(FailGame));
	}

	public override void Reset(int index)
	{
		restTime = setTime[index];
		if (restartFlag)
		{
			CitySenceController.instance.ClearAI();
		}
		PlayerController.instance.transform.position = TaskLabelController.instance.deliverTaskInfo[index].startPos;
		PlayerController.instance.ResetPlayer();
		PoliceLevelCtl.ResetPoliceLevel();
		finishLabel.SetActiveRecursively(true);
		finishLabel.transform.position = endPos[index];
		minimapController.mapDrawPath.ClearLine();
		minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(endPos[index]) + new Vector3(1000f, 480f, 0f));
		Invoke("LateInvoke", 0.5f);
		failFlag = false;
		beforeGameFlag = true;
		beforeGameCount = 4.5f;
		GameUIController.instance.timeCountFlag = true;
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(true);
		GameUIController.instance.taskBoardController.DisableNum();
		GameUIController.instance.taskBoardController.beforeGameFlag = true;
		PlayerController.instance.moveCtl.disableMoveFlag = true;
		GameUIController.instance.taskBoardController.bottomLabel.text = "Reach the destination as soon as possible!";
		GameUIController.instance.taskBoardController.topBottom.width = (int)(470f * GlobalDefine.screenWidthFit);
		failUIFlag = false;
		CarManage.instance.ResetPlayerCarPos();
		restartFlag = false;
		GameUIController.instance.taskBoardController.SetTime(1f);
		GameUIController.instance.taskBoardController.SetTime(0f);
	}

	public void LateInvoke()
	{
		BlackScreen.instance.TurnOnScreen();
		GameUIController.instance.InitUI();
		GameUIController.instance.EnableLocateLabel(finishLabel);
	}

	public override void MyUpdate()
	{
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
		if (GameController.instance.curGameMode == GAMEMODE.DELIVER)
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
		}
	}

	public override void Exit()
	{
		base.Exit();
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
		}
	}
}
