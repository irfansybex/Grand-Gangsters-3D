using System.Collections.Generic;
using UnityEngine;

public class AIDetect : DetectRoot
{
	public GameObject nearestObs;

	public Transform AI;

	public List<GameObject> objList;

	private GameObject tempObj;

	private int count;

	private void OnTriggerEnter(Collider other)
	{
		tempObj = other.gameObject;
		if (tempObj.layer != LayerMask.NameToLayer("Triger") && tempObj.layer != LayerMask.NameToLayer("Floor") && tempObj.layer != LayerMask.NameToLayer("Ragdoll") && tempObj.layer != LayerMask.NameToLayer("DodgeArea"))
		{
			if (nearestObs == null)
			{
				nearestObs = tempObj;
			}
			else if ((tempObj.transform.position - AI.position).sqrMagnitude < (nearestObs.transform.position - AI.position).sqrMagnitude)
			{
				nearestObs = tempObj;
			}
			if (!objList.Contains(tempObj))
			{
				objList.Add(tempObj);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		tempObj = other.gameObject;
		if (tempObj.layer == LayerMask.NameToLayer("Triger") || tempObj.layer == LayerMask.NameToLayer("Floor") || tempObj.layer == LayerMask.NameToLayer("Ragdoll") || tempObj.layer == LayerMask.NameToLayer("DodgeArea"))
		{
			return;
		}
		objList.Remove(tempObj);
		if (!(tempObj == nearestObs))
		{
			return;
		}
		if (objList.Count == 0)
		{
			nearestObs = null;
			return;
		}
		if (objList.Count == 1)
		{
			nearestObs = objList[0];
			return;
		}
		nearestObs = objList[0];
		for (int i = 1; i < objList.Count; i++)
		{
			if ((objList[i].transform.position - AI.position).sqrMagnitude < (nearestObs.transform.position - AI.position).sqrMagnitude)
			{
				nearestObs = objList[i];
			}
		}
	}

	private void Update()
	{
		if (objList.Count <= 1)
		{
			return;
		}
		count++;
		if (count % 5 != 0)
		{
			return;
		}
		count = 0;
		for (int i = 0; i < objList.Count; i++)
		{
			if ((objList[i].transform.position - AI.position).sqrMagnitude < (nearestObs.transform.position - AI.position).sqrMagnitude)
			{
				nearestObs = objList[i];
			}
		}
	}

	public override void RemoveDisappearObj(GameObject other)
	{
		if (!(other == nearestObs))
		{
			return;
		}
		objList.Remove(other);
		if (objList.Count == 0)
		{
			nearestObs = null;
			return;
		}
		if (objList.Count == 1)
		{
			nearestObs = objList[0];
			return;
		}
		nearestObs = objList[0];
		for (int i = 1; i < objList.Count; i++)
		{
			if ((objList[i].transform.position - AI.position).sqrMagnitude < (nearestObs.transform.position - AI.position).sqrMagnitude)
			{
				nearestObs = objList[i];
			}
		}
	}
}
