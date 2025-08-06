using System;
using UnityEngine;

[Serializable]
public class RunAwayState : STATEInstance
{
	public Vector3 direction;

	public float speed;

	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.RUNAWAY;
		if (anima != null)
		{
			anima.CrossFade(animaClip[animaIndex].name);
		}
	}

	public override void MyUpdate()
	{
		base.MyUpdate();
		ai.transform.position = ai.transform.position + direction * speed * Time.deltaTime;
		ai.transform.forward = Vector3.Lerp(ai.transform.forward, new Vector3(direction.x, 0f, direction.z).normalized, Time.deltaTime * 5f);
	}
}
