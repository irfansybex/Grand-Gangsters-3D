using UnityEngine;

public class MenuSenceBackBtnCtl : MonoBehaviour
{
	public MENUUISTATE curState;

	public MyStack<MENUUISTATE> preState;

	public static MenuSenceBackBtnCtl instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		preState = new MyStack<MENUUISTATE>();
		if (!GlobalInf.backToHandGunPageFlag && !GlobalInf.backToCarPageFlag)
		{
			curState = MENUUISTATE.MAINPAGE;
		}
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	private void Update()
	{
		if (!Input.GetKeyUp(KeyCode.Escape) || GlobalInf.upgradeTutorialFlag)
		{
			return;
		}
		if (Platform.isFullScreenSmallShowing())
		{
			ADPageBack();
		}
		else if (!GlobalInf.videoAdsFlag)
		{
			switch (curState)
			{
			case MENUUISTATE.BUYCASHPAGE:
				BuyCashPageBack();
				break;
			case MENUUISTATE.BUYGOLDPAGE:
				BuyGoldPageBack();
				break;
			case MENUUISTATE.CHARACTERPAGE:
				CharacterPageBack();
				break;
			case MENUUISTATE.ACHIEVEMENTPAGE:
				AchievementPageBack();
				break;
			case MENUUISTATE.COLLECTINGPAGE:
				CollectingPageBack();
				break;
			case MENUUISTATE.DAILYBOUNSEPAGE:
				DailyBounsePageBack();
				break;
			case MENUUISTATE.EXITPAGE:
				ExitPageBack();
				break;
			case MENUUISTATE.HANDGUNPAGE:
				HandGunPageBack();
				break;
			case MENUUISTATE.ITEMSPAGE:
				ItemsPageBack();
				break;
			case MENUUISTATE.MACHINEGUNPAGE:
				MachineGunPageBack();
				break;
			case MENUUISTATE.MAINPAGE:
				MainPageBack();
				break;
			case MENUUISTATE.PACKSPAGE:
				PacksPageBack();
				break;
			case MENUUISTATE.SALEPAGE:
				SalePageBack();
				break;
			case MENUUISTATE.SETTINGPAGE:
				SettingPageBack();
				break;
			case MENUUISTATE.VECHICLESPAGE:
				VechiclesPageBack();
				break;
			case MENUUISTATE.CLAIMPAGE:
				ClaimPageBack();
				break;
			case MENUUISTATE.DAILYTASKPAGE:
				DailyTaskPageBack();
				break;
			case MENUUISTATE.BUYHARTPAGE:
				BuyHartPageBack();
				break;
			}
		}
	}

	public void ChangeMenuUIState(MENUUISTATE newState)
	{
		preState.Push(curState);
		curState = newState;
	}

	public void PopMenuUIState()
	{
		curState = preState.Pop();
	}

	public void MainPageBack()
	{
		MenuSenceController.instance.OnEnableExitUI();
	}

	public void SettingPageBack()
	{
		if (SettingController.instance != null)
		{
			SettingController.instance.OnClickBackBtn(null);
		}
	}

	public void DailyBounsePageBack()
	{
		if (DailyBonusController.instance != null)
		{
			DailyBonusController.instance.OnClickBackBtn(null);
		}
	}

	public void CharacterPageBack()
	{
		MenuSenceController.instance.characterMenu.OnClickBackBtn(null);
	}

	public void AchievementPageBack()
	{
		MenuSenceController.instance.characterMenu.OnClickBackBtn(null);
	}

	public void CollectingPageBack()
	{
		MenuSenceController.instance.characterMenu.OnClickBackBtn(null);
	}

	public void PacksPageBack()
	{
		MenuSenceController.instance.storeMenu.OnClickBackBtn(null);
	}

	public void HandGunPageBack()
	{
		MenuSenceController.instance.storeMenu.OnClickBackBtn(null);
	}

	public void MachineGunPageBack()
	{
		MenuSenceController.instance.storeMenu.OnClickBackBtn(null);
	}

	public void VechiclesPageBack()
	{
		MenuSenceController.instance.storeMenu.OnClickBackBtn(null);
	}

	public void ItemsPageBack()
	{
		MenuSenceController.instance.storeMenu.OnClickBackBtn(null);
	}

	public void BuyCashPageBack()
	{
		if (TopLineController.instance != null && TopLineController.instance.rechargeCashObj != null)
		{
			TopLineController.instance.rechargeCashObj.OnClickBackBtn(null);
		}
	}

	public void BuyGoldPageBack()
	{
		if (TopLineController.instance != null && TopLineController.instance.rechargeGoldObj != null)
		{
			TopLineController.instance.rechargeGoldObj.OnClickBackBtn(null);
		}
	}

	public void ClaimPageBack()
	{
		MenuSenceController.instance.getItemPage.OnClickClaimBtn(null);
	}

	public void ADPageBack()
	{
		Platform.hideFullScreenSmall();
	}

	public void SalePageBack()
	{
	}

	public void ExitPageBack()
	{
		MenuSenceController.instance.OnDisableExitUI();
	}

	public void DailyTaskPageBack()
	{
		MenuSenceController.instance.startMenu.dailyTaskPage.OnClickBackBtn(null);
	}

	public void BuyHartPageBack()
	{
		if (BuyHartPageController.instance != null)
		{
			BuyHartPageController.instance.OnClickBackBtn(null);
		}
	}
}
