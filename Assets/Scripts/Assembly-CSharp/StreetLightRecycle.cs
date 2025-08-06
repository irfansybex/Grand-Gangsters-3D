using UnityEngine;

public class StreetLightRecycle : MonoBehaviour
{
	public bool recycleFlag;

	public BuildingPool objPool;

	public GameObject obj;

	private void OnEnable()
	{
		recycleFlag = false;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (!recycleFlag)
		{
			recycleFlag = true;
			Invoke("RecycleDelay", 1f);
		}
	}

	private void RecycleDelay()
	{
		objPool.RecycleObj(obj);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
