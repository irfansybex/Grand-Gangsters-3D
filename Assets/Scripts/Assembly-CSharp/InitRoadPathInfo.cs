using UnityEngine;

[ExecuteInEditMode]
public class InitRoadPathInfo : MonoBehaviour
{
	public bool runFlag;

	public roadInfoListNew roadList;

	public RoadPathInfo pathInfo;

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			SetInfo();
			MonoBehaviour.print("run");
		}
	}

	public void SetInfo()
	{
		for (int i = 0; i < roadList.roadList.Length; i++)
		{
			pathInfo.VP[i * 2].roadIndex = i;
			pathInfo.VP[i * 2 + 1].roadIndex = i;
		}
	}
}
