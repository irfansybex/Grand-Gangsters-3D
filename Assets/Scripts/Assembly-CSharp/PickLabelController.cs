using UnityEngine;

public class PickLabelController : MonoBehaviour
{
	public TweenPosition tweenPos;

	public TweenColor tweenCol;

	public string[] carName;

	public string[] handGunName;

	public string[] machineGunName;

	public UILabel label;

	public void Reset(ITEMTYPE type, int num, Vector3 pos)
	{
		base.gameObject.SetActiveRecursively(true);
		switch (type)
		{
		case ITEMTYPE.HANDGUN:
			label.text = handGunName[num];
			break;
		case ITEMTYPE.HANDGUNBULLET:
			label.text = "HandGunBullet *" + num;
			break;
		case ITEMTYPE.MACHINEGUNBULLET:
			label.text = "MachineGunBullet *" + num;
			break;
		case ITEMTYPE.MONEY:
			label.text = "$" + num;
			break;
		}
		Vector3 vector = Camera.main.WorldToViewportPoint(pos);
		tweenPos.from = new Vector3(vector.x * GlobalDefine.screenRatioWidth - 400f, vector.y * 480f - 240f, 0f);
		tweenPos.to = tweenPos.from + new Vector3(0f, 200f, 0f);
		tweenPos.ResetToBeginning();
		tweenPos.PlayForward();
		tweenCol.ResetToBeginning();
		tweenCol.PlayForward();
		Invoke("DelayDisable", 1f);
	}

	public void Reset(Vector3 pos)
	{
		base.gameObject.SetActiveRecursively(true);
		Vector3 vector = Camera.main.WorldToViewportPoint(pos);
		tweenPos.from = new Vector3(vector.x * GlobalDefine.screenRatioWidth - 400f, vector.y * 480f - 240f, 0f);
		tweenPos.to = tweenPos.from + new Vector3(0f, 200f, 0f);
		tweenPos.ResetToBeginning();
		tweenPos.PlayForward();
		tweenCol.ResetToBeginning();
		tweenCol.PlayForward();
		Invoke("DelayDisable", 1f);
	}

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
