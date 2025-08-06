using UnityEngine;

public class SkillDrivingFinalObj : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter()
	{
		GameUIController.instance.DisableLocateLabel();
		Invoke("DelayEndMission", 0.1f);
		base.gameObject.SetActiveRecursively(false);
		GameUIController.instance.DisableLocateLabel();
	}

	private void DelayEndMission()
	{
		EndMission(false);
	}

	public void EndMission(bool isFail)
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("UI/TaskEndUI")) as GameObject;
		TaskEndUIController component = gameObject.GetComponent<TaskEndUIController>();
		component.Reset(isFail, GameUIController.instance.gameMode, GameUIController.instance.taskIndex, (int)GameController.instance.skillDrivingMode.restTime, GameUIController.instance.rewardIndex);
		gameObject.transform.parent = GameUIController.instance.uiRoot.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localRotation = Quaternion.identity;
		GameUIController.instance.controlUIRoot.SetActiveRecursively(false);
	}
}
