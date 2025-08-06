using UnityEngine;

public class BlurScreen : MonoBehaviour
{
	public static BlurScreen instance;

	public GameObject blurCam;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public void TurnOnBlurScreen()
	{
	}

	public void TurnOffBlurScreen()
	{
		if (!GlobalDefine.smallPhoneFlag)
		{
			blurCam.SetActiveRecursively(false);
		}
	}

	private void OnDestroy()
	{
		if (instance != null)
		{
			instance = null;
		}
	}
}
