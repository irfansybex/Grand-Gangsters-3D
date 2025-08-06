using UnityEngine;

public class TaskLabelUIController : MonoBehaviour
{
	public UILabel modeLabel;

	public UILabel describeLabel;

	public GameObject backGround;

	public GameObject rootObj;

	public GameObject[] star;

	public GameObject[] emptyStar;

	public UIEventListener okBtn;

	public UIEventListener cancelBtn;

	public UIEventListener buyHandGunBulletBtn;

	public UIEventListener buyMachineGunBulletBtn;

	public int taskIndex;

	public int starNum;

	public UISprite[] line1Pic;

	public UISprite[] line2Pic;

	public UISprite[] line3Pic;

	public UILabel[] lineLabel;

	public int toolKitPrise;

	public int healthKitPrise;

	public int handGunBulletPrise;

	public UILabel handGunBulletPriseLabel;

	public int machineGunBulletPrise;

	public UILabel machineGunBulletPriseLabel;

	public UILabel handGunBulletNumLabel;

	public UILabel machineGunBulletNumLabel;

	public void ResetTaskCheckUI(GAMEMODE gameMode, int taskIndex)
	{
		switch (gameMode)
		{
		case GAMEMODE.DELIVER:
			modeLabel.text = "Reach the Destination";
			describeLabel.text = "By any route, please\nReach the destination as soon as possible!";
			starNum = TaskLabelController.instance.deliverTaskInfo[taskIndex].starNum;
			CheckTaskRewardInfo(TaskLabelController.instance.deliverTaskInfo[taskIndex].rewardIndex);
			break;
		case GAMEMODE.DRIVING0:
			modeLabel.text = "Keep in Route";
			describeLabel.text = "Drive along the designated route\nDon't miss any checkpoint !";
			starNum = TaskLabelController.instance.drivingTaskInfo[taskIndex].starNum;
			CheckTaskRewardInfo(TaskLabelController.instance.drivingTaskInfo[taskIndex].rewardIndex);
			break;
		case GAMEMODE.SURVIVAL:
			modeLabel.text = "Survive in Desperate";
			describeLabel.text = "You have been surrounded by enemies\nSurvive and kill them all";
			starNum = TaskLabelController.instance.survivalTaskInfo[taskIndex].starNum;
			CheckTaskRewardInfo(TaskLabelController.instance.survivalTaskInfo[taskIndex].rewardIndex);
			break;
		case GAMEMODE.GUNKILLING:
			modeLabel.text = "Shooting Time";
			describeLabel.text = "Shooting time!  Raise your gun!\nKill as many citizens as possible!";
			starNum = TaskLabelController.instance.gunKillingTaskInfo[taskIndex].starNum;
			CheckTaskRewardInfo(TaskLabelController.instance.gunKillingTaskInfo[taskIndex].rewardIndex);
			break;
		case GAMEMODE.CARKILLING:
			modeLabel.text = "Road of Fury";
			describeLabel.text = "Sometimes a car is the best weapon\nKnock down as many citizens as possible!";
			starNum = TaskLabelController.instance.carKillingTaskInfo[taskIndex].starNum;
			CheckTaskRewardInfo(TaskLabelController.instance.carKillingTaskInfo[taskIndex].rewardIndex);
			break;
		case GAMEMODE.SKILLDRIVING:
			modeLabel.text = "Driving Skill Test";
			describeLabel.text = "Time is money, but first of all:\nKeep driving your car on the track !";
			starNum = TaskLabelController.instance.skillDrivingTaskInfo[taskIndex].starNum;
			CheckTaskRewardInfo(TaskLabelController.instance.skillDrivingTaskInfo[taskIndex].rewardIndex);
			break;
		case GAMEMODE.ROBCAR:
			modeLabel.text = "Car Robbery";
			describeLabel.text = "Rob the vehicle that shown in the picture\nThen drive it to the  destination!";
			starNum = TaskLabelController.instance.robbingCarTaskInfo[taskIndex].starNum;
			CheckTaskRewardInfo(TaskLabelController.instance.robbingCarTaskInfo[taskIndex].rewardIndex);
			break;
		case GAMEMODE.FIGHTING:
			modeLabel.text = "Assassinate";
			describeLabel.text = "The process is not important\nJust kill the target NPC!";
			starNum = TaskLabelController.instance.fightingTaskInfo[taskIndex].starNum;
			CheckTaskRewardInfo(TaskLabelController.instance.fightingTaskInfo[taskIndex].rewardIndex);
			break;
		case GAMEMODE.ROBMOTOR:
			modeLabel.text = "Motorcycle Robbery";
			describeLabel.text = "Rob the motorcycle from enemies!\nThen ride it to the destination!";
			starNum = TaskLabelController.instance.robMotorTaskInfo[taskIndex].starNum;
			CheckTaskRewardInfo(TaskLabelController.instance.robMotorTaskInfo[taskIndex].rewardIndex);
			break;
		}
		for (int i = 0; i < 3; i++)
		{
			if (i < starNum)
			{
				star[i].SetActiveRecursively(true);
			}
			else
			{
				star[i].SetActiveRecursively(false);
			}
		}
		if (PlayerController.instance.gun != null)
		{
			handGunBulletPrise = GlobalInf.handGunInfo.bulletPrise * (PlayerController.instance.gun.gunInfo.maxBulletNum - PlayerController.instance.gun.gunInfo.restBulletNum - (PlayerController.instance.gun.gunInfo.curBulletNum - PlayerController.instance.gun.bulletCount));
			handGunBulletNumLabel.text = string.Empty + (PlayerController.instance.gun.gunInfo.restBulletNum + (PlayerController.instance.gun.gunInfo.curBulletNum - PlayerController.instance.gun.bulletCount)) + "/" + PlayerController.instance.gun.gunInfo.maxBulletNum;
		}
		if (PlayerController.instance.machineGun != null)
		{
			machineGunBulletPrise = GlobalInf.machineGunInfo.bulletPrise * (PlayerController.instance.machineGun.gunInfo.maxBulletNum - PlayerController.instance.machineGun.gunInfo.restBulletNum - (PlayerController.instance.machineGun.gunInfo.curBulletNum - PlayerController.instance.machineGun.bulletCount));
			machineGunBulletNumLabel.text = string.Empty + (PlayerController.instance.machineGun.gunInfo.restBulletNum + (PlayerController.instance.machineGun.gunInfo.curBulletNum - PlayerController.instance.machineGun.bulletCount)) + "/" + PlayerController.instance.machineGun.gunInfo.maxBulletNum;
		}
		else
		{
			machineGunBulletNumLabel.text = "0/0";
		}
		handGunBulletPriseLabel.text = string.Empty + handGunBulletPrise;
		machineGunBulletPriseLabel.text = string.Empty + machineGunBulletPrise;
		GameUIController.instance.topLine.checkPageFlag = true;
	}

	public void CheckTaskRewardInfo(int rewardIndex)
	{
		CheckRewardInfoInLine(line1Pic, rewardIndex, 0);
		if (TaskRewardInfoList.instance.taskRewardList[rewardIndex].itemList.Length > 1)
		{
			CheckRewardInfoInLine(line2Pic, rewardIndex, 1);
		}
		else
		{
			for (int i = 0; i < line2Pic.Length; i++)
			{
				line2Pic[i].gameObject.SetActiveRecursively(false);
			}
			lineLabel[1].gameObject.SetActiveRecursively(false);
		}
		if (TaskRewardInfoList.instance.taskRewardList[rewardIndex].itemList.Length > 2)
		{
			CheckRewardInfoInLine(line3Pic, rewardIndex, 2);
			return;
		}
		for (int j = 0; j < line3Pic.Length; j++)
		{
			line3Pic[j].gameObject.SetActiveRecursively(false);
		}
		lineLabel[2].gameObject.SetActiveRecursively(false);
	}

	public void CheckRewardInfoInLine(UISprite[] line, int rewardIndex, int lineIndex)
	{
		for (int i = 0; i < line.Length; i++)
		{
			line[i].gameObject.SetActiveRecursively(false);
		}
		switch (TaskRewardInfoList.instance.taskRewardList[rewardIndex].itemList[lineIndex].itemType)
		{
		case ITEMTYPE.CAR:
			line[0].gameObject.SetActiveRecursively(true);
			lineLabel[lineIndex].text = TaskRewardInfoList.instance.carName[TaskRewardInfoList.instance.taskRewardList[rewardIndex].itemList[lineIndex].index];
			break;
		case ITEMTYPE.HANDGUN:
			line[1].gameObject.SetActiveRecursively(true);
			lineLabel[lineIndex].text = TaskRewardInfoList.instance.handGunName[TaskRewardInfoList.instance.taskRewardList[rewardIndex].itemList[lineIndex].index];
			break;
		case ITEMTYPE.MACHINEGUN:
			line[2].gameObject.SetActiveRecursively(true);
			lineLabel[lineIndex].text = TaskRewardInfoList.instance.machineGunName[TaskRewardInfoList.instance.taskRewardList[rewardIndex].itemList[lineIndex].index];
			break;
		}
	}
}
