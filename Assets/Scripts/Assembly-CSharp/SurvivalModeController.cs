using System;
using UnityEngine;

public class SurvivalModeController : GameModeController
{
	public GameObject testPlayerMode0;

	public float waveInterval;

	public float countTime;

	public SurvivalModeInfo info;

	public int curGroupNum;

	public int curWaveNum;

	public bool genAIFlag;

	public int genNumCount;

	public bool NoAILeftFlag;

	public float genInterval;

	public float countGenInterval;

	public int killNum;

	public float useTime;

	public bool beforeGameFlag;

	private float beforeGameCount;

	public bool genDoneFlag;

	public float intervalCount;

	public float intervalExecuteTime = 0.5f;

	public bool failFlag;

	private float countEndTime;

	private float endTimeDelay = 1f;

	private bool endShowFlag;

	private AIController tempAI;

	private int randomNum;

	private int preRandomNum;

	private AIInfo tempInfo;

	private void Start()
	{
		PlayerController instance = PlayerController.instance;
		instance.resetSence = (PlayerController.ResetSence)Delegate.Combine(instance.resetSence, new PlayerController.ResetSence(FailGame));
		PlayerController instance2 = PlayerController.instance;
		instance2.onPlayerDie = (PlayerController.OnPlayerDie)Delegate.Combine(instance2.onPlayerDie, new PlayerController.OnPlayerDie(OnPlayerDie));
	}

	public override void Reset(int index)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("SurvivalMode/SurvivalMode" + index)) as GameObject;
		info = gameObject.GetComponent<SurvivalModeInfo>();
		testPlayerMode0 = gameObject;
		InitSurvivalMode();
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
		GameUIController.instance.taskBoardController.bottomLabel.text = "Survive and kill all enemies!";
		GameUIController.instance.taskBoardController.topBottom.width = (int)(360f * GlobalDefine.screenWidthFit);
		GameUIController.instance.minimapController.mapDrawPath.ClearLine();
		GameUIController.instance.taskBoardController.beforeGameFlag = true;
		GameUIController.instance.taskBoardController.SetTime(1f);
		GameUIController.instance.taskBoardController.SetTime(0f);
		PlayerController.instance.ChangeState(PLAYERSTATE.HANDGUN);
	}

	public void ResetDelay()
	{
		BlackScreen.instance.TurnOnScreen();
		GameUIController.instance.InitUI();
	}

	public void InitSurvivalMode()
	{
		PlayerController.instance.transform.position = info.playerDefaultPos;
		testPlayerMode0.transform.position = Vector3.zero;
		testPlayerMode0.SetActiveRecursively(true);
		countTime = 0f;
		waveInterval = info.npcGroupList[0].waveInfoList[0].delayTime;
		curGroupNum = 0;
		curWaveNum = 0;
		NoAILeftFlag = false;
		countTime = 0f;
		genNumCount = 0;
		countEndTime = 0f;
		endShowFlag = false;
		failFlag = false;
		useTime = 0f;
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
				GameUIController.instance.survivalWaveLabel.Reset("Wave 1/" + info.npcGroupList.Length);
			}
			return;
		}
		useTime += Time.deltaTime;
		GameUIController.instance.taskBoardController.SetTime(useTime);
		intervalCount += Time.deltaTime;
		if (PlayerController.instance.fireTarget == null && PlayerController.instance.car == null && intervalCount >= intervalExecuteTime)
		{
			intervalCount = 0f;
			PlayerController.instance.preFireTarget = CitySenceController.instance.GetPreFireTarget();
		}
		if (!NoAILeftFlag)
		{
			if (genAIFlag)
			{
				countGenInterval += Time.deltaTime;
				if (!(countGenInterval > genInterval))
				{
					return;
				}
				if (genNumCount < info.npcGroupList[curGroupNum].waveInfoList[curWaveNum].waveAIList.Length)
				{
					tempAI = NPCPoolController.instance.GetAttackAI(info.npcGroupList[curGroupNum].waveInfoList[curWaveNum].waveAIList[genNumCount].typeIndex);
					tempAI.gameObject.SetActiveRecursively(true);
					randomNum = UnityEngine.Random.Range(0, info.startPointList.Count);
					if (randomNum == preRandomNum)
					{
						randomNum = (randomNum + 1) % info.startPointList.Count;
					}
					preRandomNum = randomNum;
					tempAI.transform.position = info.startPointList[randomNum].position;
					tempAI.runStateCurPoint = info.firstPointList[randomNum];
					tempAI.moveTarget = tempAI.RunStateGetMoveTarget();
					tempInfo = AIInfoList.instance.GetAIInfo(info.npcGroupList[curGroupNum].waveInfoList[curWaveNum].waveAIList[genNumCount].typeIndex, info.npcGroupList[curGroupNum].waveInfoList[curWaveNum].waveAIList[genNumCount].level);
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
					tempAI.ChangeState(AISTATE.RUN);
					tempAI.gameObject.GetComponent<Collider>().enabled = true;
					tempAI.chasingStartPos = tempAI.transform.position;
					genNumCount++;
				}
				else
				{
					genAIFlag = false;
					if (curWaveNum == info.npcGroupList[curGroupNum].waveInfoList.Length - 1)
					{
						curWaveNum = 0;
						if (curGroupNum == info.npcGroupList.Length)
						{
							NoAILeftFlag = true;
						}
						else
						{
							waveInterval = -1f;
						}
					}
					else
					{
						curWaveNum++;
						waveInterval = info.npcGroupList[curGroupNum].waveInfoList[curWaveNum].delayTime;
					}
				}
				countGenInterval = 0f;
			}
			else if (waveInterval >= 0f)
			{
				countTime += Time.deltaTime;
				if (countTime > waveInterval)
				{
					countTime = 0f;
					genAIFlag = true;
					genNumCount = 0;
				}
			}
			else if (NPCPoolController.instance.enableAIList.Count == 0)
			{
				GroupDone();
			}
		}
		else
		{
			countEndTime += Time.deltaTime;
			if (countEndTime > endTimeDelay && !endShowFlag)
			{
				endShowFlag = true;
				SuccessMission();
			}
		}
	}

	public void SuccessMission()
	{
		EndMission(false);
	}

	public void OnPlayerDie()
	{
		if (GameController.instance.curMode == GameController.instance.survivalMode)
		{
			failFlag = true;
		}
	}

	public void FailGame()
	{
		if (GameController.instance.curMode == GameController.instance.survivalMode)
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
		component.Reset(isFail, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, (int)useTime, GameUIController.instance.rewardIndex);
		gameObject.transform.parent = GameUIController.instance.uiRoot.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localRotation = Quaternion.identity;
		GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
	}

	public void GroupDone()
	{
		if (curGroupNum + 1 == info.npcGroupList.Length)
		{
			NoAILeftFlag = true;
			countEndTime = 0f;
			return;
		}
		curGroupNum++;
		waveInterval = info.npcGroupList[curGroupNum].waveInfoList[curWaveNum].delayTime;
		GameUIController.instance.pickLabel.Reset("Wave " + (curGroupNum + 1) + "/" + info.npcGroupList.Length);
	}

	public void AIDie()
	{
		if (waveInterval < 0f && NPCPoolController.instance.enableAIList.Count == 1)
		{
			if (curGroupNum == info.npcGroupList.Length - 1)
			{
				GroupDone();
			}
			else
			{
				GameUIController.instance.pickLabel.Reset("WaveClear");
			}
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
		testPlayerMode0.SetActiveRecursively(false);
		UnityEngine.Object.Destroy(testPlayerMode0);
		testPlayerMode0 = null;
		info = null;
		Resources.UnloadUnusedAssets();
		PoliceLevelCtl.level = 0;
		PoliceLevelCtl.score = 0;
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
		GameUIController.instance.timeCountFlag = false;
		PlayerController.instance.transform.position = GameUIController.instance.curTaskInfo.exitPlayerPos;
		PlayerController.instance.transform.eulerAngles = GameUIController.instance.curTaskInfo.exitPlayerAngle;
		PlayerController.instance.moveCtl.playerFaceAngle = PlayerController.instance.transform.eulerAngles.y;
		CarManage.instance.playerCar.ResetPlayerCar();
	}
}
