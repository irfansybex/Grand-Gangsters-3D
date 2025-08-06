using System;
using UnityEngine;

[Serializable]
public class RunState : STATEInstance
{
	public float speed;

	public Vector3 direction;

	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.RUN;
		if (anima != null)
		{
			anima.CrossFade(animaClip[0].name);
		}
	}

	public override void MyUpdate()
	{
		base.MyUpdate();
		ai.moveMotor.movementDirection = direction;
		ai.moveMotor.speed = speed;
		if (direction != Vector3.zero)
		{
			ai.anima.transform.forward = Vector3.Lerp(ai.anima.transform.forward, direction, Time.deltaTime * 5f);
		}
	}

	public override void MyExit()
	{
		base.MyExit();
		ai.moveMotor.movementDirection = Vector3.zero;
		ai.moveMotor.speed = 0f;
	}
}
