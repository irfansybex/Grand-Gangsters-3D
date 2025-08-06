using UnityEngine;

[ExecuteInEditMode]
public class GneerateWayPoint : MonoBehaviour
{
	public bool runAddRoadInfoComponentFlag;

	public bool runAddComponentFlag;

	public GameObject wayRoot;

	private void Start()
	{
	}

	private void Update()
	{
		if (runAddRoadInfoComponentFlag)
		{
			MonoBehaviour.print("run GneerateWayPoint");
			runAddRoadInfoComponentFlag = false;
			AddRoadInfoComponent(wayRoot);
		}
		if (runAddComponentFlag)
		{
			MonoBehaviour.print("run runAddComponentFlag");
			runAddComponentFlag = false;
			AddComp(wayRoot);
		}
	}

	public void AddComp(GameObject root)
	{
		for (int i = 0; i < root.transform.childCount; i++)
		{
			GameObject gameObject = root.transform.GetChild(i).gameObject;
			if (gameObject.GetComponent<RoadInfo>() == null)
			{
				gameObject.AddComponent<RoadInfo>();
			}
			AddSubComp(gameObject);
		}
	}

	public void AddSubComp(GameObject root)
	{
		for (int i = 0; i < root.transform.childCount; i++)
		{
			GameObject gameObject = root.transform.GetChild(i).gameObject;
			if (gameObject.GetComponent<RoadPointInfo>() == null)
			{
				gameObject.AddComponent<RoadPointInfo>();
			}
		}
	}

	public void AddRoadInfoComponent(GameObject root)
	{
		if (root.GetComponent<RoadInfoList>() == null)
		{
			root.AddComponent<RoadInfoList>();
		}
		if (root.GetComponent<RoadInfoList>().roadInfoList.Count > 0)
		{
			root.GetComponent<RoadInfoList>().roadInfoList.Clear();
		}
		for (int i = 0; i < root.transform.childCount; i++)
		{
			GameObject gameObject = root.transform.GetChild(i).gameObject;
			if (gameObject.GetComponent<RoadInfo>() == null)
			{
				MonoBehaviour.print("ffffffffff");
				MonoBehaviour.print(gameObject.name);
				gameObject.AddComponent<RoadInfo>();
			}
			if (gameObject.transform.childCount < 4)
			{
				gameObject.GetComponent<RoadInfo>().straitRoadFlag = true;
			}
			MonoBehaviour.print(gameObject.name);
			InitRoadInfo(gameObject.GetComponent<RoadInfo>());
			root.GetComponent<RoadInfoList>().roadInfoList.Add(gameObject.GetComponent<RoadInfo>());
		}
	}

	public void InitRoadInfo(RoadInfo roadInf)
	{
		if (roadInf.roadPointList.Count > 0)
		{
			roadInf.roadPointList.Clear();
		}
		for (int i = 0; i < roadInf.transform.childCount; i++)
		{
			GameObject gameObject = roadInf.transform.GetChild(i).gameObject;
			if (gameObject.GetComponent<RoadPointInfo>() == null)
			{
				gameObject.AddComponent<RoadPointInfo>();
			}
			roadInf.roadPointList.Add(gameObject.GetComponent<RoadPointInfo>());
		}
	}
}
