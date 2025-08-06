using UnityEngine;

public class MyButton : MonoBehaviour
{
	public delegate void NormalMethod(int thisID);

	public delegate void ActivedMethod(int thisID);

	public delegate void DisableMethod(int thisID);

	public delegate void onBtnClick();

	public delegate void onTouchEnter();

	public delegate void onTouchEnterLeave();

	public ButtonActivedType activedType = ButtonActivedType.ChangeTexture;

	protected Texture normalTexture;

	public Texture activatedTexture;

	public Vector3 activedScale = new Vector3(0.01f, 0.01f, 0f);

	public Color32 activedColor = new Color32(64, 64, 64, 128);

	public Color32 normalColor = new Color32(128, 128, 128, 128);

	public float offset;

	public bool isClick;

	public bool isTouchEnter;

	public bool isTwoTouch;

	protected Rect normalPixelInset;

	protected Rect activatedPixelInset;

	protected bool isInit;

	private Vector3 normalScale = Vector3.zero;

	public ButtonState state;

	public NormalMethod normalMethod;

	public ActivedMethod activedMethod;

	public DisableMethod disableMethod;

	protected static int idCount;

	public int id;

	public bool isPressSound = true;

	public Camera renderCamera;

	public onBtnClick OnBtnClick;

	public onTouchEnter OnTouchEnter;

	public onTouchEnterLeave OnTouchEnterLeave;

	public int fingerId = -1;

	public Touch touchInf;

	private Touch emptyTouch;

	private void Start()
	{
		init();
	}

	private void Update()
	{
		if (state == ButtonState.Disable || Input.touchCount <= 0)
		{
			return;
		}
		if (Input.touchCount == 1)
		{
			touchBehaviour(Input.GetTouch(0));
		}
		else
		{
			if (Input.touchCount != 2 || !isTwoTouch)
			{
				return;
			}
			if (fingerId != -1)
			{
				touchBehaviour(Input.GetTouch(fingerId));
				return;
			}
			touchBehaviour(Input.GetTouch(1));
			if (fingerId == -1)
			{
				touchBehaviour(Input.GetTouch(0));
			}
		}
	}

	protected void init()
	{
		if (!isInit)
		{
			/*if (!base.GetComponent<>)
			{
				Debug.LogError("you should add a GUITexture component to this object:" + base.gameObject.name);
			}
			normalTexture = base.guiTexture.texture;
			if (base.guiTexture.pixelInset.width != 0f)
			{
				textureScale();
				normalPixelInset = base.guiTexture.pixelInset;
			}
			else
			{
				normalPixelInset = new Rect(0f - (float)Screen.width * base.transform.localScale.x / 2f, 0f - (float)Screen.height * base.transform.localScale.y / 2f, (float)Screen.width * base.transform.localScale.x, (float)Screen.height * base.transform.localScale.y);
			}
			activatedPixelInset = new Rect(normalPixelInset.x, normalPixelInset.y + offset, normalPixelInset.width, normalPixelInset.height);
			id = idCount++;
			isInit = true;
			emptyTouch = default(Touch);*/
		}
	}

	protected void textureScale()
	{
	/*	float left = base.guiTexture.pixelInset.x * GlobalDefine.screenScale.x;
		float top = base.guiTexture.pixelInset.y * GlobalDefine.screenScale.y;
		float width = base.guiTexture.pixelInset.width * GlobalDefine.screenScale.x;
		float height = base.guiTexture.pixelInset.height * GlobalDefine.screenScale.y;
		base.guiTexture.pixelInset = new Rect(left, top, width, height);*/
	}

	public void show()
	{
		base.gameObject.SetActiveRecursively(true);
		init();
	}

	public void hide()
	{
		base.gameObject.SetActiveRecursively(false);
	}

	public void changeMainTexture(Texture texture)
	{
		init();
		normalTexture = texture;
		//base.guiTexture.texture = normalTexture;
	}

	public void setState(ButtonState newState)
	{
		init();
		if (state != newState)
		{
			state = newState;
			switch (state)
			{
			case ButtonState.Normal:
				setNormal();
				break;
			case ButtonState.Actived:
				SetActiveRecursivelyd();
				break;
			case ButtonState.Disable:
				setDisable();
				break;
			}
		}
	}

	private void setNormal()
	{
		if (activedType == ButtonActivedType.None)
		{
			return;
		}
		if (activedType == ButtonActivedType.Extend)
		{
			if (normalMethod != null)
			{
				normalMethod(id);
			}
			return;
		}
		if (activedType == ButtonActivedType.Animated)
		{
			base.GetComponent<Animation>().Stop();
		}
	//	base.guiTexture.texture = normalTexture;
		base.transform.localScale = normalScale;
	//	base.guiTexture.color = normalColor;
		//base.guiTexture.pixelInset = normalPixelInset;
	}

	private void SetActiveRecursivelyd()
	{
		switch (activedType)
		{
		case ButtonActivedType.None:
			return;
		case ButtonActivedType.ChangeTexture:
		//	base.guiTexture.texture = activatedTexture;
			break;
		case ButtonActivedType.ChangeScale:
			base.transform.localScale = activedScale;
			break;
		case ButtonActivedType.ChangeColor:
			//base.guiTexture.color = activedColor;
			break;
		case ButtonActivedType.Extend:
			if (activedMethod != null)
			{
				activedMethod(id);
			}
			return;
		case ButtonActivedType.Animated:
			if ((bool)base.GetComponent<Animation>())
			{
				base.GetComponent<Animation>().Play();
			}
			return;
		}
		//base.guiTexture.pixelInset = activatedPixelInset;
	}

	private void setDisable()
	{
		if (activedType == ButtonActivedType.Extend)
		{
			if (disableMethod != null)
			{
				disableMethod(id);
			}
		}
		else if (activedType == ButtonActivedType.ChangeTexture)
		{
			//base.guiTexture.texture = activatedTexture;
		}
		else if (activedType != ButtonActivedType.ChangeScale && activedType == ButtonActivedType.ChangeColor)
		{
		//	base.guiTexture.color = activedColor;
		}
	}

	protected void touchBehaviour(Touch touch)
	{
		bool flag = isPosIn(touch.position, renderCamera);
		if (touch.phase == TouchPhase.Began && flag)
		{
			setState(ButtonState.Actived);
			isTouchEnter = true;
			if (OnTouchEnter != null)
			{
				OnTouchEnter();
			}
			if (fingerId == -1)
			{
				fingerId = touch.fingerId;
				touchInf = touch;
			}
		}
		if (touch.phase == TouchPhase.Moved)
		{
			if (isTouchEnter)
			{
				setState(ButtonState.Actived);
				touchInf = touch;
			}
		}
		else
		{
			touchInf = emptyTouch;
		}
		if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
		{
			return;
		}
		if (isTouchEnter)
		{
			if (OnBtnClick != null)
			{
				OnBtnClick();
			}
			if (fingerId != -1)
			{
				fingerId = -1;
				touchInf = emptyTouch;
			}
		}
		setState(ButtonState.Normal);
		isTouchEnter = false;
		if (OnTouchEnterLeave != null)
		{
			OnTouchEnterLeave();
		}
	}

	protected void updateWindowsInput()
	{
		bool flag = isPosIn(Input.mousePosition, renderCamera);
		if (Input.GetMouseButtonDown(0) && flag)
		{
			setState(ButtonState.Actived);
			isTouchEnter = true;
			if (OnTouchEnter != null)
			{
				OnTouchEnter();
			}
		}
		if (Input.GetMouseButton(0))
		{
			if (isTouchEnter && flag)
			{
				setState(ButtonState.Actived);
			}
			else
			{
				setState(ButtonState.Normal);
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (isTouchEnter && isPosIn(Input.mousePosition, renderCamera) && OnBtnClick != null)
			{
				OnBtnClick();
			}
			setState(ButtonState.Normal);
			isTouchEnter = false;
			if (OnTouchEnterLeave != null)
			{
				OnTouchEnterLeave();
			}
		}
	}

	public bool isPosIn(Vector3 pos, Camera cam)
	{
		return isPosIn(new Vector2(pos.x, pos.y), cam);
	}

	private bool isPosIn(Vector2 pos, Camera cam)
	{
		Vector3 vector;
		if ((bool)cam)
		{
			if (base.transform.position.x < 0f || base.transform.position.x > 1f || base.transform.position.y < 0f || base.transform.position.y > 1f)
			{
				return false;
			}
			vector = new Vector3(cam.rect.x + base.transform.position.x * cam.rect.width, cam.rect.y + base.transform.position.y * cam.rect.height, 0f);
		}
		else
		{
			vector = base.transform.position;
		}
		float x;
		float x2;
		if (normalPixelInset.width < 0f)
		{
			x = vector.x * (float)Screen.width + normalPixelInset.x + normalPixelInset.width;
			x2 = vector.x * (float)Screen.width + normalPixelInset.x;
		}
		else
		{
			x = vector.x * (float)Screen.width + normalPixelInset.x;
			x2 = vector.x * (float)Screen.width + normalPixelInset.x + normalPixelInset.width;
		}
		float y;
		float y2;
		if (normalPixelInset.height > 0f)
		{
			y = vector.y * (float)Screen.height + normalPixelInset.y;
			y2 = vector.y * (float)Screen.height + normalPixelInset.height + normalPixelInset.y;
		}
		else
		{
			y = vector.y * (float)Screen.height + normalPixelInset.height + normalPixelInset.y;
			y2 = vector.y * (float)Screen.height + normalPixelInset.y;
		}
		Vector2 vector2 = new Vector2(x, y);
		Vector2 vector3 = new Vector2(x2, y2);
		return pos.x >= vector2.x && pos.x <= vector3.x && pos.y >= vector2.y && pos.y <= vector3.y;
	}
}
