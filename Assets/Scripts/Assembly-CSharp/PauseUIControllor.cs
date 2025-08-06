using System;
using UnityEngine;

public class PauseUIControllor : MonoBehaviour
{
	public UIEventListener resumeBtn;

	public UIEventListener settingBtn;

	public UIEventListener mainMenuBtn;

	public UIEventListener giveUpBtn;

	public UISprite btnPic;

	public UILabel curKillLabel;

	public UILabel curTimeSpentLabel;

	public UILabel curDistance;

	private void Awake()
	{
		UIEventListener uIEventListener = resumeBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickResumeBtn));
		UIEventListener uIEventListener2 = settingBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickSettingBtn));
		UIEventListener uIEventListener3 = mainMenuBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickMainMenuBtn));
		UIEventListener uIEventListener4 = giveUpBtn;
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickGiveUpBtn));
	}

	public void ResetPauseUI()
	{
		GameUIController.instance.controlUIRoot.gameObject.SetActiveRecursively(false);
		GameUIController.instance.carUI.gameObject.SetActiveRecursively(false);
		GameUIController.instance.tempUI.SetActiveRecursively(false);
		GameUIController.instance.tempUI.SetActive(true);
		GameUIController.instance.taskBoardController.gameObject.SetActiveRecursively(false);
		GameUIController.instance.stateUIController.gameObject.SetActiveRecursively(false);
		GameUIController.instance.resetPlayerCarBtn.gameObject.SetActiveRecursively(false);
		GameUIController.instance.outOfAmmoLabel.gameObject.SetActiveRecursively(false);
		Time.timeScale = 0f;
		if (GameController.instance.curGameMode == GAMEMODE.NORMAL)
		{
			giveUpBtn.gameObject.SetActiveRecursively(false);
			btnPic.spriteName = "home";
		}
		else
		{
			Platform.flurryEvent_onTaskPause((int)GameUIController.instance.gameMode, GameUIController.instance.taskIndex);
			mainMenuBtn.gameObject.SetActiveRecursively(false);
			btnPic.spriteName = "giveup";
		}
		Platform.showFeatureView(FeatureViewPosType.MIDDLE);
		curKillLabel.text = string.Empty + GlobalInf.curKill;
		curTimeSpentLabel.text = string.Empty + ChangeSecToMin((int)Time.time - GlobalInf.curStartTime);
		curDistance.text = string.Empty + string.Format("{0:F}", GlobalInf.curDistance) + "km";
		if (ADShowController.instance.showFlag)
		{
			ADShowController.instance.showFlag = false;
			Platform.showFullScreenSmall();
		}
	}

	public string ChangeSecToMin(int sec)
	{
		return string.Empty + sec / 60 + ":" + sec % 60;
	}

	public void QuitPauseUI()
	{
		GameUIController.instance.InitUI();
		if (GameUIController.instance.locateTargetList.Count != 0)
		{
			for (int i = 0; i < GameUIController.instance.locateTargetList.Count; i++)
			{
				GameUIController.instance.locateLabelList[i].gameObject.SetActiveRecursively(true);
			}
		}
		Time.timeScale = 1f;
		Platform.hideFeatureView();
	}

	public void OnClickResumeBtn(GameObject obj)
	{
		QuitPauseUI();
		base.gameObject.SetActiveRecursively(false);
		GameSenceBackBtnCtl.instance.PopGameUIState();
		AudioController.instance.resumeSounds();
	}

	private void OnClickSettingBtn(GameObject btn)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UI/Setting")) as GameObject;
		gameObject.transform.parent = base.transform.parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.SETTING);
		base.gameObject.SetActiveRecursively(false);
	}

	public void OnClickMainMenuBtn(GameObject obj)
	{
		GlobalInf.nextSence = "MenuSence";
		Application.LoadLevel("LoadingSence");
		if (PlayerController.instance.machineGun != null)
		{
			StoreDateController.SetMachineGunBulletNum(GlobalInf.machineGunIndex, PlayerController.instance.machineGun.gunInfo.restBulletNum + PlayerController.instance.machineGun.gunInfo.curBulletNum - PlayerController.instance.machineGun.bulletCount);
		}
		if (PlayerController.instance.gun != null)
		{
			StoreDateController.SetHandGunBulletNum(GlobalInf.handgunIndex, PlayerController.instance.gun.gunInfo.restBulletNum + PlayerController.instance.gun.gunInfo.curBulletNum - PlayerController.instance.gun.bulletCount);
		}
		GlobalInf.totalTimeSpent += (int)Time.time - GlobalInf.startGameTime;
		GlobalInf.dailyPlayTime += (int)Time.time - GlobalInf.startGameTime;
		GlobalInf.startGameTime = (int)Time.time;
		StoreDateController.SetCountData();
		Platform.hideFeatureView();
	}

	private void OnClickGiveUpBtn(GameObject obj)
	{
		Time.timeScale = 1f;
		BlackScreen.instance.TurnOffScreen();
		GameUIController.instance.DisableLocateLabel();
		Invoke("DelayGiveUp", 0.5f);
		Platform.hideFeatureView();
		PlayerController.instance.moveCtl.disableMoveFlag = false;
		GameUIController.instance.disableCarBtnFlag = false;
		GameSenceBackBtnCtl.instance.PopGameUIState();
		Platform.flurryEvent_onTaskQuit((int)GameUIController.instance.gameMode, GameUIController.instance.taskIndex);
	}

	private void DelayGiveUp()
	{
		QuitPauseUI();
		base.gameObject.SetActiveRecursively(false);
		GameController.instance.ChangeMode(GAMEMODE.NORMAL, 0);
	}
}
