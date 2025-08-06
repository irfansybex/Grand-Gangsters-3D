using System;
using UnityEngine;

[Serializable]
public class MachineGunState : STATEInstance
{
	public GunCtl machineGun;

	public AnimationClip machineGunShotClip;

	public AnimationClip machineGunReloadClip;

	public float countShotedInterval;

	public float shottedInterval;

	public Transform machineGunParent;

	private float hitTime;

	private float beginWait;

	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.MACHINEGUN;
		if (anima != null)
		{
			anima.CrossFade(animaClip[animaIndex].name);
		}
		if (machineGun == null)
		{
			machineGun = GunPoolList.instance.machineGunPool.GetGun();
		}
		machineGun.gameObject.SetActiveRecursively(true);
		machineGun.muzzleFlash.SetActiveRecursively(false);
		machineGun.transform.parent = machineGunParent;
		machineGun.transform.localPosition = new Vector3(-0.09516156f, 0.006603119f, 0.05108002f);
		machineGun.transform.localEulerAngles = new Vector3(-2.050919f, -106.3213f, -93.16479f);
		anima[machineGunShotClip.name].layer = 4;
		anima[machineGunShotClip.name].speed = 2f;
		anima[machineGunShotClip.name].blendMode = AnimationBlendMode.Additive;
		anima[machineGunShotClip.name].AddMixingTransform(ai.waist);
		anima[machineGunReloadClip.name].layer = 1;
		GunCtl gunCtl = machineGun;
		gunCtl.onReload = (GunCtl.OnReload)Delegate.Combine(gunCtl.onReload, new GunCtl.OnReload(OnReload));
		GunCtl gunCtl2 = machineGun;
		gunCtl2.onReloadDone = (GunCtl.OnReloadDone)Delegate.Combine(gunCtl2.onReloadDone, new GunCtl.OnReloadDone(OnReloadDone));
		beginWait = 0f;
		machineGun.reloadFlag = false;
		machineGun.bulletCount = 0;
		machineGun.gunInfo.accuracy = ai.gunInfo.accuracy;
		machineGun.gunInfo.damage = ai.gunInfo.damage;
		machineGun.gunInfo.shotInterval = ai.gunInfo.shotInterval;
	}

	public override void MyUpdate()
	{
		base.MyUpdate();
		if (machineGun.reloadFlag)
		{
			return;
		}
		if (!ai.damagedFlag && PlayerController.instance.curState != PLAYERSTATE.DIE)
		{
			if ((double)beginWait < 0.5)
			{
				beginWait += Time.deltaTime;
			}
			else
			{
				machineGun.Shot(PlayerController.instance.firedTarget);
			}
			if (machineGun.shotFlag)
			{
				anima.Play(machineGunShotClip.name);
				if (machineGun.hitTargetFlag)
				{
					hitTime = (PlayerController.instance.firedTarget.transform.position - PlayerController.instance.transform.position).magnitude / machineGun.gunInfo.bulletSpeed;
					ai.InvokeBulletAttack(hitTime);
				}
			}
		}
		else
		{
			countShotedInterval += Time.deltaTime;
			if (countShotedInterval > shottedInterval)
			{
				countShotedInterval = 0f;
				ai.damagedFlag = false;
			}
		}
	}

	public override void MyExit()
	{
		if (machineGun != null)
		{
			machineGun.gameObject.SetActiveRecursively(false);
		}
		base.MyExit();
	}

	public void OnReload()
	{
		anima.CrossFade(machineGunReloadClip.name);
	}

	public void OnReloadDone()
	{
		anima.CrossFade(animaClip[animaIndex].name);
	}
}
