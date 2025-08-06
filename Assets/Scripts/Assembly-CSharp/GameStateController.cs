using UnityEngine;

public class GameStateController : MonoBehaviour
{
	public static GameStateController instance;

	public MiniMapController minimapController;

	public static int MAXSTATENUM = 8;

	public static int NEWMAXSTATENUM = 11;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	public void CheckGameState()
	{
		if (!GlobalInf.newUserFlag)
		{
			if (GameController.instance.curGameMode == GAMEMODE.NORMAL && GameSenceTutorialController.instance == null)
			{
				if (GlobalInf.gameState == 1)
				{
					minimapController.mapDrawPath.ClearLine();
					minimapController.FindWay(TaskLabelController.instance.gunKillingTaskInfo[0].startPos);
					if (!minimapController.mapDrawPath.targetFlag)
					{
						minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.gunKillingTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
					}
					GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
					GameUIController.instance.stateUIController.label.text = "Follow The Map To The First Mission";
					GameUIController.instance.stateUIController.backPic.width = (int)(GlobalDefine.screenWidthFit * 390f);
				}
				else if (GlobalInf.gameState == 2)
				{
					minimapController.mapDrawPath.ClearLine();
					minimapController.FindWay(TaskLabelController.instance.drivingTaskInfo[0].startPos);
					if (!minimapController.mapDrawPath.targetFlag)
					{
						minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.drivingTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
					}
					GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
					GameUIController.instance.stateUIController.label.text = "Follow The Map To The Next Mission";
				}
				else if (GlobalInf.gameState == 3)
				{
					minimapController.mapDrawPath.ClearLine();
					minimapController.FindWay(TaskLabelController.instance.carKillingTaskInfo[0].startPos);
					if (!minimapController.mapDrawPath.targetFlag)
					{
						minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.carKillingTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
					}
					GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
				}
				else if (GlobalInf.gameState == 4)
				{
					minimapController.mapDrawPath.ClearLine();
					minimapController.FindWay(TaskLabelController.instance.survivalTaskInfo[0].startPos);
					if (!minimapController.mapDrawPath.targetFlag)
					{
						minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.survivalTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
					}
					GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
				}
				else if (GlobalInf.gameState == 5)
				{
					minimapController.mapDrawPath.ClearLine();
					minimapController.FindWay(TaskLabelController.instance.deliverTaskInfo[0].startPos);
					if (!minimapController.mapDrawPath.targetFlag)
					{
						minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.deliverTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
					}
					GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
				}
				else if (GlobalInf.gameState == 6)
				{
					minimapController.mapDrawPath.ClearLine();
					minimapController.FindWay(TaskLabelController.instance.skillDrivingTaskInfo[0].startPos);
					if (!minimapController.mapDrawPath.targetFlag)
					{
						minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.skillDrivingTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
					}
					GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
				}
				else
				{
					GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(false);
				}
			}
			else
			{
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(false);
			}
		}
		else if (GameController.instance.curGameMode == GAMEMODE.NORMAL && GameSenceTutorialController.instance == null)
		{
			if (GlobalInf.gameState == 1)
			{
				minimapController.mapDrawPath.ClearLine();
				minimapController.FindWay(TaskLabelController.instance.gunKillingTaskInfo[0].startPos);
				if (!minimapController.mapDrawPath.targetFlag)
				{
					minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.gunKillingTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
				}
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
				GameUIController.instance.stateUIController.label.text = "Follow The Map To The First Mission";
				GameUIController.instance.stateUIController.backPic.width = (int)(GlobalDefine.screenWidthFit * 390f);
			}
			else if (GlobalInf.gameState == 2)
			{
				minimapController.mapDrawPath.ClearLine();
				minimapController.FindWay(TaskLabelController.instance.drivingTaskInfo[0].startPos);
				if (!minimapController.mapDrawPath.targetFlag)
				{
					minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.drivingTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
				}
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
				GameUIController.instance.stateUIController.label.text = "Follow The Map To The Next Mission";
			}
			else if (GlobalInf.gameState == 3)
			{
				minimapController.mapDrawPath.ClearLine();
				minimapController.FindWay(TaskLabelController.instance.robbingCarTaskInfo[0].startPos);
				if (!minimapController.mapDrawPath.targetFlag)
				{
					minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.robbingCarTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
				}
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
			}
			else if (GlobalInf.gameState == 4)
			{
				minimapController.mapDrawPath.ClearLine();
				minimapController.FindWay(TaskLabelController.instance.fightingTaskInfo[0].startPos);
				if (!minimapController.mapDrawPath.targetFlag)
				{
					minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.fightingTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
				}
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
			}
			else if (GlobalInf.gameState == 5)
			{
				minimapController.mapDrawPath.ClearLine();
				minimapController.FindWay(TaskLabelController.instance.carKillingTaskInfo[0].startPos);
				if (!minimapController.mapDrawPath.targetFlag)
				{
					minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.carKillingTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
				}
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
			}
			else if (GlobalInf.gameState == 6)
			{
				minimapController.mapDrawPath.ClearLine();
				minimapController.FindWay(TaskLabelController.instance.deliverTaskInfo[0].startPos);
				if (!minimapController.mapDrawPath.targetFlag)
				{
					minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.deliverTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
				}
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
			}
			else if (GlobalInf.gameState == 7)
			{
				minimapController.mapDrawPath.ClearLine();
				minimapController.FindWay(TaskLabelController.instance.survivalTaskInfo[0].startPos);
				if (!minimapController.mapDrawPath.targetFlag)
				{
					minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.survivalTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
				}
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
			}
			else if (GlobalInf.gameState == 8)
			{
				minimapController.mapDrawPath.ClearLine();
				minimapController.FindWay(TaskLabelController.instance.skillDrivingTaskInfo[0].startPos);
				if (!minimapController.mapDrawPath.targetFlag)
				{
					minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.skillDrivingTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
				}
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
			}
			else if (GlobalInf.gameState == 9)
			{
				minimapController.mapDrawPath.ClearLine();
				minimapController.FindWay(TaskLabelController.instance.robMotorTaskInfo[0].startPos);
				if (!minimapController.mapDrawPath.targetFlag)
				{
					minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(TaskLabelController.instance.robMotorTaskInfo[0].startPos) + new Vector3(1000f, 480f, 0f));
				}
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(true);
			}
			else
			{
				GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(false);
			}
		}
		else
		{
			GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(false);
		}
	}

	public void ChangeGameState()
	{
		GlobalInf.gameState++;
		StoreDateController.SetGameState();
		CheckGameState();
		if (GlobalInf.gameState == 7 && !GlobalInf.newUserFlag)
		{
			GameUIController.instance.taskEndUIControllor.dialogFlag = true;
		}
		else if (GlobalInf.gameState == 10 && GlobalInf.newUserFlag)
		{
			GameUIController.instance.taskEndUIControllor.dialogFlag = true;
		}
	}
}
