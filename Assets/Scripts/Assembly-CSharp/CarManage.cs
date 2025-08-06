using System.Collections.Generic;
using UnityEngine;

public class CarManage : MonoBehaviour
{
	public static CarManage instance;

	public PlayerController player;

	public VechicleController nearestCar;

	public List<CarController> carList;

	public VechicleController tempCar;

	public Vector3 inversePos;

	public CamRotate cam;

	public VechicleController playerCar;

	public float coutt;

	private void Start()
	{
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Update()
	{
		if (!GlobalInf.firstOpenGameFlag && playerCar != null)
		{
			coutt += Time.deltaTime;
			if (coutt > 5f)
			{
				coutt = 0f;
				if (ToolFunction.SqrDistance(playerCar.transform.position, player.transform.position) > 10000f)
				{
					if (playerCar.gameObject.active)
					{
						playerCar.gameObject.SetActiveRecursively(false);
					}
				}
				else if (!playerCar.gameObject.active)
				{
					playerCar.gameObject.SetActiveRecursively(true);
				}
			}
		}
		if (PlayerController.instance.curState == PLAYERSTATE.CAR)
		{
			return;
		}
		if (PlayerController.instance.curState == PLAYERSTATE.DIE)
		{
			GameUIController.instance.OnDisableGetOnCarBtn();
			return;
		}
		nearestCar = GetNearestCar();
		if (!(nearestCar != null))
		{
			return;
		}
		inversePos = nearestCar.getOnPoint.transform.InverseTransformPoint(player.transform.position);
		if (inversePos.z > -3f && inversePos.z < 0.3f && inversePos.x > -3f && inversePos.x < 1.5f)
		{
			if (nearestCar.AICarCtl.policeFlag)
			{
				if (nearestCar.AICarCtl.moveFlag)
				{
					GameUIController.instance.OnDisableGetOnCarBtn();
				}
				else
				{
					GameUIController.instance.OnEnableGetOnCarBtn();
				}
			}
			else if (GameController.instance.curGameMode == GAMEMODE.DRIVING0)
			{
				if (nearestCar != playerCar)
				{
					GameUIController.instance.OnDisableGetOnCarBtn();
				}
				else
				{
					GameUIController.instance.OnEnableGetOnCarBtn();
				}
			}
			else
			{
				GameUIController.instance.OnEnableGetOnCarBtn();
			}
		}
		else
		{
			GameUIController.instance.OnDisableGetOnCarBtn();
		}
	}

	public void OnGetOnCarBtnClick()
	{
		if (!player.changingTheCarFlag)
		{
			if (player.curState == PLAYERSTATE.CAR)
			{
				player.GetOffCar();
			}
			else
			{
				player.GetOnCar(nearestCar.getOnPoint.transform, nearestCar);
			}
			player.changingTheCarFlag = true;
		}
	}

	public VechicleController GetNearestCar()
	{
		if (AICarPoolController.instance.enableList.Count > 0)
		{
			tempCar = AICarPoolController.instance.enableList[0].carCtl;
			for (int i = 1; i < AICarPoolController.instance.enableList.Count; i++)
			{
				if (ToolFunction.SqrDistance(tempCar.getOnPoint.transform.position, player.transform.position) > ToolFunction.SqrDistance(AICarPoolController.instance.enableList[i].transform.position, player.transform.position))
				{
					tempCar = AICarPoolController.instance.enableList[i].carCtl;
				}
			}
		}
		if (playerCar != null && playerCar.gameObject.active)
		{
			if (tempCar != null)
			{
				if (ToolFunction.SqrDistance(tempCar.getOnPoint.transform.position, player.transform.position) > ToolFunction.SqrDistance(playerCar.transform.position, player.transform.position))
				{
					tempCar = playerCar;
				}
			}
			else
			{
				tempCar = playerCar;
			}
		}
		if (GameController.instance.curGameMode == GAMEMODE.ROBMOTOR)
		{
			if (tempCar != null)
			{
				if (ToolFunction.SqrDistance(tempCar.getOnPoint.transform.position, player.transform.position) > ToolFunction.SqrDistance(GameController.instance.robMotorMode.targetMotor.transform.position, player.transform.position))
				{
					tempCar = GameController.instance.robMotorMode.targetMotor;
				}
			}
			else
			{
				tempCar = GameController.instance.robMotorMode.targetMotor;
			}
		}
		return tempCar;
	}

	public void ResetPlayerCarPos()
	{
		if (playerCar != null)
		{
			if (GameUIController.instance.curTaskInfo != null && GameUIController.instance.curTaskInfo.taskMode != 0)
			{
				playerCar.transform.position = GameUIController.instance.curTaskInfo.exitPlayerCarPos;
				playerCar.transform.eulerAngles = GameUIController.instance.curTaskInfo.exitPlayerCarAngle;
			}
			else
			{
				playerCar.transform.position = TaskLabelController.instance.lastInfo.exitPlayerCarPos;
				playerCar.transform.eulerAngles = TaskLabelController.instance.lastInfo.exitPlayerCarAngle;
			}
			playerCar.ResetPlayerCar();
			GameController.instance.driveMode.minimapController.EnablePlayerCarPos(playerCar.transform.position);
		}
	}

	private void OnDestroy()
	{
		instance = null;
	}
}
