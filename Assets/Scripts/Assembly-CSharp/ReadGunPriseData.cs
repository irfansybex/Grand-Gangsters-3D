using UnityEngine;

[ExecuteInEditMode]
public class ReadGunPriseData : ReadTXT
{
	public bool runflag;

	public string fileName;

	public HandGunPageController handGunPage;

	public MachineGunPageController machineGunPage;

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
		for (int i = 0; i < handGunPage.itemHandGunList.Length; i++)
		{
			handGunPage.itemHandGunList[i].gunGoldPrise = GetInt(i + 1, 5);
			handGunPage.itemHandGunList[i].gunPrise = GetInt(i + 1, 4);
			handGunPage.itemHandGunList[i].upgradeGunNum[0] = GetInt(i + 1, 6);
			handGunPage.itemHandGunList[i].upgradeGunNum[1] = GetInt(i + 1, 7);
			handGunPage.itemHandGunList[i].gunInfo.bulletPrise = GetInt(i + 1, 8);
		}
		for (int j = 0; j < machineGunPage.itemMachineGunList.Length; j++)
		{
			machineGunPage.itemMachineGunList[j].gunPrise = GetInt(j + 7, 4);
			machineGunPage.itemMachineGunList[j].gunGoldPrise = GetInt(j + 7, 5);
			machineGunPage.itemMachineGunList[j].upgradeGunNum[0] = GetInt(j + 7, 6);
			machineGunPage.itemMachineGunList[j].upgradeGunNum[1] = GetInt(j + 7, 7);
			machineGunPage.itemMachineGunList[j].gunInfo.bulletPrise = GetInt(j + 7, 8);
		}
		for (int k = 0; k < carPage.itemCarList.Length; k++)
		{
			carPage.itemCarList[k].carPrise = GetInt(k + 12, 4);
			carPage.itemCarList[k].carGoldPrise = GetInt(k + 12, 5);
			carPage.itemCarList[k].upgradeCarNum[0] = GetInt(k + 12, 6);
			carPage.itemCarList[k].upgradeCarNum[1] = GetInt(k + 12, 7);
		}
	}
}
