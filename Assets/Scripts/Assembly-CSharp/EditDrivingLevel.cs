using UnityEngine;

[ExecuteInEditMode]
public class EditDrivingLevel : MonoBehaviour
{
	public bool runFlag;

	public Transform root;

	public DrivingModeController drivingModeController;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Run();
			MonoBehaviour.print("run");
		}
	}

	public void Run()
	{
		for (int i = 0; i < root.childCount; i++)
		{
			InitLevel(i);
		}
	}

	public void InitLevel(int index)
	{
		Transform transform = root.Find(string.Empty + index);
		MonoBehaviour.print(transform.name);
		drivingModeController.drivingLevel[index].startPos = transform.Find("Start").position;
		drivingModeController.drivingLevel[index].startAngle = transform.Find("Start").eulerAngles;
		drivingModeController.drivingLevel[index].posList = null;
		drivingModeController.drivingLevel[index].posList = new Vector3[transform.childCount - 1];
		for (int i = 0; i < transform.childCount - 1; i++)
		{
			drivingModeController.drivingLevel[index].posList[i] = transform.Find("Cube" + i).position;
		}
	}
}
