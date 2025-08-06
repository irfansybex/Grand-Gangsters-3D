using System;
using UnityEngine;

public class DailyBonusController : MonoBehaviour
{
	public static DailyBonusController instance;

	public GameObject[] todayLabel;

	public GameObject[] claimedLabel;

	public UIEventListener backBtn;

	public int dailyBounseDay;

	public int tempDailyBounseDay;

	public int[] bounseMoney;

	private bool firstOpenFlag;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Destroy()
	{
		if (instance != null)
		{
			instance = null;
		}
	}

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.DAILYBOUNSEPAGE);
		tempDailyBounseDay = Platform.getDailyBonusDay();
		dailyBounseDay = PlayerPrefs.GetInt("dailyBounseDay", 0);
		firstOpenFlag = false;
		for (int i = 0; i < 7; i++)
		{
			todayLabel[i].gameObject.SetActiveRecursively(false);
		}
		if (tempDailyBounseDay == -1)
		{
			todayLabel[dailyBounseDay].gameObject.SetActiveRecursively(true);
		}
		else
		{
			dailyBounseDay = tempDailyBounseDay;
			PlayerPrefs.SetInt("dailyBounseDay", dailyBounseDay);
			Platform.flurryEvent_onDailyBonus(dailyBounseDay);
			if (dailyBounseDay == 1 || dailyBounseDay == 6)
			{
				GlobalInf.gold += bounseMoney[dailyBounseDay];
				StoreDateController.SetGold();
				GlobalInf.totalGoldEarned += bounseMoney[dailyBounseDay];
				StoreDateController.SetTotalGoldEarned();
			}
			else
			{
				GlobalInf.cash += bounseMoney[dailyBounseDay];
				GlobalInf.totalCashEarned += bounseMoney[dailyBounseDay];
				StoreDateController.SetTotalCashEarned(GlobalInf.totalCashEarned);
				StoreDateController.SetCash();
			}
			Platform.setDailyBounsDay();
			firstOpenFlag = true;
			todayLabel[dailyBounseDay].gameObject.SetActiveRecursively(false);
			Invoke("DelayPlayAnima", 0.5f);
		}
		PlayerPrefs.SetInt("ClickDailyBonuseFlag", 1);
		for (int j = 0; j < 7; j++)
		{
			if (j < dailyBounseDay)
			{
				claimedLabel[j].gameObject.SetActiveRecursively(true);
			}
			else
			{
				claimedLabel[j].gameObject.SetActiveRecursively(false);
			}
		}
		UIEventListener uIEventListener = backBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickBackBtn));
		Platform.showFeatureView(FeatureViewPosType.MIDDLE);
	}

	public void DelayPlayAnima()
	{
		todayLabel[dailyBounseDay].gameObject.SetActiveRecursively(true);
		todayLabel[dailyBounseDay].GetComponent<TweenScale>().PlayForward();
		if (AudioController.instance != null)
		{
			AudioController.instance.play(AudioType.NUM_ROLL);
		}
	}

	public void OnClickBackBtn(GameObject obj)
	{
		UnityEngine.Object.Destroy(base.gameObject);
		Resources.UnloadUnusedAssets();
		MenuSenceBackBtnCtl.instance.PopMenuUIState();
		MenuSenceController.instance.startMenu.startMenuObj.gameObject.SetActiveRecursively(true);
		MenuSenceController.instance.startMenu.startMenuObj.transform.localPosition = Vector3.zero;
		MenuSenceController.instance.startMenu.startBackGroundObj.gameObject.SetActiveRecursively(true);
		MenuSenceController.instance.startMenu.CheckSignal();
		MenuSenceController.instance.startMenu.dailyBounseDisableObj.gameObject.SetActiveRecursively(false);
		if (Platform.limitedTimeOfferFlag)
		{
			MenuSenceController.instance.startMenu.saleRoot.gameObject.SetActiveRecursively(true);
		}
		else
		{
			MenuSenceController.instance.startMenu.saleRoot.gameObject.SetActiveRecursively(false);
		}
		Platform.hideFeatureView();
	}
}
