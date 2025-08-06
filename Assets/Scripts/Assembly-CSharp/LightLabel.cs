using System.Collections.Generic;
using UnityEngine;

public class LightLabel : MonoBehaviour
{
	public int curIndex;

	public List<Vector3> lightLabelPosList;

	public float addTime;

	public bool endMissionFlag;

	public FinishLabel finishLabel;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("PlayerCar"))
		{
			AudioController.instance.play(AudioType.PICK_ITEM);
			if (curIndex < lightLabelPosList.Count - 2)
			{
				TempObjControllor.instance.GetEatLightLabel().Play();
				curIndex++;
				base.transform.position = lightLabelPosList[curIndex];
				GameController.instance.driveMode.restTime += addTime;
				GameController.instance.driveMode.minimapController.mapDrawPath.ClearLine();
				GameController.instance.driveMode.minimapController.FindWay(lightLabelPosList[curIndex]);
				GameController.instance.driveMode.minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(GameController.instance.driveMode.lightLabel.lightLabelPosList[curIndex]) + new Vector3(1000f, 480f, 0f));
			}
			else
			{
				TempObjControllor.instance.GetEatLightLabel().Play();
				curIndex++;
				finishLabel.transform.position = lightLabelPosList[curIndex];
				finishLabel.transform.eulerAngles = GameController.instance.driveMode.endAngle;
				finishLabel.gameObject.SetActiveRecursively(true);
				GameController.instance.driveMode.restTime += addTime;
				GameController.instance.driveMode.minimapController.mapDrawPath.ClearLine();
				GameController.instance.driveMode.minimapController.FindWay(lightLabelPosList[curIndex]);
				GameController.instance.driveMode.minimapController.EnableTargetPos(AttackAILabelPool.instance.ChangToMapLocalPos(finishLabel.transform.position) + new Vector3(1000f, 480f, 0f));
				base.gameObject.SetActiveRecursively(false);
				GameUIController.instance.EnableLocateLabel(finishLabel.gameObject);
			}
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
