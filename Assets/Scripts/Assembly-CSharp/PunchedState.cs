using System;
using UnityEngine;

[Serializable]
public class PunchedState : STATEInstance
{
	public GameObject target;

	private bool punchedFlag;

	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.PUNCHED;
		if (anima != null)
		{
			animaIndex = UnityEngine.Random.Range(0, animaClip.Length);
			animaIndex = 2;
			anima[animaClip[animaIndex].name].layer = 2;
			anima[animaClip[animaIndex].name].AddMixingTransform(ai.waist);
			anima.CrossFade(animaClip[animaIndex].name);
		}
		punchedFlag = true;
	}

	public override void MyUpdate()
	{
		base.MyUpdate();
		ai.anima.transform.forward = Vector3.Lerp(ai.anima.transform.forward, target.transform.position - ai.anima.transform.position, Time.deltaTime * 5f);
		if (punchedFlag && animaIndex == 0)
		{
			ai.transform.position -= new Vector3(ai.transform.forward.x, 0f, ai.transform.forward.z) * Time.deltaTime;
		}
		if (anima[animaClip[animaIndex].name].normalizedTime > 0.95f)
		{
			punchedFlag = false;
		}
	}
}
