using System.Collections.Generic;
using UnityEngine;

public class BuildingPool : MonoBehaviour
{
	public List<GameObject> enableList;

	public List<GameObject> disableList;

	public GameObject sourceObj;

	public int index;

	private GameObject tempObj;

	public GameObject GetObj()
	{
		if (disableList.Count > 0)
		{
			tempObj = disableList[0];
			disableList.RemoveAt(0);
			enableList.Add(tempObj);
		}
		else
		{
			tempObj = Object.Instantiate(sourceObj) as GameObject;
			tempObj.name = string.Empty + index;
			tempObj.transform.parent = base.transform;
			enableList.Add(tempObj);
		}
		tempObj.SetActiveRecursively(true);
		return tempObj;
	}

	public void RecycleObj(GameObject obj)
	{
		obj.gameObject.SetActiveRecursively(false);
		if (enableList.Remove(obj))
		{
			disableList.Add(obj);
		}
	}
}
