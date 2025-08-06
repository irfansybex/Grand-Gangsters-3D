using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class InitCollectingPosInfo : MonoBehaviour
{
	public bool runFlag;

	public GameObject source;

	public BlockMapDateNew mapData;

	public int carIndex;

	public int handGunIndex;

	public int machineGunIndex;

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
		carIndex = 0;
		handGunIndex = 0;
		machineGunIndex = 0;
		List<CollectingInfo> list = new List<CollectingInfo>();
		int num = 0;
		for (int i = 0; i < mapData.blockLine.Length; i++)
		{
			for (int j = 0; j < mapData.blockLine[i].blockLine.Length; j++)
			{
				GetCollectingList(j, i, list);
				mapData.blockLine[i].blockLine[j].collectingInfoList = null;
				if (list.Count > 0)
				{
					mapData.blockLine[i].blockLine[j].collectingInfoList = new CollectingInfo[list.Count];
					num = 0;
					for (int k = 0; k < list.Count; k++)
					{
						mapData.blockLine[i].blockLine[j].collectingInfoList[num] = new CollectingInfo();
						mapData.blockLine[i].blockLine[j].collectingInfoList[num].pos = list[k].pos;
						mapData.blockLine[i].blockLine[j].collectingInfoList[num].type = list[k].type;
						mapData.blockLine[i].blockLine[j].collectingInfoList[num].index = list[k].index;
						num++;
					}
				}
			}
		}
	}

	public void GetCollectingList(int x, int y, List<CollectingInfo> tempList)
	{
		tempList.Clear();
		float num = mapData.startX + x * 100;
		float num2 = num + 100f;
		float num3 = mapData.startY + y * 100;
		float num4 = num3 + 100f;
		for (int i = 0; i < source.transform.childCount; i++)
		{
			for (int j = 0; j < source.transform.GetChild(i).childCount; j++)
			{
				if (source.transform.GetChild(i).GetChild(j).position.x > num && source.transform.GetChild(i).GetChild(j).position.x < num2 && source.transform.GetChild(i).GetChild(j).position.z > num3 && source.transform.GetChild(i).GetChild(j).position.z < num4)
				{
					CollectingInfo collectingInfo = new CollectingInfo();
					if (source.transform.GetChild(i).GetChild(j).transform.parent.gameObject.name.Equals("Car"))
					{
						collectingInfo.type = COLLECTTYPE.CAR;
						collectingInfo.index = carIndex;
						carIndex++;
					}
					else if (source.transform.GetChild(i).GetChild(j).transform.parent.gameObject.name.Equals("HandGun"))
					{
						MonoBehaviour.print("x:y  == " + x + ":" + y);
						MonoBehaviour.print("handGunIndex : " + handGunIndex);
						collectingInfo.type = COLLECTTYPE.HANDGUN;
						collectingInfo.index = handGunIndex;
						handGunIndex++;
					}
					else if (source.transform.GetChild(i).GetChild(j).transform.parent.gameObject.name.Equals("MachineGun"))
					{
						MonoBehaviour.print("x:y  == " + x + ":" + y);
						MonoBehaviour.print("machineGunIndex : " + machineGunIndex);
						collectingInfo.type = COLLECTTYPE.MACHINEGUN;
						collectingInfo.index = machineGunIndex;
						machineGunIndex++;
					}
					collectingInfo.pos = source.transform.GetChild(i).GetChild(j).transform.position;
					tempList.Add(collectingInfo);
				}
			}
		}
	}
}
