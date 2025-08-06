using UnityEngine;

public class GameSenceBackBtnCtl : MonoBehaviour
{
	public GAMEUISTATE curState;

	public MyStack<GAMEUISTATE> preState;

	public static GameSenceBackBtnCtl instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		preState = new MyStack<GAMEUISTATE>();
		curState = GAMEUISTATE.NORMAL;
	}

	private void Destroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	private void Update()
	{
		if (GlobalInf.firstOpenGameFlag || GameUIController.instance.healthKitTutorialFlag || GameUIController.instance.toolKitTutorialFlag)
		{
			if (Input.GetKeyUp(KeyCode.Escape) && Platform.isFullScreenSmallShowing())
			{
				Platform.hideFullScreenSmall();
			}
		}
		else
		{
			if (!Input.GetKeyUp(KeyCode.Escape))
			{
				return;
			}
			if (Platform.isFullScreenSmallShowing())
			{
				Platform.hideFullScreenSmall();
			}
			else if (!GlobalInf.videoAdsFlag)
			{
				switch (curState)
				{
				case GAMEUISTATE.BUYCASH:
					BuyCashStateBack();
					break;
				case GAMEUISTATE.BUYGOLD:
					BuyGoldStateBack();
					break;
				case GAMEUISTATE.ENDMISSION:
					EndMissionStateBack();
					break;
				case GAMEUISTATE.NORMAL:
					NormalStateBack();
					break;
				case GAMEUISTATE.PAUSE:
					PauseStateBack();
					break;
				case GAMEUISTATE.SLOT:
					SlotStateBack();
					break;
				case GAMEUISTATE.TASKCHECK:
					TaskCheckStateBack();
					break;
				case GAMEUISTATE.MAP:
					MapStateBack();
					break;
				case GAMEUISTATE.DEAD:
					DeadStateBack();
					break;
				case GAMEUISTATE.SETTING:
					SettingStateBack();
					break;
				case GAMEUISTATE.LEVELUP:
					LevelUpStateBack();
					break;
				case GAMEUISTATE.RATE:
					RatePageBack();
					break;
				case GAMEUISTATE.BUYHART:
					BuyHartBack();
					break;
				case GAMEUISTATE.BUYKIT:
					BuyKitPageBack();
					break;
				case GAMEUISTATE.ADD:
				case GAMEUISTATE.NONE:
					break;
				}
			}
		}
	}

	public void BuyKitPageBack()
	{
		GameUIController.instance.buyKitPage.OnClickCloseBtn(null);
	}

	public void RatePageBack()
	{
		if (RatePageController.instance != null)
		{
			RatePageController.instance.OnClickCloseBtn(null);
		}
	}

	public void LevelUpStateBack()
	{
		if (GameUIController.instance.taskEndUIControllor != null && GameUIController.instance.taskEndUIControllor.levelUpUIController != null)
		{
			GameUIController.instance.taskEndUIControllor.levelUpUIController.OnClickOKBtn(null);
		}
	}

	public void ChangeGameUIState(GAMEUISTATE newState)
	{
		preState.Push(curState);
		curState = newState;
	}

	public void PopGameUIState()
	{
		curState = preState.Pop();
	}

	public void SettingStateBack()
	{
		if (SettingController.instance != null)
		{
			SettingController.instance.OnClickBackBtn(null);
		}
	}

	public void NormalStateBack()
	{
		GameUIController.instance.OnClickPauseBtn(null);
	}

	public void PauseStateBack()
	{
		GameUIController.instance.pauseUIControllor.OnClickResumeBtn(null);
	}

	public void TaskCheckStateBack()
	{
		GameUIController.instance.OnClickBackTaskBtn(null);
	}

	public void EndMissionStateBack()
	{
		if (GameUIController.instance.taskEndUIControllor != null)
		{
			GameUIController.instance.taskEndUIControllor.OnClickOKBtn(null);
		}
	}

	public void SlotStateBack()
	{
		GameController.instance.slotMode.OnClickSlotBackBtn(null);
	}

	public void BuyCashStateBack()
	{
		if (TopLineController.instance != null && TopLineController.instance.rechargeCashObj != null)
		{
			TopLineController.instance.rechargeCashObj.OnClickBackBtn(null);
		}
	}

	public void BuyGoldStateBack()
	{
		if (TopLineController.instance != null && TopLineController.instance.rechargeGoldObj != null)
		{
			TopLineController.instance.rechargeGoldObj.OnClickBackBtn(null);
		}
	}

	public void MapStateBack()
	{
		GameUIController.instance.minimapController.mapBackBtn.OnClick();
	}

	public void DeadStateBack()
	{
		GameUIController.instance.OnClickDieCheckBtn(null);
	}

	public void BuyHartBack()
	{
		if (BuyHartPageController.instance != null)
		{
			BuyHartPageController.instance.OnClickBackBtn(null);
		}
	}
}
