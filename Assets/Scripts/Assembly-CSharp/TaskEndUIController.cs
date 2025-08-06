using System;
using UnityEngine;

public class TaskEndUIController : MonoBehaviour
{
	public UIEventListener okBtn;

	public UIEventListener restartBtn;

	public UILabel taskTypeLabel;

	public UILabel taskLevelLabel;

	public int taskLevel;

	public UILabel scoresLabel;

	public UILabel scoresNextStarLabel;

	public UILabel bestScoreLabel;

	public UILabel scoresLabelNum;

	public UILabel scoresNextStarLabelNum;

	public UILabel bestScoreLabelNum;

	public UILabel moneyLabel;

	public UILabel kitLabel;

	public UILabel itemLabel;

	public UISprite cashPic;

	public UISprite goldPic;

	public UISprite kitPic;

	public UISprite handGunPic;

	public UISprite machineGunPic;

	public UISprite carPic;

	public GameObject[] stars;

	public GameObject[] emptyStars;

	public UISprite gameLabelPic;

	public TweenPosition star1TweenPos;

	public TweenPosition star3TweenPos;

	public int starNum;

	public int score;

	public int nextStarScore;

	public GAMEMODE gameMode;

	public int taskIndex;

	public bool failFlag;

	public TaskInfo taskInfo;

	private float labelTime;

	public bool enableBtn;

	public int firstGetNewStartIndex;

	public float lerpCountTime;

	public bool lerpFlag;

	public int scoreTarget;

	public int nextStartScoreTarget;

	public int bestScoreTarget;

	public bool cashFlag;

	public bool goldFlag;

	public bool kitFlag;

	public bool carFlag;

	public bool handGunFlag;

	public bool machineGunFlag;

	public LevelUpUIController levelUpUIController;

	public RatePageController ratePageController;

	public bool dialogFlag;

	public GameObject successObjRoot;

	public GameObject failObjRoot;

	public UIEventListener storeBtn;

	public bool levelUpUIFlag;

	private float beginTime;

	public int rewardMoney;

	public int healthToolKitFlag;

	public ITEMTYPE itemType;

	public int itemIndex;

	private void Awake()
	{
		Init();
		enableBtn = false;
		Platform.showFeatureView(FeatureViewPosType.MIDDLE);
		AudioController.instance.stop(AudioType.ENGINE);
		AudioController.instance.stop(AudioType.MACHINE_GUN);
		dialogFlag = false;
		GameUIController.instance.player_ShotPressFlag = false;
	}

	public void Init()
	{
		Time.timeScale = 0f;
		UIEventListener uIEventListener = okBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickOKBtn));
		UIEventListener uIEventListener2 = restartBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickRestartBtn));
		UIEventListener uIEventListener3 = storeBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickStoreBtn));
		GameUIController.instance.taskEndUIControllor = this;
		GameUIController.instance.DisableLocateLabel();
		GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.ENDMISSION);
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
		GameUIController.instance.resetPlayerCarBtn.gameObject.SetActiveRecursively(false);
		GameUIController.instance.outOfAmmoLabel.gameObject.SetActiveRecursively(false);
		GlobalInf.showADScreenNum++;
		levelUpUIFlag = false;
	}

	public void OnClickStoreBtn(GameObject btn)
	{
		if (enableBtn)
		{
			if (gameMode == GAMEMODE.SURVIVAL || gameMode == GAMEMODE.GUNKILLING || gameMode == GAMEMODE.FIGHTING || gameMode == GAMEMODE.ROBMOTOR)
			{
				GlobalInf.backToHandGunPageFlag = true;
				GlobalInf.backToCarPageFlag = false;
			}
			else
			{
				GlobalInf.backToHandGunPageFlag = false;
				GlobalInf.backToCarPageFlag = true;
			}
			GlobalInf.playerHealthVal = GameController.instance.citySenceHealthVal;
			GlobalInf.nextSence = "MenuSence";
			Application.LoadLevel("LoadingSence");
			if (PlayerController.instance.machineGun != null)
			{
				StoreDateController.SetMachineGunBulletNum(GlobalInf.machineGunIndex, PlayerController.instance.machineGun.gunInfo.restBulletNum + PlayerController.instance.machineGun.gunInfo.curBulletNum - PlayerController.instance.machineGun.bulletCount);
			}
			if (PlayerController.instance.gun != null)
			{
				StoreDateController.SetHandGunBulletNum(GlobalInf.handgunIndex, PlayerController.instance.gun.gunInfo.restBulletNum + PlayerController.instance.gun.gunInfo.curBulletNum - PlayerController.instance.gun.bulletCount);
			}
			GlobalInf.totalTimeSpent += (int)Time.time - GlobalInf.startGameTime;
			GlobalInf.dailyPlayTime += (int)Time.time - GlobalInf.startGameTime;
			GlobalInf.startGameTime = (int)Time.time;
			StoreDateController.SetCountData();
			Platform.hideFeatureView();
		}
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup - beginTime > labelTime && !gameLabelPic.gameObject.active)
		{
			gameLabelPic.gameObject.SetActiveRecursively(true);
			AudioController.instance.play(AudioType.STAR_LABEL);
			if (!Controller.instance.levelUpFlag && !dialogFlag && !GlobalInf.upgradeTutorialFlag)
			{
				enableBtn = true;
			}
			ADShowController.instance.showFlag = false;
			ADShowController.instance.countTime = 0f;
			if (!GlobalInf.upgradeTutorialFlag && !dialogFlag)
			{
				Platform.showFullScreenSmall();
			}
		}
		if (Controller.instance.levelUpFlag && Time.realtimeSinceStartup - beginTime > labelTime + 0.5f && !levelUpUIFlag)
		{
			levelUpUIFlag = true;
			GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.LEVELUP);
			levelUpUIController = ((GameObject)UnityEngine.Object.Instantiate(Resources.Load("UI/LevelUpUI"))).GetComponent<LevelUpUIController>();
		}
		if (dialogFlag && !levelUpUIFlag && Time.realtimeSinceStartup - beginTime > labelTime + 0.5f)
		{
			levelUpUIFlag = true;
			GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(false);
			GameUIController.instance.dialogUIController.Reset();
		}
		if (GlobalInf.upgradeTutorialFlag && !levelUpUIFlag && Time.realtimeSinceStartup - beginTime > labelTime + 0.5f)
		{
			levelUpUIFlag = true;
			GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(false);
			GameUIController.instance.dialogUIController.Reset();
		}
		if (Time.realtimeSinceStartup - beginTime > 2f && starNum > 2 && !stars[2].active)
		{
			stars[2].gameObject.SetActiveRecursively(true);
			AudioController.instance.play(AudioType.STAR);
		}
		if (Time.realtimeSinceStartup - beginTime > 1.6f && starNum > 1 && !stars[1].active)
		{
			stars[1].gameObject.SetActiveRecursively(true);
			AudioController.instance.play(AudioType.STAR);
		}
		if (Time.realtimeSinceStartup - beginTime > 1.2f)
		{
			if (starNum > 0 && !stars[0].active)
			{
				stars[0].gameObject.SetActiveRecursively(true);
				AudioController.instance.play(AudioType.STAR);
			}
		}
		else if (Time.realtimeSinceStartup - beginTime > 0.6f)
		{
			if (carFlag)
			{
				if (!carPic.gameObject.active)
				{
					carPic.gameObject.SetActiveRecursively(true);
					itemLabel.gameObject.SetActiveRecursively(true);
					if (!kitFlag)
					{
						carPic.transform.localPosition = new Vector3(-0.146f * GlobalDefine.screenRatioWidth, -53.00423f, 0f);
						itemLabel.transform.localPosition = new Vector3(0.08f * GlobalDefine.screenRatioWidth, -53.00423f, 0f);
					}
					else
					{
						carPic.transform.localPosition = new Vector3(-0.146f * GlobalDefine.screenRatioWidth, -94.00751f, 0f);
						itemLabel.transform.localPosition = new Vector3(0.08f * GlobalDefine.screenRatioWidth, -94.00751f, 0f);
					}
				}
			}
			else if (handGunFlag)
			{
				if (!handGunPic.gameObject.active)
				{
					handGunPic.gameObject.SetActiveRecursively(true);
					itemLabel.gameObject.SetActiveRecursively(true);
					if (!kitFlag)
					{
						handGunPic.transform.localPosition = new Vector3(-0.146f * GlobalDefine.screenRatioWidth, -53.00423f, 0f);
						itemLabel.transform.localPosition = new Vector3(0.08f * GlobalDefine.screenRatioWidth, -53.00423f, 0f);
					}
					else
					{
						handGunPic.transform.localPosition = new Vector3(-0.146f * GlobalDefine.screenRatioWidth, -94.00751f, 0f);
						itemLabel.transform.localPosition = new Vector3(0.08f * GlobalDefine.screenRatioWidth, -94.00751f, 0f);
					}
				}
			}
			else if (machineGunFlag && !machineGunPic.gameObject.active)
			{
				machineGunPic.gameObject.SetActiveRecursively(true);
				itemLabel.gameObject.SetActiveRecursively(true);
				if (!kitFlag)
				{
					machineGunPic.transform.localPosition = new Vector3(-0.146f * GlobalDefine.screenRatioWidth, -53.00423f, 0f);
					itemLabel.transform.localPosition = new Vector3(0.08f * GlobalDefine.screenRatioWidth, -53.00423f, 0f);
				}
				else
				{
					machineGunPic.transform.localPosition = new Vector3(-0.146f * GlobalDefine.screenRatioWidth, -94.00751f, 0f);
					itemLabel.transform.localPosition = new Vector3(0.08f * GlobalDefine.screenRatioWidth, -94.00751f, 0f);
				}
			}
		}
		else if (Time.realtimeSinceStartup - beginTime > 0.4f)
		{
			if (kitFlag && !kitPic.gameObject.active)
			{
				kitPic.gameObject.SetActiveRecursively(true);
				kitLabel.gameObject.SetActiveRecursively(true);
			}
		}
		else if (Time.realtimeSinceStartup - beginTime > 0.2f)
		{
			if (cashFlag)
			{
				if (!cashPic.gameObject.active)
				{
					cashPic.gameObject.SetActiveRecursively(true);
					moneyLabel.gameObject.SetActiveRecursively(true);
				}
			}
			else if (goldFlag && !goldPic.gameObject.active)
			{
				goldPic.gameObject.SetActiveRecursively(true);
				moneyLabel.gameObject.SetActiveRecursively(true);
			}
		}
		if (!lerpFlag)
		{
			return;
		}
		lerpCountTime = (Time.realtimeSinceStartup - beginTime) * 2f;
		if (!taskInfo.inverseScoresFlag)
		{
			scoresLabelNum.text = string.Empty + (int)Mathf.Lerp(0f, scoreTarget, lerpCountTime);
			bestScoreLabelNum.text = string.Empty + (int)Mathf.Lerp(0f, bestScoreTarget, lerpCountTime);
		}
		else
		{
			scoresLabelNum.text = string.Empty + (int)Mathf.Lerp(0f, scoreTarget, lerpCountTime) + "s";
			bestScoreLabelNum.text = string.Empty + (int)Mathf.Lerp(0f, bestScoreTarget, lerpCountTime) + "s";
		}
		if (nextStartScoreTarget != -1)
		{
			if (!taskInfo.inverseScoresFlag)
			{
				scoresNextStarLabelNum.text = string.Empty + (int)Mathf.Lerp(0f, nextStartScoreTarget, lerpCountTime);
			}
			else
			{
				scoresNextStarLabelNum.text = string.Empty + (int)Mathf.Lerp(0f, nextStartScoreTarget, lerpCountTime) + "s";
			}
		}
		if (lerpCountTime >= 1f)
		{
			lerpFlag = false;
			AudioController.instance.stop(AudioType.NUM_ROLL);
		}
	}

	private void Destroy()
	{
		if (GameUIController.instance.taskEndUIControllor == this)
		{
			GameUIController.instance.taskEndUIControllor = null;
		}
		Resources.UnloadUnusedAssets();
	}

	public void Reset(bool isFail, GAMEMODE mode, int tIndex, int sc, int rewardIndex)
	{
		beginTime = Time.realtimeSinceStartup;
		failFlag = isFail;
		gameMode = mode;
		taskIndex = tIndex;
		score = sc;
		lerpFlag = true;
		lerpCountTime = 0f;
		scoreTarget = score;
		switch (gameMode)
		{
		case GAMEMODE.DELIVER:
			taskInfo = TaskLabelController.instance.deliverTaskInfo[taskIndex];
			taskTypeLabel.text = "Deliver the Package";
			scoresLabel.text = "Time Left :";
			break;
		case GAMEMODE.DRIVING0:
			taskInfo = TaskLabelController.instance.drivingTaskInfo[taskIndex];
			taskTypeLabel.text = "Keep in Route";
			scoresLabel.text = "Time Left :";
			break;
		case GAMEMODE.SURVIVAL:
			taskInfo = TaskLabelController.instance.survivalTaskInfo[taskIndex];
			taskTypeLabel.text = "Survive in Desperate";
			scoresLabel.text = "Use Time :";
			break;
		case GAMEMODE.GUNKILLING:
			taskInfo = TaskLabelController.instance.gunKillingTaskInfo[taskIndex];
			taskTypeLabel.text = "Shooting Time";
			scoresLabel.text = "Total Kill :";
			break;
		case GAMEMODE.CARKILLING:
			taskInfo = TaskLabelController.instance.carKillingTaskInfo[taskIndex];
			taskTypeLabel.text = "Road of Fury";
			scoresLabel.text = "Total Kill :";
			break;
		case GAMEMODE.SKILLDRIVING:
			taskInfo = TaskLabelController.instance.skillDrivingTaskInfo[taskIndex];
			taskTypeLabel.text = "Driving Skill Test";
			scoresLabel.text = "Time Left :";
			break;
		case GAMEMODE.ROBCAR:
			taskInfo = TaskLabelController.instance.robbingCarTaskInfo[taskIndex];
			taskTypeLabel.text = "Car Robbery";
			scoresLabel.text = "Time Left :";
			break;
		case GAMEMODE.FIGHTING:
			taskInfo = TaskLabelController.instance.fightingTaskInfo[taskIndex];
			taskTypeLabel.text = "Kill Target";
			scoresLabel.text = "Time Left : ";
			break;
		case GAMEMODE.ROBMOTOR:
			taskInfo = TaskLabelController.instance.robMotorTaskInfo[taskIndex];
			taskTypeLabel.text = "Motor Robbery";
			scoresLabel.text = "Time Left : ";
			break;
		}
		firstGetNewStartIndex = 0;
		if (!failFlag)
		{
			if (!taskInfo.inverseScoresFlag)
			{
				if (sc >= taskInfo.highestScore)
				{
					taskInfo.highestScore = score;
					int num = taskInfo.starNum;
					TaskLabelController.instance.GetTaskStarNum(taskInfo);
					StoreDateController.SetTaskHighestScore(taskInfo);
					Controller.instance.sumStarNum += taskInfo.starNum - num;
					Controller.instance.CountLevel();
					if (num == 0 && (taskInfo.starNum == 1 || taskInfo.starNum == 2))
					{
						firstGetNewStartIndex = 1;
						GlobalInf.completeDifTaskNum++;
						if (GlobalInf.gameState == 1 && PlayerPrefs.GetInt("UpgradeTutorialFlag", 0) == 0)
						{
							GlobalInf.upgradeTutorialFlag = true;
							GlobalInf.cash += 200;
							StoreDateController.SetCash();
						}
						if (!GlobalInf.newUserFlag)
						{
							if (GlobalInf.gameState <= GameStateController.MAXSTATENUM)
							{
								GameStateController.instance.ChangeGameState();
							}
						}
						else if (GlobalInf.gameState <= GameStateController.NEWMAXSTATENUM)
						{
							GameStateController.instance.ChangeGameState();
						}
						Platform.flurryEvent_onTaskFirstWin((int)gameMode, taskIndex);
					}
					else if (num > 0 && taskInfo.starNum == 3)
					{
						firstGetNewStartIndex = 2;
					}
					else if (num == 0 && taskInfo.starNum == 3)
					{
						firstGetNewStartIndex = 3;
						GlobalInf.completeDifTaskNum++;
						if (GlobalInf.gameState == 1 && PlayerPrefs.GetInt("UpgradeTutorialFlag", 0) == 0)
						{
							GlobalInf.upgradeTutorialFlag = true;
							GlobalInf.cash += 200;
							StoreDateController.SetCash();
						}
						if (!GlobalInf.newUserFlag)
						{
							if (GlobalInf.gameState <= GameStateController.MAXSTATENUM)
							{
								GameStateController.instance.ChangeGameState();
							}
						}
						else if (GlobalInf.gameState <= GameStateController.NEWMAXSTATENUM)
						{
							GameStateController.instance.ChangeGameState();
						}
						Platform.flurryEvent_onTaskFirstWin((int)gameMode, taskIndex);
					}
				}
			}
			else if (sc <= taskInfo.highestScore || taskInfo.highestScore == 0)
			{
				taskInfo.highestScore = score;
				int num2 = taskInfo.starNum;
				TaskLabelController.instance.GetTaskStarNum(taskInfo);
				StoreDateController.SetTaskHighestScore(taskInfo);
				Controller.instance.sumStarNum += taskInfo.starNum - num2;
				Controller.instance.CountLevel();
				if (num2 == 0 && (taskInfo.starNum == 1 || taskInfo.starNum == 2))
				{
					firstGetNewStartIndex = 1;
					GlobalInf.completeDifTaskNum++;
					if (!GlobalInf.newUserFlag)
					{
						if (GlobalInf.gameState <= GameStateController.MAXSTATENUM)
						{
							GameStateController.instance.ChangeGameState();
						}
					}
					else if (GlobalInf.gameState <= GameStateController.NEWMAXSTATENUM)
					{
						GameStateController.instance.ChangeGameState();
					}
					Platform.flurryEvent_onTaskFirstWin((int)gameMode, taskIndex);
				}
				else if (num2 > 0 && taskInfo.starNum == 3)
				{
					firstGetNewStartIndex = 2;
				}
				else if (num2 == 0 && taskInfo.starNum == 3)
				{
					firstGetNewStartIndex = 3;
					GlobalInf.completeDifTaskNum++;
					if (!GlobalInf.newUserFlag)
					{
						if (GlobalInf.gameState <= GameStateController.MAXSTATENUM)
						{
							GameStateController.instance.ChangeGameState();
						}
					}
					else if (GlobalInf.gameState <= GameStateController.NEWMAXSTATENUM)
					{
						GameStateController.instance.ChangeGameState();
					}
					Platform.flurryEvent_onTaskFirstWin((int)gameMode, taskIndex);
				}
			}
			GlobalInf.completeTaskNum++;
		}
		starNum = 0;
		if (!failFlag)
		{
			if (!taskInfo.inverseScoresFlag)
			{
				if (!GlobalDefine.smallPhoneFlag)
				{
					for (int num3 = 2; num3 >= 0; num3--)
					{
						if (score >= taskInfo.starScore[num3])
						{
							starNum = num3 + 1;
							break;
						}
					}
				}
				else if (taskInfo.taskMode != GAMEMODE.GUNKILLING && taskInfo.taskMode != GAMEMODE.CARKILLING)
				{
					for (int num4 = 2; num4 >= 0; num4--)
					{
						if (score >= taskInfo.starScore[num4])
						{
							starNum = num4 + 1;
							break;
						}
					}
				}
				else
				{
					for (int num5 = 2; num5 >= 0; num5--)
					{
						if (score >= taskInfo.starScore[num5] / 2)
						{
							starNum = num5 + 1;
							break;
						}
					}
				}
			}
			else
			{
				for (int num6 = 2; num6 >= 0; num6--)
				{
					if (score <= taskInfo.starScore[num6])
					{
						starNum = num6 + 1;
						break;
					}
				}
			}
		}
		if (failFlag || starNum == 0)
		{
			NGUITools.SetActiveRecursively(successObjRoot, false);
			NGUITools.SetActiveRecursively(failObjRoot, true);
			okBtn.transform.localPosition = new Vector3(0.16f * GlobalDefine.screenRatioWidth, -149f, 0f);
			restartBtn.transform.localPosition = new Vector3(-0.16f * GlobalDefine.screenRatioWidth, -149f, 0f);
			Platform.flurryEvent_onTaskFail((int)gameMode, taskIndex);
		}
		else
		{
			NGUITools.SetActiveRecursively(successObjRoot, true);
			NGUITools.SetActiveRecursively(failObjRoot, false);
			okBtn.transform.localPosition = new Vector3(0.1f * GlobalDefine.screenRatioWidth, -149f, 0f);
			restartBtn.transform.localPosition = new Vector3(-0.1f * GlobalDefine.screenRatioWidth, -149f, 0f);
			Platform.flurryEvent_onTaskWin((int)gameMode, taskIndex);
		}
		if (starNum == 0)
		{
			scoresNextStarLabel.gameObject.SetActiveRecursively(false);
			scoresNextStarLabelNum.gameObject.SetActiveRecursively(false);
		}
		else if (starNum < taskInfo.starScore.Length)
		{
			scoresNextStarLabel.text = "Next Star :";
			if (GlobalDefine.smallPhoneFlag && (taskInfo.taskMode == GAMEMODE.CARKILLING || taskInfo.taskMode == GAMEMODE.GUNKILLING))
			{
				nextStartScoreTarget = taskInfo.starScore[starNum] / 2;
			}
			else
			{
				nextStartScoreTarget = taskInfo.starScore[starNum];
			}
		}
		else
		{
			scoresNextStarLabel.text = "You Have Got Three Stars!";
			scoresNextStarLabelNum.gameObject.SetActiveRecursively(false);
			nextStartScoreTarget = -1;
		}
		if (!taskInfo.inverseScoresFlag)
		{
			bestScoreTarget = taskInfo.highestScore;
			bestScoreLabel.text = "Best :";
		}
		else
		{
			bestScoreTarget = taskInfo.highestScore;
			bestScoreLabel.text = "Best :";
		}
		for (int i = 0; i < 3; i++)
		{
			stars[i].SetActiveRecursively(false);
		}
		star1TweenPos.to = stars[0].transform.localPosition;
		star1TweenPos.from = stars[0].transform.localPosition - new Vector3(50f, 0f, 0f);
		star3TweenPos.to = stars[2].transform.localPosition;
		star3TweenPos.from = stars[2].transform.localPosition + new Vector3(50f, 0f, 0f);
		if (starNum == 0)
		{
			gameLabelPic.spriteName = "bad";
		}
		else if (starNum == 1)
		{
			gameLabelPic.spriteName = "good";
			GlobalInf.dailyCompleteTaskNum++;
		}
		else if (starNum == 2)
		{
			gameLabelPic.spriteName = "excellent";
			GlobalInf.dailyCompleteTaskNum++;
		}
		else if (starNum == 3)
		{
			gameLabelPic.spriteName = "perfect";
			GlobalInf.threeStarTaskNum++;
			GlobalInf.dailyCompleteTaskNum++;
			Platform.flurryEvent_onTask3Stars((int)gameMode, taskIndex);
		}
		gameLabelPic.gameObject.SetActiveRecursively(false);
		labelTime = (float)starNum * 0.4f + 1f;
		moneyLabel.gameObject.SetActiveRecursively(false);
		cashPic.gameObject.SetActiveRecursively(false);
		goldPic.gameObject.SetActiveRecursively(false);
		cashFlag = false;
		goldFlag = false;
		if (firstGetNewStartIndex == 0)
		{
			if (starNum == 1)
			{
				rewardMoney = TaskRewardInfoList.instance.taskRewardList[rewardIndex].cashOneStar;
				GlobalInf.cash += rewardMoney;
				GlobalInf.totalCashEarned += rewardMoney;
				StoreDateController.SetCash();
				cashFlag = true;
			}
			else if (starNum == 2)
			{
				rewardMoney = TaskRewardInfoList.instance.taskRewardList[rewardIndex].cashTwoStar;
				GlobalInf.cash += rewardMoney;
				GlobalInf.totalCashEarned += rewardMoney;
				StoreDateController.SetCash();
				cashFlag = true;
			}
			else if (starNum == 3)
			{
				rewardMoney = TaskRewardInfoList.instance.taskRewardList[rewardIndex].cashThreeStar;
				GlobalInf.cash += rewardMoney;
				GlobalInf.totalCashEarned += rewardMoney;
				StoreDateController.SetCash();
				cashFlag = true;
			}
			moneyLabel.text = "Cash : " + rewardMoney;
		}
		else if (firstGetNewStartIndex == 1)
		{
			rewardMoney = TaskRewardInfoList.instance.taskRewardList[rewardIndex].firstGold;
			GlobalInf.gold += rewardMoney;
			StoreDateController.SetGold();
			GlobalInf.totalGoldEarned += rewardMoney;
			StoreDateController.SetTotalGoldEarned();
			moneyLabel.text = "Gold : " + rewardMoney;
			goldFlag = true;
		}
		else if (firstGetNewStartIndex == 2)
		{
			rewardMoney = TaskRewardInfoList.instance.taskRewardList[rewardIndex].threeStarGold;
			GlobalInf.gold += rewardMoney;
			StoreDateController.SetGold();
			GlobalInf.totalGoldEarned += rewardMoney;
			StoreDateController.SetTotalGoldEarned();
			moneyLabel.text = "Gold : " + rewardMoney;
			goldFlag = true;
		}
		else if (firstGetNewStartIndex == 3)
		{
			rewardMoney = TaskRewardInfoList.instance.taskRewardList[rewardIndex].threeStarGold;
			rewardMoney += TaskRewardInfoList.instance.taskRewardList[rewardIndex].firstGold;
			GlobalInf.gold += rewardMoney;
			StoreDateController.SetGold();
			GlobalInf.totalGoldEarned += rewardMoney;
			StoreDateController.SetTotalGoldEarned();
			moneyLabel.text = "Gold : " + rewardMoney;
			goldFlag = true;
		}
		healthToolKitFlag = 0;
		kitPic.gameObject.SetActiveRecursively(false);
		kitLabel.gameObject.SetActiveRecursively(false);
		kitFlag = false;
		if (starNum >= 2)
		{
			int num7 = UnityEngine.Random.Range(0, 100);
			if (num7 <= TaskRewardInfoList.instance.taskRewardList[rewardIndex].kitRate)
			{
				kitFlag = true;
				if (UnityEngine.Random.Range(0, 2) > 0)
				{
					healthToolKitFlag = 1;
					kitLabel.text = "HealthKit * 1";
				}
				else
				{
					healthToolKitFlag = 2;
					kitLabel.text = "ToolKit * 1";
				}
			}
		}
		if (healthToolKitFlag == 1)
		{
			GlobalInf.healthKitNum++;
			StoreDateController.SetHealthKitNum(GlobalInf.healthKitNum);
		}
		else if (healthToolKitFlag == 2)
		{
			GlobalInf.toolKitNum++;
			StoreDateController.SetToolKitNum(GlobalInf.toolKitNum);
		}
		handGunPic.gameObject.SetActiveRecursively(false);
		machineGunPic.gameObject.SetActiveRecursively(false);
		carPic.gameObject.SetActiveRecursively(false);
		itemLabel.gameObject.SetActiveRecursively(false);
		carFlag = false;
		handGunFlag = false;
		machineGunFlag = false;
		if (starNum == 3)
		{
			int num7 = UnityEngine.Random.Range(0, 100);
			for (int j = 0; j < TaskRewardInfoList.instance.taskRewardList[rewardIndex].itemList.Length; j++)
			{
				if (num7 > TaskRewardInfoList.instance.taskRewardList[rewardIndex].itemList[j].rate)
				{
					continue;
				}
				itemType = TaskRewardInfoList.instance.taskRewardList[rewardIndex].itemList[j].itemType;
				itemIndex = TaskRewardInfoList.instance.taskRewardList[rewardIndex].itemList[j].index;
				switch (itemType)
				{
				case ITEMTYPE.HANDGUN:
				{
					int carNum = StoreDateController.GetHandGunNum(itemIndex);
					if (carNum == 0)
					{
						Platform.flurryEvent_onEquipmentHandGunGetReward(itemIndex);
					}
					carNum++;
					StoreDateController.SetHandGunNum(itemIndex, carNum);
					itemLabel.text = TaskRewardInfoList.instance.handGunName[itemIndex];
					handGunFlag = true;
					break;
				}
				case ITEMTYPE.MACHINEGUN:
				{
					int carNum = StoreDateController.GetMachineGunNum(itemIndex);
					if (carNum == 0)
					{
						Platform.flurryEvent_onEquipmentMachineGunGetReward(itemIndex);
					}
					carNum++;
					StoreDateController.SetMachineGunNum(itemIndex, carNum);
					itemLabel.text = TaskRewardInfoList.instance.machineGunName[itemIndex];
					machineGunFlag = true;
					break;
				}
				case ITEMTYPE.CAR:
				{
					int carNum = StoreDateController.GetCarNum(itemIndex);
					if (carNum == 0)
					{
						Platform.flurryEvent_onEquipmentCarGetReward(itemIndex);
					}
					carNum++;
					StoreDateController.SetCarNum(itemIndex, carNum);
					itemLabel.text = TaskRewardInfoList.instance.carName[itemIndex];
					carFlag = true;
					break;
				}
				}
				break;
			}
		}
		AudioController.instance.play(AudioType.NUM_ROLL);
	}

	public void OnClickOKBtn(GameObject btn)
	{
		if (enableBtn)
		{
			Time.timeScale = 1f;
			BlackScreen.instance.TurnOffScreen();
			Invoke("ChangeMode", 0.5f);
			GameSenceBackBtnCtl.instance.PopGameUIState();
			Platform.hideFeatureView();
		}
	}

	public void ChangeMode()
	{
		if (dialogFlag)
		{
			Controller.instance.CountLevel();
		}
		GameController.instance.ChangeMode(GAMEMODE.NORMAL, 0);
		BlackScreen.instance.TurnOnScreen();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnClickRestartBtn(GameObject btn)
	{
		if (!enableBtn)
		{
			return;
		}
		Time.timeScale = 1f;
		if (GameController.instance.curMode == GameController.instance.driveMode)
		{
			GameController.instance.driveMode.restartFlag = true;
			GameUIController.instance.gameMode = GAMEMODE.DRIVING0;
			GameUIController.instance.OnClickTaskCheckBtn(null);
			Invoke("Restart", 0.5f);
		}
		else if (GameController.instance.curGameMode == GAMEMODE.DELIVER)
		{
			GameController.instance.deliverMode.finishLabel.SetActiveRecursively(false);
			GameController.instance.deliverMode.restartFlag = true;
			GameController.instance.curGameMode = GAMEMODE.DELIVER;
			GameUIController.instance.OnClickTaskCheckBtn(null);
			Invoke("Restart", 0.5f);
		}
		else if (GameController.instance.curMode == GameController.instance.survivalMode)
		{
			GameUIController.instance.gameMode = GAMEMODE.SURVIVAL;
			GameUIController.instance.OnClickTaskCheckBtn(null);
			Invoke("Restart", 0.5f);
		}
		else if (GameController.instance.curGameMode == GAMEMODE.GUNKILLING)
		{
			GameUIController.instance.gameMode = GAMEMODE.GUNKILLING;
			GameUIController.instance.OnClickTaskCheckBtn(null);
			GameController.instance.normalMode.ClearAI();
			Invoke("Restart", 0.5f);
		}
		else if (GameController.instance.curGameMode == GAMEMODE.CARKILLING)
		{
			GameController.instance.carKillingMode.restartFlag = true;
			GameUIController.instance.gameMode = GAMEMODE.CARKILLING;
			GameUIController.instance.OnClickTaskCheckBtn(null);
			if (PlayerController.instance.car != null)
			{
				PlayerController.instance.QuitGetOnCar();
				PlayerController.instance.car.OnDisableCar();
				PlayerController.instance.cam.OnChangeTarget(false);
				PlayerController.instance.GetOffCarDone();
				PlayerController.instance.transform.eulerAngles = new Vector3(0f, PlayerController.instance.transform.eulerAngles.y, 0f);
				GameUIController.instance.playerUI.gameObject.SetActiveRecursively(false);
				GameUIController.instance.controlUI.SetActiveRecursively(false);
			}
			GameController.instance.normalMode.ClearAI();
			Invoke("Restart", 0.5f);
		}
		else if (GameController.instance.curGameMode == GAMEMODE.SKILLDRIVING)
		{
			GameController.instance.skillDrivingMode.restartFlag = true;
			GameUIController.instance.gameMode = GAMEMODE.SKILLDRIVING;
			GameUIController.instance.OnClickTaskCheckBtn(null);
			Invoke("Restart", 0.5f);
		}
		else if (GameController.instance.curGameMode == GAMEMODE.ROBCAR)
		{
			GameController.instance.deliverMode.finishLabel.SetActiveRecursively(false);
			GameController.instance.robbingCarMode.restartFlag = true;
			GameUIController.instance.gameMode = GAMEMODE.ROBCAR;
			GameUIController.instance.OnClickTaskCheckBtn(null);
			Invoke("Restart", 0.5f);
		}
		else if (GameController.instance.curGameMode == GAMEMODE.FIGHTING)
		{
			GameUIController.instance.gameMode = GAMEMODE.FIGHTING;
			GameUIController.instance.OnClickTaskCheckBtn(null);
			Invoke("Restart", 0.5f);
		}
		else if (GameController.instance.curGameMode == GAMEMODE.ROBMOTOR)
		{
			GameController.instance.robMotorMode.finishLabel.SetActiveRecursively(false);
			GameController.instance.robMotorMode.restartFlag = true;
			GameUIController.instance.gameMode = GAMEMODE.ROBMOTOR;
			GameUIController.instance.OnClickTaskCheckBtn(null);
			Invoke("Restart", 0.5f);
		}
		GameSenceBackBtnCtl.instance.PopGameUIState();
		Platform.hideFeatureView();
		enableBtn = false;
		GlobalInf.restarFlag = true;
	}

	public void Restart()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
