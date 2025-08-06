using UnityEngine;

public class GlobalInf : MonoBehaviour
{
	public static bool firstOpenGameFlag = false;

	public static bool startInitGameFlag = false;

	public static CARCTRLTYPE carCtrlType;

	public static bool isDrawPathInfo;

	public static bool mapMode;

	public static string nextSence;

	public static int loadingProgress;

	public static int handgunIndex = -1;

	public static int machineGunIndex = -1;

	public static GunsInfo handGunInfo;

	public static GunsInfo machineGunInfo;

	public static CarInfo carInfo;

	public static CarInfo preCarInfo;

	public static Vector2 playerCurBlock;

	public static int cash;

	public static int gold;

	public static int gameLevel;

	public static int healthKitNum;

	public static int toolKitNum;

	public static int totalTimeSpent;

	public static int totalKillNum;

	public static int totalCashEarned;

	public static int totalCashSpent;

	public static int totalGoldEarned;

	public static float drivingDistance;

	public static int policeKillNum;

	public static int totalKillCitizens;

	public static int totalKillGangsters;

	public static int punchKillNum;

	public static int handGunKillNum;

	public static int machineGunKillNum;

	public static int carKillNum;

	public static int completeTaskNum;

	public static int completeDifTaskNum;

	public static int threeStarTaskNum;

	public static int startGameTime;

	public static int playerCarIndex = -1;

	public static int preCarIndex;

	public static int[] collectHandGunData = new int[60];

	public static int[] collectMachineGunData = new int[60];

	public static int[] collectCarData = new int[60];

	public static int collectHandGunNum;

	public static int collectMachineGunNum;

	public static int collectCarNum;

	public static float musicVolume;

	public static float soundVolume;

	public static bool collectHandGunDoneFlag;

	public static bool collectMachineGunDoneFlag;

	public static bool collectCarDoneFlag;

	public static bool firstOpenDailyBounseFlag;

	public static bool musicFlag;

	public static bool soundFlag;

	public static int showADScreenNum;

	public static int gameState;

	public static int curKill;

	public static float curDistance;

	public static int curStartTime;

	public static float dailyDriveDis;

	public static int dailyCompleteTaskNum;

	public static int dailyKillNum;

	public static int dailyKillGangstarNum;

	public static int dailyKillPoliceNum;

	public static int dailyPlayTime;

	public static int dailyKnockDownLight;

	public static int dailyUpgradeItemNum;

	public static int dailyDieNum;

	public static int DAILY_TASK_NUM = 9;

	public static int DAILY_TASK_INDEX_NUM = 3;

	public static int[] dailyTaskIndex = new int[DAILY_TASK_INDEX_NUM];

	public static int[] dailyTaskCompleteIndex = new int[DAILY_TASK_INDEX_NUM];

	public static bool dailyTaskInitFlag = false;

	public static int hartCount;

	public static int preHartCount;

	public static bool collectingTipsFlag;

	public static bool healthKitTutorialFlag;

	public static bool toolKitTutorialFlag;

	public static bool backToHandGunPageFlag;

	public static bool backToCarPageFlag;

	public static bool upgradeTutorialFlag;

	public static bool notificationFlag;

	public static CHARGESHOWTYPE chargeShowType;

	public static GAMEMODE lastMode;

	public static int lastIndex;

	public static int clothIndex;

	public static bool newUserFlag;

	public static int playerHealthVal;

	public static bool playerRagdollFlag;

	public static bool restarFlag;

	public static bool onFetchCompletedFlag;

	public static bool onPlayCompletedFlag;

	public static VIDEOTYPE videoType;

	public static bool adsHandGunBulletFlag;

	public static bool adsMachineGunBulletFlag;

	public static bool adsHealthKitFlag;

	public static bool adsToolKitFlag;

	public static bool videoAdsFlag;

	public static bool cashVideoFlag;

	public static int goldVideoCount;

	public static int GetCurTime()
	{
		return (int)Time.time - curStartTime;
	}
}
