using UnityEngine;

public class MyUIPanelFit : MonoBehaviour
{
	public bool initFlag;

	public UIPanel pannel;

	private void Awake()
	{
		if (!initFlag)
		{
			initFlag = true;
			pannel = base.gameObject.GetComponent<UIPanel>();
			GlobalDefine.init();
			if (pannel != null)
			{
				pannel.baseClipRegion = new Vector4(0f, 0f, pannel.width * GlobalDefine.screenWidthFit, pannel.height);
			}
		}
	}

	private void Update()
	{
	}
}
