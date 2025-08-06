using UnityEngine;

public class AIsystem_script : MonoBehaviour
{
	public int CARINDEX;

	public AICarMotor motor;

	public bool policeFlag;

	public bool moveFlag;

	public VechicleController carCtl;

	public Transform FrontRight;

	public Transform FrontLeft;

	public Transform BackRight;

	public Transform BackLeft;

	public RoadPointNew CurrentTarget;

	public Vector3 target;

	public RoadPointNew PrevTarget;

	public float speed;

	public float MaxSpeed;

	public float MaxAngle;

	public float dummyspeed;

	public float acc;

	public float accT;

	public float reduce;

	public float reduceT;

	public float brakeSpeed;

	public float rotationSpeed;

	public float WheelRotationValue;

	public float[] offset;

	public int offsetindex;

	public float offsetvalue;

	public bool isbrake;

	public bool iscrash;

	public bool iscross;

	protected bool ischangelane;

	public Transform m_player;

	public float checkRecycleTime;

	public float countRecycleTime;

	public GameObject[] carBodyObj;

	public GameObject topCollider;

	public GameObject carInsideObj;

	public GameObject insideNPC;

	public AnimationClip robCarNpcAnima;

	public Transform insiceNPCSeatPos;

	public Vector3 moveDirection;

	public float turnSpeed;

	public Transform leftSideRayPos;

	public Transform m_transform;

	public Rigidbody m_rigidbody;

	public bool fff;

	public float countRayTime;

	public float rayTime;

	public Transform leftFrontRayPos;

	public Transform rightFrontRayPos;

	public Transform midFrontRayPos;

	public bool obstacleFlag;

	public LayerMask carDetectLayer;

	public float carForwardDetectDis;

	public RaycastHit hitInfo;

	public AIController tempAI;

	private bool preFlag;

	public float inputSteer;

	public float deltaAngle;

	public float countTunTime;

	public float speedP;

	private void Start()
	{
		countRecycleTime = 0f;
		m_transform = base.transform;
		m_rigidbody = base.GetComponent<Rigidbody>();
	}

	public void SetDefaultValStart()
	{
		m_transform = base.transform;
		m_rigidbody = base.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		countRecycleTime += Time.deltaTime;
		if (countRecycleTime >= checkRecycleTime)
		{
			countRecycleTime = 0f;
			CheckDis();
		}
		BaseMove();
	}

	public void SendRay()
	{
		obstacleFlag = false;
		if (Physics.Raycast(leftFrontRayPos.position, leftFrontRayPos.forward, out hitInfo, carForwardDetectDis, carDetectLayer))
		{
			if (!preFlag && moveFlag && (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("PlayerCar") || hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player")))
			{
				AudioController.instance.play(AudioType.CARALARM);
			}
			obstacleFlag = true;
			if (m_rigidbody.velocity.sqrMagnitude >= 2f && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("AI") && hitInfo.collider.gameObject.transform.GetChild(0).InverseTransformPoint(base.transform.position).z > 0f)
			{
				tempAI = hitInfo.collider.gameObject.GetComponent<AIController>();
				if (tempAI.curState.stateName != AISTATE.DODGE)
				{
					tempAI.dodgeState.carPos = m_transform;
					tempAI.ChangeState(AISTATE.DODGE);
					if (carCtl.enabled)
					{
						if (tempAI.type == NPCTYPE.NORMALWOMEN || tempAI.type == NPCTYPE.NORMALWOMEN2)
						{
							AudioController.instance.play(AudioType.WOMENSCREAM);
						}
						else
						{
							AudioController.instance.play(AudioType.MANSCREAM);
						}
					}
				}
			}
		}
		if (Physics.Raycast(rightFrontRayPos.position, rightFrontRayPos.forward, out hitInfo, carForwardDetectDis, carDetectLayer))
		{
			if (!preFlag && moveFlag && (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("PlayerCar") || hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player")))
			{
				AudioController.instance.play(AudioType.CARALARM);
			}
			obstacleFlag = true;
			if (m_rigidbody.velocity.sqrMagnitude >= 2f && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("AI") && hitInfo.collider.gameObject.transform.GetChild(0).InverseTransformPoint(base.transform.position).z > 0f)
			{
				tempAI = hitInfo.collider.gameObject.GetComponent<AIController>();
				if (tempAI.curState.stateName != AISTATE.DODGE)
				{
					tempAI.dodgeState.carPos = m_transform;
					tempAI.ChangeState(AISTATE.DODGE);
					if (carCtl.enabled)
					{
						if (tempAI.type == NPCTYPE.NORMALWOMEN || tempAI.type == NPCTYPE.NORMALWOMEN2)
						{
							AudioController.instance.play(AudioType.WOMENSCREAM);
						}
						else
						{
							AudioController.instance.play(AudioType.MANSCREAM);
						}
					}
				}
			}
		}
		preFlag = obstacleFlag;
	}

	public void BaseMove()
	{
		if (moveFlag)
		{
			countRayTime += Time.deltaTime;
			if (countRayTime > rayTime)
			{
				countRayTime = 0f;
				SendRay();
			}
			rollWheel();
			CheckCross();
			if (!obstacleFlag)
			{
				MoveTo();
				ChangeTarget();
			}
			else
			{
				brakecontroll();
			}
			motor.movementDirection = moveDirection;
			motor.speed = speed;
		}
	}

	public void MoveTo()
	{
		moveDirection = (target - m_transform.position).normalized;
		if (speed < MaxSpeed)
		{
			speed += Time.deltaTime;
		}
	}

	public void ChangeTarget()
	{
		Vector3 vector = m_transform.InverseTransformPoint(target);
		if (!(Mathf.Abs(vector.z) <= 2.5f) || !(Mathf.Abs(vector.x) < 20f))
		{
			return;
		}
		if (!CurrentTarget.crossFlag)
		{
			if (PrevTarget != CurrentTarget.GetLinkPoint(0))
			{
				PrevTarget = CurrentTarget;
				CurrentTarget = CurrentTarget.GetLinkPoint(0);
			}
			else
			{
				PrevTarget = CurrentTarget;
				CurrentTarget = CurrentTarget.GetLinkPoint(1);
			}
			if (offsetindex == 1 && CurrentTarget.roadInfo.arrowRoad)
			{
				offsetindex = 0;
				offsetvalue = AICarPoolController.instance.roadLane[0];
			}
		}
		else if (PrevTarget.crossFlag)
		{
			for (int i = 0; i < CurrentTarget.linkPoint.Length; i++)
			{
				if (CurrentTarget.GetLinkPoint(i) != null && CurrentTarget.GetLinkPoint(i).roadInfo == CurrentTarget.roadInfo)
				{
					PrevTarget = CurrentTarget;
					CurrentTarget = CurrentTarget.GetLinkPoint(i);
					break;
				}
			}
		}
		else
		{
			PrevTarget = CurrentTarget;
			if (!CurrentTarget.threeCrossFlag)
			{
				if (LightSystemNew.instance.straitFlag)
				{
					if (CurrentTarget.GetLinkPoint(1) != null)
					{
						CurrentTarget = CurrentTarget.GetLinkPoint(1);
					}
				}
				else if (offsetindex == 0)
				{
					if (CurrentTarget.GetLinkPoint(2) != null)
					{
						CurrentTarget = CurrentTarget.GetLinkPoint(2);
					}
				}
				else if (CurrentTarget.GetLinkPoint(3) != null)
				{
					CurrentTarget = CurrentTarget.GetLinkPoint(3);
				}
			}
			else if (CurrentTarget.GetLinkPoint(1) != null)
			{
				CurrentTarget = CurrentTarget.GetLinkPoint(1);
			}
			else if (offsetindex == 0)
			{
				if (CurrentTarget.GetLinkPoint(2) != null)
				{
					CurrentTarget = CurrentTarget.GetLinkPoint(2);
				}
			}
			else if (CurrentTarget.GetLinkPoint(3) != null)
			{
				CurrentTarget = CurrentTarget.GetLinkPoint(3);
			}
		}
		dummyspeed = speed;
		SetTarget();
	}

	public void SetTarget()
	{
		if (ToolFunction.isForward(base.transform.position - CurrentTarget.position, CurrentTarget.forward))
		{
			target = CurrentTarget.position - CurrentTarget.right * offsetvalue;
		}
		else
		{
			target = CurrentTarget.position + CurrentTarget.right * offsetvalue;
		}
	}

	private float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA -= Vector3.Project(dirA, axis);
		dirB -= Vector3.Project(dirB, axis);
		float num = Vector3.Angle(dirA, dirB);
		return num * (float)((!(Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0f)) ? 1 : (-1));
	}

	public void rollWheel()
	{
		speedP = speed / MaxSpeed;
		FrontLeft.Rotate(Vector3.right * 720f * speedP * Time.deltaTime, Space.Self);
		BackLeft.Rotate(Vector3.right * 720f * speedP * Time.deltaTime, Space.Self);
		FrontRight.Rotate(Vector3.right * 720f * speedP * Time.deltaTime, Space.Self);
		BackRight.Rotate(Vector3.right * 720f * speedP * Time.deltaTime, Space.Self);
	}

	public void brakecontroll()
	{
		if (speed > 0f)
		{
			speed -= brakeSpeed * Time.deltaTime;
			return;
		}
		speed = 0f;
		dummyspeed = 0f;
	}

	public void changelane()
	{
		if (ischangelane)
		{
			if (offsetindex == 0)
			{
				offsetindex = 1;
			}
			else
			{
				offsetindex = 0;
			}
			ischangelane = false;
		}
		offsetvalue = Mathf.Lerp(offsetvalue, offset[offsetindex], Time.deltaTime * 4f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo("PlayerAI") == 0)
		{
			ischangelane = true;
		}
	}

	public void CheckDis()
	{
		float sqrMagnitude = (m_transform.position - PlayerController.instance.transform.position).sqrMagnitude;
		if (sqrMagnitude > AICarPoolController.instance.carRecycleSqrDis)
		{
			AICarPoolController.instance.recylecar(this);
		}
	}

	public void ResetCar()
	{
		base.gameObject.SetActiveRecursively(true);
		if (TempObjControllor.instance.curBrokenCar == carCtl)
		{
			TempObjControllor.instance.brokenCar.SetActiveRecursively(false);
		}
		carInsideObj.SetActiveRecursively(false);
		carCtl.initFlag = false;
		base.GetComponent<Rigidbody>().drag = 0f;
		base.GetComponent<Rigidbody>().angularDrag = 0.1f;
	}

	public void CheckCross()
	{
		if (!CurrentTarget.crossFlag || PrevTarget.crossFlag)
		{
			return;
		}
		if (CurrentTarget.nsDirectionFlag)
		{
			if (!LightSystemNew.instance.nsPassFlag)
			{
				obstacleFlag = true;
			}
		}
		else if (!LightSystemNew.instance.ewPassFlag)
		{
			obstacleFlag = true;
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (moveFlag && (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("PlayerCar")))
		{
			obstacleFlag = true;
			AudioController.instance.play(AudioType.CARALARM);
		}
	}
}
