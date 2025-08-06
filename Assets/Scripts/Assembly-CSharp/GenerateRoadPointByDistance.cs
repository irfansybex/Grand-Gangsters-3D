using UnityEngine;

[ExecuteInEditMode]
public class GenerateRoadPointByDistance : MonoBehaviour
{
	public bool runFlag;

	public int difDistance;

	public int curveDistance;

	public RoadInfoList roadList;

	private int AddPointNum;

	public MyCRSpline crSpline;

	public GameObject roadPointResource;

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			GeneratePoint(roadList);
		}
	}

	public void GeneratePoint(RoadInfoList list)
	{
		for (int i = 0; i < list.roadInfoList.Count; i++)
		{
			if (!list.roadInfoList[i].straitRoadFlag)
			{
				AddCurvePoint(list.roadInfoList[i]);
			}
			else
			{
				AddStritePoint(list.roadInfoList[i]);
			}
		}
	}

	public void AddCurvePoint(RoadInfo roadInfo)
	{
		if (roadInfo == null)
		{
			MonoBehaviour.print("null");
		}
		crSpline.pts = new Transform[roadInfo.roadPointList.Count];
		float num = 0f;
		for (int i = 0; i < roadInfo.roadPointList.Count; i++)
		{
			crSpline.pts[i] = roadInfo.roadPointList[i].transform;
			if (i > 1 && i < roadInfo.roadPointList.Count - 1)
			{
				num += Vector3.Distance(crSpline.pts[i].position, crSpline.pts[i - 1].position);
			}
		}
		AddPointNum = (int)num / curveDistance;
		for (int j = 0; j <= AddPointNum - 1; j++)
		{
			if (AddPointNum == 1)
			{
				break;
			}
			float t = (float)j / (float)(AddPointNum - 1);
			Vector3 position = crSpline.Interp(t);
			Vector3 forward = crSpline.Velocity(t);
			GameObject gameObject = (GameObject)Object.Instantiate(roadPointResource);
			gameObject.transform.position = position;
			gameObject.transform.forward = forward;
			gameObject.name = "RoadPoint" + (j + 1);
			gameObject.transform.parent = roadInfo.transform;
			gameObject.layer = LayerMask.NameToLayer("Floor");
		}
		crSpline.pts[0].gameObject.name = "RoadPoint0";
		crSpline.pts[crSpline.pts.Length - 1].gameObject.name = "RoadPoint" + (AddPointNum + 1);
		GameObject gameObject2 = (GameObject)Object.Instantiate(crSpline.pts[crSpline.pts.Length - 1].gameObject);
		gameObject2.transform.parent = roadInfo.transform;
		gameObject2.gameObject.name = "RoadPoint" + (AddPointNum + 1);
		gameObject2.transform.position = crSpline.pts[crSpline.pts.Length - 1].position;
		gameObject2.transform.rotation = crSpline.pts[crSpline.pts.Length - 1].rotation;
		for (int k = 1; k < crSpline.pts.Length; k++)
		{
			Object.DestroyImmediate(crSpline.pts[k].gameObject);
		}
		crSpline.pts = null;
	}

	public void AddStritePoint(RoadInfo roadInfo)
	{
		float num = Vector3.Distance(roadInfo.roadPointList[0].transform.position, roadInfo.roadPointList[1].transform.position);
		AddPointNum = (int)num / difDistance;
		Vector3 vector = new Vector3((roadInfo.roadPointList[0].transform.position - roadInfo.roadPointList[1].transform.position).x, (roadInfo.roadPointList[0].transform.position - roadInfo.roadPointList[1].transform.position).y, (roadInfo.roadPointList[0].transform.position - roadInfo.roadPointList[1].transform.position).z);
		vector.Normalize();
		roadInfo.roadPointList[0].transform.forward = vector;
		roadInfo.roadPointList[1].transform.forward = vector;
		for (int i = 0; i < AddPointNum; i++)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(roadPointResource);
			gameObject.transform.position = roadInfo.roadPointList[0].transform.position - vector * difDistance * i;
			gameObject.transform.forward = vector;
			gameObject.name = "RoadPoint" + i;
			gameObject.transform.parent = roadInfo.transform;
			gameObject.layer = LayerMask.NameToLayer("Floor");
		}
	}

	public void GeneRateStraitPoint(RoadInfoList list)
	{
		for (int i = 0; i < list.roadInfoList.Count; i++)
		{
			if (list.roadInfoList[i].straitRoadFlag)
			{
				AddStraitSomePoint(list.roadInfoList[i]);
			}
		}
	}

	public void AddStraitSomePoint(RoadInfo roadInfo)
	{
		if (Vector3.Distance(roadInfo.roadPointList[0].transform.position, roadInfo.roadPointList[1].transform.position) > 30f)
		{
			AddStraitPointFromSide(roadInfo.roadPointList[0].transform, roadInfo.roadPointList[1].transform, 2, roadInfo);
			AddStraitPointFromSide(roadInfo.roadPointList[1].transform, roadInfo.roadPointList[0].transform, 2, roadInfo);
		}
	}

	public void AddStraitPointFromSide(Transform fromPoint, Transform toPoint, int num, RoadInfo roadInfo)
	{
		Vector3 normalized = (toPoint.position - fromPoint.position).normalized;
		for (int i = 1; i <= num; i++)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(roadPointResource);
			gameObject.transform.position = fromPoint.position + normalized * difDistance * i;
			gameObject.transform.forward = fromPoint.forward;
			gameObject.name = fromPoint.gameObject.name + i;
			gameObject.transform.parent = roadInfo.transform;
			gameObject.layer = LayerMask.NameToLayer("Floor");
		}
	}
}
