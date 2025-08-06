using System;
using UnityEngine;

public class WeaponPageController : MonoBehaviour
{
	public MENUUISTATE uiType;

	public GameObject weaponPageObj;

	public UIEventListener leftBtn;

	public UIEventListener rightBtn;

	public TweenAlpha rightBtnAnima;

	public TweenAlpha leftBtnAnima;

	public UISprite leftBtnSprite;

	public UISprite rightBtnSprite;

	public MyUIScrollView myUIScrollView;

	public MyPackPageScrollView myPackPageScrollView;

	public bool playEnterAnimaFlag;

	public bool signalFlag;

	private void OnEnable()
	{
		weaponPageObj.SetActiveRecursively(true);
		if (leftBtn != null)
		{
			leftBtn.gameObject.GetComponent<UIAnchor>().Run();
		}
		if (rightBtn != null)
		{
			rightBtn.gameObject.GetComponent<UIAnchor>().Run();
		}
		if (myUIScrollView.curIndex == 0)
		{
			leftBtn.gameObject.SetActiveRecursively(false);
		}
		else
		{
			rightBtnAnima.PlayForward();
		}
		if (myUIScrollView.curIndex == myUIScrollView.maxIndex)
		{
			rightBtn.gameObject.SetActiveRecursively(false);
			rightBtnAnima.enabled = false;
			rightBtnSprite.alpha = 1f;
		}
		else
		{
			leftBtnAnima.enabled = false;
			leftBtnSprite.alpha = 1f;
		}
	}

	private void OnDisable()
	{
		if (weaponPageObj != null)
		{
			weaponPageObj.SetActiveRecursively(false);
		}
	}

	public virtual void Init()
	{
		UIEventListener uIEventListener = leftBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickLeftBtn));
		UIEventListener uIEventListener2 = rightBtn;
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnClickRightBtn));
	}

	public void OnClickLeftBtn(GameObject btn)
	{
		if (myUIScrollView != null)
		{
			myUIScrollView.OnChangeLeft();
			if (myUIScrollView.curIndex == 1)
			{
				leftBtn.gameObject.SetActiveRecursively(false);
				rightBtnAnima.PlayForward();
			}
			else if (myUIScrollView.curIndex == myUIScrollView.maxIndex)
			{
				rightBtn.gameObject.SetActiveRecursively(true);
				leftBtnAnima.enabled = false;
				rightBtnAnima.enabled = false;
				leftBtnSprite.alpha = 1f;
				rightBtnSprite.alpha = 1f;
			}
		}
		else
		{
			myPackPageScrollView.OnChangeLeft();
			if (myPackPageScrollView.curIndex == 1)
			{
				leftBtn.gameObject.SetActiveRecursively(false);
				rightBtnAnima.PlayForward();
			}
			else if (myPackPageScrollView.curIndex == 2)
			{
				rightBtn.gameObject.SetActiveRecursively(true);
				leftBtnAnima.enabled = false;
				rightBtnAnima.enabled = false;
				leftBtnSprite.alpha = 1f;
				rightBtnSprite.alpha = 1f;
			}
		}
	}

	public void OnClickRightBtn(GameObject btn)
	{
		if (myUIScrollView != null)
		{
			myUIScrollView.OnChangeRight();
			if (myUIScrollView.curIndex == myUIScrollView.maxIndex - 1)
			{
				rightBtn.gameObject.SetActiveRecursively(false);
				leftBtnAnima.PlayForward();
			}
			else if (myUIScrollView.curIndex == 0)
			{
				leftBtn.gameObject.SetActiveRecursively(true);
				leftBtnAnima.enabled = false;
				rightBtnAnima.enabled = false;
				leftBtnSprite.alpha = 1f;
				rightBtnSprite.alpha = 1f;
			}
		}
		else
		{
			myPackPageScrollView.OnChangeRight();
			if (myPackPageScrollView.curIndex == 1)
			{
				rightBtn.gameObject.SetActiveRecursively(false);
				leftBtnAnima.PlayForward();
			}
			else if (myPackPageScrollView.curIndex == 0)
			{
				leftBtn.gameObject.SetActiveRecursively(true);
				leftBtnAnima.enabled = false;
				rightBtnAnima.enabled = false;
				leftBtnSprite.alpha = 1f;
				rightBtnSprite.alpha = 1f;
			}
		}
	}
}
