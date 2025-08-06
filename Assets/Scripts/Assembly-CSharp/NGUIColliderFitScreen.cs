using UnityEngine;

public class NGUIColliderFitScreen : MonoBehaviour
{
	public bool initFlag;

	private void Awake()
	{
		if (!initFlag)
		{
			initFlag = true;
			GlobalDefine.init();
			BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.size = new Vector3(GlobalDefine.screenScale.x * component.size.x / GlobalDefine.screenScale.y, component.size.y, 0f);
			}
		}
	}
}
