using UnityEngine;

[ExecuteInEditMode]
public class ReadPoliceLevelData : ReadTXT
{
	public bool runFlag;

	public string fileName;

	public PoliceLevelCtl instance;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			ReadFile();
		}
	}

	public void ReadFile()
	{
		InitArray(fileName);
		instance.attackPerson = GetInt(1, 2);
		instance.killPerson = GetInt(2, 2);
		instance.attackPolice = GetInt(4, 2);
		instance.killPolice = GetInt(5, 2);
		instance.robCar = GetInt(6, 2);
		instance.robPoliceCar = GetInt(7, 2);
		instance.scratchPerson = GetInt(8, 2);
		instance.crashPerson = GetInt(9, 2);
		instance.scratchPolice = GetInt(10, 2);
		instance.crashPolice = GetInt(11, 2);
		instance.scratchCar = GetInt(12, 2);
		instance.crashCar = GetInt(13, 2);
		instance.scratchPoliceCar = GetInt(14, 2);
		instance.crashPoliceCar = GetInt(15, 2);
	}
}
