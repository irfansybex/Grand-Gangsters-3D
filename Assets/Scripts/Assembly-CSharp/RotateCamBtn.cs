using UnityEngine;

public class RotateCamBtn : MonoBehaviour
{
	//public GUITexture[] exceptArea;

	private int fingerId;

	private Touch tempTouch;

	public Touch touch;

	private Touch emptyTouch;

	public bool isTouchInPos;

//	public GUIText inPos;

	//public GUIText f;

//	public GUIText flag;

	private void Update()
	{
		if (Input.touchCount > 0)
		{
			if (fingerId == -1)
			{
				for (int i = 0; i < Input.touchCount; i++)
				{
					tempTouch = Input.GetTouch(i);
					if (tempTouch.phase == TouchPhase.Began && !isInExceptArea(tempTouch))
					{
						fingerId = tempTouch.fingerId;
						isTouchInPos = true;
					}
				}
				return;
			}
			for (int j = 0; j < Input.touchCount + 1; j++)
			{
				tempTouch = Input.GetTouch(j);
				if (tempTouch.fingerId == fingerId)
				{
					touch = tempTouch;
					break;
				}
			}
			if (tempTouch.phase == TouchPhase.Ended)
			{
				fingerId = -1;
				touch = emptyTouch;
				isTouchInPos = false;
			}
		}
		else
		{
			touch = emptyTouch;
			fingerId = -1;
			isTouchInPos = false;
		}
	}

	public bool isInExceptArea(Touch tInfo)
	{
		bool result = false;
		/*for (int i = 0; i < exceptArea.Length; i++)
		{
			if (isPosIn(tInfo.position, exceptArea[i].pixelInset, exceptArea[i].transform, null))
			{
				result = true;
				break;
			}
		}*/
		return result;
	}

	public bool isInExceptArea(Vector2 pos)
	{
		bool result = false;
		/*for (int i = 0; i < exceptArea.Length; i++)
		{
			if (isPosIn(pos, exceptArea[i].pixelInset, exceptArea[i].transform, null))
			{
				result = true;
				break;
			}
		}*/
		return result;
	}

	public bool isPosIn(Vector3 pos, Rect pixRect, Transform iconPos, Camera cam)
	{
		return isPosIn(new Vector2(pos.x, pos.y), pixRect, iconPos, cam);
	}

	private bool isPosIn(Vector2 pos, Rect pixRect, Transform iconPos, Camera cam)
	{
		Vector3 vector;
		if ((bool)cam)
		{
			if (iconPos.position.x < 0f || iconPos.position.x > 1f || iconPos.position.y < 0f || iconPos.position.y > 1f)
			{
				return false;
			}
			vector = new Vector3(cam.rect.x + iconPos.position.x * cam.rect.width, cam.rect.y + iconPos.position.y * cam.rect.height, 0f);
		}
		else
		{
			vector = iconPos.position;
		}
		float x;
		float x2;
		if (pixRect.width < 0f)
		{
			x = vector.x * (float)Screen.width + pixRect.x + pixRect.width;
			x2 = vector.x * (float)Screen.width + pixRect.x;
		}
		else
		{
			x = vector.x * (float)Screen.width + pixRect.x;
			x2 = vector.x * (float)Screen.width + pixRect.x + pixRect.width;
		}
		float y;
		float y2;
		if (pixRect.height > 0f)
		{
			y = vector.y * (float)Screen.height + pixRect.y;
			y2 = vector.y * (float)Screen.height + pixRect.height + pixRect.y;
		}
		else
		{
			y = vector.y * (float)Screen.height + pixRect.height + pixRect.y;
			y2 = vector.y * (float)Screen.height + pixRect.y;
		}
		Vector2 vector2 = new Vector2(x, y);
		Vector2 vector3 = new Vector2(x2, y2);
		return pos.x >= vector2.x && pos.x <= vector3.x && pos.y >= vector2.y && pos.y <= vector3.y;
	}
}
