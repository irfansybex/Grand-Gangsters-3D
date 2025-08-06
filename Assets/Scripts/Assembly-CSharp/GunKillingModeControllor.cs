using System;
using UnityEngine;

public class GunKillingModeControllor : GameModeController
{
	public bool countTimeFlag;

	public float restTime;

	public float setTime;

	public int killNum;

	public float[] gunKillingMissionTime;

	public bool failFlag;

	public bool beforeGameFlag;

	public float beforeGameCount;

	private void Start()
	{
		PlayerController instance = PlayerController.instance;
		instance.resetSence = (PlayerController.ResetSence)Delegate.Combine(instance.resetSence, new PlayerController.ResetSence(FailGame));
		PlayerController instance2 = PlayerController.instance;
		instance2.onPlayerDie = (PlayerController.OnPlayerDie)Delegate.Combine(instance2.onPlayerDie, new PlayerController.OnPlayerDie(OnPlayerDie));
	}

	public override void Reset(int index)
	{
		restTime = gunKillingMissionTime[index];
		PoliceLevelCtl.ResetPoliceLevel();
		PlayerController.instance.transform.position = TaskLabelController.instance.gunKillingTaskInfo[index].startPos;
		PlayerController.instance.ResetPlayer();
		Invoke("LateInvoke", 0.5f);
		failFlag = false;
		GameController.instance.normalMode.npcProduceTime = 2f;
		killNum = 0;
		beforeGameFlag = true;
		beforeGameCount = 4.5f;
		GameUIController.instance.timeCountFlag = true;
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(true);
		GameUIController.instance.taskBoardController.beforeGameFlag = true;
		PlayerController.instance.moveCtl.disableMoveFlag = true;
		GameUIController.instance.taskBoardController.bottomLabel.text = "Kill as many citizens as possible!";
		GameUIController.instance.taskBoardController.topBottom.width = (int)(380f * GlobalDefine.screenWidthFit);
		GameUIController.instance.minimapController.mapDrawPath.ClearLine();
		GameUIController.instance.taskBoardController.SetTime(1f);
		GameUIController.instance.taskBoardController.SetTime(0f);
		PlayerController.instance.ChangeState(PLAYERSTATE.HANDGUN);
	}

	public void LateInvoke()
	{
		BlackScreen.instance.TurnOnScreen();
		GameUIController.instance.InitUI();
	}

	public override void MyUpdate()
	{
		if (failFlag)
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
				PlayerController.instance.moveCtl.disableMoveFlag = false;
				GameUIController.instance.taskBoardController.baskMask.SetActiveRecursively(false);
				GameUIController.instance.taskBoardController.tweenAlph[0].gameObject.SetActiveRecursively(false);
			}
		}
		else if (restTime > 0f)
		{
			restTime -= Time.deltaTime;
			GameUIController.instance.taskBoardController.SetTime(restTime);
			if (restTime < 0f)
			{
				EndMission(false);
			}
		}
	}

	private void OnPlayerDie()
	{
		if (GameController.instance.curGameMode == GAMEMODE.GUNKILLING)
		{
			failFlag = true;
		}
	}

	private void FailGame()
	{
		if (GameController.instance.curGameMode == GAMEMODE.GUNKILLING)
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
	}

	public override void Exit()
	{
		base.Exit();
		GameController.instance.normalMode.npcProduceTime = 5f;
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
		GameUIController.instance.timeCountFlag = false;
		PlayerController.instance.transform.position = GameUIController.instance.curTaskInfo.exitPlayerPos;
		PlayerController.instance.transform.eulerAngles = GameUIController.instance.curTaskInfo.exitPlayerAngle;
		PlayerController.instance.moveCtl.playerFaceAngle = PlayerController.instance.transform.eulerAngles.y;
		CarManage.instance.playerCar.ResetPlayerCar();
	}
}
