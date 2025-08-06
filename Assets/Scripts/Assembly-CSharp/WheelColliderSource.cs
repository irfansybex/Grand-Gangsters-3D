using System;
using UnityEngine;

public class WheelColliderSource : MonoBehaviour
{
	public Transform m_dummyWheel;

	public Rigidbody m_rigidbody;

	public SphereCollider m_collider;

	public WheelFrictionCurveSource m_forwardFriction;

	public WheelFrictionCurveSource m_sidewaysFriction;

	public float m_forwardSlip;

	public float m_sidewaysSlip;

	public Vector3 m_totalForce;

	public Vector3 m_center;

	public Vector3 m_prevPosition;

	public bool m_isGrounded;

	public float m_wheelMotorTorque;

	public float m_wheelBrakeTorque;

	public float m_wheelSteerAngle;

	public float m_wheelAngularVelocity;

	public float m_wheelRotationAngle;

	public float m_wheelRadius;

	public float m_wheelMass;

	public RaycastHit m_raycastHit;

	public float m_suspensionDistance;

	public float m_suspensionCompression;

	public float m_suspensionCompressionPrev;

	public JointSpringSource m_suspensionSpring;

	public float curSuspensionDis;

	public bool useRay;

	public float distance;

	public int sidewaysFriction;

	public int forwardFriction;

	public LayerMask layer;

	public Vector3 Center
	{
		get
		{
			return m_center;
		}
		set
		{
			m_center = value;
			m_dummyWheel.localPosition = base.transform.localPosition + m_center;
			m_prevPosition = m_dummyWheel.localPosition;
		}
	}

	public float WheelRadius
	{
		get
		{
			return m_wheelRadius;
		}
		set
		{
			m_wheelRadius = value;
			m_collider.center = new Vector3(0f, m_wheelRadius, 0f);
		}
	}

	public float SuspensionDistance
	{
		get
		{
			return m_suspensionDistance;
		}
		set
		{
			m_suspensionDistance = value;
		}
	}

	public JointSpringSource SuspensionSpring
	{
		get
		{
			return m_suspensionSpring;
		}
		set
		{
			m_suspensionSpring = value;
		}
	}

	public float Mass
	{
		get
		{
			return m_wheelMass;
		}
		set
		{
			m_wheelMass = Mathf.Max(value, 0.0001f);
		}
	}

	public WheelFrictionCurveSource ForwardFriction
	{
		get
		{
			return m_forwardFriction;
		}
		set
		{
			m_forwardFriction = value;
		}
	}

	public WheelFrictionCurveSource SidewaysFriction
	{
		get
		{
			return m_sidewaysFriction;
		}
		set
		{
			m_sidewaysFriction = value;
		}
	}

	public float MotorTorque
	{
		get
		{
			return m_wheelMotorTorque;
		}
		set
		{
			m_wheelMotorTorque = value;
		}
	}

	public float BrakeTorque
	{
		get
		{
			return m_wheelBrakeTorque;
		}
		set
		{
			m_wheelBrakeTorque = value;
		}
	}

	public float SteerAngle
	{
		get
		{
			return m_wheelSteerAngle;
		}
		set
		{
			m_wheelSteerAngle = value;
		}
	}

	public bool IsGrounded
	{
		get
		{
			return m_isGrounded;
		}
	}

	public float RPM
	{
		get
		{
			return m_wheelAngularVelocity;
		}
	}

	public void Awake()
	{
		Center = Vector3.zero;
		m_suspensionDistance = 0.2f;
		m_suspensionCompression = 0f;
		Mass = 1f;
		m_suspensionSpring = default(JointSpringSource);
		m_suspensionSpring.Spring = 15000f;
		m_suspensionSpring.Damper = 1500f;
		m_suspensionSpring.TargetPosition = 0f;
	}

	public void Init(int forwardF, int sidewaysF)
	{
		m_forwardFriction = new WheelFrictionCurveSource(forwardF, forwardF - 1000);
		m_sidewaysFriction = new WheelFrictionCurveSource(sidewaysF, sidewaysF - 2000);
	}

	public void OnChangeStyle(bool flag)
	{
		if (flag)
		{
			useRay = true;
		}
		else
		{
			useRay = false;
		}
	}

	public void Start()
	{
		Transform parent = base.transform.parent;
		while (parent != null)
		{
			if (parent.gameObject.GetComponent<Rigidbody>() != null)
			{
				m_rigidbody = parent.gameObject.GetComponent<Rigidbody>();
				break;
			}
			parent = parent.parent;
		}
		if (m_rigidbody == null)
		{
			Debug.LogError("WheelColliderSource: Unable to find associated Rigidbody.");
		}
	}

	public void FixedUpdate()
	{
		UpdateSuspension();
		UpdateWheel();
		if (m_isGrounded)
		{
			CalculateSlips();
			CalculateForcesFromSlips();
			m_rigidbody.AddForceAtPosition(m_totalForce, base.transform.position);
		}
	}

	public bool GetGroundHit(out WheelHitSource wheelHit)
	{
		wheelHit = default(WheelHitSource);
		if (m_isGrounded)
		{
			wheelHit.Collider = m_raycastHit.collider;
			wheelHit.Point = m_raycastHit.point;
			wheelHit.Normal = m_raycastHit.normal;
			wheelHit.ForwardDir = m_dummyWheel.forward;
			wheelHit.SidewaysDir = m_dummyWheel.right;
			wheelHit.Force = m_totalForce;
			wheelHit.ForwardSlip = m_forwardSlip;
			wheelHit.SidewaysSlip = m_sidewaysSlip;
		}
		return m_isGrounded;
	}

	private void UpdateSuspension()
	{
		bool flag = false;
		if (useRay)
		{
			flag = Physics.Raycast(new Ray(m_dummyWheel.position, -m_dummyWheel.up), out m_raycastHit, m_wheelRadius + m_suspensionDistance, layer);
			if (!flag)
			{
				flag = Physics.Raycast(new Ray(m_dummyWheel.position + m_dummyWheel.up * 0.5f, -m_dummyWheel.up), out m_raycastHit, m_wheelRadius + m_suspensionDistance, layer);
			}
		}
		else
		{
			distance = m_dummyWheel.transform.position.y;
			if (distance < m_wheelRadius + m_suspensionDistance)
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (!m_isGrounded)
			{
				m_prevPosition = m_dummyWheel.position;
			}
			m_isGrounded = true;
			m_suspensionCompressionPrev = m_suspensionCompression;
			if (useRay)
			{
				m_suspensionCompression = m_suspensionDistance + m_wheelRadius - (m_raycastHit.point - m_dummyWheel.position).magnitude;
			}
			else
			{
				m_suspensionCompression = m_suspensionDistance + m_wheelRadius - distance;
			}
		}
		else
		{
			m_suspensionCompression = 0f;
			m_isGrounded = false;
		}
	}

	public static float ComputeDistance(Transform wheelPos, Transform floorPos)
	{
		float y = floorPos.InverseTransformPoint(wheelPos.position).y;
		float num = Vector3.Angle(floorPos.up, wheelPos.up);
		return y / Mathf.Cos(num / 180f * (float)Math.PI);
	}

	private void UpdateWheel()
	{
		m_dummyWheel.localEulerAngles = new Vector3(0f, m_wheelSteerAngle, 0f);
		if (m_wheelAngularVelocity > 720f)
		{
			m_wheelRotationAngle += 720f * Time.deltaTime;
		}
		else
		{
			m_wheelRotationAngle += m_wheelAngularVelocity * Time.deltaTime;
		}
		base.transform.localEulerAngles = new Vector3(m_wheelRotationAngle, m_wheelSteerAngle, 0f);
		float num = ((!(m_suspensionCompression > m_suspensionDistance)) ? m_suspensionCompression : m_suspensionDistance);
		curSuspensionDis = m_suspensionDistance - num;
		base.transform.localPosition = m_dummyWheel.localPosition - Vector3.up * curSuspensionDis;
		if (m_isGrounded && m_wheelMotorTorque == 0f)
		{
			m_wheelAngularVelocity -= Mathf.Sign(m_forwardSlip) * m_forwardFriction.Evaluate(m_forwardSlip) / ((float)Math.PI * 2f * m_wheelRadius) / m_wheelMass * Time.deltaTime;
		}
		m_wheelAngularVelocity += m_wheelMotorTorque / m_wheelRadius / m_wheelMass * Time.deltaTime;
		m_wheelAngularVelocity -= Mathf.Sign(m_wheelAngularVelocity) * Mathf.Min(Mathf.Abs(m_wheelAngularVelocity), m_wheelBrakeTorque * m_wheelRadius / m_wheelMass * Time.deltaTime);
	}

	private void CalculateSlips()
	{
		Vector3 lhs = (m_dummyWheel.position - m_prevPosition) / Time.deltaTime;
		m_prevPosition = m_dummyWheel.position;
		Vector3 forward = m_dummyWheel.forward;
		Vector3 vector = -m_dummyWheel.right;
		Vector3 rhs = Vector3.Dot(lhs, forward) * forward;
		Vector3 rhs2 = Vector3.Dot(lhs, vector) * vector;
		m_forwardSlip = (0f - Mathf.Sign(Vector3.Dot(forward, rhs))) * rhs.magnitude + m_wheelAngularVelocity * (float)Math.PI / 180f * m_wheelRadius;
		m_sidewaysSlip = (0f - Mathf.Sign(Vector3.Dot(vector, rhs2))) * rhs2.magnitude;
	}

	private void CalculateForcesFromSlips()
	{
		m_totalForce = m_dummyWheel.forward * Mathf.Sign(m_forwardSlip) * m_forwardFriction.Evaluate(m_forwardSlip);
		m_totalForce -= m_dummyWheel.right * Mathf.Sign(m_sidewaysSlip) * m_sidewaysFriction.Evaluate(m_sidewaysSlip) * 1.1f;
		m_totalForce += m_dummyWheel.up * (m_suspensionCompression - m_suspensionDistance * m_suspensionSpring.TargetPosition) * m_suspensionSpring.Spring;
		m_totalForce += m_dummyWheel.up * (m_suspensionCompression - m_suspensionCompressionPrev) / Time.deltaTime * m_suspensionSpring.Damper;
	}

	private void OnDrawGizmos()
	{
		float num = 0.01f;
		if (base.transform == null)
		{
			return;
		}
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Color color = Gizmos.color;
		Gizmos.color = Color.green;
		Vector3 vector = Vector3.zero;
		Vector3 from = Vector3.zero;
		for (float num2 = 0f; num2 < (float)Math.PI * 2f; num2 += num)
		{
			float y = m_wheelRadius * Mathf.Cos(num2);
			float z = m_wheelRadius * Mathf.Sin(num2);
			Vector3 vector2 = new Vector3(0f, y, z);
			if (num2 == 0f)
			{
				from = vector2;
			}
			else
			{
				Gizmos.DrawLine(vector, vector2);
			}
			vector = vector2;
		}
		Gizmos.DrawLine(from, vector);
		Gizmos.color = color;
		Gizmos.matrix = matrix;
	}
}
