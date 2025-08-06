using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public delegate void OnGetOnCarDone();

	public delegate void OnGetOffCarDone();

	public delegate void ResetSence();

	public delegate void OnPlayerDie();

	public PlayerAnimationCtl animaCtl;

	public PlayerMovementCtl moveCtl;

	public PLAYERSTATE curState;

	public PLAYERSTATE preState;

	public RotateCamBtn exceptArea;

	public AIController preFireTarget;

	public AIController fireTarget;

	public Vector2 clickPos;

	public HealthController healCtl;

	public GunCtl gun;

	public GunCtl machineGun;

	public InputBehaviorCheck touchClick;

	public float punchDamageVal;

	public bool punchedFlag;

	public float punchedInterval;

	private float countPunchedInterval;

	public bool fightReadyFlag = true;

	public float fightInterval;

	public float fightAccuracy;

	private float countFightInterval;

	public Transform firedTarget;

	public Vector3 normalFiredPos;

	public bool shottedFlag;

	public float shottedInterval;

	private float countShottedInterval;

	public AIManage aiManage;

	public bool getOnCarFlag;

	public Transform getOnCarPos;

	public Vector3 getOnStartPos;

	public Quaternion getOnStartQua;

	public bool getOnCarLerpFlag;

	public float getOnCarLerpPercent;

	public VechicleController car;

	public CamRotate cam;

	public Transform rootPos;

	public OnGetOnCarDone onGetOnCarDone;

	public OnGetOffCarDone onGetOffCarDone;

	public bool changingTheCarFlag;

	public static PlayerController instance;

	public Transform handGunPos;

	public Transform machineGunPos;

	public ParticleSystem screenBlood;

	public GameObject occlusion;

	public MyPlayerRagdollController ragdollController;

	private Vector3 defaultHandGunPos;

	private Vector3 defaultMachineGunPos;

	private Vector3 defaultHandGunAngle;

	private Vector3 defaultMachineGunAngle;

	private Touch tempTouch;

	private bool touchFlag;

	private float hitTime;

	private float hitDamage;

	private AIController lastHitTarget;

	public bool toAimFlag;

	public ResetSence resetSence;

	public OnPlayerDie onPlayerDie;

	public CamChangeRoot camChangeRoot;

	public Transform deadTar;

	private VechicleController temCar;

	private float startGetOnCarTime;

	public GameObject playerShadow;

	public int occlusionCount;

	private Vector3 occlusionTar;

	public void CopyHandGunInfo(GunsInfo info)
	{
		if (GlobalInf.handGunInfo != null)
		{
			info.accuracy = GlobalInf.handGunInfo.accuracy;
			info.level = GlobalInf.handGunInfo.level;
			info.bulletNum = GlobalInf.handGunInfo.bulletNum;
			info.bulletSpeed = GlobalInf.handGunInfo.bulletSpeed;
			info.damage = GlobalInf.handGunInfo.damage;
			info.gunName = GlobalInf.handGunInfo.gunName;
			info.maxBulletNum = GlobalInf.handGunInfo.maxBulletNum;
			info.reloadTime = GlobalInf.handGunInfo.reloadTime;
			if (GlobalInf.handGunInfo.restBulletNum < 0)
			{
				GlobalInf.handGunInfo.restBulletNum = 0;
			}
			info.restBulletNum = GlobalInf.handGunInfo.restBulletNum;
			info.shotInterval = GlobalInf.handGunInfo.shotInterval;
		}
	}

	public void CopyMachineGunInfo(GunsInfo info)
	{
		if (GlobalInf.machineGunInfo != null)
		{
			info.accuracy = GlobalInf.machineGunInfo.accuracy;
			info.level = GlobalInf.machineGunInfo.level;
			info.bulletNum = GlobalInf.machineGunInfo.bulletNum;
			info.bulletSpeed = GlobalInf.machineGunInfo.bulletSpeed;
			info.damage = GlobalInf.machineGunInfo.damage;
			info.gunName = GlobalInf.machineGunInfo.gunName;
			info.maxBulletNum = GlobalInf.machineGunInfo.maxBulletNum;
			info.reloadTime = GlobalInf.machineGunInfo.reloadTime;
			if (GlobalInf.machineGunInfo.restBulletNum < 0)
			{
				GlobalInf.machineGunInfo.restBulletNum = 0;
			}
			info.restBulletNum = GlobalInf.machineGunInfo.restBulletNum;
			info.shotInterval = GlobalInf.machineGunInfo.shotInterval;
		}
	}

	private void Awake()
	{
		HealthController healthController = healCtl;
		healthController.OnDestroy = (HealthController.onDestroy)Delegate.Combine(healthController.OnDestroy, new HealthController.onDestroy(OnDie));
		defaultHandGunPos = handGunPos.localPosition;
		defaultHandGunAngle = handGunPos.localEulerAngles;
		defaultMachineGunPos = machineGunPos.localPosition;
		defaultMachineGunAngle = machineGunPos.localEulerAngles;
		if (instance == null)
		{
			instance = this;
		}
	}

	public void EnableHandGun()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("HandGun/HandGun" + GlobalInf.handgunIndex)) as GameObject;
		gun = gameObject.GetComponent<GunCtl>();
		CopyHandGunInfo(gun.gunInfo);
		if (gun.gunInfo.restBulletNum >= gun.gunInfo.bulletNum)
		{
			gun.gunInfo.curBulletNum = gun.gunInfo.bulletNum;
			gun.gunInfo.restBulletNum -= gun.gunInfo.bulletNum;
		}
		else
		{
			gun.gunInfo.curBulletNum = gun.gunInfo.restBulletNum;
			gun.gunInfo.restBulletNum = 0;
		}
		gameObject.transform.parent = handGunPos;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.gameObject.SetActiveRecursively(false);
		GunCtl gunCtl = gun;
		gunCtl.onReloadDone = (GunCtl.OnReloadDone)Delegate.Combine(gunCtl.onReloadDone, new GunCtl.OnReloadDone(OnReloadDone));
		gun.gunInfo.reloadTime = animaCtl.handGunReload.length / animaCtl.GetComponent<Animation>()[animaCtl.handGunReload.name].speed + 0.5f;
	}

	public void EnableMachineGun()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("MachineGun/MachineGun" + GlobalInf.machineGunIndex)) as GameObject;
		machineGun = gameObject.GetComponent<GunCtl>();
		CopyMachineGunInfo(machineGun.gunInfo);
		if (machineGun.gunInfo.restBulletNum >= machineGun.gunInfo.bulletNum)
		{
			machineGun.gunInfo.curBulletNum = machineGun.gunInfo.bulletNum;
			machineGun.gunInfo.restBulletNum = machineGun.gunInfo.restBulletNum - machineGun.gunInfo.bulletNum;
		}
		else
		{
			machineGun.gunInfo.curBulletNum = machineGun.gunInfo.restBulletNum;
			machineGun.gunInfo.restBulletNum = 0;
		}
		gameObject.transform.parent = machineGunPos;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.gameObject.SetActiveRecursively(false);
		GunCtl gunCtl = machineGun;
		gunCtl.onReloadDone = (GunCtl.OnReloadDone)Delegate.Combine(gunCtl.onReloadDone, new GunCtl.OnReloadDone(OnReloadDone));
	}

	private void Start()
	{
		if (GlobalInf.handgunIndex != -1)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("HandGun/HandGun" + GlobalInf.handgunIndex)) as GameObject;
			gun = gameObject.GetComponent<GunCtl>();
			CopyHandGunInfo(gun.gunInfo);
			if (gun.gunInfo.restBulletNum > gun.gunInfo.bulletNum)
			{
				gun.gunInfo.curBulletNum = gun.gunInfo.bulletNum;
				gun.gunInfo.restBulletNum -= gun.gunInfo.bulletNum;
			}
			else
			{
				gun.gunInfo.curBulletNum = gun.gunInfo.restBulletNum;
				gun.gunInfo.restBulletNum = 0;
			}
			gameObject.transform.parent = handGunPos;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.gameObject.SetActiveRecursively(false);
			GunCtl gunCtl = gun;
			gunCtl.onReloadDone = (GunCtl.OnReloadDone)Delegate.Combine(gunCtl.onReloadDone, new GunCtl.OnReloadDone(OnReloadDone));
		}
		else
		{
			gun = null;
		}
		if (GlobalInf.machineGunIndex != -1)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("MachineGun/MachineGun" + GlobalInf.machineGunIndex)) as GameObject;
			machineGun = gameObject2.GetComponent<GunCtl>();
			CopyMachineGunInfo(machineGun.gunInfo);
			if (machineGun.gunInfo.restBulletNum > machineGun.gunInfo.bulletNum)
			{
				machineGun.gunInfo.curBulletNum = machineGun.gunInfo.bulletNum;
				machineGun.gunInfo.restBulletNum = machineGun.gunInfo.restBulletNum - machineGun.gunInfo.bulletNum;
			}
			else
			{
				machineGun.gunInfo.curBulletNum = machineGun.gunInfo.restBulletNum;
				machineGun.gunInfo.restBulletNum = 0;
			}
			gameObject2.transform.parent = machineGunPos;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			gameObject2.gameObject.SetActiveRecursively(false);
			GunCtl gunCtl2 = machineGun;
			gunCtl2.onReloadDone = (GunCtl.OnReloadDone)Delegate.Combine(gunCtl2.onReloadDone, new GunCtl.OnReloadDone(OnReloadDone));
		}
		else
		{
			machineGun = null;
		}
		animaCtl.AwakeInit();
		MyPlayerRagdollController myPlayerRagdollController = ragdollController;
		myPlayerRagdollController.getUpDone = (MyPlayerRagdollController.GetUpDone)Delegate.Combine(myPlayerRagdollController.getUpDone, new MyPlayerRagdollController.GetUpDone(OnGetUpDone));
	}

	private void Update()
	{
		if (changingTheCarFlag)
		{
			startGetOnCarTime += Time.deltaTime;
			if (startGetOnCarTime >= 2.5f)
			{
				DirectGetOnCar();
			}
		}
		if (punchedFlag)
		{
			countPunchedInterval += Time.deltaTime;
			if (countPunchedInterval > punchedInterval)
			{
				punchedFlag = false;
				countPunchedInterval = 0f;
			}
		}
		if (shottedFlag)
		{
			countShottedInterval += Time.deltaTime;
			if (countShottedInterval > shottedInterval)
			{
				shottedFlag = false;
				countShottedInterval = 0f;
			}
		}
		if (curState == PLAYERSTATE.FIGHT && !fightReadyFlag)
		{
			countFightInterval += Time.deltaTime;
			if (countFightInterval > fightInterval)
			{
				fightReadyFlag = true;
			}
		}
		if (curState == PLAYERSTATE.CAR)
		{
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
		}
		if (!getOnCarLerpFlag)
		{
			return;
		}
		getOnCarLerpPercent += Time.deltaTime * 5f;
		base.transform.localPosition = Vector3.Lerp(getOnStartPos, Vector3.zero, getOnCarLerpPercent);
		base.transform.localRotation = Quaternion.Lerp(getOnStartQua, Quaternion.identity, getOnCarLerpPercent);
		if (getOnCarLerpPercent >= 1f)
		{
			getOnCarLerpFlag = false;
			cam.OnChangeTarget(true);
			if (!car.AICarCtl.moveFlag)
			{
				car.OnOpenDoor();
				Invoke("InvokeGetOnCar", 1.5f);
			}
			else
			{
				car.OnRobCarOpenDoor();
				Invoke("InvokeGetOnCar", 2.5f);
			}
		}
	}

	private void OnChangeToAimState()
	{
		animaCtl.OnChangeAimState(false);
	}

	private void OnChangeStateClick()
	{
		if (curState == PLAYERSTATE.NORMAL)
		{
			ChangeState(PLAYERSTATE.HANDGUN);
		}
		else if (curState == PLAYERSTATE.HANDGUN)
		{
			ChangeState(PLAYERSTATE.MACHINEGUN);
		}
		else if (curState == PLAYERSTATE.MACHINEGUN)
		{
			ChangeState(PLAYERSTATE.NORMAL);
		}
		else if (curState == PLAYERSTATE.FIGHT)
		{
			ChangeState(PLAYERSTATE.HANDGUN);
		}
	}

	public void OnChangeState(bool isLeft)
	{
		PLAYERSTATE pLAYERSTATE;
		if (isLeft)
		{
			if (curState == PLAYERSTATE.FIGHT)
			{
				curState = PLAYERSTATE.NORMAL;
			}
			pLAYERSTATE = (PLAYERSTATE)((int)(curState - 1 + 3) % 3);
			if (machineGun == null && pLAYERSTATE == PLAYERSTATE.MACHINEGUN)
			{
				pLAYERSTATE = (PLAYERSTATE)((int)(pLAYERSTATE - 1 + 3) % 3);
			}
			if (gun == null && pLAYERSTATE == PLAYERSTATE.HANDGUN)
			{
				pLAYERSTATE = (PLAYERSTATE)((int)(pLAYERSTATE - 1 + 3) % 3);
			}
		}
		else
		{
			if (curState == PLAYERSTATE.FIGHT)
			{
				curState = PLAYERSTATE.NORMAL;
			}
			pLAYERSTATE = (PLAYERSTATE)((int)(curState + 1) % 3);
			if (gun == null && pLAYERSTATE == PLAYERSTATE.HANDGUN)
			{
				pLAYERSTATE = (PLAYERSTATE)((int)(pLAYERSTATE + 1) % 3);
			}
			if (machineGun == null && pLAYERSTATE == PLAYERSTATE.MACHINEGUN)
			{
				pLAYERSTATE = (PLAYERSTATE)((int)(pLAYERSTATE + 1) % 3);
			}
		}
		if (pLAYERSTATE == PLAYERSTATE.HANDGUN || pLAYERSTATE == PLAYERSTATE.MACHINEGUN)
		{
			AudioController.instance.play(AudioType.RELOAD);
		}
		AudioController.instance.stop(AudioType.MACHINE_GUN);
		ChangeState(pLAYERSTATE);
	}

	public void ChangeState(PLAYERSTATE newState)
	{
		fireTarget = null;
		preState = curState;
		curState = newState;
		if (curState == PLAYERSTATE.NORMAL)
		{
			fightReadyFlag = true;
			firedTarget.transform.localPosition = normalFiredPos;
		}
		animaCtl.OnChangePlayerState(curState);
	}

	public void OnFireBtnClick()
	{
		if (shottedFlag)
		{
			return;
		}
		OnClickFire();
		if ((curState == PLAYERSTATE.NORMAL || curState == PLAYERSTATE.FIGHT) && fightReadyFlag && !punchedFlag)
		{
			fightReadyFlag = false;
			if (curState != PLAYERSTATE.FIGHT)
			{
				OnChangeFight();
			}
			if (fireTarget != null && (float)UnityEngine.Random.Range(0, 100) < fightAccuracy && (fireTarget.transform.position - base.transform.position).sqrMagnitude < 3f)
			{
				Invoke("OnPunchDone", 0.2f);
			}
			animaCtl.Fight();
		}
	}

	public void OnClickPlayer_BoxingBtn()
	{
		if (shottedFlag || (curState != 0 && curState != PLAYERSTATE.FIGHT) || !fightReadyFlag || punchedFlag)
		{
			return;
		}
		fightReadyFlag = false;
		if (curState != PLAYERSTATE.FIGHT)
		{
			OnChangeFight();
		}
		if (fireTarget == null)
		{
			if (preFireTarget != null)
			{
				fireTarget = preFireTarget;
			}
			else
			{
				fireTarget = CitySenceController.instance.GetPreFireTarget();
			}
		}
		if (fireTarget != null && (float)UnityEngine.Random.Range(0, 100) < fightAccuracy && (fireTarget.transform.position - base.transform.position).sqrMagnitude < 3f)
		{
			Invoke("OnPunchDone", 0.2f);
		}
		countFightInterval = 0f;
		animaCtl.Fight();
	}

	private void OnChangeFight()
	{
		preState = curState;
		curState = PLAYERSTATE.FIGHT;
		animaCtl.OnChangePlayerState(curState);
	}

	private void OnPunchDone()
	{
		if (fireTarget != null)
		{
			fireTarget.OnPunched(this, punchDamageVal);
		}
	}

	private void OnClickFire()
	{
		if (shottedFlag)
		{
			return;
		}
		if (curState == PLAYERSTATE.HANDGUN && fireTarget != null)
		{
			if (!animaCtl.needToAimFlag)
			{
				animaCtl.OnChangeAimState(true);
				return;
			}
			gun.PlayerShot(fireTarget.fireTarget.transform, fireTarget.gameObject);
		}
		if (gun.shotFlag && gun.gunInfo.curBulletNum != 0)
		{
			animaCtl.OnClickFire();
			if (gun.hitTargetFlag)
			{
				hitTime = (fireTarget.fireTarget.transform.position - gun.muzzlePos.position).magnitude / gun.gunInfo.bulletSpeed;
				hitDamage = gun.gunInfo.damage;
				Invoke("OnBulletAttack", hitTime);
				lastHitTarget = fireTarget;
			}
		}
		if (curState == PLAYERSTATE.MACHINEGUN && fireTarget != null)
		{
			if (!animaCtl.needToAimFlag)
			{
				animaCtl.OnChangeAimState(true);
				return;
			}
			machineGun.PlayerShot(fireTarget.fireTarget.transform, fireTarget.gameObject);
		}
		if (machineGun.shotFlag)
		{
			animaCtl.OnClickFire();
			if (machineGun.hitTargetFlag)
			{
				hitTime = (fireTarget.fireTarget.transform.position - machineGun.muzzlePos.position).magnitude / machineGun.gunInfo.bulletSpeed;
				hitDamage = machineGun.gunInfo.damage;
				Invoke("OnBulletAttack", hitTime);
				lastHitTarget = fireTarget;
			}
		}
	}

	public void InvokeShot()
	{
		toAimFlag = false;
	}

	public void OnPlayer_ShotBtnPress()
	{
		if (shottedFlag)
		{
			return;
		}
		if (curState == PLAYERSTATE.HANDGUN)
		{
			if (fireTarget == null && preFireTarget != null)
			{
				fireTarget = preFireTarget;
			}
			if (fireTarget != null)
			{
				if (!animaCtl.needToAimFlag)
				{
					animaCtl.OnChangeAimState(true);
					toAimFlag = true;
					Invoke("InvokeShot", 0.5f);
					return;
				}
				if (!toAimFlag && animaCtl.aimConditionFlag)
				{
					gun.PlayerShot(fireTarget.fireTarget.transform, fireTarget.gameObject);
				}
			}
		}
		if (gun != null && gun.shotFlag && gun.gunInfo.curBulletNum != 0)
		{
			animaCtl.OnClickFire();
			if (gun.hitTargetFlag)
			{
				hitTime = (fireTarget.fireTarget.transform.position - gun.muzzlePos.position).magnitude / gun.gunInfo.bulletSpeed;
				hitDamage = gun.gunInfo.damage;
				Invoke("OnBulletAttack", hitTime);
				lastHitTarget = fireTarget;
			}
		}
		if (curState == PLAYERSTATE.MACHINEGUN)
		{
			if (fireTarget == null && preFireTarget != null)
			{
				fireTarget = preFireTarget;
			}
			if (fireTarget != null)
			{
				if (!animaCtl.needToAimFlag)
				{
					animaCtl.OnChangeAimState(true);
					toAimFlag = true;
					Invoke("InvokeShot", 0.5f);
					return;
				}
				if (!toAimFlag && animaCtl.aimConditionFlag)
				{
					machineGun.PlayerShot(fireTarget.fireTarget.transform, fireTarget.gameObject);
				}
			}
		}
		if (machineGun != null && machineGun.shotFlag)
		{
			animaCtl.OnClickFire();
			if (machineGun.hitTargetFlag)
			{
				hitTime = (fireTarget.fireTarget.transform.position - machineGun.muzzlePos.position).magnitude / machineGun.gunInfo.bulletSpeed;
				hitDamage = machineGun.gunInfo.damage;
				Invoke("OnBulletAttack", hitTime);
				lastHitTarget = fireTarget;
			}
		}
	}

	private void OnBulletAttack()
	{
		lastHitTarget.OnShotted(this, hitDamage);
	}

	public void OnReloadDone()
	{
		if (curState == PLAYERSTATE.HANDGUN)
		{
			gun.reloadFlag = false;
			gun.bulletCount = 0;
			GameUIController.instance.ReflashHandGunBulletNum();
		}
		else if (curState == PLAYERSTATE.MACHINEGUN)
		{
			machineGun.reloadFlag = false;
			machineGun.bulletCount = 0;
			GameUIController.instance.ReflashMachineGunBulletNum();
		}
		animaCtl.OnReloadDone();
	}

	public void OnPunched(float damageVal, AIController ai)
	{
		if (curState != PLAYERSTATE.DIE)
		{
			punchedFlag = true;
			countPunchedInterval = 0f;
			animaCtl.OnPunched();
			if (curState == PLAYERSTATE.NORMAL)
			{
				OnChangeFight();
			}
			if (fireTarget == null)
			{
				fireTarget = ai;
			}
			healCtl.Damaged(damageVal);
			CitySenceController.instance.AlarmNPC();
			AudioController.instance.play(AudioType.PUNCH_PERSON);
		}
	}

	public void OnShotted(float damageVal, AIController ai)
	{
		if (curState == PLAYERSTATE.DIE)
		{
			return;
		}
		if (curState != PLAYERSTATE.CAR)
		{
			animaCtl.OnShotted();
			shottedFlag = true;
			countShottedInterval = 0f;
			healCtl.Damaged(damageVal);
			CitySenceController.instance.AlarmNPC();
			if (fireTarget == null && ai.visableRander.GetComponent<Renderer>().isVisible && car == null)
			{
				fireTarget = ai;
			}
		}
		else if (car != null)
		{
			if (car.carType == CARTYPE.MOTOR)
			{
				animaCtl.OnShottedInMotor();
			}
			car.Damage(damageVal);
		}
	}

	public void OnDie()
	{
		QuitGetOnCar();
		if (car != null && car.carType == CARTYPE.MOTOR)
		{
			cam.OnChangeTarget(false);
			base.transform.parent = null;
			moveCtl.enabled = true;
			moveCtl.playerFaceAngle = base.transform.eulerAngles.y;
			car.gameObject.layer = LayerMask.NameToLayer("Car");
			car.body.layer = LayerMask.NameToLayer("Car");
			car.OnDisableCar();
			if (car.AICarCtl.topCollider != null)
			{
				car.AICarCtl.topCollider.gameObject.layer = LayerMask.NameToLayer("Car");
			}
			car = null;
			AudioController.instance.stop(AudioType.ENGINE);
		}
		moveCtl.joyStick.position = Vector2.zero;
		GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
		GameUIController.instance.taskLabelUI.gameObject.SetActiveRecursively(false);
		GameUIController.instance.player_ShotPressFlag = false;
		camChangeRoot.StartChange(deadTar, 2f);
		ChangeState(PLAYERSTATE.DIE);
		base.GetComponent<Rigidbody>().velocity = Vector3.zero;
		base.GetComponent<Rigidbody>().GetComponent<Collider>().enabled = false;
		base.GetComponent<Rigidbody>().useGravity = false;
		Invoke("DelayResetSence", 4f);
		if (onPlayerDie != null)
		{
			onPlayerDie();
		}
		StoreDateController.SetCountData();
		GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.NONE);
		GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(false);
	}

	public void DelayResetSence()
	{
		GameSenceBackBtnCtl.instance.PopGameUIState();
		if (GameController.instance.curMode == GameController.instance.normalMode)
		{
			GameUIController.instance.OnDieUIEnable();
		}
		else
		{
			resetSence();
		}
	}

	public void ResetPlayer()
	{
		if (curState == PLAYERSTATE.DIE)
		{
			camChangeRoot.transform.position = camChangeRoot.preRoot.position;
			camChangeRoot.transform.rotation = camChangeRoot.preRoot.rotation;
			camChangeRoot.transform.parent = camChangeRoot.preRoot;
			camChangeRoot.targetRoot = camChangeRoot.preRoot;
			camChangeRoot.preRoot = deadTar;
			base.GetComponent<Rigidbody>().GetComponent<Collider>().enabled = true;
			base.GetComponent<Rigidbody>().useGravity = true;
		}
		if (curState == PLAYERSTATE.RAGDOLL)
		{
			ragdollController.state = MyPlayerRagdollController.RagdollState.animated;
			ragdollController.setKinematic(true);
			ragdollController.anim.enabled = true;
			OnGetUpDone();
			GlobalInf.playerRagdollFlag = false;
			cam.OnChangeTarget(false);
		}
		ChangeState(PLAYERSTATE.NORMAL);
		fireTarget = null;
		healCtl.Reset();
		if (machineGun != null)
		{
			machineGun.reloadFlag = false;
			machineGun.bulletCount = 0;
			if (machineGun.gunInfo.restBulletNum + machineGun.gunInfo.bulletNum > machineGun.gunInfo.maxBulletNum)
			{
				machineGun.gunInfo.restBulletNum = machineGun.gunInfo.maxBulletNum - machineGun.gunInfo.bulletNum;
			}
		}
		if (gun != null)
		{
			gun.reloadFlag = false;
			gun.bulletCount = 0;
			if (gun.gunInfo.restBulletNum + gun.gunInfo.bulletNum > gun.gunInfo.maxBulletNum)
			{
				gun.gunInfo.restBulletNum = gun.gunInfo.maxBulletNum - gun.gunInfo.bulletNum;
			}
		}
		fightReadyFlag = true;
		punchedFlag = false;
		moveCtl.enabled = true;
		occlusion.SetActiveRecursively(false);
		occlusionCount = 0;
	}

	public void QuitGetOnCar()
	{
		if (temCar != null)
		{
			moveCtl.moveToTargetFlag = false;
			temCar = null;
			changingTheCarFlag = false;
		}
	}

	public void DirectGetOnCar()
	{
		if (temCar != null && base.GetComponent<Rigidbody>().GetComponent<Collider>().enabled)
		{
			base.transform.position = temCar.getOnPoint.transform.position;
			base.transform.eulerAngles = temCar.getOnPoint.transform.eulerAngles;
			base.GetComponent<Rigidbody>().GetComponent<Collider>().enabled = false;
		}
	}

	public void GetOnCar(Transform pos, VechicleController cc)
	{
		moveCtl.MoveToTarget(pos);
		ChangeState(PLAYERSTATE.NORMAL);
		getOnCarPos = pos;
		temCar = cc;
		temCar.AICarCtl.isbrake = true;
		fireTarget = null;
		preFireTarget = null;
		temCar.GetComponent<Rigidbody>().isKinematic = false;
		temCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
		temCar.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		temCar.AICarCtl.motor.enabled = false;
		startGetOnCarTime = 0f;
		if (cc == CarManage.instance.playerCar)
		{
			GameController.instance.driveMode.minimapController.DisablePlayerCarPos();
		}
	}

	public void OnGetOnCarMoveDone()
	{
		base.transform.parent = getOnCarPos;
		moveCtl.enabled = false;
		base.GetComponent<Rigidbody>().GetComponent<Collider>().enabled = false;
		base.GetComponent<Rigidbody>().useGravity = false;
		getOnCarLerpPercent = 0f;
		getOnCarLerpFlag = true;
		getOnStartPos = base.transform.localPosition;
		getOnStartQua = base.transform.localRotation;
		car = temCar;
		temCar = null;
		car.gameObject.layer = LayerMask.NameToLayer("PlayerCar");
		car.body.layer = LayerMask.NameToLayer("PlayerCar");
		if (car.AICarCtl.topCollider != null)
		{
			car.AICarCtl.topCollider.gameObject.layer = LayerMask.NameToLayer("PlayerCar");
		}
		car.enabled = true;
		if (car.AICarCtl.policeFlag)
		{
			PoliceLevelCtl.RobPoliceCar();
		}
		else if (car.AICarCtl.moveFlag)
		{
			PoliceLevelCtl.RobCar();
		}
		ChangeState(PLAYERSTATE.CAR);
		onGetOnCarDone();
	}

	public void InvokeGetOnCar()
	{
		playerShadow.gameObject.SetActiveRecursively(false);
		changingTheCarFlag = false;
		if (car != null)
		{
			car.OnEnableCar();
		}
		firedTarget.transform.position = ragdollController.hipsTrans.position;
	}

	public void CrashFromMotor()
	{
		if (!(car == null))
		{
			if (car == CarManage.instance.playerCar)
			{
				GameController.instance.driveMode.minimapController.EnablePlayerCarPos(CarManage.instance.playerCar.transform.position);
			}
			ragdollController.ragdolled = true;
			for (int i = 0; i < ragdollController.rigidbodyList.Count; i++)
			{
				ragdollController.rigidbodyList[i].velocity = car.transform.forward * 10f + Vector3.up * 5f;
			}
			ChangeState(PLAYERSTATE.RAGDOLL);
			car.OnDisableCar();
			cam.OnChangePlayerRagdollTarget();
			base.transform.parent = null;
			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
			car.gameObject.layer = LayerMask.NameToLayer("Car");
			car.body.layer = LayerMask.NameToLayer("Car");
			if (car.AICarCtl.topCollider != null)
			{
				car.AICarCtl.topCollider.gameObject.layer = LayerMask.NameToLayer("Car");
			}
			car = null;
			onGetOffCarDone();
			playerShadow.gameObject.SetActiveRecursively(true);
			AudioController.instance.stop(AudioType.ENGINE);
			if (GameController.instance.curGameMode == GAMEMODE.ROBMOTOR)
			{
				GameController.instance.robMotorMode.CheckTitle(null);
			}
		}
	}

	public void OnGetUpDone()
	{
		moveCtl.enabled = true;
		moveCtl.playerFaceAngle = base.transform.eulerAngles.y;
		base.GetComponent<Rigidbody>().GetComponent<Collider>().enabled = true;
		base.GetComponent<Rigidbody>().useGravity = true;
		handGunPos.localPosition = defaultHandGunPos;
		handGunPos.localEulerAngles = defaultHandGunAngle;
		if (gun != null)
		{
			gun.transform.localPosition = Vector3.zero;
			gun.transform.localEulerAngles = Vector3.zero;
		}
		machineGunPos.localPosition = defaultMachineGunPos;
		machineGunPos.localEulerAngles = defaultMachineGunAngle;
		if (machineGun != null)
		{
			machineGun.transform.localPosition = Vector3.zero;
			machineGun.transform.localEulerAngles = Vector3.zero;
		}
		ChangeState(PLAYERSTATE.NORMAL);
	}

	public void GetOffCar()
	{
		if (car == null)
		{
			return;
		}
		if (!car.motorFlag)
		{
			if (Physics.Raycast(car.AICarCtl.leftSideRayPos.position, car.AICarCtl.leftSideRayPos.forward, 1.5f))
			{
				changingTheCarFlag = false;
				return;
			}
		}
		else if (Physics.Raycast(car.AICarCtl.leftSideRayPos.position, car.AICarCtl.leftSideRayPos.forward, 0.8f))
		{
			changingTheCarFlag = false;
			return;
		}
		if (car == CarManage.instance.playerCar)
		{
			GameController.instance.driveMode.minimapController.EnablePlayerCarPos(CarManage.instance.playerCar.transform.position);
		}
		car.OnDisableCar();
		car.OnCloseDoor();
		animaCtl.GetOffCarAnimaClipPlay();
		cam.OnChangeTarget(false);
	}

	public void GetOffCarDone()
	{
		if (!(car == null))
		{
			base.transform.parent = null;
			moveCtl.enabled = true;
			moveCtl.playerFaceAngle = base.transform.eulerAngles.y;
			base.GetComponent<Rigidbody>().GetComponent<Collider>().enabled = true;
			base.GetComponent<Rigidbody>().useGravity = true;
			car.gameObject.layer = LayerMask.NameToLayer("Car");
			car.body.layer = LayerMask.NameToLayer("Car");
			if (car.AICarCtl.topCollider != null)
			{
				car.AICarCtl.topCollider.gameObject.layer = LayerMask.NameToLayer("Car");
			}
			car = null;
			ChangeState(PLAYERSTATE.NORMAL);
			onGetOffCarDone();
			playerShadow.gameObject.SetActiveRecursively(true);
			AudioController.instance.stop(AudioType.ENGINE);
		}
	}

	public void DelayGetoffCarDone()
	{
		onGetOffCarDone();
	}

	public void PlayerGetOffCar()
	{
		if (curState == PLAYERSTATE.CAR)
		{
			if (car == CarManage.instance.playerCar)
			{
				car.ResetPoint();
				car.OnDisableCar();
				cam.OnChangeTartgetRightNow(false);
				GetOffCarDone();
				base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
				base.transform.position = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
				CarManage.instance.ResetPlayerCarPos();
			}
			else
			{
				car.ResetPoint();
				car.OnDisableCar();
				cam.OnChangeTartgetRightNow(false);
				base.transform.parent = null;
				AICarPoolController.instance.recylecar(car.AICarCtl);
				GetOffCarDone();
				base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
				base.transform.position = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
			}
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerWall") || other.gameObject.layer == LayerMask.NameToLayer("OutSideWall"))
		{
			occlusion.SetActiveRecursively(true);
			occlusion.transform.parent = base.transform;
			occlusionCount++;
		}
	}

	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerWall") || other.gameObject.layer == LayerMask.NameToLayer("OutSideWall"))
		{
			occlusionCount--;
			if (occlusionCount == 0)
			{
				occlusion.SetActiveRecursively(false);
				occlusion.transform.parent = null;
			}
		}
	}

	private void OnCollisionStay(Collision other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerWall") || other.gameObject.layer == LayerMask.NameToLayer("OutSideWall"))
		{
			Vector3 vector = -other.contacts[0].normal * 0.5f;
			occlusion.transform.position = new Vector3(vector.x, 0f, vector.z) + base.transform.position;
			occlusion.transform.right = other.contacts[0].normal;
			occlusion.transform.eulerAngles = new Vector3(0f, occlusion.transform.eulerAngles.y, 0f);
		}
	}
}
