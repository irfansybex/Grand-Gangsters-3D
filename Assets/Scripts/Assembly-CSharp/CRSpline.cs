using System;
using UnityEngine;

[Serializable]
public class CRSpline
{
	public Vector3[] pts;

	public CRSpline(params Vector3[] pts)
	{
		this.pts = new Vector3[pts.Length];
		Array.Copy(pts, this.pts, pts.Length);
	}

	public Vector3 Interp(float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 vector = pts[num2];
		Vector3 vector2 = pts[num2 + 1];
		Vector3 vector3 = pts[num2 + 2];
		Vector3 vector4 = pts[num2 + 3];
		return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
	}

	public Vector3 Velocity(float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 vector = pts[num2];
		Vector3 vector2 = pts[num2 + 1];
		Vector3 vector3 = pts[num2 + 2];
		Vector3 vector4 = pts[num2 + 3];
		return 1.5f * (-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * num3 + 0.5f * vector3 - 0.5f * vector;
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
