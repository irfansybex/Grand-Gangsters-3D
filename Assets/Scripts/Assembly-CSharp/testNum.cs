using UnityEngine;

[ExecuteInEditMode]
public class testNum : MonoBehaviour
{
	public bool runFlag;

	public int num;

	public RoadInfoList road;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			Count();
		}
	}

	public void Count()
	{
		num = 0;
		for (int i = 0; i < road.roadInfoList.Count; i++)
		{
			num += road.roadInfoList[i].roadPointList.Count;
		}
	}
}
