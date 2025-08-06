using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InitMapDate : MonoBehaviour
{
	public bool runFlag;

	public BlockMapDate mapDate;

	public RoadInfoList rList;

	public GenerateRoadPointByDistance gRoadPoint;

	public bool addRoadInfoFlag;

	public float linkRoadDis;

	private void Update()
	{
		if (runFlag)
		{
			MonoBehaviour.print("run InitMapDate");
			runFlag = false;
		}
		if (addRoadInfoFlag)
		{
			MonoBehaviour.print("addRoadInfoFlag");
			addRoadInfoFlag = false;
			AddRoadInfo(rList);
			LinkRoadPoint(rList);
		}
	}

	public void InitMap(RoadInfoList roadList, BlockMapDate mpDate)
	{
		gRoadPoint.GeneratePoint(roadList);
		InitRoadInfo(roadList);
		InitMap(mpDate, roadList);
		ClearStraitRoadPoint(roadList);
		gRoadPoint.GeneRateStraitPoint(roadList);
		InitRoadInfo(roadList);
		AddRoadInfo(roadList);
		LinkRoadPoint(roadList);
	}

	public void InitDirection(RoadInfoList roadList)
	{
		for (int i = 0; i < roadList.roadInfoList.Count; i++)
		{
			if (roadList.roadInfoList[i].straitRoadFlag)
			{
				Vector3 normalized = (roadList.roadInfoList[i].roadPointList[0].transform.position - roadList.roadInfoList[i].roadPointList[roadList.roadInfoList[i].roadPointList.Count - 1].transform.position).normalized;
				for (int j = 0; j < roadList.roadInfoList[i].roadPointList.Count; j++)
				{
					roadList.roadInfoList[i].roadPointList[j].transform.rotation = Quaternion.identity;
					roadList.roadInfoList[i].roadPointList[j].transform.forward = normalized;
					roadList.roadInfoList[i].roadPointList[j].transform.localScale = new Vector3(1f, 1f, 1f);
				}
			}
		}
	}

	public void AddRoadInfo(RoadInfoList roadList)
	{
		for (int i = 0; i < roadList.roadInfoList.Count; i++)
		{
			for (int j = 0; j < roadList.roadInfoList[i].roadPointList.Count; j++)
			{
				if (roadList.roadInfoList[i].roadPointList[j].roadInfo == null)
				{
					roadList.roadInfoList[i].roadPointList[j].roadInfo = roadList.roadInfoList[i].roadPointList[j].transform.parent.GetComponent<RoadInfo>();
				}
			}
		}
	}

	public void ClearStraitRoadPoint(RoadInfoList roadList)
	{
		for (int i = 0; i < roadList.roadInfoList.Count; i++)
		{
			if (!roadList.roadInfoList[i].straitRoadFlag)
			{
				continue;
			}
			for (int j = 0; j < roadList.roadInfoList[i].roadPointList.Count; j++)
			{
				if (roadList.roadInfoList[i].roadPointList[j].gameObject.name.Contains("Road"))
				{
					Object.DestroyImmediate(roadList.roadInfoList[i].roadPointList[j].gameObject);
					roadList.roadInfoList[i].roadPointList.RemoveAt(j);
					j--;
				}
			}
		}
	}

	public void InitMap(BlockMapDate mDate, RoadInfoList roadList)
	{
		int num = (mDate.endX - mDate.startX) / 100;
		int num2 = (mDate.endY - mDate.startY) / 100;
		mDate.blockLine.Clear();
		for (int i = 0; i < num2; i++)
		{
			BlockLine blockLine = new BlockLine();
			blockLine.blockLine = new List<BlockDate>();
			for (int j = 0; j < num; j++)
			{
				BlockDate blockDate = new BlockDate();
				blockDate.roadList = new List<RoadInfo>();
				blockLine.blockLine.Add(blockDate);
			}
			mDate.blockLine.Add(blockLine);
		}
		InitMapBlockDate(mDate, roadList);
	}

	public void InitRoadInfo(RoadInfoList roadList)
	{
		for (int i = 0; i < roadList.roadInfoList.Count; i++)
		{
			if (!roadList.roadInfoList[i].straitRoadFlag)
			{
				roadList.roadInfoList[i].roadPointList.Clear();
				for (int j = 0; j < roadList.roadInfoList[i].transform.childCount; j++)
				{
					string text = "RoadPoint" + j;
					roadList.roadInfoList[i].roadPointList.Add(roadList.roadInfoList[i].transform.Find(text).gameObject.GetComponent<RoadPointInfo>());
				}
			}
			else
			{
				InitStraitRoadInfo(roadList.roadInfoList[i]);
			}
		}
	}

	public void InitStraitRoadInfo(RoadInfo roadInfo)
	{
		for (int i = 0; i < roadInfo.transform.childCount; i++)
		{
			RoadPointInfo component = roadInfo.transform.GetChild(i).GetComponent<RoadPointInfo>();
			if (roadInfo.roadPointList.Contains(component))
			{
				continue;
			}
			for (int j = 1; j < roadInfo.roadPointList.Count; j++)
			{
				if (Vector3.Distance(component.transform.position, roadInfo.roadPointList[0].transform.position) < Vector3.Distance(roadInfo.roadPointList[j].transform.position, roadInfo.roadPointList[0].transform.position))
				{
					roadInfo.roadPointList.Insert(j, component);
					break;
				}
			}
		}
	}

	public void InitMapBlockDate(BlockMapDate mDate, RoadInfoList roadList)
	{
		for (int i = 0; i < roadList.roadInfoList.Count; i++)
		{
			for (int j = 0; j < roadList.roadInfoList[i].roadPointList.Count; j++)
			{
				if (roadList.roadInfoList[i].straitRoadFlag || (j != 0 && j != roadList.roadInfoList[i].roadPointList.Count - 1))
				{
					int index = ((int)roadList.roadInfoList[i].roadPointList[j].transform.position.x - mDate.startX) / 100;
					int index2 = ((int)roadList.roadInfoList[i].roadPointList[j].transform.position.z - mDate.startY) / 100;
					if (!mDate.blockLine[index2].blockLine[index].roadList.Contains(roadList.roadInfoList[i]))
					{
						mDate.blockLine[index2].blockLine[index].roadList.Add(roadList.roadInfoList[i]);
					}
				}
			}
		}
	}

	public void LinkRoadPoint(RoadInfoList rList)
	{
		for (int i = 0; i < rList.roadInfoList.Count; i++)
		{
			for (int j = 0; j < rList.roadInfoList[i].roadPointList.Count; j++)
			{
				rList.roadInfoList[i].roadPointList[j].linkPoint.Clear();
				rList.roadInfoList[i].roadPointList[j].roadDistance = null;
			}
			if (!rList.roadInfoList[i].straitRoadFlag)
			{
				rList.roadInfoList[i].roadPointList[1].linkPoint.Add(rList.roadInfoList[i].roadPointList[2]);
				for (int k = 2; k < rList.roadInfoList[i].roadPointList.Count - 2; k++)
				{
					rList.roadInfoList[i].roadPointList[k].linkPoint.Add(rList.roadInfoList[i].roadPointList[k - 1]);
					rList.roadInfoList[i].roadPointList[k].linkPoint.Add(rList.roadInfoList[i].roadPointList[k + 1]);
				}
				rList.roadInfoList[i].roadPointList[rList.roadInfoList[i].roadPointList.Count - 2].linkPoint.Add(rList.roadInfoList[i].roadPointList[rList.roadInfoList[i].roadPointList.Count - 3]);
				LinkRoad(rList.roadInfoList[i].roadPointList[1], linkRoadDis, rList);
				LinkRoad(rList.roadInfoList[i].roadPointList[rList.roadInfoList[i].roadPointList.Count - 2], linkRoadDis, rList);
				for (int l = 1; l < rList.roadInfoList[i].roadPointList.Count - 1; l++)
				{
					rList.roadInfoList[i].roadPointList[l].roadDistance = new float[rList.roadInfoList[i].roadPointList[l].linkPoint.Count];
					for (int m = 0; m < rList.roadInfoList[i].roadPointList[l].linkPoint.Count; m++)
					{
						rList.roadInfoList[i].roadPointList[l].roadDistance[m] = Vector3.Distance(rList.roadInfoList[i].roadPointList[l].transform.position, rList.roadInfoList[i].roadPointList[l].linkPoint[m].transform.position);
					}
				}
				continue;
			}
			for (int n = 1; n < rList.roadInfoList[i].roadPointList.Count - 1; n++)
			{
				rList.roadInfoList[i].roadPointList[n].linkPoint.Add(rList.roadInfoList[i].roadPointList[n - 1]);
				rList.roadInfoList[i].roadPointList[n].linkPoint.Add(rList.roadInfoList[i].roadPointList[n + 1]);
			}
			rList.roadInfoList[i].roadPointList[0].linkPoint.Add(rList.roadInfoList[i].roadPointList[1]);
			rList.roadInfoList[i].roadPointList[rList.roadInfoList[i].roadPointList.Count - 1].linkPoint.Add(rList.roadInfoList[i].roadPointList[rList.roadInfoList[i].roadPointList.Count - 2]);
			LinkRoad(rList.roadInfoList[i].roadPointList[0], linkRoadDis, rList);
			LinkRoad(rList.roadInfoList[i].roadPointList[rList.roadInfoList[i].roadPointList.Count - 1], linkRoadDis, rList);
			for (int num = 0; num < rList.roadInfoList[i].roadPointList.Count; num++)
			{
				rList.roadInfoList[i].roadPointList[num].roadDistance = new float[rList.roadInfoList[i].roadPointList[num].linkPoint.Count];
				for (int num2 = 0; num2 < rList.roadInfoList[i].roadPointList[num].linkPoint.Count; num2++)
				{
					rList.roadInfoList[i].roadPointList[num].roadDistance[num2] = Vector3.Distance(rList.roadInfoList[i].roadPointList[num].transform.position, rList.roadInfoList[i].roadPointList[num].linkPoint[num2].transform.position);
				}
			}
		}
	}

	public void LinkRoad(RoadPointInfo roadPoint, float dis, RoadInfoList roadList)
	{
		for (int i = 0; i < roadList.roadInfoList.Count; i++)
		{
			if (roadList.roadInfoList[i] == roadPoint.roadInfo)
			{
				continue;
			}
			if (roadList.roadInfoList[i].straitRoadFlag)
			{
				if (Vector3.Distance(roadList.roadInfoList[i].roadPointList[0].transform.position, roadPoint.transform.position) < dis)
				{
					roadPoint.linkPoint.Add(roadList.roadInfoList[i].roadPointList[0]);
				}
				if (Vector3.Distance(roadList.roadInfoList[i].roadPointList[roadList.roadInfoList[i].roadPointList.Count - 1].transform.position, roadPoint.transform.position) < dis)
				{
					roadPoint.linkPoint.Add(roadList.roadInfoList[i].roadPointList[roadList.roadInfoList[i].roadPointList.Count - 1]);
				}
			}
			else
			{
				if (Vector3.Distance(roadList.roadInfoList[i].roadPointList[1].transform.position, roadPoint.transform.position) < dis)
				{
					roadPoint.linkPoint.Add(roadList.roadInfoList[i].roadPointList[1]);
				}
				if (Vector3.Distance(roadList.roadInfoList[i].roadPointList[roadList.roadInfoList[i].roadPointList.Count - 2].transform.position, roadPoint.transform.position) < dis)
				{
					roadPoint.linkPoint.Add(roadList.roadInfoList[i].roadPointList[roadList.roadInfoList[i].roadPointList.Count - 2]);
				}
			}
		}
	}
}
