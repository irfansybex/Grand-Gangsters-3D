using System;
using UnityEngine;

public class AchievementPageController : CharacterPageRoot
{
	public AchievementItem[] items;

	public int[] achievementLevel;

	public int[] pageIndex;

	public float[] pageVal;

	public Color goldColor = new Color(1f, 1f, 0.192f, 1f);

	public Color copperColor = new Color(1f, 0.5686f, 0.2824f, 1f);

	public UIPanel pagePanel;

	public bool initFlag;

	public bool disableBtnFlag;

	private void Start()
	{
		InitBtn();
	}

	private new void OnEnable()
	{
		base.OnEnable();
		GetAchievementLevel();
		initFlag = false;
		InitPage();
	}

	private void Update()
	{
		if (!initFlag)
		{
			initFlag = true;
			ResetPageIndex();
			InitPage();
		}
	}

	public void InitPage()
	{
		InitItems(items[0], GlobalInf.totalKillNum, 0);
		InitItems(items[1], GlobalInf.totalKillCitizens, 1);
		InitItems(items[2], GlobalInf.totalKillGangsters, 2);
		InitItems(items[3], GlobalInf.policeKillNum, 3);
		InitItems(items[4], GlobalInf.punchKillNum, 4);
		InitItems(items[5], GlobalInf.handGunKillNum, 5);
		InitItems(items[6], GlobalInf.machineGunKillNum, 6);
		InitItems(items[7], GlobalInf.carKillNum, 7);
		InitItems(items[8], (int)GlobalInf.drivingDistance, 8);
		InitItems(items[9], GlobalInf.completeTaskNum, 9);
		InitItems(items[10], GlobalInf.completeDifTaskNum, 10);
		InitItems(items[11], GlobalInf.threeStarTaskNum, 11);
		InitItems(items[12], GlobalInf.totalCashEarned, 12);
		InitItems(items[13], GlobalInf.totalGoldEarned, 13);
		InitItems(items[14], GlobalInf.totalTimeSpent / 60, 14);
		pagePanel.Refresh();
	}

	public void ResetPageIndex()
	{
		for (int i = 0; i < items.Length; i++)
		{
			pageIndex[i] = i;
			if (achievementLevel[i] == 3)
			{
				pageVal[i] = -1f;
			}
			else
			{
				pageVal[i] = items[i].valLine.fillAmount;
			}
		}
		for (int j = 0; j < items.Length; j++)
		{
			for (int k = j; k < items.Length; k++)
			{
				if (pageVal[k] > pageVal[j])
				{
					float num = pageVal[k];
					int num2 = pageIndex[k];
					pageVal[k] = pageVal[j];
					pageIndex[k] = pageIndex[j];
					pageVal[j] = num;
					pageIndex[j] = num2;
				}
			}
		}
		for (int l = 0; l < items.Length; l++)
		{
			items[pageIndex[l]].transform.localPosition = new Vector3(0f, -100 * l, 0f);
			if (l < 3)
			{
				NGUITools.SetActiveRecursively(items[pageIndex[l]].gameObject, false);
				NGUITools.SetActiveRecursively(items[pageIndex[l]].gameObject, true);
			}
		}
	}

	public void InitBtn()
	{
		UIEventListener btn = items[0].btn;
		btn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn.onClick, new UIEventListener.VoidDelegate(OnClickTotalKillAchievement));
		UIEventListener btn2 = items[1].btn;
		btn2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn2.onClick, new UIEventListener.VoidDelegate(OnClickKillCitizens));
		UIEventListener btn3 = items[2].btn;
		btn3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn3.onClick, new UIEventListener.VoidDelegate(OnClickKillGangsters));
		UIEventListener btn4 = items[3].btn;
		btn4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn4.onClick, new UIEventListener.VoidDelegate(OnClickKillCops));
		UIEventListener btn5 = items[4].btn;
		btn5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn5.onClick, new UIEventListener.VoidDelegate(OnClickPunchKillNum));
		UIEventListener btn6 = items[5].btn;
		btn6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn6.onClick, new UIEventListener.VoidDelegate(OnClickHandGunKillNum));
		UIEventListener btn7 = items[6].btn;
		btn7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn7.onClick, new UIEventListener.VoidDelegate(OnClickMachineGunKillNum));
		UIEventListener btn8 = items[7].btn;
		btn8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn8.onClick, new UIEventListener.VoidDelegate(OnClickCarKillNum));
		UIEventListener btn9 = items[8].btn;
		btn9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn9.onClick, new UIEventListener.VoidDelegate(OnClickDrivingDistance));
		UIEventListener btn10 = items[9].btn;
		btn10.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn10.onClick, new UIEventListener.VoidDelegate(OnClickCompleteTaskNum));
		UIEventListener btn11 = items[10].btn;
		btn11.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn11.onClick, new UIEventListener.VoidDelegate(OnClickCompleteDifTaskNum));
		UIEventListener btn12 = items[11].btn;
		btn12.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn12.onClick, new UIEventListener.VoidDelegate(OnClickThreeStarTaskNum));
		UIEventListener btn13 = items[12].btn;
		btn13.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn13.onClick, new UIEventListener.VoidDelegate(OnClickEarnedCash));
		UIEventListener btn14 = items[13].btn;
		btn14.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn14.onClick, new UIEventListener.VoidDelegate(OnClickEarnedGold));
		UIEventListener btn15 = items[14].btn;
		btn15.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(btn15.onClick, new UIEventListener.VoidDelegate(OnClickSpentTime));
	}

	public void InitItems(AchievementItem item, int val, int index)
	{
		float num = 0f;
		if (achievementLevel[index] == 3)
		{
			item.golPic.gameObject.SetActiveRecursively(true);
			item.golPic.color = goldColor;
			item.valLine.fillAmount = 1f;
			item.btn.gameObject.SetActiveRecursively(false);
			item.percentLabel.gameObject.SetActiveRecursively(true);
			item.percentLabel.text = "100%";
			item.detialLabel.text = string.Empty + item.level[2];
			item.cashPic.gameObject.SetActiveRecursively(false);
			item.goldPic.gameObject.SetActiveRecursively(false);
			item.boxPic.gameObject.SetActiveRecursively(false);
			item.cashNum.gameObject.SetActiveRecursively(false);
			item.wordRoot.transform.localPosition = new Vector3(50f * GlobalDefine.screenWidthFit, 0f, 0f);
			return;
		}
		item.detialLabel.text = string.Empty + item.level[achievementLevel[index]];
		item.golPic.gameObject.SetActiveRecursively(true);
		if (item.level[achievementLevel[index]] <= val)
		{
			item.valLine.fillAmount = 1f;
			item.btn.gameObject.SetActiveRecursively(true);
			item.percentLabel.gameObject.SetActiveRecursively(false);
		}
		else
		{
			num = (float)val / (float)item.level[achievementLevel[index]];
			item.valLine.fillAmount = num;
			item.percentLabel.gameObject.SetActiveRecursively(true);
			item.btn.gameObject.SetActiveRecursively(false);
			item.percentLabel.text = string.Empty + (int)(num * 100f) + "%";
		}
		if (achievementLevel[index] == 2)
		{
			item.golPic.color = Color.white;
			item.cashPic.gameObject.SetActiveRecursively(false);
			item.goldPic.gameObject.SetActiveRecursively(false);
			item.boxPic.gameObject.SetActiveRecursively(true);
			item.cashNum.gameObject.SetActiveRecursively(false);
		}
		else if (achievementLevel[index] == 1)
		{
			item.golPic.color = copperColor;
			item.cashPic.gameObject.SetActiveRecursively(false);
			item.goldPic.gameObject.SetActiveRecursively(true);
			item.boxPic.gameObject.SetActiveRecursively(false);
			item.cashNum.gameObject.SetActiveRecursively(true);
			item.cashNum.text = "X" + item.goldMoney;
		}
		else
		{
			item.cashPic.gameObject.SetActiveRecursively(true);
			item.goldPic.gameObject.SetActiveRecursively(false);
			item.boxPic.gameObject.SetActiveRecursively(false);
			item.cashNum.gameObject.SetActiveRecursively(true);
			item.cashNum.text = "X" + item.cashMoney;
			item.golPic.color = new Color(0.2f, 0.2f, 0.2f, 1f);
		}
		item.wordRoot.transform.localPosition = Vector3.zero;
	}

	public void GetAchievementLevel()
	{
		string @string = PlayerPrefs.GetString("achievementLevel", "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0");
		string[] array = @string.Split('|');
		for (int i = 0; i < achievementLevel.Length; i++)
		{
			achievementLevel[i] = Convert.ToInt32(array[i]);
		}
	}

	public void SetAchievementLevel()
	{
		string text = string.Empty + achievementLevel[0];
		for (int i = 1; i < achievementLevel.Length; i++)
		{
			text = text + "|" + achievementLevel[i];
		}
		PlayerPrefs.SetString("achievementLevel", text);
	}

	public void OnClickAchievementBtn(int Val, int index)
	{
		if (disableBtnFlag)
		{
			return;
		}
		disableBtnFlag = true;
		Invoke("DelayEnableBtn", 0.5f);
		if (achievementLevel[index] >= 3 || Val < items[index].level[achievementLevel[index]])
		{
			return;
		}
		if (achievementLevel[index] == 0)
		{
			GlobalInf.cash += items[index].cashMoney;
			GlobalInf.totalCashEarned += items[index].cashMoney;
			StoreDateController.SetTotalCashEarned(GlobalInf.totalCashEarned);
			StoreDateController.SetCash();
			AudioController.instance.play(AudioType.PICK_CASH);
		}
		else if (achievementLevel[index] == 1)
		{
			GlobalInf.gold += items[index].goldMoney;
			StoreDateController.SetGold();
			GlobalInf.totalGoldEarned += items[index].goldMoney;
			StoreDateController.SetTotalGoldEarned();
			AudioController.instance.play(AudioType.PICK_CASH);
		}
		else
		{
			MenuSenceController.instance.getItemPage.gameObject.SetActiveRecursively(true);
			if (UnityEngine.Random.Range(0, 2) > 0)
			{
				MenuSenceController.instance.getItemPage.InitPage(ITEMTYPE.HEALTHKIT);
			}
			else
			{
				MenuSenceController.instance.getItemPage.InitPage(ITEMTYPE.TOOLKIT);
			}
			MenuSenceBackBtnCtl.instance.ChangeMenuUIState(MENUUISTATE.CLAIMPAGE);
			AudioController.instance.play(AudioType.GET_ITEM);
		}
		achievementLevel[index]++;
		SetAchievementLevel();
		InitItems(items[index], Val, index);
		InitItems(items[12], GlobalInf.totalCashEarned, 12);
		InitItems(items[13], GlobalInf.totalGoldEarned, 13);
		MenuSenceController.instance.characterMenu.CheckSignal();
		MenuSenceController.instance.characterMenu.CheckUISignal();
	}

	public void DelayEnableBtn()
	{
		disableBtnFlag = false;
	}

	public void OnClickTotalKillAchievement(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.totalKillNum, 0);
	}

	public void OnClickKillCitizens(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.totalKillCitizens, 1);
	}

	public void OnClickKillGangsters(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.totalKillGangsters, 2);
	}

	public void OnClickKillCops(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.policeKillNum, 3);
	}

	public void OnClickPunchKillNum(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.punchKillNum, 4);
	}

	public void OnClickHandGunKillNum(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.handGunKillNum, 5);
	}

	public void OnClickMachineGunKillNum(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.machineGunKillNum, 6);
	}

	public void OnClickCarKillNum(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.carKillNum, 7);
	}

	public void OnClickDrivingDistance(GameObject btn)
	{
		OnClickAchievementBtn((int)GlobalInf.drivingDistance, 8);
	}

	public void OnClickCompleteTaskNum(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.completeTaskNum, 9);
	}

	public void OnClickCompleteDifTaskNum(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.completeDifTaskNum, 10);
	}

	public void OnClickThreeStarTaskNum(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.threeStarTaskNum, 11);
	}

	public void OnClickEarnedCash(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.totalCashEarned, 12);
	}

	public void OnClickEarnedGold(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.totalGoldEarned, 13);
	}

	public void OnClickSpentTime(GameObject btn)
	{
		OnClickAchievementBtn(GlobalInf.totalTimeSpent / 60, 14);
	}

	public bool CheckAchievementState(int i)
	{
		if (achievementLevel[i] < 3)
		{
			switch (i)
			{
			case 0:
				return CheckAchievementVal(GlobalInf.totalKillNum, i);
			case 1:
				return CheckAchievementVal(GlobalInf.totalKillCitizens, i);
			case 2:
				return CheckAchievementVal(GlobalInf.totalKillGangsters, i);
			case 3:
				return CheckAchievementVal(GlobalInf.policeKillNum, i);
			case 4:
				return CheckAchievementVal(GlobalInf.punchKillNum, i);
			case 5:
				return CheckAchievementVal(GlobalInf.handGunKillNum, i);
			case 6:
				return CheckAchievementVal(GlobalInf.machineGunKillNum, i);
			case 7:
				return CheckAchievementVal(GlobalInf.carKillNum, i);
			case 8:
				return CheckAchievementVal((int)GlobalInf.drivingDistance, i);
			case 9:
				return CheckAchievementVal(GlobalInf.completeTaskNum, i);
			case 10:
				return CheckAchievementVal(GlobalInf.completeDifTaskNum, i);
			case 11:
				return CheckAchievementVal(GlobalInf.threeStarTaskNum, i);
			case 12:
				return CheckAchievementVal(GlobalInf.totalCashEarned, i);
			case 13:
				return CheckAchievementVal(GlobalInf.totalGoldEarned, i);
			case 14:
				return CheckAchievementVal(GlobalInf.totalTimeSpent / 60, i);
			}
		}
		return false;
	}

	public bool CheckAchievementVal(int val, int index)
	{
		if (val >= items[index].level[achievementLevel[index]])
		{
			return true;
		}
		return false;
	}
}
