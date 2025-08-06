using System.Collections.Generic;
using UnityEngine;

public class StartLabelPool : MonoBehaviour
{
	public static StartLabelPool instance;

	public List<OnActiveActionCtl> drivingLabelList;

	public List<OnActiveActionCtl> deliverLabelList;

	public List<OnActiveActionCtl> survivalLabelList;

	public List<OnActiveActionCtl> slotLabelList;

	public List<OnActiveActionCtl> gunKillingLabelList;

	public List<OnActiveActionCtl> carKillingLabelList;

	public List<OnActiveActionCtl> hospitalLabelList;

	public List<OnActiveActionCtl> skillDrivingLabelList;

	public List<OnActiveActionCtl> robCarLabelList;

	public List<OnActiveActionCtl> fightingLabelList;

	public List<OnActiveActionCtl> robMotorLabelList;

	public OnActiveActionCtl temp;

	public OnActiveActionCtl drivingPreferb;

	public OnActiveActionCtl deliverPreferb;

	public OnActiveActionCtl survivalPreferb;

	public OnActiveActionCtl slotPreferb;

	public OnActiveActionCtl gunKillingPreferb;

	public OnActiveActionCtl carKillingPreferb;

	public OnActiveActionCtl skillDrivingPreferb;

	public OnActiveActionCtl robCarPreferb;

	public OnActiveActionCtl fightingPreferb;

	public OnActiveActionCtl robMotorPreferb;

	public List<SlotController> slotList;

	public SlotController slotObjPreferb;

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

	public OnActiveActionCtl GetDrivingLabel(TaskInfo info)
	{
		if (drivingLabelList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(drivingPreferb.gameObject);
			return ResetInfo(gameObject.GetComponent<OnActiveActionCtl>(), info);
		}
		temp = drivingLabelList[0];
		drivingLabelList.RemoveAt(0);
		return ResetInfo(temp, info);
	}

	public void RecycleDrivingLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		drivingLabelList.Add(obj);
	}

	public OnActiveActionCtl GetDeliverLabel(TaskInfo info)
	{
		if (deliverLabelList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(deliverPreferb.gameObject);
			return ResetInfo(gameObject.GetComponent<OnActiveActionCtl>(), info);
		}
		temp = deliverLabelList[0];
		deliverLabelList.RemoveAt(0);
		return ResetInfo(temp, info);
	}

	public void RecycleDeliverLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		deliverLabelList.Add(obj);
	}

	public OnActiveActionCtl GetRobCarLabel(TaskInfo info)
	{
		if (robCarLabelList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(robCarPreferb.gameObject);
			return ResetInfo(gameObject.GetComponent<OnActiveActionCtl>(), info);
		}
		temp = robCarLabelList[0];
		robCarLabelList.RemoveAt(0);
		return ResetInfo(temp, info);
	}

	public void RecycleRobCarLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		robCarLabelList.Add(obj);
	}

	public OnActiveActionCtl GetFightingLabel(TaskInfo info)
	{
		if (fightingLabelList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(fightingPreferb.gameObject);
			return ResetInfo(gameObject.GetComponent<OnActiveActionCtl>(), info);
		}
		temp = fightingLabelList[0];
		fightingLabelList.RemoveAt(0);
		return ResetInfo(temp, info);
	}

	public void RecycleFightingLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		fightingLabelList.Add(obj);
	}

	public OnActiveActionCtl GetRobMotorLabel(TaskInfo info)
	{
		if (robMotorLabelList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(robMotorPreferb.gameObject);
			return ResetInfo(gameObject.GetComponent<OnActiveActionCtl>(), info);
		}
		temp = robMotorLabelList[0];
		robMotorLabelList.RemoveAt(0);
		return ResetInfo(temp, info);
	}

	public void RecycleRobMotorLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		robMotorLabelList.Add(obj);
	}

	public OnActiveActionCtl GetSurvivalLabel(TaskInfo info)
	{
		if (survivalLabelList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(survivalPreferb.gameObject);
			return ResetInfo(gameObject.GetComponent<OnActiveActionCtl>(), info);
		}
		temp = survivalLabelList[0];
		survivalLabelList.RemoveAt(0);
		return ResetInfo(temp, info);
	}

	public void RecycleSurvivalLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		survivalLabelList.Add(obj);
	}

	public OnActiveActionCtl GetGunKillingLabel(TaskInfo info)
	{
		if (gunKillingLabelList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(gunKillingPreferb.gameObject);
			return ResetInfo(gameObject.GetComponent<OnActiveActionCtl>(), info);
		}
		temp = gunKillingLabelList[0];
		gunKillingLabelList.RemoveAt(0);
		return ResetInfo(temp, info);
	}

	public void RecycleGunKillingLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		gunKillingLabelList.Add(obj);
	}

	public OnActiveActionCtl GetCarKillingLabel(TaskInfo info)
	{
		if (carKillingLabelList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(carKillingPreferb.gameObject);
			return ResetInfo(gameObject.GetComponent<OnActiveActionCtl>(), info);
		}
		temp = carKillingLabelList[0];
		carKillingLabelList.RemoveAt(0);
		return ResetInfo(temp, info);
	}

	public void RecycleCarKillingLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		carKillingLabelList.Add(obj);
	}

	public OnActiveActionCtl GetSkillDrivingLabel(TaskInfo info)
	{
		if (skillDrivingLabelList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(skillDrivingPreferb.gameObject);
			return ResetInfo(gameObject.GetComponent<OnActiveActionCtl>(), info);
		}
		temp = skillDrivingLabelList[0];
		skillDrivingLabelList.RemoveAt(0);
		return ResetInfo(temp, info);
	}

	public void RecycleSkillDrivingLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		skillDrivingLabelList.Add(obj);
	}

	public void RecycleHospitalLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		hospitalLabelList.Add(obj);
	}

	public OnActiveActionCtl ResetInfo(OnActiveActionCtl ctl, TaskInfo info)
	{
		ctl.gameMode = info.taskMode;
		ctl.taskIndex = info.taskIndex;
		ctl.transform.position = info.startPos;
		ctl.rewardIndex = info.rewardIndex;
		ctl.gameObject.SetActiveRecursively(true);
		ctl.transform.parent = base.transform;
		return ctl;
	}

	public OnActiveActionCtl GetSlotLabel(TaskInfo info)
	{
		if (slotLabelList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(slotPreferb.gameObject);
			return ResetInfo(gameObject.GetComponent<OnActiveActionCtl>(), info);
		}
		temp = slotLabelList[0];
		slotLabelList.RemoveAt(0);
		return ResetInfo(temp, info);
	}

	public void RecycleSlotLabel(OnActiveActionCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		slotLabelList.Add(obj);
	}

	public SlotController GetSlot(Vector3 pos, Vector3 rot)
	{
		if (slotList.Count == 0)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(slotObjPreferb.gameObject);
			gameObject.transform.position = pos;
			gameObject.transform.eulerAngles = rot;
			gameObject.gameObject.SetActiveRecursively(true);
			return gameObject.GetComponent<SlotController>();
		}
		SlotController slotController = slotList[0];
		slotList.RemoveAt(0);
		slotController.transform.position = pos;
		slotController.transform.eulerAngles = rot;
		slotController.gameObject.SetActiveRecursively(true);
		return slotController;
	}

	public void RecycleSlot(SlotController obj)
	{
		obj.gameObject.SetActiveRecursively(false);
		slotList.Add(obj);
	}
}
