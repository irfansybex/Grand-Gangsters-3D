using UnityEngine;

public class PoliceCar : AIsystem_script
{
	public CarController carController;

	public bool chasingFlag;

	public bool driveToPlayerFlag;

	public Vector3 moveDir;

	public float currentMaxSpeed;

	public float policeRotationSpeed;

	public float nTargetAngle;

	public bool chaseDoneFlag;

	public AIController insidePolice;

	public bool moveDoneFlag;

	public Vector3 polTarget;

	public bool blockFlag;

	public float currentMaxSteerAngle;

	public float steerAngle;

	public float hsSteerAngle;

	public float targetAngle;

	public bool attackLabelFlag;

	public float centerOfMassY;

	public bool initFlag;

	public bool leftAvoidFlag;

	public bool rightAvoidFlag;

	public float m_currentSpeed;

	public float carMaxMotor;

	public float carMaxBrake;

	public Vector3 localTarget;

	public float speedPercent;

	public float m_currentMaxSteerAngle;

	public float hasSteerAngle;

	public float m_targetAngle;

	public float currentAngle;

	public float steeringSpeed;

	public float aiSteerAngle;

	private bool frontContact;

	private float newSteerAngle;

	private float frontMinDistance;

	private float frontMaxDistance = -1f;

	private float leftDistance;

	private float rightDistance;

	public Transform leftRayPos;

	public Transform rightRayPos;

	public float rayDistance;

	public bool m_backwardDriving;

	public bool m_isBackwardDriving;

	private void Start()
	{
		countRecycleTime = 0f;
		m_transform = base.transform;
		m_rigidbody = base.GetComponent<Rigidbody>();
	}

	private void OnEnable()
	{
		initFlag = false;
		if (!attackLabelFlag)
		{
			attackLabelFlag = true;
			AttackAILabelPool.instance.AddAttackAI(base.gameObject);
		}
	}

	private void Update()
	{
		if (!initFlag)
		{
			initFlag = true;
			m_rigidbody.centerOfMass = new Vector3(0f, centerOfMassY, 0f);
		}
		countRecycleTime += Time.deltaTime;
		if (countRecycleTime >= checkRecycleTime)
		{
			countRecycleTime = 0f;
			PoliceCarCheckDis();
		}
		if (moveDoneFlag)
		{
			return;
		}
		if (!chaseDoneFlag)
		{
			if (!blockFlag)
			{
				if (chasingFlag)
				{
					Move();
				}
				else
				{
					BaseMove();
				}
				return;
			}
			if ((PlayerController.instance.transform.position - m_transform.position).sqrMagnitude < 100f)
			{
				if (PlayerController.instance.curState != PLAYERSTATE.CAR)
				{
					ChaseDone();
					return;
				}
				if (PlayerController.instance.car.currentSpeed < 5f)
				{
					ChaseDone();
					return;
				}
			}
			if ((PlayerController.instance.transform.position - m_transform.position).sqrMagnitude < 900f)
			{
				if (m_transform.InverseTransformPoint(PlayerController.instance.transform.position).z > 0f)
				{
					SetFrontWheelMotor(carMaxMotor * 3f);
				}
				else
				{
					SetFrontWheelMotor((0f - carMaxMotor) * 3f);
				}
			}
		}
		else if (carController.GetComponent<Animation>()[carController.closeDoor.name].normalizedTime >= 0.8f)
		{
			moveDoneFlag = true;
			moveFlag = false;
			insidePolice.GetComponent<Collider>().enabled = true;
			insidePolice.transform.parent = null;
			insidePolice.GetComponent<Rigidbody>().useGravity = true;
			Invoke("InvokeAI", 0.1f);
		}
		else
		{
			insidePolice.transform.localPosition = Vector3.zero;
			insidePolice.transform.localRotation = Quaternion.identity;
		}
	}

	public void PoliceCarCheckDis()
	{
		if (moveFlag)
		{
			if ((m_transform.position - PlayerController.instance.transform.position).sqrMagnitude > AICarPoolController.instance.policeRecycleSqrDis)
			{
				AICarPoolController.instance.recylecar(this);
			}
		}
		else if ((m_transform.position - PlayerController.instance.transform.position).sqrMagnitude > AICarPoolController.instance.policeRecycleSqrDis / 2f)
		{
			AICarPoolController.instance.recylecar(this);
		}
	}

	public void InvokeAI()
	{
		insidePolice.enabled = true;
		insidePolice.GetComponent<Rigidbody>().velocity = Vector3.zero;
		if (insidePolice.handGunFlag)
		{
			insidePolice.ChangeState(AISTATE.HANDGUN);
		}
		else if (insidePolice.machineGunFlag)
		{
			insidePolice.ChangeState(AISTATE.MACHINEGUN);
		}
	}

	public void ChaseDone()
	{
		if (!Physics.Raycast(leftSideRayPos.position, leftSideRayPos.forward, 2f))
		{
			chaseDoneFlag = true;
			int num = Random.Range(0, 100);
			int num2 = PoliceLevelCtl.instance.policeLevel[PoliceLevelCtl.level];
			NPCTYPE nPCTYPE;
			if (num < PoliceLevelCtl.instance.police1Rate[PoliceLevelCtl.level])
			{
				nPCTYPE = NPCTYPE.POLICE1_HG;
			}
			else
			{
				num = Random.Range(0, 100);
				nPCTYPE = ((num >= PoliceLevelCtl.instance.police2HGRate[PoliceLevelCtl.level]) ? NPCTYPE.POLICE2_MG : NPCTYPE.POLICE2_HG);
			}
			insidePolice = NPCPoolController.instance.GetPolice(nPCTYPE);
			insidePolice.type = nPCTYPE;
			insidePolice.npcLevel = num2;
			insidePolice.gunInfo.accuracy = AIInfoList.instance.aiData[(int)nPCTYPE].infoList[num2].accuracy;
			insidePolice.gunInfo.damage = AIInfoList.instance.aiData[(int)nPCTYPE].infoList[num2].attackVal;
			insidePolice.gunInfo.shotInterval = AIInfoList.instance.aiData[(int)nPCTYPE].infoList[num2].fireInterval;
			insidePolice.healthCtl.maxHealthVal = AIInfoList.instance.aiData[(int)nPCTYPE].infoList[num2].health;
			insidePolice.fallingBulletRate = AIInfoList.instance.aiData[(int)nPCTYPE].infoList[num2].bulletRate;
			insidePolice.fallingMoneyVal = AIInfoList.instance.aiData[(int)nPCTYPE].infoList[num2].fallingMoney;
			insidePolice.healthCtl.Reset();
			insidePolice.handGunFlag = AIInfoList.instance.aiData[(int)nPCTYPE].infoList[num2].handGunFlag;
			insidePolice.machineGunFlag = AIInfoList.instance.aiData[(int)nPCTYPE].infoList[num2].machineGunFlag;
			carInsideObj.gameObject.SetActiveRecursively(true);
			insidePolice.gameObject.SetActiveRecursively(true);
			insidePolice.gameObject.GetComponent<Collider>().enabled = false;
			insidePolice.GetComponent<Rigidbody>().velocity = Vector3.zero;
			insidePolice.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			insidePolice.GetComponent<Rigidbody>().useGravity = false;
			insidePolice.enabled = false;
			insidePolice.curState = insidePolice.idleState;
			insidePolice.transform.parent = carController.getOnPoint.transform;
			insidePolice.transform.localPosition = Vector3.zero;
			insidePolice.transform.localRotation = Quaternion.identity;
			insidePolice.chasingStartPos = m_transform.position;
			carController.GetComponent<Animation>().Play(carController.closeDoor.name);
			insidePolice.anima.Play("npcGetOffCar", PlayMode.StopAll);
			insidePolice.mixFlag = false;
			insidePolice.moveDirection = Vector3.zero;
			insidePolice.moveMotor.movementDirection = Vector3.zero;
			insidePolice.anima.transform.localPosition = Vector3.zero;
			insidePolice.anima.transform.localRotation = Quaternion.identity;
			insidePolice.GetComponent<Rigidbody>().velocity = Vector3.zero;
			m_rigidbody.velocity = Vector3.zero;
			m_rigidbody.angularVelocity = Vector3.zero;
			carController.OnDisableCar();
			motor.enabled = false;
			carBodyObj[0].GetComponent<Collider>().material = null;
			if (attackLabelFlag)
			{
				attackLabelFlag = false;
				AttackAILabelPool.instance.RemoveAttackAI(base.gameObject);
			}
		}
	}

	public void SetFrontWheelMotor(float motorVal)
	{
		carController.FrontLeftWheel.MotorTorque = motorVal;
		carController.FrontRightWheel.MotorTorque = motorVal;
		carController.FrontLeftWheel.BrakeTorque = 0f;
		carController.FrontRightWheel.BrakeTorque = 0f;
		carController.BackLeftWheel.BrakeTorque = 0f;
		carController.BackRightWheel.BrakeTorque = 0f;
	}

	public void SetWheelBrake(float brakeVal)
	{
		carController.FrontLeftWheel.MotorTorque = 0f;
		carController.FrontRightWheel.MotorTorque = 0f;
		carController.BackLeftWheel.MotorTorque = 0f;
		carController.BackRightWheel.MotorTorque = 0f;
		carController.FrontLeftWheel.BrakeTorque = brakeVal;
		carController.FrontRightWheel.BrakeTorque = brakeVal;
		carController.BackLeftWheel.BrakeTorque = brakeVal;
		carController.BackRightWheel.BrakeTorque = brakeVal;
	}

	public void SetWheelSteerAngle(float angle)
	{
		carController.FrontLeftWheel.SteerAngle = angle;
		carController.FrontRightWheel.SteerAngle = angle;
	}

	public void Move()
	{
		if (PlayerController.instance.curState != PLAYERSTATE.CAR)
		{
			if ((PlayerController.instance.transform.position - m_transform.position).sqrMagnitude < 100f)
			{
				ChaseDone();
				return;
			}
		}
		else if ((PlayerController.instance.transform.position - m_transform.position).sqrMagnitude < 100f && Mathf.Abs(PlayerController.instance.car.currentSpeed) < 10f)
		{
			ChaseDone();
			return;
		}
		if (Time.frameCount % 2 != 0)
		{
			if (PlayerController.instance.curState != PLAYERSTATE.CAR)
			{
				polTarget = PlayerController.instance.transform.position;
			}
			else
			{
				polTarget = PlayerController.instance.car.transform.position + PlayerController.instance.car.transform.forward * PlayerController.instance.car.currentSpeed / 10f;
			}
			m_currentSpeed = m_rigidbody.velocity.magnitude;
			currentMaxSpeed = MaxSpeed * (1f - Mathf.Clamp01(Mathf.Abs(m_targetAngle) / MaxAngle)) + 3f;
			if (m_currentSpeed > currentMaxSpeed)
			{
				isbrake = true;
			}
			else
			{
				isbrake = false;
			}
			AI();
		}
	}

	public void AI()
	{
		moveDir = polTarget - m_transform.position;
		localTarget = m_transform.InverseTransformPoint(polTarget);
		speedPercent = Mathf.Clamp01(m_currentSpeed / MaxSpeed);
		m_currentMaxSteerAngle = MaxAngle - (MaxAngle - hasSteerAngle) * speedPercent;
		m_targetAngle = ObstacleAvoidanceSteering();
		if (!frontContact)
		{
			m_targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * 57.29578f;
		}
		if (currentAngle < m_targetAngle)
		{
			currentAngle += Time.deltaTime * steeringSpeed;
			if (currentAngle > m_targetAngle)
			{
				currentAngle = m_targetAngle;
			}
		}
		else if (currentAngle > m_targetAngle)
		{
			currentAngle -= Time.deltaTime * steeringSpeed;
			if (currentAngle < m_targetAngle)
			{
				currentAngle = m_targetAngle;
			}
		}
		aiSteerAngle = Mathf.Clamp(currentAngle, -1f * m_currentMaxSteerAngle, m_currentMaxSteerAngle);
		SetWheelSteerAngle(aiSteerAngle);
	}

	public float ObstacleAvoidanceSteering()
	{
		frontContact = false;
		frontMinDistance = 0f;
		frontMaxDistance = -1f;
		leftDistance = 0f;
		rightDistance = 0f;
		if (Physics.Raycast(leftFrontRayPos.position, leftFrontRayPos.forward, out hitInfo, rayDistance, carDetectLayer))
		{
			frontContact = true;
			if (frontMinDistance == 0f || frontMinDistance > hitInfo.distance)
			{
				frontMinDistance = hitInfo.distance;
			}
			if (frontMaxDistance != -1f && frontMaxDistance < hitInfo.distance)
			{
				frontMaxDistance = hitInfo.distance;
			}
			if (m_rigidbody.velocity.sqrMagnitude > 2f && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("AI"))
			{
				tempAI = hitInfo.collider.gameObject.GetComponent<AIController>();
				if (tempAI.curState.stateName != AISTATE.DODGE)
				{
					tempAI.dodgeState.carPos = m_transform;
					tempAI.ChangeState(AISTATE.DODGE);
				}
			}
		}
		else
		{
			frontMaxDistance = -1f;
		}
		if (Physics.Raycast(rightFrontRayPos.position, rightFrontRayPos.forward, out hitInfo, rayDistance, carDetectLayer))
		{
			frontContact = true;
			if (frontMinDistance == 0f || frontMinDistance > hitInfo.distance)
			{
				frontMinDistance = hitInfo.distance;
			}
			if (frontMaxDistance != -1f && frontMaxDistance < hitInfo.distance)
			{
				frontMaxDistance = hitInfo.distance;
			}
			if (m_rigidbody.velocity.sqrMagnitude > 2f && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("AI"))
			{
				tempAI = hitInfo.collider.gameObject.GetComponent<AIController>();
				if (tempAI.curState.stateName != AISTATE.DODGE)
				{
					tempAI.dodgeState.carPos = m_transform;
					tempAI.ChangeState(AISTATE.DODGE);
				}
			}
		}
		else
		{
			frontMaxDistance = -1f;
		}
		if (Physics.Raycast(leftRayPos.position, leftRayPos.forward, out hitInfo, rayDistance, carDetectLayer))
		{
			leftDistance = hitInfo.distance;
		}
		if (Physics.Raycast(rightRayPos.position, rightRayPos.forward, out hitInfo, rayDistance, carDetectLayer))
		{
			rightDistance = hitInfo.distance;
		}
		newSteerAngle = SteeringDecision(leftDistance, rightDistance, frontMinDistance, frontContact);
		if (m_backwardDriving)
		{
			if (m_currentSpeed > 2f && !m_isBackwardDriving)
			{
				SetWheelBrake(carMaxBrake);
			}
			else
			{
				SetFrontWheelMotor(-0.5f * carMaxMotor);
				newSteerAngle = -1f * newSteerAngle;
				m_isBackwardDriving = true;
			}
			if (frontMinDistance > 8f || frontMinDistance == 0f)
			{
				m_backwardDriving = false;
			}
		}
		else
		{
			m_isBackwardDriving = false;
			if (!isbrake)
			{
				if (m_currentSpeed < currentMaxSpeed)
				{
					SetFrontWheelMotor(carMaxMotor);
				}
				else
				{
					SetFrontWheelMotor(0f);
				}
			}
			else
			{
				SetWheelBrake(carMaxBrake);
			}
		}
		return newSteerAngle;
	}

	private float SteeringDecision(float leftDistance, float rightDistance, float frontMinDistance, bool frontContact)
	{
		float num = m_currentMaxSteerAngle;
		float result = 0f;
		float num2 = 1f;
		float num3 = 1f;
		if (frontContact && frontMinDistance < 3f)
		{
			m_backwardDriving = true;
		}
		if ((leftDistance == 0f && rightDistance > 0f) || (rightDistance != 0f && leftDistance != 0f && leftDistance > rightDistance) || (leftDistance == 0f && frontMinDistance > 0f) || (rightDistance > leftDistance && frontMinDistance > 0f))
		{
			if (frontMinDistance > 0f)
			{
				result = -1f * num;
			}
			else
			{
				if (rightDistance > 0f)
				{
					num2 = rightDistance / rayDistance;
				}
				result = -1f * num * (1f - num2);
			}
		}
		if ((rightDistance == 0f && leftDistance > 0f) || (rightDistance != 0f && leftDistance != 0f && rightDistance > leftDistance) || (rightDistance == 0f && frontMinDistance > 0f) || (leftDistance > rightDistance && frontMinDistance > 0f))
		{
			if (frontMinDistance > 0f)
			{
				result = num;
			}
			else
			{
				if (leftDistance > 0f)
				{
					num3 = leftDistance / rayDistance;
				}
				result = num * (1f - num3);
			}
		}
		return result;
	}

	public void ChasingSendRay()
	{
		float num = float.PositiveInfinity;
		float num2 = float.PositiveInfinity;
		if (Physics.Raycast(leftRayPos.position, leftRayPos.forward, out hitInfo, carForwardDetectDis, carDetectLayer))
		{
			num = hitInfo.distance;
		}
		if (Physics.Raycast(rightRayPos.position, rightRayPos.forward, out hitInfo, carForwardDetectDis, carDetectLayer))
		{
			num2 = hitInfo.distance;
		}
		if (num < num2 && num < carForwardDetectDis + 1f)
		{
			leftAvoidFlag = true;
		}
		else
		{
			leftAvoidFlag = false;
		}
		if (num2 < num && num2 < carForwardDetectDis + 1f)
		{
			rightAvoidFlag = true;
		}
		else
		{
			rightAvoidFlag = false;
		}
	}

	public void ResetCarInChasingMode()
	{
		base.gameObject.SetActiveRecursively(true);
		carInsideObj.SetActiveRecursively(false);
	}

	public void ChangToChasing()
	{
	}
}
