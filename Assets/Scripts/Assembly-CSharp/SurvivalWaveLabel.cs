using UnityEngine;

public class SurvivalWaveLabel : MonoBehaviour
{
	public TweenPosition tweenPos;

	public TweenColor tweenCol;

	public UILabel label;

	public void Reset(string val)
	{
		base.gameObject.SetActiveRecursively(true);
		label.text = val;
		tweenPos.from = new Vector3(0f, 0f, 0f);
		tweenPos.to = new Vector3(0f, 100f, 0f);
		tweenPos.ResetToBeginning();
		tweenPos.PlayForward();
		tweenCol.ResetToBeginning();
		tweenCol.PlayForward();
		Invoke("DelayDisable", 1f);
	}

	public void DelayDisable()
	{
		base.gameObject.SetActiveRecursively(false);
	}
}
