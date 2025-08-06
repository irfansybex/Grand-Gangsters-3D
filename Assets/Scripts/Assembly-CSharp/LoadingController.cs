using System;
using UnityEngine;

public class LoadingController : MonoBehaviour
{
	private bool loadFlag;

	private AsyncOperation async;

	private int progress;

	//public GUIText progressText;

	public UISprite loadingLine;

	public UISprite loadingLineBack;

	public int p;

	private void Start()
	{
		Time.timeScale = 1f;
		loadFlag = false;
		GC.Collect();
		Resources.UnloadUnusedAssets();
		if (!GlobalInf.startInitGameFlag)
		{
			GlobalInf.startInitGameFlag = true;
			GlobalDefine.init();
			Platform.init();
			Platform.createBilling();
			Platform.hasWaitFakeLoadingOver = false;
			if (PlayerPrefs.GetInt("firstOpenGameFlag", 0) == 0)
			{
				GlobalInf.firstOpenGameFlag = true;
				GlobalInf.nextSence = "Ganstars";
				Platform.startTimeLimitedOffer();
			}
			else
			{
				GlobalInf.firstOpenGameFlag = false;
				GlobalInf.nextSence = "MenuSence";
			}
		}
		if (GlobalInf.nextSence.Equals("MenuSence"))
		{
			loadingLine.gameObject.SetActiveRecursively(false);
			loadingLineBack.gameObject.SetActiveRecursively(false);
		}
	}

	private void Update()
	{
		if (!loadFlag)
		{
			loadFlag = true;
			async = Application.LoadLevelAsync(GlobalInf.nextSence);
			progress = 0;
		}
		if (async != null && !async.isDone)
		{
			p = (int)(async.progress * 100f);
		}
		if (progress < p)
		{
			progress++;
			loadingLine.fillAmount = (float)progress / 100f;
			GlobalInf.loadingProgress = progress;
		}
	}
}
