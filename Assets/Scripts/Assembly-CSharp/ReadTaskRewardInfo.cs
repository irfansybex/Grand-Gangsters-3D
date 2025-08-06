using UnityEngine;

[ExecuteInEditMode]
public class ReadTaskRewardInfo : ReadTXT
{
	public bool runflag;

	public string fileName;

	public TaskRewardInfoList taskRewardInfoList;

	private void Update()
	{
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
		for (int i = 0; i < taskRewardInfoList.taskRewardList.Length; i++)
		{
			taskRewardInfoList.taskRewardList[i].itemList = new TaskEndItemInfo[GetItemNum(i + 1)];
			MonoBehaviour.print("i : " + i + " ItemNum : " + GetItemNum(i + 1));
		}
	}

	public void Run()
	{
		InitArray(fileName);
		for (int i = 0; i < taskRewardInfoList.taskRewardList.Length; i++)
		{
			taskRewardInfoList.taskRewardList[i].cashOneStar = GetInt(i + 1, 6);
			taskRewardInfoList.taskRewardList[i].cashTwoStar = GetInt(i + 1, 7);
			taskRewardInfoList.taskRewardList[i].cashThreeStar = GetInt(i + 1, 8);
			taskRewardInfoList.taskRewardList[i].firstGold = GetInt(i + 1, 9);
			taskRewardInfoList.taskRewardList[i].threeStarGold = GetInt(i + 1, 10);
			taskRewardInfoList.taskRewardList[i].kitRate = (int)(GetFloat(i + 1, 11) * 100f + 0.9f);
			for (int j = 0; j < taskRewardInfoList.taskRewardList[i].itemList.Length; j++)
			{
				switch (GetInt(i + 1, 12 + j * 3))
				{
				case 0:
					taskRewardInfoList.taskRewardList[i].itemList[j].itemType = ITEMTYPE.HANDGUN;
					break;
				case 1:
					taskRewardInfoList.taskRewardList[i].itemList[j].itemType = ITEMTYPE.MACHINEGUN;
					break;
				case 2:
					taskRewardInfoList.taskRewardList[i].itemList[j].itemType = ITEMTYPE.CAR;
					break;
				}
				taskRewardInfoList.taskRewardList[i].itemList[j].index = GetInt(i + 1, 12 + j * 3 + 1);
				taskRewardInfoList.taskRewardList[i].itemList[j].rate = (int)(GetFloat(i + 1, 12 + j * 3 + 2) * 100f + 0.9f);
			}
		}
	}

	public int GetItemNum(int i)
	{
		int num = 1;
		if (GetInt(i, 15) != -1)
		{
			num++;
		}
		if (GetInt(i, 18) != -1)
		{
			num++;
		}
		return num;
	}
}
