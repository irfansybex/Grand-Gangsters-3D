using System;
using UnityEngine;

[Serializable]
public class DieState : STATEInstance
{
	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.DIE;
		if (GlobalDefine.smallPhoneFlag && anima != null)
		{
			animaIndex = UnityEngine.Random.Range(0, animaClip.Length);
			anima.CrossFade(animaClip[animaIndex].name);
		}
	}
}
