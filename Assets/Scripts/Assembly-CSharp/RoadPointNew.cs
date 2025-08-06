using System;
using UnityEngine;

[Serializable]
public class RoadPointNew
{
	public bool stop;

	public bool crossFlag;

	public bool dummyCrossPoint;

	public bool nsDirectionFlag;

	public bool threeCrossFlag;

	public drive_state Drive_State;

	public bool straitLineFlag;

	public RoadInfoNew roadInfo;

	public LinkPoint[] linkPoint;

	public float[] roadDistance;

	public Vector3 position;

	public Vector3 forward;

	public Vector3 right;

	public float GetRoadPointDistance(LinkPoint point)
	{
		for (int i = 0; i < linkPoint.Length; i++)
		{
			if (linkPoint[i] == point)
			{
				return roadDistance[i];
			}
		}
		return 0f;
	}

	public float GetRoadPointDistance(RoadPointNew point)
	{
		for (int i = 0; i < linkPoint.Length; i++)
		{
			if (GetLinkPoint(i) == point)
			{
				return roadDistance[i];
			}
		}
		return 0f;
	}

	public RoadPointNew GetLinkPoint(int i)
	{
		if (linkPoint[i].road != null)
		{
			return linkPoint[i].road.roadPointList[linkPoint[i].pointIndex];
		}
		return null;
	}

	public int GetPointIndex()
	{
		for (int i = 0; i < roadInfo.roadPointList.Length; i++)
		{
			if (roadInfo.roadPointList[i] == this)
			{
				return i;
			}
		}
		return -1;
	}
}
