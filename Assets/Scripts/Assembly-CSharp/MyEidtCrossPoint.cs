using UnityEngine;

[ExecuteInEditMode]
public class MyEidtCrossPoint : MonoBehaviour
{
	public InitMapInfo initMap;

	private RoadInfoList roadinfolist;

	public GameObject preferbLightSystem;

	public LightSystem cross;

	public bool runFlag;

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			roadinfolist = initMap.roadRoot.GetComponent<RoadInfoList>();
			GameObject gameObject = Object.Instantiate(preferbLightSystem) as GameObject;
			cross = gameObject.GetComponent<LightSystem>();
			cross.transform.parent = initMap.roadRoot.transform;
			cross.transform.localPosition = Vector3.zero;
			cross.transform.localRotation = Quaternion.identity;
			AddCrossPoint();
		}
	}

	public void AddCrossPoint()
	{
		for (int i = 0; i < roadinfolist.roadInfoList.Count; i++)
		{
			if (roadinfolist.roadInfoList[i].straitRoadFlag)
			{
				CreateACross(roadinfolist.roadInfoList[i].roadPointList[0]);
				CreateACross(roadinfolist.roadInfoList[i].roadPointList[roadinfolist.roadInfoList[i].roadPointList.Count - 1]);
			}
			else
			{
				CreateACross(roadinfolist.roadInfoList[i].roadPointList[1]);
				CreateACross(roadinfolist.roadInfoList[i].roadPointList[roadinfolist.roadInfoList[i].roadPointList.Count - 2]);
			}
		}
	}

	public void CreateACross(RoadPointInfo point)
	{
		if (point.linkPoint.Count <= 2)
		{
			return;
		}
		MonoBehaviour.print(cross.lightpoint.Count);
		for (int i = 0; i < cross.lightpoint.Count; i++)
		{
			for (int j = 0; j < cross.lightpoint[i].CrossPoint.Count; j++)
			{
				if (cross.lightpoint[i].CrossPoint[j] == point)
				{
					return;
				}
			}
		}
		LightPoint lightPoint = new LightPoint();
		MonoBehaviour.print(lightPoint);
		lightPoint.CrossPoint.Add(point);
		lightPoint.CrossPoint.Add(FindRight(point));
		lightPoint.CrossPoint.Add(FindCross(point));
		lightPoint.CrossPoint.Add(FindLeft(point));
		for (int k = 0; k < lightPoint.CrossPoint.Count; k++)
		{
			lightPoint.dummyCrossPoint.Add(lightPoint.CrossPoint[k]);
		}
		cross.lightpoint.Add(lightPoint);
	}

	public RoadPointInfo FindRight(RoadPointInfo point)
	{
		for (int i = 0; i < point.linkPoint.Count; i++)
		{
			if (point.transform.InverseTransformPoint(point.linkPoint[i].transform.position).z > 0f)
			{
				if (point.transform.InverseTransformPoint(point.linkPoint[i].transform.position).x > 5f)
				{
					return point.linkPoint[i];
				}
			}
			else if (point.transform.InverseTransformPoint(point.linkPoint[i].transform.position).x < -5f)
			{
				return point.linkPoint[i];
			}
		}
		return null;
	}

	public RoadPointInfo FindLeft(RoadPointInfo point)
	{
		for (int i = 0; i < point.linkPoint.Count; i++)
		{
			if (point.transform.InverseTransformPoint(point.linkPoint[i].transform.position).z < 0f)
			{
				if (point.transform.InverseTransformPoint(point.linkPoint[i].transform.position).x > 5f)
				{
					return point.linkPoint[i];
				}
			}
			else if (point.transform.InverseTransformPoint(point.linkPoint[i].transform.position).x < -5f)
			{
				return point.linkPoint[i];
			}
		}
		return null;
	}

	public RoadPointInfo FindCross(RoadPointInfo point)
	{
		for (int i = 0; i < point.linkPoint.Count; i++)
		{
			if (point.linkPoint[i].roadInfo != point.roadInfo && (point.transform.InverseTransformPoint(point.linkPoint[i].transform.position).z > 15f || point.transform.InverseTransformPoint(point.linkPoint[i].transform.position).z < -15f))
			{
				return point.linkPoint[i];
			}
		}
		return null;
	}
}
