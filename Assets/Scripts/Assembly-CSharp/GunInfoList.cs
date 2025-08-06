using UnityEngine;

public class GunInfoList : MonoBehaviour
{
	public static GunInfoList instance;

	public GunsInfo[] machineGunInfoList;

	public GunsInfo[] handGunInfoList;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public GunsInfo GetGunInfo(int gunType, int gunLevel)
	{
		switch (gunType)
		{
		case 0:
			return machineGunInfoList[gunLevel];
		case 1:
			return handGunInfoList[gunLevel];
		default:
			return handGunInfoList[gunLevel];
		}
	}
}
