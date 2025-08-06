using System;
using UnityEngine;

public class MenuSenceVideoCheckUI : MonoBehaviour
{
	public UIEventListener okBtn;

	public UISprite okBtnSprite;

	public UISprite pic;

	public UILabel number;

	public bool cashFlag;

	private void Start()
	{
		UIEventListener uIEventListener = okBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickOKBtn));
		GlobalInf.videoAdsFlag = true;
	}

	private void Update()
	{
	}

	public void Reset(bool flag)
	{
		cashFlag = flag;
		if (!cashFlag)
		{
			if (GlobalInf.goldVideoCount < 5)
			{
				pic.spriteName = "jinbi1";
				pic.width = 108;
				pic.height = 60;
				number.text = "X1";
				okBtnSprite.spriteName = "upgrate";
			}
			else
			{
				pic.spriteName = "meiyuan2";
				pic.width = 108;
				pic.height = 66;
				number.text = "X100";
				okBtnSprite.spriteName = "play";
			}
		}
		else
		{
			pic.spriteName = "meiyuan2";
			pic.width = 108;
			pic.height = 66;
			number.text = "X100";
			okBtnSprite.spriteName = "play";
		}
	}

	public void OnClickOKBtn(GameObject btn)
	{
		GlobalInf.videoAdsFlag = false;
		if (!cashFlag)
		{
			if (GlobalInf.goldVideoCount < 5)
			{
				GlobalInf.gold++;
				StoreDateController.SetGold();
			}
			else
			{
				GlobalInf.cash += 100;
				StoreDateController.SetCash();
			}
			GlobalInf.goldVideoCount++;
			StoreDateController.SetGoldVideoCount();
		}
		else
		{
			GlobalInf.cash += 100;
			StoreDateController.SetCash();
		}
		Platform.CheckAdsCanShow();
		if (!GlobalInf.cashVideoFlag)
		{
			TopLineController.instance.CheckAdsBtn();
		}
		else
		{
			TopLineController.instance.CheckCashAdsBtn();
		}
		base.gameObject.SetActiveRecursively(false);
		UnityEngine.Object.Destroy(base.gameObject);
		Resources.UnloadUnusedAssets();
	}
}
