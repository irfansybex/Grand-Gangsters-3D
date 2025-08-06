using UnityEngine;

public class GunPoolList : MonoBehaviour
{
	public static GunPoolList instance;

	public GunPool machineGunPool;

	public GunPool handGunPool;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}
