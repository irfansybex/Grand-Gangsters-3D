using System;
using UnityEngine;

public class StoreDateController
{
	public static bool initFlag;

	public static void InitDate()
	{
		if (!initFlag)
		{
			initFlag = true;
			GlobalInf.handgunIndex = GetHandGunIndex();
			GlobalInf.machineGunIndex = GetMachineGunIndex();
			GlobalInf.playerCarIndex = GetCarIndex();
			GlobalInf.preCarIndex = GetPreCarIndex();
			GlobalInf.healthKitNum = GetHealthKitNum();
			GlobalInf.toolKitNum = GetToolKitNum();
			GlobalInf.gameLevel = GetGameLevel();
			GlobalInf.drivingDistance = GetDrivingDistance();
			GlobalInf.policeKillNum = GetPoliceKillNum();
			GlobalInf.totalCashEarned = GetTotalCashEarned();
			GlobalInf.totalCashSpent = GetTotalCashSpent();
			GlobalInf.totalGoldEarned = GetTotalGoldEarned();
			GlobalInf.totalKillNum = GetTotalKill();
			GlobalInf.totalTimeSpent = GetTotalTimeSpent();
			GlobalInf.totalKillCitizens = GetTotalKillCitizens();
			GlobalInf.totalKillGangsters = GetTotalKillGangsters();
			GlobalInf.punchKillNum = GetPunchKillNum();
			GlobalInf.handGunKillNum = GetHandGunKillNum();
			GlobalInf.machineGunKillNum = GetMachineGunKillNum();
			GlobalInf.carKillNum = GetCarKillNum();
			GlobalInf.completeTaskNum = GetCompleteTaskNum();
			GlobalInf.completeDifTaskNum = GetCompleteDifTaskNum();
			GlobalInf.threeStarTaskNum = GetThreeStarTaskNum();
			GlobalInf.hartCount = GetHartCount();
			GlobalInf.preHartCount = GlobalInf.hartCount;
			if (PlayerPrefs.GetInt("CollectingTipsFlag", 0) == 0)
			{
				GlobalInf.collectingTipsFlag = false;
			}
			else
			{
				GlobalInf.collectingTipsFlag = true;
			}
			Platform.CountHarts();
			if (GlobalInf.hartCount > 3 + GlobalInf.gameLevel)
			{
				GlobalInf.hartCount = 3 + GlobalInf.gameLevel;
			}
			GetAudioVolume();
			GetCollectingInfo();
			GetGameState();
			if (GetControlType() == 0)
			{
				GlobalInf.carCtrlType = CARCTRLTYPE.BUTTON;
			}
			else
			{
				GlobalInf.carCtrlType = CARCTRLTYPE.GRAVITY;
			}
			if (PlayerPrefs.GetInt("healthTutorialFlag", 0) == 0)
			{
				GlobalInf.healthKitTutorialFlag = true;
			}
			else
			{
				GlobalInf.healthKitTutorialFlag = false;
			}
			if (PlayerPrefs.GetInt("toolKitTutorialFlag", 0) == 0)
			{
				GlobalInf.toolKitTutorialFlag = true;
			}
			else
			{
				GlobalInf.toolKitTutorialFlag = false;
			}
			GetNotificationFlag();
			GetLastInfo();
			GetClothIndex();
			GetPlayerHealthVal();
			GetGoldVideoCount();
		}
	}

	public static void GetNotificationFlag()
	{
		if (PlayerPrefs.GetInt("NotificationFlag", 0) == 0)
		{
			GlobalInf.notificationFlag = true;
		}
		else
		{
			GlobalInf.notificationFlag = false;
		}
	}

	public static void SetNotificationFlag(bool flag)
	{
		if (flag)
		{
			GlobalInf.notificationFlag = true;
			PlayerPrefs.SetInt("NotificationFlag", 0);
		}
		else
		{
			GlobalInf.notificationFlag = false;
			PlayerPrefs.SetInt("NotificationFlag", 1);
		}
	}

	public static void GetGameState()
	{
		if (PlayerPrefs.GetInt("NewUser", 0) == 0)
		{
			Debug.Log("GlobalInf.newUserFlag = false;");
			GlobalInf.newUserFlag = false;
		}
		else
		{
			GlobalInf.newUserFlag = true;
		}
		GlobalInf.gameState = PlayerPrefs.GetInt("GameState", 0);
		if (GlobalInf.gameState == GameStateController.NEWMAXSTATENUM - 1)
		{
			GlobalInf.gameState = GameStateController.NEWMAXSTATENUM;
			SetGameState();
		}
	}

	public static void SetGameState()
	{
		PlayerPrefs.SetInt("GameState", GlobalInf.gameState);
	}

	public static void GetAudioVolume()
	{
		GlobalInf.soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
		if (!GlobalDefine.smallPhoneFlag)
		{
			GlobalInf.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
		}
		else
		{
			GlobalInf.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
		}
		if (GlobalInf.soundVolume <= 0.05f)
		{
			GlobalInf.soundFlag = false;
		}
		else
		{
			GlobalInf.soundFlag = true;
		}
		if (GlobalInf.musicVolume <= 0.05f)
		{
			GlobalInf.musicFlag = false;
		}
		else
		{
			GlobalInf.musicFlag = true;
		}
	}

	public static void SetSoundVolume(float volume)
	{
		GlobalInf.soundVolume = volume;
		PlayerPrefs.SetFloat("SoundVolume", GlobalInf.soundVolume);
	}

	public static void SetMusicVolume(float volume)
	{
		GlobalInf.musicVolume = volume;
		PlayerPrefs.SetFloat("MusicVolume", GlobalInf.musicVolume);
	}

	public static void GetCollectingInfo()
	{
		GlobalInf.collectHandGunDoneFlag = GetCollectHandGunDoneFlag();
		GlobalInf.collectMachineGunDoneFlag = GetCollectMachineGunDoneFlag();
		GlobalInf.collectCarDoneFlag = GetCollectCarDoneFlag();
		GlobalInf.collectCarData = new int[GlobalDefine.COLLECT_HAND_GUN_MAXNUM];
		GlobalInf.collectHandGunData = new int[GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM];
		GlobalInf.collectMachineGunData = new int[GlobalDefine.COLLECT_CAR_MAXNUM];
		if (!GlobalInf.collectHandGunDoneFlag)
		{
			GetCollectingListInfo(GlobalInf.collectHandGunData, "CollectingHandGunData");
			GlobalInf.collectHandGunNum = 0;
			for (int i = 0; i < GlobalDefine.COLLECT_HAND_GUN_MAXNUM; i++)
			{
				GlobalInf.collectHandGunNum += GlobalInf.collectHandGunData[i];
			}
		}
		else
		{
			for (int j = 0; j < GlobalDefine.COLLECT_HAND_GUN_MAXNUM; j++)
			{
				GlobalInf.collectHandGunData[j] = 1;
			}
			GlobalInf.collectHandGunNum = GlobalDefine.COLLECT_HAND_GUN_MAXNUM;
		}
		if (!GlobalInf.collectMachineGunDoneFlag)
		{
			GetCollectingListInfo(GlobalInf.collectMachineGunData, "CollectingMachineGunData");
			GlobalInf.collectMachineGunNum = 0;
			for (int k = 0; k < GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM; k++)
			{
				GlobalInf.collectMachineGunNum += GlobalInf.collectMachineGunData[k];
			}
		}
		else
		{
			for (int l = 0; l < GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM; l++)
			{
				GlobalInf.collectMachineGunData[l] = 1;
			}
			GlobalInf.collectMachineGunNum = GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM;
		}
		if (!GlobalInf.collectCarDoneFlag)
		{
			GetCollectingListInfo(GlobalInf.collectCarData, "CollectingCarData");
			GlobalInf.collectCarNum = 0;
			for (int m = 0; m < GlobalDefine.COLLECT_CAR_MAXNUM; m++)
			{
				GlobalInf.collectCarNum += GlobalInf.collectCarData[m];
			}
		}
		else
		{
			for (int n = 0; n < GlobalDefine.COLLECT_CAR_MAXNUM; n++)
			{
				GlobalInf.collectCarData[n] = 1;
			}
			GlobalInf.collectCarNum = GlobalDefine.COLLECT_CAR_MAXNUM;
		}
	}

	public static void SetCollectHandGunDoneFlag(bool isDone)
	{
		if (isDone)
		{
			PlayerPrefs.SetInt("collectHandGunDoneFlag", 1);
		}
		else
		{
			PlayerPrefs.SetInt("collectHandGunDoneFlag", 0);
		}
	}

	public static bool GetCollectHandGunDoneFlag()
	{
		if (PlayerPrefs.GetInt("collectHandGunDoneFlag", 0) == 1)
		{
			return true;
		}
		return false;
	}

	public static void SetCollectMachineGunDoneFlag(bool isDone)
	{
		if (isDone)
		{
			PlayerPrefs.SetInt("collectMachineGunDoneFlag", 1);
		}
		else
		{
			PlayerPrefs.SetInt("collectMachineGunDoneFlag", 0);
		}
	}

	public static bool GetCollectMachineGunDoneFlag()
	{
		if (PlayerPrefs.GetInt("collectMachineGunDoneFlag", 0) == 1)
		{
			return true;
		}
		return false;
	}

	public static void SetCollectCarDoneFlag(bool isDone)
	{
		if (isDone)
		{
			PlayerPrefs.SetInt("collectCarDoneFlag", 1);
		}
		else
		{
			PlayerPrefs.SetInt("collectCarDoneFlag", 0);
		}
	}

	public static bool GetCollectCarDoneFlag()
	{
		if (PlayerPrefs.GetInt("collectCarDoneFlag", 0) == 1)
		{
			return true;
		}
		return false;
	}

	public static void SetCollectingInfo()
	{
		SetCollectingListInfo(GlobalInf.collectCarData, "CollectingCarData");
		SetCollectingListInfo(GlobalInf.collectHandGunData, "CollectingHandGunData");
		SetCollectingListInfo(GlobalInf.collectMachineGunData, "CollectingMachineGunData");
	}

	public static void GetCollectingListInfo(int[] list, string tag)
	{
		string @string = PlayerPrefs.GetString(tag, string.Empty);
		@string += "0";
		if (@string.Equals("0"))
		{
			for (int i = 0; i < list.Length; i++)
			{
				list[i] = 0;
			}
			return;
		}
		string[] array = @string.Split("|"[0]);
		for (int j = 0; j < list.Length; j++)
		{
			list[j] = Convert.ToInt32(array[j]);
		}
	}

	public static void SetCollectingListInfo(int[] list, string tag)
	{
		string text = string.Empty;
		for (int i = 0; i < list.Length; i++)
		{
			string text2 = text;
			text = text2 + string.Empty + list[i] + "|";
		}
		PlayerPrefs.SetString(tag, text);
	}

	public static void SetHandGunInfoList(ItemGunInfo[] handGunList)
	{
		for (int i = 0; i < handGunList.Length; i++)
		{
			SetHandGunInfo(handGunList[i], i);
		}
	}

	public static void GetHandGunInfoList(ItemGunInfo[] handGunList)
	{
		for (int i = 0; i < handGunList.Length; i++)
		{
			GetHandGunInfo(handGunList[i], i);
		}
	}

	public static int GetHandGunNum(int index)
	{
		return PlayerPrefs.GetInt("handGunNum_" + index, 0);
	}

	public static void SetHandGunNum(int index, int num)
	{
		PlayerPrefs.SetInt("handGunNum_" + index, num);
	}

	public static int GetHandGunLevel(int index)
	{
		return PlayerPrefs.GetInt("handGunLevel_" + index, 0);
	}

	public static void SetHandGunLevel(int index, int num)
	{
		PlayerPrefs.SetInt("handGunLevel_" + index, num);
	}

	public static void SetHandGunInfo(ItemGunInfo handGunInfo, int index)
	{
		SetHandGunLevel(index, handGunInfo.gunInfo.level);
		SetHandGunNum(index, handGunInfo.gunNum);
		SetHandGunBulletNum(index, handGunInfo.gunInfo.restBulletNum);
	}

	public static int GetHandGunBulletNum(int gunIndex)
	{
		return PlayerPrefs.GetInt("handGunBulletNum_" + gunIndex, 900);
	}

	public static void SetHandGunBulletNum(int gunIndex, int bulletNum)
	{
		PlayerPrefs.SetInt("handGunBulletNum_" + gunIndex, bulletNum);
	}

	public static void GetHandGunInfo(ItemGunInfo info, int index)
	{
		info.gunInfo.level = GetHandGunLevel(index);
		info.gunNum = GetHandGunNum(index);
		info.gunInfo.accuracy = info.accuracyLevelList[info.gunInfo.level];
		info.gunInfo.damage = info.damageLevelList[info.gunInfo.level];
		info.gunInfo.bulletNum = info.bulletNumLevelList[info.gunInfo.level];
		info.gunInfo.shotInterval = info.shotIntervalLevelList[info.gunInfo.level];
		info.gunInfo.reloadTime = info.reloadLevelList[info.gunInfo.level];
		info.gunInfo.restBulletNum = GetHandGunBulletNum(index);
		if (info.gunInfo.restBulletNum >= info.gunInfo.maxBulletNum)
		{
			info.gunInfo.restBulletNum = info.gunInfo.maxBulletNum;
			SetHandGunBulletNum(index, info.gunInfo.maxBulletNum);
		}
		if (info.gunNum > 0)
		{
			info.buyFlag = true;
		}
		else
		{
			info.buyFlag = false;
		}
		if (GlobalInf.gameLevel >= info.unlockByLevel)
		{
			info.unlockFlag = true;
		}
		else if (info.unlockByLevel == 99)
		{
			if (GlobalInf.collectHandGunDoneFlag)
			{
				info.unlockFlag = true;
			}
			else
			{
				info.unlockFlag = false;
			}
		}
		else if (info.gunNum > 0)
		{
			info.unlockFlag = true;
		}
		else
		{
			info.unlockFlag = false;
		}
	}

	public static void SetMachineGunInfoList(ItemGunInfo[] machineGunList)
	{
		for (int i = 0; i < machineGunList.Length; i++)
		{
			SetMachineGunInfo(machineGunList[i], i);
		}
	}

	public static void GetMachineGunInfoList(ItemGunInfo[] machineGunList)
	{
		for (int i = 0; i < machineGunList.Length; i++)
		{
			GetMachineGunInfo(machineGunList[i], i);
		}
	}

	public static void SetMachineGunLevel(int index, int level)
	{
		PlayerPrefs.SetInt("machineGunLevel_" + index, level);
	}

	public static int GetMachineGunLevel(int index)
	{
		return PlayerPrefs.GetInt("machineGunLevel_" + index, 0);
	}

	public static void SetMachineGunNum(int index, int num)
	{
		PlayerPrefs.SetInt("machineGunNum_" + index, num);
	}

	public static int GetMachineGunNum(int index)
	{
		return PlayerPrefs.GetInt("machineGunNum_" + index, 0);
	}

	public static void SetMachineGunInfo(ItemGunInfo machineGunInfo, int index)
	{
		SetMachineGunLevel(index, machineGunInfo.gunInfo.level);
		SetMachineGunNum(index, machineGunInfo.gunNum);
		SetMachineGunBulletNum(index, machineGunInfo.gunInfo.restBulletNum);
	}

	public static void GetMachineGunInfo(ItemGunInfo info, int index)
	{
		info.gunInfo.level = GetMachineGunLevel(index);
		info.gunNum = GetMachineGunNum(index);
		info.gunInfo.accuracy = info.accuracyLevelList[info.gunInfo.level];
		info.gunInfo.damage = info.damageLevelList[info.gunInfo.level];
		info.gunInfo.bulletNum = info.bulletNumLevelList[info.gunInfo.level];
		info.gunInfo.shotInterval = info.shotIntervalLevelList[info.gunInfo.level];
		info.gunInfo.reloadTime = info.reloadLevelList[info.gunInfo.level];
		info.gunInfo.restBulletNum = GetMachineGunBulletNum(index);
		if (info.gunInfo.restBulletNum >= info.gunInfo.maxBulletNum)
		{
			info.gunInfo.restBulletNum = info.gunInfo.maxBulletNum;
			SetMachineGunBulletNum(index, info.gunInfo.maxBulletNum);
		}
		if (info.gunNum > 0)
		{
			info.buyFlag = true;
		}
		else
		{
			info.buyFlag = false;
		}
		if (GlobalInf.gameLevel >= info.unlockByLevel)
		{
			info.unlockFlag = true;
		}
		else if (info.unlockByLevel == 99)
		{
			if (GlobalInf.collectMachineGunDoneFlag)
			{
				info.unlockFlag = true;
			}
			else
			{
				info.unlockFlag = false;
			}
		}
		else if (info.gunNum > 0)
		{
			info.unlockFlag = true;
		}
		else
		{
			info.unlockFlag = false;
		}
	}

	public static int GetMachineGunBulletNum(int gunIndex)
	{
		return PlayerPrefs.GetInt("machineGunBulletNum_" + gunIndex, 900);
	}

	public static void SetMachineGunBulletNum(int gunIndex, int bulletNum)
	{
		PlayerPrefs.SetInt("machineGunBulletNum_" + gunIndex, bulletNum);
	}

	public static void SetCarInfoList(ItemCarInfo[] carList)
	{
		for (int i = 0; i < carList.Length; i++)
		{
			SetCarInfo(carList[i], i);
		}
	}

	public static void GetCarInfoList(ItemCarInfo[] carList)
	{
		for (int i = 0; i < carList.Length; i++)
		{
			GetCarInfo(carList[i], i);
		}
	}

	public static void SetCarInfo(ItemCarInfo carInfo, int index)
	{
		SetCarLevel(index, carInfo.carLevel);
		SetCarNum(index, carInfo.carNum);
		SetCarHealthNum(index, carInfo.carInfo.restHealthVal);
	}

	public static int GetCarLevel(int index)
	{
		return PlayerPrefs.GetInt("carLevel_" + index, 0);
	}

	public static void SetCarLevel(int index, int level)
	{
		PlayerPrefs.SetInt("carLevel_" + index, level);
	}

	public static int GetCarNum(int carIndex)
	{
		return PlayerPrefs.GetInt("carNum_" + carIndex, 0);
	}

	public static void SetCarNum(int carIndex, int num)
	{
		PlayerPrefs.SetInt("carNum_" + carIndex, num);
	}

	public static int GetCarHealthNum(int carIndex)
	{
		int num = PlayerPrefs.GetInt("carHealthNum_" + carIndex, 900);
		if (num <= 0)
		{
			num = 10;
		}
		return num;
	}

	public static void SetCarHealthNum(int carIndex, int healthNum)
	{
		PlayerPrefs.SetInt("carHealthNum_" + carIndex, healthNum);
	}

	public static void GetCarInfo(ItemCarInfo info, int index)
	{
		info.carLevel = GetCarLevel(index);
		info.carNum = GetCarNum(index);
		info.carInfo.brakeForce = info.brakeForceLevelList[info.carLevel];
		info.carInfo.maxSpeed = info.maxSpeedLevelList[info.carLevel];
		info.carInfo.maxSpeedSteerAngle = info.maxSpeedSteerAngleLevelList[info.carLevel];
		info.carInfo.maxSteerAngle = info.maxSteerAngleLevelList[info.carLevel];
		info.carInfo.maxHealthVal = info.maxHealthLevelList[info.carLevel];
		info.carInfo.restHealthVal = GetCarHealthNum(index);
		if (info.carInfo.restHealthVal > info.carInfo.maxHealthVal)
		{
			info.carInfo.restHealthVal = info.carInfo.maxHealthVal;
		}
		if (info.carPrise != 0)
		{
			if (!info.carGoldFlag)
			{
				info.repairePrise = (int)(((float)info.carInfo.maxHealthVal - (float)info.carInfo.restHealthVal) / (float)info.carInfo.maxHealthVal * (float)info.carPrise * 0.2f);
			}
			else
			{
				info.repairePrise = (int)(((float)info.carInfo.maxHealthVal - (float)info.carInfo.restHealthVal) / (float)info.carInfo.maxHealthVal * (float)info.carPrise * 0.2f * 250f);
			}
			if (index == 5)
			{
				info.repairePrise = 0;
			}
		}
		else
		{
			info.repairePrise = (int)(((float)info.carInfo.maxHealthVal - (float)info.carInfo.restHealthVal) / (float)info.carInfo.maxHealthVal * 25000f * 0.2f);
		}
		if (info.carNum > 0)
		{
			info.buyFlag = true;
		}
		else
		{
			info.buyFlag = false;
		}
		if (GlobalInf.gameLevel >= info.unlockByLevel)
		{
			info.unlockFlag = true;
		}
		else if (info.unlockByLevel == 99)
		{
			if (GlobalInf.collectCarDoneFlag)
			{
				info.unlockFlag = true;
			}
			else
			{
				info.unlockFlag = false;
			}
		}
		else if (info.carNum > 0)
		{
			info.unlockFlag = true;
		}
		else
		{
			info.unlockFlag = false;
		}
	}

	public static void GetCash()
	{
		GlobalInf.cash = Platform.readCash();
	}

	public static void SetCash()
	{
		Platform.saveCash(GlobalInf.cash);
		if (TopLineController.instance != null)
		{
			TopLineController.instance.RefreshCash();
		}
	}

	public static void GetGold()
	{
		GlobalInf.gold = Platform.readGold();
	}

	public static void SetGold()
	{
		Platform.saveGold(GlobalInf.gold);
		if (TopLineController.instance != null)
		{
			TopLineController.instance.RefreshGold();
		}
	}

	public static void SetCarIndex(int carIndex)
	{
		PlayerPrefs.SetInt("playerCarIndex", carIndex);
	}

	public static int GetCarIndex()
	{
		return PlayerPrefs.GetInt("playerCarIndex", -1);
	}

	public static void SetPreCarIndex(int carIndex)
	{
		PlayerPrefs.SetInt("preCarIndex", carIndex);
	}

	public static int GetPreCarIndex()
	{
		return PlayerPrefs.GetInt("preCarIndex", 0);
	}

	public static void SetHandGunIndex(int handgunIndex)
	{
		PlayerPrefs.SetInt("handgunIndex", handgunIndex);
	}

	public static int GetHandGunIndex()
	{
		return PlayerPrefs.GetInt("handgunIndex", -1);
	}

	public static void SetMachineGunIndex(int machinegunIndex)
	{
		PlayerPrefs.SetInt("machinegunIndex", machinegunIndex);
	}

	public static int GetMachineGunIndex()
	{
		return PlayerPrefs.GetInt("machinegunIndex", -1);
	}

	public static void SetTaskHighestScore(TaskInfo taskInfo)
	{
		string key = string.Concat(string.Empty, taskInfo.taskMode, "_", taskInfo.taskLevel, "_", taskInfo.taskIndex);
		PlayerPrefs.SetInt(key, taskInfo.highestScore);
	}

	public static void GetTaskHighestScore(TaskInfo taskInfo)
	{
		string key = string.Concat(string.Empty, taskInfo.taskMode, "_", taskInfo.taskLevel, "_", taskInfo.taskIndex);
		taskInfo.highestScore = PlayerPrefs.GetInt(key, 0);
	}

	public static int GetGameLevel()
	{
		return PlayerPrefs.GetInt("GameLevel", 0);
	}

	public static void SetGameLevel(int level)
	{
		PlayerPrefs.SetInt("GameLevel", level);
	}

	public static void SetHealthKitNum(int num)
	{
		PlayerPrefs.SetInt("HealthKit", num);
	}

	public static int GetHealthKitNum()
	{
		return PlayerPrefs.GetInt("HealthKit", 0);
	}

	public static void SetToolKitNum(int num)
	{
		PlayerPrefs.SetInt("ToolKit", num);
	}

	public static int GetToolKitNum()
	{
		return PlayerPrefs.GetInt("ToolKit", 0);
	}

	public static int GetControlType()
	{
		return PlayerPrefs.GetInt("controlType", 0);
	}

	public static void SetControlType(bool isGravity)
	{
		if (isGravity)
		{
			PlayerPrefs.SetInt("controlType", 1);
		}
		else
		{
			PlayerPrefs.SetInt("controlType", 0);
		}
	}

	public static void SetCountData()
	{
		SetTotalTimeSpent(GlobalInf.totalTimeSpent);
		SetDrivingDistance(GlobalInf.drivingDistance);
		SetPoliceKillNum(GlobalInf.policeKillNum);
		SetTotalCashEarned(GlobalInf.totalCashEarned);
		SetTotalCashSpent();
		SetTotalKill(GlobalInf.totalKillNum);
		SetTotalKillCitizens(GlobalInf.totalKillCitizens);
		SetTotalKillGangsters(GlobalInf.totalKillGangsters);
		SetPunchKillNum(GlobalInf.punchKillNum);
		SetHandGunKillNum(GlobalInf.handGunKillNum);
		SetMachineGunKillNum(GlobalInf.machineGunKillNum);
		SetCarKillNum(GlobalInf.carKillNum);
		SetCompleteTaskNum(GlobalInf.completeTaskNum);
		SetCompleteDifTaskNum(GlobalInf.completeDifTaskNum);
		SetThreeStarTaskNum(GlobalInf.threeStarTaskNum);
		SetDailyCompleteTaskNum(GlobalInf.dailyCompleteTaskNum);
		SetDailyDieNum(GlobalInf.dailyDieNum);
		SetDailyDriveDis(GlobalInf.dailyDriveDis);
		SetDailyKillGangstarNum(GlobalInf.dailyKillGangstarNum);
		SetDailyKillNum(GlobalInf.dailyKillNum);
		SetDailyKillPoliceNum(GlobalInf.dailyKillPoliceNum);
		SetDailyKnockDownLightNum(GlobalInf.dailyKnockDownLight);
		SetDailyTimeSpent(GlobalInf.dailyPlayTime);
		SetDailyUpgradeNum(GlobalInf.dailyUpgradeItemNum);
	}

	public static void SetTotalTimeSpent(int val)
	{
		PlayerPrefs.SetInt("TotalTimeSpent", val);
	}

	public static int GetTotalTimeSpent()
	{
		return PlayerPrefs.GetInt("TotalTimeSpent", 0);
	}

	public static void SetDailyTimeSpent(int val)
	{
		PlayerPrefs.SetInt("DailyTimeSpent", val);
	}

	public static int GetDailyTimeSpent()
	{
		return PlayerPrefs.GetInt("DailyTimeSpent", 0);
	}

	public static void SetDailyCompleteTaskNum(int val)
	{
		PlayerPrefs.SetInt("DailyCompleteTaskNum", val);
	}

	public static int GetDailyCompleteTaskNum()
	{
		return PlayerPrefs.GetInt("DailyCompleteTaskNum", 0);
	}

	public static void SetDailyKillGangstarNum(int val)
	{
		PlayerPrefs.SetInt("DailyKillGangstarNum", val);
	}

	public static int GetDailyKillGangstarNum()
	{
		return PlayerPrefs.GetInt("DailyKillGangstarNum", 0);
	}

	public static void SetDailyKillPoliceNum(int val)
	{
		PlayerPrefs.SetInt("DailyKillPoliceNum", val);
	}

	public static int GetDailyKillPoliceNum()
	{
		return PlayerPrefs.GetInt("DailyKillPoliceNum", 0);
	}

	public static void SetDailyKnockDownLightNum(int val)
	{
		PlayerPrefs.SetInt("DailyKnockDownLightNum", val);
	}

	public static int GetDailyKnockDownLightNum()
	{
		return PlayerPrefs.GetInt("DailyKnockDownLightNum", 0);
	}

	public static void SetDailyUpgradeNum(int val)
	{
		PlayerPrefs.SetInt("DailyUpgradeNum", val);
	}

	public static int GetDailyUpgradeNum()
	{
		return PlayerPrefs.GetInt("DailyUpgradeNum", 0);
	}

	public static void SetDailyDieNum(int val)
	{
		PlayerPrefs.SetInt("DailyDieNum", val);
	}

	public static int GetDailyDieNum()
	{
		return PlayerPrefs.GetInt("DailyDieNum", 0);
	}

	public static void SetTotalKill(int num)
	{
		PlayerPrefs.SetInt("TotalKill", num);
	}

	public static int GetTotalKill()
	{
		return PlayerPrefs.GetInt("TotalKill", 0);
	}

	public static void SetTotalKillCitizens(int num)
	{
		PlayerPrefs.SetInt("TotalKillCityzens", num);
	}

	public static int GetDailyKillNum()
	{
		return PlayerPrefs.GetInt("DailyKillNum", 0);
	}

	public static void SetDailyKillNum(int num)
	{
		PlayerPrefs.SetInt("DailyKillNum", num);
	}

	public static int GetTotalKillCitizens()
	{
		return PlayerPrefs.GetInt("TotalKillCityzens", 0);
	}

	public static void SetTotalKillGangsters(int num)
	{
		PlayerPrefs.SetInt("TotalKillGangsetrs", num);
	}

	public static int GetTotalKillGangsters()
	{
		return PlayerPrefs.GetInt("TotalKillGangsetrs", 0);
	}

	public static void SetPunchKillNum(int num)
	{
		PlayerPrefs.SetInt("PunchKillNum", num);
	}

	public static int GetPunchKillNum()
	{
		return PlayerPrefs.GetInt("PunchKillNum", 0);
	}

	public static void SetHandGunKillNum(int num)
	{
		PlayerPrefs.SetInt("HandGunKillNum", num);
	}

	public static int GetHandGunKillNum()
	{
		return PlayerPrefs.GetInt("HandGunKillNum", 0);
	}

	public static void SetMachineGunKillNum(int num)
	{
		PlayerPrefs.SetInt("MachineGunKillNum", num);
	}

	public static int GetMachineGunKillNum()
	{
		return PlayerPrefs.GetInt("MachineGunKillNum", 0);
	}

	public static void SetCarKillNum(int num)
	{
		PlayerPrefs.SetInt("CarKillNum", num);
	}

	public static int GetCarKillNum()
	{
		return PlayerPrefs.GetInt("CarKillNum", 0);
	}

	public static void SetDriveDisInCar(float val)
	{
		PlayerPrefs.SetFloat("DriveDisInCar", val);
	}

	public static float GetDriveDisInCar()
	{
		return PlayerPrefs.GetFloat("DriveDisInCar", 0f);
	}

	public static void SetDriveDisInSportsCar(float val)
	{
		PlayerPrefs.SetFloat("DriveDisInSportsCar", val);
	}

	public static float GetDriveDisInSportsCar()
	{
		return PlayerPrefs.GetFloat("DriveDisInSportsCar", 0f);
	}

	public static void SetDriveDisInSUV(float val)
	{
		PlayerPrefs.SetFloat("DriveDisInSUV", val);
	}

	public static float GetDriveDisInSUV()
	{
		return PlayerPrefs.GetFloat("DriveDisInSUV", 0f);
	}

	public static void SetDriveDisInPoliceCar(float val)
	{
		PlayerPrefs.SetFloat("DriveDisInPoliceCar", val);
	}

	public static float GetDriveDisInPoliceCar()
	{
		return PlayerPrefs.GetFloat("DriveDisInPoliceCar", 0f);
	}

	public static void SetDriveDisInTaxi(float val)
	{
		PlayerPrefs.SetFloat("DriveDisInTaxi", val);
	}

	public static float GetDriveDisInTaxi()
	{
		return PlayerPrefs.GetFloat("DriveDisInTaxi", 0f);
	}

	public static void SetCompleteTaskNum(int val)
	{
		PlayerPrefs.SetInt("CompleteTaskNum", val);
	}

	public static int GetCompleteTaskNum()
	{
		return PlayerPrefs.GetInt("CompleteTaskNum", 0);
	}

	public static void SetCompleteDifTaskNum(int val)
	{
		PlayerPrefs.SetInt("CompleteDifTaskNum", val);
	}

	public static int GetCompleteDifTaskNum()
	{
		return PlayerPrefs.GetInt("CompleteDifTaskNum", 0);
	}

	public static void SetThreeStarTaskNum(int val)
	{
		PlayerPrefs.SetInt("ThreeStarTaskNum", val);
	}

	public static int GetThreeStarTaskNum()
	{
		return PlayerPrefs.GetInt("ThreeStarTaskNum", 0);
	}

	public static void SetTotalCashEarned(int num)
	{
		PlayerPrefs.SetInt("TotalCashEarned", num);
	}

	public static int GetTotalCashEarned()
	{
		return PlayerPrefs.GetInt("TotalCashEarned", 0);
	}

	public static void SetTotalGoldEarned()
	{
		PlayerPrefs.SetInt("TotalGoldEarned", GlobalInf.totalGoldEarned);
	}

	public static int GetTotalGoldEarned()
	{
		return PlayerPrefs.GetInt("TotalGoldEarned", 0);
	}

	public static void SetTotalCashSpent()
	{
		PlayerPrefs.SetInt("TotalCashSpent", GlobalInf.totalCashSpent);
	}

	public static int GetTotalCashSpent()
	{
		return PlayerPrefs.GetInt("TotalCashSpent", 0);
	}

	public static void SetDrivingDistance(float val)
	{
		PlayerPrefs.SetFloat("DrivingDistance", val);
	}

	public static float GetDrivingDistance()
	{
		return PlayerPrefs.GetFloat("DrivingDistance", 0f);
	}

	public static void SetDailyDriveDis(float val)
	{
		PlayerPrefs.SetFloat("DailyDriveDis", val);
	}

	public static float GetDailyDriveDis()
	{
		return PlayerPrefs.GetFloat("DailyDriveDis", 0f);
	}

	public static void SetPoliceKillNum(int val)
	{
		PlayerPrefs.SetInt("PoliceKillNum", val);
	}

	public static int GetPoliceKillNum()
	{
		return PlayerPrefs.GetInt("PoliceKillNum", 0);
	}

	public static void SetTaskIndex()
	{
		string text = string.Empty;
		for (int i = 0; i < GlobalInf.dailyTaskIndex.Length; i++)
		{
			string text2 = text;
			text = text2 + string.Empty + GlobalInf.dailyTaskIndex[i] + "|";
		}
		PlayerPrefs.SetString("DailyTaskIndex", text);
	}

	public static int[] GetTaskIndex()
	{
		int[] array = new int[GlobalInf.DAILY_TASK_INDEX_NUM];
		string @string = PlayerPrefs.GetString("DailyTaskIndex", "0|1|2|");
		string[] array2 = @string.Split("|"[0]);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Convert.ToInt32(array2[i]);
		}
		return array;
	}

	public static void SetHartCount(int val)
	{
		PlayerPrefs.SetInt("HartCount", val);
	}

	public static int GetHartCount()
	{
		return PlayerPrefs.GetInt("HartCount", 3);
	}

	public static void ResetDailyTask()
	{
		SetDailyDriveDis(0f);
		SetDailyKillNum(0);
		SetDailyTimeSpent(0);
		SetDailyCompleteTaskNum(0);
		SetDailyDieNum(0);
		SetDailyKillGangstarNum(0);
		SetDailyKillPoliceNum(0);
		SetDailyKnockDownLightNum(0);
		SetDailyUpgradeNum(0);
		GlobalInf.dailyDriveDis = 0f;
		GlobalInf.dailyCompleteTaskNum = 0;
		GlobalInf.dailyKillNum = 0;
		GlobalInf.dailyKillGangstarNum = 0;
		GlobalInf.dailyKillPoliceNum = 0;
		GlobalInf.dailyPlayTime = 0;
		GlobalInf.dailyKnockDownLight = 0;
		GlobalInf.dailyUpgradeItemNum = 0;
		GlobalInf.dailyDieNum = 0;
		GlobalInf.dailyTaskIndex = GetRandomArray(GlobalInf.DAILY_TASK_INDEX_NUM, GlobalInf.DAILY_TASK_NUM);
		for (int i = 0; i < GlobalInf.dailyTaskCompleteIndex.Length; i++)
		{
			GlobalInf.dailyTaskCompleteIndex[i] = 0;
		}
		SetTaskIndex();
		SetDailyTaskCompleteIndex();
	}

	public static void SetDailyTaskCompleteIndex()
	{
		string text = string.Empty;
		for (int i = 0; i < GlobalInf.dailyTaskCompleteIndex.Length; i++)
		{
			string text2 = text;
			text = text2 + string.Empty + GlobalInf.dailyTaskCompleteIndex[i] + "|";
		}
		PlayerPrefs.SetString("DailyTaskCompleteNum", text);
	}

	public static int[] GetDailyTaskCompleteIndex()
	{
		int[] array = new int[GlobalInf.DAILY_TASK_INDEX_NUM];
		string @string = PlayerPrefs.GetString("DailyTaskCompleteNum", "0|0|0|");
		string[] array2 = @string.Split("|"[0]);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Convert.ToInt32(array2[i]);
		}
		return array;
	}

	public static int[] GetRandomArray(int num, int maxNum)
	{
		int[] array = new int[maxNum - 1];
		int[] array2 = new int[num];
		for (int i = 0; i < maxNum - 1; i++)
		{
			array[i] = i;
		}
		for (int j = 0; j < num - 2; j++)
		{
			int num2 = UnityEngine.Random.Range(0, maxNum - 1 - j);
			array2[j] = array[num2];
			array[num2] = array[maxNum - 1 - 1 - j];
		}
		array[maxNum - 1 - 1] = maxNum - 1;
		for (int k = 1; k < num; k++)
		{
			int num2 = UnityEngine.Random.Range(0, maxNum - k);
			array2[num - k] = array[num2];
			array[num2] = array[maxNum - 1 - k];
		}
		return array2;
	}

	public static void ResumeDailyTask()
	{
		GlobalInf.dailyDriveDis = GetDailyDriveDis();
		GlobalInf.dailyKillNum = GetDailyKillNum();
		GlobalInf.dailyPlayTime = GetDailyTimeSpent();
		GlobalInf.dailyCompleteTaskNum = GetDailyCompleteTaskNum();
		GlobalInf.dailyKillGangstarNum = GetDailyKillGangstarNum();
		GlobalInf.dailyDieNum = GetDailyDieNum();
		GlobalInf.dailyKillPoliceNum = GetDailyKillPoliceNum();
		GlobalInf.dailyKnockDownLight = GetDailyKnockDownLightNum();
		GlobalInf.dailyUpgradeItemNum = GetDailyUpgradeNum();
		GlobalInf.dailyTaskIndex = GetTaskIndex();
		GlobalInf.dailyTaskCompleteIndex = GetDailyTaskCompleteIndex();
	}

	public static void SetLastInfo(int taskMode, int index)
	{
		PlayerPrefs.SetInt("LastInfoMode", taskMode);
		PlayerPrefs.SetInt("LastInfoIndex", index);
	}

	public static void GetLastInfo()
	{
		if (!GlobalInf.newUserFlag)
		{
			GlobalInf.lastMode = (GAMEMODE)PlayerPrefs.GetInt("LastInfoMode", 5);
		}
		else
		{
			GlobalInf.lastMode = (GAMEMODE)PlayerPrefs.GetInt("LastInfoMode", 5);
		}
		GlobalInf.lastIndex = PlayerPrefs.GetInt("LastInfoIndex", 0);
	}

	public static void SetClothIndex(int index)
	{
		PlayerPrefs.SetInt("ClothIndex", index);
	}

	public static void GetClothIndex()
	{
		GlobalInf.clothIndex = PlayerPrefs.GetInt("ClothIndex", 0);
	}

	public static int[] GetClothBuyFlag()
	{
		int[] array = new int[5];
		string @string = PlayerPrefs.GetString("ClothBuyFlag", "1|0|0|0|0|0|");
		string[] array2 = @string.Split("|"[0]);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Convert.ToInt32(array2[i]);
		}
		return array;
	}

	public static void SetClothBuyFlag(int[] result)
	{
		string text = string.Empty + result[0] + "|";
		for (int i = 1; i < result.Length; i++)
		{
			string text2 = text;
			text = text2 + string.Empty + result[i] + "|";
		}
		text += "0|";
		PlayerPrefs.SetString("ClothBuyFlag", text);
	}

	public static void SetPlayerHealthVal()
	{
		PlayerPrefs.SetInt("PlayerHealthVal", GlobalInf.playerHealthVal);
	}

	public static void GetPlayerHealthVal()
	{
		GlobalInf.playerHealthVal = PlayerPrefs.GetInt("PlayerHealthVal", 100);
		if (GlobalInf.playerHealthVal < 0)
		{
			GlobalInf.playerHealthVal = 1;
		}
	}

	public static void SetGoldVideoCount()
	{
		PlayerPrefs.SetInt("GoldVideoCount", GlobalInf.goldVideoCount);
	}

	public static void GetGoldVideoCount()
	{
		GlobalInf.goldVideoCount = PlayerPrefs.GetInt("GoldVideoCount", 0);
	}
}
