using UnityEngine;

public class GunCtl : MonoBehaviour
{
	public delegate void OnReload();

	public delegate void OnReloadDone();

	public static int machineGunShotNum;

	public bool playerGunFlag;

	public Transform muzzlePos;

	public GameObject muzzleFlash;

	public GunsInfo gunInfo;

	public bool readyToShotFlag;

	public bool shotFlag;

	public bool reloadFlag;

	private MyBullet tempBullet;

	public int bulletCount;

	public float timeCount;

	public float flashTime;

	public float flashTimeCount;

	public OnReload onReload;

	public OnReloadDone onReloadDone;

	public float hitRate;

	public bool hitTargetFlag;

	public float countReloadTime;

	//public ParticleEmitter bulletSmoke;

	public ParticleSystem bulletShells;

	public Ray gunTempRay;

	public RaycastHit rayHit;

	public bool hitFlag;

	public bool ttt;

	public LayerMask gunShotLayer;

	public bool hitMetalFlag;

	private float accuracy;

	private void OnDisable()
	{
		/*if (bulletSmoke != null)
		{
			bulletSmoke.emit = false;
		}*/
		if (bulletShells != null)
		{
			bulletShells.Stop();
		}
		if (playerGunFlag && gunInfo.gunName == GUNNAME.MACHINEGUN)
		{
			AudioController.instance.stop(AudioType.MACHINE_GUN);
		}
	}

	private void OnEnable()
	{
		/*if (bulletSmoke != null)
		{
			bulletSmoke.emit = false;
		}*/
		if (bulletShells != null)
		{
			bulletShells.Stop();
		}
		if (playerGunFlag && gunInfo.gunName == GUNNAME.MACHINEGUN)
		{
			AudioController.instance.stop(AudioType.MACHINE_GUN);
		}
	}

	private void DelayReloadAudio()
	{
		if (AudioController.instance != null)
		{
			AudioController.instance.play(AudioType.RELOAD);
		}
	}

	private void Update()
	{
		if (ttt)
		{
			ttt = false;
			gunInfo.restBulletNum = 60;
		}
		if (!playerGunFlag)
		{
			if (shotFlag)
			{
				shotFlag = false;
				bulletCount++;
				if (bulletCount >= gunInfo.bulletNum)
				{
					reloadFlag = true;
					onReload();
				}
			}
			if (!reloadFlag)
			{
				if (!readyToShotFlag)
				{
					timeCount += Time.deltaTime;
					if (timeCount > gunInfo.shotInterval)
					{
						timeCount = 0f;
						readyToShotFlag = true;
					}
				}
			}
			else if (countReloadTime > gunInfo.reloadTime)
			{
				countReloadTime = 0f;
				reloadFlag = false;
				bulletCount = 0;
				onReloadDone();
			}
			else
			{
				countReloadTime += Time.deltaTime;
			}
			if (flashTimeCount > 0f)
			{
				flashTimeCount -= Time.deltaTime;
			}
			else if (flashTimeCount <= 0f && !GlobalDefine.smallPhoneFlag && muzzleFlash.gameObject.active)
			{
				muzzleFlash.SetActiveRecursively(false);
				//if (bulletSmoke != null)
				{
					//bulletSmoke.emit = false;
					bulletShells.Stop();
				}
			}
			return;
		}
		if (gunInfo.curBulletNum > 0 || gunInfo.restBulletNum > 0)
		{
			if (shotFlag)
			{
				shotFlag = false;
				bulletCount++;
				if (gunInfo.curBulletNum > 0)
				{
					GameUIController.instance.bulletNumLabel.text = string.Empty + (gunInfo.curBulletNum - bulletCount) + "/" + (gunInfo.restBulletNum + (gunInfo.curBulletNum - bulletCount));
				}
				if (bulletCount >= gunInfo.curBulletNum)
				{
					if (bulletCount > gunInfo.curBulletNum)
					{
						bulletCount--;
					}
					ReloadPlayerGun();
				}
			}
			if (!reloadFlag)
			{
				if (!readyToShotFlag)
				{
					timeCount += Time.deltaTime;
					if (timeCount > gunInfo.shotInterval)
					{
						timeCount = 0f;
						readyToShotFlag = true;
					}
				}
			}
			else if (countReloadTime > gunInfo.reloadTime)
			{
				countReloadTime = 0f;
				reloadFlag = false;
				int num = 0;
				num = gunInfo.bulletNum - (gunInfo.curBulletNum - bulletCount);
				if (gunInfo.restBulletNum >= num)
				{
					gunInfo.restBulletNum -= num;
					gunInfo.curBulletNum = gunInfo.bulletNum;
				}
				else
				{
					gunInfo.curBulletNum = gunInfo.curBulletNum - bulletCount + gunInfo.restBulletNum;
					gunInfo.restBulletNum = 0;
				}
				onReloadDone();
				bulletCount = 0;
				GameUIController.instance.bulletNumLabel.text = string.Empty + (gunInfo.curBulletNum - bulletCount) + "/" + (gunInfo.restBulletNum + (gunInfo.curBulletNum - bulletCount));
				timeCount = gunInfo.shotInterval - 0.2f;
				if (gunInfo.gunName == GUNNAME.HANDGUN)
				{
					StoreDateController.SetHandGunBulletNum(GlobalInf.handgunIndex, gunInfo.restBulletNum + (gunInfo.curBulletNum - bulletCount));
				}
				else if (gunInfo.gunName == GUNNAME.MACHINEGUN)
				{
					StoreDateController.SetMachineGunBulletNum(GlobalInf.machineGunIndex, gunInfo.restBulletNum + (gunInfo.curBulletNum - bulletCount));
				}
			}
			else
			{
				countReloadTime += Time.deltaTime;
			}
			if (flashTimeCount > 0f)
			{
				flashTimeCount -= Time.deltaTime;
			}
			else if (flashTimeCount <= 0f && !GlobalDefine.smallPhoneFlag && muzzleFlash.gameObject.active)
			{
				muzzleFlash.SetActiveRecursively(false);
				//if (bulletSmoke != null)
				{
				//	bulletSmoke.emit = false;
					bulletShells.Stop();
				}
				if (gunInfo.gunName == GUNNAME.MACHINEGUN && AudioController.instance != null)
				{
					AudioController.instance.stop(AudioType.MACHINE_GUN);
				}
			}
			return;
		}
		if (!GlobalDefine.smallPhoneFlag && muzzleFlash.gameObject.active)
		{
			muzzleFlash.SetActiveRecursively(false);
			//if (bulletSmoke != null)
			{
			//	bulletSmoke.emit = false;
				bulletShells.Stop();
			}
			if (gunInfo.gunName == GUNNAME.MACHINEGUN && AudioController.instance != null)
			{
				AudioController.instance.stop(AudioType.MACHINE_GUN);
			}
		}
		readyToShotFlag = false;
	}

	public void ReloadPlayerGun()
	{
		if (gunInfo.bulletNum - (gunInfo.curBulletNum - bulletCount) > 0)
		{
			reloadFlag = true;
			onReload();
			Invoke("DelayReloadAudio", 0.5f);
			timeCount = 0f;
			readyToShotFlag = false;
		}
	}

	public void Shot(RaycastHit hit)
	{
		if (BulletPool.instance.bulletPool.Count != 0 && readyToShotFlag)
		{
			if (hitRate > (float)Random.Range(0, 10))
			{
				hitTargetFlag = true;
			}
			else
			{
				hitTargetFlag = false;
			}
			tempBullet = BulletPool.instance.GetBullet();
			tempBullet.gameObject.SetActiveRecursively(true);
			readyToShotFlag = false;
			if (!GlobalDefine.smallPhoneFlag)
			{
				muzzleFlash.SetActiveRecursively(true);
			}
			flashTimeCount = flashTime;
			shotFlag = true;
		}
	}

	public void Shot(Vector3 pos)
	{
		if (BulletPool.instance.bulletPool.Count != 0 && readyToShotFlag)
		{
			if (hitRate > (float)Random.Range(0, 10))
			{
				hitTargetFlag = true;
			}
			else
			{
				hitTargetFlag = false;
			}
			tempBullet = BulletPool.instance.GetBullet();
			tempBullet.gameObject.SetActiveRecursively(true);
			readyToShotFlag = false;
			if (!GlobalDefine.smallPhoneFlag)
			{
				muzzleFlash.SetActiveRecursively(true);
			}
			flashTimeCount = flashTime;
			shotFlag = true;
		}
	}

	public void Shot(Transform pos)
	{
		if (BulletPool.instance.bulletPool.Count == 0 || !readyToShotFlag)
		{
			return;
		}
		if (playerGunFlag && gunInfo.curBulletNum == 0)
		{
			shotFlag = true;
			return;
		}
		if (AudioController.instance != null)
		{
			if (gunInfo.gunName == GUNNAME.HANDGUN)
			{
				AudioController.instance.play(AudioType.HANDGUN);
			}
			else if (playerGunFlag)
			{
				if (!GlobalDefine.smallPhoneFlag)
				{
					AudioController.instance.play(AudioType.MACHINE_GUN);
				}
				else
				{
					AudioController.instance.play(AudioType.AIMACHINEGUN);
				}
			}
			else
			{
				AudioController.instance.play(AudioType.AIMACHINEGUN);
			}
		}
		if (Vector3.SqrMagnitude(new Vector3(pos.position.x - muzzlePos.position.x, 0f, pos.position.z - muzzlePos.position.z)) < 4f)
		{
			hitFlag = true;
			hitTargetFlag = true;
		}
		else
		{
			gunTempRay = new Ray(muzzlePos.position, (pos.position - muzzlePos.position).normalized);
			if (Physics.Raycast(gunTempRay, out rayHit, 300f, gunShotLayer))
			{
				hitFlag = true;
				if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Player") || rayHit.collider.gameObject.layer == LayerMask.NameToLayer("AI") || rayHit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerCar"))
				{
					if (gunInfo.accuracy > Random.Range(0f, 1f))
					{
						hitTargetFlag = true;
					}
					else
					{
						hitFlag = false;
						hitTargetFlag = false;
					}
				}
				else
				{
					hitTargetFlag = false;
				}
				if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerCar"))
				{
					if (PlayerController.instance.car.carType != CARTYPE.MOTOR)
					{
						hitMetalFlag = true;
					}
					else
					{
						hitMetalFlag = false;
					}
				}
				else
				{
					hitMetalFlag = false;
				}
			}
			else
			{
				hitFlag = false;
			}
		}
		tempBullet = BulletPool.instance.GetBullet();
		Vector3 dir = pos.position - muzzlePos.position;
		tempBullet.gameObject.SetActiveRecursively(true);
		if (hitTargetFlag)
		{
			tempBullet.Init(muzzlePos.position, dir, gunInfo.bulletSpeed, pos, rayHit, hitFlag, hitTargetFlag, hitMetalFlag);
		}
		else
		{
			tempBullet.Init(muzzlePos.position, dir, gunInfo.bulletSpeed, null, rayHit, hitFlag, hitTargetFlag, hitMetalFlag);
		}
		readyToShotFlag = false;
		if (!GlobalDefine.smallPhoneFlag)
		{
			muzzleFlash.SetActiveRecursively(true);
			//if (bulletSmoke != null)
			{
			//	bulletSmoke.emit = true;
				bulletShells.Play();
			}
		}
		flashTimeCount = flashTime;
		shotFlag = true;
	}

	public void PlayerShot(Transform pos, GameObject aiTarget)
	{
		if (BulletPool.instance.bulletPool.Count == 0 || !readyToShotFlag)
		{
			return;
		}
		if (playerGunFlag && gunInfo.curBulletNum == 0)
		{
			shotFlag = true;
			return;
		}
		if (AudioController.instance != null)
		{
			if (gunInfo.gunName == GUNNAME.HANDGUN)
			{
				AudioController.instance.play(AudioType.HANDGUN);
			}
			else if (playerGunFlag)
			{
				if (!GlobalDefine.smallPhoneFlag)
				{
					AudioController.instance.play(AudioType.MACHINE_GUN);
				}
				else
				{
					AudioController.instance.play(AudioType.AIMACHINEGUN);
				}
			}
			else
			{
				AudioController.instance.play(AudioType.AIMACHINEGUN);
			}
		}
		if (Vector3.SqrMagnitude(new Vector3(pos.position.x - muzzlePos.position.x, 0f, pos.position.z - muzzlePos.position.z)) < 4f)
		{
			hitFlag = true;
			hitTargetFlag = true;
		}
		else
		{
			gunTempRay = new Ray(muzzlePos.position, (pos.position - muzzlePos.position).normalized);
			if (Physics.Raycast(gunTempRay, out rayHit, 300f, gunShotLayer))
			{
				hitFlag = true;
				if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("AI"))
				{
					if (rayHit.collider.gameObject != aiTarget)
					{
						AIController component = rayHit.collider.GetComponent<AIController>();
						if (!component.dieFlag)
						{
							if (component != PlayerController.instance.fireTarget)
							{
								PlayerController.instance.fireTarget = component;
							}
							if (!PlayerController.instance.animaCtl.needToAimFlag)
							{
								PlayerController.instance.animaCtl.OnChangeAimState(true);
							}
						}
					}
					if (gunInfo.accuracy > Random.Range(0f, 1f))
					{
						hitTargetFlag = true;
					}
					else
					{
						hitFlag = false;
						hitTargetFlag = false;
					}
				}
				else
				{
					hitTargetFlag = false;
				}
				if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerCar"))
				{
					hitMetalFlag = true;
				}
				else
				{
					hitMetalFlag = false;
				}
			}
			else
			{
				hitFlag = false;
			}
		}
		tempBullet = BulletPool.instance.GetBullet();
		Vector3 dir = pos.position - muzzlePos.position;
		tempBullet.gameObject.SetActiveRecursively(true);
		if (hitTargetFlag)
		{
			tempBullet.Init(muzzlePos.position, dir, gunInfo.bulletSpeed, pos, rayHit, hitFlag, hitTargetFlag, hitMetalFlag);
		}
		else
		{
			tempBullet.Init(muzzlePos.position, dir, gunInfo.bulletSpeed, null, rayHit, hitFlag, hitTargetFlag, hitMetalFlag);
		}
		readyToShotFlag = false;
		if (!GlobalDefine.smallPhoneFlag)
		{
			muzzleFlash.SetActiveRecursively(true);
			//if (bulletSmoke != null)
			{
				//bulletSmoke.emit = true;
				bulletShells.Play();
			}
		}
		flashTimeCount = flashTime;
		shotFlag = true;
	}
}
