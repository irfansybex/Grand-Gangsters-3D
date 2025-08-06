using UnityEngine;

[ExecuteInEditMode]
public class InitMapInfo : MonoBehaviour
{
	public bool runFlag1;

	public bool runFlag2;

	public bool runFlag3;

	public GameObject roadRoot;

	public GenerateRoadPointByDistance genRoadPoint;

	public InitMapDate initMap;

	public int startX;

	public int endX;

	public int startY;

	public int endY;

	public RoadInfoList rList;

	public bool DrawLineFlag;

	private void Update()
	{
		if (runFlag1)
		{
			runFlag1 = false;
			AddScript(roadRoot);
		}
		if (runFlag2)
		{
			runFlag2 = false;
			SetBaseInfo(roadRoot);
		}
		if (runFlag3)
		{
			runFlag3 = false;
			initMap.InitMap(roadRoot.GetComponent<RoadInfoList>(), roadRoot.GetComponent<BlockMapDate>());
		}
	}

	public void InitMap()
	{
		initMap.InitMap(roadRoot.GetComponent<RoadInfoList>(), roadRoot.GetComponent<BlockMapDate>());
	}

	public void AddScript(GameObject root)
	{
		root.AddComponent<BlockMapDate>();
		root.AddComponent<RoadInfoList>();
		root.GetComponent<BlockMapDate>().startX = startX;
		root.GetComponent<BlockMapDate>().endX = endX;
		root.GetComponent<BlockMapDate>().startY = startY;
		root.GetComponent<BlockMapDate>().endY = endY;
		for (int i = 0; i < root.transform.childCount; i++)
		{
			root.transform.GetChild(i).gameObject.AddComponent<RoadInfo>();
			for (int j = 0; j < root.transform.GetChild(i).childCount; j++)
			{
				if (root.transform.GetChild(i).GetChild(j).gameObject.GetComponent<RoadPointInfo>() == null)
				{
					root.transform.GetChild(i).GetChild(j).gameObject.AddComponent<RoadPointInfo>();
				}
			}
		}
	}

	public void SetBaseInfo(GameObject root)
	{
		RoadInfoList roadInfoList = (rList = root.GetComponent<RoadInfoList>());
		roadInfoList.roadInfoList.Clear();
		for (int i = 0; i < root.transform.childCount; i++)
		{
			RoadInfo component = root.transform.GetChild(i).gameObject.GetComponent<RoadInfo>();
			roadInfoList.roadInfoList.Add(component);
			string text = component.gameObject.name;
			component.roadPointList.Clear();
			for (int j = 0; j < root.transform.GetChild(i).childCount; j++)
			{
				string text2 = ((j >= 9) ? (string.Empty + (j + 1)) : ("0" + (j + 1)));
				string text3 = text + "_point" + text2;
				RoadPointInfo component2 = root.transform.GetChild(i).Find(text3).gameObject.GetComponent<RoadPointInfo>();
				component.roadPointList.Add(component2);
			}
			if (component.transform.childCount == 2)
			{
				component.straitRoadFlag = true;
			}
			else
			{
				component.straitRoadFlag = false;
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!DrawLineFlag)
		{
			return;
		}
		for (int i = 0; i < rList.roadInfoList.Count; i++)
		{
			for (int j = 0; j < rList.roadInfoList[i].roadPointList.Count; j++)
			{
				for (int k = 0; k < rList.roadInfoList[i].roadPointList[j].linkPoint.Count; k++)
				{
					Gizmos.DrawLine(rList.roadInfoList[i].roadPointList[j].transform.position, rList.roadInfoList[i].roadPointList[j].linkPoint[k].transform.position);
				}
			}
		}
	}
}
