using System;
using UnityEngine;

[Serializable]
public class ShottedState : STATEInstance
{
	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.DAMAGED;
		if (anima != null)
		{
			animaIndex = UnityEngine.Random.Range(0, animaClip.Length);
			anima.CrossFade(animaClip[animaIndex].name);
		}
	}
}
