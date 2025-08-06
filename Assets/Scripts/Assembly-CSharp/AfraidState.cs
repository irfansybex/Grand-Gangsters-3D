using System;

[Serializable]
public class AfraidState : STATEInstance
{
	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.AFRAID;
		if (anima != null)
		{
			anima.CrossFade(animaClip[animaIndex].name);
		}
	}
}
