using System;
using UnityEngine;

public class DailyTaskController : MonoBehaviour
{
	public TweenScale tweenScale;

	public UIEventListener backBtn;

	public DailyTaskItem[] item;

	public int[] dailyTaskCompleteNum0;

	public int[] dailyTaskCompleteNum1;

	public int[] dailyTaskCompleteNum2;

	public bool initFlag;

	public int line1Prise;

	public int line2Prise;

	public int line3Prise;

	private int coutttt;

	private void Awake()
	{
		UIEventListener uIEventListener = backBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickBackBtn));
		UIEventListener claimBtnListener = item[0].claimBtnListener;
		claimBtnListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(claimBtnListener.onClick, new UIEventListener.VoidDelegate(OnClickClaim1Btn));
		UIEventListener claimBtnListener2 = item[1].claimBtnListener;
		claimBtnListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(claimBtnListener2.onClick, new UIEventListener.VoidDelegate(OnClickClaim2Btn));
		UIEventListener claimBtnListener3 = item[2].claimBtnListener;
		claimBtnListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(claimBtnListener3.onClick, new UIEventListener.VoidDelegate(OnClickClaim3Btn));
		initFlag = false;
		coutttt = 0;
	}

	private void Update()
	{
		if (!initFlag)
		{
			coutttt++;
		}
		if (coutttt > 20)
		{
			initFlag = true;
			coutttt = 0;
			backBtn.gameObject.GetComponent<UIAnchor>().Run();
		}
	}

	public void OnClickBackBtn(GameObject btn)
	{
		tweenScale.PlayReverse();
		Invoke("DelayDisable", 0.6f);
		MenuSenceController.instance.startMenu.CheckSignal();
		MenuSenceBackBtnCtl.instance.PopMenuUIState();
		Platform.hideFeatureView();
	}

	public void DelayDisable()
	{
		base.gameObject.SetActiveRecursively(false);
	}

	public void ResetDailyTask()
	{
		GlobalInf.dailyTaskInitFlag = true;
		StoreDateController.ResetDailyTask();
	}

	public void InitDailyTask()
	{
		ResetDailyItems(GlobalInf.dailyTaskIndex[0], item[0], 0);
		ResetDailyItems(GlobalInf.dailyTaskIndex[1], item[1], 1);
		ResetDailyItems(GlobalInf.dailyTaskIndex[2], item[2], 2);
	}

	public void ResetDailyItems(int taskIndex, DailyTaskItem itemLine, int linePos)
	{
		int num = 0;
		int num2 = 0;
		switch (taskIndex)
		{
		case 0:
			num = (int)GlobalInf.dailyDriveDis;
			itemLine.titleLabel.text = "Driving distance";
			break;
		case 1:
			num = GlobalInf.dailyCompleteTaskNum;
			itemLine.titleLabel.text = "Task completed";
			break;
		case 2:
			num = GlobalInf.dailyKillNum;
			itemLine.titleLabel.text = "Total kills";
			break;
		case 3:
			num = GlobalInf.dailyKillGangstarNum;
			itemLine.titleLabel.text = "Kill gangsters";
			break;
		case 4:
			num = GlobalInf.dailyKillPoliceNum;
			itemLine.titleLabel.text = "Kill cops";
			break;
		case 5:
			num = GlobalInf.dailyPlayTime / 60;
			itemLine.titleLabel.text = "Spend time";
			break;
		case 6:
			num = GlobalInf.dailyKnockDownLight;
			itemLine.titleLabel.text = "Break street lights";
			break;
		case 8:
			num = GlobalInf.dailyUpgradeItemNum;
			itemLine.titleLabel.text = "Upgrade equipment";
			break;
		case 7:
			num = GlobalInf.dailyDieNum;
			itemLine.titleLabel.text = "Get killed";
			break;
		}
		switch (linePos)
		{
		case 0:
			num2 = dailyTaskCompleteNum0[taskIndex];
			break;
		case 1:
			num2 = dailyTaskCompleteNum1[taskIndex];
			break;
		case 2:
			num2 = dailyTaskCompleteNum2[taskIndex];
			break;
		}
		if (GlobalInf.dailyTaskCompleteIndex[linePos] == 0)
		{
			if (num >= num2)
			{
				num = num2;
				itemLine.claimBtn.isEnabled = true;
			}
			else
			{
				itemLine.claimBtn.isEnabled = false;
			}
			itemLine.btnLabel.text = "claim";
		}
		else
		{
			num = num2;
			itemLine.claimBtn.isEnabled = false;
			itemLine.btnLabel.text = "completed";
		}
		itemLine.valLabel.text = string.Empty + num + "/" + num2;
		itemLine.lineSprite.fillAmount = (float)num / (float)num2;
	}

	public void ResumeDailyTask()
	{
		GlobalInf.dailyTaskInitFlag = true;
		StoreDateController.ResumeDailyTask();
	}

	public void OnClickClaim1Btn(GameObject btn)
	{
		if (item[0].claimBtn.isEnabled)
		{
			item[0].claimBtn.isEnabled = false;
			item[0].btnLabel.text = "completed";
			GetReward(GlobalInf.dailyTaskIndex[0], 0);
			GlobalInf.dailyTaskCompleteIndex[0] = 1;
			StoreDateController.SetDailyTaskCompleteIndex();
			AudioController.instance.play(AudioType.PICK_CASH);
			GlobalInf.cash += line1Prise;
			StoreDateController.SetCash();
		}
	}

	public void OnClickClaim2Btn(GameObject btn)
	{
		if (item[1].claimBtn.isEnabled)
		{
			item[1].claimBtn.isEnabled = false;
			item[1].btnLabel.text = "completed";
			GetReward(GlobalInf.dailyTaskIndex[1], 1);
			GlobalInf.dailyTaskCompleteIndex[1] = 1;
			StoreDateController.SetDailyTaskCompleteIndex();
			AudioController.instance.play(AudioType.PICK_CASH);
			GlobalInf.cash += line2Prise;
			StoreDateController.SetCash();
		}
	}

	public void OnClickClaim3Btn(GameObject btn)
	{
		if (item[2].claimBtn.isEnabled)
		{
			item[2].claimBtn.isEnabled = false;
			item[2].btnLabel.text = "completed";
			GetReward(GlobalInf.dailyTaskIndex[2], 2);
			GlobalInf.dailyTaskCompleteIndex[2] = 1;
			StoreDateController.SetDailyTaskCompleteIndex();
			AudioController.instance.play(AudioType.PICK_CASH);
			GlobalInf.gold += line3Prise;
			StoreDateController.SetGold();
		}
	}

	public void GetReward(int index, int linePos)
	{
		if (linePos == 0)
		{
		}
	}

	public bool CheckDailySignal()
	{
		InitDailyTask();
		if (item[0].claimBtn.isEnabled || item[1].claimBtn.isEnabled || item[2].claimBtn.isEnabled)
		{
			return true;
		}
		return false;
	}
}
