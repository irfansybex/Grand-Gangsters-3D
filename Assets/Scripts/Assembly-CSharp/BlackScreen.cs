using UnityEngine;

public class BlackScreen : MonoBehaviour
{
	public static BlackScreen instance;

	public GameObject camObj;

	public GameObject blackPix;

	public bool pauseFlag;

	public float startTime;

	public string animaName;

	public bool disActiveFlag;

	public float disActiveTimeStart;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public void DelayTurnOffScreen(float t)
	{
		Invoke("TurnOffScreen", t);
	}

	public void TurnOnScreen()
	{
		disActiveFlag = false;
		disActiveTimeStart = Time.realtimeSinceStartup;
		if (Time.timeScale == 0f)
		{
			pauseFlag = true;
			camObj.SetActiveRecursively(true);
			blackPix.GetComponent<Animation>().Play("TurnOnScreen");
			startTime = Time.realtimeSinceStartup;
			animaName = "TurnOnScreen";
		}
		else
		{
			pauseFlag = false;
			camObj.SetActiveRecursively(true);
			blackPix.GetComponent<Animation>().Play("TurnOnScreen");
		}
	}

	public void SetDisactive()
	{
		disActiveFlag = false;
		camObj.SetActiveRecursively(false);
	}

	public void TurnOffScreen()
	{
		disActiveFlag = false;
		if (Time.timeScale == 0f)
		{
			pauseFlag = true;
			camObj.SetActiveRecursively(true);
			blackPix.GetComponent<Animation>().Play("TurnOffScreen");
			startTime = Time.realtimeSinceStartup;
			animaName = "TurnOffScreen";
		}
		else
		{
			pauseFlag = false;
			camObj.SetActiveRecursively(true);
			blackPix.GetComponent<Animation>().Play("TurnOffScreen");
		}
	}

	public void BlackSc()
	{
		camObj.SetActiveRecursively(true);
		blackPix.GetComponent<Animation>().Play("BlackScreen");
	}

	private void Update()
	{
		if (pauseFlag)
		{
			blackPix.GetComponent<Animation>()[animaName].time = Time.realtimeSinceStartup - startTime;
			if (blackPix.GetComponent<Animation>()[animaName].time > blackPix.GetComponent<Animation>()[animaName].length)
			{
				pauseFlag = false;
				disActiveFlag = true;
			}
		}
		if (disActiveFlag && Time.realtimeSinceStartup - disActiveTimeStart > 0.6f)
		{
			SetDisactive();
			disActiveFlag = false;
		}
	}
}
