using System;
using UnityEngine;

public class LevelUpUIController : MonoBehaviour
{
	public UIEventListener okBtn;

	private void Start()
	{
		UIEventListener uIEventListener = okBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickOKBtn));
	}

	private void OnEnable()
	{
	}

	public void OnClickOKBtn(GameObject btn)
	{
		GameSenceBackBtnCtl.instance.PopGameUIState();
		Controller.instance.levelUpFlag = false;
		GameUIController.instance.taskEndUIControllor.enableBtn = true;
		UnityEngine.Object.Destroy(base.gameObject);
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}
}
