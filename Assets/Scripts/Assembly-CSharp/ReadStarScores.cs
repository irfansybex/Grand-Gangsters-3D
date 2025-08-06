using UnityEngine;

[ExecuteInEditMode]
public class ReadStarScores : ReadTXT
{
	public bool runflag;

	public string fileName;

	public TaskLabelController taskLabelController;

	public int rewardIndex;

	public DrivingModeController drivingmode;

	public DeliverModeController deliverMode;

	public GunKillingModeControllor gunKillMode;

	public CarKillingModeControllor carKillMode;

	public SkillDrivingMode skillMode;

	public RobCarModeController robCarMode;

	public FightingModeController fingtingMode;

	private void Update()
	{
		if (runflag)
		{
			runflag = false;
			Run();
			MonoBehaviour.print("run");
		}
	}

	public void Run()
	{
		InitArray(fileName);
		rewardIndex = 0;
		ReadSurvival();
		ReadDriving();
		ReadDeliver();
		ReadGunKill();
		ReadCarKill();
		ReadSkillDriving();
		ReadFightingData();
		ReadRobCarData();
	}

	public void ReadSurvival()
	{
		for (int i = 1; i < 5; i++)
		{
			for (int j = 3; j < 6; j++)
			{
				taskLabelController.survivalTaskInfo[(i - 1) * 2].starScore[j - 3] = GetInt(i, j);
				taskLabelController.survivalTaskInfo[(i - 1) * 2].rewardIndex = rewardIndex;
				taskLabelController.survivalTaskInfo[(i - 1) * 2 + 1].starScore[j - 3] = GetInt(i, j);
				taskLabelController.survivalTaskInfo[(i - 1) * 2 + 1].rewardIndex = rewardIndex;
			}
			rewardIndex++;
		}
	}

	public void ReadDriving()
	{
		for (int i = 5; i < 9; i++)
		{
			for (int j = 3; j < 6; j++)
			{
				taskLabelController.drivingTaskInfo[i - 5].starScore[j - 3] = GetInt(i, j);
				taskLabelController.drivingTaskInfo[i - 5].rewardIndex = rewardIndex;
			}
			drivingmode.startSetTime[i - 5] = GetInt(i, 2);
			rewardIndex++;
		}
	}

	public void ReadDeliver()
	{
		for (int i = 9; i < 13; i++)
		{
			for (int j = 3; j < 6; j++)
			{
				taskLabelController.deliverTaskInfo[i - 9].starScore[j - 3] = GetInt(i, j);
				taskLabelController.deliverTaskInfo[i - 9].rewardIndex = rewardIndex;
			}
			deliverMode.setTime[i - 9] = GetInt(i, 2);
			rewardIndex++;
		}
	}

	public void ReadGunKill()
	{
		for (int i = 13; i < 17; i++)
		{
			for (int j = 3; j < 6; j++)
			{
				taskLabelController.gunKillingTaskInfo[i - 13].starScore[j - 3] = GetInt(i, j);
				taskLabelController.gunKillingTaskInfo[i - 13].rewardIndex = rewardIndex;
			}
			gunKillMode.gunKillingMissionTime[i - 13] = GetInt(i, 2);
			rewardIndex++;
		}
	}

	public void ReadCarKill()
	{
		for (int i = 17; i < 21; i++)
		{
			for (int j = 3; j < 6; j++)
			{
				taskLabelController.carKillingTaskInfo[i - 17].starScore[j - 3] = GetInt(i, j);
				taskLabelController.carKillingTaskInfo[i - 17].rewardIndex = rewardIndex;
			}
			carKillMode.carKillingMissionTime[i - 17] = GetInt(i, 2);
			rewardIndex++;
		}
	}

	public void ReadSkillDriving()
	{
		for (int i = 21; i < 25; i++)
		{
			for (int j = 3; j < 6; j++)
			{
				taskLabelController.skillDrivingTaskInfo[i - 21].starScore[j - 3] = GetInt(i, j);
				taskLabelController.skillDrivingTaskInfo[i - 21].rewardIndex = rewardIndex;
			}
			skillMode.skillDrivingMissionTime[i - 21] = GetInt(i, 2);
			rewardIndex++;
		}
	}

	public void ReadFightingData()
	{
		for (int i = 25; i < 29; i++)
		{
			for (int j = 3; j < 6; j++)
			{
				taskLabelController.fightingTaskInfo[i - 25].starScore[j - 3] = GetInt(i, j);
				taskLabelController.fightingTaskInfo[i - 25].rewardIndex = rewardIndex;
			}
			fingtingMode.levelTime[i - 25] = GetInt(i, 2);
			rewardIndex++;
		}
	}

	public void ReadRobCarData()
	{
		for (int i = 29; i < 33; i++)
		{
			for (int j = 3; j < 6; j++)
			{
				taskLabelController.robbingCarTaskInfo[i - 29].starScore[j - 3] = GetInt(i, j);
				taskLabelController.robbingCarTaskInfo[i - 29].rewardIndex = rewardIndex;
			}
			robCarMode.setTime[i - 29] = GetInt(i, 2);
			rewardIndex++;
		}
	}
}
