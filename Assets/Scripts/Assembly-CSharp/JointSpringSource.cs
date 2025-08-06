using UnityEngine;

public struct JointSpringSource
{
	private float m_spring;

	private float m_damper;

	private float m_targetPosition;

	public float Spring
	{
		get
		{
			return m_spring;
		}
		set
		{
			m_spring = Mathf.Max(0f, value);
		}
	}

	public float Damper
	{
		get
		{
			return m_damper;
		}
		set
		{
			m_damper = Mathf.Max(0f, value);
		}
	}

	public float TargetPosition
	{
		get
		{
			return m_targetPosition;
		}
		set
		{
			m_targetPosition = Mathf.Clamp01(value);
		}
	}
}
