using UnityEngine;

public class OnActiveActionCtl : MonoBehaviour
{
	public float countTime;

	public bool exitFlag;

	public GameObject lightLabel;

	public GAMEMODE gameMode;

	public int taskIndex;

	public int rewardIndex;

	public SlotController slotObj;

	public bool noAttackAIFlag;

	private void Start()
	{
	}

	private void Update()
	{
		if (!exitFlag)
		{
			return;
		}
		countTime += Time.deltaTime;
		if (countTime > 2f)
		{
			exitFlag = false;
			if (!GameUIController.instance.clickTaskLabelFlag)
			{
				TaskLabelController.instance.taskLabelUI.transform.GetChild(0).GetComponent<Animation>().Play("TaskLabelOut");
				TaskLabelController.instance.taskLabelUI.OnDisableOKBtn();
				GameUIController.instance.gameMode = GAMEMODE.NORMAL;
				GameUIController.instance.curTaskInfo = null;
				Invoke("LateDisabelLabel", 0.5f);
			}
		}
	}

	public void LateDisabelLabel()
	{
		TaskLabelController.instance.taskLabelUI.gameObject.SetActiveRecursively(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Player") && other.gameObject.layer != LayerMask.NameToLayer("PlayerCar"))
		{
			return;
		}
		if (PoliceLevelCtl.level == 0 && AICarPoolController.instance.policeCarCount == 0)
		{
			if (!TaskLabelController.instance.taskLabelUI.gameObject.active)
			{
				TaskLabelController.instance.ResetTaskLabelUI(gameMode, taskIndex);
				TaskLabelController.instance.taskLabelUI.OnEnableOKBtn();
				GameUIController.instance.gameMode = gameMode;
				GameUIController.instance.taskIndex = taskIndex;
				GameUIController.instance.rewardIndex = rewardIndex;
				if (gameMode == GAMEMODE.SLOT)
				{
					GameController.instance.slotMode.slot = slotObj;
				}
				GameController.instance.startLabel = this;
			}
			if (!GlobalDefine.smallPhoneFlag)
			{
				lightLabel.GetComponent<Animation>().Play("LightLabel");
			}
		}
		else
		{
			GameUIController.instance.EnableOutOffAmmo("You have to escape from police chase\nbefore entering a task");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Player") && other.gameObject.layer != LayerMask.NameToLayer("PlayerCar"))
		{
			return;
		}
		if (PoliceLevelCtl.level == 0 && AICarPoolController.instance.policeCarCount == 0)
		{
			if (!TaskLabelController.instance.taskLabelUI.gameObject.active)
			{
				TaskLabelController.instance.ResetTaskLabelUI(gameMode, taskIndex);
				TaskLabelController.instance.taskLabelUI.OnEnableOKBtn();
				GameUIController.instance.gameMode = gameMode;
				GameUIController.instance.taskIndex = taskIndex;
				GameUIController.instance.rewardIndex = rewardIndex;
				if (gameMode == GAMEMODE.SLOT)
				{
					GameController.instance.slotMode.slot = slotObj;
				}
				GameController.instance.startLabel = this;
				if (!GlobalDefine.smallPhoneFlag)
				{
					lightLabel.GetComponent<Animation>().Play("LightLabel");
				}
				GameUIController.instance.taskLabelTimeCountFlag = false;
			}
		}
		else
		{
			if (TaskLabelController.instance.taskLabelUI.gameObject.active)
			{
				TaskLabelController.instance.taskLabelUI.transform.GetChild(0).GetComponent<Animation>().Play("TaskLabelOut");
				TaskLabelController.instance.taskLabelUI.OnDisableOKBtn();
				GameUIController.instance.gameMode = GAMEMODE.NORMAL;
				GameUIController.instance.curTaskInfo = null;
				Invoke("LateDisabelLabel", 0.5f);
			}
			GameUIController.instance.EnableOutOffAmmo("You have to escape from police chase\nbefore entering a task");
		}
	}

	public void DelayDisableLabel()
	{
		GameUIController.instance.DisableOutOffAmmo();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("PlayerCar"))
		{
			if (TaskLabelController.instance.taskLabelUI.gameObject.active)
			{
				exitFlag = true;
				countTime = 0f;
			}
			if (!GlobalDefine.smallPhoneFlag)
			{
				lightLabel.GetComponent<Animation>().Stop();
			}
			Invoke("DelayDisableLabel", 1f);
		}
	}
}
