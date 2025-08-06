using System.Collections.Generic;
using UnityEngine;

public class AttackAILabelPool : MonoBehaviour
{
	public static AttackAILabelPool instance;

	public List<GameObject> enableList;

	public List<GameObject> disableList;

	public List<GameObject> targetPos;

	public GameObject sourceObj;

	public Transform parentRoot;

	private GameObject tempObj;

	public Material picMat;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Update()
	{
		for (int i = 0; i < targetPos.Count; i++)
		{
			enableList[i].transform.localPosition = ChangToMapLocalPos(targetPos[i].transform.position);
		}
	}

	public Vector3 ChangToMapLocalPos(Vector3 pos)
	{
		float x = (pos.x - (float)VirtualminiMapController.instance.blockMap.startX + 200f) / 5f;
		float z = (pos.z - (float)VirtualminiMapController.instance.blockMap.startY + 100f) / 5f;
		return new Vector3(x, 5f, z);
	}

	public void AddAttackAI(GameObject obj)
	{
		if (!targetPos.Contains(obj))
		{
			targetPos.Add(obj);
			GetObj();
		}
	}

	public void RemoveAttackAI(GameObject obj)
	{
		if (targetPos.Contains(obj))
		{
			targetPos.Remove(obj);
			RecycleObj();
		}
	}

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
			tempObj.transform.parent = parentRoot;
			enableList.Add(tempObj);
			tempObj.GetComponent<Renderer>().sharedMaterial = picMat;
		}
		tempObj.SetActiveRecursively(true);
		return tempObj;
	}

	public void RecycleObj()
	{
		tempObj = enableList[0];
		tempObj.gameObject.SetActiveRecursively(false);
		enableList.RemoveAt(0);
		disableList.Add(tempObj);
	}
}
