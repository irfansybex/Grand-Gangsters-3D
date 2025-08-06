using UnityEngine;

public class InputBehaviorCheck : MonoBehaviour
{
	public delegate void onTouchClick();

	public Touch clickTouch;

	public bool clickFlag;

	public float clickTime;

	public int fingerId;

	public Touch tempTouch;

	public float startTime;

	public Vector2 clickPos;

	private Vector2 pos;

	public onTouchClick OnTouchClick;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.touchCount == 1)
		{
			tempTouch = Input.GetTouch(0);
			if (tempTouch.phase == TouchPhase.Began)
			{
				fingerId = tempTouch.fingerId;
				startTime = Time.time;
			}
			if (tempTouch.phase == TouchPhase.Ended)
			{
				if (Time.time - startTime < clickTime)
				{
					clickPos = tempTouch.position;
					OnTouchClick();
				}
				else
				{
					clickPos = Vector2.zero;
				}
			}
		}
		else
		{
			if (Input.touchCount <= 1)
			{
				return;
			}
			tempTouch = Input.GetTouch(1);
			if (tempTouch.phase == TouchPhase.Began)
			{
				fingerId = tempTouch.fingerId;
				startTime = Time.time;
			}
			if (tempTouch.phase == TouchPhase.Ended)
			{
				if (Time.time - startTime < clickTime)
				{
					clickPos = tempTouch.position;
					OnTouchClick();
				}
				else
				{
					clickPos = Vector2.zero;
				}
			}
		}
	}
}
