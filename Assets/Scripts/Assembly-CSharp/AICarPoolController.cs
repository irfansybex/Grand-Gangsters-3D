using System.Collections.Generic;
using UnityEngine;

public class AICarPoolController : MonoBehaviour
{
	public static AICarPoolController instance;

	public List<AIsystem_script> enableList;

	public List<AIsystem_script> car0DisableList;

	public List<AIsystem_script> car1DisableList;

	public List<AIsystem_script> car2DisableList;

	public List<AIsystem_script> car3DisableList;

	public List<AIsystem_script> car4DisableList;

	public List<AIsystem_script> motorDisableList;

	public List<AIsystem_script> motor1DisableList;

	public List<AIsystem_script> motor2DisableList;

	public List<PoliceCar> policeDisableList;

	public float[] roadLane;

	public float carRecycleSqrDis;

	public float policeRecycleSqrDis;

	public int policeCarCount;

	public PhysicMaterial colliderMat;

	public int[] aiCarNumCount;

	public int[] enableAICarNumCount;

	public int policeCarNumCount;

	public AIsystem_script[] aiCarPreferb;

	public PoliceCar policePreferb;

	public Transform genPos;

	public int difNum;

	public int preCarTypeIndex;

	public int preTargetCarNum;

	private int randomNum;

	private int motorIndex;

	private void Awake()
	{
		instance = this;
		instance.carRecycleSqrDis = CitySenceController.instance.carProcuceDistance * CitySenceController.instance.carProcuceDistance * 4f;
		instance.policeRecycleSqrDis = CitySenceController.instance.carProcuceDistance * CitySenceController.instance.carProcuceDistance * 4f;
		policeCarNumCount = 1;
	}

	public void GetCar(Vector3 pos, RoadPointNew currentpo, RoadPointNew prevpo, int lane)
	{
		if (!GlobalDefine.smallPhoneFlag)
		{
			if (GameController.instance.curGameMode != GAMEMODE.ROBCAR)
			{
				randomNum = (randomNum + 1) % (aiCarPreferb.Length - 1);
			}
			else if (enableAICarNumCount[(int)GameController.instance.robbingCarMode.targetCarType] < GameController.instance.robbingCarMode.targetCarMaxNum[GameUIController.instance.taskIndex])
			{
				if (difNum >= 2)
				{
					randomNum = (int)GameController.instance.robbingCarMode.targetCarType;
					preTargetCarNum = enableAICarNumCount[(int)GameController.instance.robbingCarMode.targetCarType];
				}
				else
				{
					if ((preCarTypeIndex + 1) % 5 != (int)GameController.instance.robbingCarMode.targetCarType)
					{
						randomNum = (preCarTypeIndex + 1) % 5;
						preCarTypeIndex = randomNum;
					}
					else
					{
						randomNum = (preCarTypeIndex + 2) % 5;
						preCarTypeIndex = randomNum;
					}
					difNum++;
				}
			}
			else
			{
				if ((preCarTypeIndex + 1) % 5 != (int)GameController.instance.robbingCarMode.targetCarType)
				{
					randomNum = (preCarTypeIndex + 1) % 5;
					preCarTypeIndex = randomNum;
				}
				else
				{
					randomNum = (preCarTypeIndex + 2) % 5;
					preCarTypeIndex = randomNum;
				}
				difNum++;
			}
			switch (randomNum)
			{
			case 0:
				GetRightCar(pos, currentpo, prevpo, lane, car0DisableList, 0);
				break;
			case 1:
				GetRightCar(pos, currentpo, prevpo, lane, car1DisableList, 1);
				break;
			case 2:
				GetRightCar(pos, currentpo, prevpo, lane, car2DisableList, 2);
				break;
			case 3:
				GetRightCar(pos, currentpo, prevpo, lane, car3DisableList, 3);
				break;
			case 4:
				GetRightCar(pos, currentpo, prevpo, lane, car4DisableList, 4);
				break;
			}
			if (GameController.instance.curGameMode == GAMEMODE.ROBCAR && randomNum == (int)GameController.instance.robbingCarMode.targetCarType && enableAICarNumCount[(int)GameController.instance.robbingCarMode.targetCarType] != preTargetCarNum)
			{
				difNum = 0;
			}
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			if (enableAICarNumCount[(preCarTypeIndex + i) % 5] == 0)
			{
				randomNum = (preCarTypeIndex + i) % 5;
				preCarTypeIndex = randomNum;
				break;
			}
		}
		switch (randomNum)
		{
		case 0:
			GetRightCar(pos, currentpo, prevpo, lane, car0DisableList, 0);
			break;
		case 1:
			GetRightCar(pos, currentpo, prevpo, lane, car1DisableList, 1);
			break;
		case 2:
			GetRightCar(pos, currentpo, prevpo, lane, car2DisableList, 2);
			break;
		case 3:
			GetRightCar(pos, currentpo, prevpo, lane, car3DisableList, 3);
			break;
		case 4:
			GetRightCar(pos, currentpo, prevpo, lane, car4DisableList, 4);
			break;
		}
	}

	public void GetRightCar(Vector3 pos, RoadPointNew currentpo, RoadPointNew prevpo, int lane, List<AIsystem_script> carList, int index)
	{
		genPos.forward = (currentpo.position - prevpo.position).normalized;
		pos += genPos.right * roadLane[lane];
		if (!(ToolFunction.InverseZ(currentpo.position, pos, currentpo.forward) < 3f) && CheckCarEmpty(pos))
		{
			if (carList.Count > 0)
			{
				AIsystem_script aIsystem_script = carList[0];
				carList.RemoveAt(0);
				enableList.Add(aIsystem_script);
				aIsystem_script.ResetCar();
				aIsystem_script.CurrentTarget = currentpo;
				aIsystem_script.PrevTarget = prevpo;
				aIsystem_script.transform.position = pos;
				aIsystem_script.transform.forward = genPos.forward;
				aIsystem_script.offsetvalue = roadLane[lane];
				aIsystem_script.offsetindex = lane;
				aIsystem_script.moveFlag = true;
				aIsystem_script.speed = Random.Range(0f, aIsystem_script.MaxSpeed);
				aIsystem_script.isbrake = false;
				aIsystem_script.iscross = false;
				aIsystem_script.carCtl.body.GetComponent<Collider>().sharedMaterial = colliderMat;
				aIsystem_script.moveDirection = genPos.forward;
				aIsystem_script.SetTarget();
				enableAICarNumCount[index]++;
			}
			else if (aiCarNumCount[index] < 4)
			{
				enableAICarNumCount[index]++;
				aiCarNumCount[index]++;
				AIsystem_script aIsystem_script2 = (AIsystem_script)Object.Instantiate(aiCarPreferb[index]);
				aIsystem_script2.transform.parent = base.transform;
				enableList.Add(aIsystem_script2);
				aIsystem_script2.ResetCar();
				aIsystem_script2.CurrentTarget = currentpo;
				aIsystem_script2.PrevTarget = prevpo;
				aIsystem_script2.transform.position = pos;
				aIsystem_script2.transform.forward = genPos.forward;
				aIsystem_script2.offsetvalue = roadLane[lane];
				aIsystem_script2.offsetindex = lane;
				aIsystem_script2.moveFlag = true;
				aIsystem_script2.speed = Random.Range(0f, aIsystem_script2.MaxSpeed);
				aIsystem_script2.isbrake = false;
				aIsystem_script2.iscross = false;
				aIsystem_script2.carCtl.body.GetComponent<Collider>().sharedMaterial = colliderMat;
				aIsystem_script2.moveDirection = genPos.forward;
				aIsystem_script2.SetTarget();
			}
		}
	}

	public void ForceGetRightCar(Vector3 pos, RoadPointNew currentpo, RoadPointNew prevpo, int lane, List<AIsystem_script> carList, int index)
	{
		genPos.forward = (currentpo.position - prevpo.position).normalized;
		pos += genPos.right * roadLane[lane];
		if (!(ToolFunction.InverseZ(currentpo.position, pos, currentpo.forward) < 3f))
		{
			if (carList.Count > 0)
			{
				AIsystem_script aIsystem_script = carList[0];
				carList.RemoveAt(0);
				enableList.Add(aIsystem_script);
				aIsystem_script.ResetCar();
				aIsystem_script.CurrentTarget = currentpo;
				aIsystem_script.PrevTarget = prevpo;
				aIsystem_script.transform.position = pos;
				aIsystem_script.transform.forward = genPos.forward;
				aIsystem_script.offsetvalue = roadLane[lane];
				aIsystem_script.offsetindex = lane;
				aIsystem_script.moveFlag = true;
				aIsystem_script.speed = Random.Range(0f, aIsystem_script.MaxSpeed);
				aIsystem_script.isbrake = false;
				aIsystem_script.iscross = false;
				aIsystem_script.carCtl.body.GetComponent<Collider>().sharedMaterial = colliderMat;
				aIsystem_script.moveDirection = genPos.forward;
				aIsystem_script.SetTarget();
				enableAICarNumCount[index]++;
			}
			else if (aiCarNumCount[index] < 4)
			{
				enableAICarNumCount[index]++;
				aiCarNumCount[index]++;
				AIsystem_script aIsystem_script2 = (AIsystem_script)Object.Instantiate(aiCarPreferb[index]);
				aIsystem_script2.transform.parent = base.transform;
				enableList.Add(aIsystem_script2);
				aIsystem_script2.ResetCar();
				aIsystem_script2.CurrentTarget = currentpo;
				aIsystem_script2.PrevTarget = prevpo;
				aIsystem_script2.transform.position = pos;
				aIsystem_script2.transform.forward = genPos.forward;
				aIsystem_script2.offsetvalue = roadLane[lane];
				aIsystem_script2.offsetindex = lane;
				aIsystem_script2.moveFlag = true;
				aIsystem_script2.speed = Random.Range(0f, aIsystem_script2.MaxSpeed);
				aIsystem_script2.isbrake = false;
				aIsystem_script2.iscross = false;
				aIsystem_script2.carCtl.body.GetComponent<Collider>().sharedMaterial = colliderMat;
				aIsystem_script2.moveDirection = genPos.forward;
				aIsystem_script2.SetTarget();
			}
		}
	}

	public void GetMotor(Vector3 pos, Quaternion rot)
	{
		if (!((pos - PlayerController.instance.transform.position).sqrMagnitude > carRecycleSqrDis) && CheckCarEmpty(pos))
		{
			motorIndex = (motorIndex + 1) % 3;
			switch (motorIndex)
			{
			case 0:
				GetTargetMotor(motorDisableList, 5, pos, rot);
				break;
			case 1:
				GetTargetMotor(motor1DisableList, 6, pos, rot);
				break;
			case 2:
				GetTargetMotor(motor2DisableList, 7, pos, rot);
				break;
			}
		}
	}

	public void GetTargetMotor(List<AIsystem_script> targetList, int targetIndex, Vector3 pos, Quaternion rot)
	{
		if (targetList.Count > 0)
		{
			AIsystem_script aIsystem_script = targetList[0];
			targetList.RemoveAt(0);
			enableList.Add(aIsystem_script);
			aIsystem_script.gameObject.SetActiveRecursively(true);
			aIsystem_script.transform.position = pos;
			aIsystem_script.transform.rotation = rot;
			aIsystem_script.moveFlag = false;
			enableAICarNumCount[targetIndex]++;
		}
		else if (aiCarNumCount[targetIndex] < 10)
		{
			enableAICarNumCount[targetIndex]++;
			aiCarNumCount[targetIndex]++;
			AIsystem_script aIsystem_script2 = (AIsystem_script)Object.Instantiate(aiCarPreferb[targetIndex]);
			aIsystem_script2.transform.parent = base.transform;
			enableList.Add(aIsystem_script2);
			aIsystem_script2.transform.position = pos;
			aIsystem_script2.transform.rotation = rot;
			aIsystem_script2.gameObject.SetActiveRecursively(true);
			aIsystem_script2.moveFlag = false;
		}
	}

	public void GetStaticCar(Transform pos)
	{
		randomNum = (randomNum + 1) % (aiCarPreferb.Length - 1);
		switch (randomNum)
		{
		case 0:
			GetRightStaticCar(pos, car0DisableList, 0);
			break;
		case 1:
			GetRightStaticCar(pos, car1DisableList, 1);
			break;
		case 2:
			GetRightStaticCar(pos, car2DisableList, 2);
			break;
		case 3:
			GetRightStaticCar(pos, car3DisableList, 3);
			break;
		case 4:
			GetRightStaticCar(pos, car4DisableList, 4);
			break;
		}
	}

	public void GetRightStaticCar(Transform pos, List<AIsystem_script> carList, int index)
	{
		if (carList.Count > 0)
		{
			AIsystem_script aIsystem_script = carList[0];
			carList.RemoveAt(0);
			enableList.Add(aIsystem_script);
			aIsystem_script.ResetCar();
			aIsystem_script.transform.position = pos.position;
			aIsystem_script.transform.rotation = pos.rotation;
			aIsystem_script.moveFlag = false;
		}
	}

	public void recylecar(AIsystem_script ob)
	{
		ob.gameObject.SetActiveRecursively(false);
		enableList.Remove(ob);
		ob.carCtl.enabled = false;
		ob.motor.enabled = true;
		if (ob.carCtl.carHealth.healthVal <= 50f && TempObjControllor.instance.curSmokeCar == ob.carCtl)
		{
			TempObjControllor.instance.RecycleSmoke();
		}
		ob.carCtl.carHealth.Reset();
		if (!ob.policeFlag)
		{
			switch (ob.CARINDEX)
			{
			case 0:
				car0DisableList.Add(ob);
				enableAICarNumCount[0]--;
				break;
			case 1:
				car1DisableList.Add(ob);
				enableAICarNumCount[1]--;
				break;
			case 2:
				car2DisableList.Add(ob);
				enableAICarNumCount[2]--;
				break;
			case 3:
				car3DisableList.Add(ob);
				enableAICarNumCount[3]--;
				break;
			case 4:
				car4DisableList.Add(ob);
				enableAICarNumCount[4]--;
				break;
			case 5:
				motorDisableList.Add(ob);
				enableAICarNumCount[5]--;
				break;
			case 6:
				motor1DisableList.Add(ob);
				enableAICarNumCount[6]--;
				break;
			case 7:
				motor2DisableList.Add(ob);
				enableAICarNumCount[7]--;
				break;
			}
			return;
		}
		policeDisableList.Add((PoliceCar)ob);
		if (!((PoliceCar)ob).chaseDoneFlag)
		{
			policeCarCount--;
			if (policeCarCount <= 0)
			{
				GameUIController.instance.CheckPoliceLevel();
			}
		}
		if (((PoliceCar)ob).attackLabelFlag)
		{
			((PoliceCar)ob).attackLabelFlag = false;
			AttackAILabelPool.instance.RemoveAttackAI(ob.gameObject);
		}
	}

	public void GetNormalPoliceCar(Vector3 pos, RoadPointNew currentpo, RoadPointNew prevpo, int lane)
	{
		genPos.forward = (currentpo.position - prevpo.position).normalized;
		pos += genPos.right * roadLane[lane];
		if (CheckCarEmpty(pos))
		{
			if (policeDisableList.Count > 0)
			{
				policeCarCount++;
				PoliceCar policeCar = policeDisableList[0];
				policeDisableList.RemoveAt(0);
				enableList.Add(policeCar);
				policeCar.ResetCar();
				policeCar.CurrentTarget = currentpo;
				policeCar.PrevTarget = prevpo;
				policeCar.transform.position = pos;
				policeCar.transform.forward = genPos.forward;
				policeCar.offsetvalue = roadLane[lane];
				policeCar.offsetindex = lane;
				policeCar.moveFlag = true;
				policeCar.speed = Random.Range(0f, policeCar.MaxSpeed);
				policeCar.isbrake = false;
				policeCar.chasingFlag = false;
				policeCar.driveToPlayerFlag = false;
				policeCar.moveDoneFlag = false;
				policeCar.chaseDoneFlag = false;
				policeCar.moveDoneFlag = false;
				policeCar.blockFlag = false;
				policeCar.carBodyObj[0].GetComponent<Collider>().sharedMaterial = colliderMat;
				policeCar.SetTarget();
			}
			else if (policeCarNumCount < 10)
			{
				policeCarCount++;
				policeCarNumCount++;
				PoliceCar policeCar2 = (PoliceCar)Object.Instantiate(policePreferb);
				policeCar2.transform.parent = base.transform;
				enableList.Add(policeCar2);
				policeCar2.ResetCar();
				policeCar2.CurrentTarget = currentpo;
				policeCar2.PrevTarget = prevpo;
				policeCar2.transform.position = pos;
				policeCar2.transform.forward = genPos.forward;
				policeCar2.offsetvalue = roadLane[lane];
				policeCar2.offsetindex = lane;
				policeCar2.moveFlag = true;
				policeCar2.speed = Random.Range(0f, policeCar2.MaxSpeed);
				policeCar2.isbrake = false;
				policeCar2.chasingFlag = false;
				policeCar2.driveToPlayerFlag = false;
				policeCar2.moveDoneFlag = false;
				policeCar2.chaseDoneFlag = false;
				policeCar2.moveDoneFlag = false;
				policeCar2.blockFlag = false;
				policeCar2.carBodyObj[0].GetComponent<Collider>().sharedMaterial = colliderMat;
				policeCar2.SetTarget();
			}
			else
			{
				policeCarCount++;
				PoliceCar policeCar3 = FindDisablePoliceCar();
				policeCar3.transform.parent = base.transform;
				enableList.Add(policeCar3);
				policeCar3.ResetCar();
				policeCar3.CurrentTarget = currentpo;
				policeCar3.PrevTarget = prevpo;
				policeCar3.transform.position = pos;
				policeCar3.transform.forward = genPos.forward;
				policeCar3.offsetvalue = roadLane[lane];
				policeCar3.offsetindex = lane;
				policeCar3.moveFlag = true;
				policeCar3.speed = Random.Range(0f, policeCar3.MaxSpeed);
				policeCar3.isbrake = false;
				policeCar3.chasingFlag = false;
				policeCar3.driveToPlayerFlag = false;
				policeCar3.moveDoneFlag = false;
				policeCar3.chaseDoneFlag = false;
				policeCar3.moveDoneFlag = false;
				policeCar3.blockFlag = false;
				policeCar3.carBodyObj[0].GetComponent<Collider>().sharedMaterial = colliderMat;
				policeCar3.SetTarget();
			}
		}
	}

	public PoliceCar FindDisablePoliceCar()
	{
		for (int i = 0; i < enableList.Count; i++)
		{
			if (enableList[i].policeFlag && ((PoliceCar)enableList[i]).chaseDoneFlag)
			{
				return (PoliceCar)enableList[i];
			}
		}
		policeCarNumCount++;
		return (PoliceCar)Object.Instantiate(policePreferb);
	}

	public void GetChasingCar(Vector3 pos, RoadPointNew currentpo, RoadPointNew prevpo)
	{
		genPos.forward = (currentpo.position - prevpo.position).normalized;
		pos += genPos.right * 2.5f;
		if (policeDisableList.Count > 0)
		{
			policeCarCount++;
			PoliceCar policeCar = policeDisableList[0];
			policeDisableList.RemoveAt(0);
			enableList.Add(policeCar);
			policeCar.ResetCar();
			policeCar.polTarget = PlayerController.instance.transform.position;
			policeCar.driveToPlayerFlag = true;
			policeCar.CurrentTarget = currentpo;
			policeCar.PrevTarget = prevpo;
			policeCar.transform.position = pos;
			policeCar.transform.forward = genPos.forward;
			policeCar.moveFlag = true;
			policeCar.speed = Random.Range(0f, policeCar.MaxSpeed);
			policeCar.isbrake = false;
			policeCar.chasingFlag = true;
			policeCar.chaseDoneFlag = false;
			policeCar.moveDoneFlag = false;
			policeCar.blockFlag = false;
			policeCar.carCtl.OnEnableCar();
			policeCar.carCtl.enableFlag = false;
		}
		else if (policeDisableList.Count == 0)
		{
			if (policeCarNumCount < 10)
			{
				policeCarCount++;
				policeCarNumCount++;
				PoliceCar policeCar2 = (PoliceCar)Object.Instantiate(policePreferb);
				policeCar2.transform.parent = base.transform;
				enableList.Add(policeCar2);
				policeCar2.ResetCar();
				policeCar2.polTarget = PlayerController.instance.transform.position;
				policeCar2.driveToPlayerFlag = true;
				policeCar2.CurrentTarget = currentpo;
				policeCar2.PrevTarget = prevpo;
				policeCar2.transform.position = pos;
				policeCar2.transform.forward = genPos.forward;
				policeCar2.moveFlag = true;
				policeCar2.speed = Random.Range(0f, policeCar2.MaxSpeed);
				policeCar2.isbrake = false;
				policeCar2.chasingFlag = true;
				policeCar2.chaseDoneFlag = false;
				policeCar2.moveDoneFlag = false;
				policeCar2.blockFlag = false;
				policeCar2.carCtl.OnEnableCar();
				policeCar2.carCtl.enableFlag = false;
			}
			else
			{
				policeCarCount++;
				PoliceCar policeCar3 = FindDisablePoliceCar();
				policeCar3.transform.parent = base.transform;
				enableList.Add(policeCar3);
				policeCar3.ResetCar();
				policeCar3.polTarget = PlayerController.instance.transform.position;
				policeCar3.driveToPlayerFlag = true;
				policeCar3.CurrentTarget = currentpo;
				policeCar3.PrevTarget = prevpo;
				policeCar3.transform.position = pos;
				policeCar3.transform.forward = genPos.forward;
				policeCar3.moveFlag = true;
				policeCar3.speed = Random.Range(0f, policeCar3.MaxSpeed);
				policeCar3.isbrake = false;
				policeCar3.chasingFlag = true;
				policeCar3.chaseDoneFlag = false;
				policeCar3.moveDoneFlag = false;
				policeCar3.blockFlag = false;
				policeCar3.carCtl.OnEnableCar();
				policeCar3.carCtl.enableFlag = false;
			}
		}
	}

	public void GetBlockPoliceCar(Vector3 pos, Vector3 forward)
	{
		genPos.forward = forward;
		pos += genPos.forward * 2.5f * Random.Range(0, 3);
		if (policeDisableList.Count > 0)
		{
			policeCarCount++;
			PoliceCar policeCar = policeDisableList[0];
			policeDisableList.RemoveAt(0);
			enableList.Add(policeCar);
			policeCar.ResetCar();
			policeCar.transform.position = pos;
			policeCar.transform.forward = genPos.right;
			policeCar.moveFlag = true;
			policeCar.blockFlag = false;
			policeCar.chasingFlag = true;
			policeCar.chaseDoneFlag = false;
			policeCar.moveDoneFlag = false;
			policeCar.carCtl.OnEnableCar();
			policeCar.driveToPlayerFlag = true;
			policeCar.carCtl.enableFlag = false;
		}
		else if (policeDisableList.Count == 0)
		{
			if (policeCarNumCount < 10)
			{
				policeCarCount++;
				policeCarNumCount++;
				PoliceCar policeCar2 = (PoliceCar)Object.Instantiate(policePreferb);
				policeCar2.transform.parent = base.transform;
				enableList.Add(policeCar2);
				policeCar2.ResetCar();
				policeCar2.transform.position = pos;
				policeCar2.transform.forward = genPos.right;
				policeCar2.moveFlag = true;
				policeCar2.blockFlag = false;
				policeCar2.chasingFlag = true;
				policeCar2.chaseDoneFlag = false;
				policeCar2.moveDoneFlag = false;
				policeCar2.carCtl.OnEnableCar();
				policeCar2.driveToPlayerFlag = true;
				policeCar2.carCtl.enableFlag = false;
			}
			else
			{
				policeCarCount++;
				PoliceCar policeCar3 = FindDisablePoliceCar();
				policeCar3.transform.parent = base.transform;
				enableList.Add(policeCar3);
				policeCar3.ResetCar();
				policeCar3.transform.position = pos;
				policeCar3.transform.forward = genPos.right;
				policeCar3.moveFlag = true;
				policeCar3.blockFlag = false;
				policeCar3.chasingFlag = true;
				policeCar3.chaseDoneFlag = false;
				policeCar3.moveDoneFlag = false;
				policeCar3.carCtl.OnEnableCar();
				policeCar3.driveToPlayerFlag = true;
				policeCar3.carCtl.enableFlag = false;
			}
		}
	}

	public bool CheckCarEmpty(Vector3 pos)
	{
		for (int i = 0; i < instance.enableList.Count; i++)
		{
			if ((instance.enableList[i].transform.position - pos).sqrMagnitude < 100f)
			{
				return false;
			}
		}
		return true;
	}
}
