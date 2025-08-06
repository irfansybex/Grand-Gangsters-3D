using UnityEngine;

public class TargetLabelPicPos : MonoBehaviour
{
	public GameObject label;

	private void Update()
	{
		if (base.transform.localPosition.y == 400f)
		{
			base.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
			label.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
		}
		else if (base.transform.localPosition.y == 30f)
		{
			base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			label.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		}
		else if (base.transform.localPosition.x == 40f)
		{
			base.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
			label.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
		}
		else if (base.transform.localPosition.x == GlobalDefine.screenRatioWidth - 40f)
		{
			base.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
			label.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
		}
		else
		{
			base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			label.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		}
	}
}
