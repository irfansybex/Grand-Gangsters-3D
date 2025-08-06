using UnityEngine;

[ExecuteInEditMode]
public class GenerateCurveRoadPoint : MonoBehaviour
{
	public bool runFlag;

	public RoadInfoList roadList;

	public int AddPointNum;

	public MyCRSpline crSpline;

	public GameObject roadPointResource;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			MonoBehaviour.print("run GenerateCurveRoadPoint");
			GeneRateCurvePoint(roadList);
		}
	}

	public void GeneRateCurvePoint(RoadInfoList list)
	{
		for (int i = 0; i < list.roadInfoList.Count; i++)
		{
			if (!list.roadInfoList[i].straitRoadFlag)
			{
				AddCurvePoint(list.roadInfoList[i]);
			}
		}
	}

	public void AddCurvePoint(RoadInfo roadInfo)
	{
		crSpline.pts = new Transform[roadInfo.roadPointList.Count];
		for (int i = 0; i < roadInfo.roadPointList.Count; i++)
		{
			crSpline.pts[i] = roadInfo.roadPointList[i].transform;
		}
		for (int j = 0; j <= AddPointNum - 1; j++)
		{
			float t = (float)j / (float)(AddPointNum - 1);
			Vector3 position = crSpline.Interp(t);
			Vector3 forward = crSpline.Velocity(t);
			GameObject gameObject = (GameObject)Object.Instantiate(roadPointResource);
			gameObject.transform.position = position;
			gameObject.transform.forward = forward;
			gameObject.name = "RoadPoint" + (j + 1);
			gameObject.transform.parent = roadInfo.transform;
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
}
