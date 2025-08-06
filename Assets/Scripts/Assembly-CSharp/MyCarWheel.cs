using System;
using UnityEngine;

public class MyCarWheel : MonoBehaviour
{
	public Transform localForward;

	public bool onTheFloor;

	public float streeAngle;

	public float motor;

	public float friction;

	public float f;

	private Vector3 relativeVelocity;

	private float speedVal;

	private void Update()
	{
		localForward.localEulerAngles = new Vector3(0f, streeAngle, 0f);
	}

	private void FixedUpdate()
	{
		relativeVelocity = base.transform.InverseTransformDirection(base.GetComponent<Rigidbody>().velocity);
		if (!onTheFloor)
		{
			return;
		}
		speedVal = base.GetComponent<Rigidbody>().velocity.magnitude - f;
		if (speedVal < 0f)
		{
			speedVal = 0f;
		}
		if (relativeVelocity.z > 0f)
		{
			base.GetComponent<Rigidbody>().velocity = speedVal * base.transform.forward;
		}
		else
		{
			base.GetComponent<Rigidbody>().velocity = speedVal * base.transform.forward * -1f;
		}
		base.transform.parent.GetComponent<Rigidbody>().AddForceAtPosition(motor * Mathf.Cos((float)Math.PI / 180f * streeAngle) * base.transform.forward, base.transform.position);
		if (!(Mathf.Abs(streeAngle) > 0.1f))
		{
			return;
		}
		if (relativeVelocity.z > 0f)
		{
			if (streeAngle > 0f)
			{
				base.transform.parent.GetComponent<Rigidbody>().AddForceAtPosition(friction * speedVal * speedVal * localForward.right, base.transform.position);
			}
			else
			{
				base.transform.parent.GetComponent<Rigidbody>().AddForceAtPosition((0f - friction) * speedVal * speedVal * localForward.right, base.transform.position);
			}
		}
		else if (streeAngle > 0f)
		{
			base.transform.parent.GetComponent<Rigidbody>().AddForceAtPosition((0f - friction) * speedVal * speedVal * localForward.right, base.transform.position);
		}
		else
		{
			base.transform.parent.GetComponent<Rigidbody>().AddForceAtPosition(friction * speedVal * speedVal * localForward.right, base.transform.position);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		onTheFloor = true;
	}

	private void OnCollisionExit(Collision other)
	{
		onTheFloor = false;
	}
}
