using UnityEngine;

public class NGUIFitWord : MonoBehaviour
{
	public bool initFlag;

	private void Awake()
	{
		if (!initFlag)
		{
			initFlag = true;
			base.transform.localScale = new Vector3(base.transform.localScale.x * GlobalDefine.screenWordFit, base.transform.localScale.y, 1f);
		}
	}
}
