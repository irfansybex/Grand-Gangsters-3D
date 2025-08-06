using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestRacingMode : MonoBehaviour
{
	public bool runFlag;

	public List<RoadPointNew> roadPointList;

	public roadInfoListNew allRoadList;

	public GameObject source;

	public Transform pa;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			InitRoadPoint();
			MonoBehaviour.print("run");
		}
	}

	public void PutLightLabel(float dis)
	{
		List<Vector3> list = new List<Vector3>();
		float num = dis;
		for (int i = 0; i < roadPointList.Count - 1; i++)
		{
			if (!roadPointList[i].crossFlag || !roadPointList[i + 1].crossFlag)
			{
				float roadPointDistance = roadPointList[i].GetRoadPointDistance(roadPointList[i + 1]);
				MonoBehaviour.print("awayDis : " + num);
				MonoBehaviour.print("roadDis : " + roadPointDistance);
				MonoBehaviour.print(dis);
				while (num < roadPointDistance)
				{
					list.Add(GetRoadBtnPos(roadPointList[i], roadPointList[i + 1], num, Random.Range(-2, 2) * 3));
					num += dis;
					MonoBehaviour.print("while awayDis : " + num);
				}
				num -= roadPointDistance;
			}
		}
		MonoBehaviour.print(list.Count);
		for (int j = 0; j < list.Count; j++)
		{
			GameObject gameObject = Object.Instantiate(source) as GameObject;
			gameObject.transform.position = list[j];
			gameObject.transform.parent = pa;
			gameObject.name = string.Empty + j;
			MonoBehaviour.print("hhhhhhhhh");
		}
	}

	public Vector3 GetRoadBtnPos(RoadPointNew curPoint, RoadPointNew nextPoint, float forwardDis, float sideDis)
	{
		Vector3 vector = new Vector3(0f, 0f, 0f);
		if (ToolFunction.isForward(nextPoint.position - curPoint.position, curPoint.forward))
		{
			return curPoint.position + curPoint.forward * forwardDis + curPoint.right * sideDis;
		}
		return curPoint.position - curPoint.forward * forwardDis - curPoint.right * sideDis;
	}

	public void InitRoadPoint()
	{
		roadPointList.Clear();
		roadPointList.Add(allRoadList.roadList[187].roadPointList[3]);
		RoadPointNew roadPointNew = null;
		RoadPointNew roadPointNew2 = allRoadList.roadList[187].roadPointList[3];
		int num = 0;
		for (int i = 0; i < 53; i++)
		{
			if (!roadPointNew2.crossFlag)
			{
				if (roadPointNew2.GetLinkPoint(0) != roadPointNew)
				{
					roadPointList.Add(roadPointNew2.GetLinkPoint(0));
					roadPointNew = roadPointNew2;
					roadPointNew2 = roadPointNew2.GetLinkPoint(0);
				}
				else
				{
					roadPointList.Add(roadPointNew2.GetLinkPoint(1));
					roadPointNew = roadPointNew2;
					roadPointNew2 = roadPointNew2.GetLinkPoint(1);
				}
			}
			else if (!roadPointNew.crossFlag)
			{
				num++;
				if (num % 3 == 0)
				{
					roadPointNew = roadPointNew2;
					if (roadPointNew2.GetLinkPoint(1) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(1);
					}
					else if (roadPointNew2.GetLinkPoint(2) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(2);
					}
					else if (roadPointNew2.GetLinkPoint(3) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(3);
					}
				}
				else if (num % 3 == 1)
				{
					roadPointNew = roadPointNew2;
					if (roadPointNew2.GetLinkPoint(2) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(2);
					}
					else if (roadPointNew2.GetLinkPoint(3) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(3);
					}
					else if (roadPointNew2.GetLinkPoint(1) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(1);
					}
				}
				else
				{
					roadPointNew = roadPointNew2;
					if (roadPointNew2.GetLinkPoint(3) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(3);
					}
					else if (roadPointNew2.GetLinkPoint(1) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(1);
					}
					else if (roadPointNew2.GetLinkPoint(2) != null)
					{
						roadPointNew2 = roadPointNew2.GetLinkPoint(2);
					}
				}
				roadPointList.Add(roadPointNew2);
			}
			else
			{
				roadPointNew = roadPointNew2;
				roadPointNew2 = roadPointNew2.GetLinkPoint(0);
				roadPointList.Add(roadPointNew2);
			}
			if (roadPointList.Count > 2)
			{
			}
		}
		PutLightLabel(20f);
	}
}
