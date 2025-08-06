using UnityEngine;

public class MyJoyStick : MonoBehaviour
{
	public Vector2 position;

	private Vector2 center;

	//public GUITexture imgJoyButton;

	public Transform transJoyButton;

	//public GUITexture imgJoyBack;

	private Rect rectCenter;

	private float radius;

	private float sqrRadius;

	private float inverseRadius;

	public bool isInActiveState;

	private int fingerId = -1;

	private Vector2 centerInch = new Vector2(0.5f, 0.5f);

	private float joyBtSizeInch = 0.15f;

	private float radiusInch = 0.25f;

	private float[] inchSizeList = new float[3] { 4f, 4.4f, 5f };

	private float[] factorList = new float[3] { 0.7f, 0.8f, 0.9f };

	private void Start()
	{
		float dpi = Screen.dpi;
		dpi *= 2f;
		if (Application.platform != RuntimePlatform.Android)
		{
			dpi = 100f;
		}
		float num = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / dpi;
		float num2 = getFactorByInch(num);
		if (Application.platform != RuntimePlatform.Android)
		{
			num2 = ((!(num > 8f)) ? (num2 * 1f) : (num2 * 2f));
		}
		dpi *= num2;
		radius = dpi * radiusInch;
		sqrRadius = radius * radius;
		inverseRadius = 1f / radius;
		center = dpi * centerInch;
		float num3 = dpi * joyBtSizeInch;
		rectCenter = new Rect(center.x - num3, center.y - num3, num3 * 2f, num3 * 2f);
	//	imgJoyButton.pixelInset = new Rect((0f - rectCenter.width) * 0.5f, (0f - rectCenter.height) * 0.5f, rectCenter.width, rectCenter.height);
	//	imgJoyBack.pixelInset = new Rect(0f - radius, 0f - radius, radius * 2f, radius * 2f);
	//	imgJoyBack.transform.position = new Vector3(center.x * 1f / (float)Screen.width, center.y * 1f / (float)Screen.height, imgJoyBack.transform.position.z);
		setJoyButtonPos(center);
	}

	public int getFingerId()
	{
		return fingerId;
	}

	private float getFactorByInch(float inch)
	{
		for (int i = 0; i < inchSizeList.Length; i++)
		{
			if (inch < inchSizeList[i])
			{
				return factorList[i];
			}
		}
		return 1f;
	}

	private void OnDisable()
	{
		position = Vector3.zero;
	}

	private void Update()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
			{
				Vector2 vector = Input.mousePosition;
				if (rectCenter.Contains(vector))
				{
					onTouchEnter(vector);
				}
			}
			if (isInActiveState)
			{
				if (Input.GetMouseButton(0))
				{
					onTouchMove(Input.mousePosition);
				}
				if (Input.GetMouseButtonUp(0))
				{
					onTouchExit(Input.mousePosition);
				}
			}
		}
		else
		{
			if (Input.touchCount <= 0)
			{
				return;
			}
			for (int i = 0; i < Input.touchCount; i++)
			{
				if (fingerId == -1)
				{
					touchBehaviour(Input.GetTouch(i));
				}
				else if (Input.GetTouch(i).fingerId == fingerId)
				{
					touchBehaviour(Input.GetTouch(i));
					break;
				}
			}
		}
	}

	private void touchBehaviour(Touch touch)
	{
		if (touch.phase == TouchPhase.Began)
		{
			Vector2 vector = touch.position;
			if (rectCenter.Contains(vector))
			{
				onTouchEnter(vector);
				fingerId = touch.fingerId;
			}
		}
		if (isInActiveState)
		{
			if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
			{
				onTouchMove(touch.position);
			}
			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{
				onTouchExit(touch.position);
				fingerId = -1;
			}
		}
	}

	private void onTouchEnter(Vector2 pos)
	{
		isInActiveState = true;
	}

	private void onTouchMove(Vector2 pos)
	{
		Vector2 vector = pos - center;
		if (vector.sqrMagnitude > sqrRadius)
		{
			Vector2 normalized = vector.normalized;
			setJoyButtonPos(normalized * radius + center);
			position = normalized;
		}
		else
		{
			setJoyButtonPos(pos);
			position = vector * inverseRadius;
		}
	}

	private void onTouchExit(Vector2 pos)
	{
		isInActiveState = false;
		setJoyButtonPos(center);
		position = Vector2.zero;
	}

	private void setJoyButtonPos(Vector2 pos)
	{
		transJoyButton.position = new Vector3(pos.x * 1f / (float)Screen.width, pos.y * 1f / (float)Screen.height, transJoyButton.position.z);
	}
}
