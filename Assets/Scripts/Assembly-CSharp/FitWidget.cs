using UnityEngine;

public class FitWidget : MonoBehaviour
{
	public bool initFlag;

	public UIWidget widget;

	private void Awake()
	{
		if (!initFlag)
		{
			initFlag = true;
			GlobalDefine.init();
			widget = base.gameObject.GetComponent<UIWidget>();
			if (widget != null)
			{
				widget.width = (int)((float)widget.width * GlobalDefine.screenWidthFit);
			}
		}
	}
}
