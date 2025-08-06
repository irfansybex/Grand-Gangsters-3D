using System;
using UnityEngine;

public class HandGunPageController : WeaponPageController
{
	public ItemGunInfo[] itemHandGunList;

	public UIPanel pan;

	public string[] itemPicName;

	public Vector2[] bulletLinePos;

	public Vector2[] picSize;

	public UIEventListener cashBuyBtn;

	public UIEventListener goldBuyBtn;

	public UIEventListener equipBtn;

	public UIEventListener ammoBtn;

	public UIEventListener upgradeBtn;

	public UIButton cashBuyUIBtn;

	public UIButton goldBuyUIBtn;

	public UIButton equipUIBtn;

	public UILabel equipLabel;

	public UIButton ammoUIBtn;

	public UISprite buyBtnSprite;

	public UILabel gunCashPrise;

	public UILabel gunGoldPrise;

	public UILabel bulletPrise;

	public UISprite[] attributeLine;

	public UISprite[] attributeRedLine;

	public UILabel[] attributeLabel;

	public int[] attributeVal;

	public int[] preAttributeVal;

	public int[] attributeMaxVal;

	public float[] attributePercent;

	public float[] preAttributePercent;

	private float changeAttributeCountTime;

	public int bulletNum;

	public int preBulletNum;

	public int bulletMaxNum;

	public float bulletPercent;

	public float preBulletPercent;

	public UISprite bulletLine;

	public UISprite bulletRedLine;

	public UILabel bulletLabel;

	private float changeBulletCountTime;

	private bool changeBulletValFlag;

	public UILabel gunName;

	public UILabel gunLevel;

	public UILabel ownedNum;

	public int buyBulletNum;

	public bool playAnimaFlag;

	public UISprite buyBtnSignal;

	public UISprite upGradeBtnSignal;

	public ParticleSystem gunBackParticle;

	public TweenColor gunBackLight;

	public UILabel unLockTipsLabel;

	public UIWidget upgradeLineRoot;

	public UISprite upgradePic;

	public int maxGunIndex;

	private int preIndex;

	private bool afterEnableFlag;

	private int bulletTempPrise;

	private bool upgradeFlag;

	public bool changeValFlag;

	private void Awake()
	{
	}

	private void OnEnable()
	{
		myUIScrollView.curIndex = GlobalInf.handgunIndex;
		if (GlobalInf.upgradeTutorialFlag)
		{
			myUIScrollView.curIndex = 0;
		}
		weaponPageObj.gameObject.SetActiveRecursively(true);
		leftBtn.gameObject.GetComponent<UIAnchor>().Run();
		rightBtn.gameObject.GetComponent<UIAnchor>().Run();
		if (myUIScrollView.curIndex == 0)
		{
			SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 2);
			SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex + 1);
			SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex);
		}
		else if (myUIScrollView.curIndex == itemHandGunList.Length - 1)
		{
			SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 2);
			SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex - 1);
			SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex);
		}
		else
		{
			SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 1);
			SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 1);
			SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex);
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
		myUIScrollView.maxIndex = itemHandGunList.Length - 1;
		StoreDateController.GetHandGunInfoList(itemHandGunList);
		ResetEquip();
		if (GlobalInf.handgunIndex != -1)
		{
			itemHandGunList[GlobalInf.handgunIndex].equipedFlag = true;
		}
		cashBuyBtn.onClick = OnClickCashPurchaseBtn;
		goldBuyBtn.onClick = OnClickGoldBuyBtn;
		equipBtn.onClick = OnClickEquipBtn;
		ammoBtn.onClick = OnClickAmmoBtn;
		upgradeBtn.onClick = OnClickUpgradeBtn;
		myUIScrollView.Init();
		CheckHandGunSignal();
	}

	public void CheckHandGunSignal()
	{
		MenuSenceController.instance.handGunBtnSignal = false;
		MenuSenceController.instance.handGunBtnNewSignal = false;
		for (int i = 0; i <= myUIScrollView.maxIndex - 1; i++)
		{
			if (itemHandGunList[i].gunInfo.level < 2 && itemHandGunList[i].gunNum >= itemHandGunList[i].upgradeGunNum[itemHandGunList[i].gunInfo.level])
			{
				MenuSenceController.instance.handGunBtnSignal = true;
				break;
			}
			if (itemHandGunList[i].unlockFlag && itemHandGunList[i].gunNum == 0)
			{
				MenuSenceController.instance.handGunBtnNewSignal = true;
				break;
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
		if (changeBulletValFlag)
		{
			changeBulletCountTime += Time.deltaTime * 2f;
			bulletLine.fillAmount = Mathf.Lerp(preBulletPercent, bulletPercent, changeBulletCountTime);
			bulletRedLine.fillAmount = bulletLine.fillAmount - 0.06f;
			if (changeBulletCountTime > 1f)
			{
				changeBulletValFlag = false;
			}
		}
	}

	public void OnInitPage(ItemPageController itemPage, int index)
	{
		itemPage.index = index;
		itemPage.myUIScrollView = myUIScrollView;
		SetPage(itemPage, index);
	}

	public void SetPage(ItemPageController itemPage, int index)
	{
		if (itemHandGunList[index].unlockFlag)
		{
			if (itemHandGunList[index].buyFlag)
			{
				if (itemHandGunList[index].equipedFlag)
				{
					if (!playAnimaFlag)
					{
						NGUITools.SetActiveRecursively(itemPage.animaObj.gameObject, false);
						NGUITools.SetActiveRecursively(itemPage.animaObj.gameObject, true);
						itemPage.animaObj.SetFinalState();
					}
					else
					{
						playAnimaFlag = false;
						NGUITools.SetActiveRecursively(itemPage.animaObj.gameObject, false);
						NGUITools.SetActiveRecursively(itemPage.animaObj.gameObject, true);
						itemPage.animaObj.GetComponent<Animation>().Play();
					}
				}
				else
				{
					NGUITools.SetActiveRecursively(itemPage.animaObj.gameObject, false);
				}
			}
			else
			{
				NGUITools.SetActiveRecursively(itemPage.animaObj.gameObject, false);
			}
			itemPage.lockPic.gameObject.SetActiveRecursively(false);
		}
		else
		{
			NGUITools.SetActiveRecursively(itemPage.animaObj.gameObject, false);
			itemPage.lockPic.gameObject.SetActiveRecursively(true);
		}
		itemPage.itemPic.spriteName = itemPicName[index];
		itemPage.itemPic.width = (int)(picSize[index].x * GlobalDefine.screenWidthFit);
		itemPage.itemPic.height = (int)picSize[index].y;
	}

	public void OnClickUpgradeBtn(GameObject obj)
	{
		PlayerPrefs.SetInt("UpgradeTutorialFlag", 1);
		itemHandGunList[myUIScrollView.curIndex].gunNum = itemHandGunList[myUIScrollView.curIndex].gunNum - itemHandGunList[myUIScrollView.curIndex].upgradeGunNum[itemHandGunList[myUIScrollView.curIndex].gunInfo.level] + 1;
		if (itemHandGunList[myUIScrollView.curIndex].gunNum <= 0)
		{
			itemHandGunList[myUIScrollView.curIndex].gunNum = 1;
		}
		itemHandGunList[myUIScrollView.curIndex].gunInfo.level++;
		Platform.flurryEvent_onEquipmentHandGunUpgrade(myUIScrollView.curIndex, itemHandGunList[myUIScrollView.curIndex].gunInfo.level);
		upgradeFlag = true;
		OnResetBtn();
		upgradeFlag = false;
		StoreDateController.SetHandGunInfo(itemHandGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
		GlobalInf.dailyUpgradeItemNum++;
		gunBackLight.ResetToBeginning();
		gunBackLight.PlayForward();
		gunBackParticle.Play();
		if (AudioController.instance != null)
		{
			AudioController.instance.play(AudioType.UPGRADE);
		}
		CheckHandGunSignal();
		MenuSenceController.instance.storeMenu.CheckSignal();
		if (itemHandGunList[myUIScrollView.curIndex].gunInfo.level < 2)
		{
			preBulletNum = bulletNum;
			bulletNum = itemHandGunList[myUIScrollView.curIndex].gunNum;
			preBulletPercent = bulletPercent;
			bulletPercent = (float)itemHandGunList[myUIScrollView.curIndex].gunNum / (float)itemHandGunList[myUIScrollView.curIndex].upgradeGunNum[itemHandGunList[myUIScrollView.curIndex].gunInfo.level];
			changeBulletValFlag = true;
			changeBulletCountTime = 0f;
		}
	}

	public void OnClickAmmoBtn(GameObject obj)
	{
		bulletTempPrise = (itemHandGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum - itemHandGunList[myUIScrollView.curIndex].gunInfo.restBulletNum) * itemHandGunList[myUIScrollView.curIndex].gunInfo.bulletPrise;
		if (GlobalInf.cash > bulletTempPrise)
		{
			itemHandGunList[myUIScrollView.curIndex].gunInfo.restBulletNum = itemHandGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum;
			StoreDateController.SetHandGunInfo(itemHandGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
			CopyHandGunInfo(itemHandGunList[myUIScrollView.curIndex].gunInfo);
			GlobalInf.cash -= bulletTempPrise;
			StoreDateController.SetCash();
			GlobalInf.totalCashSpent += bulletTempPrise;
			StoreDateController.SetTotalCashSpent();
			bulletLabel.text = string.Empty + itemHandGunList[myUIScrollView.curIndex].gunInfo.restBulletNum + "/" + itemHandGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum;
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
		for (int num = itemHandGunList.Length - 1; num >= 0; num--)
		{
			if (itemHandGunList[num].gunNum > 0)
			{
				maxGunIndex = num;
				break;
			}
		}
		if (maxGunIndex == 0)
		{
			maxGunIndex = 1;
		}
		if (itemHandGunList[myUIScrollView.curIndex].unlockFlag)
		{
			unLockTipsLabel.gameObject.SetActiveRecursively(false);
			if (itemHandGunList[myUIScrollView.curIndex].buyFlag)
			{
				ammoUIBtn.gameObject.SetActiveRecursively(true);
				equipUIBtn.gameObject.SetActiveRecursively(true);
				if (itemHandGunList[myUIScrollView.curIndex].gunInfo.restBulletNum < itemHandGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum)
				{
					ammoUIBtn.isEnabled = true;
				}
				else
				{
					ammoUIBtn.isEnabled = false;
				}
				if (itemHandGunList[myUIScrollView.curIndex].equipedFlag)
				{
					equipUIBtn.isEnabled = false;
					equipLabel.text = "EQUIPPED";
				}
				else
				{
					equipUIBtn.isEnabled = true;
					equipLabel.text = "EQUIP";
				}
				if (itemHandGunList[myUIScrollView.curIndex].upgradeGunNum[0] != 0)
				{
					if (itemHandGunList[myUIScrollView.curIndex].gunInfo.level < 2)
					{
						if (itemHandGunList[myUIScrollView.curIndex].gunNum < itemHandGunList[myUIScrollView.curIndex].upgradeGunNum[itemHandGunList[myUIScrollView.curIndex].gunInfo.level])
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
				equipLabel.text = "EQUIP";
				upgradeBtn.gameObject.SetActiveRecursively(false);
			}
			if (itemHandGunList[myUIScrollView.curIndex].gunInfo.level < 2)
			{
				if (!upgradeBtn.gameObject.active)
				{
					if (itemHandGunList[myUIScrollView.curIndex].gunPrise != 0)
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
			if (itemHandGunList[myUIScrollView.curIndex].gunPrise != 0)
			{
				if (myUIScrollView.curIndex <= maxGunIndex + 1)
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
			equipLabel.text = "EQUIP";
			upgradeBtn.gameObject.SetActiveRecursively(false);
			unLockTipsLabel.gameObject.SetActiveRecursively(true);
			if (itemHandGunList[myUIScrollView.curIndex].unlockByLevel != 99)
			{
				unLockTipsLabel.text = "Unlock at area " + (itemHandGunList[myUIScrollView.curIndex].unlockByLevel + 1);
			}
			else
			{
				unLockTipsLabel.text = "Collecting Box to Unlock";
			}
		}
		gunName.text = itemHandGunList[myUIScrollView.curIndex].gunName;
		gunLevel.gameObject.SetActiveRecursively(true);
		gunLevel.text = "Lv" + (itemHandGunList[myUIScrollView.curIndex].gunInfo.level + 1);
		if (itemHandGunList[myUIScrollView.curIndex].upgradeGunNum[0] != 0)
		{
			NGUITools.SetActiveRecursively(upgradeLineRoot.gameObject, true);
			if (itemHandGunList[myUIScrollView.curIndex].gunInfo.level < 2)
			{
				ownedNum.text = string.Empty + itemHandGunList[myUIScrollView.curIndex].gunNum + "/" + itemHandGunList[myUIScrollView.curIndex].upgradeGunNum[itemHandGunList[myUIScrollView.curIndex].gunInfo.level];
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
			gunLevel.gameObject.SetActiveRecursively(false);
			NGUITools.SetActiveRecursively(upgradeLineRoot.gameObject, false);
		}
		if (preIndex != myUIScrollView.curIndex || upgradeFlag)
		{
			preIndex = myUIScrollView.curIndex;
			changeValFlag = true;
			preAttributeVal[0] = attributeVal[0];
			preAttributeVal[1] = attributeVal[1];
			preAttributeVal[2] = attributeVal[2];
			attributeVal[0] = (int)itemHandGunList[myUIScrollView.curIndex].damageLevelList[itemHandGunList[myUIScrollView.curIndex].gunInfo.level];
			attributeVal[1] = itemHandGunList[myUIScrollView.curIndex].bulletNumLevelList[itemHandGunList[myUIScrollView.curIndex].gunInfo.level];
			attributeVal[2] = (int)(1f / itemHandGunList[myUIScrollView.curIndex].shotIntervalLevelList[itemHandGunList[myUIScrollView.curIndex].gunInfo.level] / 3.5f * 100f);
			preAttributePercent[0] = attributePercent[0];
			preAttributePercent[1] = attributePercent[1];
			preAttributePercent[2] = attributePercent[2];
			attributePercent[0] = (float)attributeVal[0] / (float)attributeMaxVal[0];
			attributePercent[1] = (float)attributeVal[1] / (float)attributeMaxVal[1];
			attributePercent[2] = (float)attributeVal[2] / (float)attributeMaxVal[2];
			changeAttributeCountTime = 0f;
			preBulletNum = bulletNum;
			bulletNum = itemHandGunList[myUIScrollView.curIndex].gunNum;
			preBulletPercent = bulletPercent;
			if (itemHandGunList[myUIScrollView.curIndex].gunInfo.level < 2)
			{
				if (itemHandGunList[myUIScrollView.curIndex].upgradeGunNum[itemHandGunList[myUIScrollView.curIndex].gunInfo.level] != 0)
				{
					bulletPercent = (float)itemHandGunList[myUIScrollView.curIndex].gunNum / (float)itemHandGunList[myUIScrollView.curIndex].upgradeGunNum[itemHandGunList[myUIScrollView.curIndex].gunInfo.level];
				}
				else
				{
					bulletPercent = 0f;
				}
			}
			else
			{
				bulletPercent = 1f;
			}
			changeBulletValFlag = true;
			changeBulletCountTime = 0f;
			bulletLabel.text = string.Empty + itemHandGunList[myUIScrollView.curIndex].gunInfo.restBulletNum + "/" + itemHandGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum;
		}
		if (itemHandGunList[myUIScrollView.curIndex].gunPrise != 0)
		{
			gunCashPrise.text = string.Empty + itemHandGunList[myUIScrollView.curIndex].gunPrise;
			gunGoldPrise.text = string.Empty + itemHandGunList[myUIScrollView.curIndex].gunGoldPrise;
		}
		else
		{
			gunCashPrise.gameObject.SetActiveRecursively(false);
			gunGoldPrise.gameObject.SetActiveRecursively(false);
		}
		bulletPrise.text = string.Empty + (itemHandGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum - itemHandGunList[myUIScrollView.curIndex].gunInfo.restBulletNum) * itemHandGunList[myUIScrollView.curIndex].gunInfo.bulletPrise;
		CheckSignal();
	}

	public void CheckSignal()
	{
		if (itemHandGunList[myUIScrollView.curIndex].unlockFlag && itemHandGunList[myUIScrollView.curIndex].gunNum == 0)
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
		itemHandGunList[myUIScrollView.curIndex].equipedFlag = true;
		GlobalInf.handgunIndex = myUIScrollView.curIndex;
		StoreDateController.SetHandGunIndex(GlobalInf.handgunIndex);
		CopyHandGunInfo(itemHandGunList[myUIScrollView.curIndex].gunInfo);
		playAnimaFlag = true;
		if (myUIScrollView.curIndex == 0)
		{
			SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 2);
			SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex + 1);
			SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex);
		}
		else if (myUIScrollView.curIndex == itemHandGunList.Length - 1)
		{
			SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 2);
			SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex - 1);
			SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex);
		}
		else
		{
			SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 1);
			SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 1);
			SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex);
		}
		OnResetBtn();
	}

	public void OnClickCashPurchaseBtn(GameObject obj)
	{
		if (GlobalInf.cash - itemHandGunList[myUIScrollView.curIndex].gunPrise >= 0)
		{
			GlobalInf.cash -= itemHandGunList[myUIScrollView.curIndex].gunPrise;
			StoreDateController.SetCash();
			Platform.flurryEvent_onEquipmentHandGunPurchase(myUIScrollView.curIndex);
			GlobalInf.totalCashSpent += itemHandGunList[myUIScrollView.curIndex].gunPrise;
			StoreDateController.SetTotalCashSpent();
			if (!itemHandGunList[myUIScrollView.curIndex].buyFlag)
			{
				itemHandGunList[myUIScrollView.curIndex].buyFlag = true;
				ResetEquip();
				itemHandGunList[myUIScrollView.curIndex].equipedFlag = true;
				GlobalInf.handgunIndex = myUIScrollView.curIndex;
				StoreDateController.SetHandGunInfo(itemHandGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
				StoreDateController.SetHandGunIndex(GlobalInf.handgunIndex);
				CopyHandGunInfo(itemHandGunList[myUIScrollView.curIndex].gunInfo);
				playAnimaFlag = true;
				Platform.flurryEvent_onEquipmentHandGunGetPurchase(myUIScrollView.curIndex);
			}
			itemHandGunList[myUIScrollView.curIndex].gunNum++;
			if (myUIScrollView.curIndex == 0)
			{
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 2);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex + 1);
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex);
			}
			else if (myUIScrollView.curIndex == itemHandGunList.Length - 1)
			{
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 2);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex - 1);
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex);
			}
			else
			{
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 1);
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 1);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex);
			}
			StoreDateController.SetHandGunInfo(itemHandGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
			CheckHandGunSignal();
			OnResetBtn();
			gunBackLight.ResetToBeginning();
			gunBackLight.PlayForward();
			gunBackParticle.Play();
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.GET_ITEM);
			}
			MenuSenceController.instance.storeMenu.CheckSignal();
			preBulletNum = bulletNum;
			bulletNum = itemHandGunList[myUIScrollView.curIndex].gunNum;
			preBulletPercent = bulletPercent;
			bulletPercent = (float)itemHandGunList[myUIScrollView.curIndex].gunNum / (float)itemHandGunList[myUIScrollView.curIndex].upgradeGunNum[itemHandGunList[myUIScrollView.curIndex].gunInfo.level];
			changeBulletValFlag = true;
			changeBulletCountTime = 0f;
		}
		else
		{
			TopLineController.instance.OnClickCashBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.EQUIPMENT;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void OnClickGoldBuyBtn(GameObject btn)
	{
		if (GlobalInf.gold - itemHandGunList[myUIScrollView.curIndex].gunGoldPrise >= 0)
		{
			GlobalInf.gold -= itemHandGunList[myUIScrollView.curIndex].gunGoldPrise;
			StoreDateController.SetGold();
			Platform.flurryEvent_onEquipmentHandGunPurchase(myUIScrollView.curIndex);
			if (!itemHandGunList[myUIScrollView.curIndex].unlockFlag)
			{
				itemHandGunList[myUIScrollView.curIndex].unlockFlag = true;
			}
			if (!itemHandGunList[myUIScrollView.curIndex].buyFlag)
			{
				itemHandGunList[myUIScrollView.curIndex].buyFlag = true;
				ResetEquip();
				itemHandGunList[myUIScrollView.curIndex].equipedFlag = true;
				GlobalInf.handgunIndex = myUIScrollView.curIndex;
				StoreDateController.SetHandGunInfo(itemHandGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
				StoreDateController.SetHandGunIndex(GlobalInf.handgunIndex);
				CopyHandGunInfo(itemHandGunList[myUIScrollView.curIndex].gunInfo);
				playAnimaFlag = true;
				Platform.flurryEvent_onEquipmentHandGunGetPurchase(myUIScrollView.curIndex);
			}
			itemHandGunList[myUIScrollView.curIndex].gunNum++;
			if (myUIScrollView.curIndex == 0)
			{
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 2);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex + 1);
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex);
			}
			else if (myUIScrollView.curIndex == itemHandGunList.Length - 1)
			{
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 2);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex - 1);
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex);
			}
			else
			{
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 1);
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 1);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex);
			}
			StoreDateController.SetHandGunInfo(itemHandGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
			CheckHandGunSignal();
			OnResetBtn();
			gunBackLight.ResetToBeginning();
			gunBackLight.PlayForward();
			gunBackParticle.Play();
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.GET_ITEM);
			}
			MenuSenceController.instance.storeMenu.CheckSignal();
			preBulletNum = bulletNum;
			bulletNum = itemHandGunList[myUIScrollView.curIndex].gunNum;
			preBulletPercent = bulletPercent;
			bulletPercent = (float)itemHandGunList[myUIScrollView.curIndex].gunNum / (float)itemHandGunList[myUIScrollView.curIndex].upgradeGunNum[itemHandGunList[myUIScrollView.curIndex].gunInfo.level];
			changeBulletValFlag = true;
			changeBulletCountTime = 0f;
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
		for (int i = 0; i < itemHandGunList.Length; i++)
		{
			itemHandGunList[i].equipedFlag = false;
		}
	}

	public void CopyHandGunInfo(GunsInfo info)
	{
		GlobalInf.handGunInfo.accuracy = info.accuracy;
		GlobalInf.handGunInfo.level = info.level;
		GlobalInf.handGunInfo.bulletNum = info.bulletNum;
		GlobalInf.handGunInfo.bulletSpeed = info.bulletSpeed;
		GlobalInf.handGunInfo.damage = info.damage;
		GlobalInf.handGunInfo.gunName = info.gunName;
		GlobalInf.handGunInfo.maxBulletNum = info.maxBulletNum;
		GlobalInf.handGunInfo.reloadTime = info.reloadTime;
		GlobalInf.handGunInfo.restBulletNum = info.restBulletNum;
		GlobalInf.handGunInfo.shotInterval = info.shotInterval;
		GlobalInf.handGunInfo.bulletPrise = info.bulletPrise;
	}
}
