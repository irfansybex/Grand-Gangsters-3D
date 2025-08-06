using UnityEngine;

public class HeartUIControllor : MonoBehaviour
{
	public HealthController playerHealth;

	public UISprite hartPic;

	public float curHealthVal;

	public float curHartRate;

	public UILabel healValLabel;

	public bool carHartFlag;

	public bool changeFlag;

	public float changeCount;

	public float timeCount;

	public float startVal;

	public float targetVal;

	public float changeStarVal;

	private void Start()
	{
	}

	private void OnEnable()
	{
		if (carHartFlag && PlayerController.instance != null && PlayerController.instance.car != null)
		{
			playerHealth = PlayerController.instance.car.carHealth;
		}
	}

	public void ResetCureState()
	{
		timeCount = 0f;
		startVal = playerHealth.healthVal;
		targetVal = playerHealth.maxHealthVal;
		playerHealth.cureFlag = true;
	}

	private void Update()
	{
		if (playerHealth.cureFlag)
		{
			timeCount += Time.deltaTime;
			if (timeCount >= 5f)
			{
				timeCount = 0f;
				playerHealth.cureFlag = false;
				TempObjControllor.instance.healKitObj.gameObject.SetActiveRecursively(false);
			}
			if (Mathf.Abs(playerHealth.healthVal - (float)(int)curHealthVal) > 1f)
			{
				curHealthVal = Mathf.Lerp(startVal, playerHealth.healthVal, timeCount / 5f);
				curHartRate = curHealthVal / playerHealth.maxHealthVal;
				hartPic.fillAmount = curHartRate;
				healValLabel.text = string.Empty + (int)curHealthVal;
			}
			else
			{
				healValLabel.text = string.Empty + (int)playerHealth.healthVal;
			}
		}
		else
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			if (playerHealth.healthVal <= 0f)
			{
				healValLabel.text = "0";
			}
			else if (Mathf.Abs(playerHealth.healthVal - (float)(int)curHealthVal) > 1f)
			{
				if (!changeFlag)
				{
					changeFlag = true;
					changeCount = 0f;
					changeStarVal = curHealthVal;
				}
				changeCount += Time.deltaTime;
				curHealthVal = Mathf.Lerp(changeStarVal, playerHealth.healthVal, changeCount) + 0.1f;
				curHartRate = curHealthVal / playerHealth.maxHealthVal;
				hartPic.fillAmount = curHartRate;
				healValLabel.text = string.Empty + (int)curHealthVal;
			}
			else
			{
				changeFlag = false;
				healValLabel.text = string.Empty + (int)playerHealth.healthVal;
			}
		}
	}
}
