using System;
using UnityEngine;

public class ExitUIController : MonoBehaviour
{
	public GameObject exitUIObj;

	public UIEventListener okBtn;

	public UIEventListener cancelBtn;

	public UIEventListener moreGamesBtn;

	public GameObject topLabel;

	private void Awake()
	{
		UIEventListener uIEventListener = okBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickOKBtn));
		UIEventListener uIEventListener2 = cancelBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickCancelBtn));
		UIEventListener uIEventListener3 = moreGamesBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickMoreGamesBtn));
	}

	private void OnEnable()
	{
		exitUIObj.gameObject.SetActiveRecursively(true);
	}

	private void OnDisable()
	{
		if (exitUIObj != null)
		{
			exitUIObj.gameObject.SetActiveRecursively(false);
		}
	}

	public void Reset(bool showADFlag)
	{
		if (showADFlag)
		{
			Platform.showFullScreenSmallExit();
			okBtn.transform.localPosition = new Vector3(GlobalDefine.screenRatioWidth * -0.2f, -200f, 0f);
			cancelBtn.transform.localPosition = new Vector3(GlobalDefine.screenRatioWidth * 0.2f, -200f, 0f);
			topLabel.transform.localPosition = new Vector3(0f, 170f, 0f);
			moreGamesBtn.transform.localPosition = new Vector3(0f, -200f, 0f);
		}
		else
		{
			okBtn.transform.localPosition = new Vector3(GlobalDefine.screenRatioWidth * -0.2f, -50f, 0f);
			cancelBtn.transform.localPosition = new Vector3(GlobalDefine.screenRatioWidth * 0.2f, -50f, 0f);
			moreGamesBtn.transform.localPosition = new Vector3(0f, -50f, 0f);
			topLabel.transform.localPosition = new Vector3(0f, 50f, 0f);
		}
	}

	public void OnClickOKBtn(GameObject btn)
	{
		Platform.SetNotification();
		Application.Quit();
	}

	public void OnClickCancelBtn(GameObject btn)
	{
		MenuSenceController.instance.OnDisableExitUI();
	}

	public void OnClickMoreGamesBtn(GameObject btn)
	{
		Platform.showMoreGames();
	}
}
