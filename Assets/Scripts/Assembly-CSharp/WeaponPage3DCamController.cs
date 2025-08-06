using UnityEngine;

public class WeaponPage3DCamController : MonoBehaviour
{
	public GameObject target;

	public GameObject nextTarget;

	public bool changeLeftFlag;

	public bool changeRightFlag;

	public float speedx = 0.5f;

	public float speedy = 0.5f;

	public float rotationX;

	public float rotationY;

	public float preRotationX;

	public float preRotationY;

	public float sensitivityX = 1f;

	public float sensitivityY = 1f;

	public float lerpRotationX;

	public float lerpRotationY;

	public bool flag;

	public bool lockCamera;

	public float lerpPercent;

	private void Start()
	{
	}

	private void Awake()
	{
		lerpPercent = 1f;
	}

	private void Update()
	{
		rotationX %= 360f;
		if (lockCamera)
		{
			if (!(lerpPercent >= 1f))
			{
				if (rotationX > 0f)
				{
					rotationX = Mathf.Lerp(lerpRotationX, 147.5f, lerpPercent);
					rotationY = Mathf.Lerp(lerpRotationY, 0f, lerpPercent);
				}
				else
				{
					rotationX = Mathf.Lerp(lerpRotationX, -212.5f, lerpPercent);
					rotationY = Mathf.Lerp(lerpRotationY, 0f, lerpPercent);
				}
				lerpPercent += Time.deltaTime * 1.5f;
				if (lerpPercent >= 1f)
				{
					lerpPercent = 1f;
				}
				target.transform.rotation = Quaternion.AngleAxis(rotationX, Vector3.up);
				target.transform.rotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
			}
			return;
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			preRotationX = rotationX;
			preRotationY = rotationY;
			rotationX -= Input.GetTouch(0).deltaPosition.x * sensitivityX;
			rotationY -= Input.GetTouch(0).deltaPosition.y * sensitivityY;
			speedx = rotationX - preRotationX;
			speedy = rotationY - preRotationY;
			flag = false;
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			flag = true;
		}
		if (Input.GetMouseButton(1))
		{
			preRotationX = rotationX;
			preRotationY = rotationY;
			rotationX -= Input.GetAxis("Mouse X") * sensitivityX;
			if ((rotationX > 90f && rotationX < 270f) || (rotationX < -90f && rotationX > -270f))
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			}
			else
			{
				rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
			}
			speedx = rotationX - preRotationX;
			speedy = rotationY - preRotationY;
			flag = false;
		}
		if (Input.GetMouseButtonUp(1))
		{
			flag = true;
		}
		if (flag)
		{
			rotationX += speedx;
			rotationY += speedy;
			speedx -= speedx / 20f;
			speedy -= speedy / 20f;
		}
		doRotate();
	}

	private void doRotate()
	{
		if (rotationY > 10f)
		{
			rotationY = 10f;
		}
		else if (rotationY < -10f)
		{
			rotationY = -10f;
		}
		target.transform.rotation = Quaternion.AngleAxis(rotationX, Vector3.up);
		target.transform.rotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
	}
}
