using UnityEngine;

[ExecuteInEditMode]
public class TestBuildingLoad : MonoBehaviour
{
	public bool runFlag;

	public int XNum;

	public int YNum;

	public BlockMapDateNew mapDate;

	public BuildingPoolList poolList;

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Reset();
			SetBuildingEnable(XNum, YNum);
			MonoBehaviour.print("run");
		}
	}

	public void Reset()
	{
	}

	public void SetBuildingEnable(int x, int y)
	{
		EnableBuilding(x, y);
		if (x > 0)
		{
			EnableBuilding(x - 1, y);
			if (y > 0)
			{
				EnableBuilding(x - 1, y - 1);
				if (x < mapDate.blockLine[0].blockLine.Length - 1)
				{
					EnableBuilding(x + 1, y - 1);
				}
			}
		}
		if (x < mapDate.blockLine[0].blockLine.Length - 1)
		{
			EnableBuilding(x + 1, y);
			if (y < mapDate.blockLine.Length - 1)
			{
				EnableBuilding(x + 1, y + 1);
				if (x > 0)
				{
					EnableBuilding(x - 1, y + 1);
				}
			}
		}
		if (y > 0)
		{
			EnableBuilding(x, y - 1);
		}
		if (y < mapDate.blockLine.Length - 1)
		{
			EnableBuilding(x, y + 1);
		}
	}

	public void EnableBuilding(int x, int y)
	{
		if (mapDate.blockLine[y].blockLine[x].buildingList != null)
		{
			for (int i = 0; i < mapDate.blockLine[y].blockLine[x].buildingList.Length; i++)
			{
				GameObject obj = poolList.poolList[mapDate.blockLine[y].blockLine[x].buildingList[i].index].GetObj();
				obj.transform.position = mapDate.blockLine[y].blockLine[x].buildingList[i].position;
				obj.transform.eulerAngles = mapDate.blockLine[y].blockLine[x].buildingList[i].rotation;
			}
		}
	}
}
