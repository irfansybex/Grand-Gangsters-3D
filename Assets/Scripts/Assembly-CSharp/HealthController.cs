using UnityEngine;

public class HealthController : MonoBehaviour
{
	public delegate void onDestroy();

	public bool isDead;

	public float maxHealthVal = 100f;

	public float healthVal = 100f;

	public bool invincible;

	public bool playerCarFlag;

	public bool playerFlag;

	public bool cureFlag;

	public bool killFlag;

	public onDestroy OnDestroy;

	private void Start()
	{
	}

	private void Update()
	{
		if (killFlag)
		{
			killFlag = false;
			Damaged(200f);
		}
	}

	public void Damaged(float damageVal)
	{
		if (cureFlag || isDead)
		{
			return;
		}
		healthVal -= damageVal;
		if (GlobalInf.healthKitTutorialFlag && playerFlag && damageVal < 90f && healthVal <= 35f)
		{
			GlobalInf.healthKitTutorialFlag = false;
			Time.timeScale = 0f;
			GameUIController.instance.healthKitTutorialFlag = true;
			GameUIController.instance.miniMapFinger.depth = 15;
			GameUIController.instance.fingerObj.transform.localPosition = new Vector3(320f * GlobalDefine.screenWidthFit, 76f, 0f);
			GameUIController.instance.addHealthBtn.gameObject.GetComponent<UISprite>().depth = 16;
			if (GlobalInf.healthKitNum == 0)
			{
				GlobalInf.healthKitNum = 1;
				GameUIController.instance.OnInitHealthKitNum();
			}
		}
		if (GlobalInf.toolKitTutorialFlag && playerCarFlag && healthVal <= 35f)
		{
			GlobalInf.toolKitTutorialFlag = false;
			Time.timeScale = 0f;
			GameUIController.instance.toolKitTutorialFlag = true;
			GameUIController.instance.miniMapFinger.depth = 15;
			GameUIController.instance.fingerObj.transform.localPosition = new Vector3(320f * GlobalDefine.screenWidthFit, 76f, 0f);
			GameUIController.instance.addToolKitBtn.gameObject.GetComponent<UISprite>().depth = 16;
			if (GlobalInf.toolKitNum == 0)
			{
				GlobalInf.toolKitNum = 1;
				GameUIController.instance.OnInitToolKitNum();
			}
		}
		if (healthVal <= 0f)
		{
			isDead = true;
			if (!invincible)
			{
				OnDestroy();
			}
		}
		if (playerCarFlag && GlobalInf.carInfo != null)
		{
			GlobalInf.carInfo.restHealthVal = (int)healthVal;
			StoreDateController.SetCarHealthNum(GlobalInf.playerCarIndex, GlobalInf.carInfo.restHealthVal);
		}
		if (playerFlag)
		{
			if ((int)healthVal < 0)
			{
				healthVal = 0f;
			}
			GlobalInf.playerHealthVal = (int)healthVal;
			StoreDateController.SetPlayerHealthVal();
		}
	}

	public void Reset()
	{
		isDead = false;
		if (!playerCarFlag)
		{
			if (!playerFlag)
			{
				healthVal = maxHealthVal;
			}
			else
			{
				healthVal = GlobalInf.playerHealthVal;
			}
		}
		else if (GlobalInf.carInfo != null)
		{
			if (GlobalInf.carInfo.restHealthVal < 10)
			{
				GlobalInf.carInfo.restHealthVal = 10;
			}
			healthVal = GlobalInf.carInfo.restHealthVal;
		}
	}

	public void RepaireCar()
	{
		if (!playerCarFlag)
		{
			healthVal = maxHealthVal;
		}
		else if (GlobalInf.carInfo != null)
		{
			GlobalInf.carInfo.restHealthVal = GlobalInf.carInfo.maxHealthVal;
			healthVal = GlobalInf.carInfo.maxHealthVal;
			StoreDateController.SetCarHealthNum(GlobalInf.playerCarIndex, GlobalInf.carInfo.restHealthVal);
		}
	}
}
