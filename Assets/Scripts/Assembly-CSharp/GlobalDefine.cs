using UnityEngine;

public class GlobalDefine
{
	public const float STANDARD_SCREEN_WIDTH = 800f;

	public const float STANDARD_SCREEN_HEIGHT = 480f;

	public static Vector2 screenScale = new Vector2(1f, 1f);

	public static bool smallPhoneFlag;

	public static float screenRatioWidth;

	public static float screenWHRatio;

	public static float screenWidthFit;

	public static float screenWordFit;

	public static int COLLECT_HAND_GUN_MAXNUM = 20;

	public static int COLLECT_MACHINE_GUN_MAXNUM = 20;

	public static int COLLECT_CAR_MAXNUM = 20;

	private static bool isInit;

	public static void init()
	{
		if (!isInit)
		{
			isInit = true;
			screenScale.x = (float)Screen.width / 800f;
			screenScale.y = (float)Screen.height / 480f;
			screenWHRatio = (float)Screen.width / (float)Screen.height;
			screenRatioWidth = 480f * screenWHRatio;
			screenWidthFit = screenRatioWidth / 800f;
			screenWordFit = screenWHRatio / 1.6666666f;
			if (Screen.width < 500 || Screen.height < 330 || SystemInfo.systemMemorySize < 500)
			{
				smallPhoneFlag = true;
				Shader.globalMaximumLOD = 201;
			}
			else
			{
				smallPhoneFlag = false;
				Shader.globalMaximumLOD = 301;
			}
		}
	}
}
