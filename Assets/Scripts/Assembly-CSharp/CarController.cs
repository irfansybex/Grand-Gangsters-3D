using System;
using UnityEngine;

public class CarController : VechicleController
{
	public float maxBrakeTorque;

	public float maxTorque;

	public int preWheelForwardFriction;

	public int preWheelSidewaysFriction;

	public int wheelForwardFriction;

	public int wheelSidewaysFriction;

	public float centerOfMassY;

	public float maxBackwardSpeed;

	public bool isBraking;

	public int directionFlag;

	public float speedPercent;

	public float speedControlledMaxSteerAngle;

	public float accelerationY;

	public float camLookAngle;

	public AnimationCurve camCurve;

	public float rotateTempTime;

	public float brakeRotateTempTime;

	public float rotateTime = 1f;

	public float startAngle;

	public float brakeStartAngle;

	public AnimationCurve rotateCurve;

	public float preAngle;

	public AnimationCurve gravityCurve;

	public Transform FrontRight;

	public Transform FrontLeft;

	public Transform BackRight;

	public Transform BackLeft;

	public WheelColliderSource FrontRightWheel;

	public WheelColliderSource FrontLeftWheel;

	public WheelColliderSource BackRightWheel;

	public WheelColliderSource BackLeftWheel;

	public GameObject defaultGetOnPoint;

	public AnimationClip openDoor;

	public AnimationClip closeDoor;

	public AnimationClip robCarOpenDoor;

	public bool AIFlag;

	public bool smokeFlag;

	public GameObject tempBrokenCar;

	public FloatQueue floatQueue = new FloatQueue(64, 0.5f);

	private int countPitch;

	private float dis;

	public float countRolloverTime;

	public bool brakeAudioFlag;

	public int occlusionCount;

	private Vector3 occlusionTar;

	public override void OnOpenDoor()
	{
		AICarCtl.carInsideObj.SetActiveRecursively(true);
		base.GetComponent<Animation>().Play(openDoor.name);
	}

	public override void OnRobCarOpenDoor()
	{
		AICarCtl.carInsideObj.SetActiveRecursively(true);
		base.GetComponent<Animation>().Play(robCarOpenDoor.name);
		AICarCtl.insideNPC = InsideNPCPool.instance.GetInsideNPC();
		AICarCtl.insideNPC.gameObject.SetActiveRecursively(true);
		AICarCtl.insideNPC.transform.position = AICarCtl.insiceNPCSeatPos.transform.position;
		AICarCtl.insideNPC.transform.rotation = AICarCtl.insiceNPCSeatPos.transform.rotation;
		AICarCtl.insideNPC.GetComponent<Animation>().Play(AICarCtl.robCarNpcAnima.name);
		AICarCtl.moveFlag = false;
		Invoke("RecycleInsideNPC", 8f);
	}

	public void RecycleInsideNPC()
	{
		AICarCtl.insideNPC.gameObject.SetActiveRecursively(false);
		AICarCtl.insideNPC = null;
	}

	public override void OnCloseDoor()
	{
		base.GetComponent<Animation>().Play(closeDoor.name);
		Invoke("ResetPoint", base.GetComponent<Animation>()[closeDoor.name].length);
	}

	public override void ResetPoint()
	{
		base.GetComponent<Animation>().Stop();
		getOnPoint.transform.position = defaultGetOnPoint.transform.position;
		getOnPoint.transform.rotation = defaultGetOnPoint.transform.rotation;
		AICarCtl.carInsideObj.SetActiveRecursively(false);
	}

	private void OnEnable()
	{
		initFlag = false;
	}

	private void Awake()
	{
		m_rigidbody = base.GetComponent<Rigidbody>();
		m_transform = base.transform;
		OnDisableCar();
		HealthController healthController = carHealth;
		healthController.OnDestroy = (HealthController.onDestroy)Delegate.Combine(healthController.OnDestroy, new HealthController.onDestroy(DestroyCar));
		preWheelForwardFriction = wheelForwardFriction;
		preWheelSidewaysFriction = wheelSidewaysFriction;
		FrontLeftWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
		FrontRightWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
		BackLeftWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
		BackRightWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
		floatQueue.reset();
		AICarCtl.SetDefaultValStart();
	}

	public void DestroyCar()
	{
		tempBrokenCar = TempObjControllor.instance.GetBrokenCar(this);
		tempBrokenCar.gameObject.SetActiveRecursively(true);
		tempBrokenCar.transform.parent = base.transform;
		tempBrokenCar.transform.localPosition = Vector3.zero;
		tempBrokenCar.transform.localRotation = Quaternion.identity;
		body.SetActiveRecursively(false);
		FrontLeft.gameObject.SetActiveRecursively(false);
		BackLeft.gameObject.SetActiveRecursively(false);
		FrontRight.gameObject.SetActiveRecursively(false);
		BackRight.gameObject.SetActiveRecursively(false);
		base.gameObject.name = "Broken";
		PlayerController.instance.cam.OnChangeTarget(false);
		PlayerController.instance.GetOffCarDone();
		PlayerController.instance.healCtl.Damaged(100f);
		OnDisableCar();
		getOnPoint.transform.position = defaultGetOnPoint.transform.position;
		getOnPoint.transform.rotation = defaultGetOnPoint.transform.rotation;
		PlayerController.instance.transform.position = defaultGetOnPoint.transform.position;
		PlayerController.instance.transform.eulerAngles = new Vector3(0f, defaultGetOnPoint.transform.eulerAngles.y + 180f, 0f);
		PlayerController.instance.moveCtl.enabled = false;
		PlayerController.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
		TempObjControllor.instance.GetExplosion().transform.position = base.transform.position + Vector3.up * 0.5f;
		TempObjControllor.instance.GetExplosion().Play();
		TempObjControllor.instance.RecycleSmoke();
		AudioController.instance.play(AudioType.EXPLODE);
		carHealth.Reset();
	}

	public override void OnEnableCar()
	{
		BoxCollider boxCollider = body.GetComponent<Collider>() as BoxCollider;
		boxCollider.center = new Vector3(boxCollider.center.x, 0.65f, boxCollider.center.z);
		FrontRightWheel.enabled = true;
		FrontLeftWheel.enabled = true;
		BackRightWheel.enabled = true;
		BackLeftWheel.enabled = true;
		m_rigidbody.drag = 0f;
		m_rigidbody.angularDrag = 0.05f;
		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.angularVelocity = Vector3.zero;
		FrontRightWheel.MotorTorque = 0f;
		FrontLeftWheel.MotorTorque = 0f;
		BackRightWheel.MotorTorque = 0f;
		BackLeftWheel.MotorTorque = 0f;
		FrontRightWheel.m_wheelAngularVelocity = 0f;
		FrontLeftWheel.m_wheelAngularVelocity = 0f;
		BackRightWheel.m_wheelAngularVelocity = 0f;
		BackLeftWheel.m_wheelAngularVelocity = 0f;
		body.GetComponent<Collider>().sharedMaterial = null;
		enableFlag = true;
		initFlag = false;
		AICarCtl.motor.enabled = false;
	}

	public override void OnDisableCar()
	{
		FrontRightWheel.enabled = false;
		FrontLeftWheel.enabled = false;
		BackRightWheel.enabled = false;
		BackLeftWheel.enabled = false;
		m_rigidbody.velocity = Vector3.zero;
		m_rigidbody.angularVelocity = Vector3.zero;
		m_rigidbody.drag = 1f;
		m_rigidbody.angularDrag = 1f;
		BoxCollider boxCollider = body.GetComponent<Collider>() as BoxCollider;
		boxCollider.center = new Vector3(boxCollider.center.x, 0.4f, boxCollider.center.z);
		enableFlag = false;
	}

	private void Update()
	{
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
			FrontLeftWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
			FrontRightWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
			BackLeftWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
			BackRightWheel.Init(wheelForwardFriction, wheelSidewaysFriction);
		}
	}

	private void SetCarAudio()
	{
		AudioController.instance.audioEngine.pitch = Mathf.Lerp(AudioController.instance.audioEngine.pitch, (speedPercent + 0.5f) / 1.5f, 0.5f);
	}

	private void FixedUpdate()
	{
		if (enableFlag)
		{
			if (!initFlag)
			{
				initFlag = true;
				m_rigidbody.centerOfMass = new Vector3(0f, centerOfMassY, 0f);
			}
			else
			{
				currentSpeed = Mathf.Round(m_rigidbody.velocity.magnitude * 3.6f);
				if (m_transform.InverseTransformDirection(m_rigidbody.velocity).z < 0f)
				{
					currentSpeed *= -1f;
				}
				speedPercent = currentSpeed / maxSpeed;
				speedPercent = Mathf.Clamp01(speedPercent);
				speedControlledMaxSteerAngle = maxSteerAngle - (maxSteerAngle - maxSpeedSteerAngle) * speedPercent;
				AndroidControl();
				if (currentSpeed > 20f)
				{
					m_rigidbody.AddForce(base.transform.forward * -1f * 1000f);
				}
				dis = Mathf.Abs(currentSpeed) * Time.deltaTime / 3600f;
				GlobalInf.drivingDistance += dis;
				GlobalInf.curDistance += dis;
				GlobalInf.dailyDriveDis += dis;
			}
		}
		if (Time.frameCount % 2 == 0)
		{
			AICarCtl.SendRay();
		}
		if (base.transform.up.y < 0f)
		{
			GameUIController.instance.rolloverFlag = true;
			countRolloverTime += Time.deltaTime;
			if (countRolloverTime > 1f)
			{
				countRolloverTime = 0f;
				base.transform.position = base.transform.position + Vector3.up;
				base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
			}
		}
		else
		{
			GameUIController.instance.rolloverFlag = false;
			countRolloverTime = 0f;
		}
	}

	public void AndroidControl()
	{
		if (base.transform.eulerAngles.x > 340f || base.transform.eulerAngles.x < 30f)
		{
			if ((currentSpeed > 0f && brakeBtnPressFlag) || (currentSpeed < 0f && accBtnPressFlag) || (currentSpeed == 0f && (FrontLeftWheel.m_wheelAngularVelocity > 300f || FrontLeftWheel.m_wheelAngularVelocity < -300f)))
			{
				isBraking = true;
			}
			else
			{
				isBraking = false;
				BackRightWheel.BrakeTorque = 0f;
				BackLeftWheel.BrakeTorque = 0f;
				FrontRightWheel.BrakeTorque = 0f;
				FrontLeftWheel.BrakeTorque = 0f;
			}
		}
		else
		{
			isBraking = false;
			BackRightWheel.BrakeTorque = 0f;
			BackLeftWheel.BrakeTorque = 0f;
			FrontRightWheel.BrakeTorque = 0f;
			FrontLeftWheel.BrakeTorque = 0f;
		}
		if (!isBraking)
		{
			if (currentSpeed < maxSpeed && currentSpeed > maxBackwardSpeed * -1f)
			{
				if (accBtnPressFlag)
				{
					if (directionFlag != 0 && currentSpeed > 50f)
					{
						FrontRightWheel.MotorTorque = 0f;
						FrontLeftWheel.MotorTorque = 0f;
					}
					else
					{
						FrontRightWheel.MotorTorque = maxTorque;
						FrontLeftWheel.MotorTorque = maxTorque;
					}
				}
				else if (brakeBtnPressFlag)
				{
					FrontRightWheel.MotorTorque = maxTorque * -1f;
					FrontLeftWheel.MotorTorque = maxTorque * -1f;
				}
				else
				{
					FrontRightWheel.MotorTorque = 0f;
					FrontLeftWheel.MotorTorque = 0f;
					if (currentSpeed > 0f)
					{
						m_rigidbody.AddForce((0f - brakeForce) * m_transform.forward);
					}
				}
			}
			else
			{
				FrontRightWheel.MotorTorque = 0f;
				FrontLeftWheel.MotorTorque = 0f;
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
			brakeAudioFlag = false;
		}
		else
		{
			FrontRightWheel.BrakeTorque = maxBrakeTorque;
			FrontLeftWheel.BrakeTorque = maxBrakeTorque;
			BackRightWheel.BrakeTorque = maxBrakeTorque;
			BackLeftWheel.BrakeTorque = maxBrakeTorque;
			if (currentSpeed > 0f)
			{
				m_rigidbody.AddForce((0f - brakeForce) * m_transform.forward);
			}
			else
			{
				m_rigidbody.AddForce(brakeForce * m_transform.forward);
			}
			FrontRightWheel.MotorTorque = 0f;
			FrontLeftWheel.MotorTorque = 0f;
			if (Mathf.Abs(currentSpeed) < 5f)
			{
				FrontRightWheel.m_wheelAngularVelocity = 0f;
				FrontLeftWheel.m_wheelAngularVelocity = 0f;
			}
			else if (!brakeAudioFlag)
			{
				brakeAudioFlag = true;
				AudioController.instance.play(AudioType.BRAKE);
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
			FrontRightWheel.SteerAngle = (float)directionFlag * speedControlledMaxSteerAngle;
			FrontLeftWheel.SteerAngle = (float)directionFlag * speedControlledMaxSteerAngle;
			preAngle = Mathf.Lerp(preAngle, directionFlag, Time.deltaTime * 5f);
			RotateSelf(preAngle * Mathf.Clamp01(currentSpeed / 30f));
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
			FrontRightWheel.SteerAngle = accelerationY * speedControlledMaxSteerAngle;
			FrontLeftWheel.SteerAngle = accelerationY * speedControlledMaxSteerAngle;
			preAngle = Mathf.Lerp(preAngle, accelerationY, Time.deltaTime * 5f);
			RotateSelf(preAngle * Mathf.Clamp01(currentSpeed / 30f));
		}
	}

	public void UnityControl()
	{
		if (base.transform.eulerAngles.x > 340f || base.transform.eulerAngles.x < 30f)
		{
			if ((currentSpeed > 0f && Input.GetAxis("Vertical") < 0f) || (currentSpeed < 0f && Input.GetAxis("Vertical") > 0f) || (currentSpeed == 0f && (FrontLeftWheel.m_wheelAngularVelocity > 500f || FrontLeftWheel.m_wheelAngularVelocity < -500f)))
			{
				isBraking = true;
			}
			else
			{
				isBraking = false;
				FrontRightWheel.BrakeTorque = 0f;
				FrontLeftWheel.BrakeTorque = 0f;
				BackRightWheel.BrakeTorque = 0f;
				BackLeftWheel.BrakeTorque = 0f;
			}
		}
		else
		{
			isBraking = false;
			FrontRightWheel.BrakeTorque = 0f;
			FrontLeftWheel.BrakeTorque = 0f;
			BackRightWheel.BrakeTorque = 0f;
			BackLeftWheel.BrakeTorque = 0f;
		}
		if (!isBraking)
		{
			if (currentSpeed < maxSpeed && currentSpeed > maxBackwardSpeed * -1f)
			{
				if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.5f && currentSpeed > 50f)
				{
					FrontRightWheel.MotorTorque = 0f;
					FrontLeftWheel.MotorTorque = 0f;
				}
				else
				{
					FrontRightWheel.MotorTorque = maxTorque * Input.GetAxis("Vertical");
					FrontLeftWheel.MotorTorque = maxTorque * Input.GetAxis("Vertical");
				}
			}
			else
			{
				FrontRightWheel.MotorTorque = 0f;
				FrontLeftWheel.MotorTorque = 0f;
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
			brakeAudioFlag = false;
		}
		else
		{
			FrontRightWheel.BrakeTorque = maxBrakeTorque;
			FrontLeftWheel.BrakeTorque = maxBrakeTorque;
			BackRightWheel.BrakeTorque = maxBrakeTorque;
			BackLeftWheel.BrakeTorque = maxBrakeTorque;
			if (currentSpeed > 0f)
			{
				m_rigidbody.AddForce((0f - brakeForce) * m_transform.forward);
			}
			else
			{
				m_rigidbody.AddForce(brakeForce * m_transform.forward);
			}
			FrontRightWheel.MotorTorque = 0f;
			FrontLeftWheel.MotorTorque = 0f;
			if (Mathf.Abs(currentSpeed) < 5f)
			{
				FrontRightWheel.m_wheelAngularVelocity = 0f;
				FrontLeftWheel.m_wheelAngularVelocity = 0f;
			}
			if (!brakeAudioFlag && Mathf.Abs(currentSpeed) >= 10f)
			{
				brakeAudioFlag = true;
				AudioController.instance.play(AudioType.BRAKE);
			}
		}
		FrontRightWheel.SteerAngle = Input.GetAxis("Horizontal") * speedControlledMaxSteerAngle;
		FrontLeftWheel.SteerAngle = Input.GetAxis("Horizontal") * speedControlledMaxSteerAngle;
		preAngle = Mathf.Lerp(preAngle, Input.GetAxis("Horizontal"), Time.deltaTime * 5f);
		RotateSelf(preAngle * Mathf.Clamp01(currentSpeed / 30f));
	}

	public void AICtl(float steerAngle)
	{
		if (!isBraking)
		{
			if (currentSpeed < maxSpeed && currentSpeed > maxBackwardSpeed * -1f)
			{
				FrontRightWheel.MotorTorque = maxTorque;
				FrontLeftWheel.MotorTorque = maxTorque;
			}
			else
			{
				FrontRightWheel.MotorTorque = 0f;
				FrontLeftWheel.MotorTorque = 0f;
			}
		}
		else
		{
			FrontRightWheel.BrakeTorque = maxBrakeTorque;
			FrontLeftWheel.BrakeTorque = maxBrakeTorque;
			BackRightWheel.BrakeTorque = maxBrakeTorque;
			BackLeftWheel.BrakeTorque = maxBrakeTorque;
			if (currentSpeed > 0f)
			{
				m_rigidbody.AddForce((0f - brakeForce) * m_transform.forward);
			}
			else
			{
				m_rigidbody.AddForce(brakeForce * m_transform.forward);
				rotateTempTime = rotateTime;
			}
			FrontRightWheel.MotorTorque = 0f;
			FrontLeftWheel.MotorTorque = 0f;
		}
		FrontRightWheel.SteerAngle = steerAngle;
		FrontLeftWheel.SteerAngle = steerAngle;
	}

	public override void Damage(float val)
	{
		carHealth.Damaged(val);
		if (carHealth.healthVal < 50f && !TempObjControllor.instance.smoke.gameObject.active)
		{
			ParticleSystem smoke = TempObjControllor.instance.GetSmoke(this);
			smoke.transform.parent = base.transform;
			smoke.transform.localPosition = Vector3.zero;
			smoke.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
			smoke.Play();
		}
	}

	public void RotateSelf(float val)
	{
		body.transform.localRotation = Quaternion.Euler(0f, 0f, rotateCurve.Evaluate(val) * 4f);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (!enableFlag || AIFlag)
		{
			return;
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

	public override void ResetPlayerCar()
	{
		base.gameObject.SetActiveRecursively(true);
		if (TempObjControllor.instance.curBrokenCar == this)
		{
			TempObjControllor.instance.brokenCar.SetActiveRecursively(false);
		}
		startAngle = 0f;
		brakeStartAngle = 0f;
		AICarCtl.carInsideObj.SetActiveRecursively(false);
		carHealth.Reset();
		if (carHealth.healthVal < 50f && !TempObjControllor.instance.smoke.gameObject.active)
		{
			ParticleSystem smoke = TempObjControllor.instance.GetSmoke(this);
			smoke.transform.parent = base.transform;
			smoke.transform.localPosition = Vector3.zero;
			smoke.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
			smoke.Play();
		}
		ResetPoint();
	}

	private void OnCollisionExit(Collision other)
	{
		if (enableFlag && !AIFlag && (other.gameObject.layer == LayerMask.NameToLayer("PlayerWall") || other.gameObject.layer == LayerMask.NameToLayer("OutSideWall")))
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
		if (enableFlag && !AIFlag && (other.gameObject.layer == LayerMask.NameToLayer("PlayerWall") || other.gameObject.layer == LayerMask.NameToLayer("OutSideWall")))
		{
			PlayerController.instance.occlusion.transform.position = new Vector3(other.contacts[0].point.x, 0f, other.contacts[0].point.z);
			PlayerController.instance.occlusion.transform.right = other.contacts[0].normal;
			PlayerController.instance.occlusion.transform.eulerAngles = new Vector3(0f, PlayerController.instance.occlusion.transform.eulerAngles.y, 0f);
		}
	}

	public override void EnableCarWheelRay()
	{
		FrontLeftWheel.useRay = true;
		FrontRightWheel.useRay = true;
		BackLeftWheel.useRay = true;
		BackRightWheel.useRay = true;
	}

	public override void OnDisableWheelRay()
	{
		FrontLeftWheel.useRay = false;
		FrontRightWheel.useRay = false;
		BackLeftWheel.useRay = false;
		BackRightWheel.useRay = false;
	}

	public override void ResetWheel()
	{
		FrontLeftWheel.MotorTorque = 0f;
		FrontLeftWheel.m_wheelAngularVelocity = 0f;
		FrontRightWheel.MotorTorque = 0f;
		FrontRightWheel.m_wheelAngularVelocity = 0f;
		BackLeftWheel.MotorTorque = 0f;
		BackLeftWheel.m_wheelAngularVelocity = 0f;
		BackRightWheel.MotorTorque = 0f;
		BackRightWheel.m_wheelAngularVelocity = 0f;
	}
}
