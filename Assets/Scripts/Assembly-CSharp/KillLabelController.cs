using UnityEngine;

public class KillLabelController : MonoBehaviour
{
	public TweenScale tweenScale;

	public TweenAlpha tweenAlph;

	public UILabel label;

	public void Reset(int num, Vector3 pos)
	{
		base.gameObject.SetActiveRecursively(true);
		Vector3 vector = Camera.main.WorldToViewportPoint(pos);
		base.transform.localPosition = new Vector3(vector.x * GlobalDefine.screenRatioWidth - GlobalDefine.screenRatioWidth / 2f, vector.y * 480f - 240f, 0f);
		tweenScale.ResetToBeginning();
		tweenScale.PlayForward();
		tweenAlph.ResetToBeginning();
		tweenAlph.PlayForward();
		label.text = string.Empty + num;
	}

	public void DelayDisable()
	{
		base.gameObject.SetActiveRecursively(false);
	}
}
