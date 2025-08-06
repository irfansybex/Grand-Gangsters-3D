using UnityEngine;

[ExecuteInEditMode]
public class testProfb : MonoBehaviour
{
	public RoadInfo road;

	public bool runFlag;

	public bool flag;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			if (flag)
			{
				road.roadPointList[1].stop = true;
			}
			else
			{
				road.roadPointList[1].stop = false;
			}
		}
	}
}
