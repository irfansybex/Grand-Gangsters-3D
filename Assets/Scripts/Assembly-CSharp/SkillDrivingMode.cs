using UnityEngine;

public class SkillDrivingMode : GameModeController
{
	public bool countTimeFlag;

	public float restTime;

	public float setTime;

	public float[] skillDrivingMissionTime;

	public bool failFlag;

	public bool restartFlag;

	public bool beforeGameFlag;

	public float beforeGameCount;

	public float enterCarHealthVal;

	public bool endGameFlag;

	private GameObject itemObj;

	public int curState;

	public Vector3 startPos;

	public Vector3 startAngle;

	public VechicleController curPlayerCar;

	public SkillDrivingInfo curSkillDrivingInfo;

	public override void Reset(int index)
	{
		itemObj = Object.Instantiate(Resources.Load("SkillDrivingMode/SkillDriving" + index)) as GameObject;
		curSkillDrivingInfo = itemObj.GetComponent<SkillDrivingInfo>();
		endGameFlag = false;
		restTime = skillDrivingMissionTime[index];
		PoliceLevelCtl.ResetPoliceLevel();
		Invoke("LateInvoke", 0.5f);
		if (!restartFlag)
		{
			if (CarManage.instance.playerCar.carType == CARTYPE.MOTOR)
			{
				GameObject gameObject = Object.Instantiate(Resources.Load("PlayerCar/Car" + GlobalInf.preCarIndex)) as GameObject;
				gameObject.name = "NewPlayerCar";
				gameObject.transform.parent = null;
				curPlayerCar = gameObject.GetComponent<CarController>();
				curPlayerCar.AICarCtl.enabled = false;
				curPlayerCar.AICarCtl.insideNPC = null;
				curPlayerCar.enabled = false;
				curPlayerCar.AICarCtl.motor.enabled = false;
				if (GlobalInf.preCarInfo != null)
				{
					curPlayerCar.maxSpeed = GlobalInf.preCarInfo.maxSpeed;
					curPlayerCar.brakeForce = GlobalInf.preCarInfo.brakeForce;
					curPlayerCar.maxSteerAngle = GlobalInf.preCarInfo.maxSteerAngle;
					curPlayerCar.maxSpeedSteerAngle = GlobalInf.preCarInfo.maxSpeedSteerAngle;
					curPlayerCar.carHealth.maxHealthVal = GlobalInf.preCarInfo.maxHealthVal;
					curPlayerCar.carHealth.healthVal = GlobalInf.preCarInfo.restHealthVal;
					curPlayerCar.carHealth.playerCarFlag = true;
				}
				CarManage.instance.playerCar.gameObject.SetActiveRecursively(false);
			}
			else
			{
				curPlayerCar = CarManage.instance.playerCar;
			}
			ResetPlayerCar(index);
			enterCarHealthVal = CarManage.instance.playerCar.carHealth.healthVal;
		}
		else
		{
			curPlayerCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
			curPlayerCar.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			curPlayerCar.transform.position = startPos;
			curPlayerCar.transform.eulerAngles = startAngle;
			curPlayerCar.ResetWheel();
			restartFlag = false;
		}
		failFlag = false;
		beforeGameFlag = true;
		beforeGameCount = 4.5f;
		GameUIController.instance.disableCarBtnFlag = true;
		GameUIController.instance.taskBoardController.beforeGameFlag = true;
		GameUIController.instance.taskBoardController.bottomLabel.text = "Keep driving your car on the track";
		GameUIController.instance.taskBoardController.topBottom.width = (int)(400f * GlobalDefine.screenWidthFit);
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
		curPlayerCar.EnableCarWheelRay();
		GameUIController.instance.minimapController.mapDrawPath.ClearLine();
		curState = -1;
		GameUIController.instance.taskBoardController.SetTime(1f);
		GameUIController.instance.taskBoardController.SetTime(0f);
	}

	public void LateInvoke()
	{
		BlackScreen.instance.TurnOnScreen();
		GameUIController.instance.InitUI();
		GameUIController.instance.EnableLocateLabel(curSkillDrivingInfo.stateLabelList[0].gameObject);
	}

	public override void MyUpdate()
	{
		if (endGameFlag || failFlag)
		{
			return;
		}
		base.MyUpdate();
		if (beforeGameFlag)
		{
			beforeGameCount -= Time.deltaTime;
			GameUIController.instance.taskBoardController.SetTime(beforeGameCount);
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
				EndMission(true);
			}
		}
	}

	public void ResetPlayerCarPos()
	{
		if (curState == -1)
		{
			curPlayerCar.transform.position = startPos;
			curPlayerCar.transform.eulerAngles = startAngle;
		}
		else
		{
			curPlayerCar.transform.position = curSkillDrivingInfo.stateLabelList[curState].transform.position;
			curPlayerCar.transform.eulerAngles = curSkillDrivingInfo.stateLabelList[curState].transform.eulerAngles;
			curPlayerCar.ResetWheel();
		}
		curPlayerCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	public void MoveState()
	{
		curState++;
		if (curState + 1 < curSkillDrivingInfo.stateLabelList.Length)
		{
			curSkillDrivingInfo.stateLabelList[curState + 1].gameObject.SetActiveRecursively(true);
			GameUIController.instance.EnableLocateLabel(curSkillDrivingInfo.stateLabelList[curState + 1].gameObject);
		}
		else
		{
			curSkillDrivingInfo.finalObj.gameObject.SetActiveRecursively(true);
			GameUIController.instance.EnableLocateLabel(curSkillDrivingInfo.finalObj.gameObject);
		}
	}

	public void ResetPlayerCar(int index)
	{
		curPlayerCar.transform.position = curSkillDrivingInfo.startPos;
		curPlayerCar.transform.eulerAngles = curSkillDrivingInfo.startAngle;
		startPos = curSkillDrivingInfo.startPos;
		startAngle = curSkillDrivingInfo.startAngle;
		curPlayerCar.gameObject.SetActiveRecursively(true);
		curPlayerCar.AICarCtl.moveFlag = false;
		if (CarManage.instance.playerCar == TempObjControllor.instance.curBrokenCar)
		{
			TempObjControllor.instance.brokenCar.SetActiveRecursively(false);
		}
		PlayerController.instance.transform.position = curPlayerCar.getOnPoint.transform.position;
		PlayerController.instance.transform.forward = curPlayerCar.getOnPoint.transform.forward;
		PlayerController.instance.GetOnCar(curPlayerCar.getOnPoint.transform, curPlayerCar);
		PlayerController.instance.cam.transform.forward = curPlayerCar.transform.forward;
		TempObjControllor.instance.GetEatLightLabel().transform.parent = curPlayerCar.particleRoot;
		TempObjControllor.instance.GetEatLightLabel().transform.localPosition = Vector3.zero;
	}

	private void FailGame()
	{
		if (GameController.instance.curGameMode == GAMEMODE.SKILLDRIVING)
		{
			EndMission(true);
		}
	}

	public void EndMission(bool isFail)
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("UI/TaskEndUI")) as GameObject;
		TaskEndUIController component = gameObject.GetComponent<TaskEndUIController>();
		component.Reset(isFail, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, (int)restTime, GameUIController.instance.rewardIndex);
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
		base.Exit();
		GameController.instance.normalMode.npcProduceTime = 5f;
		Object.Destroy(itemObj);
		if (!restartFlag)
		{
			if (!failFlag)
			{
				curPlayerCar.OnCloseDoor();
				curPlayerCar.OnDisableCar();
				PlayerController.instance.cam.OnChangeTarget(false);
				PlayerController.instance.GetOffCarDone();
				PlayerController.instance.transform.eulerAngles = new Vector3(0f, PlayerController.instance.transform.eulerAngles.y, 0f);
				CarManage.instance.ResetPlayerCarPos();
			}
			curPlayerCar.OnDisableWheelRay();
			if (CarManage.instance.playerCar.carType == CARTYPE.MOTOR)
			{
				Object.Destroy(curPlayerCar.gameObject);
				curPlayerCar = null;
				CarManage.instance.playerCar.gameObject.SetActiveRecursively(true);
			}
			PlayerController.instance.transform.position = GameUIController.instance.curTaskInfo.exitPlayerPos;
			PlayerController.instance.transform.eulerAngles = GameUIController.instance.curTaskInfo.exitPlayerAngle;
			PlayerController.instance.moveCtl.playerFaceAngle = PlayerController.instance.transform.eulerAngles.y;
			CarManage.instance.playerCar.ResetPlayerCar();
			curSkillDrivingInfo = null;
			Resources.UnloadUnusedAssets();
		}
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
		GameUIController.instance.timeCountFlag = false;
		itemObj = null;
	}
}
