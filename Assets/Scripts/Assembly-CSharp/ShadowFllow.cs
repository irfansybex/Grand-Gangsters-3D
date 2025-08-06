using UnityEngine;

public class ShadowFllow : MonoBehaviour
{
	private void LateUpdate()
	{
		base.transform.position = base.transform.parent.position - (base.transform.parent.position.y - 0.05f) * Vector3.up;
		base.transform.eulerAngles = new Vector3(0f, base.transform.parent.eulerAngles.y, 0f);
	}
}
