using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditBuildingBlock : MonoBehaviour
{
	public bool runFlag;

	public BlockMapDateNew mapData;

	public GameObject rootObj;

	public BuildingPoolList buildingPoolList;

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			EditBuilding();
		}
	}

	public void EditBuilding()
	{
		List<GameObject> list = new List<GameObject>();
		int num = 0;
		for (int i = 0; i < mapData.blockLine.Length; i++)
		{
			for (int j = 0; j < mapData.blockLine[i].blockLine.Length; j++)
			{
				GetBuildingList(j, i, list);
				mapData.blockLine[i].blockLine[j].buildingList = null;
				if (list.Count <= 0)
				{
					continue;
				}
				mapData.blockLine[i].blockLine[j].buildingList = new BuildingInfo[CountBuildingNum(list)];
				num = 0;
				for (int k = 0; k < list.Count; k++)
				{
					mapData.blockLine[i].blockLine[j].buildingList[num] = new BuildingInfo();
					mapData.blockLine[i].blockLine[j].buildingList[num].position = list[k].transform.position;
					mapData.blockLine[i].blockLine[j].buildingList[num].rotation = list[k].transform.eulerAngles;
					mapData.blockLine[i].blockLine[j].buildingList[num].index = GetBuildingIndex(list[k].name);
					num++;
					if (list[k].transform.childCount > 0)
					{
						for (int l = 0; l < list[k].transform.childCount; l++)
						{
							mapData.blockLine[i].blockLine[j].buildingList[num] = new BuildingInfo();
							mapData.blockLine[i].blockLine[j].buildingList[num].position = list[k].transform.GetChild(l).position;
							mapData.blockLine[i].blockLine[j].buildingList[num].rotation = list[k].transform.GetChild(l).eulerAngles;
							mapData.blockLine[i].blockLine[j].buildingList[num].index = GetBuildingIndex(list[k].transform.GetChild(l).gameObject.name);
							num++;
						}
					}
				}
			}
		}
	}

	public int CountBuildingNum(List<GameObject> objList)
	{
		int num = 0;
		for (int i = 0; i < objList.Count; i++)
		{
			num += objList[i].transform.childCount;
		}
		return num + objList.Count;
	}

	public int GetBuildingIndex(string name)
	{
		for (int i = 0; i < buildingPoolList.poolList.Count; i++)
		{
			if (buildingPoolList.poolList[i].gameObject.name.Equals(name))
			{
				return buildingPoolList.poolList[i].index;
			}
		}
		return -1;
	}

	public void GetBuildingList(int x, int y, List<GameObject> tempList)
	{
		tempList.Clear();
		float num = mapData.startX + x * 100;
		float num2 = num + 100f;
		float num3 = mapData.startY + y * 100;
		float num4 = num3 + 100f;
		for (int i = 0; i < rootObj.transform.childCount; i++)
		{
			if (rootObj.transform.GetChild(i).position.x > num && rootObj.transform.GetChild(i).position.x < num2 && rootObj.transform.GetChild(i).position.z > num3 && rootObj.transform.GetChild(i).position.z < num4)
			{
				tempList.Add(rootObj.transform.GetChild(i).gameObject);
			}
		}
	}
}
