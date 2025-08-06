using UnityEngine;

[ExecuteInEditMode]
public class ReadSurvialInfo : ReadTXT
{
	public bool initDataFlag;

	public bool initDataFlag2;

	public bool initDataFlag3;

	public bool initDataFlag4;

	public string fileName;

	public SurvivalModeInfo[] survivalMode;

	public int stape = -1;

	private void Update()
	{
		if (initDataFlag4)
		{
			initDataFlag4 = false;
			InitArray(fileName);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 1; i < array.Length; i++)
			{
				if (!GetString(i, 0).Trim().Equals(string.Empty) && !GetString(i, 0).Equals("#"))
				{
					num = GetInt(i, 0) - 1;
					MonoBehaviour.print("infoIndex : " + num);
				}
				if (!GetString(i, 1).Trim().Equals(string.Empty))
				{
					num2 = GetInt(i, 1);
					MonoBehaviour.print("groupIndex : " + num2);
				}
				if (!GetString(i, 2).Trim().Equals(string.Empty))
				{
					num3 = GetInt(i, 2);
					MonoBehaviour.print("waveIndex : " + num3);
					InitAIData(survivalMode[num].npcGroupList[num2].waveInfoList[num3], i, num);
				}
			}
			MonoBehaviour.print("initDataFlag4");
		}
		if (initDataFlag3)
		{
			initDataFlag3 = false;
			InitArray(fileName);
			for (int j = 0; j < survivalMode.Length; j++)
			{
				for (int k = 0; k < survivalMode[j].npcGroupList.Length; k++)
				{
					for (int l = 0; l < survivalMode[j].npcGroupList[k].waveInfoList.Length; l++)
					{
						MonoBehaviour.print(" K : " + l);
						InitAIInfo(survivalMode[j].npcGroupList[k].waveInfoList[l]);
					}
				}
			}
			MonoBehaviour.print("stape2 done");
			stape = 3;
		}
		if (initDataFlag2)
		{
			initDataFlag2 = false;
			InitArray(fileName);
			int num4 = 0;
			MonoBehaviour.print("array.Length : " + array.Length);
			for (int m = 1; m < array.Length; m++)
			{
				if (!GetString(m, 0).Trim().Equals(string.Empty) && !GetString(m, 0).Equals("#"))
				{
					num4 = GetInt(m, 0) - 1;
					MonoBehaviour.print("infoIndex : " + num4);
				}
				if (!GetString(m, 1).Trim().Equals(string.Empty))
				{
					int @int = GetInt(m, 1);
					MonoBehaviour.print("infoIndex : " + num4 + " groupIndex : " + @int);
					InitWave(survivalMode[num4].npcGroupList[@int], m);
				}
			}
			MonoBehaviour.print("stape1 done");
			stape = 2;
		}
		if (initDataFlag)
		{
			initDataFlag = false;
			Run();
			MonoBehaviour.print("array.Length : " + array.Length);
			MonoBehaviour.print("run");
		}
	}

	public void Run()
	{
		InitArray(fileName);
		for (int i = 1; i < array.Length; i++)
		{
			if (!GetString(i, 0).Trim().Equals(string.Empty) && !GetString(i, 0).Equals("#"))
			{
				InitGroup(GetInt(i, 0) - 1, i);
			}
		}
		stape = 1;
	}

	public void InitGroup(int index, int startLine)
	{
		int num = 0;
		for (int i = startLine; i < array.Length; i++)
		{
			if (!GetString(i, 1).Trim().Equals(string.Empty))
			{
				if (GetInt(i, 1) < num)
				{
					break;
				}
				num = GetInt(i, 1);
			}
		}
		survivalMode[index].npcGroupList = new NPCGroupInfo[num + 1];
	}

	public void InitWave(NPCGroupInfo groupInfo, int startLine)
	{
		int num = 0;
		for (int i = startLine; i < array.Length; i++)
		{
			if (!GetString(i, 2).Trim().Equals(string.Empty))
			{
				if (GetInt(i, 2) < num)
				{
					break;
				}
				num = GetInt(i, 2);
			}
		}
		MonoBehaviour.print("curNum : " + (num + 1));
		groupInfo.waveInfoList = new NPCWaveInfo[num + 1];
	}

	public void InitAIInfo(NPCWaveInfo waveInfo)
	{
		waveInfo.waveAIList = new WaveAIList[1];
	}

	public void InitAIData(NPCWaveInfo waveInfo, int line, int level)
	{
		waveInfo.waveAIList[0].typeIndex = (NPCTYPE)GetInt(line, 4);
		waveInfo.waveAIList[0].level = level;
		waveInfo.delayTime = GetInt(line, 3);
	}
}
