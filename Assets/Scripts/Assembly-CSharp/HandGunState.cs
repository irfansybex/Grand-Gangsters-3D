using System;
using UnityEngine;

[Serializable]
public class HandGunState : STATEInstance
{
	public GunCtl handGun;

	public AnimationClip handGunShotClip;

	public AnimationClip handGunReloadClip;

	public float countShotedInterval;

	public float shottedInterval;

	private float hitTime;

	private float beginWait;

	public float shotDis;

	public Transform handGunParent;

	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.HANDGUN;
		if (anima != null)
		{
			anima.CrossFade(animaClip[animaIndex].name);
		}
		if (handGun == null)
		{
			handGun = GunPoolList.instance.handGunPool.GetGun();
		}
		handGun.gameObject.SetActiveRecursively(true);
		handGun.muzzleFlash.SetActiveRecursively(false);
		handGun.transform.parent = handGunParent;
		handGun.transform.localPosition = new Vector3(-0.1193908f, 0.01227923f, 0.01968526f);
		handGun.transform.localEulerAngles = new Vector3(-0.9075928f, -86.67847f, -90.05267f);
		anima[handGunShotClip.name].layer = 4;
		anima[handGunShotClip.name].blendMode = AnimationBlendMode.Additive;
		anima[handGunShotClip.name].AddMixingTransform(ai.waist);
		anima[handGunReloadClip.name].layer = 1;
		anima[handGunReloadClip.name].speed = 0.5f;
		GunCtl gunCtl = handGun;
		gunCtl.onReload = (GunCtl.OnReload)Delegate.Combine(gunCtl.onReload, new GunCtl.OnReload(OnReload));
		GunCtl gunCtl2 = handGun;
		gunCtl2.onReloadDone = (GunCtl.OnReloadDone)Delegate.Combine(gunCtl2.onReloadDone, new GunCtl.OnReloadDone(OnReloadDone));
		beginWait = 0f;
		handGun.reloadFlag = false;
		handGun.bulletCount = 0;
		ai.curRoadPoint = null;
		handGun.gunInfo.accuracy = ai.gunInfo.accuracy;
		handGun.gunInfo.damage = ai.gunInfo.damage;
		handGun.gunInfo.shotInterval = ai.gunInfo.shotInterval;
	}

	public override void MyUpdate()
	{
		base.MyUpdate();
		if (handGun.reloadFlag)
		{
			return;
		}
		if (!ai.damagedFlag && ai.player.curState != PLAYERSTATE.DIE)
		{
			if ((double)beginWait < 0.5)
			{
				beginWait += Time.deltaTime;
			}
			else
			{
				handGun.Shot(ai.player.firedTarget);
			}
			if (handGun.shotFlag)
			{
				anima.CrossFade(handGunShotClip.name);
				if (handGun.hitTargetFlag)
				{
					hitTime = (ai.player.firedTarget.transform.position - ai.transform.position).magnitude / handGun.gunInfo.bulletSpeed;
					ai.InvokeBulletAttack(hitTime);
				}
			}
		}
		else
		{
			countShotedInterval += Time.deltaTime;
			if (countShotedInterval > handGun.gunInfo.shotInterval)
			{
				countShotedInterval = 0f;
				ai.damagedFlag = false;
			}
		}
	}

	public override void MyExit()
	{
		if (handGun != null)
		{
			handGun.gameObject.SetActiveRecursively(false);
		}
		base.MyExit();
	}

	public void OnReload()
	{
		anima.CrossFade(handGunReloadClip.name);
	}

	public void OnReloadDone()
	{
		anima.CrossFade(animaClip[animaIndex].name);
	}
}
