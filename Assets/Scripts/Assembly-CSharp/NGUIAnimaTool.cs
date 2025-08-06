using UnityEngine;

[ExecuteInEditMode]
public class NGUIAnimaTool : MonoBehaviour
{
	public bool flag;

	public Vector3 pos;

	public Vector3 rot;

	public Vector3 sc;

	public Vector3 finalPos;

	public Vector3 finalRot;

	public Vector3 finalSc;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		if (flag)
		{
			base.transform.localPosition = new Vector3(pos.x * GlobalDefine.screenRatioWidth, pos.y * 480f, 0f);
			base.transform.localEulerAngles = rot;
			base.transform.localScale = sc;
		}
	}

	public void SetFinalState()
	{
		base.transform.localPosition = new Vector3(finalPos.x * GlobalDefine.screenRatioWidth, finalPos.y * 480f, 0f);
		base.transform.localEulerAngles = finalRot;
		base.transform.localScale = finalSc;
	}
}
