using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditPlayerWallController : MonoBehaviour
{
	public bool runFlag;

	public GameObject rootObj;

	public BlockMapDateNew mapData;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Run();
		}
	}

	public void Run()
	{
		List<GameObject> list = new List<GameObject>();
		int num = 0;
		for (int i = 0; i < mapData.blockLine.Length; i++)
		{
			for (int j = 0; j < mapData.blockLine[i].blockLine.Length; j++)
			{
				GetPlayerWallList(j, i, list);
				mapData.blockLine[i].blockLine[j].playerWall = null;
				if (list.Count > 0)
				{
					mapData.blockLine[i].blockLine[j].playerWall = new PlayerWallInfo[list.Count];
					num = 0;
					for (int k = 0; k < list.Count; k++)
					{
						mapData.blockLine[i].blockLine[j].playerWall[num] = new PlayerWallInfo();
						mapData.blockLine[i].blockLine[j].playerWall[num].position = list[k].transform.position;
						mapData.blockLine[i].blockLine[j].playerWall[num].rotation = list[k].transform.eulerAngles;
						num++;
					}
				}
			}
		}
	}

	public void GetPlayerWallList(int x, int y, List<GameObject> tempList)
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

	public int CountBuildingNum(List<GameObject> objList)
	{
		int num = 0;
		for (int i = 0; i < objList.Count; i++)
		{
			num += objList[i].transform.childCount;
		}
		return num + objList.Count;
	}
}
