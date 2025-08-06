using System.Collections.Generic;
using UnityEngine;

public class SearchPathInfo : MonoBehaviour
{
	public static SearchPathInfo instance;

	public TwoDimIntArray[] pathInfo;

	public TwoDimFloatArray[] pathDis;

	public List<RoadPointInfo> roadPointList;

	private List<RoadPointInfo> tempPath = new List<RoadPointInfo>();

	public bool flag;

	private void Awake()
	{
		if (flag && instance == null)
		{
			instance = this;
		}
	}

	public int GetRoadPointIndex(RoadPointInfo point)
	{
		return roadPointList.IndexOf(point);
	}

	public void GetPath(List<RoadPointInfo> path, RoadPointInfo startPoint, RoadPointInfo targetPoint)
	{
		path.Clear();
		if (startPoint.roadInfo != targetPoint.roadInfo)
		{
			RoadPointInfo roadPointInfo;
			RoadPointInfo roadPointInfo2;
			if (startPoint.roadInfo.straitRoadFlag)
			{
				roadPointInfo = startPoint.roadInfo.roadPointList[0];
				roadPointInfo2 = startPoint.roadInfo.roadPointList[startPoint.roadInfo.roadPointList.Count - 1];
			}
			else
			{
				roadPointInfo = startPoint.roadInfo.roadPointList[1];
				roadPointInfo2 = startPoint.roadInfo.roadPointList[startPoint.roadInfo.roadPointList.Count - 2];
			}
			int num = roadPointList.IndexOf(roadPointInfo);
			int num2 = roadPointList.IndexOf(roadPointInfo2);
			RoadPointInfo roadPointInfo3;
			RoadPointInfo roadPointInfo4;
			if (targetPoint.roadInfo.straitRoadFlag)
			{
				roadPointInfo3 = targetPoint.roadInfo.roadPointList[0];
				roadPointInfo4 = targetPoint.roadInfo.roadPointList[targetPoint.roadInfo.roadPointList.Count - 1];
			}
			else
			{
				roadPointInfo3 = targetPoint.roadInfo.roadPointList[1];
				roadPointInfo4 = targetPoint.roadInfo.roadPointList[targetPoint.roadInfo.roadPointList.Count - 2];
			}
			int num3 = roadPointList.IndexOf(roadPointInfo3);
			int num4 = roadPointList.IndexOf(roadPointInfo4);
			RoadPointInfo roadPointInfo5 = roadPointInfo;
			RoadPointInfo roadPointInfo6 = roadPointInfo3;
			float num5 = pathDis[num].line[num3];
			float num6 = pathDis[num].line[num4];
			if (num6 < num5)
			{
				roadPointInfo5 = roadPointInfo;
				roadPointInfo6 = roadPointInfo4;
				num5 = num6;
			}
			num6 = pathDis[num2].line[num3];
			if (num6 < num5)
			{
				roadPointInfo5 = roadPointInfo2;
				roadPointInfo6 = roadPointInfo3;
				num5 = num6;
			}
			num6 = pathDis[num2].line[num4];
			if (num6 < num5)
			{
				roadPointInfo5 = roadPointInfo2;
				roadPointInfo6 = roadPointInfo4;
				num5 = num6;
			}
			if (startPoint.roadInfo.roadPointList.IndexOf(startPoint) < startPoint.roadInfo.roadPointList.IndexOf(roadPointInfo5))
			{
				for (int i = startPoint.roadInfo.roadPointList.IndexOf(startPoint); i < startPoint.roadInfo.roadPointList.IndexOf(roadPointInfo5); i++)
				{
					path.Add(startPoint.roadInfo.roadPointList[i]);
				}
			}
			else
			{
				for (int num7 = startPoint.roadInfo.roadPointList.IndexOf(startPoint); num7 > startPoint.roadInfo.roadPointList.IndexOf(roadPointInfo5); num7--)
				{
					path.Add(startPoint.roadInfo.roadPointList[num7]);
				}
			}
			GetCrossingPath(path, roadPointInfo5, roadPointInfo6);
			if (targetPoint.roadInfo.roadPointList.IndexOf(targetPoint) < targetPoint.roadInfo.roadPointList.IndexOf(roadPointInfo6))
			{
				for (int num8 = targetPoint.roadInfo.roadPointList.IndexOf(roadPointInfo6) - 1; num8 > targetPoint.roadInfo.roadPointList.IndexOf(targetPoint); num8--)
				{
					path.Add(targetPoint.roadInfo.roadPointList[num8]);
				}
			}
			else
			{
				for (int j = targetPoint.roadInfo.roadPointList.IndexOf(roadPointInfo6) + 1; j < targetPoint.roadInfo.roadPointList.IndexOf(targetPoint); j++)
				{
					path.Add(targetPoint.roadInfo.roadPointList[j]);
				}
			}
		}
		else if (startPoint.roadInfo.roadPointList.IndexOf(startPoint) < startPoint.roadInfo.roadPointList.IndexOf(targetPoint))
		{
			for (int k = startPoint.roadInfo.roadPointList.IndexOf(startPoint); k < startPoint.roadInfo.roadPointList.IndexOf(targetPoint) + 1; k++)
			{
				path.Add(startPoint.roadInfo.roadPointList[k]);
			}
		}
		else
		{
			for (int num9 = startPoint.roadInfo.roadPointList.IndexOf(startPoint); num9 > startPoint.roadInfo.roadPointList.IndexOf(targetPoint) - 1; num9--)
			{
				path.Add(startPoint.roadInfo.roadPointList[num9]);
			}
		}
	}

	private void GetCrossingPath(List<RoadPointInfo> pathList, RoadPointInfo startPoint, RoadPointInfo targetPoint)
	{
		int roadPointIndex = GetRoadPointIndex(startPoint);
		int roadPointIndex2 = GetRoadPointIndex(targetPoint);
		int num = pathInfo[roadPointIndex].line[roadPointIndex2];
		tempPath.Clear();
		tempPath.Add(startPoint);
		while (num != roadPointIndex2)
		{
			tempPath.Add(roadPointList[num]);
			num = pathInfo[num].line[roadPointIndex2];
		}
		tempPath.Add(targetPoint);
		GetDetailPath(pathList);
	}

	private void GetDetailPath(List<RoadPointInfo> pathList)
	{
		RoadPointInfo roadPointInfo = tempPath[0];
		RoadPointInfo roadPointInfo2 = tempPath[0];
		for (int i = 0; i < tempPath.Count - 1; i++)
		{
			roadPointInfo = tempPath[i];
			if (tempPath[i].roadInfo != tempPath[i + 1].roadInfo)
			{
				pathList.Add(roadPointInfo);
				continue;
			}
			while (roadPointInfo != tempPath[i + 1])
			{
				pathList.Add(roadPointInfo);
				for (int j = 0; j < roadPointInfo.linkPoint.Count; j++)
				{
					if (roadPointInfo.linkPoint[j].roadInfo == roadPointInfo.roadInfo && roadPointInfo.linkPoint[j] != roadPointInfo2)
					{
						roadPointInfo2 = roadPointInfo;
						roadPointInfo = roadPointInfo.linkPoint[j];
						break;
					}
				}
			}
		}
		pathList.Add(tempPath[tempPath.Count - 1]);
	}
}
