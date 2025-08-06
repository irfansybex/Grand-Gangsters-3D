using UnityEngine;

public class MyNGUIFitPos : MonoBehaviour
{
	public Vector2 pos;

	private void Start()
	{
		base.transform.localPosition = new Vector3(pos.x * GlobalDefine.screenRatioWidth, pos.y * 480f, 0f);
		MonoBehaviour.print("fitPos : " + base.transform.localPosition);
	}

	private void Update()
	{
	}
}
