using UnityEngine;

public class AICarMovementCtl : MonoBehaviour
{
	public bool policeFlag;

	public bool moveFlag;

	public CarController carCtl;

	public Transform FrontRight;

	public Transform FrontLeft;

	public Transform BackRight;

	public Transform BackLeft;

	public RoadPointInfo CurrentTarget;

	public Vector3 target;

	public RoadPointInfo PrevTarget;

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

	public GameObject carInsideObj;

	public Material carBodyAppearMat;

	public Material carBodyNormalMat;

	public GameObject[] wheels;

	public Material wheelAppearMat;

	public Material wheelNormalMat;

	public bool appearFlag;

	public float appearCount;

	public GameObject insideNPC;

	public AnimationClip robCarNpcAnima;

	public Transform insiceNPCSeatPos;

	public AICarForwardCheck forwardCheck;

	private void Start()
	{
		countRecycleTime = 0f;
	}

	private void Update()
	{
		if (appearFlag)
		{
			appearCount += Time.deltaTime;
			carBodyAppearMat.color = new Color(1f, 1f, 1f, appearCount);
			wheelAppearMat.color = new Color(1f, 1f, 1f, appearCount);
			if (appearCount >= 0.95f)
			{
				appearCount = 0f;
				appearFlag = false;
				for (int i = 0; i < carBodyObj.Length; i++)
				{
					carBodyObj[i].GetComponent<Renderer>().material = carBodyNormalMat;
				}
				for (int j = 0; j < wheels.Length; j++)
				{
					wheels[j].GetComponent<Renderer>().sharedMaterial = wheelNormalMat;
				}
			}
		}
		countRecycleTime += Time.deltaTime;
		if (countRecycleTime >= checkRecycleTime)
		{
			countRecycleTime = 0f;
			CheckDis();
		}
		BaseMove();
	}

	public void BaseMove()
	{
		if (moveFlag)
		{
			rollWheel();
			CheckCross();
			if (!isbrake && !iscross)
			{
				MoveTo();
				ChangeTarget();
			}
			else
			{
				brakecontroll();
			}
		}
	}

	public void MoveTo()
	{
		MaxAngle = 10f / speed + 5f;
		reduce = speed / 10f;
		rotationSpeed = speed / 10f + 4f;
		Vector3 to = CurrentTarget.transform.position - PrevTarget.transform.position;
		Vector3 forward = CurrentTarget.transform.forward;
		float num = Vector3.Angle(forward, to);
		if (num < 90f)
		{
			target = CurrentTarget.transform.position + CurrentTarget.transform.right * offsetvalue;
		}
		else
		{
			target = CurrentTarget.transform.position - CurrentTarget.transform.right * offsetvalue;
		}
		base.transform.position = base.transform.position + base.transform.forward * speed * Time.deltaTime;
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.LookRotation(target - base.transform.position), rotationSpeed * Time.deltaTime);
		forward = target - base.transform.position;
		num = Vector3.Angle(forward, base.transform.forward);
		if (num > MaxAngle)
		{
			if (speed > 2f)
			{
				reduceT += Time.deltaTime;
				speed = dummyspeed - reduce * reduceT * reduceT;
			}
			else
			{
				speed = 2f;
			}
		}
		else
		{
			dummyspeed = speed;
			reduceT = 0f;
		}
		num = Vector3.Angle(forward, base.transform.forward);
		if (num < MaxAngle - 1f)
		{
			if (speed < MaxSpeed)
			{
				accT += Time.deltaTime;
				speed = dummyspeed + acc * accT * accT;
			}
			else
			{
				accT = 0f;
				speed = MaxSpeed;
			}
		}
	}

	public void ChangeTarget()
	{
		if (!(Mathf.Abs(base.transform.InverseTransformPoint(target).z) <= 2f))
		{
			return;
		}
		if (CurrentTarget.linkPoint.Count == 2)
		{
			if (PrevTarget != CurrentTarget.linkPoint[0])
			{
				PrevTarget = CurrentTarget;
				CurrentTarget = CurrentTarget.linkPoint[0];
			}
			else
			{
				PrevTarget = CurrentTarget;
				CurrentTarget = CurrentTarget.linkPoint[1];
			}
			if (offsetindex == 1 && CurrentTarget.roadInfo.arrowRoad)
			{
				offsetindex = 0;
				offsetvalue = AICarPoolController.instance.roadLane[0];
			}
		}
		else
		{
			bool flag = false;
			for (int i = 0; i < CurrentTarget.linkPoint.Count; i++)
			{
				RoadPointInfo roadPointInfo = CurrentTarget.linkPoint[i];
				RoadPointInfo currentTarget = CurrentTarget;
				if (CurrentTarget.linkPoint[i] != PrevTarget && currentTarget.roadInfo == roadPointInfo.roadInfo)
				{
					PrevTarget = CurrentTarget;
					CurrentTarget = CurrentTarget.linkPoint[i];
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				LightPoint lightpointsystem = CurrentTarget.lightpointsystem;
				int num = 0;
				for (int j = 0; j < lightpointsystem.CrossPoint.Count; j++)
				{
					if (CurrentTarget == lightpointsystem.CrossPoint[j])
					{
						num = j;
						break;
					}
				}
				if (CurrentTarget.Drive_State == drive_state.straight)
				{
					if (lightpointsystem.CrossPoint[(num + 2) % 4] != null)
					{
						PrevTarget = CurrentTarget;
						CurrentTarget = lightpointsystem.CrossPoint[(num + 2) % 4];
					}
				}
				else if (CurrentTarget.Drive_State == drive_state.turn)
				{
					if (offsetindex == 0)
					{
						if (lightpointsystem.CrossPoint[(num + 3) % 4] != null)
						{
							PrevTarget = CurrentTarget;
							CurrentTarget = lightpointsystem.CrossPoint[(num + 3) % 4];
						}
					}
					else if (offsetindex == 1 && lightpointsystem.CrossPoint[(num + 1) % 4] != null)
					{
						PrevTarget = CurrentTarget;
						CurrentTarget = lightpointsystem.CrossPoint[(num + 1) % 4];
						if (CurrentTarget.roadInfo.arrowRoad)
						{
							offsetindex = 0;
							offsetvalue = AICarPoolController.instance.roadLane[0];
						}
					}
				}
			}
		}
		dummyspeed = speed;
	}

	public void rollWheel()
	{
		if (!isbrake && !iscross)
		{
			FrontLeft.rotation *= Quaternion.Euler(WheelRotationValue, 0f, 0f);
			FrontRight.rotation *= Quaternion.Euler(WheelRotationValue, 0f, 0f);
			BackLeft.rotation *= Quaternion.Euler(WheelRotationValue, 0f, 0f);
			BackRight.rotation *= Quaternion.Euler(WheelRotationValue, 0f, 0f);
		}
	}

	public void brakecontroll()
	{
		if (speed > 0f)
		{
			speed -= brakeSpeed * Time.deltaTime;
		}
		else
		{
			speed = 0f;
			dummyspeed = 0f;
		}
		base.transform.position = base.transform.position + base.transform.forward * speed * Time.deltaTime;
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
		float sqrMagnitude = (base.transform.position - PlayerController.instance.transform.position).sqrMagnitude;
		if (!(sqrMagnitude > AICarPoolController.instance.carRecycleSqrDis))
		{
		}
	}

	public void ResetCar()
	{
		base.gameObject.SetActiveRecursively(true);
		carInsideObj.SetActiveRecursively(false);
		if (insideNPC != null)
		{
			insideNPC.SetActiveRecursively(false);
		}
	}

	public void CheckCross()
	{
		if (!CurrentTarget.dummyCrossPoint)
		{
			return;
		}
		if (CurrentTarget.stop && !PrevTarget.CrossPoint)
		{
			iscross = true;
		}
		else if (!PrevTarget.CrossPoint)
		{
			LightPoint lightpointsystem = CurrentTarget.lightpointsystem;
			int num = 0;
			for (int i = 0; i < lightpointsystem.dummyCrossPoint.Count; i++)
			{
				if (CurrentTarget == lightpointsystem.dummyCrossPoint[i])
				{
					num = i;
					break;
				}
			}
			if (lightpointsystem.CrossPoint[num].Drive_State == drive_state.straight)
			{
				if (lightpointsystem.CrossPoint[(num + 2) % 4] != null)
				{
					iscross = false;
				}
				else
				{
					iscross = true;
				}
			}
			else
			{
				if (lightpointsystem.CrossPoint[num].Drive_State != drive_state.turn)
				{
					return;
				}
				if (offsetindex == 0)
				{
					if (lightpointsystem.CrossPoint[(num + 3) % 4] != null)
					{
						iscross = false;
					}
					else
					{
						iscross = true;
					}
				}
				else if (offsetindex == 1)
				{
					if (lightpointsystem.CrossPoint[(num + 1) % 4] != null)
					{
						iscross = false;
					}
					else
					{
						iscross = true;
					}
				}
			}
		}
		else
		{
			iscross = false;
		}
	}
}
