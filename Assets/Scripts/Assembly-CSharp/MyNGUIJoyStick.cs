using System;
using UnityEngine;

public class MyNGUIJoyStick : MonoBehaviour
{
	public UIEventListener joyStickAreaBtn;

	public UISprite insideCircle;

	public UIWidget outsideCircle;

	public float joyStickLeftBorder;

	public float joyStickBottomBorder;

	public Vector2 defaultCenter;

	public Vector2 center;

	public float sqrRadius;

	public float radius;

	public Vector2 position;

	public Vector2 dir;

	public bool isPressed;

	private UICamera.MouseOrTouch currentTouch;

	private Vector2 touchLocalPos;

	public bool initFlag;

	public bool enableFlag = true;

	private void Awake()
	{
		radius = outsideCircle.width / 2;
		sqrRadius = outsideCircle.width * outsideCircle.width / 4;
		UIEventListener uIEventListener = joyStickAreaBtn;
		uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnBtnPress));
		if (!initFlag)
		{
			initFlag = true;
			BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
			component.center = new Vector3(component.size.x / 2f, component.size.y / 2f, 0f);
			defaultCenter = new Vector2(component.size.x / 2f, component.size.y / 2f);
		}
	}

	public void OnBtnPress(GameObject obj, bool isPress)
	{
		if (!enableFlag)
		{
			return;
		}
		isPressed = isPress;
		if (isPress)
		{
			currentTouch = UICamera.currentTouch;
			touchLocalPos = new Vector2(currentTouch.pos.x / (float)Screen.width * GlobalDefine.screenRatioWidth, currentTouch.pos.y / GlobalDefine.screenScale.y);
			dir = touchLocalPos - center;
			if (dir.sqrMagnitude > sqrRadius)
			{
				Vector2 vector = touchLocalPos - dir.normalized * radius;
				if (vector.x < joyStickLeftBorder)
				{
					vector.x = joyStickLeftBorder;
				}
				if (vector.y < joyStickBottomBorder)
				{
					vector.y = joyStickBottomBorder;
				}
				SetCenter(vector);
			}
		}
		else
		{
			currentTouch = null;
			SetCenter(defaultCenter);
			position = Vector2.zero;
		}
	}

	private void Update()
	{
		if (enableFlag && isPressed)
		{
			touchLocalPos = new Vector2(currentTouch.pos.x / (float)Screen.width * GlobalDefine.screenRatioWidth, currentTouch.pos.y / GlobalDefine.screenScale.y);
			dir = touchLocalPos - center;
			if (dir.sqrMagnitude > sqrRadius)
			{
				position = dir.normalized;
				SetInsideCircle(center + position * radius);
			}
			else
			{
				SetInsideCircle(touchLocalPos);
				position = dir / radius;
			}
		}
	}

	private void OnEnable()
	{
		isPressed = false;
		SetCenter(defaultCenter);
	}

	public void SetInsideCircle(Vector2 pos)
	{
		insideCircle.transform.localPosition = pos;
	}

	public void SetCenter(Vector2 pos)
	{
		center = pos;
		outsideCircle.transform.localPosition = center;
		insideCircle.transform.localPosition = center;
	}
}
