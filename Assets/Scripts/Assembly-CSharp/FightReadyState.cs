using System;
using UnityEngine;

[Serializable]
public class FightReadyState : STATEInstance
{
	public AnimationClip leftPunchClip;

	public AnimationClip rightPunchClip;

	public AnimationClip boxingWalkClip;

	public AnimationClip boxingRunClip;

	public float boxingWalkSpeed;

	public float boxingRunSpeed;

	public float leftInterval;

	public float rightInterval;

	public float countTime;

	private bool leftFightFlag;

	public bool walkFlag;

	public bool runFlag;

	public float hitRate;

	public Vector3 moveDirection;

	private bool fightFlag;

	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.FIGHTREADY;
		if (anima != null)
		{
			anima.CrossFade(animaClip[animaIndex].name);
		}
		anima[leftPunchClip.name].layer = 1;
		anima[rightPunchClip.name].layer = 1;
		anima[boxingWalkClip.name].layer = 1;
		anima[boxingRunClip.name].layer = 1;
		countTime = 0f;
		walkFlag = false;
		runFlag = false;
		fightFlag = false;
		leftFightFlag = false;
	}

	public override void MyUpdate()
	{
		base.MyUpdate();
		countTime += Time.deltaTime;
		if ((ai.player.transform.position - ai.transform.position).sqrMagnitude < 1f)
		{
			walkFlag = false;
			runFlag = false;
			if (leftFightFlag)
			{
				if (countTime > rightInterval && ai.player.curState != PLAYERSTATE.DIE)
				{
					countTime = 0f;
					leftFightFlag = false;
					anima.CrossFade(leftPunchClip.name);
					fightFlag = true;
				}
			}
			else if (countTime > leftInterval && ai.player.curState != PLAYERSTATE.DIE)
			{
				countTime = 0f;
				leftFightFlag = true;
				anima.CrossFade(rightPunchClip.name);
				fightFlag = true;
			}
			if (anima[rightPunchClip.name].normalizedTime > 0.5f || anima[leftPunchClip.name].normalizedTime > 0.5f)
			{
				anima.CrossFade(animaClip[animaIndex].name);
				if (fightFlag)
				{
					fightFlag = false;
					if ((float)UnityEngine.Random.Range(0, 10) < hitRate)
					{
						ai.player.OnPunched(ai.punchedDamageVal, ai);
					}
				}
			}
			ai.moveMotor.speed = 0f;
			ai.anima.transform.forward = (PlayerController.instance.transform.position - ai.transform.position).normalized;
		}
		else if ((ai.player.transform.position - ai.transform.position).sqrMagnitude < 5f)
		{
			runFlag = false;
			fightFlag = false;
			if (!walkFlag)
			{
				walkFlag = true;
				anima.CrossFade(boxingWalkClip.name);
			}
			ai.moveMotor.movementDirection = (PlayerController.instance.transform.position - ai.transform.position).normalized;
			ai.moveMotor.speed = boxingWalkSpeed;
			ai.anima.transform.forward = ai.moveMotor.movementDirection;
		}
		else
		{
			walkFlag = false;
			fightFlag = false;
			if (!runFlag)
			{
				runFlag = true;
				anima.CrossFade(boxingRunClip.name);
			}
			ai.moveMotor.movementDirection = moveDirection.normalized;
			ai.moveMotor.speed = boxingRunSpeed;
			ai.anima.transform.forward = Vector3.Lerp(ai.anima.transform.forward, ai.moveMotor.movementDirection, Time.deltaTime * 5f);
		}
	}

	public override void MyExit()
	{
		base.MyExit();
		ai.moveMotor.movementDirection = Vector3.zero;
		ai.moveMotor.speed = 0f;
	}
}
