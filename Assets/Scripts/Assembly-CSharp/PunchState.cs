using System;

[Serializable]
public class PunchState : STATEInstance
{
	public float damage;

	public float punchInterval;

	public override void MyEnter()
	{
		base.MyEnter();
		stateName = AISTATE.PUNCH;
		if (anima != null)
		{
			anima.CrossFade(animaClip[animaIndex].name);
		}
	}
}
