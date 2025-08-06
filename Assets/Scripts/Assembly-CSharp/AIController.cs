using System;
using UnityEngine;

public class AIController : MonoBehaviour
{
	public bool policeFlag;

	public bool handGunFlag;

	public bool machineGunFlag;

	public bool fightBackFlag;

	public NPCTYPE type;

	public int npcLevel;

	public STATEInstance curState;

	public STATEInstance preState;

	public WalkState walkState;

	public IdleState idleState;

	public ShottedState shottedState;

	public DieState dieState;

	public RunAwayState runawayState;

	public AfraidState afraidState;

	public PunchedState punchedState;

	public FightReadyState fightReadyState;

	public PunchState punchLeftState;

	public PunchState punchRightState;

	public MachineGunState machineGunState;

	public DodgeState dodgeState;

	public HandGunState handGunState;

	public RunState runState;

	public GameObject fireTarget;

	public Transform waist;

	public Transform neck;

	public Transform rootBone;

	public float punchedDamageVal;

	public Vector3 moveDirection;

	public HealthController healthCtl;

	public Animation anima;

	public bool runFlag;

	public bool dieFlag;

	public Vector3 moveTarget;

	public PlayerController player;

	public Transform resetPos;

	public float GunStartFireSqrDistance;

	public float GunStopFireSqrDistance;

	public Vector3 lowerBodyForwardTarget = Vector3.forward;

	public Vector3 lowerBodyForward = Vector3.forward;

	public float lowerBodyDeltaAngle;

	private bool changeStateFlag;

	public bool damagedFlag;

	public AIWayPointsCtl wayPoints;

	public RoadPointNew curRoadPoint;

	public RoadPointNew preRoadPoint;

	public AIManage aiManage;

	public GameObject visableRander;

	public GameObject ragDollObj;

	public GameObject ragDollRoot;

	public GameObject skinObj;

	public Material skinNormalMat;

	public Material policeAppearSkinMat;

	public bool appearFlag;

	public float appearCount;

	public bool alarmFlag;

	public bool attackedFlag;

	public AIMovementMotor moveMotor;

	public GameObject[] ornaments;

	public bool attackLabelFlag;

	public Transform m_transform;

	public Rigidbody m_rigidbody;

	public int fallingMoneyVal;

	public float fallingBulletRate;

	public float intervalTime;

	private float countIntervalTime;

	public int ttt;

	public float turnSpeed;

	public LayerMask aiDetectLayer;

	public LayerMask aiRunStateDetectLayer;

	public RaycastHit hitInfo;

	public float rayCheckTime;

	public float countRayCheckTime;

	public float countDis;

	public GameObject nearestObj;

	public Vector3 hitTangent;

	public bool personBlockFlag;

	public GunsInfo gunInfo;

	private float aiRayTimeCount;

	private Vector3 dir;

	private Vector3 preNormal = Vector3.forward;

	private Vector3 preDir;

	public RoadPointInfo fightingStateCurPoint;

	public bool standFlag;

	private Vector3 inverseVal;

	public bool fightModeStandFlag;

	public bool attackAIFlag;

	private int countAlarm;

	private float checkPlayerDis;

	public float rayCountInterval;

	public RoadPointInfo runStateCurPoint;

	public bool attackFlag;

	public bool firstPunchFlag = true;

	public bool chasingStartFlag;

	public Vector3 chasingStartPos;

	public bool mixFlag;

	private bool onlyPunchFlag;

	private bool onlyHandGunFlag;

	private bool onlyMachineGunFlag;

	public bool killedByCarFlag;

	public bool dodgeFlag;

	private void Awake()
	{
		if (machineGunState.machineGun != null)
		{
			machineGunState.machineGun.gameObject.SetActiveRecursively(false);
		}
		if (handGunState.handGun != null)
		{
			handGunState.handGun.gameObject.SetActiveRecursively(false);
		}
		StateInit();
		healthCtl.OnDestroy = OnDie;
		preNormal = Vector3.zero;
		m_transform = base.transform;
		m_rigidbody = base.GetComponent<Rigidbody>();
		if (GlobalDefine.smallPhoneFlag)
		{
			rayCountInterval = 0.5f;
		}
		else
		{
			rayCountInterval = 0.1f;
		}
	}

	private void Start()
	{
		dieFlag = false;
	}

	private void StateInit()
	{
		walkState.anima = anima;
		runState.anima = anima;
		idleState.anima = anima;
		shottedState.anima = anima;
		dieState.anima = anima;
		runawayState.anima = anima;
		afraidState.anima = anima;
		punchedState.anima = anima;
		anima[punchedState.animaClip[0].name].speed = 2f;
		fightReadyState.anima = anima;
		punchLeftState.anima = anima;
		punchRightState.anima = anima;
		machineGunState.anima = anima;
		handGunState.anima = anima;
		dodgeState.anima = anima;
		walkState.ai = this;
		runState.ai = this;
		idleState.ai = this;
		shottedState.ai = this;
		dieState.ai = this;
		runawayState.ai = this;
		afraidState.ai = this;
		punchedState.ai = this;
		fightReadyState.ai = this;
		punchLeftState.ai = this;
		punchRightState.ai = this;
		machineGunState.ai = this;
		handGunState.ai = this;
		dodgeState.ai = this;
		anima[shottedState.animaClip[0].name].speed = 2f;
	}

	private void Update()
	{
		ttt++;
		if (ttt > 1)
		{
			CheckState();
			ttt = 0;
		}
		curState.MyUpdate();
		countIntervalTime += Time.deltaTime;
		if (countIntervalTime > intervalTime)
		{
			RecycleAI();
			countIntervalTime = 0f;
		}
		if (appearFlag)
		{
			appearCount += Time.deltaTime;
			skinObj.GetComponent<Renderer>().sharedMaterial.color = new Color(1f, 1f, 1f, appearCount);
			if (appearCount >= 0.95f)
			{
				skinObj.GetComponent<Renderer>().sharedMaterial = skinNormalMat;
				appearFlag = false;
				appearCount = 0f;
			}
		}
	}

	public void RecycleAI()
	{
		if (((GameController.instance.curGameMode != GAMEMODE.SURVIVAL && GameController.instance.curGameMode != GAMEMODE.FIGHTING && GameController.instance.curGameMode != GAMEMODE.ROBMOTOR) || !attackFlag) && PlayerController.instance.fireTarget != this && Vector3.SqrMagnitude(player.transform.position - m_transform.position) > NPCPoolController.instance.NPCRecycleDIstance)
		{
			if (PlayerController.instance.preFireTarget == this)
			{
				PlayerController.instance.preFireTarget = null;
			}
			if (machineGunState.machineGun != null)
			{
				machineGunState.machineGun.transform.parent = GunPoolList.instance.machineGunPool.transform;
				machineGunState.machineGun.transform.localPosition = Vector3.zero;
				GunPoolList.instance.machineGunPool.Recycle(machineGunState.machineGun);
				machineGunState.machineGun = null;
			}
			if (handGunState.handGun != null)
			{
				handGunState.handGun.transform.parent = GunPoolList.instance.handGunPool.transform;
				handGunState.handGun.transform.localPosition = Vector3.zero;
				GunPoolList.instance.handGunPool.Recycle(handGunState.handGun);
				handGunState.handGun = null;
			}
			standFlag = false;
			NPCPoolController.instance.RecycleAI(this);
		}
	}

	public void ForceRecycleAI()
	{
		if (PlayerController.instance.preFireTarget == this)
		{
			PlayerController.instance.preFireTarget = null;
		}
		if (machineGunState.machineGun != null)
		{
			machineGunState.machineGun.transform.parent = GunPoolList.instance.machineGunPool.transform;
			machineGunState.machineGun.transform.localPosition = Vector3.zero;
			GunPoolList.instance.machineGunPool.Recycle(machineGunState.machineGun);
			machineGunState.machineGun = null;
		}
		if (handGunState.handGun != null)
		{
			handGunState.handGun.transform.parent = GunPoolList.instance.handGunPool.transform;
			handGunState.handGun.transform.localPosition = Vector3.zero;
			GunPoolList.instance.handGunPool.Recycle(handGunState.handGun);
			handGunState.handGun = null;
		}
		standFlag = false;
		NPCPoolController.instance.RecycleAI(this);
	}

	public void SenceRecycle()
	{
		if (machineGunState.machineGun != null)
		{
			machineGunState.machineGun.transform.parent = GunPoolList.instance.machineGunPool.transform;
			machineGunState.machineGun.transform.localPosition = Vector3.zero;
			GunPoolList.instance.machineGunPool.Recycle(machineGunState.machineGun);
			machineGunState.machineGun = null;
		}
		if (handGunState.handGun != null)
		{
			handGunState.handGun.transform.parent = GunPoolList.instance.handGunPool.transform;
			handGunState.handGun.transform.localPosition = Vector3.zero;
			GunPoolList.instance.handGunPool.Recycle(handGunState.handGun);
			handGunState.handGun = null;
		}
		NPCPoolController.instance.RecycleAI(this);
	}

	private void LateUpdate()
	{
		CheckLateUpdateState();
	}

	private void CheckLateUpdateState()
	{
		AISTATE stateName = curState.stateName;
		if (stateName == AISTATE.MACHINEGUN)
		{
			MachineGunLateUpdateCheck();
		}
	}

	private void MachineGunLateUpdateCheck()
	{
		if (changeStateFlag)
		{
			changeStateFlag = false;
			lowerBodyForward = m_transform.forward;
			lowerBodyForwardTarget = m_transform.forward;
		}
		lowerBodyForward = Vector3.RotateTowards(lowerBodyForward, lowerBodyForwardTarget, Time.deltaTime * 300f * ((float)Math.PI / 180f), 1f);
		if (anima.isPlaying)
		{
			lowerBodyDeltaAngle = Mathf.DeltaAngle(MoveAnimation.HorizontalAngle(m_transform.forward), MoveAnimation.HorizontalAngle(lowerBodyForward));
		}
		else
		{
			lowerBodyDeltaAngle = 0f;
		}
		if (lowerBodyDeltaAngle > 20f || lowerBodyDeltaAngle < -25f)
		{
			lowerBodyForwardTarget = m_transform.forward;
		}
		Quaternion quaternion = Quaternion.Euler(0f, lowerBodyDeltaAngle, 0f);
		rootBone.rotation = quaternion * rootBone.rotation;
		waist.rotation = Quaternion.Inverse(quaternion) * waist.rotation;
	}

	private void AI()
	{
		aiRayTimeCount += Time.deltaTime;
		if (aiRayTimeCount >= rayCountInterval)
		{
			aiRayTimeCount = 0f;
			dir = (moveTarget - m_transform.position).normalized;
			if (!policeFlag)
			{
				if (Physics.Raycast(fireTarget.transform.position - Vector3.up * (fireTarget.transform.position.y - 0.5f), dir, out hitInfo, 2f, aiDetectLayer))
				{
					if (preNormal.x * hitInfo.normal.x + preNormal.z * hitInfo.normal.z < 0.99f)
					{
						preNormal = hitInfo.normal;
						hitTangent = new Vector3(0f - hitInfo.normal.z, 0f, hitInfo.normal.x);
						if (hitTangent.x * dir.x + hitTangent.z * dir.z > 0f)
						{
							dir = hitTangent;
						}
						else
						{
							dir = new Vector3(0f - hitTangent.x, 0f, 0f - hitTangent.z);
						}
						preDir = dir;
					}
					else
					{
						dir = preDir;
					}
				}
			}
			else if (Physics.Raycast(fireTarget.transform.position - Vector3.up * (fireTarget.transform.position.y - 0.5f), dir, out hitInfo, 2f, aiRunStateDetectLayer))
			{
				if (preNormal.x * hitInfo.normal.x + preNormal.z * hitInfo.normal.z < 0.99f)
				{
					preNormal = hitInfo.normal;
					hitTangent = new Vector3(0f - hitInfo.normal.z, 0f, hitInfo.normal.x);
					if (hitTangent.x * dir.x + hitTangent.z * dir.z > 0f)
					{
						dir = hitTangent;
					}
					else
					{
						dir = new Vector3(0f - hitTangent.x, 0f, 0f - hitTangent.z);
					}
					preDir = dir;
				}
				else
				{
					dir = preDir;
				}
			}
		}
		moveDirection = Vector3.Lerp(moveDirection, new Vector3(dir.x, 0f, dir.z).normalized, Time.deltaTime * 10f);
		CheckPassWayPoint();
	}

	public void CheckPassWayPoint()
	{
		if (chasingStartFlag)
		{
			moveTarget = PlayerController.instance.transform.position;
		}
		else if (fightingStateCurPoint == null)
		{
			if (curRoadPoint != null)
			{
				if (curRoadPoint.roadInfo != null)
				{
					if (ToolFunction.InverseZ(curRoadPoint.position, m_transform.position, curRoadPoint.forward) < 1f)
					{
						moveTarget = GetNextWayPoint();
						if (UnityEngine.Random.Range(0, 5) > 3)
						{
							ChangeState(AISTATE.IDLE);
						}
					}
				}
				else
				{
					moveTarget = PlayerController.instance.transform.position;
				}
			}
			else
			{
				moveTarget = PlayerController.instance.transform.position;
			}
		}
		else if (Vector3.Distance(base.transform.position, moveTarget) < 1f)
		{
			moveTarget = GetNextFightingStatePoint();
			if (UnityEngine.Random.Range(0, 5) > 3)
			{
				ChangeState(AISTATE.IDLE);
			}
		}
	}

	public Vector3 GetNextFightingStatePoint()
	{
		if (fightingStateCurPoint.linkPoint.Count > 0)
		{
			fightingStateCurPoint = fightingStateCurPoint.linkPoint[0];
			return fightingStateCurPoint.transform.position;
		}
		return Vector3.zero;
	}

	public Vector3 GetNextWayPoint()
	{
		RoadPointNew roadPointNew = curRoadPoint;
		RoadPointNew linkPoint;
		do
		{
			linkPoint = curRoadPoint.GetLinkPoint(UnityEngine.Random.Range(0, curRoadPoint.linkPoint.Length));
		}
		while (linkPoint == null || linkPoint == preRoadPoint);
		curRoadPoint = linkPoint;
		preRoadPoint = roadPointNew;
		return GetMoveTarget(m_transform.position);
	}

	public Vector3 GetMoveTarget(Vector3 pos)
	{
		if (ToolFunction.isForward(pos - curRoadPoint.position, curRoadPoint.right))
		{
			return curRoadPoint.position + curRoadPoint.right * UnityEngine.Random.Range(curRoadPoint.roadInfo.minSideWalkDis, curRoadPoint.roadInfo.maxSideWalkDis);
		}
		return curRoadPoint.position - curRoadPoint.right * UnityEngine.Random.Range(curRoadPoint.roadInfo.minSideWalkDis, curRoadPoint.roadInfo.maxSideWalkDis);
	}

	public void CheckState()
	{
		switch (curState.stateName)
		{
		case AISTATE.IDLE:
			IdleStateCheck();
			break;
		case AISTATE.WALK:
			WalkStateCheck();
			break;
		case AISTATE.RUN:
			RunStateCheck();
			break;
		case AISTATE.DAMAGED:
			ShottedStateCheck();
			break;
		case AISTATE.DIE:
			DieStateCheck();
			break;
		case AISTATE.RUNAWAY:
			RunawayStateCheck();
			break;
		case AISTATE.PUNCHED:
			PunchedStateCheck();
			break;
		case AISTATE.FIGHTREADY:
			FightReadyStateCheck();
			break;
		case AISTATE.PUNCH:
			PunchStateCheck();
			break;
		case AISTATE.MACHINEGUN:
			MachineGunStateCheck();
			break;
		case AISTATE.AFRAID:
			AfraidStateCheck();
			break;
		case AISTATE.DODGE:
			DodgeStateCheck();
			break;
		case AISTATE.HANDGUN:
			HandGunStateCheck();
			break;
		case AISTATE.ATTACK:
			break;
		}
	}

	public void DodgeStateCheck()
	{
		if (dieFlag || !(dodgeState.sumTime >= dodgeState.endTime))
		{
			return;
		}
		if (preState == dodgeState)
		{
			if (!policeFlag)
			{
				ChangeState(idleState.stateName);
			}
			else if (handGunFlag)
			{
				ChangeState(AISTATE.HANDGUN);
			}
			else if (machineGunFlag)
			{
				ChangeState(AISTATE.MACHINEGUN);
			}
		}
		else
		{
			ChangeState(preState.stateName);
		}
	}

	public void AfraidStateCheck()
	{
		checkChangeMachineGunState();
	}

	public void MachineGunStateCheck()
	{
		if (!damagedFlag)
		{
			if ((PlayerController.instance.transform.position - m_transform.position).sqrMagnitude < 200f)
			{
				Vector3 normalized = (player.transform.position - m_transform.position).normalized;
				moveDirection = Vector3.Lerp(moveDirection, new Vector3(normalized.x, 0f, normalized.z).normalized, Time.deltaTime * 10f);
				anima.transform.forward = Vector3.Lerp(anima.transform.forward, moveDirection, Time.deltaTime * 10f);
			}
			else
			{
				ChangeState(AISTATE.RUN);
			}
		}
		if (player.curState == PLAYERSTATE.DIE)
		{
			ChangeState(AISTATE.WALK);
		}
	}

	public void HandGunStateCheck()
	{
		if (!damagedFlag)
		{
			if ((PlayerController.instance.transform.position - m_transform.position).sqrMagnitude < 200f)
			{
				Vector3 normalized = (PlayerController.instance.transform.position - m_transform.position).normalized;
				moveDirection = Vector3.Lerp(moveDirection, new Vector3(normalized.x, 0f, normalized.z).normalized, Time.deltaTime * 10f);
				anima.transform.forward = Vector3.Lerp(anima.transform.forward, moveDirection, Time.deltaTime * 10f);
			}
			else
			{
				ChangeState(AISTATE.RUN);
			}
		}
		if (player.curState == PLAYERSTATE.DIE)
		{
			ChangeState(AISTATE.IDLE);
		}
	}

	public void FightReadyStateCheck()
	{
		if (PlayerController.instance.curState == PLAYERSTATE.CAR && chasingStartFlag)
		{
			chasingStartFlag = false;
			moveTarget = curRoadPoint.position;
			ChangeState(AISTATE.IDLE);
		}
		aiRayTimeCount += Time.deltaTime;
		if (aiRayTimeCount >= rayCountInterval)
		{
			aiRayTimeCount = 0f;
			dir = (PlayerController.instance.transform.position - m_transform.position).normalized;
			if (Physics.Raycast(fireTarget.transform.position - new Vector3(0f, 0.8f, 0f), dir, out hitInfo, 2f, aiRunStateDetectLayer))
			{
				if (preNormal.x * hitInfo.normal.x + preNormal.z * hitInfo.normal.z < 0.99f)
				{
					preNormal = hitInfo.normal;
					hitTangent = new Vector3(0f - hitInfo.normal.z, 0f, hitInfo.normal.x);
					if (hitTangent.x * dir.x + hitTangent.z * dir.z > 0.1f)
					{
						dir = hitTangent;
					}
					else
					{
						dir = new Vector3(0f - hitTangent.x, 0f, 0f - hitTangent.z);
					}
					preDir = dir;
				}
				else
				{
					dir = preDir;
				}
			}
		}
		moveDirection = Vector3.Lerp(moveDirection, new Vector3(dir.x, 0f, dir.z).normalized, Time.deltaTime * 10f);
		fightReadyState.moveDirection = moveDirection;
		if (player.curState == PLAYERSTATE.DIE)
		{
			Invoke("InvokeChangeWalk", 1f);
		}
		if (!chasingStartFlag || !(Vector3.SqrMagnitude(m_transform.position - chasingStartPos) > 2500f))
		{
			return;
		}
		chasingStartFlag = false;
		if (curRoadPoint == null)
		{
			if (PlayerController.instance.fireTarget == this)
			{
				PlayerController.instance.fireTarget = null;
			}
			ForceRecycleAI();
		}
		else if (curRoadPoint.roadInfo == null)
		{
			if (PlayerController.instance.fireTarget != this)
			{
				PlayerController.instance.fireTarget = null;
			}
			ForceRecycleAI();
		}
		else
		{
			moveTarget = curRoadPoint.position;
			ChangeState(AISTATE.IDLE);
		}
	}

	private void InvokeChangeWalk()
	{
		ChangeState(AISTATE.WALK);
	}

	public void PunchStateCheck()
	{
		if (firstPunchFlag)
		{
			if (punchLeftState.sumTime > punchLeftState.punchInterval)
			{
				ChangeState(AISTATE.FIGHTREADY);
				firstPunchFlag = false;
			}
		}
		else if (punchRightState.sumTime > punchRightState.punchInterval)
		{
			ChangeState(AISTATE.FIGHTREADY);
			firstPunchFlag = true;
		}
	}

	public void IdleStateCheck()
	{
		moveMotor.movementDirection = Vector3.zero;
		if (standFlag)
		{
			return;
		}
		if (!fightModeStandFlag && curState.sumTime > 3f && PlayerController.instance.curState != PLAYERSTATE.DIE)
		{
			ChangeState(AISTATE.WALK);
		}
		countAlarm++;
		if (!attackAIFlag || PlayerController.instance.curState == PLAYERSTATE.DIE || countAlarm % 5 != 0)
		{
			return;
		}
		inverseVal = anima.transform.InverseTransformPoint(PlayerController.instance.transform.position);
		if (!(inverseVal.z > 0f) || !(Mathf.Abs(inverseVal.x) / inverseVal.z < 1f))
		{
			return;
		}
		checkPlayerDis = (PlayerController.instance.transform.position - m_transform.position).sqrMagnitude;
		if (!fightModeStandFlag)
		{
			if (handGunFlag)
			{
				if (checkPlayerDis < 625f)
				{
					ChangeState(AISTATE.HANDGUN);
				}
			}
			else if (machineGunFlag)
			{
				if (checkPlayerDis < 625f)
				{
					ChangeState(AISTATE.MACHINEGUN);
				}
			}
			else if (PlayerController.instance.curState != PLAYERSTATE.CAR && checkPlayerDis < 625f)
			{
				ChangeState(AISTATE.FIGHTREADY);
			}
		}
		else if (handGunFlag)
		{
			if (checkPlayerDis < 225f)
			{
				ChangeState(AISTATE.HANDGUN);
			}
		}
		else if (machineGunFlag)
		{
			if (checkPlayerDis < 225f)
			{
				ChangeState(AISTATE.MACHINEGUN);
			}
		}
		else if (PlayerController.instance.curState != PLAYERSTATE.CAR && checkPlayerDis < 225f)
		{
			ChangeState(AISTATE.FIGHTREADY);
		}
	}

	public void WalkStateCheck()
	{
		AI();
		walkState.direction = moveDirection.normalized;
		countAlarm++;
		if (!attackAIFlag || PlayerController.instance.curState == PLAYERSTATE.DIE || countAlarm % 5 != 0)
		{
			return;
		}
		countAlarm = 0;
		inverseVal = anima.transform.InverseTransformPoint(PlayerController.instance.transform.position);
		if (!(inverseVal.z > 0f) || !(Mathf.Abs(inverseVal.x) / inverseVal.z < 1f))
		{
			return;
		}
		checkPlayerDis = (PlayerController.instance.transform.position - m_transform.position).sqrMagnitude;
		if (handGunFlag)
		{
			if (checkPlayerDis < 625f)
			{
				ChangeState(AISTATE.HANDGUN);
			}
		}
		else if (machineGunFlag)
		{
			if (checkPlayerDis < 625f)
			{
				ChangeState(AISTATE.MACHINEGUN);
			}
		}
		else if (PlayerController.instance.curState != PLAYERSTATE.CAR && checkPlayerDis < 625f)
		{
			ChangeState(AISTATE.FIGHTREADY);
		}
	}

	public void RunStateCheck()
	{
		if (policeFlag)
		{
			AI();
		}
		else
		{
			RunStateAI();
		}
		runState.direction = moveDirection;
		if ((PlayerController.instance.transform.position - m_transform.position).sqrMagnitude < 100f)
		{
			if (handGunFlag)
			{
				ChangeState(AISTATE.HANDGUN);
			}
			else if (machineGunFlag)
			{
				ChangeState(AISTATE.MACHINEGUN);
			}
			else
			{
				ChangeState(AISTATE.FIGHTREADY);
			}
		}
		if (!chasingStartFlag || !(Vector3.SqrMagnitude(m_transform.position - chasingStartPos) > 2500f))
		{
			return;
		}
		chasingStartFlag = false;
		if (curRoadPoint == null)
		{
			if (PlayerController.instance.fireTarget == this)
			{
				PlayerController.instance.fireTarget = null;
				PlayerController.instance.animaCtl.OnChangeAimState(false);
			}
			ForceRecycleAI();
		}
		else if (curRoadPoint.roadInfo == null)
		{
			if (PlayerController.instance.fireTarget == this)
			{
				PlayerController.instance.fireTarget = null;
				PlayerController.instance.animaCtl.OnChangeAimState(false);
			}
			ForceRecycleAI();
		}
		else
		{
			moveTarget = curRoadPoint.position;
			ChangeState(AISTATE.IDLE);
		}
	}

	public void RunStateAI()
	{
		aiRayTimeCount += Time.deltaTime;
		if (aiRayTimeCount >= rayCountInterval)
		{
			aiRayTimeCount = 0f;
			dir = (moveTarget - m_transform.position).normalized;
			if (Physics.Raycast(fireTarget.transform.position - new Vector3(0f, 0.8f, 0f), dir, out hitInfo, 2f, aiRunStateDetectLayer))
			{
				if (preNormal.x * hitInfo.normal.x + preNormal.z * hitInfo.normal.z < 0.99f)
				{
					preNormal = hitInfo.normal;
					hitTangent = new Vector3(0f - hitInfo.normal.z, 0f, hitInfo.normal.x);
					if (hitTangent.x * dir.x + hitTangent.z * dir.z > 0.1f)
					{
						dir = hitTangent;
					}
					else
					{
						dir = new Vector3(0f - hitTangent.x, 0f, 0f - hitTangent.z);
					}
					preDir = dir;
				}
				else
				{
					dir = preDir;
				}
			}
		}
		moveDirection = Vector3.Lerp(moveDirection, new Vector3(dir.x, 0f, dir.z).normalized, Time.deltaTime * 10f);
		RunStateCheckPassWayPoint();
	}

	public void RunStateCheckPassWayPoint()
	{
		if (runStateCurPoint != null)
		{
			if (Vector3.Distance(m_transform.position, moveTarget) < 2f)
			{
				if (runStateCurPoint.linkPoint.Count > 0)
				{
					moveTarget = RunStateGetNextWayPoint();
				}
				else
				{
					moveTarget = PlayerController.instance.transform.position;
					runStateCurPoint = null;
				}
				chasingStartPos = m_transform.position;
			}
		}
		else
		{
			moveTarget = PlayerController.instance.transform.position;
		}
	}

	public Vector3 RunStateGetNextWayPoint()
	{
		if (runStateCurPoint.linkPoint.Count > 0)
		{
			runStateCurPoint = runStateCurPoint.linkPoint[UnityEngine.Random.Range(0, runStateCurPoint.linkPoint.Count)];
		}
		return RunStateGetMoveTarget();
	}

	public Vector3 RunStateGetMoveTarget()
	{
		return runStateCurPoint.transform.position + runStateCurPoint.transform.right * UnityEngine.Random.Range(-2f, 2f);
	}

	public void checkChangeMachineGunState()
	{
		if (machineGunFlag && player.curState != PLAYERSTATE.DIE && (player.transform.position - m_transform.position).sqrMagnitude < GunStartFireSqrDistance)
		{
			ChangeState(AISTATE.MACHINEGUN);
		}
	}

	public void ShottedStateCheck()
	{
		if (!(shottedState.anima[shottedState.animaClip[shottedState.animaIndex].name].normalizedTime >= 0.85f))
		{
			return;
		}
		if (!policeFlag)
		{
			if (standFlag)
			{
				ChangeState(AISTATE.AFRAID);
			}
			else if (!attackFlag && !fightBackFlag)
			{
				ChangeState(AISTATE.AFRAID);
			}
			else if (machineGunFlag)
			{
				ChangeState(AISTATE.MACHINEGUN);
			}
			else if (handGunFlag)
			{
				ChangeState(AISTATE.HANDGUN);
			}
			else
			{
				ChangeState(AISTATE.FIGHTREADY);
			}
		}
		else if (handGunFlag)
		{
			ChangeState(AISTATE.HANDGUN);
		}
		else if (machineGunFlag)
		{
			ChangeState(AISTATE.MACHINEGUN);
		}
	}

	public void PunchedStateCheck()
	{
		if (standFlag)
		{
			if (punchedState.anima[punchedState.animaClip[punchedState.animaIndex].name].normalizedTime >= 0.85f)
			{
				punchedState.anima[punchedState.animaClip[punchedState.animaIndex].name].normalizedTime = 0f;
				ChangeState(AISTATE.AFRAID);
			}
		}
		else if (!attackFlag && !fightBackFlag)
		{
			if (punchedState.anima[punchedState.animaClip[punchedState.animaIndex].name].normalizedTime >= 0.85f)
			{
				punchedState.anima[punchedState.animaClip[punchedState.animaIndex].name].normalizedTime = 0f;
				ChangeState(AISTATE.AFRAID);
			}
		}
		else if (punchedState.anima[punchedState.animaClip[punchedState.animaIndex].name].normalizedTime >= 0.85f)
		{
			punchedState.anima[punchedState.animaClip[punchedState.animaIndex].name].normalizedTime = 0f;
			if (machineGunFlag)
			{
				ChangeState(AISTATE.MACHINEGUN);
			}
			else if (handGunFlag)
			{
				ChangeState(AISTATE.HANDGUN);
			}
			else
			{
				ChangeState(AISTATE.FIGHTREADY);
			}
		}
	}

	public void DieStateCheck()
	{
		if (curState.sumTime > 3f)
		{
			standFlag = false;
			if (machineGunState.machineGun != null)
			{
				machineGunState.machineGun.transform.parent = GunPoolList.instance.machineGunPool.transform;
				machineGunState.machineGun.transform.localPosition = Vector3.zero;
				GunPoolList.instance.machineGunPool.Recycle(machineGunState.machineGun);
				machineGunState.machineGun = null;
			}
			if (handGunState.handGun != null)
			{
				handGunState.handGun.transform.parent = GunPoolList.instance.handGunPool.transform;
				handGunState.handGun.transform.localPosition = Vector3.zero;
				GunPoolList.instance.handGunPool.Recycle(handGunState.handGun);
				handGunState.handGun = null;
			}
			NPCPoolController.instance.RecycleAI(this);
		}
	}

	public void RunawayStateCheck()
	{
	}

	public void ChangeState(AISTATE newState)
	{
		changeStateFlag = true;
		preState = curState;
		if ((preState.stateName != AISTATE.MACHINEGUN || newState != AISTATE.PUNCHED) && (preState.stateName != AISTATE.HANDGUN || newState != AISTATE.PUNCHED))
		{
			preState.MyExit();
		}
		if (preState.stateName == AISTATE.DODGE)
		{
			dodgeFlag = false;
		}
		aiRayTimeCount = 9f;
		switch (newState)
		{
		case AISTATE.WALK:
			if (attackLabelFlag)
			{
				attackLabelFlag = false;
				AttackAILabelPool.instance.RemoveAttackAI(base.gameObject);
			}
			curState = walkState;
			break;
		case AISTATE.RUN:
			if (GameController.instance.curGameMode != GAMEMODE.FIGHTING && GameController.instance.curGameMode != GAMEMODE.ROBMOTOR && !chasingStartFlag)
			{
				chasingStartFlag = true;
				chasingStartPos = m_transform.position;
			}
			if (!attackLabelFlag)
			{
				attackLabelFlag = true;
				AttackAILabelPool.instance.AddAttackAI(base.gameObject);
			}
			curState = runState;
			break;
		case AISTATE.IDLE:
			if (attackLabelFlag)
			{
				attackLabelFlag = false;
				AttackAILabelPool.instance.RemoveAttackAI(base.gameObject);
			}
			curState = idleState;
			break;
		case AISTATE.DAMAGED:
			curState = shottedState;
			break;
		case AISTATE.DIE:
			curState = dieState;
			break;
		case AISTATE.RUNAWAY:
			moveDirection = new Vector3(UnityEngine.Random.Range(-10, 10), 0f, UnityEngine.Random.Range(-10, 10)).normalized;
			runawayState.direction = moveDirection;
			curState = runawayState;
			break;
		case AISTATE.AFRAID:
			curState = afraidState;
			break;
		case AISTATE.PUNCHED:
			curState = punchedState;
			break;
		case AISTATE.FIGHTREADY:
			if (GameController.instance.curGameMode != GAMEMODE.FIGHTING && GameController.instance.curGameMode != GAMEMODE.ROBMOTOR && !chasingStartFlag)
			{
				chasingStartFlag = true;
				chasingStartPos = m_transform.position;
			}
			if (!attackLabelFlag)
			{
				attackLabelFlag = true;
				AttackAILabelPool.instance.AddAttackAI(base.gameObject);
			}
			curState = fightReadyState;
			break;
		case AISTATE.PUNCH:
			if (firstPunchFlag)
			{
				curState = punchLeftState;
			}
			else
			{
				curState = punchRightState;
			}
			break;
		case AISTATE.MACHINEGUN:
			if (!attackLabelFlag)
			{
				attackLabelFlag = true;
				AttackAILabelPool.instance.AddAttackAI(base.gameObject);
			}
			curState = machineGunState;
			break;
		case AISTATE.DODGE:
			curState = dodgeState;
			break;
		case AISTATE.HANDGUN:
			if (!attackLabelFlag)
			{
				attackLabelFlag = true;
				AttackAILabelPool.instance.AddAttackAI(base.gameObject);
			}
			curState = handGunState;
			break;
		}
		curState.MyEnter();
	}

	public void OnShotted(PlayerController tar, float damageVal)
	{
		chasingStartPos = m_transform.position;
		damagedFlag = true;
		alarmFlag = true;
		CitySenceController.instance.AlarmNPC();
		if (!attackedFlag)
		{
			attackedFlag = true;
			if (!policeFlag)
			{
				if (!attackFlag)
				{
					PoliceLevelCtl.AttackPerson();
				}
			}
			else
			{
				PoliceLevelCtl.AttackPolice();
			}
		}
		if (curState.stateName == AISTATE.RUNAWAY || curState.stateName == AISTATE.FIGHTREADY)
		{
			anima[shottedState.animaClip[0].name].layer = 2;
			anima[shottedState.animaClip[0].name].AddMixingTransform(waist);
			anima[shottedState.animaClip[0].name].wrapMode = WrapMode.Once;
			anima.CrossFade(shottedState.animaClip[0].name);
			mixFlag = true;
		}
		else if (curState.stateName == AISTATE.IDLE || curState.stateName == AISTATE.WALK || curState.stateName == AISTATE.AFRAID)
		{
			anima[shottedState.animaClip[0].name].layer = 1;
			anima[shottedState.animaClip[0].name].wrapMode = WrapMode.Loop;
			if (mixFlag)
			{
				anima[shottedState.animaClip[0].name].RemoveMixingTransform(waist);
				mixFlag = false;
			}
			ChangeState(AISTATE.DAMAGED);
		}
		else if (curState.stateName == AISTATE.MACHINEGUN)
		{
			anima[shottedState.animaClip[0].name].layer = 2;
			anima[shottedState.animaClip[0].name].AddMixingTransform(neck);
			anima[shottedState.animaClip[0].name].wrapMode = WrapMode.Once;
			anima[shottedState.animaClip[0].name].blendMode = AnimationBlendMode.Additive;
			anima.Play(shottedState.animaClip[0].name);
			machineGunState.countShotedInterval = 0f;
			mixFlag = true;
		}
		else if (curState.stateName == AISTATE.HANDGUN)
		{
			anima[shottedState.animaClip[0].name].layer = 2;
			anima[shottedState.animaClip[0].name].AddMixingTransform(neck);
			anima[shottedState.animaClip[0].name].wrapMode = WrapMode.Once;
			anima[shottedState.animaClip[0].name].blendMode = AnimationBlendMode.Additive;
			anima.Play(shottedState.animaClip[0].name);
			handGunState.countShotedInterval = 0f;
			mixFlag = true;
		}
		healthCtl.Damaged(damageVal);
		if (PlayerController.instance.curState == PLAYERSTATE.HANDGUN)
		{
			onlyHandGunFlag = true;
		}
		else if (PlayerController.instance.curState == PLAYERSTATE.MACHINEGUN)
		{
			onlyMachineGunFlag = true;
		}
	}

	public void OnPunched(PlayerController tar, float damageVal)
	{
		if (dieFlag)
		{
			return;
		}
		if (!attackedFlag)
		{
			attackedFlag = true;
			if (!policeFlag)
			{
				if (!attackFlag)
				{
					PoliceLevelCtl.AttackPerson();
				}
			}
			else
			{
				PoliceLevelCtl.AttackPolice();
			}
		}
		alarmFlag = true;
		CitySenceController.instance.AlarmNPC();
		punchedState.target = tar.gameObject;
		ChangeState(AISTATE.PUNCHED);
		healthCtl.Damaged(damageVal);
		chasingStartPos = m_transform.position;
		onlyPunchFlag = true;
		AudioController.instance.play(AudioType.PUNCH_PERSON);
		if (type == NPCTYPE.NORMALWOMEN || type == NPCTYPE.NORMALWOMEN2)
		{
			AudioController.instance.play(AudioType.WOMENPUNCHED);
		}
		else
		{
			AudioController.instance.play(AudioType.MANPUNCHED);
		}
	}

	public void OnDie()
	{
		if (dieFlag)
		{
			return;
		}
		dieFlag = true;
		ChangeState(AISTATE.DIE);
		base.GetComponent<Collider>().enabled = false;
		base.GetComponent<Rigidbody>().useGravity = false;
		if (onlyPunchFlag && !onlyHandGunFlag && !onlyMachineGunFlag)
		{
			GlobalInf.punchKillNum++;
		}
		else if (!onlyPunchFlag && onlyHandGunFlag && !onlyMachineGunFlag)
		{
			GlobalInf.handGunKillNum++;
		}
		else if (!onlyPunchFlag && !onlyHandGunFlag && onlyMachineGunFlag)
		{
			GlobalInf.machineGunKillNum++;
		}
		if (PlayerController.instance.fireTarget == this)
		{
			PlayerController.instance.fireTarget = null;
			if (PlayerController.instance.preFireTarget == this)
			{
				PlayerController.instance.preFireTarget = null;
			}
			PlayerController.instance.animaCtl.OnChangeAimState(false);
		}
		GlobalInf.totalKillNum++;
		GlobalInf.curKill++;
		GlobalInf.dailyKillNum++;
		if (policeFlag)
		{
			GlobalInf.policeKillNum++;
			GlobalInf.dailyKillPoliceNum++;
		}
		else if (type == NPCTYPE.NORMALWHITE || type == NPCTYPE.NORMALWOMEN || type == NPCTYPE.NORMALBLACK_HG || type == NPCTYPE.NORMALBLACK_PUNCH || type == NPCTYPE.NORMALWHITE2 || type == NPCTYPE.NORMALWOMEN2)
		{
			GlobalInf.totalKillCitizens++;
		}
		else
		{
			GlobalInf.totalKillGangsters++;
			GlobalInf.dailyKillGangstarNum++;
		}
		if (!GlobalDefine.smallPhoneFlag)
		{
			if (ragDollObj == null)
			{
				ragDollObj = RagDollPool.instance.GetNpcRagdoll(type);
			}
			if (ragDollObj != null)
			{
				ragDollObj.transform.parent = m_transform;
				ragDollObj.transform.localPosition = Vector3.zero;
				ragDollObj.transform.localRotation = Quaternion.identity;
				SetChildPos(anima.transform, ragDollObj.transform);
				anima.gameObject.SetActiveRecursively(false);
				ragDollObj.SetActiveRecursively(true);
				Vector3 normalized = (m_transform.position - PlayerController.instance.transform.position).normalized;
				normalized = new Vector3(normalized.x, 0.2f, normalized.z).normalized;
				if (!killedByCarFlag)
				{
					for (int i = 0; i < ragDollObj.transform.childCount; i++)
					{
						if (ragDollObj.transform.GetChild(i).GetComponent<Rigidbody>() != null)
						{
							ragDollObj.transform.GetChild(i).GetComponent<Rigidbody>().velocity = normalized * 30f;
							break;
						}
					}
				}
			}
		}
		if (!attackFlag)
		{
			if (!killedByCarFlag)
			{
				if (!policeFlag)
				{
					PoliceLevelCtl.KillPerson();
				}
				else
				{
					PoliceLevelCtl.KillPolice();
				}
			}
			else
			{
				killedByCarFlag = false;
				if (GameController.instance.curGameMode == GAMEMODE.CARKILLING)
				{
					GameController.instance.carKillingMode.killNum++;
					GameUIController.instance.killLabel.Reset(GameController.instance.carKillingMode.killNum, base.transform.position + Vector3.up * 2f);
				}
				if (policeFlag)
				{
					PoliceLevelCtl.CrashPolice();
				}
				else
				{
					PoliceLevelCtl.CrashPerson();
				}
			}
		}
		if (machineGunState.machineGun != null)
		{
			machineGunState.machineGun.transform.parent = GunPoolList.instance.machineGunPool.transform;
			machineGunState.machineGun.transform.localPosition = Vector3.zero;
			GunPoolList.instance.machineGunPool.Recycle(machineGunState.machineGun);
			machineGunState.machineGun = null;
		}
		if (handGunState.handGun != null)
		{
			handGunState.handGun.transform.parent = GunPoolList.instance.handGunPool.transform;
			handGunState.handGun.transform.localPosition = Vector3.zero;
			GunPoolList.instance.handGunPool.Recycle(handGunState.handGun);
			handGunState.handGun = null;
		}
		if (GameController.instance.curGameMode == GAMEMODE.GUNKILLING)
		{
			GameController.instance.gunKillingMode.killNum++;
			GameUIController.instance.killLabel.Reset(GameController.instance.gunKillingMode.killNum, base.transform.position + Vector3.up * 2f);
		}
		else if (GameController.instance.curGameMode == GAMEMODE.SURVIVAL)
		{
			GameController.instance.survivalMode.killNum++;
			GameUIController.instance.killLabel.Reset(GameController.instance.survivalMode.killNum, base.transform.position + Vector3.up * 2f);
		}
		else if (GameController.instance.curGameMode == GAMEMODE.FIGHTING)
		{
			GameController.instance.fightingMode.killNum++;
			GameUIController.instance.killLabel.Reset(GameController.instance.fightingMode.killNum, base.transform.position + Vector3.up * 2f);
		}
		if (!standFlag)
		{
			FallingObjPool.instance.GetFallingObj(m_transform.position, fallingMoneyVal, fallingBulletRate);
		}
		if (GameController.instance.curGameMode == GAMEMODE.SURVIVAL)
		{
			GameController.instance.survivalMode.AIDie();
		}
		else if (GameController.instance.curGameMode == GAMEMODE.FIGHTING)
		{
			GameController.instance.fightingMode.AIDie(this);
		}
	}

	public void SetChildPos(Transform target, Transform temp)
	{
		if (temp.gameObject.GetComponent<Rigidbody>() != null)
		{
			temp.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			temp.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
		temp.localPosition = target.localPosition;
		temp.localRotation = target.localRotation;
		for (int i = 0; i < target.childCount; i++)
		{
			if (i < temp.childCount && i <= target.childCount)
			{
				SetChildPos(target.GetChild(i), temp.GetChild(i));
			}
		}
	}

	public void ResetAI(RoadPointNew cPoint, RoadPointNew pPoint, Vector3 pos, bool atFlag, bool fightBFlag, bool handGunFlag, bool machineGunFlag)
	{
		base.gameObject.SetActiveRecursively(true);
		preNormal = Vector3.zero;
		chasingStartFlag = false;
		attackAIFlag = atFlag;
		fightBackFlag = fightBFlag;
		this.handGunFlag = handGunFlag;
		this.machineGunFlag = machineGunFlag;
		base.GetComponent<Collider>().enabled = true;
		base.transform.position = pos;
		base.transform.forward = (cPoint.position - pos).normalized;
		if (UnityEngine.Random.Range(0, 3) > 0)
		{
			ChangeState(AISTATE.WALK);
		}
		else
		{
			ChangeState(AISTATE.IDLE);
		}
		healthCtl.Reset();
		dieFlag = false;
		curRoadPoint = cPoint;
		preRoadPoint = pPoint;
		moveTarget = GetMoveTarget(base.transform.position);
		if (machineGunState.machineGun != null)
		{
			machineGunState.machineGun.gameObject.SetActiveRecursively(false);
		}
		if (handGunState.handGun != null)
		{
			handGunState.handGun.gameObject.SetActiveRecursively(false);
		}
		ResetAppear();
	}

	public void SetOrnaments()
	{
		for (int i = 0; i < ornaments.Length; i++)
		{
			ornaments[i].gameObject.SetActiveRecursively(false);
		}
		if (ornaments.Length != 0)
		{
			switch (UnityEngine.Random.Range(0, ornaments.Length + 1))
			{
			case 0:
				ornaments[0].gameObject.SetActiveRecursively(true);
				break;
			case 1:
				ornaments[1].gameObject.SetActiveRecursively(true);
				break;
			}
		}
	}

	public void ResetAttackAI(RoadPointNew cPoint, RoadPointNew pPoint, Vector3 pos, GunsInfo info, int healthVal)
	{
		base.gameObject.SetActiveRecursively(true);
		base.GetComponent<Collider>().enabled = true;
		base.transform.position = pos;
		base.transform.forward = (cPoint.position - pos).normalized;
		healthCtl.Reset();
		healthCtl.maxHealthVal = healthVal;
		dieFlag = false;
		curRoadPoint = cPoint;
		preRoadPoint = pPoint;
		moveTarget = GetMoveTarget(base.transform.position);
		if (machineGunState.machineGun != null)
		{
			machineGunState.machineGun.gameObject.SetActiveRecursively(false);
		}
		if (handGunState.handGun != null)
		{
			handGunState.handGun.gameObject.SetActiveRecursively(false);
		}
		gunInfo = info;
		ResetAppear();
	}

	public void ResetAppear()
	{
		skinObj.GetComponent<Renderer>().sharedMaterial = MaterialPool.instance.GetNpcMaterial(type);
		skinObj.GetComponent<Renderer>().sharedMaterial.color = new Color(1f, 1f, 1f, 0f);
		appearFlag = true;
		appearCount = 0f;
		SetOrnaments();
	}

	public void ResetAI(Transform pos)
	{
		base.transform.position = pos.position;
		base.transform.forward = pos.forward;
		ChangeState(AISTATE.IDLE);
		healthCtl.Reset();
		base.GetComponent<Collider>().enabled = true;
		runFlag = false;
		dieFlag = false;
	}

	public void InvokeBulletAttack(float t)
	{
		Invoke("BulletAttack", t);
	}

	public void BulletAttack()
	{
		if (curState.stateName == AISTATE.MACHINEGUN)
		{
			player.OnShotted(machineGunState.machineGun.gunInfo.damage, this);
		}
		else if (curState.stateName == AISTATE.HANDGUN)
		{
			player.OnShotted(handGunState.handGun.gunInfo.damage, this);
		}
	}

	public void OnAlarm()
	{
		if (policeFlag && curState.stateName != AISTATE.DIE)
		{
			if (handGunFlag)
			{
				if (curState.stateName != AISTATE.HANDGUN)
				{
					ChangeState(AISTATE.HANDGUN);
				}
			}
			else if (machineGunFlag)
			{
				if (curState.stateName != AISTATE.MACHINEGUN)
				{
					ChangeState(AISTATE.MACHINEGUN);
				}
			}
			else
			{
				ChangeState(AISTATE.FIGHTREADY);
			}
			chasingStartPos = base.transform.position;
			chasingStartFlag = true;
			return;
		}
		alarmFlag = true;
		if (curState.stateName == AISTATE.AFRAID || curState.stateName == AISTATE.DIE)
		{
			return;
		}
		if (attackFlag)
		{
			if (machineGunFlag)
			{
				if (curState.stateName != AISTATE.MACHINEGUN)
				{
					ChangeState(AISTATE.MACHINEGUN);
				}
			}
			else if (handGunFlag)
			{
				if (curState.stateName != AISTATE.HANDGUN)
				{
					ChangeState(AISTATE.HANDGUN);
				}
			}
			else if (curState.stateName != AISTATE.FIGHTREADY)
			{
				ChangeState(AISTATE.FIGHTREADY);
			}
		}
		else if (curState.stateName != AISTATE.AFRAID)
		{
			ChangeState(AISTATE.AFRAID);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("PlayerCar"))
		{
			return;
		}
		VechicleController component = other.gameObject.GetComponent<VechicleController>();
		if (component.currentSpeed > 20f || component.currentSpeed < -20f)
		{
			if (Mathf.Abs(ToolFunction.InverseX(component.transform.position, base.transform.position, component.transform.right)) < 0.65f)
			{
				killedByCarFlag = true;
				GlobalInf.carKillNum++;
				OnDie();
				AudioController.instance.play(AudioType.CAR_HIT_HUMAN);
				if (type == NPCTYPE.NORMALWOMEN || type == NPCTYPE.NORMALWOMEN2)
				{
					AudioController.instance.play(AudioType.WOMENSCREAM);
				}
				else
				{
					AudioController.instance.play(AudioType.MANSCREAM);
				}
			}
			else if (!attackedFlag)
			{
				attackedFlag = true;
				if (!attackFlag)
				{
					PoliceLevelCtl.ScratchPerson();
				}
				dodgeState.carPos = component.transform;
				ChangeState(AISTATE.DODGE);
			}
			return;
		}
		if (!attackedFlag)
		{
			attackedFlag = true;
			if (!attackFlag)
			{
				PoliceLevelCtl.ScratchPerson();
			}
		}
		dodgeState.carPos = component.transform;
		ChangeState(AISTATE.DODGE);
		if (type == NPCTYPE.NORMALWOMEN || type == NPCTYPE.NORMALWOMEN2)
		{
			AudioController.instance.play(AudioType.WOMENSCREAM);
		}
		else
		{
			AudioController.instance.play(AudioType.MANSCREAM);
		}
	}
}
