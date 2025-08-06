using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CrossPointEdit : MonoBehaviour
{
	public InitMapInfo initMap;

	private RoadInfoList roadinfolist;

	public GameObject preferbLightSystem;

	public LightSystem cross;

	public bool isgo;

	private void Update()
	{
		if (isgo)
		{
			isgo = false;
			roadinfolist = initMap.roadRoot.GetComponent<RoadInfoList>();
			GameObject gameObject = Object.Instantiate(preferbLightSystem) as GameObject;
			cross = gameObject.GetComponent<LightSystem>();
			cross.transform.parent = initMap.roadRoot.transform;
			cross.transform.localPosition = Vector3.zero;
			cross.transform.localRotation = Quaternion.identity;
			addCrossPoint();
		}
	}

	public void addCrossPoint()
	{
		for (int i = 0; i < roadinfolist.roadInfoList.Count; i++)
		{
			if (roadinfolist.roadInfoList[i].straitRoadFlag)
			{
				if (roadinfolist.roadInfoList[i].roadPointList[0].linkPoint.Count == 4)
				{
					addlp(0, 4, i);
				}
				else if (roadinfolist.roadInfoList[i].roadPointList[0].linkPoint.Count == 3)
				{
					addlp(0, 3, i);
				}
				int count = roadinfolist.roadInfoList[i].roadPointList.Count;
				if (roadinfolist.roadInfoList[i].roadPointList[count - 1].linkPoint.Count == 4)
				{
					addlp(count - 1, 4, i);
				}
				else if (roadinfolist.roadInfoList[i].roadPointList[count - 1].linkPoint.Count == 3)
				{
					addlp(count - 1, 3, i);
				}
			}
			else
			{
				if (roadinfolist.roadInfoList[i].roadPointList[1].linkPoint.Count == 4)
				{
					addlp(1, 4, i);
				}
				else if (roadinfolist.roadInfoList[i].roadPointList[1].linkPoint.Count == 3)
				{
					addlp(1, 3, i);
				}
				int count2 = roadinfolist.roadInfoList[i].roadPointList.Count;
				if (roadinfolist.roadInfoList[i].roadPointList[count2 - 2].linkPoint.Count == 4)
				{
					addlp(count2 - 2, 4, i);
				}
				else if (roadinfolist.roadInfoList[i].roadPointList[count2 - 2].linkPoint.Count == 3)
				{
					addlp(count2 - 2, 3, i);
				}
			}
		}
	}

	public void addlp(int a, int b, int c)
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < cross.lightpoint.Count; i++)
		{
			for (int j = 0; j < cross.lightpoint[i].CrossPoint.Count; j++)
			{
				if (roadinfolist.roadInfoList[c].roadPointList[a] == cross.lightpoint[i].CrossPoint[j])
				{
					flag2 = true;
					break;
				}
			}
		}
		if (flag2)
		{
			return;
		}
		for (int k = 0; k < b; k++)
		{
			if (roadinfolist.roadInfoList[c].roadPointList[a].roadInfo == roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[k].roadInfo)
			{
				Vector3 to = roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[k].transform.position - roadinfolist.roadInfoList[c].roadPointList[a].transform.position;
				Vector3 forward = roadinfolist.roadInfoList[c].roadPointList[a].transform.forward;
				float num = Vector3.Angle(forward, to);
				flag = num > 90f;
				break;
			}
		}
		if (flag)
		{
			LightPoint lightPoint = new LightPoint();
			lightPoint.CrossPoint = new List<RoadPointInfo>();
			lightPoint.dummyCrossPoint = new List<RoadPointInfo>();
			lightPoint.CrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a]);
			lightPoint.dummyCrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a]);
			bool flag3 = false;
			for (int l = 0; l < b; l++)
			{
				flag3 = false;
				if (roadinfolist.roadInfoList[c].roadPointList[a].roadInfo != roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[l].roadInfo && roadinfolist.roadInfoList[c].roadPointList[a].transform.InverseTransformPoint(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[l].transform.position).x > 8f)
				{
					lightPoint.CrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[l]);
					lightPoint.dummyCrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[l]);
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				lightPoint.CrossPoint.Add(null);
				lightPoint.dummyCrossPoint.Add(null);
			}
			for (int m = 0; m < b; m++)
			{
				flag3 = false;
				if (roadinfolist.roadInfoList[c].roadPointList[a].roadInfo != roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[m].roadInfo && roadinfolist.roadInfoList[c].roadPointList[a].transform.InverseTransformPoint(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[m].transform.position).z > 18f)
				{
					lightPoint.CrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[m]);
					lightPoint.dummyCrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[m]);
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				lightPoint.CrossPoint.Add(null);
				lightPoint.dummyCrossPoint.Add(null);
			}
			for (int n = 0; n < b; n++)
			{
				flag3 = false;
				if (roadinfolist.roadInfoList[c].roadPointList[a].roadInfo != roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[n].roadInfo && roadinfolist.roadInfoList[c].roadPointList[a].transform.InverseTransformPoint(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[n].transform.position).x < -8f)
				{
					lightPoint.CrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[n]);
					lightPoint.dummyCrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[n]);
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				lightPoint.CrossPoint.Add(null);
				lightPoint.dummyCrossPoint.Add(null);
			}
			cross.lightpoint.Add(lightPoint);
			return;
		}
		LightPoint lightPoint2 = new LightPoint();
		lightPoint2.CrossPoint = new List<RoadPointInfo>();
		lightPoint2.dummyCrossPoint = new List<RoadPointInfo>();
		lightPoint2.CrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a]);
		lightPoint2.dummyCrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a]);
		bool flag4 = false;
		for (int num2 = 0; num2 < b; num2++)
		{
			flag4 = false;
			if (roadinfolist.roadInfoList[c].roadPointList[a].roadInfo != roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num2].roadInfo && roadinfolist.roadInfoList[c].roadPointList[a].transform.InverseTransformPoint(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num2].transform.position).x < -8f)
			{
				lightPoint2.CrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num2]);
				lightPoint2.dummyCrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num2]);
				flag4 = true;
				break;
			}
		}
		if (!flag4)
		{
			lightPoint2.CrossPoint.Add(null);
			lightPoint2.dummyCrossPoint.Add(null);
		}
		for (int num3 = 0; num3 < b; num3++)
		{
			flag4 = false;
			if (roadinfolist.roadInfoList[c].roadPointList[a].roadInfo != roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num3].roadInfo && roadinfolist.roadInfoList[c].roadPointList[a].transform.InverseTransformPoint(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num3].transform.position).z < -18f)
			{
				lightPoint2.CrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num3]);
				lightPoint2.dummyCrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num3]);
				flag4 = true;
				break;
			}
		}
		if (!flag4)
		{
			lightPoint2.CrossPoint.Add(null);
			lightPoint2.dummyCrossPoint.Add(null);
		}
		for (int num4 = 0; num4 < b; num4++)
		{
			flag4 = false;
			if (roadinfolist.roadInfoList[c].roadPointList[a].roadInfo != roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num4].roadInfo && roadinfolist.roadInfoList[c].roadPointList[a].transform.InverseTransformPoint(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num4].transform.position).x > 8f)
			{
				lightPoint2.CrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num4]);
				lightPoint2.dummyCrossPoint.Add(roadinfolist.roadInfoList[c].roadPointList[a].linkPoint[num4]);
				flag4 = true;
				break;
			}
		}
		if (!flag4)
		{
			lightPoint2.CrossPoint.Add(null);
			lightPoint2.dummyCrossPoint.Add(null);
		}
		cross.lightpoint.Add(lightPoint2);
	}
}
