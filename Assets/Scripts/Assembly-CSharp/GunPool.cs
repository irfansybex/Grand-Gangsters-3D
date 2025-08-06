using System.Collections.Generic;
using UnityEngine;

public class GunPool : MonoBehaviour
{
	public List<GunCtl> enableList;

	public List<GunCtl> disableList;

	public GunCtl souce;

	private GunCtl tempGun;

	private GameObject tempObj;

	private int index;

	public GunCtl GetGun()
	{
		if (disableList.Count > 0)
		{
			tempGun = disableList[0];
			disableList.RemoveAt(0);
			enableList.Add(tempGun);
		}
		else
		{
			tempObj = Object.Instantiate(souce.gameObject) as GameObject;
			tempObj.name = tempObj.name + string.Empty + index++;
			tempGun = tempObj.GetComponent<GunCtl>();
			enableList.Add(tempGun);
		}
		return tempGun;
	}

	public void Recycle(GunCtl gun)
	{
		gun.gameObject.SetActiveRecursively(false);
		enableList.Remove(gun);
		disableList.Add(gun);
	}
}
