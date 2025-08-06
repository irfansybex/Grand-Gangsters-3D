using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	public Transform target;

	public float distance;

	public float height;

	public float heightDamping;

	public float rotationDamping;

	private float wantedRotationAngle;

	private float wantedHeight;

	private float currentRotationAngle;

	private float currentHeight;

	private Quaternion currentRotation;

	private Transform m_Transform;

	public float raduis;

	public float tempHeight;

	public bool onChangeFlag;

	public float onChangePercent;

	public float startDistance;

	public float targetDistance;

	private Vector3 dir;

	private void Awake()
	{
		m_Transform = base.transform;
		tempHeight = height;
		raduis = height + distance;
		distance = raduis - height / 2f;
	}

	private void LateUpdate()
	{
		if (target == null)
		{
			return;
		}
		if (onChangeFlag)
		{
			onChangePercent += Time.deltaTime;
			distance = Mathf.Lerp(startDistance, targetDistance, onChangePercent);
			if (onChangePercent >= 1f)
			{
				onChangeFlag = false;
			}
		}
		wantedRotationAngle = target.eulerAngles.y;
		wantedHeight = target.position.y + Mathf.Clamp(0f - (target.transform.forward * distance).y, 0f, 999f) + tempHeight;
		currentRotationAngle = m_Transform.eulerAngles.y;
		currentHeight = m_Transform.position.y;
		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		currentRotation = Quaternion.Euler(0f, currentRotationAngle, 0f);
		m_Transform.position = target.position;
		m_Transform.position -= currentRotation * Vector3.forward * distance;
		m_Transform.position = new Vector3(m_Transform.position.x, currentHeight, m_Transform.position.z);
		base.transform.LookAt(target);
	}

	public void SetHeight(float val)
	{
		tempHeight = val;
		distance = raduis - tempHeight / 2f;
	}

	public void OnChangeTarget(bool isCarFlag)
	{
		startDistance = distance;
		if (isCarFlag)
		{
			tempHeight = 1.5f;
			targetDistance = 8f;
		}
		else
		{
			tempHeight = 0.5f;
			targetDistance = 3.5f;
		}
		onChangeFlag = true;
		onChangePercent = 0f;
	}
}
