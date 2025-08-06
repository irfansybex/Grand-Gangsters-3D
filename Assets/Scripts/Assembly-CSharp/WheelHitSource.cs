using UnityEngine;

public struct WheelHitSource
{
	public Collider Collider;

	public Vector3 Point;

	public Vector3 Normal;

	public Vector3 ForwardDir;

	public Vector3 SidewaysDir;

	public Vector3 Force;

	public float ForwardSlip;

	public float SidewaysSlip;
}
