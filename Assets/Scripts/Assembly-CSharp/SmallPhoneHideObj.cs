using UnityEngine;

public class SmallPhoneHideObj : MonoBehaviour
{
	private void OnEnable()
	{
		if (GlobalDefine.smallPhoneFlag)
		{
			base.gameObject.SetActiveRecursively(false);
		}
	}
}
