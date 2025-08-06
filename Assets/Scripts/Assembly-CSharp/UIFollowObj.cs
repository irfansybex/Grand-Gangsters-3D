using UnityEngine;

public class UIFollowObj : MonoBehaviour
{
	public GameObject targetObj;

	public Camera cam;

	private Vector3 pos;

	private void Start()
	{
	}

	private void LateUpdate()
	{
		pos = cam.WorldToViewportPoint(targetObj.transform.position);
		base.transform.localPosition = new Vector3(pos.x * GlobalDefine.screenRatioWidth, pos.y * 480f, 0f);
	}
}
