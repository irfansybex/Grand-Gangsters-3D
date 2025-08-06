using UnityEngine;

public class VechicleController : MonoBehaviour
{
	public bool motorFlag;

	public bool targetMotorFlag;

	public CARTYPE carType;

	public float maxSpeed;

	public float brakeForce = 4000f;

	public float maxSteerAngle = 35f;

	public float maxSpeedSteerAngle = 10f;

	public GameObject body;

	public GameObject getOnPoint;

	public Transform camLookTra;

	public Transform particleRoot;

	public HealthController carHealth;

	public AIsystem_script AICarCtl;

	public Rigidbody m_rigidbody;

	public Transform m_transform;

	public bool enableFlag;

	public bool initFlag;

	public bool policeFlag;

	public float currentSpeed;

	public bool leftArrowPressFlag;

	public bool rightArrowPressFlag;

	public bool accBtnPressFlag;

	public bool brakeBtnPressFlag;

	public bool lockCamFlag;

	public virtual void OnOpenDoor()
	{
	}

	public virtual void OnRobCarOpenDoor()
	{
	}

	public virtual void OnCloseDoor()
	{
	}

	public virtual void ResetPoint()
	{
	}

	public virtual void OnEnableCar()
	{
	}

	public virtual void OnDisableCar()
	{
	}

	public virtual void Damage(float val)
	{
	}

	public virtual void ResetPlayerCar()
	{
	}

	public virtual void ResetWheel()
	{
	}

	public virtual void EnableCarWheelRay()
	{
	}

	public virtual void OnDisableWheelRay()
	{
	}
}
