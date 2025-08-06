using UnityEngine;

public class FinishLabel : MonoBehaviour
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
			GameUIController.instance.DisableLocateLabel();
			Invoke("DelayEndMission", 0.1f);
			flag = true;
			base.gameObject.SetActiveRecursively(false);
		}
	}

	private void DelayEndMission()
	{
		EndMission(false);
	}

	public void EndMission(bool isFail)
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("UI/TaskEndUI")) as GameObject;
		TaskEndUIController component = gameObject.GetComponent<TaskEndUIController>();
		component.Reset(isFail, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, (int)GameController.instance.driveMode.restTime, GameUIController.instance.rewardIndex);
		gameObject.transform.parent = GameUIController.instance.uiRoot.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localRotation = Quaternion.identity;
		GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
	}
}
