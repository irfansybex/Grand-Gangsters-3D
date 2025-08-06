using UnityEngine;

[ExecuteInEditMode]
public class ReadPoliceLevelDate2 : ReadTXT
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
		ReadCol(instance.levelScorce, 1);
		ReadCol(instance.policeCarNum, 2);
		ReadCol(instance.policeNum, 3);
		ReadCol(instance.policeLevel, 4);
		ReadCol(instance.safeDis, 5);
	}

	public void ReadCol(int[] data, int colNum)
	{
		for (int i = 0; i < 5; i++)
		{
			data[i] = GetInt(i + 1, colNum);
		}
	}

	public void ReadCol(float[] data, int colNum)
	{
		for (int i = 0; i < 5; i++)
		{
			data[i] = GetFloat(i + 1, colNum);
		}
	}
}
