using UnityEngine;

[ExecuteInEditMode]
public class InitBuildingPool : MonoBehaviour
{
	public bool runFlag;

	public BuildingPoolList poolList;

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			InitPool();
		}
	}

	public void InitPool()
	{
	}
}
