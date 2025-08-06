using UnityEngine;

public class DeliverModeFinishLabel : MonoBehaviour
{
	public bool flag;

	private void OnEnable()
	{
		flag = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("PlayerCar")) && !flag)
		{
			Invoke("EndMission", 0.1f);
			flag = true;
			GameUIController.instance.DisableLocateLabel();
		}
	}

	public void EndMission()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("UI/TaskEndUI")) as GameObject;
		TaskEndUIController component = gameObject.GetComponent<TaskEndUIController>();
		component.Reset(false, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, (int)GameController.instance.deliverMode.restTime, GameUIController.instance.rewardIndex);
		gameObject.transform.parent = GameUIController.instance.uiRoot.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localRotation = Quaternion.identity;
		GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
	}
}
