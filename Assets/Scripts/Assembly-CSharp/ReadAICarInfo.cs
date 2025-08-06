using UnityEngine;

[ExecuteInEditMode]
public class ReadAICarInfo : ReadTXT
{
	public bool runflag;

	public string fileName;

	public CarController[] aiCarList;

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
		for (int i = 0; i < aiCarList.Length - 1; i++)
		{
			aiCarList[i].maxSpeed = GetInt(i + 14, 3);
			aiCarList[i].maxSteerAngle = GetFloat(i + 14, 4);
			aiCarList[i].maxSpeedSteerAngle = GetFloat(i + 14, 5);
			aiCarList[i].carHealth.maxHealthVal = GetInt(i + 14, 6);
			aiCarList[i].carHealth.healthVal = aiCarList[i].carHealth.maxHealthVal;
		}
	}
}
