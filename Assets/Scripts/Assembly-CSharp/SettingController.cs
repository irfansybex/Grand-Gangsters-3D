using System;
using UnityEngine;

public class SettingController : MonoBehaviour
{
	public static SettingController instance;

	public UIEventListener gravityBtn;

	public UIEventListener handleBtn;

	public UIToggle gravityBtnToggle;

	public UIToggle handleBtnToggle;

	public UIEventListener voiceBtn;

	public UIEventListener controlBtn;

	public UIEventListener creditsBtn;

	public GameObject auidioPageRoot;

	public GameObject drivingPageRoot;

	public GameObject creditsRoot;

	public UIEventListener backBtn;

	public GameObject gravityPicRoot;

	public GameObject handlePicRoot;

	public UIEventListener musicBtn;

	public UIEventListener soundBtn;

	public UIEventListener notifiBtn;

	public GameObject musicBtnMaskObj;

	public GameObject soundBtnMaskObj;

	public GameObject notifiBtnMaskObj;

	private bool startFlag;

	private void Start()
	{
		UIEventListener uIEventListener = gravityBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickGravityBtn));
		UIEventListener uIEventListener2 = handleBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickHandleBtn));
		UIEventListener uIEventListener3 = voiceBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickVoiceBtn));
		UIEventListener uIEventListener4 = controlBtn;
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickControlBtn));
		UIEventListener uIEventListener5 = creditsBtn;
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnClickCreditsBtn));
		UIEventListener uIEventListener6 = backBtn;
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnClickBackBtn));
		UIEventListener uIEventListener7 = notifiBtn;
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener7.onClick, new UIEventListener.VoidDelegate(OnClickNotificationBtn));
		UIEventListener uIEventListener8 = musicBtn;
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener8.onClick, new UIEventListener.VoidDelegate(OnClickMusicBtn));
		UIEventListener uIEventListener9 = soundBtn;
		uIEventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener9.onClick, new UIEventListener.VoidDelegate(OnClickSoundBtn));
		if (instance == null)
		{
			instance = this;
		}
		Platform.showFeatureView(FeatureViewPosType.MIDDLE);
		startFlag = false;
	}

	private void Update()
	{
		if (!startFlag)
		{
			startFlag = true;
			OnInitVoicePage();
		}
	}

	public void OnClickBackBtn(GameObject obj)
	{
		if (instance == this)
		{
			instance = null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		Resources.UnloadUnusedAssets();
		if (MenuSenceBackBtnCtl.instance != null)
		{
			MenuSenceBackBtnCtl.instance.PopMenuUIState();
			MenuSenceController.instance.startMenu.startMenuObj.gameObject.SetActiveRecursively(true);
			MenuSenceController.instance.startMenu.startBackGroundObj.gameObject.SetActiveRecursively(true);
			MenuSenceController.instance.startMenu.CheckSignal();
			Platform.hideFeatureView();
			MenuSenceController.instance.startMenu.dailyBounseDisableObj.gameObject.SetActiveRecursively(false);
			if (Platform.limitedTimeOfferFlag)
			{
				MenuSenceController.instance.startMenu.saleRoot.gameObject.SetActiveRecursively(true);
			}
			else
			{
				MenuSenceController.instance.startMenu.saleRoot.gameObject.SetActiveRecursively(false);
			}
		}
		else if (GameSenceBackBtnCtl.instance != null)
		{
			GameSenceBackBtnCtl.instance.PopGameUIState();
			GameUIController.instance.pauseUIControllor.gameObject.SetActiveRecursively(true);
			GameUIController.instance.pauseUIControllor.ResetPauseUI();
		}
	}

	public void OnInitVoicePage()
	{
		auidioPageRoot.gameObject.SetActiveRecursively(true);
		drivingPageRoot.gameObject.SetActiveRecursively(false);
		creditsRoot.gameObject.SetActiveRecursively(false);
		if (GlobalInf.soundFlag)
		{
			soundBtnMaskObj.gameObject.SetActiveRecursively(true);
		}
		else
		{
			soundBtnMaskObj.gameObject.SetActiveRecursively(false);
		}
		if (GlobalInf.musicFlag)
		{
			musicBtnMaskObj.SetActiveRecursively(true);
		}
		else
		{
			musicBtnMaskObj.SetActiveRecursively(false);
		}
		if (GlobalInf.notificationFlag)
		{
			notifiBtnMaskObj.SetActiveRecursively(true);
		}
		else
		{
			notifiBtnMaskObj.SetActiveRecursively(false);
		}
	}

	public void OnClickSoundBtn(GameObject btn)
	{
		if (GlobalInf.soundFlag)
		{
			GlobalInf.soundFlag = false;
			soundBtnMaskObj.SetActiveRecursively(false);
			AudioController.instance.turnOffSound();
			StoreDateController.SetSoundVolume(0f);
		}
		else
		{
			GlobalInf.soundFlag = true;
			soundBtnMaskObj.SetActiveRecursively(true);
			AudioController.instance.turnOnSound();
			StoreDateController.SetSoundVolume(1f);
		}
	}

	public void OnClickMusicBtn(GameObject btn)
	{
		if (GlobalInf.musicFlag)
		{
			GlobalInf.musicFlag = false;
			musicBtnMaskObj.SetActiveRecursively(false);
			AudioController.instance.turnOffMusic();
			StoreDateController.SetMusicVolume(0f);
		}
		else
		{
			GlobalInf.musicFlag = true;
			musicBtnMaskObj.SetActiveRecursively(true);
			AudioController.instance.turnOnMusic();
			StoreDateController.SetMusicVolume(1f);
		}
	}

	public void OnClickNotificationBtn(GameObject btn)
	{
		if (GlobalInf.notificationFlag)
		{
			GlobalInf.notificationFlag = false;
			notifiBtnMaskObj.SetActiveRecursively(false);
			StoreDateController.SetNotificationFlag(false);
		}
		else
		{
			GlobalInf.notificationFlag = true;
			notifiBtnMaskObj.SetActiveRecursively(true);
			StoreDateController.SetNotificationFlag(true);
		}
	}

	public void ResetControlBtn()
	{
		if (GlobalInf.carCtrlType == CARCTRLTYPE.BUTTON)
		{
			handleBtnToggle.value = true;
			handlePicRoot.SetActiveRecursively(true);
			gravityPicRoot.SetActiveRecursively(false);
		}
		else
		{
			gravityBtnToggle.value = true;
			gravityPicRoot.SetActiveRecursively(true);
			handlePicRoot.SetActiveRecursively(false);
		}
	}

	public void OnInitControlPage()
	{
		auidioPageRoot.gameObject.SetActiveRecursively(false);
		drivingPageRoot.gameObject.SetActiveRecursively(true);
		creditsRoot.gameObject.SetActiveRecursively(false);
		ResetControlBtn();
	}

	public void OnInitCreditsPage()
	{
		auidioPageRoot.gameObject.SetActiveRecursively(false);
		drivingPageRoot.gameObject.SetActiveRecursively(false);
		creditsRoot.gameObject.SetActiveRecursively(true);
	}

	public void OnClickGravityBtn(GameObject obj)
	{
		GlobalInf.carCtrlType = CARCTRLTYPE.GRAVITY;
		StoreDateController.SetControlType(true);
		OnInitControlPage();
	}

	public void OnClickHandleBtn(GameObject obj)
	{
		GlobalInf.carCtrlType = CARCTRLTYPE.BUTTON;
		StoreDateController.SetControlType(false);
		OnInitControlPage();
	}

	public void OnClickVoiceBtn(GameObject obj)
	{
		OnInitVoicePage();
	}

	public void OnClickControlBtn(GameObject obj)
	{
		OnInitControlPage();
	}

	public void OnClickCreditsBtn(GameObject obj)
	{
		OnInitCreditsPage();
	}
}
