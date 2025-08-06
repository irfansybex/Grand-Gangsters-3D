using System.Collections.Generic;
using UnityEngine;

public class RoadPointInfo : MonoBehaviour
{
	public bool stop;

	public bool CrossPoint;

	public bool dummyCrossPoint;

	public drive_state Drive_State;

	public LightPoint lightpointsystem;

	public RoadInfo roadInfo;

	public List<RoadPointInfo> linkPoint;

	public float[] roadDistance;

	public int GetRoadPointNum(RoadPointInfo point)
	{
		for (int i = 0; i < linkPoint.Count; i++)
		{
			if (point == linkPoint[i])
			{
				return i;
			}
		}
		return -1;
	}

	public float GetRoadPointDistance(RoadPointInfo point)
	{
		for (int i = 0; i < linkPoint.Count; i++)
		{
			if (point == linkPoint[i])
			{
				return roadDistance[i];
			}
		}
		return -1f;
	}

	public bool IsConnect(RoadPointInfo point)
	{
		for (int i = 0; i < linkPoint.Count; i++)
		{
			if (point == linkPoint[i])
			{
				return true;
			}
		}
		return false;
	}
}
