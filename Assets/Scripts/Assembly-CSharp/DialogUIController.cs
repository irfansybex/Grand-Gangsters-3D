using System;
using UnityEngine;

public class DialogUIController : MonoBehaviour
{
	public UILabel titleLabel;

	public UILabel wordsLabel;

	public TweenScale scale;

	public UIEventListener okBtn;

	public UISprite back;

	private void Start()
	{
		UIEventListener uIEventListener = okBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickOKBtn));
	}

	public void Reset()
	{
		base.gameObject.SetActiveRecursively(true);
		scale.PlayForward();
		Time.timeScale = 0f;
		if (GlobalInf.gameState == 7 && !GlobalInf.newUserFlag)
		{
			titleLabel.text = "TUTORIAL COMPLETE";
			wordsLabel.text = "You have passed all the\ntutorials. I'm quite sure that\nyou know what to do next.\nGood luck and have fun!";
		}
		else if (GlobalInf.gameState == 10 && GlobalInf.newUserFlag)
		{
			titleLabel.text = "TUTORIAL COMPLETE";
			wordsLabel.text = "You have passed all the\ntutorials. I'm quite sure that\nyou know what to do next.\nGood luck and have fun!";
		}
		else if (GlobalInf.upgradeTutorialFlag)
		{
			titleLabel.text = "UPGRADE";
			wordsLabel.text = "Good job!\nBut before you start next,\nlet's do something in the store.\n";
		}
		else
		{
			titleLabel.text = "SECRET ITEM";
			wordsLabel.text = "There're secret items scartterd\naround the whole city. The man\nwho collect all of them will be\nrewarded with Top Equipment";
		}
	}

	public void OnClickOKBtn(GameObject btn)
	{
		if (GlobalInf.gameState == 7 && !GlobalInf.newUserFlag)
		{
			GameStateController.instance.ChangeGameState();
			scale.PlayReverse();
			Invoke("DelayDisable", 0.9f);
			GameUIController.instance.taskEndUIControllor.enableBtn = true;
		}
		else if (GlobalInf.gameState == 10 && GlobalInf.newUserFlag)
		{
			GameStateController.instance.ChangeGameState();
			scale.PlayReverse();
			Invoke("DelayDisable", 0.9f);
			GameUIController.instance.taskEndUIControllor.enableBtn = true;
		}
		else if (GlobalInf.upgradeTutorialFlag)
		{
			GlobalInf.backToHandGunPageFlag = true;
			GlobalInf.backToCarPageFlag = false;
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
		else
		{
			scale.PlayReverse();
			Time.timeScale = 1f;
			Invoke("DelayDisable", 0.9f);
		}
	}

	public void DelayDisable()
	{
		base.gameObject.SetActiveRecursively(false);
	}
}
