using UnityEngine;

[ExecuteInEditMode]
public class InitMiniMapCrossPoint : MonoBehaviour
{
	public bool runFlag;

	private bool preInitFlag;

	public InitMapInfo initMap;

	private RoadInfoList roadList;

	private void Update()
	{
		if (preInitFlag)
		{
			preInitFlag = false;
			InitCrossPoint();
			MonoBehaviour.print("Done");
		}
		if (runFlag)
		{
			runFlag = false;
			MonoBehaviour.print("ffffffffffff");
			roadList = initMap.roadRoot.GetComponent<RoadInfoList>();
			PreInit();
			preInitFlag = true;
		}
	}

	public void PreInit()
	{
		for (int i = 0; i < roadList.roadInfoList.Count; i++)
		{
			if (roadList.roadInfoList[i].straitRoadFlag)
			{
				if (roadList.roadInfoList[i].roadPointList[0].gameObject.GetComponent<CrossingPoint>() == null)
				{
					roadList.roadInfoList[i].roadPointList[0].gameObject.AddComponent<CrossingPoint>();
				}
				if (roadList.roadInfoList[i].roadPointList[roadList.roadInfoList[i].roadPointList.Count - 1].gameObject.GetComponent<CrossingPoint>() == null)
				{
					roadList.roadInfoList[i].roadPointList[roadList.roadInfoList[i].roadPointList.Count - 1].gameObject.AddComponent<CrossingPoint>();
				}
			}
			else
			{
				if (roadList.roadInfoList[i].roadPointList[1].gameObject.GetComponent<CrossingPoint>() == null)
				{
					roadList.roadInfoList[i].roadPointList[1].gameObject.AddComponent<CrossingPoint>();
				}
				if (roadList.roadInfoList[i].roadPointList[roadList.roadInfoList[i].roadPointList.Count - 2].gameObject.GetComponent<CrossingPoint>() == null)
				{
					roadList.roadInfoList[i].roadPointList[roadList.roadInfoList[i].roadPointList.Count - 2].gameObject.AddComponent<CrossingPoint>();
				}
			}
		}
	}

	public void InitCrossPoint()
	{
		for (int i = 0; i < roadList.roadInfoList.Count; i++)
		{
			for (int j = 0; j < roadList.roadInfoList[i].roadPointList.Count; j++)
			{
				if (!(roadList.roadInfoList[i].roadPointList[j].gameObject.GetComponent<CrossingPoint>() != null))
				{
					continue;
				}
				CrossingPoint component = roadList.roadInfoList[i].roadPointList[j].gameObject.GetComponent<CrossingPoint>();
				for (int k = 0; k < roadList.roadInfoList[i].roadPointList[j].linkPoint.Count; k++)
				{
					if (roadList.roadInfoList[i].roadPointList[j].linkPoint[k].roadInfo != roadList.roadInfoList[i].roadPointList[j].roadInfo)
					{
						component.linkPoint.Add(roadList.roadInfoList[i].roadPointList[j].linkPoint[k]);
						component.linkDis.Add(roadList.roadInfoList[i].roadPointList[j].roadDistance[k]);
					}
				}
				if (roadList.roadInfoList[i].roadPointList[j].roadInfo.straitRoadFlag)
				{
					if (roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList[0] == roadList.roadInfoList[i].roadPointList[j])
					{
						component.linkPoint.Add(roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList[roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList.Count - 1]);
					}
					else
					{
						component.linkPoint.Add(roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList[0]);
					}
					component.linkDis.Add(Vector3.Distance(roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList[0].transform.position, roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList[roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList.Count - 1].transform.position));
				}
				else
				{
					if (roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList[1] == roadList.roadInfoList[i].roadPointList[j])
					{
						component.linkPoint.Add(roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList[roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList.Count - 2]);
					}
					else
					{
						component.linkPoint.Add(roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList[1]);
					}
					component.linkDis.Add((roadList.roadInfoList[i].roadPointList[j].roadInfo.roadPointList.Count - 3) * 5);
				}
			}
		}
	}
}
