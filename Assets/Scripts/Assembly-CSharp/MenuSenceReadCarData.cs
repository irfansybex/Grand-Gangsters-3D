using UnityEngine;

[ExecuteInEditMode]
public class MenuSenceReadCarData : ReadTXT
{
	public bool runflag;

	public string fileName;

	public CarPageControllor carPage;

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
		for (int i = 0; i < carPage.itemCarList.Length - 1; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				carPage.itemCarList[i].maxSpeedLevelList[j] = GetInt(i * 3 + j + 1, 3);
				carPage.itemCarList[i].maxSteerAngleLevelList[j] = GetFloat(i * 3 + j + 1, 4);
				carPage.itemCarList[i].maxSpeedSteerAngleLevelList[j] = GetFloat(i * 3 + j + 1, 5);
				carPage.itemCarList[i].maxHealthLevelList[j] = GetInt(i * 3 + j + 1, 6);
			}
		}
		carPage.itemCarList[4].maxSpeedLevelList[0] = GetInt(13, 3);
		carPage.itemCarList[4].maxSteerAngleLevelList[0] = GetFloat(13, 4);
		carPage.itemCarList[4].maxSpeedSteerAngleLevelList[0] = GetFloat(13, 5);
		carPage.itemCarList[4].maxHealthLevelList[0] = GetInt(13, 6);
	}
}
