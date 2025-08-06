using System;
using UnityEngine;

public class CharacterPageController : CharacterPageRoot
{
	public Material playerMat;

	public ClothItem[] clothItems;

	public UICenterOnChild centerOnChild;

	public bool initPosFlag;

	public GameObject centerObj;

	public UIScrollView scrollView;

	public UIEventListener[] clothBtn;

	public int[] buyFlag;

	public int[] clothPrise;

	public bool[] goldFlag;

	public UIEventListener buyBtn;

	public UISprite buySprite;

	public UILabel buyBtnLabel;

	public UISprite buyPic;

	public int curClothIndex;

	public Texture[] clothPic;

	public void InitClothData()
	{
		int[] clothBuyFlag = StoreDateController.GetClothBuyFlag();
		for (int i = 0; i < buyFlag.Length; i++)
		{
			buyFlag[i] = clothBuyFlag[i];
		}
	}

	private void Start()
	{
		for (int i = 0; i < clothBtn.Length; i++)
		{
			UIEventListener obj = clothBtn[i];
			obj.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(obj.onClick, new UIEventListener.VoidDelegate(OnClickClothBtn));
		}
		UIEventListener uIEventListener = buyBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickBuyBtn));
	}

	public void OnClickBuyBtn(GameObject btn)
	{
		if (buyFlag[curClothIndex] == 1)
		{
			GlobalInf.clothIndex = curClothIndex;
			StoreDateController.SetClothIndex(GlobalInf.clothIndex);
			ResetBtn(curClothIndex);
			AudioController.instance.play(AudioType.UPGRADE);
		}
		else if (!goldFlag[curClothIndex])
		{
			if (GlobalInf.cash >= clothPrise[curClothIndex])
			{
				GlobalInf.cash -= clothPrise[curClothIndex];
				StoreDateController.SetCash();
				GlobalInf.totalCashSpent += clothPrise[curClothIndex];
				StoreDateController.SetTotalCashSpent();
				buyFlag[curClothIndex] = 1;
				StoreDateController.SetClothBuyFlag(buyFlag);
				GlobalInf.clothIndex = curClothIndex;
				StoreDateController.SetClothIndex(GlobalInf.clothIndex);
				ResetBtn(curClothIndex);
				AudioController.instance.play(AudioType.GET_ITEM);
				Platform.flurryEvent_onEquipmentCarPurchase(GlobalInf.clothIndex);
			}
			else
			{
				TopLineController.instance.OnClickCashBtn(null);
				GlobalInf.chargeShowType = CHARGESHOWTYPE.CLOTH;
				Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
			}
		}
		else if (GlobalInf.gold >= clothPrise[curClothIndex])
		{
			GlobalInf.gold -= clothPrise[curClothIndex];
			StoreDateController.SetGold();
			buyFlag[curClothIndex] = 1;
			StoreDateController.SetClothBuyFlag(buyFlag);
			GlobalInf.clothIndex = curClothIndex;
			StoreDateController.SetClothIndex(GlobalInf.clothIndex);
			ResetBtn(curClothIndex);
			AudioController.instance.play(AudioType.GET_ITEM);
			Platform.flurryEvent_onEquipmentClothPurchase(GlobalInf.clothIndex);
		}
		else
		{
			TopLineController.instance.OnClickGoldBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.CLOTH;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	public void OnClickClothBtn(GameObject btn)
	{
		for (int i = 0; i < clothBtn.Length; i++)
		{
			if (btn == clothBtn[i].gameObject)
			{
				SetCloth(i);
				break;
			}
		}
	}

	public void ResetBtn(int i)
	{
		if (buyFlag[i] == 1)
		{
			buyPic.gameObject.SetActiveRecursively(false);
			if (GlobalInf.clothIndex == i)
			{
				buySprite.spriteName = "huidu";
				buyBtnLabel.text = "EQUIPED";
				buyBtnLabel.transform.localPosition = Vector3.zero;
			}
			else
			{
				buySprite.spriteName = "equip";
				buyBtnLabel.text = "EQUIP";
				buyBtnLabel.transform.localPosition = Vector3.zero;
			}
		}
		else
		{
			buyPic.gameObject.SetActiveRecursively(true);
			if (goldFlag[i])
			{
				buySprite.spriteName = "upgrate";
				buyPic.spriteName = "G";
			}
			else
			{
				buySprite.spriteName = "play";
				buyPic.spriteName = "$";
			}
			buyBtnLabel.text = string.Empty + clothPrise[i];
			buyBtnLabel.transform.localPosition = new Vector3(GlobalDefine.screenWidthFit * 15f, 0f, 0f);
		}
	}

	private new void OnEnable()
	{
		base.OnEnable();
		OnInit();
	}

	private new void OnDisable()
	{
		base.OnDisable();
		if (GlobalInf.clothIndex != curClothIndex)
		{
			playerMat.mainTexture = clothPic[GlobalInf.clothIndex];
		}
	}

	private void Update()
	{
		if (initPosFlag)
		{
			initPosFlag = false;
			centerOnChild.CenterOn(clothItems[GlobalInf.clothIndex].transform);
			SetCloth(GlobalInf.clothIndex);
		}
	}

	public void OnInit()
	{
		initPosFlag = true;
	}

	public void SetCloth(GameObject obj)
	{
		for (int i = 0; i < clothItems.Length; i++)
		{
			if (obj == clothItems[i].gameObject)
			{
				SetCloth(i);
				break;
			}
		}
	}

	public void SetCloth(int index)
	{
		curClothIndex = index;
		playerMat.mainTexture = clothPic[index];
		ResetBtn(index);
	}
}
