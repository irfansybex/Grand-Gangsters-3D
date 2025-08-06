using System;
using UnityEngine;

public class CarPageControllor : WeaponPageController
{
	public ItemCarInfo[] itemCarList;

	public UIPanel pan;

	public int[] carInfoIndex;

	public int[] pageIndex;

	public string[] itemPicName;

	public Vector2[] repaireLinePos;

	public Vector2[] picSize;

	public UIEventListener cashBuyBtn;

	public UIEventListener goldBuyBtn;

	public UIEventListener equipBtn;

	public UIEventListener ammoBtn;

	public UIEventListener upgradeBtn;

	public UIButton cashBuyUIBtn;

	public UIButton goldBuyUIBtn;

	public UIButton equipUIBtn;

	public UILabel equipBtnLabel;

	public UIButton ammoUIBtn;

	public UISprite buyBtnSprite;

	public UILabel carPriseLabel;

	public UILabel carGoldPrise;

	public UILabel repairePriseLabel;

	public UISprite[] attributeLine;

	public UISprite[] attributeRedLine;

	public UILabel[] attributeLabel;

	public int[] attributeVal;

	public int[] preAttributeVal;

	public int[] attributeMaxVal;

	public float[] attributePercent;

	public float[] preAttributePercent;

	public int curHealthNum;

	public int preHealthNum;

	public int healthMaxNum;

	public float curHealthPercent;

	public float preHealthPercent;

	public UISprite healthLine;

	public UISprite healthRedLine;

	public UILabel healthLabel;

	private float changeHealthCountTime;

	private bool changeHealthValFlag;

	public UILabel carName;

	public UILabel ownedNum;

	public int buyHealthNum;

	public bool playAnimaFlag;

	public UISprite buyBtnSignal;

	public UISprite upGradeBtnSignal;

	public ParticleSystem gunBackParticle;

	public TweenColor gunBackLight;

	public UILabel unLockTipsLabel;

	public UILabel gunLevelLabel;

	public GameObject upGradeLineRoot;

	public GameObject upgradePic;

	private int preIndex;

	private bool afterEnableFlag;

	private bool upgradeFlag;

	private float changeAttributeCountTime;

	public int maxCarIndex;

	public bool changeValFlag;

	public int MOTORINDEX = 3;

	private void Awake()
	{
	}

	private void OnEnable()
	{
		myUIScrollView.curIndex = pageIndex[GlobalInf.playerCarIndex];
		weaponPageObj.gameObject.SetActiveRecursively(true);
		leftBtn.gameObject.GetComponent<UIAnchor>().Run();
		rightBtn.gameObject.GetComponent<UIAnchor>().Run();
		if (myUIScrollView.curIndex == 0)
		{
			SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex + 2]);
			SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex + 1]);
			SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex]);
		}
		else if (myUIScrollView.curIndex == itemCarList.Length - 1)
		{
			SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex - 2]);
			SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex - 1]);
			SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex]);
		}
		else
		{
			SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex - 1]);
			SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex + 1]);
			SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex]);
		}
		afterEnableFlag = true;
		preIndex = -1;
		OnResetBtn();
		if (playEnterAnimaFlag)
		{
			playEnterAnimaFlag = false;
			weaponPageObj.GetComponent<Animation>().Play("StorePageEnter");
		}
		if (myUIScrollView.curIndex == 0)
		{
			leftBtn.gameObject.SetActiveRecursively(false);
			rightBtnAnima.PlayForward();
		}
		else
		{
			rightBtnAnima.enabled = false;
			rightBtnSprite.alpha = 1f;
		}
		if (myUIScrollView.curIndex == myUIScrollView.maxIndex)
		{
			rightBtn.gameObject.SetActiveRecursively(false);
			leftBtnAnima.PlayForward();
		}
		else
		{
			leftBtnAnima.enabled = false;
			leftBtnSprite.alpha = 1f;
		}
	}

	public override void Init()
	{
		base.Init();
		MyUIScrollView obj = myUIScrollView;
		obj.onInitPage = (MyUIScrollView.OnInitPage)Delegate.Combine(obj.onInitPage, new MyUIScrollView.OnInitPage(OnInitPage));
		MyUIScrollView obj2 = myUIScrollView;
		obj2.onResetBtn = (MyUIScrollView.OnResetBtn)Delegate.Combine(obj2.onResetBtn, new MyUIScrollView.OnResetBtn(OnResetBtn));
		myUIScrollView.maxIndex = itemCarList.Length - 1;
		StoreDateController.GetCarInfoList(itemCarList);
		ResetEquip();
		if (GlobalInf.playerCarIndex != -1)
		{
			itemCarList[GlobalInf.playerCarIndex].equipedFlag = true;
		}
		cashBuyBtn.onClick = OnClickCashPurchaseBtn;
		goldBuyBtn.onClick = OnClickGoldPurchaseBtn;
		equipBtn.onClick = OnClickEquipBtn;
		ammoBtn.onClick = OnClickAmmoBtn;
		upgradeBtn.onClick = OnClickUpgradeBtn;
		myUIScrollView.Init();
		CheckCarSignal();
	}

	public void CheckCarSignal()
	{
		MenuSenceController.instance.carBtnSignal = false;
		MenuSenceController.instance.carBtnNewSignal = false;
		for (int i = 0; i <= myUIScrollView.maxIndex; i++)
		{
			if (i != 4)
			{
				if (itemCarList[i].carLevel < 2 && itemCarList[i].carNum >= itemCarList[i].upgradeCarNum[itemCarList[i].carLevel])
				{
					MenuSenceController.instance.carBtnSignal = true;
					break;
				}
				if (itemCarList[i].unlockFlag && itemCarList[i].carNum == 0)
				{
					MenuSenceController.instance.carBtnNewSignal = true;
					break;
				}
			}
		}
	}

	private void Update()
	{
		if (afterEnableFlag)
		{
			afterEnableFlag = false;
			myUIScrollView.curItemPage.animaObj.transform.localPosition += Vector3.right;
			myUIScrollView.curItemPage.itemPic.transform.localPosition += Vector3.right;
		}
		if (changeValFlag)
		{
			changeAttributeCountTime += Time.deltaTime * 2f;
			for (int i = 0; i < 3; i++)
			{
				attributeLine[i].fillAmount = Mathf.Lerp(preAttributePercent[i], attributePercent[i], changeAttributeCountTime);
				attributeRedLine[i].fillAmount = attributeLine[i].fillAmount - 0.06f;
				attributeLabel[i].text = string.Empty + (int)Mathf.Lerp(preAttributeVal[i], attributeVal[i], changeAttributeCountTime);
			}
			if (changeAttributeCountTime > 1f)
			{
				changeValFlag = false;
			}
		}
		if (changeHealthValFlag)
		{
			changeHealthCountTime += Time.deltaTime * 2f;
			healthLine.fillAmount = Mathf.Lerp(preHealthPercent, curHealthPercent, changeHealthCountTime);
			healthRedLine.fillAmount = healthLine.fillAmount - 0.06f;
			if (changeHealthCountTime > 1f)
			{
				changeHealthValFlag = false;
			}
		}
	}

	public void OnInitPage(ItemPageController itemPage, int index)
	{
		itemPage.index = index;
		itemPage.myUIScrollView = myUIScrollView;
		SetPage(itemPage, carInfoIndex[index]);
	}

	public void SetPage(ItemPageController itemPage, int index)
	{
		if (itemCarList[index].unlockFlag)
		{
			if (itemCarList[index].buyFlag)
			{
				if (itemCarList[index].equipedFlag)
				{
					if (!playAnimaFlag)
					{
						itemPage.animaObj.gameObject.SetActiveRecursively(true);
						itemPage.animaObj.SetFinalState();
					}
					else
					{
						playAnimaFlag = false;
						itemPage.animaObj.gameObject.SetActiveRecursively(true);
						itemPage.animaObj.GetComponent<Animation>().Play();
					}
				}
				else
				{
					itemPage.animaObj.gameObject.SetActiveRecursively(false);
				}
			}
			else
			{
				itemPage.animaObj.gameObject.SetActiveRecursively(false);
			}
			itemPage.lockPic.gameObject.SetActiveRecursively(false);
		}
		else
		{
			itemPage.animaObj.gameObject.SetActiveRecursively(false);
			itemPage.lockPic.gameObject.SetActiveRecursively(true);
		}
		itemPage.itemPic.spriteName = itemPicName[index];
		itemPage.itemPic.width = (int)(picSize[index].x * GlobalDefine.screenWidthFit);
		itemPage.itemPic.height = (int)picSize[index].y;
	}

	public void OnClickUpgradeBtn(GameObject obj)
	{
		itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum = itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum - itemCarList[carInfoIndex[myUIScrollView.curIndex]].upgradeCarNum[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel] + 1;
		if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum <= 0)
		{
			itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum = 1;
		}
		itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel++;
		upgradeFlag = true;
		OnResetBtn();
		upgradeFlag = false;
		StoreDateController.SetCarInfo(itemCarList[carInfoIndex[myUIScrollView.curIndex]], carInfoIndex[myUIScrollView.curIndex]);
		Platform.flurryEvent_onEquipmentCarUpgrade(carInfoIndex[myUIScrollView.curIndex], itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel);
		gunBackLight.ResetToBeginning();
		gunBackLight.PlayForward();
		gunBackParticle.Play();
		GlobalInf.dailyUpgradeItemNum++;
		if (AudioController.instance != null)
		{
			AudioController.instance.play(AudioType.UPGRADE);
		}
		CheckCarSignal();
		MenuSenceController.instance.storeMenu.CheckSignal();
		if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel < 2)
		{
			preHealthNum = curHealthNum;
			curHealthNum = itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum;
			preHealthPercent = curHealthPercent;
			curHealthPercent = (float)itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum / (float)itemCarList[carInfoIndex[myUIScrollView.curIndex]].upgradeCarNum[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel];
			changeHealthCountTime = 0f;
			changeHealthValFlag = true;
		}
	}

	public void OnClickAmmoBtn(GameObject obj)
	{
		if (GlobalInf.cash > itemCarList[carInfoIndex[myUIScrollView.curIndex]].repairePrise)
		{
			itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.restHealthVal = itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.maxHealthVal;
			if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.restHealthVal > itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.maxHealthVal)
			{
				itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.restHealthVal = itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.maxHealthVal;
			}
			StoreDateController.SetCarInfo(itemCarList[carInfoIndex[myUIScrollView.curIndex]], carInfoIndex[myUIScrollView.curIndex]);
			CopyCarInfo(itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo);
			if (myUIScrollView.curIndex == MOTORINDEX)
			{
				CopyPreCarInfo(itemCarList[GlobalInf.preCarIndex].carInfo);
			}
			GlobalInf.cash -= itemCarList[carInfoIndex[myUIScrollView.curIndex]].repairePrise;
			StoreDateController.SetCash();
			GlobalInf.totalCashSpent += itemCarList[carInfoIndex[myUIScrollView.curIndex]].repairePrise;
			StoreDateController.SetTotalCashSpent();
			itemCarList[carInfoIndex[myUIScrollView.curIndex]].repairePrise = 0;
			healthLabel.text = string.Empty + itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.restHealthVal + "/" + itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.maxHealthVal;
			OnResetBtn();
		}
		else
		{
			TopLineController.instance.OnClickCashBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.BULLET;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void OnResetBtn()
	{
		for (int num = itemCarList.Length - 1; num >= 0; num--)
		{
			if (itemCarList[num].carNum > 0)
			{
				maxCarIndex = num;
				break;
			}
		}
		if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].unlockFlag)
		{
			unLockTipsLabel.gameObject.SetActiveRecursively(false);
			if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].buyFlag)
			{
				ammoUIBtn.gameObject.SetActiveRecursively(true);
				equipUIBtn.gameObject.SetActiveRecursively(true);
				if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.restHealthVal < itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.maxHealthVal)
				{
					ammoUIBtn.isEnabled = true;
				}
				else
				{
					ammoUIBtn.isEnabled = false;
				}
				if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].equipedFlag)
				{
					equipUIBtn.isEnabled = false;
					equipBtnLabel.text = "EQUIPPED";
				}
				else
				{
					equipUIBtn.isEnabled = true;
					equipBtnLabel.text = "EQUIP";
				}
				if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].upgradeCarNum[0] != 0)
				{
					if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel < 2)
					{
						if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum < itemCarList[carInfoIndex[myUIScrollView.curIndex]].upgradeCarNum[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel])
						{
							upgradeBtn.gameObject.SetActiveRecursively(false);
						}
						else
						{
							upgradeBtn.gameObject.SetActiveRecursively(true);
						}
					}
					else
					{
						upgradeBtn.gameObject.SetActiveRecursively(false);
					}
				}
				else
				{
					upgradeBtn.gameObject.SetActiveRecursively(false);
				}
			}
			else
			{
				ammoUIBtn.gameObject.SetActiveRecursively(false);
				equipUIBtn.gameObject.SetActiveRecursively(false);
				ammoUIBtn.isEnabled = false;
				equipUIBtn.isEnabled = false;
				equipBtnLabel.text = "EQUIP";
				upgradeBtn.gameObject.SetActiveRecursively(false);
			}
			if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel < 2)
			{
				if (!upgradeBtn.gameObject.active)
				{
					if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carPrise != 0)
					{
						cashBuyUIBtn.gameObject.SetActiveRecursively(true);
						cashBuyUIBtn.isEnabled = true;
						goldBuyUIBtn.transform.localPosition = new Vector3(0.37f * GlobalDefine.screenRatioWidth, -110f, 0f);
						goldBuyUIBtn.gameObject.SetActiveRecursively(true);
						goldBuyUIBtn.isEnabled = true;
					}
					else
					{
						cashBuyUIBtn.gameObject.SetActiveRecursively(false);
						cashBuyUIBtn.isEnabled = false;
						goldBuyUIBtn.gameObject.SetActiveRecursively(false);
						goldBuyUIBtn.isEnabled = false;
					}
				}
				else
				{
					cashBuyUIBtn.gameObject.SetActiveRecursively(false);
					goldBuyUIBtn.gameObject.SetActiveRecursively(false);
				}
			}
			else
			{
				cashBuyUIBtn.gameObject.SetActiveRecursively(false);
				cashBuyUIBtn.isEnabled = false;
				goldBuyUIBtn.gameObject.SetActiveRecursively(false);
				goldBuyUIBtn.isEnabled = false;
			}
		}
		else
		{
			cashBuyUIBtn.gameObject.SetActiveRecursively(false);
			cashBuyUIBtn.isEnabled = false;
			if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carPrise != 0)
			{
				if (carInfoIndex[myUIScrollView.curIndex] <= maxCarIndex + 1)
				{
					goldBuyUIBtn.gameObject.SetActiveRecursively(true);
					goldBuyUIBtn.isEnabled = true;
					goldBuyUIBtn.transform.localPosition = new Vector3(0.27f * GlobalDefine.screenRatioWidth, -110f, 0f);
				}
				else
				{
					goldBuyUIBtn.gameObject.SetActiveRecursively(false);
					goldBuyUIBtn.isEnabled = false;
				}
			}
			else
			{
				goldBuyUIBtn.gameObject.SetActiveRecursively(false);
				goldBuyUIBtn.isEnabled = false;
			}
			ammoUIBtn.gameObject.SetActiveRecursively(false);
			equipUIBtn.gameObject.SetActiveRecursively(false);
			ammoUIBtn.isEnabled = false;
			equipUIBtn.isEnabled = false;
			equipBtnLabel.text = "EQUIP";
			upgradeBtn.gameObject.SetActiveRecursively(false);
			unLockTipsLabel.gameObject.SetActiveRecursively(true);
			if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].unlockByLevel != 99)
			{
				unLockTipsLabel.text = "Unlock at area " + (itemCarList[carInfoIndex[myUIScrollView.curIndex]].unlockByLevel + 1);
			}
			else
			{
				unLockTipsLabel.text = "Collecting Box to Unlock";
			}
		}
		carName.text = itemCarList[carInfoIndex[myUIScrollView.curIndex]].carName;
		gunLevelLabel.text = "Lv" + (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel + 1);
		if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].upgradeCarNum[0] != 0)
		{
			NGUITools.SetActiveRecursively(upGradeLineRoot, true);
			if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel < 2)
			{
				ownedNum.text = string.Empty + itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum + "/" + itemCarList[carInfoIndex[myUIScrollView.curIndex]].upgradeCarNum[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel];
				upgradePic.gameObject.SetActiveRecursively(true);
			}
			else
			{
				ownedNum.text = "MAX";
				upgradePic.gameObject.SetActiveRecursively(false);
			}
		}
		else
		{
			NGUITools.SetActiveRecursively(upGradeLineRoot, false);
		}
		if (preIndex != myUIScrollView.curIndex || upgradeFlag)
		{
			preIndex = myUIScrollView.curIndex;
			changeValFlag = true;
			preAttributeVal[0] = attributeVal[0];
			preAttributeVal[1] = attributeVal[1];
			preAttributeVal[2] = attributeVal[2];
			attributeVal[0] = (int)itemCarList[carInfoIndex[myUIScrollView.curIndex]].maxSpeedLevelList[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel];
			attributeVal[1] = (int)itemCarList[carInfoIndex[myUIScrollView.curIndex]].maxSteerAngleLevelList[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel];
			attributeVal[2] = itemCarList[carInfoIndex[myUIScrollView.curIndex]].maxHealthLevelList[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel];
			preAttributePercent[0] = attributePercent[0];
			preAttributePercent[1] = attributePercent[1];
			preAttributePercent[2] = attributePercent[2];
			attributePercent[0] = (float)attributeVal[0] / (float)attributeMaxVal[0];
			attributePercent[1] = (float)attributeVal[1] / (float)attributeMaxVal[1];
			attributePercent[2] = (float)attributeVal[2] / (float)attributeMaxVal[2];
			changeAttributeCountTime = 0f;
			preHealthNum = curHealthNum;
			curHealthNum = itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum;
			preHealthPercent = curHealthPercent;
			if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel < 2)
			{
				if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].upgradeCarNum[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel] != 0)
				{
					curHealthPercent = (float)itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum / (float)itemCarList[carInfoIndex[myUIScrollView.curIndex]].upgradeCarNum[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel];
				}
				else
				{
					curHealthPercent = 0f;
				}
			}
			else
			{
				curHealthPercent = 1f;
			}
			changeHealthCountTime = 0f;
			changeHealthValFlag = true;
			healthLabel.text = string.Empty + itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.restHealthVal + "/" + itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo.maxHealthVal;
		}
		if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].carPrise != 0)
		{
			carPriseLabel.text = string.Empty + itemCarList[carInfoIndex[myUIScrollView.curIndex]].carPrise;
			carGoldPrise.text = string.Empty + itemCarList[carInfoIndex[myUIScrollView.curIndex]].carGoldPrise;
		}
		else
		{
			carPriseLabel.gameObject.SetActiveRecursively(false);
			carGoldPrise.gameObject.SetActiveRecursively(false);
		}
		repairePriseLabel.text = string.Empty + itemCarList[carInfoIndex[myUIScrollView.curIndex]].repairePrise;
		CheckSignal();
	}

	public void CheckSignal()
	{
		if (itemCarList[carInfoIndex[myUIScrollView.curIndex]].unlockFlag && itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum == 0)
		{
			buyBtnSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			buyBtnSignal.gameObject.SetActiveRecursively(false);
		}
		if (upgradeBtn.gameObject.active)
		{
			upGradeBtnSignal.gameObject.SetActiveRecursively(true);
		}
		else
		{
			upGradeBtnSignal.gameObject.SetActiveRecursively(false);
		}
		MenuSenceController.instance.storeMenu.CheckSignal();
	}

	public void OnClickEquipBtn(GameObject obj)
	{
		ResetEquip();
		itemCarList[carInfoIndex[myUIScrollView.curIndex]].equipedFlag = true;
		if (myUIScrollView.curIndex == MOTORINDEX)
		{
			GlobalInf.preCarIndex = GlobalInf.playerCarIndex;
			StoreDateController.SetPreCarIndex(GlobalInf.preCarIndex);
		}
		GlobalInf.playerCarIndex = carInfoIndex[myUIScrollView.curIndex];
		StoreDateController.SetCarIndex(GlobalInf.playerCarIndex);
		CopyCarInfo(itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo);
		if (myUIScrollView.curIndex == MOTORINDEX)
		{
			CopyPreCarInfo(itemCarList[GlobalInf.preCarIndex].carInfo);
		}
		playAnimaFlag = true;
		if (myUIScrollView.curIndex == 0)
		{
			SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex + 2]);
			SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex + 1]);
			SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex]);
		}
		else if (myUIScrollView.curIndex == itemCarList.Length - 1)
		{
			SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex - 2]);
			SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex - 1]);
			SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex]);
		}
		else
		{
			SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex - 1]);
			SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex]);
			SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex + 1]);
		}
		OnResetBtn();
	}

	public void OnClickCashPurchaseBtn(GameObject obj)
	{
		if (GlobalInf.cash - itemCarList[carInfoIndex[myUIScrollView.curIndex]].carPrise > 0)
		{
			GlobalInf.cash -= itemCarList[carInfoIndex[myUIScrollView.curIndex]].carPrise;
			StoreDateController.SetCash();
			GlobalInf.totalCashSpent += itemCarList[carInfoIndex[myUIScrollView.curIndex]].carPrise;
			StoreDateController.SetTotalCashSpent();
			Platform.flurryEvent_onEquipmentCarPurchase(carInfoIndex[myUIScrollView.curIndex]);
			if (!itemCarList[carInfoIndex[myUIScrollView.curIndex]].buyFlag)
			{
				itemCarList[carInfoIndex[myUIScrollView.curIndex]].buyFlag = true;
				ResetEquip();
				itemCarList[carInfoIndex[myUIScrollView.curIndex]].equipedFlag = true;
				if (myUIScrollView.curIndex == MOTORINDEX)
				{
					GlobalInf.preCarIndex = GlobalInf.playerCarIndex;
					StoreDateController.SetPreCarIndex(GlobalInf.preCarIndex);
				}
				GlobalInf.playerCarIndex = carInfoIndex[myUIScrollView.curIndex];
				StoreDateController.SetCarInfo(itemCarList[carInfoIndex[myUIScrollView.curIndex]], carInfoIndex[myUIScrollView.curIndex]);
				StoreDateController.SetCarIndex(GlobalInf.playerCarIndex);
				CopyCarInfo(itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo);
				if (myUIScrollView.curIndex == MOTORINDEX)
				{
					CopyPreCarInfo(itemCarList[GlobalInf.preCarIndex].carInfo);
				}
				playAnimaFlag = true;
				Platform.flurryEvent_onEquipmentCarGetPurchase(carInfoIndex[myUIScrollView.curIndex]);
			}
			itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum++;
			if (myUIScrollView.curIndex == 0)
			{
				SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex + 2]);
				SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex + 1]);
				SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex]);
			}
			else if (myUIScrollView.curIndex == itemCarList.Length - 1)
			{
				SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex - 2]);
				SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex - 1]);
				SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex]);
			}
			else
			{
				SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex - 1]);
				SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex]);
				SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex + 1]);
			}
			StoreDateController.SetCarInfo(itemCarList[carInfoIndex[myUIScrollView.curIndex]], carInfoIndex[myUIScrollView.curIndex]);
			CheckCarSignal();
			MenuSenceController.instance.storeMenu.CheckSignal();
			OnResetBtn();
			gunBackLight.ResetToBeginning();
			gunBackLight.PlayForward();
			gunBackParticle.Play();
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.GET_ITEM);
			}
			preHealthNum = curHealthNum;
			curHealthNum = itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum;
			preHealthPercent = curHealthPercent;
			curHealthPercent = (float)itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum / (float)itemCarList[carInfoIndex[myUIScrollView.curIndex]].upgradeCarNum[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel];
			changeHealthCountTime = 0f;
			changeHealthValFlag = true;
		}
		else
		{
			TopLineController.instance.OnClickCashBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.EQUIPMENT;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void OnClickGoldPurchaseBtn(GameObject obj)
	{
		if (GlobalInf.gold - itemCarList[carInfoIndex[myUIScrollView.curIndex]].carGoldPrise > 0)
		{
			GlobalInf.gold -= itemCarList[carInfoIndex[myUIScrollView.curIndex]].carGoldPrise;
			StoreDateController.SetGold();
			Platform.flurryEvent_onEquipmentCarPurchase(carInfoIndex[myUIScrollView.curIndex]);
			if (!itemCarList[carInfoIndex[myUIScrollView.curIndex]].unlockFlag)
			{
				itemCarList[carInfoIndex[myUIScrollView.curIndex]].unlockFlag = true;
			}
			if (!itemCarList[carInfoIndex[myUIScrollView.curIndex]].buyFlag)
			{
				itemCarList[carInfoIndex[myUIScrollView.curIndex]].buyFlag = true;
				ResetEquip();
				itemCarList[carInfoIndex[myUIScrollView.curIndex]].equipedFlag = true;
				if (myUIScrollView.curIndex == MOTORINDEX)
				{
					GlobalInf.preCarIndex = GlobalInf.playerCarIndex;
					StoreDateController.SetPreCarIndex(GlobalInf.preCarIndex);
				}
				GlobalInf.playerCarIndex = carInfoIndex[myUIScrollView.curIndex];
				StoreDateController.SetCarInfo(itemCarList[carInfoIndex[myUIScrollView.curIndex]], carInfoIndex[myUIScrollView.curIndex]);
				StoreDateController.SetCarIndex(GlobalInf.playerCarIndex);
				CopyCarInfo(itemCarList[carInfoIndex[myUIScrollView.curIndex]].carInfo);
				if (myUIScrollView.curIndex == MOTORINDEX)
				{
					CopyPreCarInfo(itemCarList[GlobalInf.preCarIndex].carInfo);
				}
				playAnimaFlag = true;
				Platform.flurryEvent_onEquipmentCarGetPurchase(carInfoIndex[myUIScrollView.curIndex]);
			}
			itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum++;
			if (myUIScrollView.curIndex == 0)
			{
				SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex + 2]);
				SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex + 1]);
				SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex]);
			}
			else if (myUIScrollView.curIndex == itemCarList.Length - 1)
			{
				SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex - 2]);
				SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex - 1]);
				SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex]);
			}
			else
			{
				SetPage(myUIScrollView.leftItemPage, carInfoIndex[myUIScrollView.curIndex - 1]);
				SetPage(myUIScrollView.middleItemPage, carInfoIndex[myUIScrollView.curIndex]);
				SetPage(myUIScrollView.rightItemPage, carInfoIndex[myUIScrollView.curIndex + 1]);
			}
			StoreDateController.SetCarInfo(itemCarList[carInfoIndex[myUIScrollView.curIndex]], carInfoIndex[myUIScrollView.curIndex]);
			CheckCarSignal();
			MenuSenceController.instance.storeMenu.CheckSignal();
			OnResetBtn();
			gunBackLight.ResetToBeginning();
			gunBackLight.PlayForward();
			gunBackParticle.Play();
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.GET_ITEM);
			}
			preHealthNum = curHealthNum;
			curHealthNum = itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum;
			preHealthPercent = curHealthPercent;
			curHealthPercent = (float)itemCarList[carInfoIndex[myUIScrollView.curIndex]].carNum / (float)itemCarList[carInfoIndex[myUIScrollView.curIndex]].upgradeCarNum[itemCarList[carInfoIndex[myUIScrollView.curIndex]].carLevel];
			changeHealthCountTime = 0f;
			changeHealthValFlag = true;
		}
		else
		{
			TopLineController.instance.OnClickGoldBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.EQUIPMENT;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void ResetEquip()
	{
		for (int i = 0; i < itemCarList.Length; i++)
		{
			itemCarList[i].equipedFlag = false;
		}
	}

	public void CopyCarInfo(CarInfo info)
	{
		GlobalInf.carInfo.brakeForce = info.brakeForce;
		GlobalInf.carInfo.maxHealthVal = info.maxHealthVal;
		GlobalInf.carInfo.maxSpeed = info.maxSpeed;
		GlobalInf.carInfo.maxSpeedSteerAngle = info.maxSpeedSteerAngle;
		GlobalInf.carInfo.maxSteerAngle = info.maxSteerAngle;
		GlobalInf.carInfo.restHealthVal = info.restHealthVal;
	}

	public void CopyPreCarInfo(CarInfo info)
	{
		GlobalInf.preCarInfo.brakeForce = info.brakeForce;
		GlobalInf.preCarInfo.maxHealthVal = info.maxHealthVal;
		GlobalInf.preCarInfo.maxSpeed = info.maxSpeed;
		GlobalInf.preCarInfo.maxSpeedSteerAngle = info.maxSpeedSteerAngle;
		GlobalInf.preCarInfo.maxSteerAngle = info.maxSteerAngle;
		GlobalInf.preCarInfo.restHealthVal = info.restHealthVal;
	}
}
