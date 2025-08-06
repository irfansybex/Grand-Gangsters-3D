using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InitSearchPathInfo : MonoBehaviour
{
	public bool runFlag;

	public InitMapInfo initMap;

	public RoadInfoList roadInfo;

	public SearchPathInfo searchPath;

	public RoadPointInfo startPoint;

	public RoadPointInfo targetPoint;

	public List<RoadPointInfo> path;

	public List<RoadPointInfo> detailPath;

	private bool nextFrameFlag;

	private void Update()
	{
		if (nextFrameFlag)
		{
			nextFrameFlag = false;
			searchPath = initMap.roadRoot.GetComponent<SearchPathInfo>();
			InitSearchPath();
		}
		if (runFlag)
		{
			runFlag = false;
			roadInfo = initMap.roadRoot.GetComponent<RoadInfoList>();
			initMap.roadRoot.AddComponent<SearchPathInfo>();
			nextFrameFlag = true;
		}
	}

	public void InitPathInfo()
	{
		searchPath.pathInfo = null;
		searchPath.pathInfo = new TwoDimIntArray[searchPath.roadPointList.Count];
		for (int i = 0; i < searchPath.roadPointList.Count; i++)
		{
			searchPath.pathInfo[i] = new TwoDimIntArray();
			searchPath.pathInfo[i].line = new int[searchPath.roadPointList.Count];
		}
		searchPath.pathDis = null;
		searchPath.pathDis = new TwoDimFloatArray[searchPath.roadPointList.Count];
		for (int j = 0; j < searchPath.roadPointList.Count; j++)
		{
			searchPath.pathDis[j] = new TwoDimFloatArray();
			searchPath.pathDis[j].line = new float[searchPath.roadPointList.Count];
		}
	}

	public void InitSearchPath()
	{
		searchPath.roadPointList.Clear();
		for (int i = 0; i < roadInfo.roadInfoList.Count; i++)
		{
			if (roadInfo.roadInfoList[i].straitRoadFlag)
			{
				searchPath.roadPointList.Add(roadInfo.roadInfoList[i].roadPointList[0]);
				searchPath.roadPointList.Add(roadInfo.roadInfoList[i].roadPointList[roadInfo.roadInfoList[i].roadPointList.Count - 1]);
			}
			else
			{
				searchPath.roadPointList.Add(roadInfo.roadInfoList[i].roadPointList[1]);
				searchPath.roadPointList.Add(roadInfo.roadInfoList[i].roadPointList[roadInfo.roadInfoList[i].roadPointList.Count - 2]);
			}
		}
		InitPathInfo();
		for (int j = 0; j < searchPath.roadPointList.Count; j++)
		{
			for (int k = 0; k < searchPath.roadPointList.Count; k++)
			{
				searchPath.pathDis[j].line[k] = float.PositiveInfinity;
			}
		}
		for (int l = 0; l < searchPath.roadPointList.Count; l++)
		{
			CrossingPoint component = searchPath.roadPointList[l].gameObject.GetComponent<CrossingPoint>();
			for (int m = 0; m < searchPath.roadPointList[l].linkPoint.Count; m++)
			{
				searchPath.pathDis[l].line[searchPath.GetRoadPointIndex(component.linkPoint[m])] = component.linkDis[m];
			}
		}
		for (int n = 0; n < searchPath.roadPointList.Count; n++)
		{
			for (int num = 0; num < searchPath.roadPointList.Count; num++)
			{
				searchPath.pathInfo[n].line[num] = num;
			}
		}
		for (int num2 = 0; num2 < searchPath.roadPointList.Count; num2++)
		{
			for (int num3 = 0; num3 < searchPath.roadPointList.Count; num3++)
			{
				for (int num4 = 0; num4 < searchPath.roadPointList.Count; num4++)
				{
					if (searchPath.pathDis[num3].line[num4] > searchPath.pathDis[num3].line[num2] + searchPath.pathDis[num2].line[num4])
					{
						searchPath.pathDis[num3].line[num4] = searchPath.pathDis[num3].line[num2] + searchPath.pathDis[num2].line[num4];
						searchPath.pathInfo[num3].line[num4] = searchPath.pathInfo[num3].line[num2];
					}
				}
			}
		}
	}

	public void GetPathFinal(RoadPointInfo start, RoadPointInfo target)
	{
		detailPath.Clear();
		if (start.roadInfo != target.roadInfo)
		{
			RoadPointInfo roadPointInfo;
			RoadPointInfo roadPointInfo2;
			if (start.roadInfo.straitRoadFlag)
			{
				roadPointInfo = start.roadInfo.roadPointList[0];
				roadPointInfo2 = start.roadInfo.roadPointList[start.roadInfo.roadPointList.Count - 1];
			}
			else
			{
				roadPointInfo = start.roadInfo.roadPointList[1];
				roadPointInfo2 = start.roadInfo.roadPointList[start.roadInfo.roadPointList.Count - 2];
			}
			int num = searchPath.roadPointList.IndexOf(roadPointInfo);
			int num2 = searchPath.roadPointList.IndexOf(roadPointInfo2);
			RoadPointInfo roadPointInfo3;
			RoadPointInfo roadPointInfo4;
			if (target.roadInfo.straitRoadFlag)
			{
				roadPointInfo3 = target.roadInfo.roadPointList[0];
				roadPointInfo4 = target.roadInfo.roadPointList[target.roadInfo.roadPointList.Count - 1];
			}
			else
			{
				roadPointInfo3 = target.roadInfo.roadPointList[1];
				roadPointInfo4 = target.roadInfo.roadPointList[target.roadInfo.roadPointList.Count - 2];
			}
			int num3 = searchPath.roadPointList.IndexOf(roadPointInfo3);
			int num4 = searchPath.roadPointList.IndexOf(roadPointInfo4);
			RoadPointInfo roadPointInfo5 = roadPointInfo;
			RoadPointInfo roadPointInfo6 = roadPointInfo3;
			float num5 = searchPath.pathDis[num].line[num3];
			float num6 = searchPath.pathDis[num].line[num4];
			if (num6 < num5)
			{
				roadPointInfo5 = roadPointInfo;
				roadPointInfo6 = roadPointInfo4;
				num5 = num6;
			}
			num6 = searchPath.pathDis[num2].line[num3];
			if (num6 < num5)
			{
				roadPointInfo5 = roadPointInfo2;
				roadPointInfo6 = roadPointInfo3;
				num5 = num6;
			}
			num6 = searchPath.pathDis[num2].line[num4];
			if (num6 < num5)
			{
				roadPointInfo5 = roadPointInfo2;
				roadPointInfo6 = roadPointInfo4;
				num5 = num6;
			}
			if (start.roadInfo.roadPointList.IndexOf(start) < start.roadInfo.roadPointList.IndexOf(roadPointInfo5))
			{
				for (int i = start.roadInfo.roadPointList.IndexOf(start); i < start.roadInfo.roadPointList.IndexOf(roadPointInfo5); i++)
				{
					detailPath.Add(start.roadInfo.roadPointList[i]);
				}
			}
			else
			{
				for (int num7 = start.roadInfo.roadPointList.IndexOf(start); num7 > start.roadInfo.roadPointList.IndexOf(roadPointInfo5); num7--)
				{
					detailPath.Add(start.roadInfo.roadPointList[num7]);
				}
			}
			GetPath(roadPointInfo5, roadPointInfo6);
			if (target.roadInfo.roadPointList.IndexOf(target) < target.roadInfo.roadPointList.IndexOf(roadPointInfo6))
			{
				for (int num8 = target.roadInfo.roadPointList.IndexOf(roadPointInfo6) - 1; num8 > target.roadInfo.roadPointList.IndexOf(target); num8--)
				{
					detailPath.Add(target.roadInfo.roadPointList[num8]);
				}
			}
			else
			{
				for (int j = target.roadInfo.roadPointList.IndexOf(roadPointInfo6) + 1; j < target.roadInfo.roadPointList.IndexOf(target); j++)
				{
					detailPath.Add(target.roadInfo.roadPointList[j]);
				}
			}
		}
		else if (start.roadInfo.roadPointList.IndexOf(start) < start.roadInfo.roadPointList.IndexOf(target))
		{
			for (int k = start.roadInfo.roadPointList.IndexOf(start); k < start.roadInfo.roadPointList.IndexOf(target) + 1; k++)
			{
				detailPath.Add(start.roadInfo.roadPointList[k]);
			}
		}
		else
		{
			for (int num9 = start.roadInfo.roadPointList.IndexOf(start); num9 > start.roadInfo.roadPointList.IndexOf(target) - 1; num9--)
			{
				detailPath.Add(start.roadInfo.roadPointList[num9]);
			}
		}
	}

	public void GetPath(RoadPointInfo start, RoadPointInfo target)
	{
		int roadPointIndex = searchPath.GetRoadPointIndex(start);
		int roadPointIndex2 = searchPath.GetRoadPointIndex(target);
		MonoBehaviour.print(roadPointIndex + " " + roadPointIndex2);
		int num = searchPath.pathInfo[roadPointIndex].line[roadPointIndex2];
		path.Clear();
		path.Add(start);
		MonoBehaviour.print("Path From " + start.gameObject.name + " to " + target.gameObject.name + " : " + start.gameObject.name);
		while (num != roadPointIndex2)
		{
			path.Add(searchPath.roadPointList[num]);
			MonoBehaviour.print(" -> " + searchPath.roadPointList[num].gameObject.name);
			num = searchPath.pathInfo[num].line[roadPointIndex2];
		}
		path.Add(target);
		MonoBehaviour.print(" -> " + target.gameObject.name);
		GetDetailPath();
	}

	public void GetDetailPath()
	{
		RoadPointInfo roadPointInfo = path[0];
		RoadPointInfo roadPointInfo2 = path[0];
		for (int i = 0; i < path.Count - 1; i++)
		{
			roadPointInfo = path[i];
			if (path[i].roadInfo != path[i + 1].roadInfo)
			{
				detailPath.Add(roadPointInfo);
				continue;
			}
			while (roadPointInfo != path[i + 1])
			{
				detailPath.Add(roadPointInfo);
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
		detailPath.Add(path[path.Count - 1]);
	}
}
