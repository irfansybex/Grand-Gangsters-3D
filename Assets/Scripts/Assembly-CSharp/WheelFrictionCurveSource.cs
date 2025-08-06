using UnityEngine;

public class WheelFrictionCurveSource
{
	private struct WheelFrictionCurvePoint
	{
		public float TValue;

		public Vector2 SlipForcePoint;
	}

	private float m_extremumSlip;

	private float m_extremumValue;

	private float m_asymptoteSlip;

	private float m_asymptoteValue;

	private float m_stiffness;

	private int m_arraySize;

	private WheelFrictionCurvePoint[] m_extremePoints;

	private WheelFrictionCurvePoint[] m_asymptotePoints;

	public float ExtremumSlip
	{
		get
		{
			return m_extremumSlip;
		}
		set
		{
			m_extremumSlip = value;
			UpdateArrays();
		}
	}

	public float ExtremumValue
	{
		get
		{
			return m_extremumValue;
		}
		set
		{
			m_extremumValue = value;
			UpdateArrays();
		}
	}

	public float AsymptoteSlip
	{
		get
		{
			return m_asymptoteSlip;
		}
		set
		{
			m_asymptoteSlip = value;
			UpdateArrays();
		}
	}

	public float AsymptoteValue
	{
		get
		{
			return m_asymptoteValue;
		}
		set
		{
			m_asymptoteValue = value;
			UpdateArrays();
		}
	}

	public float Stiffness
	{
		get
		{
			return m_stiffness;
		}
		set
		{
			m_stiffness = value;
		}
	}

	public WheelFrictionCurveSource()
	{
		m_extremumSlip = 3f;
		m_asymptoteSlip = 4f;
		m_extremumValue = 6000f;
		m_asymptoteValue = 5500f;
		m_stiffness = 1f;
		m_arraySize = 50;
		m_extremePoints = new WheelFrictionCurvePoint[m_arraySize];
		m_asymptotePoints = new WheelFrictionCurvePoint[m_arraySize];
		UpdateArrays();
	}

	public WheelFrictionCurveSource(int extremum, int asymptote)
	{
		m_extremumSlip = 3f;
		m_asymptoteSlip = 4f;
		m_extremumValue = extremum;
		m_asymptoteValue = asymptote;
		m_stiffness = 1f;
		m_arraySize = 50;
		m_extremePoints = new WheelFrictionCurvePoint[m_arraySize];
		m_asymptotePoints = new WheelFrictionCurvePoint[m_arraySize];
		UpdateArrays();
	}

	private void UpdateArrays()
	{
		for (int i = 0; i < m_arraySize; i++)
		{
			m_extremePoints[i].TValue = (float)i / (float)m_arraySize;
			m_extremePoints[i].SlipForcePoint = Hermite((float)i / (float)m_arraySize, Vector2.zero, new Vector2(m_extremumSlip, m_extremumValue), Vector2.zero, new Vector2(m_extremumSlip * 0.5f + 1f, 0f));
			m_asymptotePoints[i].TValue = (float)i / (float)m_arraySize;
			m_asymptotePoints[i].SlipForcePoint = Hermite((float)i / (float)m_arraySize, new Vector2(m_extremumSlip, m_extremumValue), new Vector2(m_asymptoteSlip, m_asymptoteValue), new Vector2((m_asymptoteSlip - m_extremumSlip) * 0.5f + 1f, 0f), new Vector2((m_asymptoteSlip - m_extremumSlip) * 0.5f + 1f, 0f));
		}
	}

	public float Evaluate(float slip)
	{
		slip = Mathf.Abs(slip);
		if (slip < m_extremumSlip)
		{
			return Evaluate(slip, m_extremePoints) * m_stiffness;
		}
		if (slip < m_asymptoteSlip)
		{
			return Evaluate(slip, m_asymptotePoints) * m_stiffness;
		}
		return m_asymptoteValue * m_stiffness;
	}

	private float Evaluate(float slip, WheelFrictionCurvePoint[] curvePoints)
	{
		int num = m_arraySize - 1;
		int num2 = 0;
		int num3 = (int)((float)(num + num2) * 0.5f);
		WheelFrictionCurvePoint wheelFrictionCurvePoint = curvePoints[num3];
		while (num != num2 && num - num2 > 1)
		{
			if (wheelFrictionCurvePoint.SlipForcePoint.x <= slip)
			{
				num2 = num3;
			}
			else if (wheelFrictionCurvePoint.SlipForcePoint.x >= slip)
			{
				num = num3;
			}
			num3 = (int)((float)(num + num2) * 0.5f);
			wheelFrictionCurvePoint = curvePoints[num3];
		}
		float x = curvePoints[num2].SlipForcePoint.x;
		float x2 = curvePoints[num].SlipForcePoint.x;
		float y = curvePoints[num2].SlipForcePoint.y;
		float y2 = curvePoints[num].SlipForcePoint.y;
		float num4 = (slip - x) / (x2 - x);
		return y * (1f - num4) + y2 * num4;
	}

	private Vector2 Hermite(float t, Vector2 p0, Vector2 p1, Vector2 m0, Vector2 m1)
	{
		float num = t * t;
		float num2 = num * t;
		return (2f * num2 - 3f * num + 1f) * p0 + (num2 - 2f * num + t) * m0 + (-2f * num2 + 3f * num) * p1 + (num2 - num) * m1;
	}
}
