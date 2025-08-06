using System;
using UnityEngine;

[Serializable]
public class AIWayPointsCtl
{
	public WayPoints wayPoints;

	private int index;

	public Vector3 GetPoint()
	{
		Vector3 position = wayPoints.points[index].position;
		index = (index + 1) % wayPoints.length;
		return new Vector3(position.x + UnityEngine.Random.Range(-1f, 1f), 0f, position.z + UnityEngine.Random.Range(-1f, 1f));
	}
}
