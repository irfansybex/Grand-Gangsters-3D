using UnityEngine;

public class RobMotorFinishCollider : MonoBehaviour
{
	public bool flag;

	private void OnEnable()
	{
		MonoBehaviour.print("RobMotorFinishCollider");
		flag = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerCar") && !flag)
		{
			if (PlayerController.instance.car.targetMotorFlag)
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
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerCar") && !flag && !PlayerController.instance.car.targetMotorFlag)
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
		component.Reset(false, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, (int)GameController.instance.robMotorMode.restTime, GameUIController.instance.rewardIndex);
		gameObject.transform.parent = GameUIController.instance.uiRoot.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localRotation = Quaternion.identity;
		GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
	}
}
