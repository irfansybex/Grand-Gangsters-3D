using UnityEngine;

public class MoveUI : MonoBehaviour
{
	public Vector3 startPos;

	public Vector3 targetPos;

	public float duration;

	public TweenPosition tPos;

	public UIPanel pan;

	private void Awake()
	{
		tPos = base.gameObject.GetComponent<TweenPosition>();
		if (tPos != null)
		{
			tPos.from = ToolFunction.NGUIFitScreenPos(startPos);
			tPos.to = ToolFunction.NGUIFitScreenPos(targetPos);
			tPos.duration = duration;
		}
	}

	private void OnEnable()
	{
		tPos.enabled = true;
		tPos.ResetToBeginning();
	}
}
