using System;
using UnityEngine;

public class GameSenceVideoCheckUI : MonoBehaviour
{
	public UIEventListener okBtn;

	public UISprite pic;

	public VIDEOTYPE type;

	public UILabel words;

	private void Start()
	{
		UIEventListener uIEventListener = okBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickOKBtn));
		GlobalInf.videoAdsFlag = true;
	}

	private void Update()
	{
	}

	public void Reset(VIDEOTYPE t)
	{
		type = t;
		switch (type)
		{
		case VIDEOTYPE.HANDGUNBULLET:
			ResetBullet();
			break;
		case VIDEOTYPE.MACHINEGUNBULLET:
			ResetMachineGunBullet();
			break;
		case VIDEOTYPE.HEALTHKIT:
			ResetHealthKit();
			break;
		case VIDEOTYPE.TOOLKIT:
			ResetToolKit();
			break;
		}
	}

	public void ResetHealthKit()
	{
		pic.spriteName = "doctor";
		pic.width = 62;
		pic.height = 62;
		words.text = "Congratulations, you gain a\nfree health kit from video!";
	}

	public void ResetToolKit()
	{
		pic.spriteName = "car-tool";
		pic.width = (int)(108f * GlobalDefine.screenWidthFit);
		pic.height = 66;
		words.text = "Congratulations, you gain a\nfree tool kit from video!";
	}

	public void ResetBullet()
	{
		Time.timeScale = 0f;
		pic.spriteName = "sq-zidan";
		pic.width = (int)(80f * GlobalDefine.screenWidthFit);
		pic.height = 50;
		words.text = "Congratulations, you gain\nfree bullets from video!";
	}

	public void ResetMachineGunBullet()
	{
		Time.timeScale = 0f;
		pic.spriteName = "jq-zidan";
		pic.width = (int)(90f * GlobalDefine.screenWidthFit);
		pic.height = 50;
		words.text = "Congratulations, you gain\nfree bullets from video!";
	}

	public void OnClickOKBtn(GameObject btn)
	{
		switch (type)
		{
		case VIDEOTYPE.HEALTHKIT:
			GlobalInf.healthKitNum++;
			StoreDateController.SetHealthKitNum(GlobalInf.healthKitNum);
			GameUIController.instance.buyKitPage.OnClickCloseBtn(null);
			GameUIController.instance.OnClickAddHealthBtn(null);
			GlobalInf.adsHealthKitFlag = false;
			PlayerPrefs.SetInt("AdsHealthKit", 1);
			break;
		case VIDEOTYPE.TOOLKIT:
			GlobalInf.toolKitNum++;
			StoreDateController.SetToolKitNum(GlobalInf.toolKitNum);
			GameUIController.instance.buyKitPage.OnClickCloseBtn(null);
			GameUIController.instance.OnClickToolKitBtn(null);
			GlobalInf.adsToolKitFlag = false;
			PlayerPrefs.SetInt("AdsToolKit", 1);
			break;
		case VIDEOTYPE.HANDGUNBULLET:
			Time.timeScale = 1f;
			PlayerController.instance.gun.gunInfo.restBulletNum = PlayerController.instance.gun.gunInfo.maxBulletNum - PlayerController.instance.gun.gunInfo.bulletNum;
			PlayerController.instance.gun.bulletCount = 0;
			PlayerController.instance.gun.gunInfo.curBulletNum = PlayerController.instance.gun.gunInfo.bulletNum;
			StoreDateController.SetHandGunBulletNum(GlobalInf.handgunIndex, PlayerController.instance.gun.gunInfo.maxBulletNum);
			GameUIController.instance.bulletVideoBtn.gameObject.SetActiveRecursively(false);
			GameUIController.instance.ReflashHandGunBulletNum();
			GlobalInf.adsHandGunBulletFlag = false;
			PlayerPrefs.SetInt("AdsHandGunBullet", 1);
			break;
		case VIDEOTYPE.MACHINEGUNBULLET:
			Time.timeScale = 1f;
			PlayerController.instance.machineGun.gunInfo.restBulletNum = PlayerController.instance.machineGun.gunInfo.maxBulletNum - PlayerController.instance.machineGun.gunInfo.bulletNum;
			PlayerController.instance.machineGun.bulletCount = 0;
			PlayerController.instance.machineGun.gunInfo.curBulletNum = PlayerController.instance.machineGun.gunInfo.bulletNum;
			StoreDateController.SetMachineGunBulletNum(GlobalInf.machineGunIndex, PlayerController.instance.machineGun.gunInfo.maxBulletNum);
			GameUIController.instance.bulletVideoBtn.gameObject.SetActiveRecursively(false);
			GameUIController.instance.ReflashMachineGunBulletNum();
			GlobalInf.adsMachineGunBulletFlag = false;
			PlayerPrefs.SetInt("AdsMachineGunBullet", 1);
			break;
		}
		Platform.CheckAdsCanShow();
		GlobalInf.videoAdsFlag = false;
		base.gameObject.SetActiveRecursively(false);
		UnityEngine.Object.Destroy(base.gameObject);
		Resources.UnloadUnusedAssets();
	}
}
