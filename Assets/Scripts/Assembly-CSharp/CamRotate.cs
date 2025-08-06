using System;
using UnityEngine;
using UnityEngine.UI;

public class CamRotate : MonoBehaviour
{
	public RotateCamBtn rotateArea;

	public SmoothFollow cam;

	public GameObject target;

	public PlayerController player;

	public float height;

	private Vector3 camTargetPosXZ;

	public Vector3 camPosXZ;

	private Transform m_transform;

	private float targetAngle;

	private float lerpAngle;

	public Transform tempTarget;

	public bool onChangeFlag;

	public float lerpPercent;

	private Vector3 startLerpPos;

	private Quaternion startLerpQua;

	public bool inTheCarFlag;

	public UIEventListener rotateAreaBtn;

	public bool isPress;

	public bool isDrag;

	public Vector2 dragDeta;

	public Vector2 screenScale;

	public Text t;

	public Text R;

	private void Awake()
	{
		height = cam.height;
		m_transform = base.transform;
		tempTarget = player.transform;
		UIEventListener uIEventListener = rotateAreaBtn;
		uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressArea));
		UIEventListener uIEventListener2 = rotateAreaBtn;
		uIEventListener2.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener2.onDrag, new UIEventListener.VectorDelegate(OnDragCam));
		screenScale = new Vector2(800f / (float)Screen.width, 480f / (float)Screen.height);
	}

	public void OnDragCam(GameObject obj, Vector2 deta)
	{
		dragDeta = deta;
		isDrag = true;
		PlayerController.instance.fireTarget = null;
		if (PlayerController.instance.curState == PLAYERSTATE.FIGHT)
		{
			PlayerController.instance.ChangeState(PLAYERSTATE.NORMAL);
		}
		else if (PlayerController.instance.curState != PLAYERSTATE.CAR && (PlayerController.instance.animaCtl.isAimFlag || PlayerController.instance.animaCtl.needToAimFlag))
		{
			PlayerController.instance.ChangeState(PlayerController.instance.curState);
		}
	}

	public void OnPressArea(GameObject obj, bool press)
	{
		isPress = press;
		if (!press)
		{
			dragDeta = Vector2.zero;
		}
	}

	private void LateUpdate()
	{
		isDrag = false;
	}

	public void NewRotate()
	{
		if (player.curState == PLAYERSTATE.DIE || GlobalInf.playerRagdollFlag)
		{
			return;
		}
		if (!inTheCarFlag)
		{
			if (isPress)
			{
				if (isDrag)
				{
					height = Mathf.Lerp(height, dragDeta.y * -0.1f + height, Time.deltaTime * 5f);
					height = Mathf.Clamp(height, 0f, cam.raduis / 2f);
					cam.SetHeight(height);
					target.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.y + dragDeta.x * 2f, Time.deltaTime * 5f), 0f);
				}
				return;
			}
			if (player.fireTarget == null)
			{
				camTargetPosXZ = new Vector3(target.transform.position.x, 0f, target.transform.position.z);
				camPosXZ = new Vector3(m_transform.position.x, 0f, m_transform.position.z);
				target.transform.forward = Vector3.Lerp(target.transform.forward, (camTargetPosXZ + target.transform.right * target.transform.InverseTransformDirection(player.GetComponent<Rigidbody>().velocity).x * 0.25f - camPosXZ).normalized, Time.deltaTime * 5f);
				return;
			}
			targetAngle = GetXZEulerAngle(target.transform.forward, player.animaCtl.hitDir, Vector3.up);
			if (targetAngle > 20f)
			{
				targetAngle -= 20f;
			}
			else if (targetAngle < -20f)
			{
				targetAngle += 20f;
			}
			else
			{
				targetAngle = 0f;
			}
			target.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.y + targetAngle, Time.deltaTime * 15f), 0f);
		}
		else
		{
			if (isDrag)
			{
				height = cam.tempHeight;
				height = Mathf.Lerp(height, dragDeta.y * -0.1f + height, Time.deltaTime * 5f);
				height = Mathf.Clamp(height, 1f, 5f);
				cam.tempHeight = height;
				tempTarget.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(tempTarget.transform.rotation.eulerAngles.y, tempTarget.transform.rotation.eulerAngles.y + dragDeta.x * 2f, Time.deltaTime * 10f), 0f);
			}
			else
			{
				cam.tempHeight = 1.5f;
			}
			if (isPress)
			{
				cam.tempHeight = height;
				player.car.lockCamFlag = true;
			}
			else
			{
				player.car.lockCamFlag = false;
			}
		}
	}

	public void OnChangeTartgetRightNow(bool isCarFlag)
	{
		inTheCarFlag = isCarFlag;
		if (isCarFlag)
		{
			tempTarget = player.car.camLookTra;
			tempTarget.rotation = target.transform.rotation;
		}
		else
		{
			tempTarget = player.transform;
		}
		onChangeFlag = false;
		cam.OnChangeTarget(isCarFlag);
	}

	public void OnChangeTarget(bool isCarFlag)
	{
		inTheCarFlag = isCarFlag;
		if (isCarFlag)
		{
			tempTarget = player.car.camLookTra;
			tempTarget.rotation = target.transform.rotation;
		}
		else
		{
			tempTarget = player.transform;
		}
		onChangeFlag = true;
		lerpPercent = 0f;
		startLerpPos = target.transform.position;
		startLerpQua = target.transform.rotation;
		cam.OnChangeTarget(isCarFlag);
	}

	public void OnChangePlayerRagdollTarget()
	{
		inTheCarFlag = false;
		tempTarget = player.animaCtl.transform.GetChild(0);
	}

	private void Update()
	{
		NewRotate();
		if (GlobalInf.playerRagdollFlag)
		{
			target.transform.position = Vector3.Lerp(target.transform.position, tempTarget.position, 0.5f);
		}
		else if (onChangeFlag)
		{
			lerpPercent += Time.deltaTime * 2f;
			if (inTheCarFlag)
			{
				target.transform.position = Vector3.Lerp(startLerpPos, tempTarget.position, lerpPercent);
			}
			else
			{
				target.transform.position = Vector3.Lerp(startLerpPos, tempTarget.position + Vector3.up * 1.5f, lerpPercent);
			}
			target.transform.rotation = Quaternion.Lerp(startLerpQua, tempTarget.rotation, lerpPercent);
			if (lerpPercent >= 1f)
			{
				onChangeFlag = false;
				lerpPercent = 0f;
				height = cam.tempHeight;
			}
		}
		else if (!GlobalInf.playerRagdollFlag)
		{
			if (!inTheCarFlag)
			{
				target.transform.position = tempTarget.transform.position + Vector3.up * 1.5f;
				return;
			}
			target.transform.position = tempTarget.position;
			target.transform.rotation = tempTarget.rotation;
		}
		else
		{
			target.transform.position = Vector3.Lerp(target.transform.position, tempTarget.position, 0.5f);
		}
	}

	public void UnityRotate()
	{
		if (!inTheCarFlag)
		{
			if (Input.GetMouseButton(1))
			{
				height = Mathf.Lerp(height, Input.GetAxis("Mouse Y") * -0.2f + height, Time.deltaTime * 5f);
				height = Mathf.Clamp(height, 0f, cam.raduis / 2f);
				cam.SetHeight(height);
				target.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * 100f, Time.deltaTime * 10f), 0f);
				return;
			}
			if (player.fireTarget == null)
			{
				camTargetPosXZ = new Vector3(target.transform.position.x, 0f, target.transform.position.z);
				camPosXZ = new Vector3(m_transform.position.x, 0f, m_transform.position.z);
				target.transform.forward = (camTargetPosXZ + target.transform.right * target.transform.InverseTransformDirection(player.GetComponent<Rigidbody>().velocity).x * 0.25f - camPosXZ).normalized;
				return;
			}
			targetAngle = GetXZEulerAngle(target.transform.forward, player.animaCtl.hitDir, Vector3.up);
			if (targetAngle > 20f)
			{
				targetAngle -= 20f;
			}
			else if (targetAngle < -20f)
			{
				targetAngle += 20f;
			}
			else
			{
				targetAngle = 0f;
			}
			target.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.y + targetAngle, Time.deltaTime * 15f), 0f);
		}
		else
		{
			if (Input.GetMouseButtonDown(1))
			{
				height = cam.tempHeight;
			}
			if (Input.GetMouseButton(1))
			{
				height = Mathf.Lerp(height, Input.GetAxis("Mouse Y") * -1f + height, Time.deltaTime * 5f);
				height = Mathf.Clamp(height, 1f, 5f);
				cam.tempHeight = height;
				tempTarget.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(tempTarget.transform.rotation.eulerAngles.y, tempTarget.transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * 100f, Time.deltaTime * 10f), 0f);
				player.car.lockCamFlag = true;
			}
			else
			{
				cam.tempHeight = 1.5f;
				player.car.lockCamFlag = false;
			}
		}
	}

	public void AndroidRotate()
	{
		if (!inTheCarFlag)
		{
			if (rotateArea.isTouchInPos)
			{
				height = Mathf.Lerp(height, rotateArea.touch.deltaPosition.y * -0.2f + height, Time.deltaTime * 5f);
				height = Mathf.Clamp(height, 0f, cam.raduis / 2f);
				cam.SetHeight(height);
				target.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.y + rotateArea.touch.deltaPosition.x * 5f, Time.deltaTime * 5f), 0f);
				return;
			}
			if (player.fireTarget == null)
			{
				camTargetPosXZ = new Vector3(target.transform.position.x, 0f, target.transform.position.z);
				camPosXZ = new Vector3(m_transform.position.x, 0f, m_transform.position.z);
				target.transform.forward = (camTargetPosXZ + target.transform.right * target.transform.InverseTransformDirection(player.GetComponent<Rigidbody>().velocity).x * 0.25f - camPosXZ).normalized;
				return;
			}
			targetAngle = GetXZEulerAngle(target.transform.forward, player.animaCtl.hitDir, Vector3.up);
			if (targetAngle > 20f)
			{
				targetAngle -= 20f;
			}
			else if (targetAngle < -20f)
			{
				targetAngle += 20f;
			}
			else
			{
				targetAngle = 0f;
			}
			target.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.y + targetAngle, Time.deltaTime * 15f), 0f);
		}
		else if (rotateArea.isTouchInPos)
		{
			height = cam.tempHeight;
			height = Mathf.Lerp(height, rotateArea.touch.deltaPosition.y * -0.2f + height, Time.deltaTime * 5f);
			height = Mathf.Clamp(height, 1f, 5f);
			cam.tempHeight = height;
			tempTarget.transform.rotation = Quaternion.Euler(0f, Mathf.Lerp(tempTarget.transform.rotation.eulerAngles.y, tempTarget.transform.rotation.eulerAngles.y + rotateArea.touch.deltaPosition.x * 2f, Time.deltaTime * 10f), 0f);
			player.car.lockCamFlag = true;
		}
		else
		{
			cam.tempHeight = 1.5f;
			player.car.lockCamFlag = false;
		}
	}

	private float GetXZEulerAngle(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA -= Vector3.Project(dirA, axis);
		dirB -= Vector3.Project(dirB, axis);
		float num = Vector3.Angle(dirA, dirB);
		return num * (float)((!(Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0f)) ? 1 : (-1));
	}
}
