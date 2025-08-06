using UnityEngine;

public class TestUITween : MonoBehaviour
{
	public bool runFlag;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Run();
		}
	}

	public void Run()
	{
		if (base.gameObject.GetComponent<TweenPosition>() != null)
		{
			base.gameObject.GetComponent<TweenPosition>().ResetToBeginning();
			base.gameObject.GetComponent<TweenPosition>().PlayForward();
		}
		if (base.gameObject.GetComponent<TweenRotation>() != null)
		{
			base.gameObject.GetComponent<TweenRotation>().ResetToBeginning();
			base.gameObject.GetComponent<TweenRotation>().PlayForward();
		}
		if (base.gameObject.GetComponent<TweenScale>() != null)
		{
			base.gameObject.GetComponent<TweenScale>().ResetToBeginning();
			base.gameObject.GetComponent<TweenScale>().PlayForward();
		}
		if (base.gameObject.GetComponent<TweenColor>() != null)
		{
			base.gameObject.GetComponent<TweenColor>().ResetToBeginning();
			base.gameObject.GetComponent<TweenColor>().PlayForward();
		}
		if (base.gameObject.GetComponent<TweenAlpha>() != null)
		{
			base.gameObject.GetComponent<TweenAlpha>().ResetToBeginning();
			base.gameObject.GetComponent<TweenAlpha>().PlayForward();
		}
	}
}
