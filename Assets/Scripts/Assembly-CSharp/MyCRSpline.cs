using System;
using UnityEngine;

[Serializable]
public class MyCRSpline
{
	public Transform[] pts;

	public MyCRSpline(params Transform[] pts)
	{
		this.pts = new Transform[pts.Length];
		Array.Copy(pts, this.pts, pts.Length);
	}

	public Vector3 Interp(float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 position = pts[num2].position;
		Vector3 position2 = pts[num2 + 1].position;
		Vector3 position3 = pts[num2 + 2].position;
		Vector3 position4 = pts[num2 + 3].position;
		return 0.5f * ((-position + 3f * position2 - 3f * position3 + position4) * (num3 * num3 * num3) + (2f * position - 5f * position2 + 4f * position3 - position4) * (num3 * num3) + (-position + position3) * num3 + 2f * position2);
	}

	public Vector3 Velocity(float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 position = pts[num2].position;
		Vector3 position2 = pts[num2 + 1].position;
		Vector3 position3 = pts[num2 + 2].position;
		Vector3 position4 = pts[num2 + 3].position;
		return 1.5f * (-position + 3f * position2 - 3f * position3 + position4) * (num3 * num3) + (2f * position - 5f * position2 + 4f * position3 - position4) * num3 + 0.5f * position3 - 0.5f * position;
	}

	public void GizmoDraw(float t)
	{
		Gizmos.color = Color.white;
		Vector3 to = Interp(0f);
		for (int i = 1; i <= 20; i++)
		{
			float t2 = (float)i / 20f;
			Vector3 vector = Interp(t2);
			Gizmos.DrawLine(vector, to);
			to = vector;
		}
		Gizmos.color = Color.blue;
		Vector3 vector2 = Interp(t);
		Gizmos.DrawLine(vector2, vector2 + Velocity(t));
	}

	public void GizmoDraw()
	{
		Gizmos.color = Color.white;
		Vector3 to = Interp(0f);
		for (int i = 1; i <= 50; i++)
		{
			float t = (float)i / 50f;
			Vector3 vector = Interp(t);
			Gizmos.DrawLine(vector, to);
			to = vector;
		}
	}
}
