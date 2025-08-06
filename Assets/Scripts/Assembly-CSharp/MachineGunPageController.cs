using System;
using UnityEngine;

public class MachineGunPageController : WeaponPageController
{
	public ItemGunInfo[] itemMachineGunList;

	public UIPanel pan;

	public string[] itemPicName;

	public Vector2[] bulletLinePos;

	public UIAtlas picAtlas;

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

	public UILabel unLockTips;

	public GameObject upgradeLineRoot;

	public GameObject upgradePic;

	private int preIndex;

	private bool afterEnableFlag;

	private bool upgradeFlag;

	private int bulletTmepPrise;

	private float changeAttributeCountTime;

	public int maxGunIndex;

	public bool changeValFlag;

	private void Awake()
	{
	}

	private void OnEnable()
	{
		myUIScrollView.curIndex = GlobalInf.machineGunIndex;
		if (myUIScrollView.curIndex == -1)
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
		else if (myUIScrollView.curIndex == itemMachineGunList.Length - 1)
		{
			SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 2);
			SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex - 1);
			SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex);
		}
		else
		{
			SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 1);
			SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex);
			SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 1);
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
		myUIScrollView.maxIndex = itemMachineGunList.Length - 1;
		StoreDateController.GetMachineGunInfoList(itemMachineGunList);
		ResetEquip();
		if (GlobalInf.machineGunIndex != -1)
		{
			itemMachineGunList[GlobalInf.machineGunIndex].equipedFlag = true;
		}
		cashBuyBtn.onClick = OnClickCashPurchaseBtn;
		goldBuyBtn.onClick = OnClickGoldBuyBtn;
		equipBtn.onClick = OnClickEquipBtn;
		ammoBtn.onClick = OnClickAmmoBtn;
		upgradeBtn.onClick = OnClickUpgradeBtn;
		myUIScrollView.Init();
		CheckMachineGunSignal();
	}

	public void CheckMachineGunSignal()
	{
		MenuSenceController.instance.machineGunBtnSignal = false;
		MenuSenceController.instance.machineGunBtnNewSignal = false;
		for (int i = 0; i <= myUIScrollView.maxIndex - 1; i++)
		{
			if (itemMachineGunList[i].gunInfo.level < 2 && itemMachineGunList[i].gunNum >= itemMachineGunList[i].upgradeGunNum[itemMachineGunList[i].gunInfo.level])
			{
				MenuSenceController.instance.machineGunBtnSignal = true;
				break;
			}
			if (itemMachineGunList[i].unlockFlag && itemMachineGunList[i].gunNum == 0)
			{
				MenuSenceController.instance.machineGunBtnNewSignal = true;
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
		if (itemMachineGunList[index].unlockFlag)
		{
			if (itemMachineGunList[index].buyFlag)
			{
				if (itemMachineGunList[index].equipedFlag)
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
		itemPage.itemPic.width = (int)((float)picAtlas.GetSprite(itemPicName[index]).width * GlobalDefine.screenWidthFit);
		itemPage.itemPic.height = picAtlas.GetSprite(itemPicName[index]).height;
	}

	public void OnClickUpgradeBtn(GameObject obj)
	{
		itemMachineGunList[myUIScrollView.curIndex].gunNum = itemMachineGunList[myUIScrollView.curIndex].gunNum - itemMachineGunList[myUIScrollView.curIndex].upgradeGunNum[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level] + 1;
		if (itemMachineGunList[myUIScrollView.curIndex].gunNum <= 0)
		{
			itemMachineGunList[myUIScrollView.curIndex].gunNum = 1;
		}
		itemMachineGunList[myUIScrollView.curIndex].gunInfo.level++;
		upgradeFlag = true;
		OnResetBtn();
		upgradeFlag = false;
		StoreDateController.SetMachineGunInfo(itemMachineGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
		gunBackLight.ResetToBeginning();
		gunBackLight.PlayForward();
		gunBackParticle.Play();
		GlobalInf.dailyUpgradeItemNum++;
		if (AudioController.instance != null)
		{
			AudioController.instance.play(AudioType.UPGRADE);
		}
		CheckMachineGunSignal();
		MenuSenceController.instance.storeMenu.CheckSignal();
		Platform.flurryEvent_onEquipmentMachineGunUpgrade(myUIScrollView.curIndex, itemMachineGunList[myUIScrollView.curIndex].gunInfo.level);
		if (itemMachineGunList[myUIScrollView.curIndex].gunInfo.level < 2)
		{
			preBulletNum = bulletNum;
			bulletNum = itemMachineGunList[myUIScrollView.curIndex].gunNum;
			preBulletPercent = bulletPercent;
			bulletPercent = (float)itemMachineGunList[myUIScrollView.curIndex].gunNum / (float)itemMachineGunList[myUIScrollView.curIndex].upgradeGunNum[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level];
			changeBulletValFlag = true;
			changeBulletCountTime = 0f;
		}
	}

	public void OnClickAmmoBtn(GameObject obj)
	{
		bulletTmepPrise = (itemMachineGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum - itemMachineGunList[myUIScrollView.curIndex].gunInfo.restBulletNum) * itemMachineGunList[myUIScrollView.curIndex].gunInfo.bulletPrise;
		if (GlobalInf.cash > bulletTmepPrise)
		{
			itemMachineGunList[myUIScrollView.curIndex].gunInfo.restBulletNum = itemMachineGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum;
			StoreDateController.SetMachineGunInfo(itemMachineGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
			CopyMachineGunInfo(itemMachineGunList[myUIScrollView.curIndex].gunInfo);
			GlobalInf.cash -= bulletTmepPrise;
			StoreDateController.SetCash();
			GlobalInf.totalCashSpent += bulletTmepPrise;
			StoreDateController.SetTotalCashSpent();
			bulletLabel.text = string.Empty + itemMachineGunList[myUIScrollView.curIndex].gunInfo.restBulletNum + "/" + itemMachineGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum;
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
		for (int num = itemMachineGunList.Length - 1; num >= 0; num--)
		{
			if (itemMachineGunList[num].gunNum > 0)
			{
				maxGunIndex = num;
				break;
			}
		}
		if (itemMachineGunList[myUIScrollView.curIndex].unlockFlag)
		{
			unLockTips.gameObject.SetActiveRecursively(false);
			if (itemMachineGunList[myUIScrollView.curIndex].buyFlag)
			{
				ammoUIBtn.gameObject.SetActiveRecursively(true);
				equipUIBtn.gameObject.SetActiveRecursively(true);
				if (itemMachineGunList[myUIScrollView.curIndex].gunInfo.restBulletNum < itemMachineGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum)
				{
					ammoUIBtn.isEnabled = true;
				}
				else
				{
					ammoUIBtn.isEnabled = false;
				}
				if (itemMachineGunList[myUIScrollView.curIndex].equipedFlag)
				{
					equipUIBtn.isEnabled = false;
					equipLabel.text = "EQUIPPED";
				}
				else
				{
					equipUIBtn.isEnabled = true;
					equipLabel.text = "EQUIP";
				}
				if (itemMachineGunList[myUIScrollView.curIndex].upgradeGunNum[0] != 0)
				{
					if (itemMachineGunList[myUIScrollView.curIndex].gunInfo.level < 2)
					{
						if (itemMachineGunList[myUIScrollView.curIndex].gunNum < itemMachineGunList[myUIScrollView.curIndex].upgradeGunNum[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level])
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
			if (itemMachineGunList[myUIScrollView.curIndex].gunInfo.level < 2)
			{
				if (!upgradeBtn.gameObject.active)
				{
					if (itemMachineGunList[myUIScrollView.curIndex].gunPrise != 0)
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
			if (itemMachineGunList[myUIScrollView.curIndex].gunPrise != 0)
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
			unLockTips.gameObject.SetActiveRecursively(true);
			if (itemMachineGunList[myUIScrollView.curIndex].unlockByLevel != 99)
			{
				unLockTips.text = "Unlock at area " + (itemMachineGunList[myUIScrollView.curIndex].unlockByLevel + 1);
			}
			else
			{
				unLockTips.text = "Collecting Box to Unlock";
			}
		}
		gunName.text = itemMachineGunList[myUIScrollView.curIndex].gunName;
		gunLevel.text = "Lv" + (itemMachineGunList[myUIScrollView.curIndex].gunInfo.level + 1);
		if (itemMachineGunList[myUIScrollView.curIndex].upgradeGunNum[0] != 0)
		{
			NGUITools.SetActiveRecursively(upgradeLineRoot, true);
			if (itemMachineGunList[myUIScrollView.curIndex].gunInfo.level < 2)
			{
				ownedNum.text = string.Empty + itemMachineGunList[myUIScrollView.curIndex].gunNum + "/" + itemMachineGunList[myUIScrollView.curIndex].upgradeGunNum[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level];
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
			NGUITools.SetActiveRecursively(upgradeLineRoot, false);
		}
		if (preIndex != myUIScrollView.curIndex || upgradeFlag)
		{
			preIndex = myUIScrollView.curIndex;
			changeValFlag = true;
			preAttributeVal[0] = attributeVal[0];
			preAttributeVal[1] = attributeVal[1];
			preAttributeVal[2] = attributeVal[2];
			attributeVal[0] = (int)itemMachineGunList[myUIScrollView.curIndex].damageLevelList[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level];
			attributeVal[1] = itemMachineGunList[myUIScrollView.curIndex].bulletNumLevelList[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level];
			attributeVal[2] = (int)(1f / itemMachineGunList[myUIScrollView.curIndex].shotIntervalLevelList[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level] / 3.5f * 100f);
			preAttributePercent[0] = attributePercent[0];
			preAttributePercent[1] = attributePercent[1];
			preAttributePercent[2] = attributePercent[2];
			attributePercent[0] = (float)attributeVal[0] / (float)attributeMaxVal[0];
			attributePercent[1] = (float)attributeVal[1] / (float)attributeMaxVal[1];
			attributePercent[2] = (float)attributeVal[2] / (float)attributeMaxVal[2];
			changeAttributeCountTime = 0f;
			preBulletNum = bulletNum;
			bulletNum = itemMachineGunList[myUIScrollView.curIndex].gunNum;
			preBulletPercent = bulletPercent;
			if (itemMachineGunList[myUIScrollView.curIndex].gunInfo.level < 2)
			{
				if (itemMachineGunList[myUIScrollView.curIndex].upgradeGunNum[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level] != 0)
				{
					bulletPercent = (float)itemMachineGunList[myUIScrollView.curIndex].gunNum / (float)itemMachineGunList[myUIScrollView.curIndex].upgradeGunNum[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level];
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
			bulletLabel.text = string.Empty + itemMachineGunList[myUIScrollView.curIndex].gunInfo.restBulletNum + "/" + itemMachineGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum;
		}
		if (itemMachineGunList[myUIScrollView.curIndex].gunPrise != 0)
		{
			gunCashPrise.text = string.Empty + itemMachineGunList[myUIScrollView.curIndex].gunPrise;
			gunGoldPrise.text = string.Empty + itemMachineGunList[myUIScrollView.curIndex].gunGoldPrise;
		}
		else
		{
			gunCashPrise.gameObject.SetActiveRecursively(false);
			gunGoldPrise.gameObject.SetActiveRecursively(false);
		}
		bulletPrise.text = string.Empty + (itemMachineGunList[myUIScrollView.curIndex].gunInfo.maxBulletNum - itemMachineGunList[myUIScrollView.curIndex].gunInfo.restBulletNum) * itemMachineGunList[myUIScrollView.curIndex].gunInfo.bulletPrise;
		CheckSignal();
	}

	public void CheckSignal()
	{
		if (itemMachineGunList[myUIScrollView.curIndex].unlockFlag && itemMachineGunList[myUIScrollView.curIndex].gunNum == 0)
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
		itemMachineGunList[myUIScrollView.curIndex].equipedFlag = true;
		GlobalInf.machineGunIndex = myUIScrollView.curIndex;
		StoreDateController.SetMachineGunIndex(GlobalInf.machineGunIndex);
		CopyMachineGunInfo(itemMachineGunList[myUIScrollView.curIndex].gunInfo);
		playAnimaFlag = true;
		if (myUIScrollView.curIndex == 0)
		{
			SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 2);
			SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex + 1);
			SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex);
		}
		else if (myUIScrollView.curIndex == itemMachineGunList.Length - 1)
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
		if (GlobalInf.cash - itemMachineGunList[myUIScrollView.curIndex].gunPrise >= 0)
		{
			GlobalInf.cash -= itemMachineGunList[myUIScrollView.curIndex].gunPrise;
			StoreDateController.SetCash();
			GlobalInf.totalCashSpent += itemMachineGunList[myUIScrollView.curIndex].gunPrise;
			StoreDateController.SetTotalCashSpent();
			Platform.flurryEvent_onEquipmentMachineGunPurchase(myUIScrollView.curIndex);
			if (!itemMachineGunList[myUIScrollView.curIndex].buyFlag)
			{
				itemMachineGunList[myUIScrollView.curIndex].buyFlag = true;
				ResetEquip();
				itemMachineGunList[myUIScrollView.curIndex].equipedFlag = true;
				StoreDateController.SetMachineGunInfo(itemMachineGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
				GlobalInf.machineGunIndex = myUIScrollView.curIndex;
				StoreDateController.SetMachineGunIndex(GlobalInf.machineGunIndex);
				CopyMachineGunInfo(itemMachineGunList[myUIScrollView.curIndex].gunInfo);
				playAnimaFlag = true;
				Platform.flurryEvent_onEquipmentMachineGunGetPurchase(myUIScrollView.curIndex);
			}
			itemMachineGunList[myUIScrollView.curIndex].gunNum++;
			if (myUIScrollView.curIndex == 0)
			{
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 2);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex + 1);
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex);
			}
			else if (myUIScrollView.curIndex == itemMachineGunList.Length - 1)
			{
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 2);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex - 1);
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex);
			}
			else
			{
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 1);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex);
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 1);
			}
			StoreDateController.SetMachineGunInfo(itemMachineGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
			CheckMachineGunSignal();
			MenuSenceController.instance.storeMenu.CheckSignal();
			OnResetBtn();
			gunBackLight.ResetToBeginning();
			gunBackLight.PlayForward();
			gunBackParticle.Play();
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.GET_ITEM);
			}
			preBulletNum = bulletNum;
			bulletNum = itemMachineGunList[myUIScrollView.curIndex].gunNum;
			preBulletPercent = bulletPercent;
			bulletPercent = (float)itemMachineGunList[myUIScrollView.curIndex].gunNum / (float)itemMachineGunList[myUIScrollView.curIndex].upgradeGunNum[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level];
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
		if (GlobalInf.gold - itemMachineGunList[myUIScrollView.curIndex].gunGoldPrise >= 0)
		{
			GlobalInf.gold -= itemMachineGunList[myUIScrollView.curIndex].gunGoldPrise;
			StoreDateController.SetGold();
			Platform.flurryEvent_onEquipmentHandGunPurchase(myUIScrollView.curIndex);
			if (!itemMachineGunList[myUIScrollView.curIndex].unlockFlag)
			{
				itemMachineGunList[myUIScrollView.curIndex].unlockFlag = true;
			}
			if (!itemMachineGunList[myUIScrollView.curIndex].buyFlag)
			{
				itemMachineGunList[myUIScrollView.curIndex].buyFlag = true;
				ResetEquip();
				itemMachineGunList[myUIScrollView.curIndex].equipedFlag = true;
				StoreDateController.SetMachineGunInfo(itemMachineGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
				GlobalInf.machineGunIndex = myUIScrollView.curIndex;
				StoreDateController.SetMachineGunIndex(GlobalInf.machineGunIndex);
				CopyMachineGunInfo(itemMachineGunList[myUIScrollView.curIndex].gunInfo);
				playAnimaFlag = true;
				Platform.flurryEvent_onEquipmentMachineGunGetPurchase(myUIScrollView.curIndex);
			}
			itemMachineGunList[myUIScrollView.curIndex].gunNum++;
			if (myUIScrollView.curIndex == 0)
			{
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 2);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex + 1);
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex);
			}
			else if (myUIScrollView.curIndex == itemMachineGunList.Length - 1)
			{
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 2);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex - 1);
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex);
			}
			else
			{
				SetPage(myUIScrollView.leftItemPage, myUIScrollView.curIndex - 1);
				SetPage(myUIScrollView.middleItemPage, myUIScrollView.curIndex);
				SetPage(myUIScrollView.rightItemPage, myUIScrollView.curIndex + 1);
			}
			StoreDateController.SetMachineGunInfo(itemMachineGunList[myUIScrollView.curIndex], myUIScrollView.curIndex);
			CheckMachineGunSignal();
			MenuSenceController.instance.storeMenu.CheckSignal();
			OnResetBtn();
			gunBackLight.ResetToBeginning();
			gunBackLight.PlayForward();
			gunBackParticle.Play();
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.GET_ITEM);
			}
			preBulletNum = bulletNum;
			bulletNum = itemMachineGunList[myUIScrollView.curIndex].gunNum;
			preBulletPercent = bulletPercent;
			bulletPercent = (float)itemMachineGunList[myUIScrollView.curIndex].gunNum / (float)itemMachineGunList[myUIScrollView.curIndex].upgradeGunNum[itemMachineGunList[myUIScrollView.curIndex].gunInfo.level];
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
		for (int i = 0; i < itemMachineGunList.Length; i++)
		{
			itemMachineGunList[i].equipedFlag = false;
		}
	}

	public void CopyMachineGunInfo(GunsInfo info)
	{
		GlobalInf.machineGunInfo.accuracy = info.accuracy;
		GlobalInf.machineGunInfo.level = info.level;
		GlobalInf.machineGunInfo.bulletNum = info.bulletNum;
		GlobalInf.machineGunInfo.bulletSpeed = info.bulletSpeed;
		GlobalInf.machineGunInfo.damage = info.damage;
		GlobalInf.machineGunInfo.gunName = info.gunName;
		GlobalInf.machineGunInfo.maxBulletNum = info.maxBulletNum;
		GlobalInf.machineGunInfo.reloadTime = info.reloadTime;
		GlobalInf.machineGunInfo.restBulletNum = info.restBulletNum;
		GlobalInf.machineGunInfo.shotInterval = info.shotInterval;
		GlobalInf.machineGunInfo.bulletPrise = info.bulletPrise;
	}
}
