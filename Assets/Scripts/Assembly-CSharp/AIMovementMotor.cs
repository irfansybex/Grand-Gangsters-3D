using UnityEngine;

public class AIMovementMotor : MonoBehaviour
{
	public float speed;

	public float snappyness;

	public Vector3 movementDirection;

	private Vector3 targetVelocity;

	private Vector3 deltaVelocity;

	public Rigidbody m_rigidbody;

	private void Awake()
	{
		m_rigidbody = base.GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		targetVelocity = movementDirection * movementDirection.magnitude * speed;
		deltaVelocity = targetVelocity - m_rigidbody.velocity;
		if (m_rigidbody.useGravity)
		{
			deltaVelocity.y = 0f;
		}
		m_rigidbody.AddForce(deltaVelocity * snappyness, ForceMode.Acceleration);
		m_rigidbody.angularVelocity = Vector3.zero;
	}
}
