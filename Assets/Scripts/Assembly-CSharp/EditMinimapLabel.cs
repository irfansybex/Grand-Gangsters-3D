using UnityEngine;

[ExecuteInEditMode]
public class EditMinimapLabel : MonoBehaviour
{
	public bool runFlag;

	public TaskLabelController taskLabelController;

	public GameObject drivingPreFerb;

	public GameObject deliverPreFerb;

	public GameObject survivalPreFerb;

	public GameObject gunKillingPreferb;

	public GameObject carKillingPreferb;

	public GameObject skillDrivingPreferb;

	public GameObject drivingRoot;

	public GameObject deliverRoot;

	public GameObject survivalRoot;

	public GameObject gunKillingRoot;

	public GameObject carKillingRoot;

	public GameObject skillDrivingRoot;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			MonoBehaviour.print("run");
			Execute();
		}
	}

	public void Execute()
	{
		for (int i = 0; i < taskLabelController.drivingTaskInfo.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(drivingPreFerb) as GameObject;
			gameObject.transform.parent = drivingRoot.transform;
			Vector2 vector = ChangeToMapPos(taskLabelController.drivingTaskInfo[i].startPos);
			gameObject.transform.localPosition = new Vector3(vector.x, 5f, vector.y);
			gameObject.gameObject.name = string.Empty + taskLabelController.drivingTaskInfo[i].taskIndex;
		}
		for (int j = 0; j < taskLabelController.deliverTaskInfo.Length; j++)
		{
			GameObject gameObject2 = Object.Instantiate(deliverPreFerb) as GameObject;
			gameObject2.transform.parent = deliverRoot.transform;
			Vector2 vector2 = ChangeToMapPos(taskLabelController.deliverTaskInfo[j].startPos);
			gameObject2.transform.localPosition = new Vector3(vector2.x, 5f, vector2.y);
			gameObject2.gameObject.name = string.Empty + taskLabelController.deliverTaskInfo[j].taskIndex;
		}
		for (int k = 0; k < taskLabelController.survivalTaskInfo.Length; k++)
		{
			GameObject gameObject3 = Object.Instantiate(survivalPreFerb) as GameObject;
			gameObject3.transform.parent = survivalRoot.transform;
			Vector2 vector3 = ChangeToMapPos(taskLabelController.survivalTaskInfo[k].startPos);
			gameObject3.transform.localPosition = new Vector3(vector3.x, 5f, vector3.y);
			gameObject3.gameObject.name = string.Empty + taskLabelController.survivalTaskInfo[k].taskIndex;
		}
		for (int l = 0; l < taskLabelController.gunKillingTaskInfo.Length; l++)
		{
			GameObject gameObject4 = Object.Instantiate(gunKillingPreferb) as GameObject;
			gameObject4.transform.parent = gunKillingRoot.transform;
			Vector2 vector4 = ChangeToMapPos(taskLabelController.gunKillingTaskInfo[l].startPos);
			gameObject4.transform.localPosition = new Vector3(vector4.x, 5f, vector4.y);
			gameObject4.gameObject.name = string.Empty + taskLabelController.gunKillingTaskInfo[l].taskIndex;
		}
		for (int m = 0; m < taskLabelController.carKillingTaskInfo.Length; m++)
		{
			GameObject gameObject5 = Object.Instantiate(carKillingPreferb) as GameObject;
			gameObject5.transform.parent = carKillingRoot.transform;
			Vector2 vector5 = ChangeToMapPos(taskLabelController.carKillingTaskInfo[m].startPos);
			gameObject5.transform.localPosition = new Vector3(vector5.x, 5f, vector5.y);
			gameObject5.gameObject.name = string.Empty + taskLabelController.carKillingTaskInfo[m].taskIndex;
		}
		for (int n = 0; n < taskLabelController.skillDrivingTaskInfo.Length; n++)
		{
			GameObject gameObject6 = Object.Instantiate(skillDrivingPreferb) as GameObject;
			gameObject6.transform.parent = skillDrivingRoot.transform;
			Vector2 vector6 = ChangeToMapPos(taskLabelController.skillDrivingTaskInfo[n].startPos);
			gameObject6.transform.localPosition = new Vector3(vector6.x, 5f, vector6.y);
			gameObject6.gameObject.name = string.Empty + taskLabelController.skillDrivingTaskInfo[n].taskIndex;
		}
	}

	public Vector2 ChangeToMapPos(Vector3 worldPos)
	{
		float x = (worldPos.x + 1000f) / 5f;
		float y = (worldPos.z + 1000f) / 5f;
		return new Vector2(x, y);
	}
}
