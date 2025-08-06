using System;
using System.Collections.Generic;
using UnityEngine;

public class DrivingModeController : GameModeController
{
	public LightLabel lightLabel;

	public Transform lightLabelRoot;

	public List<RoadPointNew> roadPointList;

	public bool restartFlag;

	public GameObject redScreen;

	public Vector2[] startPoint;

	public Vector2[] endPoint;

	public Vector3[] endAngleArray;

	public Vector3 endAngle;

	public float[] startSetTime;

	public float restTime;

	public MiniMapController minimapController;

	public bool failUIFlag;

	public bool beforeGameFlag;

	public float beforeGameTimeCount;

	public DrivingPos[] drivingLevel;

	public int curLevelIndex;

	private void Start()
	{
		PlayerController instance = PlayerController.instance;
		instance.resetSence = (PlayerController.ResetSence)Delegate.Combine(instance.resetSence, new PlayerController.ResetSence(FailGame));
		PlayerController instance2 = PlayerController.instance;
		instance2.onPlayerDie = (PlayerController.OnPlayerDie)Delegate.Combine(instance2.onPlayerDie, new PlayerController.OnPlayerDie(OnPlayerDie));
	}

	public override void MyUpdate()
	{
		GameController.instance.normalMode.MyUpdate();
		if (beforeGameFlag)
		{
			beforeGameTimeCount -= Time.deltaTime;
			GameUIController.instance.taskBoardController.SetBeforeGameCount((int)(beforeGameTimeCount - 0.1f));
			if (beforeGameTimeCount <= 0f)
			{
				beforeGameFlag = false;
				GameUIController.instance.disableCarBtnFlag = false;
				GameUIController.instance.taskBoardController.beforeGameFlag = false;
				GameUIController.instance.taskBoardController.baskMask.SetActiveRecursively(false);
				GameUIController.instance.taskBoardController.tweenAlph[0].gameObject.SetActiveRecursively(false);
			}
		}
		else if (!failUIFlag && restTime > 0f)
		{
			restTime -= Time.deltaTime;
			GameUIController.instance.taskBoardController.SetTime(restTime);
			if (restTime < 0f && !failUIFlag)
			{
				failUIFlag = true;
				FailGame();
			}
		}
	}

	private void OnPlayerDie()
	{
		if (GameController.instance.curGameMode == GAMEMODE.CARKILLING)
		{
			failUIFlag = true;
		}
	}

	public void FailGame()
	{
		if (GameController.instance.curMode == GameController.instance.driveMode)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/TaskEndUI")) as GameObject;
			TaskEndUIController component = gameObject.GetComponent<TaskEndUIController>();
			component.Reset(true, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, (int)GameController.instance.driveMode.restTime, GameUIController.instance.rewardIndex);
			gameObject.transform.parent = GameUIController.instance.uiRoot.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localRotation = Quaternion.identity;
			GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
		}
	}

	public override void Reset(int index)
	{
		curLevelIndex = index;
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(true);
		GameUIController.instance.taskBoardController.DisableNum();
		InitLightLabel(index);
		restTime = startSetTime[index];
		failUIFlag = false;
		beforeGameFlag = true;
		beforeGameTimeCount = 4.5f;
		GameUIController.instance.disableCarBtnFlag = true;
		GameUIController.instance.taskBoardController.beforeGameFlag = true;
		GameUIController.instance.timeCountFlag = true;
		GameUIController.instance.taskBoardController.bottomLabel.text = "Drive along the designated route";
		GameUIController.instance.taskBoardController.topBottom.width = (int)(390f * GlobalDefine.screenWidthFit);
		Invoke("LateInvoke", 0.5f);
		GameUIController.instance.taskBoardController.SetTime(1f);
		GameUIController.instance.taskBoardController.SetTime(0f);
	}

	public void LateInvoke()
	{
		BlackScreen.instance.TurnOnScreen();
		GameUIController.instance.InitUI();
		GameUIController.instance.EnableLocateLabel(lightLabel.gameObject);
	}

	public void InitLightLabel(int index)
	{
		endAngle = endAngleArray[index];
		lightLabel.lightLabelPosList.Clear();
		for (int i = 0; i < drivingLevel[index].posList.Length; i++)
		{
			lightLabel.lightLabelPosList.Add(drivingLevel[index].posList[i]);
		}
		lightLabel.transform.position = lightLabel.lightLabelPosList[0];
		lightLabel.gameObject.SetActiveRecursively(true);
		lightLabel.curIndex = 0;
		lightLabel.endMissionFlag = false;
		minimapController.mapDrawPath.ClearLine();
		minimapController.FindWay(lightLabel.lightLabelPosList[0]);
		minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(lightLabel.lightLabelPosList[0]) + new Vector3(1000f, 480f, 0f));
		if (!restartFlag)
		{
			ResetPlayerCar();
			return;
		}
		CarManage.instance.playerCar.transform.position = drivingLevel[index].startPos;
		CarManage.instance.playerCar.transform.eulerAngles = drivingLevel[index].startAngle;
		GameUIController.instance.OnChangCarUI();
		CarManage.instance.playerCar.OnEnableCar();
		restartFlag = false;
	}

	public void ResetPlayerCar()
	{
		CarManage.instance.playerCar.transform.position = drivingLevel[curLevelIndex].startPos;
		CarManage.instance.playerCar.transform.eulerAngles = drivingLevel[curLevelIndex].startAngle;
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
		TempObjControllor.instance.GetEatLightLabel().transform.parent = CarManage.instance.playerCar.particleRoot;
		TempObjControllor.instance.GetEatLightLabel().transform.localPosition = Vector3.zero;
	}

	public void InitRoadPointList(int index)
	{
		roadPointList = RoadPathInfo.instance.Getpath(roadInfoListNew.instance.roadList[(int)startPoint[index].x].roadPointList[(int)startPoint[index].y], roadInfoListNew.instance.roadList[(int)endPoint[index].x].roadPointList[(int)endPoint[index].y]);
		minimapController.mapDrawPath.ClearLine();
		minimapController.FindWay(roadInfoListNew.instance.roadList[(int)endPoint[index].x].roadPointList[(int)endPoint[index].y].position);
	}

	public void InitRoadPointList()
	{
		roadPointList.Clear();
		roadPointList.Add(roadInfoListNew.instance.roadList[187].roadPointList[3]);
		RoadPointNew roadPointNew = null;
		RoadPointNew roadPointNew2 = roadInfoListNew.instance.roadList[187].roadPointList[3];
		int num = 0;
		for (int i = 0; i < 55; i++)
		{
			if (!roadPointNew2.crossFlag)
			{
				if (roadPointNew2.GetLinkPoint(0) != roadPointNew)
				{
					roadPointList.Add(roadPointNew2.GetLinkPoint(0));
					roadPointNew = roadPointNew2;
					roadPointNew2 = roadPointNew2.GetLinkPoint(0);
				}
				else
				{
					roadPointList.Add(roadPointNew2.GetLinkPoint(1));
					roadPointNew = roadPointNew2;
					roadPointNew2 = roadPointNew2.GetLinkPoint(1);
				}
			}
			else if (!roadPointNew.crossFlag)
			{
				num++;
				if (num % 3 == 0)
				{
					roadPointNew = roadPointNew2;
					if (roadPointNew2.GetLinkPoint(1) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(1);
					}
					else if (roadPointNew2.GetLinkPoint(2) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(2);
					}
					else if (roadPointNew2.GetLinkPoint(3) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(3);
					}
				}
				else if (num % 3 == 1)
				{
					roadPointNew = roadPointNew2;
					if (roadPointNew2.GetLinkPoint(2) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(2);
					}
					else if (roadPointNew2.GetLinkPoint(3) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(3);
					}
					else if (roadPointNew2.GetLinkPoint(1) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(1);
					}
				}
				else
				{
					roadPointNew = roadPointNew2;
					if (roadPointNew2.GetLinkPoint(3) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(3);
					}
					else if (roadPointNew2.GetLinkPoint(1) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(1);
					}
					else if (roadPointNew2.GetLinkPoint(2) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(2);
					}
				}
				roadPointList.Add(roadPointNew2);
			}
			else
			{
				roadPointNew = roadPointNew2;
				roadPointNew2 = roadPointNew2.GetLinkPoint(0);
				roadPointList.Add(roadPointNew2);
			}
		}
	}

	public void PutLightLabel(float dis)
	{
		lightLabel.lightLabelPosList.Clear();
		float num = dis;
		int num2 = 0;
		for (int i = 0; i < roadPointList.Count - 1; i++)
		{
			if (!roadPointList[i].crossFlag || !roadPointList[i + 1].crossFlag)
			{
				float roadPointDistance;
				for (roadPointDistance = roadPointList[i].GetRoadPointDistance(roadPointList[i + 1]); num < roadPointDistance; num += dis)
				{
					num2 = Mathf.Clamp(num2 + UnityEngine.Random.Range(-2, 3), -2, 2);
					lightLabel.lightLabelPosList.Add(GetRoadBtnPos(roadPointList[i], roadPointList[i + 1], num, num2 * 3));
				}
				num -= roadPointDistance;
			}
		}
		lightLabel.transform.position = lightLabel.lightLabelPosList[0];
		lightLabel.gameObject.SetActiveRecursively(true);
		lightLabel.curIndex = 0;
		GameUIController.instance.EnableLocateLabel(lightLabel.gameObject);
	}

	public Vector3 GetRoadBtnPos(RoadPointNew curPoint, RoadPointNew nextPoint, float forwardDis, float sideDis)
	{
		Vector3 vector = new Vector3(0f, 0f, 0f);
		if (ToolFunction.isForward(nextPoint.position - curPoint.position, curPoint.forward))
		{
			return curPoint.position + curPoint.forward * forwardDis + curPoint.right * sideDis;
		}
		return curPoint.position - curPoint.forward * forwardDis - curPoint.right * sideDis;
	}

	public override void Exit()
	{
		base.Exit();
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
		GameUIController.instance.timeCountFlag = false;
		for (int num = lightLabelRoot.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(lightLabelRoot.GetChild(num).gameObject);
		}
		if (!restartFlag)
		{
			if (PlayerController.instance.curState == PLAYERSTATE.CAR)
			{
				PlayerController.instance.car.OnCloseDoor();
				PlayerController.instance.car.OnDisableCar();
				PlayerController.instance.cam.OnChangeTarget(false);
				PlayerController.instance.GetOffCarDone();
			}
			PlayerController.instance.transform.position = GameUIController.instance.curTaskInfo.exitPlayerPos;
			PlayerController.instance.transform.eulerAngles = GameUIController.instance.curTaskInfo.exitPlayerAngle;
			PlayerController.instance.moveCtl.playerFaceAngle = PlayerController.instance.transform.eulerAngles.y;
			CarManage.instance.ResetPlayerCarPos();
		}
		minimapController.mapDrawPath.ClearLine();
		lightLabel.gameObject.SetActiveRecursively(false);
		GameUIController.instance.DisableLocateLabel();
	}
}
