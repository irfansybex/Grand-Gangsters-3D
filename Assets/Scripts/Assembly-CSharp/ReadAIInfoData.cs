using UnityEngine;

[ExecuteInEditMode]
public class ReadAIInfoData : ReadTXT
{
	public bool runflag;

	public string fileName;

	public AIInfoList aiInfoList;

	public NPCPoolController poolController;

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
		for (int i = 0; i < aiInfoList.aiData.Length; i++)
		{
			for (int j = 0; j < aiInfoList.aiData[i].infoList.Length; j++)
			{
				aiInfoList.aiData[i].infoList[j].accuracy = GetFloat(i + 1, 9);
				aiInfoList.aiData[i].infoList[j].attackVal = GetFloat(i + 1, 7);
				aiInfoList.aiData[i].infoList[j].fireInterval = 1f / GetFloat(i + 1, 8);
				aiInfoList.aiData[i].infoList[j].health = GetInt(i + 1, 3 + j);
				if (GetString(i + 1, 2).Equals("HG"))
				{
					aiInfoList.aiData[i].infoList[j].handGunFlag = true;
					aiInfoList.aiData[i].infoList[j].machineGunFlag = false;
				}
				else if (GetString(i + 1, 2).Equals("MG"))
				{
					aiInfoList.aiData[i].infoList[j].handGunFlag = false;
					aiInfoList.aiData[i].infoList[j].machineGunFlag = true;
				}
				else
				{
					aiInfoList.aiData[i].infoList[j].handGunFlag = false;
					aiInfoList.aiData[i].infoList[j].machineGunFlag = false;
				}
				aiInfoList.aiData[i].infoList[j].fallingMoney = GetInt(i + 1, j + 10);
				aiInfoList.aiData[i].infoList[j].bulletRate = GetFloat(i + 1, 14);
			}
		}
		for (int k = 0; k < poolController.normalAIHealthVal.Length; k++)
		{
			poolController.normalAIHealthVal[k] = GetInt(14, k + 3);
			poolController.normalAIFallingMoney[k] = GetInt(14, k + 10);
		}
		poolController.normalAIBulletRate = GetFloat(14, 14);
	}
}
