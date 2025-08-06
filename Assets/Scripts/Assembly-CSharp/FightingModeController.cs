using System;
using UnityEngine;

public class FightingModeController : GameModeController
{
	public FightingModeInfo fightingModeInfo;

	public int curGroupNum;

	public int curWaveNum;

	public bool genAIFlag;

	public int genNumCount;

	public bool NoAILeftFlag;

	public float genInterval;

	public float countGenInterval;

	public int killNum;

	public float restTime;

	public bool beforeGameFlag;

	public int[] levelTime;

	private float beforeGameCount;

	public bool genDoneFlag;

	public float intervalCount;

	public float intervalExecuteTime = 0.5f;

	public bool failFlag;

	private AIController tempAI;

	private AIInfo tempInfo;

	public AIController targetAI;

	private void Start()
	{
		PlayerController instance = PlayerController.instance;
		instance.resetSence = (PlayerController.ResetSence)Delegate.Combine(instance.resetSence, new PlayerController.ResetSence(FailGame));
		PlayerController instance2 = PlayerController.instance;
		instance2.onPlayerDie = (PlayerController.OnPlayerDie)Delegate.Combine(instance2.onPlayerDie, new PlayerController.OnPlayerDie(OnPlayerDie));
	}

	public override void Reset(int index)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("FightingMode/Fighting" + index)) as GameObject;
		fightingModeInfo = gameObject.GetComponent<FightingModeInfo>();
		InitFightingMode();
		CitySenceController.instance.virtualMapController.GetPlayerLocation(PlayerController.instance.transform);
		Invoke("ResetDelay", 0.5f);
		PlayerController.instance.preFireTarget = null;
		PlayerController.instance.fireTarget = null;
		PlayerController.instance.ResetPlayer();
		killNum = 0;
		beforeGameCount = 4.5f;
		beforeGameFlag = true;
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(true);
		GameUIController.instance.taskBoardController.DisableNum();
		GameUIController.instance.timeCountFlag = true;
		GameUIController.instance.taskBoardController.bottomLabel.text = "Kill the target NPC";
		GameUIController.instance.taskBoardController.topBottom.width = (int)(360f * GlobalDefine.screenWidthFit);
		GameUIController.instance.minimapController.mapDrawPath.ClearLine();
		GameUIController.instance.taskBoardController.beforeGameFlag = true;
		GameUIController.instance.taskBoardController.SetTime(1f);
		GameUIController.instance.taskBoardController.SetTime(0f);
		restTime = levelTime[index];
		PlayerController.instance.ChangeState(PLAYERSTATE.HANDGUN);
		GameUIController.instance.minimapController.DisablePlayerCarPos();
	}

	public void ResetDelay()
	{
		BlackScreen.instance.TurnOnScreen();
		GameUIController.instance.InitUI();
		GameUIController.instance.EnableLocateLabel(targetAI.gameObject);
	}

	public void InitFightingMode()
	{
		PlayerController.instance.transform.position = fightingModeInfo.playerDefaultPos;
		PlayerController.instance.transform.eulerAngles = fightingModeInfo.playerDefaultAngle;
		fightingModeInfo.transform.position = Vector3.zero;
		fightingModeInfo.gameObject.SetActiveRecursively(true);
		InitAI();
		curGroupNum = 0;
		curWaveNum = 0;
		NoAILeftFlag = false;
		genNumCount = 0;
		failFlag = false;
	}

	public void InitAI()
	{
		for (int i = 0; i < fightingModeInfo.npcGroupList.Length; i++)
		{
			for (int j = 0; j < fightingModeInfo.npcGroupList[i].waveInfoList.Length; j++)
			{
				tempAI = NPCPoolController.instance.GetAttackAI(fightingModeInfo.npcGroupList[i].waveInfoList[0].waveAIList[j].typeIndex);
				tempAI.gameObject.SetActiveRecursively(true);
				tempAI.attackAIFlag = true;
				tempAI.fightModeStandFlag = fightingModeInfo.npcGroupList[i].waveInfoList[0].waveAIList[j].standFlag;
				tempAI.transform.position = fightingModeInfo.startPointList[i].transform.position;
				tempAI.transform.forward = fightingModeInfo.startPointList[i].transform.forward;
				tempAI.anima.transform.localEulerAngles = Vector3.zero;
				tempAI.fightingStateCurPoint = fightingModeInfo.startPointList[i];
				tempAI.moveTarget = tempAI.GetNextFightingStatePoint();
				tempInfo = AIInfoList.instance.GetAIInfo(fightingModeInfo.npcGroupList[i].waveInfoList[0].waveAIList[j].typeIndex, fightingModeInfo.npcGroupList[i].waveInfoList[0].waveAIList[j].level);
				tempAI.healthCtl.healthVal = tempInfo.health;
				tempAI.healthCtl.isDead = false;
				tempAI.dieFlag = false;
				if (tempInfo.handGunFlag)
				{
					tempAI.gunInfo = GunInfoList.instance.GetGunInfo(1, 0);
				}
				else if (tempInfo.machineGunFlag)
				{
					tempAI.gunInfo = GunInfoList.instance.GetGunInfo(0, 0);
				}
				tempAI.gunInfo.accuracy = tempInfo.accuracy;
				tempAI.gunInfo.damage = tempInfo.attackVal;
				tempAI.gunInfo.shotInterval = tempInfo.fireInterval;
				tempAI.policeFlag = false;
				tempAI.handGunFlag = tempInfo.handGunFlag;
				tempAI.machineGunFlag = tempInfo.machineGunFlag;
				tempAI.fallingMoneyVal = tempInfo.fallingMoney;
				tempAI.fallingBulletRate = tempInfo.bulletRate;
				if (!tempAI.fightModeStandFlag)
				{
					if (UnityEngine.Random.Range(0, 3) > 0)
					{
						tempAI.ChangeState(AISTATE.WALK);
					}
					else
					{
						tempAI.ChangeState(AISTATE.IDLE);
					}
				}
				else
				{
					tempAI.ChangeState(AISTATE.IDLE);
				}
				tempAI.gameObject.GetComponent<Collider>().enabled = true;
				tempAI.chasingStartPos = tempAI.transform.position;
				genNumCount++;
				if (i == 0 && j == 0)
				{
					targetAI = tempAI;
				}
				tempAI = null;
			}
		}
	}

	public override void MyUpdate()
	{
		if (failFlag)
		{
			return;
		}
		base.MyUpdate();
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
			return;
		}
		if (restTime > 0f)
		{
			restTime -= Time.deltaTime;
			GameUIController.instance.taskBoardController.SetTime(restTime);
			if (restTime <= 0f)
			{
				EndMission(true);
			}
		}
		intervalCount += Time.deltaTime;
		if (PlayerController.instance.fireTarget == null && PlayerController.instance.car == null && intervalCount >= intervalExecuteTime)
		{
			intervalCount = 0f;
			PlayerController.instance.preFireTarget = CitySenceController.instance.GetPreFireTarget();
		}
	}

	public void SuccessMission()
	{
		EndMission(false);
	}

	public void OnPlayerDie()
	{
		if (GameController.instance.curMode == GameController.instance.fightingMode)
		{
			failFlag = true;
		}
	}

	public void FailGame()
	{
		if (GameController.instance.curMode == GameController.instance.fightingMode)
		{
			EndMission(true);
			PlayerController.instance.preFireTarget = null;
			PlayerController.instance.fireTarget = null;
			ClearAI();
		}
	}

	public void EndMission(bool isFail)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/TaskEndUI")) as GameObject;
		TaskEndUIController component = gameObject.GetComponent<TaskEndUIController>();
		component.Reset(isFail, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, (int)restTime, GameUIController.instance.rewardIndex);
		gameObject.transform.parent = GameUIController.instance.uiRoot.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localRotation = Quaternion.identity;
		GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
	}

	public void AIDie(AIController ai)
	{
		if (ai == targetAI)
		{
			EndMission(false);
		}
	}

	public void ClearAI()
	{
		while (NPCPoolController.instance.enableAIList.Count > 0)
		{
			NPCPoolController.instance.enableAIList[0].SenceRecycle();
		}
	}

	public override void Exit()
	{
		base.Exit();
		fightingModeInfo.gameObject.SetActiveRecursively(false);
		UnityEngine.Object.Destroy(fightingModeInfo.gameObject);
		fightingModeInfo = null;
		Resources.UnloadUnusedAssets();
		PoliceLevelCtl.level = 0;
		PoliceLevelCtl.score = 0;
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
		GameUIController.instance.timeCountFlag = false;
		PlayerController.instance.transform.position = GameUIController.instance.curTaskInfo.exitPlayerPos;
		PlayerController.instance.transform.eulerAngles = GameUIController.instance.curTaskInfo.exitPlayerAngle;
		PlayerController.instance.moveCtl.playerFaceAngle = PlayerController.instance.transform.eulerAngles.y;
		CarManage.instance.playerCar.ResetPlayerCar();
		GameController.instance.normalMode.ClearAI();
		GameUIController.instance.minimapController.EnablePlayerCarPos(CarManage.instance.playerCar.transform.position);
	}
}
