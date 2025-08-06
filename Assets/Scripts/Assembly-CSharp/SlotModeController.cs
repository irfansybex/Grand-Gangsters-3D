using System;
using UnityEngine;

public class SlotModeController : GameModeController
{
	public SlotController slot;

	public CamChangeRoot cam;

	public GameObject slotActiveObj;

	public GameObject miniMap;

	private RaycastHit hitRay;

	public LayerMask hitLayer;

	public UIEventListener slotBackBtn;

	public GameObject slotUIRoot;

	public GameObject priseLine;

	public UISprite betLine;

	public int currentIncomeVal;

	public int[] betPriseArray;

	public int betPrise;

	public float[] winPriseArray;

	public SlotUIController slotUIController;

	public float[] betLinePos;

	public float[] priseLinePos;

	public override void Reset(int index)
	{
		cam.StartChange(slot.camPos, 0.5f);
		GameUIController.instance.rootContainner.SetActiveRecursively(false);
		GameUIController.instance.rootContainner.SetActive(true);
		slot.slotActiveObj.SetActiveRecursively(false);
		PlayerController.instance.animaCtl.playerSkinnedMesh.enabled = false;
		if (PlayerController.instance.curState == PLAYERSTATE.HANDGUN)
		{
			PlayerController.instance.gun.GetComponent<Renderer>().enabled = false;
		}
		else if (PlayerController.instance.curState == PLAYERSTATE.MACHINEGUN)
		{
			PlayerController.instance.machineGun.GetComponent<Renderer>().enabled = false;
		}
		miniMap.SetActiveRecursively(false);
		SlotController slotController = slot;
		slotController.playDone = (SlotController.PlayDone)Delegate.Combine(slotController.playDone, new SlotController.PlayDone(WinPrise));
		GameUIController.instance.topLine.gameObject.SetActiveRecursively(true);
		GameUIController.instance.topLine.checkPageFlag = false;
		slotUIController = (UnityEngine.Object.Instantiate(Resources.Load("UI/SlotUI")) as GameObject).GetComponent<SlotUIController>();
		slotUIController.transform.parent = slotUIRoot.transform;
		slotUIController.transform.localPosition = Vector3.zero;
		slotUIController.transform.localScale = Vector3.one;
		priseLine = slotUIController.priseLine;
		priseLine.SetActiveRecursively(false);
		betLine = slotUIController.betLine;
		UIEventListener backBtn = slotUIController.backBtn;
		backBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(backBtn.onClick, new UIEventListener.VoidDelegate(OnClickSlotBackBtn));
		UIEventListener betBtn = slotUIController.betBtn1;
		betBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(betBtn.onClick, new UIEventListener.VoidDelegate(OnClickBetBtn1));
		UIEventListener betBtn2 = slotUIController.betBtn2;
		betBtn2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(betBtn2.onClick, new UIEventListener.VoidDelegate(OnClickBetBtn2));
		UIEventListener betBtn3 = slotUIController.betBtn3;
		betBtn3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(betBtn3.onClick, new UIEventListener.VoidDelegate(OnClickBetBtn3));
		UIEventListener playBtn = slotUIController.playBtn;
		playBtn.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(playBtn.onClick, new UIEventListener.VoidDelegate(OnClickPlayBtn));
		SetBetLine(0);
		currentIncomeVal = 0;
	}

	public void OnClickPlayBtn(GameObject btn)
	{
		if (!slot.playingFlag)
		{
			PlaySLot();
		}
	}

	public void OnClickBetBtn1(GameObject btn)
	{
		if (!slot.playingFlag)
		{
			slot.betOneBtn.GetComponent<Animation>().Play("slotBtn");
			SetBetLine(0);
		}
	}

	public void OnClickBetBtn2(GameObject btn)
	{
		if (!slot.playingFlag)
		{
			slot.betFiveBtn.GetComponent<Animation>().Play("slotBtn");
			SetBetLine(1);
		}
	}

	public void OnClickBetBtn3(GameObject btn)
	{
		if (!slot.playingFlag)
		{
			slot.betTenBtn.GetComponent<Animation>().Play("slotBtn");
			SetBetLine(2);
		}
	}

	public void OnClickSlotBackBtn(GameObject btn)
	{
		if (!slot.playingFlag)
		{
			GameController.instance.ChangeMode(GAMEMODE.NORMAL, 0);
			GameSenceBackBtnCtl.instance.PopGameUIState();
		}
	}

	public override void MyUpdate()
	{
		base.MyUpdate();
		if (Input.touchCount <= 0 || Input.GetTouch(0).phase != 0 || !Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hitRay, 5f, hitLayer))
		{
			return;
		}
		if (hitRay.collider.gameObject.name.Equals(slot.moreGameBtn.name))
		{
			if (!slot.playingFlag)
			{
				slot.moreGameBtn.GetComponent<Animation>().Play("slotBtn");
				Platform.showMoreGames();
			}
		}
		else if (hitRay.collider.gameObject.name.Equals(slot.betOneBtn.name))
		{
			if (!slot.playingFlag)
			{
				slot.betOneBtn.GetComponent<Animation>().Play("slotBtn");
				SetBetLine(0);
			}
		}
		else if (hitRay.collider.gameObject.name.Equals(slot.betFiveBtn.name))
		{
			if (!slot.playingFlag)
			{
				slot.betFiveBtn.GetComponent<Animation>().Play("slotBtn");
				SetBetLine(1);
			}
		}
		else if (hitRay.collider.gameObject.name.Equals(slot.betTenBtn.name))
		{
			if (!slot.playingFlag)
			{
				slot.betTenBtn.GetComponent<Animation>().Play("slotBtn");
				SetBetLine(2);
			}
		}
		else if (hitRay.collider.gameObject.name.Equals(slot.cashOutBtn.name))
		{
			if (!slot.playingFlag)
			{
				slot.cashOutBtn.GetComponent<Animation>().Play("slotBtn");
				GameController.instance.ChangeMode(GAMEMODE.NORMAL, 0);
			}
		}
		else if (hitRay.collider.gameObject.name.Equals(slot.slotBar.name) && !slot.playingFlag)
		{
			Invoke("PlaySLot", 0.15f);
		}
	}

	public void SetBetLine(int lineIndex)
	{
		betPrise = betPriseArray[lineIndex];
		slotUIController.SetBetLine(lineIndex);
		priseLine.SetActiveRecursively(false);
	}

	public void SetWinPriseLine(int lineIndex)
	{
		slotUIController.SetWinPriseLine(lineIndex);
	}

	private void PlaySLot()
	{
		if (GlobalInf.cash - betPrise < 0)
		{
			GameUIController.instance.topLine.OnClickCashBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.SLOT;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
		else if (!(GameUIController.instance.topLine.rechargeCashObj != null) && !(GameUIController.instance.topLine.rechargeGoldObj != null))
		{
			slot.slotBar.GetComponent<Animation>().Play("SlotBarAnima");
			currentIncomeVal -= betPrise;
			GlobalInf.cash -= betPrise;
			StoreDateController.SetCash();
			GlobalInf.totalCashSpent += betPrise;
			StoreDateController.SetTotalCashSpent();
			priseLine.gameObject.SetActiveRecursively(false);
			slot.Play();
		}
	}

	public void WinPrise()
	{
		if (slot.winFlag)
		{
			SetWinPriseLine(slot.winIndex);
			currentIncomeVal += (int)(winPriseArray[slot.winIndex] * (float)betPrise);
			GlobalInf.cash += (int)(winPriseArray[slot.winIndex] * (float)betPrise);
			GlobalInf.totalCashEarned += (int)(winPriseArray[slot.winIndex] * (float)betPrise);
			StoreDateController.SetCash();
		}
	}

	public override void Exit()
	{
		base.Exit();
		cam.StartChange(cam.preRoot, 0.5f);
		GameUIController.instance.InitUI();
		StartLabelPool.instance.RecycleSlot(slot);
		PlayerController.instance.animaCtl.playerSkinnedMesh.enabled = true;
		if (PlayerController.instance.curState == PLAYERSTATE.HANDGUN)
		{
			PlayerController.instance.gun.GetComponent<Renderer>().enabled = true;
		}
		else if (PlayerController.instance.curState == PLAYERSTATE.MACHINEGUN)
		{
			PlayerController.instance.machineGun.GetComponent<Renderer>().enabled = true;
		}
		miniMap.SetActiveRecursively(true);
		GameUIController.instance.topLine.gameObject.SetActiveRecursively(false);
		UnityEngine.Object.Destroy(slotUIController.gameObject);
		slotUIController = null;
		CarManage.instance.playerCar.ResetPlayerCar();
	}
}
