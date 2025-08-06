using UnityEngine;

public class PlayerMotorController : VechicleController
{
	public float maxBackwardSpeed = 20f;

	public float maxTorque = 300f;

	public float maxBrakeTorque = 7000f;

	public float speedControlledMaxSteerAngle;

	public int preWheelForwardFriction;

	public int preWheelSidewaysFriction;

	public int wheelForwardFriction = 4000;

	public int wheelSidewaysFriction = 6000;

	public Vector3 massCenterPos;

	public float speedPercent;

	public bool isBraking;

	public bool isCrash;

	public float accel;

	public WheelColliderSource frontLeftWheel;

	public WheelColliderSource frontRightWheel;

	public WheelColliderSource backLeftWheel;

	public WheelColliderSource backRightWheel;

	public Transform frontWheelTrans;

	public Transform backWheelTrans;

	public Transform frontWheelMeshDummyPos;

	public Transform backWheelMeshDummyPos;

	public MyPlayerRagdollController playerRagdollController;

	private FloatQueue floatQueue = new FloatQueue(64, 0.5f);

	public float rotateTempTime;

	public float brakeRotateTempTime;

	public float rotateTime = 1f;

	public float startAngle;

	public float brakeStartAngle;

	public int directionFlag;

	public float camLookAngle;

	public float preAngle;

	public float accelerationY;

	public AnimationCurve camCurve;

	public AnimationCurve gravityCurve;

	private int countPitch;

	public int occlusionCount;

	private void Awake()
	{
		m_rigidbody = base.GetComponent<Rigidbody>();
		m_transform = base.transform;
		Init();
		m_rigidbody.isKinematic = true;
	}

	public void Init()
	{
		SetCenterOfMass(false);
		preWheelForwardFriction = wheelForwardFriction;
		preWheelSidewaysFriction = wheelSidewaysFriction;
		frontLeftWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
		frontRightWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
		backLeftWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
		backRightWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
		floatQueue.reset();
	}

	private void Start()
	{
		m_rigidbody.angularDrag = 15f;
	}

	public void SetCenterOfMass(bool crash)
	{
		isCrash = crash;
		if (!isCrash)
		{
			massCenterPos = new Vector3(0f, -0.5f, 0f);
		}
	}

	public void UnityDrive()
	{
		currentSpeed = Mathf.Round(m_rigidbody.velocity.magnitude * 3.6f);
		if (m_transform.InverseTransformDirection(m_rigidbody.velocity).z < 0f)
		{
			currentSpeed *= -1f;
		}
		speedPercent = currentSpeed / maxSpeed;
		speedPercent = Mathf.Clamp01(speedPercent);
		speedControlledMaxSteerAngle = maxSteerAngle - (maxSteerAngle - maxSpeedSteerAngle) * speedPercent;
		if (base.transform.eulerAngles.x > 340f || base.transform.eulerAngles.x < 30f)
		{
			if ((currentSpeed > 0f && Input.GetAxis("Vertical") < 0f) || (currentSpeed < 0f && Input.GetAxis("Vertical") > 0f) || (currentSpeed == 0f && backLeftWheel.m_wheelAngularVelocity > 500f))
			{
				isBraking = true;
			}
			else
			{
				isBraking = false;
				frontLeftWheel.BrakeTorque = 0f;
				frontRightWheel.BrakeTorque = 0f;
				backLeftWheel.BrakeTorque = 0f;
				backRightWheel.BrakeTorque = 0f;
			}
		}
		else
		{
			isBraking = false;
			frontLeftWheel.BrakeTorque = 0f;
			frontRightWheel.BrakeTorque = 0f;
			backLeftWheel.BrakeTorque = 0f;
			backRightWheel.BrakeTorque = 0f;
		}
		if (!isBraking)
		{
			if (currentSpeed < maxSpeed && currentSpeed > maxBackwardSpeed * -1f)
			{
				if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5f && currentSpeed > 50f)
				{
					frontRightWheel.MotorTorque = 0f;
					frontLeftWheel.MotorTorque = 0f;
				}
				else
				{
					frontLeftWheel.MotorTorque = maxTorque * Input.GetAxis("Vertical");
					frontRightWheel.MotorTorque = maxTorque * Input.GetAxis("Vertical");
				}
			}
			else
			{
				frontLeftWheel.MotorTorque = 0f;
				frontRightWheel.MotorTorque = 0f;
			}
			if (!lockCamFlag)
			{
				if (Input.GetAxis("Vertical") > 0f)
				{
					rotateTempTime -= Time.deltaTime;
					brakeRotateTempTime = rotateTime;
					camLookAngle = Mathf.LerpAngle(0f, startAngle, camCurve.Evaluate(rotateTempTime));
					camLookTra.localRotation = Quaternion.Euler(new Vector3(base.transform.eulerAngles.x, camLookAngle, 0f));
					brakeStartAngle = camLookAngle;
				}
				else if (Input.GetAxis("Vertical") < 0f && currentSpeed <= -20f)
				{
					brakeRotateTempTime -= Time.deltaTime;
					rotateTempTime = rotateTime;
					camLookAngle = Mathf.Lerp(170f, brakeStartAngle, camCurve.Evaluate(brakeRotateTempTime));
					camLookTra.localRotation = Quaternion.Euler(new Vector3(base.transform.eulerAngles.x, camLookAngle, 0f));
					startAngle = camLookAngle;
				}
			}
			if (currentSpeed > maxSpeed)
			{
				m_rigidbody.AddForce(-20000f * m_transform.forward);
			}
			else if (currentSpeed < maxBackwardSpeed * -1f)
			{
				m_rigidbody.AddForce(20000f * m_transform.forward);
			}
		}
		else
		{
			frontLeftWheel.BrakeTorque = maxBrakeTorque;
			frontRightWheel.BrakeTorque = maxBrakeTorque;
			backRightWheel.BrakeTorque = maxBrakeTorque;
			backLeftWheel.BrakeTorque = maxBrakeTorque;
			if (currentSpeed > 0f)
			{
				m_rigidbody.AddForce((0f - brakeForce) * m_transform.forward);
			}
			else
			{
				m_rigidbody.AddForce(brakeForce * m_transform.forward);
			}
			frontLeftWheel.MotorTorque = 0f;
			frontRightWheel.MotorTorque = 0f;
			if (Mathf.Abs(currentSpeed) < 5f)
			{
				frontLeftWheel.m_wheelAngularVelocity = 0f;
				frontRightWheel.m_wheelAngularVelocity = 0f;
			}
		}
		frontLeftWheel.SteerAngle = Input.GetAxis("Horizontal") * speedControlledMaxSteerAngle;
		frontRightWheel.SteerAngle = Input.GetAxis("Horizontal") * speedControlledMaxSteerAngle;
		massCenterPos.z = (0f - accel) * 0.2f;
	}

	public void AndroidDrive()
	{
		currentSpeed = Mathf.Round(m_rigidbody.velocity.magnitude * 3.6f);
		if (m_transform.InverseTransformDirection(m_rigidbody.velocity).z < 0f)
		{
			currentSpeed *= -1f;
		}
		speedPercent = currentSpeed / maxSpeed;
		speedPercent = Mathf.Clamp01(speedPercent);
		speedControlledMaxSteerAngle = maxSteerAngle - (maxSteerAngle - maxSpeedSteerAngle) * speedPercent;
		if (base.transform.eulerAngles.x > 340f || base.transform.eulerAngles.x < 30f)
		{
			if ((currentSpeed > 0f && brakeBtnPressFlag) || (currentSpeed < 0f && accBtnPressFlag) || (currentSpeed == 0f && (frontLeftWheel.m_wheelAngularVelocity > 300f || frontLeftWheel.m_wheelAngularVelocity < -300f)))
			{
				isBraking = true;
			}
			else
			{
				isBraking = false;
				frontLeftWheel.BrakeTorque = 0f;
				frontRightWheel.BrakeTorque = 0f;
				backLeftWheel.BrakeTorque = 0f;
				backRightWheel.BrakeTorque = 0f;
			}
		}
		else
		{
			isBraking = false;
			frontLeftWheel.BrakeTorque = 0f;
			frontRightWheel.BrakeTorque = 0f;
			backLeftWheel.BrakeTorque = 0f;
			backRightWheel.BrakeTorque = 0f;
		}
		if (!isBraking)
		{
			if (currentSpeed < maxSpeed && currentSpeed > maxBackwardSpeed * -1f)
			{
				if (accBtnPressFlag)
				{
					if (directionFlag != 0 && currentSpeed > 50f)
					{
						frontRightWheel.MotorTorque = 0f;
						frontLeftWheel.MotorTorque = 0f;
					}
					else
					{
						frontRightWheel.MotorTorque = maxTorque;
						frontLeftWheel.MotorTorque = maxTorque;
					}
				}
				else if (brakeBtnPressFlag)
				{
					frontRightWheel.MotorTorque = maxTorque * -1f;
					frontLeftWheel.MotorTorque = maxTorque * -1f;
				}
				else
				{
					frontRightWheel.MotorTorque = 0f;
					frontLeftWheel.MotorTorque = 0f;
					if (currentSpeed > 0f)
					{
						m_rigidbody.AddForce((0f - brakeForce) * m_transform.forward);
					}
				}
			}
			else
			{
				frontLeftWheel.MotorTorque = 0f;
				frontRightWheel.MotorTorque = 0f;
			}
			if (currentSpeed > maxSpeed)
			{
				m_rigidbody.AddForce(-20000f * m_transform.forward);
			}
			else if (currentSpeed < maxBackwardSpeed * -1f)
			{
				m_rigidbody.AddForce(20000f * m_transform.forward);
			}
			if (accBtnPressFlag)
			{
				if (!lockCamFlag)
				{
					rotateTempTime -= Time.deltaTime;
					brakeRotateTempTime = rotateTime;
					camLookAngle = Mathf.LerpAngle(0f, startAngle, camCurve.Evaluate(rotateTempTime));
					camLookTra.localRotation = Quaternion.Euler(new Vector3(0f, camLookAngle, 0f));
					brakeStartAngle = camLookAngle;
				}
			}
			else if (brakeBtnPressFlag && !lockCamFlag && currentSpeed <= -20f)
			{
				brakeRotateTempTime -= Time.deltaTime;
				rotateTempTime = rotateTime;
				camLookAngle = Mathf.Lerp(170f, brakeStartAngle, camCurve.Evaluate(brakeRotateTempTime));
				camLookTra.localRotation = Quaternion.Euler(new Vector3(0f, camLookAngle, 0f));
				startAngle = camLookAngle;
			}
		}
		else
		{
			frontLeftWheel.BrakeTorque = maxBrakeTorque;
			frontRightWheel.BrakeTorque = maxBrakeTorque;
			backRightWheel.BrakeTorque = maxBrakeTorque;
			backLeftWheel.BrakeTorque = maxBrakeTorque;
			if (currentSpeed > 0f)
			{
				m_rigidbody.AddForce((0f - brakeForce) * m_transform.forward);
			}
			else
			{
				m_rigidbody.AddForce(brakeForce * m_transform.forward);
			}
			frontLeftWheel.MotorTorque = 0f;
			frontRightWheel.MotorTorque = 0f;
			if (Mathf.Abs(currentSpeed) < 5f)
			{
				frontLeftWheel.m_wheelAngularVelocity = 0f;
				frontRightWheel.m_wheelAngularVelocity = 0f;
			}
		}
		if (GlobalInf.carCtrlType == CARCTRLTYPE.BUTTON)
		{
			if (leftArrowPressFlag)
			{
				directionFlag = -1;
			}
			else if (rightArrowPressFlag)
			{
				directionFlag = 1;
			}
			else
			{
				directionFlag = 0;
			}
			frontRightWheel.SteerAngle = (float)directionFlag * speedControlledMaxSteerAngle;
			frontLeftWheel.SteerAngle = (float)directionFlag * speedControlledMaxSteerAngle;
			preAngle = Mathf.Lerp(preAngle, directionFlag, Time.deltaTime * 5f);
		}
		else
		{
			floatQueue.update(0f - Input.acceleration.x, Time.deltaTime);
			accelerationY = gravityCurve.Evaluate(floatQueue.smoothValue);
			if (accelerationY < 0.1f && accelerationY > -0.1f)
			{
				directionFlag = 1;
			}
			else
			{
				directionFlag = 0;
			}
			frontRightWheel.SteerAngle = accelerationY * speedControlledMaxSteerAngle;
			frontLeftWheel.SteerAngle = accelerationY * speedControlledMaxSteerAngle;
			preAngle = Mathf.Lerp(preAngle, accelerationY, Time.deltaTime * 5f);
		}
		m_rigidbody.centerOfMass = massCenterPos;
		massCenterPos.z = (0f - accel) * 0.2f;
	}

	private void FixedUpdate()
	{
		if (enableFlag)
		{
			accel = Input.GetAxis("Vertical");
			AndroidDrive();
			frontWheelTrans.transform.localEulerAngles = new Vector3(frontLeftWheel.m_wheelRotationAngle, frontLeftWheel.m_wheelSteerAngle, 0f);
			backWheelTrans.transform.localEulerAngles = new Vector3(backLeftWheel.m_wheelRotationAngle, backLeftWheel.m_wheelSteerAngle, 0f);
			frontWheelTrans.transform.localPosition = frontWheelMeshDummyPos.localPosition - Vector3.up * ((frontLeftWheel.curSuspensionDis + frontRightWheel.curSuspensionDis) / 2f);
			backWheelTrans.transform.localPosition = backWheelMeshDummyPos.localPosition - Vector3.up * ((backLeftWheel.curSuspensionDis + backRightWheel.curSuspensionDis) / 2f);
		}
	}

	public override void OnEnableCar()
	{
		base.OnEnableCar();
		BoxCollider boxCollider = body.GetComponent<Collider>() as BoxCollider;
		boxCollider.center = new Vector3(boxCollider.center.x, 0.8f, boxCollider.center.z);
		frontRightWheel.enabled = true;
		frontLeftWheel.enabled = true;
		backRightWheel.enabled = true;
		backLeftWheel.enabled = true;
		m_rigidbody.isKinematic = false;
		m_rigidbody.drag = 0f;
		m_rigidbody.angularDrag = 15f;
		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.angularVelocity = Vector3.zero;
		frontRightWheel.MotorTorque = 0f;
		frontLeftWheel.MotorTorque = 0f;
		backRightWheel.MotorTorque = 0f;
		backLeftWheel.MotorTorque = 0f;
		frontRightWheel.m_wheelAngularVelocity = 0f;
		frontLeftWheel.m_wheelAngularVelocity = 0f;
		backRightWheel.m_wheelAngularVelocity = 0f;
		backLeftWheel.m_wheelAngularVelocity = 0f;
		body.GetComponent<Collider>().sharedMaterial = null;
		enableFlag = true;
		initFlag = false;
		AICarCtl.motor.enabled = false;
		m_rigidbody.centerOfMass = new Vector3(0f, -0.5f, 0f);
	}

	private void Update()
	{
		if (!enableFlag)
		{
			return;
		}
		if (GlobalInf.soundFlag)
		{
			countPitch++;
			if (countPitch % 3 == 0)
			{
				countPitch = 0;
				SetCarAudio();
			}
		}
		if (wheelForwardFriction != preWheelForwardFriction || wheelSidewaysFriction != preWheelSidewaysFriction)
		{
			preWheelForwardFriction = wheelForwardFriction;
			preWheelSidewaysFriction = wheelSidewaysFriction;
			frontLeftWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
			frontRightWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
			backLeftWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
			backRightWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
		}
	}

	private void SetCarAudio()
	{
		AudioController.instance.audioEngine.pitch = Mathf.Lerp(AudioController.instance.audioEngine.pitch, (speedPercent + 0.5f) / 1.5f, 0.5f);
	}

	public override void OnDisableCar()
	{
		base.OnDisableCar();
		frontRightWheel.enabled = false;
		frontLeftWheel.enabled = false;
		backRightWheel.enabled = false;
		backLeftWheel.enabled = false;
		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.angularVelocity = Vector3.zero;
		m_rigidbody.drag = 1f;
		BoxCollider boxCollider = body.GetComponent<Collider>() as BoxCollider;
		boxCollider.center = new Vector3(boxCollider.center.x, 0.5f, boxCollider.center.z);
		enableFlag = false;
		m_rigidbody.isKinematic = false;
		base.enabled = false;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (!enableFlag)
		{
			return;
		}
		if ((other.gameObject.layer == LayerMask.NameToLayer("BUILDING") || other.gameObject.layer == LayerMask.NameToLayer("Car") || other.gameObject.layer == LayerMask.NameToLayer("OutSideWall") || other.gameObject.layer == LayerMask.NameToLayer("PlayerWall")) && currentSpeed > 50f)
		{
			CrashMotor();
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Car"))
		{
			if (other.relativeVelocity.sqrMagnitude > 20f && other.relativeVelocity.sqrMagnitude < 100f)
			{
				PoliceLevelCtl.ScratchPoliceCar();
			}
			else if (other.relativeVelocity.sqrMagnitude > 100f)
			{
				PoliceLevelCtl.CrashPoliceCar();
			}
			if (!AICarCtl.policeFlag)
			{
				AudioController.instance.play(AudioType.CAR_HIT_CAR);
			}
		}
		else if (other.collider.gameObject.layer == LayerMask.NameToLayer("PlayerWall") || other.gameObject.layer == LayerMask.NameToLayer("OutSideWall"))
		{
			PlayerController.instance.occlusion.SetActiveRecursively(true);
			occlusionCount++;
		}
		else if ((other.collider.gameObject.layer == LayerMask.NameToLayer("BUILDING") || other.collider.gameObject.layer == LayerMask.NameToLayer("Items")) && !AICarCtl.policeFlag)
		{
			AudioController.instance.play(AudioType.CAR_HIT_WALL);
		}
	}

	private void OnCollisionExit(Collision other)
	{
		if (enableFlag && (other.gameObject.layer == LayerMask.NameToLayer("PlayerWall") || other.gameObject.layer == LayerMask.NameToLayer("OutSideWall")))
		{
			occlusionCount--;
			if (occlusionCount == 0)
			{
				PlayerController.instance.occlusion.SetActiveRecursively(false);
				PlayerController.instance.occlusion.transform.parent = null;
			}
		}
	}

	private void OnCollisionStay(Collision other)
	{
		if (enableFlag && (other.gameObject.layer == LayerMask.NameToLayer("PlayerWall") || other.gameObject.layer == LayerMask.NameToLayer("OutSideWall")))
		{
			PlayerController.instance.occlusion.transform.position = new Vector3(other.contacts[0].point.x, 0f, other.contacts[0].point.z);
			PlayerController.instance.occlusion.transform.right = other.contacts[0].normal;
			PlayerController.instance.occlusion.transform.eulerAngles = new Vector3(0f, PlayerController.instance.occlusion.transform.eulerAngles.y, 0f);
		}
	}

	public void CrashMotor()
	{
		PlayerController.instance.CrashFromMotor();
	}

	public override void Damage(float val)
	{
		PlayerController.instance.healCtl.Damaged(val);
	}
}
