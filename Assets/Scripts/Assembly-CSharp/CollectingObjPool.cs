using System.Collections.Generic;
using UnityEngine;

public class CollectingObjPool : MonoBehaviour
{
	public static CollectingObjPool instance;

	public List<GameObject> collectingCarList;

	public List<GameObject> collectingHandGunList;

	public List<GameObject> collectingMachineGunList;

	public GameObject temp;

	public GameObject collectingCarPreferb;

	public GameObject collectingHandGunPreferb;

	public GameObject collectingMachineGunPreferb;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public GameObject GetCollectingObj(COLLECTTYPE type)
	{
		switch (type)
		{
		case COLLECTTYPE.CAR:
			return GetRightCollectingObj(collectingCarList, collectingCarPreferb);
		case COLLECTTYPE.HANDGUN:
			return GetRightCollectingObj(collectingHandGunList, collectingHandGunPreferb);
		case COLLECTTYPE.MACHINEGUN:
			return GetRightCollectingObj(collectingMachineGunList, collectingMachineGunPreferb);
		default:
			return null;
		}
	}

	public GameObject GetRightCollectingObj(List<GameObject> collectingObjList, GameObject preferb)
	{
		if (collectingObjList.Count == 0)
		{
			temp = (GameObject)Object.Instantiate(preferb);
			temp.transform.parent = base.transform;
			return temp;
		}
		temp = collectingObjList[0];
		collectingObjList.RemoveAt(0);
		return temp;
	}

	public void RecycleCollectingObj(GameObject obj, COLLECTTYPE type)
	{
		switch (type)
		{
		case COLLECTTYPE.CAR:
			RecycleRightCollectingObj(collectingCarList, obj);
			break;
		case COLLECTTYPE.HANDGUN:
			RecycleRightCollectingObj(collectingHandGunList, obj);
			break;
		case COLLECTTYPE.MACHINEGUN:
			RecycleRightCollectingObj(collectingMachineGunList, obj);
			break;
		}
	}

	public void RecycleRightCollectingObj(List<GameObject> ragdoll, GameObject obj)
	{
		obj.transform.parent = base.transform;
		obj.SetActiveRecursively(false);
		ragdoll.Add(obj);
	}
}
