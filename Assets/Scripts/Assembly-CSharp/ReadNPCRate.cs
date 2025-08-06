using UnityEngine;

[ExecuteInEditMode]
public class ReadNPCRate : ReadTXT
{
	public bool initArrayFlag;

	public bool runflag;

	public string fileName;

	public NPCGenerateInfoList aiRateList;

	private int countSumRate;

	private void Update()
	{
		if (initArrayFlag)
		{
			initArrayFlag = false;
			InitArray();
			MonoBehaviour.print("initArrayFlag");
		}
		if (runflag)
		{
			runflag = false;
			Run();
			MonoBehaviour.print("run");
		}
	}

	public void InitArray()
	{
		InitArray(fileName);
		for (int i = 3; i < 15; i++)
		{
			aiRateList.infoList[i - 3].aiInfo = new WaveAIList[CountInfoLength(i)];
			aiRateList.infoList[i - 3].rate = new int[CountInfoLength(i)];
		}
	}

	public void Run()
	{
		InitArray(fileName);
		int num = 0;
		for (int i = 3; i < 15; i++)
		{
			countSumRate = 0;
			num = 0;
			for (int j = 2; j < 15; j++)
			{
				if (GetFloat(j, i) != 0f)
				{
					aiRateList.infoList[i - 3].rate[num] = (int)(GetFloat(j, i) * 100f * 2f + 0.9f);
					aiRateList.infoList[i - 3].aiInfo[num].typeIndex = GetAIType(j);
					aiRateList.infoList[i - 3].aiInfo[num].level = 0;
					countSumRate += aiRateList.infoList[i - 3].rate[num];
					num++;
				}
			}
			aiRateList.infoList[i - 3].sumAttackAIRate = countSumRate;
		}
	}

	public NPCTYPE GetAIType(int n)
	{
		switch (n)
		{
		case 2:
			return NPCTYPE.NORMALBLACK_PUNCH;
		case 3:
			return NPCTYPE.NORMALBLACK_HG;
		case 4:
			return NPCTYPE.GANSTARWHITE_PUNCH;
		case 5:
			return NPCTYPE.GANSTARWHITE_HG;
		case 6:
			return NPCTYPE.GANSTARBLACK_PUNCH;
		case 7:
			return NPCTYPE.GANSTARBLACK_HG;
		case 8:
			return NPCTYPE.GANSTARBLACK_MG;
		case 9:
			return NPCTYPE.GANSTARNAKED_PUNCH;
		case 10:
			return NPCTYPE.GANSTARNAKED_HG;
		case 11:
			return NPCTYPE.GANSTARNAKED_MG;
		case 12:
			return NPCTYPE.POLICE1_HG;
		case 13:
			return NPCTYPE.POLICE2_HG;
		case 14:
			return NPCTYPE.POLICE2_MG;
		default:
			return NPCTYPE.NORMALBLACK_PUNCH;
		}
	}

	public int CountInfoLength(int index)
	{
		int num = 0;
		for (int i = 2; i < 15; i++)
		{
			if (GetFloat(i, index) != 0f)
			{
				num++;
			}
		}
		return num;
	}
}
