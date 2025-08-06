using UnityEngine;

public class ChangeScaleForScrollView : MonoBehaviour
{
	public Transform centerObj;

	public float disOfCenterObj;

	private void Start()
	{
	}

	private void Update()
	{
		disOfCenterObj = Mathf.Abs(centerObj.InverseTransformPoint(base.transform.position).x);
		base.transform.localScale = Vector3.one * (1.2f - disOfCenterObj / GlobalDefine.screenRatioWidth * 1.5f);
	}
}
