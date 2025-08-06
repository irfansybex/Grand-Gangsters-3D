using UnityEngine;

public class TaskLabelController : MonoBehaviour
{
	public static TaskLabelController instance;

	public TaskInfo[] drivingTaskInfo;

	public TaskInfo[] deliverTaskInfo;

	public TaskInfo[] survivalTaskInfo;

	public TaskInfo[] gunKillingTaskInfo;

	public TaskInfo[] carKillingTaskInfo;

	public TaskInfo[] skillDrivingTaskInfo;

	public TaskInfo[] slotTaskInfo;

	public TaskInfo[] robbingCarTaskInfo;

	public TaskInfo[] fightingTaskInfo;

	public TaskInfo[] robMotorTaskInfo;

	public TaskBoxUIController taskLabelUI;

	public int sumStarNum;

	public TaskInfo lastInfo;

	public float countTime = 5f;

	public OnActiveActionCtl temp;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		lastInfo = GetLastTaskInfo(GlobalInf.lastMode, GlobalInf.lastIndex);
	}

	public TaskInfo GetLastTaskInfo(GAMEMODE mode, int index)
	{
		switch (mode)
		{
		case GAMEMODE.CARKILLING:
			return carKillingTaskInfo[index];
		case GAMEMODE.DELIVER:
			return deliverTaskInfo[index];
		case GAMEMODE.DRIVING0:
			return drivingTaskInfo[index];
		case GAMEMODE.GUNKILLING:
			return gunKillingTaskInfo[index];
		case GAMEMODE.SKILLDRIVING:
			return skillDrivingTaskInfo[index];
		case GAMEMODE.SURVIVAL:
			return survivalTaskInfo[index];
		case GAMEMODE.ROBCAR:
			return robbingCarTaskInfo[index];
		case GAMEMODE.FIGHTING:
			return fightingTaskInfo[index];
		case GAMEMODE.ROBMOTOR:
			return robMotorTaskInfo[index];
		default:
			return gunKillingTaskInfo[index];
		}
	}

	private void Start()
	{
		if (GlobalInf.firstOpenGameFlag)
		{
			base.gameObject.SetActiveRecursively(false);
			DisableFlag(GAMEMODE.EMPTY);
		}
	}

	public void InitTaskInfo()
	{
		SetTaskInfoVal(drivingTaskInfo);
		SetTaskInfoVal(deliverTaskInfo);
		SetTaskInfoVal(survivalTaskInfo);
		SetTaskInfoVal(gunKillingTaskInfo);
		SetTaskInfoVal(carKillingTaskInfo);
		SetTaskInfoVal(skillDrivingTaskInfo);
		SetTaskInfoVal(robbingCarTaskInfo);
		SetTaskInfoVal(fightingTaskInfo);
		SetTaskInfoVal(robMotorTaskInfo);
		CountSumStarNum();
	}

	public void SetTaskInfoVal(TaskInfo[] infoArray)
	{
		for (int i = 0; i < infoArray.Length; i++)
		{
			StoreDateController.GetTaskHighestScore(infoArray[i]);
			GetTaskStarNum(infoArray[i]);
		}
	}

	public void GetTaskStarNum(TaskInfo info)
	{
		info.starNum = 0;
		if (!info.inverseScoresFlag)
		{
			if (info.highestScore == 0)
			{
				return;
			}
			if (!GlobalDefine.smallPhoneFlag)
			{
				for (int num = 2; num >= 0; num--)
				{
					if (info.highestScore >= info.starScore[num])
					{
						info.starNum = num + 1;
						break;
					}
				}
				return;
			}
			if (info.taskMode != GAMEMODE.CARKILLING && info.taskMode != GAMEMODE.GUNKILLING)
			{
				for (int num2 = 2; num2 >= 0; num2--)
				{
					if (info.highestScore >= info.starScore[num2])
					{
						info.starNum = num2 + 1;
						break;
					}
				}
				return;
			}
			for (int num3 = 2; num3 >= 0; num3--)
			{
				if (info.highestScore >= info.starScore[num3] / 2)
				{
					info.starNum = num3 + 1;
					break;
				}
			}
		}
		else
		{
			if (info.highestScore == 0)
			{
				return;
			}
			for (int num4 = 2; num4 >= 0; num4--)
			{
				if (info.highestScore <= info.starScore[num4])
				{
					info.starNum = num4 + 1;
					break;
				}
			}
		}
	}

	public void CountSumStarNum()
	{
		sumStarNum = 0;
		sumStarNum += CountTaskSumStarNum(drivingTaskInfo);
		sumStarNum += CountTaskSumStarNum(deliverTaskInfo);
		sumStarNum += CountTaskSumStarNum(survivalTaskInfo);
		sumStarNum += CountTaskSumStarNum(gunKillingTaskInfo);
		sumStarNum += CountTaskSumStarNum(carKillingTaskInfo);
		sumStarNum += CountTaskSumStarNum(skillDrivingTaskInfo);
		sumStarNum += CountTaskSumStarNum(robbingCarTaskInfo);
		sumStarNum += CountTaskSumStarNum(fightingTaskInfo);
		sumStarNum += CountTaskSumStarNum(robMotorTaskInfo);
		Controller.instance.sumStarNum = sumStarNum;
	}

	public int CountTaskSumStarNum(TaskInfo[] infoArray)
	{
		int num = 0;
		for (int i = 0; i < infoArray.Length; i++)
		{
			num += infoArray[i].starNum;
		}
		return num;
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	public void ResetTaskLabelUI(GAMEMODE mode, int index)
	{
		NGUITools.SetActiveRecursively(taskLabelUI.gameObject, true);
		for (int i = 0; i < taskLabelUI.starList.Length; i++)
		{
			taskLabelUI.starList[i].gameObject.SetActiveRecursively(false);
		}
		switch (mode)
		{
		case GAMEMODE.DELIVER:
		{
			taskLabelUI.taskModeLabel.text = "Destination";
			for (int m = 0; m < deliverTaskInfo[index].starNum; m++)
			{
				taskLabelUI.starList[m].gameObject.SetActiveRecursively(true);
			}
			break;
		}
		case GAMEMODE.DRIVING0:
		{
			taskLabelUI.taskModeLabel.text = "Route";
			for (int num3 = 0; num3 < drivingTaskInfo[index].starNum; num3++)
			{
				taskLabelUI.starList[num3].gameObject.SetActiveRecursively(true);
			}
			break;
		}
		case GAMEMODE.SLOT:
		{
			taskLabelUI.taskModeLabel.text = "Slot";
			for (int num5 = 0; num5 < taskLabelUI.emptyStarList.Length; num5++)
			{
				taskLabelUI.emptyStarList[num5].gameObject.SetActiveRecursively(false);
			}
			break;
		}
		case GAMEMODE.SURVIVAL:
		{
			taskLabelUI.taskModeLabel.text = "Survive";
			for (int num = 0; num < survivalTaskInfo[index].starNum; num++)
			{
				taskLabelUI.starList[num].gameObject.SetActiveRecursively(true);
			}
			break;
		}
		case GAMEMODE.GUNKILLING:
		{
			taskLabelUI.taskModeLabel.text = "Shooting";
			for (int k = 0; k < gunKillingTaskInfo[index].starNum; k++)
			{
				taskLabelUI.starList[k].gameObject.SetActiveRecursively(true);
			}
			break;
		}
		case GAMEMODE.CARKILLING:
		{
			taskLabelUI.taskModeLabel.text = "Road of Fury";
			for (int num4 = 0; num4 < carKillingTaskInfo[index].starNum; num4++)
			{
				taskLabelUI.starList[num4].gameObject.SetActiveRecursively(true);
			}
			break;
		}
		case GAMEMODE.SKILLDRIVING:
		{
			taskLabelUI.taskModeLabel.text = "Skill";
			for (int num2 = 0; num2 < skillDrivingTaskInfo[index].starNum; num2++)
			{
				taskLabelUI.starList[num2].gameObject.SetActiveRecursively(true);
			}
			break;
		}
		case GAMEMODE.ROBCAR:
		{
			taskLabelUI.taskModeLabel.text = "Car Robbery";
			for (int n = 0; n < robbingCarTaskInfo[index].starNum; n++)
			{
				taskLabelUI.starList[n].gameObject.SetActiveRecursively(true);
			}
			break;
		}
		case GAMEMODE.FIGHTING:
		{
			taskLabelUI.taskModeLabel.text = "Assassinate";
			for (int l = 0; l < fightingTaskInfo[index].starNum; l++)
			{
				taskLabelUI.starList[l].gameObject.SetActiveRecursively(true);
			}
			break;
		}
		case GAMEMODE.ROBMOTOR:
		{
			taskLabelUI.taskModeLabel.text = "Shoot and Ride";
			for (int j = 0; j < robMotorTaskInfo[index].starNum; j++)
			{
				taskLabelUI.starList[j].gameObject.SetActiveRecursively(true);
			}
			break;
		}
		}
		taskLabelUI.transform.GetChild(0).GetComponent<Animation>()["TaskLabelIn"].normalizedTime = 0f;
		taskLabelUI.transform.GetChild(0).GetComponent<Animation>().Sample();
		taskLabelUI.transform.GetChild(0).GetComponent<Animation>().Play("TaskLabelIn");
	}

	private void Update()
	{
		if (GameController.instance.curGameMode == GAMEMODE.NORMAL)
		{
			countTime += Time.deltaTime;
			if (countTime > 5f)
			{
				countTime = 0f;
				CheckEnable();
			}
		}
	}

	public void CheckEnable()
	{
		CheckLabel(drivingTaskInfo);
		CheckLabel(deliverTaskInfo);
		CheckLabel(survivalTaskInfo);
		CheckLabel(slotTaskInfo);
		CheckLabel(gunKillingTaskInfo);
		CheckLabel(carKillingTaskInfo);
		CheckLabel(skillDrivingTaskInfo);
		CheckLabel(robbingCarTaskInfo);
		CheckLabel(fightingTaskInfo);
		CheckLabel(robMotorTaskInfo);
	}

	public void DisableFlag(GAMEMODE newMode)
	{
		for (int i = 0; i < drivingTaskInfo.Length; i++)
		{
			if (drivingTaskInfo[i].enableFlag)
			{
				drivingTaskInfo[i].enableFlag = false;
				if (drivingTaskInfo[i].startLabelObj != null)
				{
					StartLabelPool.instance.RecycleDrivingLabel(drivingTaskInfo[i].startLabelObj);
					drivingTaskInfo[i].startLabelObj = null;
				}
			}
		}
		for (int j = 0; j < deliverTaskInfo.Length; j++)
		{
			if (deliverTaskInfo[j].enableFlag)
			{
				deliverTaskInfo[j].enableFlag = false;
				if (deliverTaskInfo[j].startLabelObj != null)
				{
					StartLabelPool.instance.RecycleDeliverLabel(deliverTaskInfo[j].startLabelObj);
					deliverTaskInfo[j].startLabelObj = null;
				}
			}
		}
		for (int k = 0; k < robbingCarTaskInfo.Length; k++)
		{
			if (robbingCarTaskInfo[k].enableFlag)
			{
				robbingCarTaskInfo[k].enableFlag = false;
				if (robbingCarTaskInfo[k].startLabelObj != null)
				{
					StartLabelPool.instance.RecycleRobCarLabel(robbingCarTaskInfo[k].startLabelObj);
					robbingCarTaskInfo[k].startLabelObj = null;
				}
			}
		}
		for (int l = 0; l < survivalTaskInfo.Length; l++)
		{
			if (survivalTaskInfo[l].enableFlag)
			{
				survivalTaskInfo[l].enableFlag = false;
				if (survivalTaskInfo[l].startLabelObj != null)
				{
					StartLabelPool.instance.RecycleSurvivalLabel(survivalTaskInfo[l].startLabelObj);
					survivalTaskInfo[l].startLabelObj = null;
				}
			}
		}
		for (int m = 0; m < gunKillingTaskInfo.Length; m++)
		{
			if (gunKillingTaskInfo[m].enableFlag)
			{
				gunKillingTaskInfo[m].enableFlag = false;
				if (gunKillingTaskInfo[m].startLabelObj != null)
				{
					StartLabelPool.instance.RecycleGunKillingLabel(gunKillingTaskInfo[m].startLabelObj);
					gunKillingTaskInfo[m].startLabelObj = null;
				}
			}
		}
		for (int n = 0; n < carKillingTaskInfo.Length; n++)
		{
			if (carKillingTaskInfo[n].enableFlag)
			{
				carKillingTaskInfo[n].enableFlag = false;
				if (carKillingTaskInfo[n].startLabelObj != null)
				{
					StartLabelPool.instance.RecycleCarKillingLabel(carKillingTaskInfo[n].startLabelObj);
					carKillingTaskInfo[n].startLabelObj = null;
				}
			}
		}
		for (int num = 0; num < skillDrivingTaskInfo.Length; num++)
		{
			if (skillDrivingTaskInfo[num].enableFlag)
			{
				skillDrivingTaskInfo[num].enableFlag = false;
				if (skillDrivingTaskInfo[num].startLabelObj != null)
				{
					StartLabelPool.instance.RecycleSkillDrivingLabel(skillDrivingTaskInfo[num].startLabelObj);
					skillDrivingTaskInfo[num].startLabelObj = null;
				}
			}
		}
		for (int num2 = 0; num2 < fightingTaskInfo.Length; num2++)
		{
			if (fightingTaskInfo[num2].enableFlag)
			{
				fightingTaskInfo[num2].enableFlag = false;
				if (fightingTaskInfo[num2].startLabelObj != null)
				{
					StartLabelPool.instance.RecycleFightingLabel(fightingTaskInfo[num2].startLabelObj);
					fightingTaskInfo[num2].startLabelObj = null;
				}
			}
		}
		for (int num3 = 0; num3 < robMotorTaskInfo.Length; num3++)
		{
			if (robMotorTaskInfo[num3].enableFlag)
			{
				robMotorTaskInfo[num3].enableFlag = false;
				if (robMotorTaskInfo[num3].startLabelObj != null)
				{
					StartLabelPool.instance.RecycleRobMotorLabel(robMotorTaskInfo[num3].startLabelObj);
					robMotorTaskInfo[num3].startLabelObj = null;
				}
			}
		}
		if (newMode != GAMEMODE.SLOT)
		{
			for (int num4 = 0; num4 < slotTaskInfo.Length; num4++)
			{
				if (slotTaskInfo[num4].enableFlag)
				{
					slotTaskInfo[num4].enableFlag = false;
					if (slotTaskInfo[num4].startLabelObj != null)
					{
						slotTaskInfo[num4].startLabelObj.slotObj.slotActiveObj = null;
						StartLabelPool.instance.RecycleSlot(slotTaskInfo[num4].startLabelObj.slotObj);
						slotTaskInfo[num4].startLabelObj.slotObj = null;
						StartLabelPool.instance.RecycleSlotLabel(slotTaskInfo[num4].startLabelObj);
						slotTaskInfo[num4].startLabelObj = null;
					}
				}
			}
			return;
		}
		for (int num5 = 0; num5 < slotTaskInfo.Length; num5++)
		{
			if (slotTaskInfo[num5].enableFlag)
			{
				slotTaskInfo[num5].enableFlag = false;
				if (slotTaskInfo[num5].startLabelObj != null)
				{
					StartLabelPool.instance.RecycleSlotLabel(slotTaskInfo[num5].startLabelObj);
					slotTaskInfo[num5].startLabelObj = null;
				}
			}
		}
	}

	public void CheckLabel(TaskInfo[] info)
	{
		for (int i = 0; i < info.Length; i++)
		{
			if (info[i].taskLevel <= GlobalInf.gameLevel)
			{
				if (!GlobalInf.newUserFlag)
				{
					if (info[i].stateIndex < GlobalInf.gameState)
					{
						if (Vector3.SqrMagnitude(PlayerController.instance.transform.position - info[i].startPos) < 40000f)
						{
							if (!info[i].enableFlag)
							{
								info[i].enableFlag = true;
								switch (info[i].taskMode)
								{
								case GAMEMODE.DRIVING0:
									info[i].startLabelObj = StartLabelPool.instance.GetDrivingLabel(info[i]);
									break;
								case GAMEMODE.DELIVER:
									info[i].startLabelObj = StartLabelPool.instance.GetDeliverLabel(info[i]);
									break;
								case GAMEMODE.SURVIVAL:
									info[i].startLabelObj = StartLabelPool.instance.GetSurvivalLabel(info[i]);
									break;
								case GAMEMODE.SLOT:
									info[i].startLabelObj = StartLabelPool.instance.GetSlotLabel(info[i]);
									info[i].startLabelObj.slotObj = StartLabelPool.instance.GetSlot(info[i].slotPos, info[i].slotAngle);
									info[i].startLabelObj.slotObj.slotActiveObj = info[i].startLabelObj.gameObject;
									break;
								case GAMEMODE.GUNKILLING:
									info[i].startLabelObj = StartLabelPool.instance.GetGunKillingLabel(info[i]);
									break;
								case GAMEMODE.CARKILLING:
									info[i].startLabelObj = StartLabelPool.instance.GetCarKillingLabel(info[i]);
									break;
								case GAMEMODE.SKILLDRIVING:
									info[i].startLabelObj = StartLabelPool.instance.GetSkillDrivingLabel(info[i]);
									break;
								case GAMEMODE.ROBCAR:
									info[i].startLabelObj = StartLabelPool.instance.GetRobCarLabel(info[i]);
									break;
								case GAMEMODE.FIGHTING:
									info[i].startLabelObj = StartLabelPool.instance.GetFightingLabel(info[i]);
									break;
								case GAMEMODE.ROBMOTOR:
									info[i].startLabelObj = StartLabelPool.instance.GetRobMotorLabel(info[i]);
									break;
								}
							}
						}
						else
						{
							if (!info[i].enableFlag)
							{
								continue;
							}
							info[i].enableFlag = false;
							if (info[i].startLabelObj != null)
							{
								switch (info[i].taskMode)
								{
								case GAMEMODE.DRIVING0:
									StartLabelPool.instance.RecycleDrivingLabel(info[i].startLabelObj);
									break;
								case GAMEMODE.DELIVER:
									StartLabelPool.instance.RecycleDeliverLabel(info[i].startLabelObj);
									break;
								case GAMEMODE.SURVIVAL:
									StartLabelPool.instance.RecycleSurvivalLabel(info[i].startLabelObj);
									break;
								case GAMEMODE.SLOT:
									info[i].startLabelObj.slotObj.slotActiveObj = null;
									StartLabelPool.instance.RecycleSlot(info[i].startLabelObj.slotObj);
									info[i].startLabelObj.slotObj = null;
									StartLabelPool.instance.RecycleSlotLabel(info[i].startLabelObj);
									break;
								case GAMEMODE.GUNKILLING:
									StartLabelPool.instance.RecycleGunKillingLabel(info[i].startLabelObj);
									break;
								case GAMEMODE.CARKILLING:
									StartLabelPool.instance.RecycleCarKillingLabel(info[i].startLabelObj);
									break;
								case GAMEMODE.SKILLDRIVING:
									StartLabelPool.instance.RecycleSkillDrivingLabel(info[i].startLabelObj);
									break;
								case GAMEMODE.ROBCAR:
									StartLabelPool.instance.RecycleRobCarLabel(info[i].startLabelObj);
									break;
								case GAMEMODE.FIGHTING:
									StartLabelPool.instance.RecycleFightingLabel(info[i].startLabelObj);
									break;
								case GAMEMODE.ROBMOTOR:
									StartLabelPool.instance.RecycleRobMotorLabel(info[i].startLabelObj);
									break;
								}
								info[i].startLabelObj = null;
							}
						}
					}
					else
					{
						if (!info[i].enableFlag)
						{
							continue;
						}
						info[i].enableFlag = false;
						if (info[i].startLabelObj != null)
						{
							switch (info[i].taskMode)
							{
							case GAMEMODE.DRIVING0:
								StartLabelPool.instance.RecycleDrivingLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.DELIVER:
								StartLabelPool.instance.RecycleDeliverLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.SURVIVAL:
								StartLabelPool.instance.RecycleSurvivalLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.SLOT:
								info[i].startLabelObj.slotObj.slotActiveObj = null;
								StartLabelPool.instance.RecycleSlot(info[i].startLabelObj.slotObj);
								info[i].startLabelObj.slotObj = null;
								StartLabelPool.instance.RecycleSlotLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.GUNKILLING:
								StartLabelPool.instance.RecycleGunKillingLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.CARKILLING:
								StartLabelPool.instance.RecycleCarKillingLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.SKILLDRIVING:
								StartLabelPool.instance.RecycleSkillDrivingLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.ROBCAR:
								StartLabelPool.instance.RecycleRobCarLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.FIGHTING:
								StartLabelPool.instance.RecycleFightingLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.ROBMOTOR:
								StartLabelPool.instance.RecycleRobMotorLabel(info[i].startLabelObj);
								break;
							}
							info[i].startLabelObj = null;
						}
					}
				}
				else if (info[i].newStateIndex < GlobalInf.gameState)
				{
					if (Vector3.SqrMagnitude(PlayerController.instance.transform.position - info[i].startPos) < 40000f)
					{
						if (!info[i].enableFlag)
						{
							info[i].enableFlag = true;
							switch (info[i].taskMode)
							{
							case GAMEMODE.DRIVING0:
								info[i].startLabelObj = StartLabelPool.instance.GetDrivingLabel(info[i]);
								break;
							case GAMEMODE.DELIVER:
								info[i].startLabelObj = StartLabelPool.instance.GetDeliverLabel(info[i]);
								break;
							case GAMEMODE.SURVIVAL:
								info[i].startLabelObj = StartLabelPool.instance.GetSurvivalLabel(info[i]);
								break;
							case GAMEMODE.SLOT:
								info[i].startLabelObj = StartLabelPool.instance.GetSlotLabel(info[i]);
								info[i].startLabelObj.slotObj = StartLabelPool.instance.GetSlot(info[i].slotPos, info[i].slotAngle);
								info[i].startLabelObj.slotObj.slotActiveObj = info[i].startLabelObj.gameObject;
								break;
							case GAMEMODE.GUNKILLING:
								info[i].startLabelObj = StartLabelPool.instance.GetGunKillingLabel(info[i]);
								break;
							case GAMEMODE.CARKILLING:
								info[i].startLabelObj = StartLabelPool.instance.GetCarKillingLabel(info[i]);
								break;
							case GAMEMODE.SKILLDRIVING:
								info[i].startLabelObj = StartLabelPool.instance.GetSkillDrivingLabel(info[i]);
								break;
							case GAMEMODE.ROBCAR:
								info[i].startLabelObj = StartLabelPool.instance.GetRobCarLabel(info[i]);
								break;
							case GAMEMODE.FIGHTING:
								info[i].startLabelObj = StartLabelPool.instance.GetFightingLabel(info[i]);
								break;
							case GAMEMODE.ROBMOTOR:
								info[i].startLabelObj = StartLabelPool.instance.GetRobMotorLabel(info[i]);
								break;
							}
						}
					}
					else
					{
						if (!info[i].enableFlag)
						{
							continue;
						}
						info[i].enableFlag = false;
						if (info[i].startLabelObj != null)
						{
							switch (info[i].taskMode)
							{
							case GAMEMODE.DRIVING0:
								StartLabelPool.instance.RecycleDrivingLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.DELIVER:
								StartLabelPool.instance.RecycleDeliverLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.SURVIVAL:
								StartLabelPool.instance.RecycleSurvivalLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.SLOT:
								info[i].startLabelObj.slotObj.slotActiveObj = null;
								StartLabelPool.instance.RecycleSlot(info[i].startLabelObj.slotObj);
								info[i].startLabelObj.slotObj = null;
								StartLabelPool.instance.RecycleSlotLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.GUNKILLING:
								StartLabelPool.instance.RecycleGunKillingLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.CARKILLING:
								StartLabelPool.instance.RecycleCarKillingLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.SKILLDRIVING:
								StartLabelPool.instance.RecycleSkillDrivingLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.ROBCAR:
								StartLabelPool.instance.RecycleRobCarLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.FIGHTING:
								StartLabelPool.instance.RecycleFightingLabel(info[i].startLabelObj);
								break;
							case GAMEMODE.ROBMOTOR:
								StartLabelPool.instance.RecycleRobMotorLabel(info[i].startLabelObj);
								break;
							}
							info[i].startLabelObj = null;
						}
					}
				}
				else
				{
					if (!info[i].enableFlag)
					{
						continue;
					}
					info[i].enableFlag = false;
					if (info[i].startLabelObj != null)
					{
						switch (info[i].taskMode)
						{
						case GAMEMODE.DRIVING0:
							StartLabelPool.instance.RecycleDrivingLabel(info[i].startLabelObj);
							break;
						case GAMEMODE.DELIVER:
							StartLabelPool.instance.RecycleDeliverLabel(info[i].startLabelObj);
							break;
						case GAMEMODE.SURVIVAL:
							StartLabelPool.instance.RecycleSurvivalLabel(info[i].startLabelObj);
							break;
						case GAMEMODE.SLOT:
							info[i].startLabelObj.slotObj.slotActiveObj = null;
							StartLabelPool.instance.RecycleSlot(info[i].startLabelObj.slotObj);
							info[i].startLabelObj.slotObj = null;
							StartLabelPool.instance.RecycleSlotLabel(info[i].startLabelObj);
							break;
						case GAMEMODE.GUNKILLING:
							StartLabelPool.instance.RecycleGunKillingLabel(info[i].startLabelObj);
							break;
						case GAMEMODE.CARKILLING:
							StartLabelPool.instance.RecycleCarKillingLabel(info[i].startLabelObj);
							break;
						case GAMEMODE.SKILLDRIVING:
							StartLabelPool.instance.RecycleSkillDrivingLabel(info[i].startLabelObj);
							break;
						case GAMEMODE.ROBCAR:
							StartLabelPool.instance.RecycleRobCarLabel(info[i].startLabelObj);
							break;
						case GAMEMODE.FIGHTING:
							StartLabelPool.instance.RecycleFightingLabel(info[i].startLabelObj);
							break;
						case GAMEMODE.ROBMOTOR:
							StartLabelPool.instance.RecycleRobMotorLabel(info[i].startLabelObj);
							break;
						}
						info[i].startLabelObj = null;
					}
				}
			}
			else
			{
				if (!info[i].enableFlag)
				{
					continue;
				}
				info[i].enableFlag = false;
				if (info[i].startLabelObj != null)
				{
					switch (info[i].taskMode)
					{
					case GAMEMODE.DRIVING0:
						StartLabelPool.instance.RecycleDrivingLabel(info[i].startLabelObj);
						break;
					case GAMEMODE.DELIVER:
						StartLabelPool.instance.RecycleDeliverLabel(info[i].startLabelObj);
						break;
					case GAMEMODE.SURVIVAL:
						StartLabelPool.instance.RecycleSurvivalLabel(info[i].startLabelObj);
						break;
					case GAMEMODE.SLOT:
						info[i].startLabelObj.slotObj.slotActiveObj = null;
						StartLabelPool.instance.RecycleSlot(info[i].startLabelObj.slotObj);
						info[i].startLabelObj.slotObj = null;
						StartLabelPool.instance.RecycleSlotLabel(info[i].startLabelObj);
						break;
					case GAMEMODE.GUNKILLING:
						StartLabelPool.instance.RecycleGunKillingLabel(info[i].startLabelObj);
						break;
					case GAMEMODE.CARKILLING:
						StartLabelPool.instance.RecycleCarKillingLabel(info[i].startLabelObj);
						break;
					case GAMEMODE.SKILLDRIVING:
						StartLabelPool.instance.RecycleSkillDrivingLabel(info[i].startLabelObj);
						break;
					case GAMEMODE.ROBCAR:
						StartLabelPool.instance.RecycleRobCarLabel(info[i].startLabelObj);
						break;
					case GAMEMODE.FIGHTING:
						StartLabelPool.instance.RecycleFightingLabel(info[i].startLabelObj);
						break;
					case GAMEMODE.ROBMOTOR:
						StartLabelPool.instance.RecycleRobMotorLabel(info[i].startLabelObj);
						break;
					}
					info[i].startLabelObj = null;
				}
			}
		}
	}

	public int GetRewardIndex(GAMEMODE mode, int index)
	{
		switch (mode)
		{
		case GAMEMODE.CARKILLING:
			return carKillingTaskInfo[index].rewardIndex;
		case GAMEMODE.DELIVER:
			return deliverTaskInfo[index].rewardIndex;
		case GAMEMODE.DRIVING0:
			return drivingTaskInfo[index].rewardIndex;
		case GAMEMODE.GUNKILLING:
			return gunKillingTaskInfo[index].rewardIndex;
		case GAMEMODE.SURVIVAL:
			return survivalTaskInfo[index].rewardIndex;
		case GAMEMODE.SKILLDRIVING:
			return skillDrivingTaskInfo[index].rewardIndex;
		case GAMEMODE.ROBCAR:
			return robbingCarTaskInfo[index].rewardIndex;
		case GAMEMODE.FIGHTING:
			return fightingTaskInfo[index].rewardIndex;
		case GAMEMODE.ROBMOTOR:
			return robMotorTaskInfo[index].rewardIndex;
		default:
			return 0;
		}
	}

	public TaskInfo GetTaskInfo(GAMEMODE mode, int index)
	{
		switch (mode)
		{
		case GAMEMODE.CARKILLING:
			return carKillingTaskInfo[index];
		case GAMEMODE.DELIVER:
			return deliverTaskInfo[index];
		case GAMEMODE.DRIVING0:
			return drivingTaskInfo[index];
		case GAMEMODE.GUNKILLING:
			return gunKillingTaskInfo[index];
		case GAMEMODE.SURVIVAL:
			return survivalTaskInfo[index];
		case GAMEMODE.SLOT:
			return slotTaskInfo[index];
		case GAMEMODE.SKILLDRIVING:
			return skillDrivingTaskInfo[index];
		case GAMEMODE.ROBCAR:
			return robbingCarTaskInfo[index];
		case GAMEMODE.FIGHTING:
			return fightingTaskInfo[index];
		case GAMEMODE.ROBMOTOR:
			return robMotorTaskInfo[index];
		default:
			return null;
		}
	}

	public int GetStarNum(GAMEMODE mode, int index)
	{
		switch (mode)
		{
		case GAMEMODE.CARKILLING:
			return carKillingTaskInfo[index].starNum;
		case GAMEMODE.DELIVER:
			return deliverTaskInfo[index].starNum;
		case GAMEMODE.DRIVING0:
			return drivingTaskInfo[index].starNum;
		case GAMEMODE.GUNKILLING:
			return gunKillingTaskInfo[index].starNum;
		case GAMEMODE.SURVIVAL:
			return survivalTaskInfo[index].starNum;
		case GAMEMODE.SKILLDRIVING:
			return skillDrivingTaskInfo[index].starNum;
		case GAMEMODE.ROBCAR:
			return robbingCarTaskInfo[index].starNum;
		case GAMEMODE.FIGHTING:
			return fightingTaskInfo[index].starNum;
		case GAMEMODE.ROBMOTOR:
			return robMotorTaskInfo[index].starNum;
		default:
			return 0;
		}
	}
}
