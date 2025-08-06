using UnityEngine;

public class TaskBoardController : MonoBehaviour
{
	public UISprite minPic;

	public UISprite secPic0;

	public UISprite secPic1;

	public UISprite topBottom;

	public UILabel bottomLabel;

	public int curSec0;

	public int curSec1;

	public int preMin;

	public int preSec0;

	public int preSec1;

	public int preSec;

	public bool minZeroFlag;

	public GameObject baskMask;

	public bool beforeGameFlag;

	public int beforeCount;

	public TweenAlpha[] tweenAlph;

	public TweenScale[] tweenScale;

	public void ResetTaskBoardUI()
	{
		base.gameObject.SetActiveRecursively(true);
		if (beforeGameFlag)
		{
			DisableNum();
			SetBeforeGameCount(beforeCount);
		}
		else
		{
			baskMask.SetActiveRecursively(false);
			DisableNum();
		}
	}

	public void SetMin(int curMin)
	{
		if (curMin == 0)
		{
			minZeroFlag = true;
		}
		else
		{
			minZeroFlag = false;
		}
		if (curMin != preMin)
		{
			minPic.spriteName = string.Empty + curMin;
			preMin = curMin;
			minPic.height = minPic.atlas.GetSprite(minPic.spriteName).height;
			minPic.width = (int)((float)minPic.atlas.GetSprite(minPic.spriteName).width * GlobalDefine.screenWidthFit);
		}
	}

	public void SetSec(int curSec)
	{
		if (preSec != curSec)
		{
			if (minZeroFlag && curSec <= 10 && GameController.instance.curGameMode != GAMEMODE.SURVIVAL && !beforeGameFlag)
			{
				AudioController.instance.play(AudioType.COUNT_DOWN);
			}
			preSec = curSec;
			curSec0 = curSec / 10;
			curSec1 = curSec % 10;
			if (curSec0 != preSec0)
			{
				secPic0.spriteName = string.Empty + curSec0;
				preSec0 = curSec0;
				secPic0.height = secPic0.atlas.GetSprite(secPic0.spriteName).height;
				secPic0.width = (int)((float)secPic0.atlas.GetSprite(secPic0.spriteName).width * GlobalDefine.screenWidthFit);
			}
			if (curSec1 != preSec1)
			{
				secPic1.spriteName = string.Empty + curSec1;
				preSec1 = curSec1;
				secPic1.height = secPic1.atlas.GetSprite(secPic1.spriteName).height;
				secPic1.width = (int)((float)secPic1.atlas.GetSprite(secPic1.spriteName).width * GlobalDefine.screenWidthFit);
			}
		}
	}

	public void SetBeforeGameCount(int num)
	{
		if (num < 3 && num != beforeCount && num >= 0)
		{
			beforeCount = num;
			DisableNum();
			tweenAlph[num].gameObject.SetActiveRecursively(true);
			tweenAlph[num].ResetToBeginning();
			tweenAlph[num].PlayForward();
			tweenScale[num].ResetToBeginning();
			tweenScale[num].PlayForward();
			AudioController.instance.play(AudioType.COUNT_DOWN);
		}
	}

	public void DisableNum()
	{
		for (int i = 0; i < tweenAlph.Length; i++)
		{
			tweenAlph[i].gameObject.SetActiveRecursively(false);
		}
	}

	public void SetTime(float curTime)
	{
		SetMin((int)curTime / 60);
		SetSec((int)curTime % 60);
	}
}
