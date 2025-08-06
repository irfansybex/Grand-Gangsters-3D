using System;

[Serializable]
public class IdleState : STATEInstance
{
	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.IDLE;
		if (anima != null)
		{
			anima.CrossFade(animaClip[animaIndex].name);
		}
	}
}
