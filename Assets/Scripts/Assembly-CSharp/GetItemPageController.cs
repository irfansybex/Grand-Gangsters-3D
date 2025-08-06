using System;
using UnityEngine;

public class GetItemPageController : MonoBehaviour
{
	public UISprite itemPic;

	public ITEMTYPE type;

	public UIEventListener claimBtn;

	public TweenScale tweenScale;

	public bool collectPageFlag;

	public UILabel title;

	public UILabel numLabel;

	private void Start()
	{
		UIEventListener uIEventListener = claimBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickClaimBtn));
	}

	public void InitBuyItemPage(string spriteName)
	{
		collectPageFlag = false;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.GetComponent<UIAnchor>() != null)
			{
				base.transform.GetChild(i).gameObject.GetComponent<UIAnchor>().Run();
			}
		}
		tweenScale.from = new Vector3(0f, 0f, 0f);
		tweenScale.PlayForward();
		itemPic.spriteName = spriteName;
		itemPic.width = (int)((float)itemPic.atlas.GetSprite(spriteName).width * GlobalDefine.screenWidthFit);
		itemPic.height = itemPic.atlas.GetSprite(spriteName).height;
	}

	public void InitPage(ITEMTYPE t)
	{
		collectPageFlag = true;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.GetComponent<UIAnchor>() != null)
			{
				base.transform.GetChild(i).gameObject.GetComponent<UIAnchor>().Run();
			}
		}
		tweenScale.from = new Vector3(0f, 0f, 0f);
		tweenScale.PlayForward();
		numLabel.gameObject.SetActiveRecursively(false);
		type = t;
		switch (type)
		{
		case ITEMTYPE.CAR:
			itemPic.spriteName = "mashaladi";
			itemPic.width = (int)(234f * GlobalDefine.screenWidthFit);
			itemPic.height = 126;
			title.text = "Car";
			break;
		case ITEMTYPE.HANDGUN:
			itemPic.spriteName = "pistol_boss";
			itemPic.width = (int)(167f * GlobalDefine.screenWidthFit);
			itemPic.height = 102;
			title.text = "HandGun";
			break;
		case ITEMTYPE.MACHINEGUN:
			itemPic.spriteName = "rifle-MR37";
			itemPic.width = (int)(413f * GlobalDefine.screenWidthFit);
			itemPic.height = 139;
			title.text = "MachineGun";
			break;
		case ITEMTYPE.HEALTHKIT:
			itemPic.spriteName = "doctor";
			itemPic.width = (int)((float)itemPic.atlas.GetSprite("doctor").width * GlobalDefine.screenWidthFit);
			itemPic.height = itemPic.atlas.GetSprite("doctor").height;
			title.text = "HealthKit";
			numLabel.gameObject.SetActiveRecursively(true);
			numLabel.text = "X3";
			break;
		case ITEMTYPE.TOOLKIT:
			itemPic.spriteName = "car-tool";
			itemPic.width = (int)((float)itemPic.atlas.GetSprite("car-tool").width * GlobalDefine.screenWidthFit);
			itemPic.height = itemPic.atlas.GetSprite("car-tool").height;
			title.text = "ToolKit";
			numLabel.gameObject.SetActiveRecursively(true);
			numLabel.text = "X3";
			break;
		}
	}

	public void OnClickClaimBtn(GameObject btn)
	{
		tweenScale.PlayReverse();
		Invoke("DelayDisable", 0.6f);
		MenuSenceBackBtnCtl.instance.PopMenuUIState();
		if (collectPageFlag)
		{
			if (type == ITEMTYPE.HANDGUN)
			{
				MenuSenceBackBtnCtl.instance.CollectingPageBack();
				MenuSenceController.instance.storeMenu.prePage = MENUUISTATE.HANDGUNPAGE;
				MenuSenceController.instance.startMenu.OnClickStoreBtn(null);
			}
			else if (type == ITEMTYPE.MACHINEGUN)
			{
				MenuSenceBackBtnCtl.instance.CollectingPageBack();
				MenuSenceController.instance.storeMenu.prePage = MENUUISTATE.MACHINEGUNPAGE;
				MenuSenceController.instance.startMenu.OnClickStoreBtn(null);
			}
			else if (type == ITEMTYPE.CAR)
			{
				MenuSenceBackBtnCtl.instance.CollectingPageBack();
				MenuSenceController.instance.storeMenu.prePage = MENUUISTATE.VECHICLESPAGE;
				MenuSenceController.instance.startMenu.OnClickStoreBtn(null);
			}
			else if (type == ITEMTYPE.HEALTHKIT)
			{
				GlobalInf.healthKitNum += 3;
				StoreDateController.SetHealthKitNum(GlobalInf.healthKitNum);
			}
			else if (type == ITEMTYPE.TOOLKIT)
			{
				GlobalInf.toolKitNum += 3;
				StoreDateController.SetToolKitNum(GlobalInf.toolKitNum);
			}
		}
	}

	public void DelayDisable()
	{
		base.gameObject.SetActiveRecursively(false);
	}
}
