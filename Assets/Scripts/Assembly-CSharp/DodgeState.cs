using System;
using UnityEngine;

[Serializable]
public class DodgeState : STATEInstance
{
	public Transform carPos;

	public AnimationClip[] dodgeLeft;

	public AnimationClip[] dodgeRight;

	public Vector3 runDirection;

	public float waitTime;

	public float moveTime;

	public float moveSpeed;

	public float endTime;

	public float timeCount;

	private Vector3 forwardTarget;

	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.DODGE;
		for (int i = 0; i < dodgeRight.Length; i++)
		{
			anima[dodgeRight[i].name].layer = 1;
		}
		for (int j = 0; j < dodgeLeft.Length; j++)
		{
			anima[dodgeLeft[j].name].layer = 1;
		}
		if (carPos.InverseTransformPoint(ai.transform.position).x > 0f)
		{
			if (anima != null)
			{
				anima.CrossFade(dodgeLeft[UnityEngine.Random.Range(0, dodgeLeft.Length)].name);
			}
			runDirection = carPos.right;
		}
		else
		{
			if (anima != null)
			{
				anima.CrossFade(dodgeRight[UnityEngine.Random.Range(0, dodgeRight.Length)].name);
			}
			runDirection = -carPos.right;
		}
		forwardTarget = -carPos.forward;
		forwardTarget = new Vector3(forwardTarget.x, 0f, forwardTarget.z);
		timeCount = 0f;
	}

	public override void MyUpdate()
	{
		base.MyUpdate();
		timeCount += Time.deltaTime;
		ai.transform.forward = Vector3.Lerp(ai.transform.forward, forwardTarget, Time.deltaTime * 20f);
		anima.transform.forward = Vector3.Lerp(anima.transform.forward, forwardTarget, Time.deltaTime * 20f);
		if (timeCount > waitTime && timeCount < moveTime)
		{
			ai.transform.position = ai.transform.position + runDirection * moveSpeed * Time.deltaTime;
		}
	}
}
