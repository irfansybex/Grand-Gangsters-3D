using System;
using UnityEngine;

public class RatePageController : MonoBehaviour
{
	public static RatePageController instance;

	public UIEventListener closeBtn;

	public UIEventListener okBtn;

	public bool enablaBtnFlag;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		UIEventListener uIEventListener = closeBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickCloseBtn));
		UIEventListener uIEventListener2 = okBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickOKBtn));
		GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.RATE);
		enablaBtnFlag = false;
	}

	public void OnClickCloseBtn(GameObject btn)
	{
		if (enablaBtnFlag)
		{
			GameSenceBackBtnCtl.instance.PopGameUIState();
			if (instance == this)
			{
				instance = null;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			Resources.UnloadUnusedAssets();
			GC.Collect();
			Time.timeScale = 1f;
		}
	}

	public void OnClickOKBtn(GameObject btn)
	{
		if (enablaBtnFlag)
		{
			Platform.rating();
			GameSenceBackBtnCtl.instance.PopGameUIState();
			if (instance == this)
			{
				instance = null;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			Resources.UnloadUnusedAssets();
			GC.Collect();
			Time.timeScale = 1f;
		}
	}
}
