using UnityEngine;

public class MinimapTest : MonoBehaviour
{
	public Renderer minimap;

	public GameObject miniMapObj;

	public Transform angle;

	private void Start()
	{
	}

	private void Update()
	{
		float x = (PlayerController.instance.transform.position.x - (float)CitySenceController.instance.virtualMapController.blockMap.startX) / 4f;
		float z = (PlayerController.instance.transform.position.z - (float)CitySenceController.instance.virtualMapController.blockMap.startY) / 4f;
		miniMapObj.transform.localPosition = new Vector3(x, -100f, z);
		miniMapObj.transform.localEulerAngles = new Vector3(90f, Mathf.LerpAngle(miniMapObj.transform.localEulerAngles.y, angle.eulerAngles.y, Time.deltaTime * 5f), 0f);
	}
}
