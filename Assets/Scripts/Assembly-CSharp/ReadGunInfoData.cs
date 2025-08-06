using UnityEngine;

[ExecuteInEditMode]
public class ReadGunInfoData : ReadTXT
{
	public bool runflag;

	public string fileName;

	public HandGunPageController handGunPage;

	public MachineGunPageController machineGunPage;

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
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				handGunPage.itemHandGunList[i].damageLevelList[j] = GetFloat(i * 3 + 2 + j, 3);
				handGunPage.itemHandGunList[i].bulletNumLevelList[j] = GetInt(i * 3 + 2 + j, 4);
				handGunPage.itemHandGunList[i].shotIntervalLevelList[j] = 1f / GetFloat(i * 3 + 2 + j, 5);
				handGunPage.itemHandGunList[i].accuracyLevelList[j] = GetFloat(i * 3 + 2 + j, 6);
				handGunPage.itemHandGunList[i].reloadLevelList[j] = GetFloat(i * 3 + 2 + j, 7);
			}
		}
		for (int k = 0; k < 5; k++)
		{
			for (int l = 0; l < 3; l++)
			{
				machineGunPage.itemMachineGunList[k].damageLevelList[l] = GetFloat(k * 3 + 20 + l, 3);
				machineGunPage.itemMachineGunList[k].bulletNumLevelList[l] = GetInt(k * 3 + 20 + l, 4);
				machineGunPage.itemMachineGunList[k].shotIntervalLevelList[l] = 1f / GetFloat(k * 3 + 20 + l, 5);
				machineGunPage.itemMachineGunList[k].accuracyLevelList[l] = GetFloat(k * 3 + 20 + l, 6);
				machineGunPage.itemMachineGunList[k].reloadLevelList[l] = GetFloat(k * 3 + 20 + l, 7);
			}
		}
	}
}
