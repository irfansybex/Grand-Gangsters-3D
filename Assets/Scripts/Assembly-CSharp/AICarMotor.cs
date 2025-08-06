using UnityEngine;

public class AICarMotor : MonoBehaviour
{
	public float speed;

	public float snappyness;

	public Vector3 movementDirection;

	public Vector3 facingDirection;

	public float turningSmoothing;

	private Vector3 targetVelocity;

	private Vector3 deltaVelocity;

	public Vector3 faceDir;

	public Rigidbody m_rigidbody;

	public Transform m_transform;

	private void Awake()
	{
		m_rigidbody = base.GetComponent<Rigidbody>();
		m_transform = base.transform;
	}

	private void FixedUpdate()
	{
		targetVelocity = movementDirection * speed;
		deltaVelocity = targetVelocity - m_rigidbody.velocity;
		if (m_rigidbody.useGravity)
		{
			deltaVelocity.y = 0f;
		}
		m_rigidbody.AddForce(deltaVelocity * snappyness, ForceMode.Acceleration);
		faceDir = movementDirection;
		if (faceDir == Vector3.zero)
		{
			if (!m_rigidbody.isKinematic)
			{
				m_rigidbody.angularVelocity = Vector3.zero;
			}
			return;
		}
		float num = AngleAroundAxis(m_transform.forward, faceDir, Vector3.up);
		if (num >= 1f)
		{
			m_rigidbody.angularVelocity = Vector3.up * Mathf.Abs(num * 2f) * Time.fixedDeltaTime;
		}
		else if (num <= -1f)
		{
			m_rigidbody.angularVelocity = Vector3.up * (0f - Mathf.Abs(num * 2f)) * Time.fixedDeltaTime;
		}
		else
		{
			m_rigidbody.angularVelocity = Vector3.zero;
		}
	}

	private float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA -= Vector3.Project(dirA, axis);
		dirB -= Vector3.Project(dirB, axis);
		float num = Vector3.Angle(dirA, dirB);
		return num * (float)((!(Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0f)) ? 1 : (-1));
	}
}
