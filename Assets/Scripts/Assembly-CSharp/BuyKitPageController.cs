using System;
using UnityEngine;

public class BuyKitPageController : MonoBehaviour
{
	public bool healthKitFlag;

	public UIEventListener buyOneBtn;

	public UIEventListener buyThreeBtn;

	public UIEventListener closeBtn;

	public GameObject healthKitPic1;

	public GameObject healthKitPic2;

	public GameObject toolKitPic1;

	public GameObject toolKitPic2;

	public GameObject lightRoot;

	public UIEventListener videoEnableBtn;

	public GameObject videoRoot;

	public GameObject videoHealthKit;

	public GameObject videoToolKit;

	public GameObject pageRoot;

	public float speed;

	private float preTime;

	private void Start()
	{
		UIEventListener uIEventListener = closeBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickCloseBtn));
		UIEventListener uIEventListener2 = buyOneBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickBuyOneBtn));
		UIEventListener uIEventListener3 = buyThreeBtn;
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnClickBuyThreeBtn));
		UIEventListener uIEventListener4 = videoEnableBtn;
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnClickVideoEnableBtn));
	}

	private void Update()
	{
		lightRoot.transform.Rotate(lightRoot.transform.forward, speed * (Time.realtimeSinceStartup - preTime));
		preTime = Time.realtimeSinceStartup;
	}

	public void OnClickVideoEnableBtn(GameObject btn)
	{
		if (healthKitFlag)
		{
			GlobalInf.videoType = VIDEOTYPE.HEALTHKIT;
		}
		else
		{
			GlobalInf.videoType = VIDEOTYPE.TOOLKIT;
		}
		Platform.internalShowUnityAds();
		Platform.flurryEvent_onClickUnityAddHealthKitAds();
	}

	public void ResetPage(bool flag)
	{
		healthKitFlag = flag;
		preTime = Time.realtimeSinceStartup;
		if (healthKitFlag)
		{
			if (Platform.CheckAdsCanShow() && GlobalInf.adsHealthKitFlag && !Platform.isLowAPILevel)
			{
				videoRoot.transform.localPosition = new Vector3(80f * GlobalDefine.screenWidthFit, 0f, 0f);
				pageRoot.transform.localPosition = new Vector3(80f * GlobalDefine.screenWidthFit, 0f, 0f);
				videoRoot.SetActiveRecursively(true);
				videoToolKit.SetActiveRecursively(false);
			}
			else
			{
				videoRoot.SetActiveRecursively(false);
				pageRoot.transform.localPosition = Vector3.zero;
			}
			healthKitPic1.gameObject.SetActiveRecursively(true);
			healthKitPic2.gameObject.SetActiveRecursively(true);
			toolKitPic1.gameObject.SetActiveRecursively(false);
			toolKitPic2.gameObject.SetActiveRecursively(false);
		}
		else
		{
			if (Platform.CheckAdsCanShow() && GlobalInf.adsToolKitFlag && !Platform.isLowAPILevel)
			{
				videoRoot.transform.localPosition = new Vector3(80f * GlobalDefine.screenWidthFit, 0f, 0f);
				pageRoot.transform.localPosition = new Vector3(80f * GlobalDefine.screenWidthFit, 0f, 0f);
				videoRoot.SetActiveRecursively(true);
				videoHealthKit.SetActiveRecursively(false);
			}
			else
			{
				videoRoot.SetActiveRecursively(false);
				pageRoot.transform.localPosition = Vector3.zero;
			}
			healthKitPic1.gameObject.SetActiveRecursively(false);
			healthKitPic2.gameObject.SetActiveRecursively(false);
			toolKitPic1.gameObject.SetActiveRecursively(true);
			toolKitPic2.gameObject.SetActiveRecursively(true);
		}
	}

	public void OnClickBuyOneBtn(GameObject btn)
	{
		if (GlobalInf.gold >= 5)
		{
			if (healthKitFlag)
			{
				GlobalInf.healthKitNum++;
				StoreDateController.SetHealthKitNum(GlobalInf.healthKitNum);
				OnClickCloseBtn(null);
				GameUIController.instance.OnClickAddHealthBtn(null);
				Platform.flurryEvent_onEquipmentKitPurchase(3);
			}
			else
			{
				GlobalInf.toolKitNum++;
				StoreDateController.SetToolKitNum(GlobalInf.toolKitNum);
				OnClickCloseBtn(null);
				GameUIController.instance.OnClickToolKitBtn(null);
				Platform.flurryEvent_onEquipmentKitPurchase(4);
			}
			GlobalInf.gold -= 5;
			StoreDateController.SetGold();
		}
		else
		{
			TopLineController.instance.OnClickGoldBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.GAMEKIT;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void OnClickBuyThreeBtn(GameObject btn)
	{
		if (GlobalInf.gold >= 10)
		{
			if (healthKitFlag)
			{
				GlobalInf.healthKitNum += 3;
				StoreDateController.SetHealthKitNum(GlobalInf.healthKitNum);
				OnClickCloseBtn(null);
				GameUIController.instance.OnClickAddHealthBtn(null);
				Platform.flurryEvent_onEquipmentKitPurchase(3);
			}
			else
			{
				GlobalInf.toolKitNum += 3;
				StoreDateController.SetToolKitNum(GlobalInf.toolKitNum);
				OnClickCloseBtn(null);
				GameUIController.instance.OnClickToolKitBtn(null);
				Platform.flurryEvent_onEquipmentKitPurchase(4);
			}
			GlobalInf.gold -= 10;
			StoreDateController.SetGold();
		}
		else
		{
			TopLineController.instance.OnClickGoldBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.GAMEKIT;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void OnClickCloseBtn(GameObject btn)
	{
		Time.timeScale = 1f;
		base.gameObject.SetActiveRecursively(false);
		GameUIController.instance.topLine.gameObject.SetActiveRecursively(false);
		GameSenceBackBtnCtl.instance.PopGameUIState();
		GameUIController.instance.pauseBtn.gameObject.SetActiveRecursively(true);
	}
}
