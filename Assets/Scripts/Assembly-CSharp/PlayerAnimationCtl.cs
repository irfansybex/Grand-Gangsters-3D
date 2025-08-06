using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationCtl : MonoBehaviour
{
	public PlayerController playerCtl;

	public PlayerMovementCtl player;

	public PLAYERACTIONSTATE curActionState;

	public PLAYERACTIONSTATE preActionState;

	public PLAYERSTATE curState;

	public PLAYERSTATE preState;

	public AnimationClip idle0;

	public AnimationClip idle1;

	public AnimationClip walk;

	public AnimationClip run;

	public AnimationClip dieClip;

	public AnimationClip fightIdle;

	public AnimationClip fightWalk;

	public AnimationClip fight0;

	public AnimationClip fight1;

	public AnimationClip fight2;

	public AnimationClip fight3;

	public AnimationClip beatenLeft;

	public AnimationClip beatenRight;

	public AnimationClip handGunIdle;

	public AnimationClip up;

	public AnimationClip down;

	public AnimationClip handGunShot;

	public AnimationClip handGunReload;

	public AnimationClip machineGunIdle;

	public AnimationClip machineGunAimIdle;

	public AnimationClip machineGunShot;

	public AnimationClip machineGunReload;

	public AnimationClip machineGunRun;

	public AnimationClip enterCarClip;

	public AnimationClip exitCarClip;

	public AnimationClip robCarClip;

	public AnimationClip enterMotorClip;

	public AnimationClip exitMotorClip;

	public Transform upArm;

	public Transform gunDriection;

	public Transform shoulder;

	public Transform upShoulder;

	public Transform downShoulder;

	public Transform head;

	public Transform waist;

	public Transform back;

	public Transform machineGunShotWaist;

	public Transform handGunShotWaist;

	public LayerMask layer;

	public float lerpSpeed;

	private RaycastHit hit;

	public Quaternion defaultHandRotation;

	public Quaternion defaultHeadRotation;

	public bool setDefaultRotationFlag;

	public bool isAimFlag;

	public bool needToAimFlag;

	private bool fadeOutAimAnimaFlag;

	private float fadeOutAimAnimaPercent;

	private Quaternion preHandRotation;

	private Quaternion preHeadRotation;

	private Quaternion preShoulderRotation;

	public Vector3 hitDir;

	private Quaternion hitRotation;

	private float hitYAngle;

	private float shoulderPercent;

	public Quaternion defaultWaistRotation;

	private Quaternion preWaistRotation;

	public Quaternion defaultBackRotation;

	public bool fightFlag;

	public float fightStateWaitTime;

	public float fightComboTime;

	public float fightInterval;

	public bool comboFlag;

	private float countFightComboTime;

	private float countFightStateTime;

	public float countFightInterval;

	public FIGHTCOMBOSTATE comboState;

	private OneLengthQueue<AnimationClip> fightQueue;

	private Animation anima;

	public float difAngle;

	public Transform rootBone;

	public Transform upperBodyBone;

	public float maxIdleSpeed = 0.5f;

	public float minWalkSpeed = 1f;

	public MoveAnimation[] moveAnimations;

	public MoveAnimation[] machineGunmoveAnimations;

	private MoveAnimation machineGunpreBestAnima;

	private Transform tr;

	private Vector3 lastPosition = Vector3.zero;

	private Vector3 velocity = Vector3.zero;

	private Vector3 localVelocity = Vector3.zero;

	private float speed;

	private float localVelocityAngle;

	private float lowerBodyDeltaAngle;

	public float idleWeight;

	private Vector3 lowerBodyForwardTarget = Vector3.forward;

	private Vector3 lowerBodyForward = Vector3.forward;

	private MoveAnimation bestAnimation;

	public SkinnedMeshRenderer playerSkinnedMesh;

	public Quaternion defaultHandRotationL;

	public Quaternion defaultHeadRotationL;

	public Transform upArmL;

	public bool reloadFlag;

	public bool aimConditionFlag;

	public bool handGunMoveFlag;

	public bool machineGunMoveFlag;

	private float smallestDiff;

	private float angleDiff;

	private float diff;

	public Text checkChange;

	public int n;

	public bool turnFlag;

	public bool firstFlag;

	private bool getOffCarFlag;

	public Text ag;

	public Text lable;

	public float fightMoveSpeed;

	public MyAnimaQue myAnimaQue;

	public void HandGunAwake()
	{
		anima[handGunIdle.name].layer = 2;
		defaultWaistRotation = GetDefaultRotation(handGunIdle, waist);
		defaultBackRotation = GetDefaultRotation(handGunIdle, back);
		anima[handGunShot.name].layer = 3;
		anima[handGunShot.name].blendMode = AnimationBlendMode.Additive;
		anima[handGunShot.name].AddMixingTransform(handGunShotWaist);
		anima[handGunReload.name].layer = 4;
		anima[handGunReload.name].AddMixingTransform(waist);
		anima[handGunReload.name].speed = anima[handGunReload.name].length / playerCtl.gun.gunInfo.reloadTime;
		anima[handGunIdle.name].layer = 1;
		for (int i = 0; i < moveAnimations.Length; i++)
		{
			moveAnimations[i].Init();
			anima[moveAnimations[i].clip.name].layer = 1;
		}
		GunCtl gun = playerCtl.gun;
		gun.onReload = (GunCtl.OnReload)Delegate.Combine(gun.onReload, new GunCtl.OnReload(OnReload));
	}

	private void OnReload()
	{
		if (curState == PLAYERSTATE.HANDGUN)
		{
			anima[handGunReload.name].enabled = true;
			anima.CrossFade(handGunReload.name);
			reloadFlag = true;
		}
		else if (curState == PLAYERSTATE.MACHINEGUN)
		{
			anima[machineGunReload.name].enabled = true;
			anima.CrossFade(machineGunReload.name);
			reloadFlag = true;
		}
	}

	private void NormalAwake()
	{
		anima[walk.name].layer = 1;
		anima[run.name].layer = 1;
		anima[idle0.name].layer = 1;
		anima[idle0.name].enabled = true;
		anima[idle0.name].weight = 1f;
		anima[dieClip.name].layer = 9;
	}

	private void FightAwake()
	{
		anima[fightWalk.name].layer = 1;
		anima[fightIdle.name].layer = 1;
		anima[fight0.name].layer = 2;
		anima[fight0.name].speed = 0.7f;
		anima[fight0.name].weight = 1f;
		anima[fight1.name].layer = 2;
		anima[fight1.name].weight = 1f;
		anima[fight1.name].speed = 0.7f;
		anima[fight2.name].layer = 2;
		anima[fight2.name].weight = 1f;
		anima[fight2.name].speed = 0.6f;
		anima[fight3.name].layer = 2;
		anima[fight3.name].weight = 1f;
		anima[fight3.name].speed = 0.7f;
		anima[beatenLeft.name].layer = 3;
		anima[beatenLeft.name].AddMixingTransform(waist);
		anima[beatenRight.name].layer = 5;
		anima[beatenRight.name].AddMixingTransform(waist);
		anima[beatenRight.name].blendMode = AnimationBlendMode.Additive;
	}

	public void MachineGunAwake()
	{
		anima[machineGunIdle.name].layer = 1;
		anima[machineGunRun.name].layer = 1;
		anima[machineGunAimIdle.name].layer = 1;
		defaultWaistRotation = GetDefaultRotation(machineGunAimIdle, waist);
		defaultBackRotation = GetDefaultRotation(machineGunAimIdle, back);
		anima[machineGunShot.name].layer = 3;
		anima[machineGunShot.name].blendMode = AnimationBlendMode.Additive;
		anima[machineGunShot.name].AddMixingTransform(machineGunShotWaist);
		anima[machineGunReload.name].layer = 4;
		anima[machineGunReload.name].AddMixingTransform(waist);
		anima[machineGunReload.name].speed = anima[machineGunReload.name].length / playerCtl.machineGun.gunInfo.reloadTime;
		for (int i = 0; i < machineGunmoveAnimations.Length; i++)
		{
			machineGunmoveAnimations[i].Init();
			anima[machineGunmoveAnimations[i].clip.name].layer = 1;
		}
		GunCtl machineGun = playerCtl.machineGun;
		machineGun.onReload = (GunCtl.OnReload)Delegate.Combine(machineGun.onReload, new GunCtl.OnReload(OnReload));
	}

	private void Awake()
	{
		anima = base.GetComponent<Animation>();
	}

	public void AwakeInit()
	{
		tr = player.gameObject.transform;
		if (playerCtl.gun != null)
		{
			HandGunAwake();
		}
		NormalAwake();
		if (playerCtl.machineGun != null)
		{
			MachineGunAwake();
		}
		FightAwake();
	}

	private void Update()
	{
		CheckAnimaState();
		if (needToAimFlag && playerCtl.fireTarget != null)
		{
			hitDir = playerCtl.fireTarget.fireTarget.transform.position - gunDriection.position;
			difAngle = GetXZEulerAngle(gunDriection.forward, hitDir, Vector3.up);
			if (difAngle > 20f)
			{
				aimConditionFlag = false;
			}
			else if (difAngle < -30f)
			{
				aimConditionFlag = false;
			}
			if (Mathf.Abs(difAngle) < 10f)
			{
				aimConditionFlag = true;
			}
			if (setDefaultRotationFlag && aimConditionFlag)
			{
				setDefaultRotationFlag = false;
				isAimFlag = true;
				firstFlag = true;
			}
		}
		if (playerCtl.fireTarget != null)
		{
			hitDir = playerCtl.fireTarget.fireTarget.transform.position - gunDriection.position;
			difAngle = GetXZEulerAngle(gunDriection.forward, hitDir, Vector3.up);
		}
		if (fadeOutAimAnimaFlag)
		{
			if (curState == PLAYERSTATE.HANDGUN)
			{
				if (fadeOutAimAnimaPercent <= 0f)
				{
					fadeOutAimAnimaFlag = false;
					anima[handGunIdle.name].enabled = false;
				}
				fadeOutAimAnimaPercent -= Time.deltaTime * 3f;
				anima[handGunIdle.name].weight = Mathf.Clamp01(fadeOutAimAnimaPercent);
			}
			else if (curState == PLAYERSTATE.MACHINEGUN)
			{
				if (fadeOutAimAnimaPercent <= 0f)
				{
					fadeOutAimAnimaFlag = false;
					anima[machineGunAimIdle.name].enabled = false;
				}
				fadeOutAimAnimaPercent -= Time.deltaTime * 3f;
				anima[machineGunAimIdle.name].weight = Mathf.Clamp01(fadeOutAimAnimaPercent);
			}
		}
		if (curState == PLAYERSTATE.FIGHT)
		{
			countFightInterval += Time.deltaTime;
		}
		if (getOffCarFlag && (anima[exitCarClip.name].normalizedTime > 0.8f || anima[exitMotorClip.name].normalizedTime > 0.8f))
		{
			getOffCarFlag = false;
			playerCtl.GetOffCarDone();
		}
	}

	public void OnReloadDone()
	{
		reloadFlag = false;
	}

	private void LateUpdate()
	{
		if (!isAimFlag)
		{
			return;
		}
		if (curState == PLAYERSTATE.HANDGUN)
		{
			HandGunLateUpdate();
			if (!reloadFlag && (aimConditionFlag || handGunMoveFlag))
			{
				float xZEulerAngle = GetXZEulerAngle(gunDriection.forward, hitDir, gunDriection.right);
				Quaternion quaternion = Quaternion.Euler(0f, 0f, 0f - xZEulerAngle);
				back.rotation *= quaternion;
			}
		}
		else if (curState == PLAYERSTATE.MACHINEGUN)
		{
			MachineGunLateUpdate();
			if (!reloadFlag && (aimConditionFlag || machineGunMoveFlag))
			{
				float xZEulerAngle2 = GetXZEulerAngle(gunDriection.forward, hitDir, gunDriection.right);
				Quaternion quaternion2 = Quaternion.Euler(0f, 0f, 0f - xZEulerAngle2);
				back.rotation *= quaternion2;
			}
		}
	}

	private void HandGunFixedUpdate()
	{
		velocity = (tr.position - lastPosition) / Time.fixedDeltaTime;
		localVelocity = tr.InverseTransformDirection(velocity);
		localVelocity.y = 0f;
		speed = localVelocity.magnitude;
		localVelocityAngle = MoveAnimation.HorizontalAngle(localVelocity);
		lastPosition = tr.position;
	}

	private void FixedUpdate()
	{
		if (needToAimFlag)
		{
			HandGunFixedUpdate();
		}
		FightMove();
	}

	private void HandGunUpdate()
	{
		idleWeight = Mathf.Lerp(idleWeight, Mathf.InverseLerp(minWalkSpeed, maxIdleSpeed, speed), Time.deltaTime * 10f);
		if (idleWeight >= 0.9f)
		{
			curActionState = PLAYERACTIONSTATE.IDLE;
		}
		else
		{
			curActionState = PLAYERACTIONSTATE.WALK;
		}
		if (speed < 0.1f)
		{
			bestAnimation = null;
			anima.CrossFade(handGunIdle.name);
			return;
		}
		smallestDiff = float.PositiveInfinity;
		for (int i = 0; i < moveAnimations.Length; i++)
		{
			angleDiff = Mathf.Abs(Mathf.DeltaAngle(localVelocityAngle, moveAnimations[i].angle));
			diff = angleDiff;
			if (moveAnimations[i] == bestAnimation)
			{
				diff *= 0.9f;
			}
			if (diff < smallestDiff)
			{
				bestAnimation = moveAnimations[i];
				smallestDiff = diff;
			}
		}
		anima.CrossFade(bestAnimation.clip.name, 0.7f);
	}

	private void MachineGunUpdate()
	{
		idleWeight = Mathf.Lerp(idleWeight, Mathf.InverseLerp(minWalkSpeed, maxIdleSpeed, speed), Time.deltaTime * 10f);
		if (idleWeight >= 0.9f)
		{
			curActionState = PLAYERACTIONSTATE.IDLE;
		}
		else
		{
			curActionState = PLAYERACTIONSTATE.WALK;
		}
		if (speed <= 0.1f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.01f && Mathf.Abs(Input.GetAxis("Vertical")) < 0.01f)
		{
			bestAnimation = null;
			anima.CrossFade(machineGunAimIdle.name);
			return;
		}
		smallestDiff = float.PositiveInfinity;
		for (int i = 0; i < machineGunmoveAnimations.Length; i++)
		{
			angleDiff = Mathf.Abs(Mathf.DeltaAngle(localVelocityAngle, machineGunmoveAnimations[i].angle));
			diff = angleDiff;
			if (machineGunmoveAnimations[i] == bestAnimation)
			{
				diff *= 0.9f;
			}
			if (diff < smallestDiff)
			{
				bestAnimation = machineGunmoveAnimations[i];
				smallestDiff = diff;
			}
		}
		anima.CrossFade(bestAnimation.clip.name, 0.7f);
	}

	private void HandGunLateUpdate()
	{
		float num = Mathf.InverseLerp(minWalkSpeed, maxIdleSpeed, speed);
		if (firstFlag)
		{
			lastPosition = tr.position;
			firstFlag = false;
			lowerBodyForward = tr.forward;
			lowerBodyForwardTarget = tr.forward;
		}
		if (num < 1f)
		{
			handGunMoveFlag = true;
			if (anima[handGunIdle.name].weight < 0.2f)
			{
				Vector3 zero = Vector3.zero;
				for (int i = 0; i < moveAnimations.Length; i++)
				{
					if (anima[moveAnimations[i].clip.name].weight != 0f && !(Vector3.Dot(moveAnimations[i].velocity, localVelocity) <= 0f))
					{
						zero += moveAnimations[i].velocity * anima[moveAnimations[i].clip.name].weight;
					}
				}
				float b = Mathf.DeltaAngle(MoveAnimation.HorizontalAngle(tr.rotation * zero), MoveAnimation.HorizontalAngle(velocity));
				lowerBodyDeltaAngle = Mathf.LerpAngle(lowerBodyDeltaAngle, b, Time.deltaTime * 10f);
				lowerBodyForwardTarget = tr.forward;
				lowerBodyForward = Quaternion.Euler(0f, lowerBodyDeltaAngle, 0f) * lowerBodyForwardTarget;
			}
		}
		else
		{
			handGunMoveFlag = false;
			lowerBodyForward = Vector3.RotateTowards(lowerBodyForward, lowerBodyForwardTarget, Time.deltaTime * 300f * ((float)Math.PI / 180f), 1f);
			lowerBodyDeltaAngle = Mathf.DeltaAngle(MoveAnimation.HorizontalAngle(tr.forward), MoveAnimation.HorizontalAngle(lowerBodyForward));
			if (lowerBodyDeltaAngle > 20f || lowerBodyDeltaAngle < -25f)
			{
				lowerBodyForwardTarget = tr.forward;
			}
		}
		Quaternion quaternion = Quaternion.Euler(0f, lowerBodyDeltaAngle, 0f);
		rootBone.rotation = quaternion * rootBone.rotation;
		waist.rotation = Quaternion.Inverse(quaternion) * waist.rotation;
	}

	public void MachineGunLateUpdate()
	{
		float num = Mathf.InverseLerp(minWalkSpeed, maxIdleSpeed, speed);
		if (firstFlag)
		{
			lastPosition = tr.position;
			firstFlag = false;
			lowerBodyForward = tr.forward;
			lowerBodyForwardTarget = tr.forward;
		}
		if (num < 1f)
		{
			machineGunMoveFlag = true;
			if (anima[machineGunAimIdle.name].weight < 0.2f)
			{
				Vector3 zero = Vector3.zero;
				for (int i = 0; i < machineGunmoveAnimations.Length; i++)
				{
					if (anima[machineGunmoveAnimations[i].clip.name].weight != 0f && !(Vector3.Dot(machineGunmoveAnimations[i].velocity, localVelocity) <= 0f))
					{
						zero += machineGunmoveAnimations[i].velocity * anima[machineGunmoveAnimations[i].clip.name].weight;
					}
				}
				float b = Mathf.DeltaAngle(MoveAnimation.HorizontalAngle(tr.rotation * zero), MoveAnimation.HorizontalAngle(velocity));
				lowerBodyDeltaAngle = Mathf.LerpAngle(lowerBodyDeltaAngle, b, Time.deltaTime * 10f);
				lowerBodyForwardTarget = tr.forward;
				lowerBodyForward = Quaternion.Euler(0f, lowerBodyDeltaAngle, 0f) * lowerBodyForwardTarget;
			}
		}
		else
		{
			machineGunMoveFlag = false;
			lowerBodyForward = Vector3.RotateTowards(lowerBodyForward, lowerBodyForwardTarget, Time.deltaTime * 300f * ((float)Math.PI / 180f), 1f);
			lowerBodyDeltaAngle = Mathf.DeltaAngle(MoveAnimation.HorizontalAngle(tr.forward), MoveAnimation.HorizontalAngle(lowerBodyForward));
			if (lowerBodyDeltaAngle > 20f || lowerBodyDeltaAngle < -25f)
			{
				lowerBodyForwardTarget = tr.forward;
			}
		}
		Quaternion quaternion = Quaternion.Euler(0f, lowerBodyDeltaAngle, 0f);
		rootBone.rotation = quaternion * rootBone.rotation;
		waist.rotation = Quaternion.Inverse(quaternion) * waist.rotation;
	}

	public void OnChangeAimState(bool isAim)
	{
		if (curState == PLAYERSTATE.HANDGUN)
		{
			if (isAim)
			{
				needToAimFlag = true;
				setDefaultRotationFlag = true;
				return;
			}
			needToAimFlag = false;
			isAimFlag = false;
			fadeOutAimAnimaFlag = true;
			fadeOutAimAnimaPercent = 1f;
			aimConditionFlag = false;
			if (anima != null)
			{
				anima.CrossFade(idle0.name);
			}
		}
		else
		{
			if (curState != PLAYERSTATE.MACHINEGUN)
			{
				return;
			}
			if (isAim)
			{
				needToAimFlag = true;
				setDefaultRotationFlag = true;
				return;
			}
			needToAimFlag = false;
			isAimFlag = false;
			fadeOutAimAnimaFlag = true;
			fadeOutAimAnimaPercent = 1f;
			aimConditionFlag = false;
			if (anima != null)
			{
				anima.CrossFade(machineGunIdle.name);
			}
		}
	}

	private void GetUpShoulderRotation(AnimationClip clip, Transform data)
	{
		base.GetComponent<Animation>()[clip.name].normalizedTime = 0.5f;
		base.GetComponent<Animation>()[clip.name].enabled = true;
		base.GetComponent<Animation>()[clip.name].weight = 1f;
		base.GetComponent<Animation>().Sample();
		data.transform.localRotation = shoulder.transform.localRotation;
		base.GetComponent<Animation>()[clip.name].weight = 0f;
		base.GetComponent<Animation>()[clip.name].enabled = false;
	}

	private Quaternion GetDefaultRotation(AnimationClip clip, Transform pos)
	{
		base.GetComponent<Animation>()[clip.name].normalizedTime = 0.5f;
		base.GetComponent<Animation>()[clip.name].enabled = true;
		base.GetComponent<Animation>()[clip.name].weight = 1f;
		base.GetComponent<Animation>().Sample();
		Quaternion rotation = pos.transform.rotation;
		base.GetComponent<Animation>()[clip.name].weight = 0f;
		base.GetComponent<Animation>()[clip.name].enabled = false;
		return rotation;
	}

	public void OnChangePlayerState(PLAYERSTATE newState)
	{
		preState = curState;
		curState = newState;
		switch (preState)
		{
		case PLAYERSTATE.HANDGUN:
			ExitHandGunState();
			break;
		case PLAYERSTATE.MACHINEGUN:
			ExitMachineGunState();
			break;
		case PLAYERSTATE.NORMAL:
			ExitNormalState();
			break;
		case PLAYERSTATE.FIGHT:
			ExitFightState();
			break;
		}
		switch (curState)
		{
		case PLAYERSTATE.NORMAL:
			EnterNormalState();
			break;
		case PLAYERSTATE.HANDGUN:
			EnterHandGunNormal();
			break;
		case PLAYERSTATE.MACHINEGUN:
			EnterMachineGunState();
			break;
		case PLAYERSTATE.FIGHT:
			EnterFightState();
			break;
		case PLAYERSTATE.DIE:
			EnterDieState();
			break;
		case PLAYERSTATE.CAR:
			EnterCarState();
			break;
		}
		curActionState = PLAYERACTIONSTATE.IDLE;
	}

	private void InvokeCarLaunch()
	{
		AudioController.instance.play(AudioType.CAR_LAUNCH);
	}

	private void InvokeEngine()
	{
		AudioController.instance.play(AudioType.ENGINE);
	}

	public void EnterCarState()
	{
		if (!playerCtl.car.AICarCtl.moveFlag)
		{
			if (!playerCtl.car.motorFlag)
			{
				anima.CrossFade(enterCarClip.name, 0.3f, PlayMode.StopAll);
			}
			else
			{
				anima.CrossFade(enterMotorClip.name, 0.3f, PlayMode.StopAll);
			}
			Invoke("InvokeCarLaunch", 0.5f);
			Invoke("InvokeEngine", 1.3f);
		}
		else
		{
			if (!playerCtl.car.motorFlag)
			{
				anima.CrossFade(robCarClip.name, 0.3f, PlayMode.StopAll);
			}
			Invoke("InvokeEngine", 0.5f);
		}
	}

	public void GetOffCarAnimaClipPlay()
	{
		if (!playerCtl.car.motorFlag)
		{
			anima.CrossFade(exitCarClip.name);
		}
		else
		{
			anima.CrossFade(exitMotorClip.name);
		}
		getOffCarFlag = true;
	}

	public void EnterDieState()
	{
		anima.CrossFade(dieClip.name);
	}

	public void EnterMachineGunState()
	{
		playerCtl.machineGun.gameObject.SetActiveRecursively(true);
		playerCtl.machineGun.muzzleFlash.SetActiveRecursively(false);
		anima.CrossFade(idle0.name);
		anima[machineGunIdle.name].enabled = true;
		anima.CrossFade(machineGunIdle.name);
	}

	public void ExitMachineGunState()
	{
		playerCtl.machineGun.gameObject.SetActiveRecursively(false);
		aimConditionFlag = false;
		isAimFlag = false;
		needToAimFlag = false;
	}

	public void EnterHandGunNormal()
	{
		anima.CrossFade(idle0.name, 0.3f, PlayMode.StopAll);
		playerCtl.gun.gameObject.SetActiveRecursively(true);
		playerCtl.gun.muzzleFlash.SetActiveRecursively(false);
	}

	public void ExitHandGunState()
	{
		playerCtl.gun.gameObject.SetActiveRecursively(false);
		aimConditionFlag = false;
		isAimFlag = false;
		needToAimFlag = false;
	}

	public void EnterNormalState()
	{
		anima.CrossFade(idle0.name, 0.3f, PlayMode.StopAll);
	}

	public void ExitNormalState()
	{
		fightFlag = false;
		comboFlag = false;
		comboState = FIGHTCOMBOSTATE.NONE;
	}

	public void EnterFightState()
	{
		countFightStateTime = 0f;
		anima.CrossFade(fightIdle.name, 0.3f, PlayMode.StopAll);
	}

	public void ExitFightState()
	{
		fightFlag = false;
		comboFlag = false;
		comboState = FIGHTCOMBOSTATE.NONE;
		myAnimaQue.queue.Clear();
	}

	private float GetXZEulerAngle(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA -= Vector3.Project(dirA, axis);
		dirB -= Vector3.Project(dirB, axis);
		float num = Vector3.Angle(dirA, dirB);
		return num * (float)((!(Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0f)) ? 1 : (-1));
	}

	public Quaternion LerpToTarget(Quaternion sourceRotation, Quaternion targetRotation, bool isShoulder)
	{
		if (isShoulder)
		{
			return Quaternion.Lerp(sourceRotation, targetRotation, Time.deltaTime * lerpSpeed * 0.5f);
		}
		return Quaternion.Lerp(sourceRotation, targetRotation, Time.deltaTime * lerpSpeed);
	}

	private void CheckAnimaState()
	{
		switch (curState)
		{
		case PLAYERSTATE.NORMAL:
			NormalStateCheck();
			break;
		case PLAYERSTATE.HANDGUN:
			HandGunStateCheck();
			break;
		case PLAYERSTATE.MACHINEGUN:
			MachineGunStateCheck();
			break;
		case PLAYERSTATE.FIGHT:
			FightStateCheck();
			break;
		case PLAYERSTATE.CAR:
			break;
		}
	}

	public void MachineGunStateCheck()
	{
		if (!needToAimFlag)
		{
			if (player.GetComponent<Rigidbody>().velocity.sqrMagnitude > 4f)
			{
				if (curActionState != PLAYERACTIONSTATE.RUN)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.RUN);
				}
			}
			else if (player.GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.01f)
			{
				if (curActionState != PLAYERACTIONSTATE.WALK)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.WALK);
				}
			}
			else if (player.joyStick.position.x < 0.001f && player.joyStick.position.y < 0.01f)
			{
				if (curActionState != 0)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.IDLE);
				}
			}
			else if (curActionState != PLAYERACTIONSTATE.WALK)
			{
				ChangePlayerActionState(PLAYERACTIONSTATE.WALK);
			}
		}
		else
		{
			MachineGunUpdate();
		}
	}

	public void NormalStateCheck()
	{
		if (!fightFlag)
		{
			if (player.GetComponent<Rigidbody>().velocity.sqrMagnitude > 4f)
			{
				if (curActionState != PLAYERACTIONSTATE.RUN)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.RUN);
				}
			}
			else if (player.GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.01f)
			{
				if (curActionState != PLAYERACTIONSTATE.WALK)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.WALK);
				}
			}
			else if (player.joyStick.position.x < 0.001f && player.joyStick.position.y < 0.01f)
			{
				if (curActionState != 0)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.IDLE);
				}
			}
			else if (curActionState != PLAYERACTIONSTATE.WALK)
			{
				ChangePlayerActionState(PLAYERACTIONSTATE.WALK);
			}
		}
		else
		{
			CheckFightState();
		}
	}

	public void FightStateCheck()
	{
		countFightStateTime += Time.deltaTime;
		if (countFightStateTime > fightStateWaitTime)
		{
			playerCtl.ChangeState(PLAYERSTATE.NORMAL);
		}
		if (!fightFlag)
		{
			if (player.GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.01f)
			{
				if (curActionState != PLAYERACTIONSTATE.WALK)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.WALK);
				}
			}
			else if (player.joyStick.position.x < 0.001f && player.joyStick.position.y < 0.01f)
			{
				if (curActionState != 0)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.IDLE);
				}
			}
			else if (curActionState != PLAYERACTIONSTATE.WALK && !playerCtl.punchedFlag)
			{
				ChangePlayerActionState(PLAYERACTIONSTATE.WALK);
			}
		}
		else
		{
			CheckFightState();
		}
	}

	public void HandGunStateCheck()
	{
		if (!needToAimFlag)
		{
			if (player.GetComponent<Rigidbody>().velocity.sqrMagnitude > 4f)
			{
				if (curActionState != PLAYERACTIONSTATE.RUN)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.RUN);
				}
			}
			else if (player.GetComponent<Rigidbody>().velocity.sqrMagnitude > 0.01f)
			{
				if (curActionState != PLAYERACTIONSTATE.WALK)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.WALK);
				}
			}
			else if (player.joyStick.position.x < 0.001f && player.joyStick.position.y < 0.01f)
			{
				if (curActionState != 0)
				{
					ChangePlayerActionState(PLAYERACTIONSTATE.IDLE);
				}
			}
			else if (curActionState != PLAYERACTIONSTATE.WALK)
			{
				ChangePlayerActionState(PLAYERACTIONSTATE.WALK);
			}
		}
		else
		{
			HandGunUpdate();
		}
	}

	public void ChangePlayerActionState(PLAYERACTIONSTATE newState)
	{
		curActionState = newState;
		if (curState == PLAYERSTATE.NORMAL)
		{
			switch (newState)
			{
			case PLAYERACTIONSTATE.IDLE:
				anima.CrossFade(idle0.name);
				break;
			case PLAYERACTIONSTATE.RUN:
				anima.CrossFade(run.name);
				break;
			case PLAYERACTIONSTATE.WALK:
				anima.CrossFade(walk.name);
				break;
			}
		}
		else if (curState == PLAYERSTATE.HANDGUN)
		{
			if (!isAimFlag)
			{
				switch (newState)
				{
				case PLAYERACTIONSTATE.IDLE:
					anima.CrossFade(idle0.name);
					break;
				case PLAYERACTIONSTATE.RUN:
					anima.CrossFade(run.name);
					break;
				case PLAYERACTIONSTATE.WALK:
					anima.CrossFade(walk.name);
					break;
				}
			}
		}
		else if (curState == PLAYERSTATE.MACHINEGUN)
		{
			if (!isAimFlag)
			{
				switch (newState)
				{
				case PLAYERACTIONSTATE.IDLE:
					anima.CrossFade(machineGunIdle.name);
					break;
				case PLAYERACTIONSTATE.RUN:
					anima.CrossFade(machineGunRun.name);
					break;
				case PLAYERACTIONSTATE.WALK:
					anima.CrossFade(machineGunRun.name);
					break;
				}
			}
		}
		else if (curState == PLAYERSTATE.FIGHT)
		{
			switch (newState)
			{
			case PLAYERACTIONSTATE.IDLE:
				anima.CrossFade(fightIdle.name);
				break;
			case PLAYERACTIONSTATE.WALK:
				anima.CrossFade(fightWalk.name);
				break;
			}
		}
	}

	public void OnClickFire()
	{
		if (curState == PLAYERSTATE.HANDGUN)
		{
			anima.Play(handGunShot.name);
		}
		else if (curState == PLAYERSTATE.NORMAL)
		{
			Fight();
		}
		else if (curState == PLAYERSTATE.MACHINEGUN)
		{
			anima.Play(machineGunShot.name);
		}
		else if (curState == PLAYERSTATE.FIGHT)
		{
			Fight();
		}
	}

	public void CheckFightState()
	{
		if (comboFlag)
		{
			countFightComboTime += Time.deltaTime;
			if (countFightComboTime > fightComboTime)
			{
				comboFlag = false;
				comboState = FIGHTCOMBOSTATE.NONE;
			}
		}
		if (fightFlag && myAnimaQue.queue.Count == 0)
		{
			fightFlag = false;
		}
	}

	public void FightMove()
	{
		if (myAnimaQue.queue.Count > 0)
		{
			if (anima[myAnimaQue.queue[0].name].normalizedTime < 0.45f)
			{
				fightMoveSpeed = Mathf.Lerp(fightMoveSpeed, 0f, Time.deltaTime * 10f);
				playerCtl.transform.position += playerCtl.transform.forward * fightMoveSpeed * Time.deltaTime;
			}
			else
			{
				fightMoveSpeed = 5f;
			}
		}
	}

	public void Fight()
	{
		if (comboState == FIGHTCOMBOSTATE.NONE)
		{
			comboState = FIGHTCOMBOSTATE.FIGHT0;
			fightFlag = true;
			comboFlag = true;
			countFightComboTime = 0f;
			countFightStateTime = 0f;
			countFightInterval = 0f;
			anima.CrossFade(fightIdle.name);
			myAnimaQue.AddAnima(fight0);
		}
		else if (comboState == FIGHTCOMBOSTATE.FIGHT0 && !anima.IsPlaying(fight3.name))
		{
			fightFlag = true;
			countFightComboTime = 0f;
			countFightStateTime = 0f;
			countFightInterval = 0f;
			comboState = FIGHTCOMBOSTATE.FIGHT1;
			myAnimaQue.AddAnima(fight1);
		}
		else if (comboState == FIGHTCOMBOSTATE.FIGHT1 && !anima.IsPlaying(fight0.name))
		{
			fightFlag = true;
			countFightComboTime = 0f;
			countFightStateTime = 0f;
			countFightInterval = 0f;
			comboState = FIGHTCOMBOSTATE.FIGHT2;
			myAnimaQue.AddAnima(fight2);
		}
		else if (comboState == FIGHTCOMBOSTATE.FIGHT2 && !anima.IsPlaying(fight1.name))
		{
			fightFlag = true;
			countFightComboTime = 0f;
			countFightStateTime = 0f;
			countFightInterval = 0f;
			comboState = FIGHTCOMBOSTATE.FIGHT3;
			myAnimaQue.AddAnima(fight3);
		}
		else if (comboState == FIGHTCOMBOSTATE.FIGHT3 && !anima.IsPlaying(fight2.name))
		{
			fightFlag = true;
			countFightComboTime = 0f;
			countFightStateTime = 0f;
			countFightInterval = 0f;
			comboState = FIGHTCOMBOSTATE.FIGHT0;
			myAnimaQue.AddAnima(fight0);
		}
	}

	public void OnPunched()
	{
		anima.CrossFade(beatenLeft.name);
		if (curActionState == PLAYERACTIONSTATE.WALK)
		{
			ChangePlayerActionState(PLAYERACTIONSTATE.IDLE);
		}
		countFightStateTime = 0f;
	}

	public void OnShotted()
	{
		if (PlayerController.instance.curState != PLAYERSTATE.CAR)
		{
			anima.Play(beatenRight.name);
		}
	}

	public void OnShottedInMotor()
	{
		anima.Play(beatenRight.name);
	}
}
