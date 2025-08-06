using UnityEngine;

public class RobCarModeFinishCollider : MonoBehaviour
{
	public bool flag;

	private void OnEnable()
	{
		flag = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerCar") && !flag)
		{
			if (PlayerController.instance.car.carType == GameController.instance.robbingCarMode.targetCarType)
			{
				Invoke("EndMission", 0.1f);
				flag = true;
				GameUIController.instance.DisableLocateLabel();
			}
			else
			{
				GameUIController.instance.EnableOutOffAmmo("WrongCar");
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerCar") && !flag && PlayerController.instance.car.carType != GameController.instance.robbingCarMode.targetCarType)
		{
			Invoke("DisableLabel", 1.5f);
		}
	}

	public void DisableLabel()
	{
		GameUIController.instance.DisableOutOffAmmo();
	}

	public void EndMission()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("UI/TaskEndUI")) as GameObject;
		TaskEndUIController component = gameObject.GetComponent<TaskEndUIController>();
		if (!GameController.instance.robbingCarMode.robCarTutorialFlag)
		{
			component.Reset(false, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, (int)GameController.instance.robbingCarMode.restTime, GameUIController.instance.rewardIndex);
		}
		else
		{
			component.Reset(false, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, 1, GameUIController.instance.rewardIndex);
			PlayerPrefs.SetInt("RobCarTutorial", 1);
		}
		gameObject.transform.parent = GameUIController.instance.uiRoot.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localRotation = Quaternion.identity;
		GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
		GameController.instance.robbingCarMode.robCarUI.gameObject.SetActiveRecursively(false);
	}
}
