using UnityEngine;

[ExecuteInEditMode]
public class ReadDailyTask : ReadTXT
{
	public bool runflag;

	public string fileName;

	public DailyTaskController dailyTaskController;

	private void Update()
	{
		if (runflag)
		{
			runflag = false;
			Run();
			MonoBehaviour.print("run");
		}
	}

	public void Run()
	{
		InitArray(fileName);
		for (int i = 0; i < dailyTaskController.dailyTaskCompleteNum0.Length; i++)
		{
			dailyTaskController.dailyTaskCompleteNum0[i] = GetInt(i + 1, 1);
		}
		for (int j = 0; j < dailyTaskController.dailyTaskCompleteNum1.Length; j++)
		{
			dailyTaskController.dailyTaskCompleteNum1[j] = GetInt(j + 1, 3);
		}
		for (int k = 0; k < dailyTaskController.dailyTaskCompleteNum2.Length; k++)
		{
			dailyTaskController.dailyTaskCompleteNum2[k] = GetInt(k + 1, 5);
		}
	}
}
