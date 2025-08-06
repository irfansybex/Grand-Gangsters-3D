using System;
using UnityEngine;

[Serializable]
public class STATEInstance
{
	public AISTATE stateName;

	public Animation anima;

	public AIController ai;

	public float sumTime;

	public AnimationClip[] animaClip;

	public int animaIndex;

	public virtual void MyEnter()
	{
		sumTime = 0f;
		if (animaClip.Length != 0 && anima != null)
		{
			for (int i = 0; i < animaClip.Length; i++)
			{
				anima[animaClip[i].name].layer = 1;
			}
		}
		animaIndex = 0;
	}

	public virtual void MyUpdate()
	{
		sumTime += Time.deltaTime;
	}

	public virtual void MyExit()
	{
	}

	public virtual void MyFixedUpdate()
	{
	}
}
