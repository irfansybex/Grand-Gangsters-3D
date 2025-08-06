using UnityEngine;

public class MoveMentMotor : MonoBehaviour
{
	public float walkingSpeed = 5f;

	public float runningSpeed = 10f;

	public float walkingSnappyness = 50f;

	public Vector3 movingDirection;

	public Vector3 targetVelocity;

	public Vector3 deltaVelocity;

	public bool isGrounded = true;

	private Rigidbody m_rigidbody;

	private void Start()
	{
		m_rigidbody = base.GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		targetVelocity = movingDirection * movingDirection.magnitude * runningSpeed;
		deltaVelocity = targetVelocity - m_rigidbody.velocity;
		if (m_rigidbody.useGravity)
		{
			deltaVelocity.y = 0f;
		}
		m_rigidbody.AddForce(deltaVelocity * walkingSnappyness, ForceMode.Acceleration);
	}
}
