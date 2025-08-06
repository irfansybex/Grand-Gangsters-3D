using System;
using UnityEngine;

[Serializable]
public class MoveAnimation
{
	public AnimationClip clip;

	public Vector3 velocity;

	public float weight;

	public float angle;

	public void Init()
	{
		velocity.y = 0f;
		angle = HorizontalAngle(velocity);
	}

	public static float HorizontalAngle(Vector3 direction)
	{
		return Mathf.Atan2(direction.x, direction.z) * 57.29578f;
	}

	public static float YAngle(Vector3 direction)
	{
		return Mathf.Atan2(direction.y, direction.z) * 57.29578f;
	}
}
