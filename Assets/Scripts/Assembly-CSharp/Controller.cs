using UnityEngine;

public class Controller : MonoBehaviour
{
	public static Controller instance;

	public GameObject tempUI;

	public GameObject deLoadingObj;

	public Vector3[] startCarPos;

	public Vector3[] startCarRot;

	public GameController gameController;

	public int[] levelUnlockStarNum;

	public int sumStarNum;

	public GameObject[] levelObj;

	public GameObject grayLine1;

	public GameObject grayLine2;

	public GameObject lightLine1;

	public GameObject lightLine2;

	public Vector3[] playerStartPos;

	public Vector3[] playerStartAngle;

	public GameObject delayLoading;

	public bool levelUpFlag;

	public bool unLockLevelFlag;

	public VechicleController motor;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		if (GlobalInf.firstOpenGameFlag)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("UI/GameTutorialsController")) as GameObject;
		}
		GlobalDefine.init();
		tempUI.transform.localPosition = new Vector3((0f - GlobalDefine.screenRatioWidth) / 2f, -240f, 0f);
	}

	private void Start()
	{
		InitGame();
		if (GlobalInf.playerCarIndex != -1)
		{
			GameObject gameObject = Object.Instantiate(Resources.Load("PlayerCar/Car" + GlobalInf.playerCarIndex)) as GameObject;
			gameObject.transform.position = startCarPos[GlobalInf.gameLevel];
			gameObject.transform.eulerAngles = startCarRot[GlobalInf.gameLevel];
			gameObject.name = "PlayerCar";
			gameObject.transform.parent = null;
			CarManage.instance.playerCar = gameObject.GetComponent<VechicleController>();
			CarManage.instance.playerCar.AICarCtl.enabled = false;
			CarManage.instance.playerCar.AICarCtl.insideNPC = null;
			CarManage.instance.playerCar.enabled = false;
			CarManage.instance.playerCar.AICarCtl.motor.enabled = false;
			if (GlobalInf.carInfo != null)
			{
				CarManage.instance.playerCar.maxSpeed = GlobalInf.carInfo.maxSpeed;
				CarManage.instance.playerCar.brakeForce = GlobalInf.carInfo.brakeForce;
				CarManage.instance.playerCar.maxSteerAngle = GlobalInf.carInfo.maxSteerAngle;
				CarManage.instance.playerCar.maxSpeedSteerAngle = GlobalInf.carInfo.maxSpeedSteerAngle;
				CarManage.instance.playerCar.carHealth.maxHealthVal = GlobalInf.carInfo.maxHealthVal;
				CarManage.instance.playerCar.carHealth.healthVal = GlobalInf.carInfo.restHealthVal;
				CarManage.instance.playerCar.carHealth.playerCarFlag = true;
			}
		}
		if (AudioController.instance != null)
		{
			AudioController.instance.play(AudioType.FIGHTING);
		}
		MinimapLightLabelController.instance.EnableLightLabel();
		for (int i = 0; i < MinimapLightLabelController.instance.lightLabel2.Length; i++)
		{
			MinimapLightLabelController.instance.lightLabel2[i].DisableStars();
		}
		GameStateController.instance.CheckGameState();
	}

	private void Destroy()
	{
		if (instance != null)
		{
			instance = null;
		}
	}

	public void InitGame()
	{
		TaskLabelController.instance.InitTaskInfo();
		CountLevel();
		InitLevleObj();
		GlobalInf.startGameTime = (int)Time.time;
		if (!GlobalDefine.smallPhoneFlag)
		{
			Object.Instantiate(Resources.Load("BigPhoneItem/PathInfo") as GameObject);
			((GameObject)Object.Instantiate(Resources.Load("BigPhoneItem/RagDollPool") as GameObject)).SetActiveRecursively(false);
		}
		GlobalInf.curKill = 0;
		GlobalInf.curStartTime = (int)Time.time;
		GlobalInf.curDistance = 0f;
	}

	public void CountLevel()
	{
		if (!GlobalInf.newUserFlag)
		{
			if (GlobalInf.gameState < GameStateController.MAXSTATENUM)
			{
				return;
			}
		}
		else if (GlobalInf.gameState < GameStateController.NEWMAXSTATENUM)
		{
			return;
		}
		int num = 0;
		for (int num2 = levelUnlockStarNum.Length - 1; num2 >= 0; num2--)
		{
			if (sumStarNum >= levelUnlockStarNum[num2])
			{
				num = num2;
				break;
			}
		}
		if (GlobalInf.gameLevel < num)
		{
			unLockLevelFlag = true;
			levelUpFlag = true;
			GameUIController.instance.fingerObj.transform.localPosition = new Vector3(-297f * GlobalDefine.screenWidthFit, 162f, 0f);
		}
	}

	public void LevelUp()
	{
		GlobalInf.gameLevel++;
		StoreDateController.SetGameLevel(GlobalInf.gameLevel);
		unLockLevelFlag = false;
		levelUpFlag = false;
		InitLevleObj();
	}

	public void InitLevleObj()
	{
		for (int i = 0; i < levelObj.Length; i++)
		{
			if (i >= GlobalInf.gameLevel)
			{
				levelObj[i].gameObject.SetActiveRecursively(true);
			}
			else
			{
				levelObj[i].gameObject.SetActiveRecursively(false);
			}
		}
		if (GlobalInf.gameLevel == 1)
		{
			grayLine1.gameObject.SetActiveRecursively(false);
			lightLine1.gameObject.SetActiveRecursively(true);
			grayLine2.gameObject.SetActiveRecursively(true);
			lightLine2.gameObject.SetActiveRecursively(false);
		}
		else if (GlobalInf.gameLevel == 2)
		{
			grayLine1.gameObject.SetActiveRecursively(false);
			lightLine1.gameObject.SetActiveRecursively(true);
			grayLine2.gameObject.SetActiveRecursively(false);
			lightLine2.gameObject.SetActiveRecursively(true);
		}
	}
}
