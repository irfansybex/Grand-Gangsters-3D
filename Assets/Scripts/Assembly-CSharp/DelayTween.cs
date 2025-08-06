using UnityEngine;

public class DelayTween : MonoBehaviour
{
	public float delayTime;

	private TweenPosition tweenPos;

	private bool initFlag;

	private float startTime;

	private void Start()
	{
		tweenPos = base.gameObject.GetComponent<TweenPosition>();
		initFlag = false;
		startTime = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		if (!initFlag && Time.realtimeSinceStartup - startTime > delayTime)
		{
			initFlag = true;
			tweenPos.ResetToBeginning();
			tweenPos.PlayForward();
		}
	}
}
