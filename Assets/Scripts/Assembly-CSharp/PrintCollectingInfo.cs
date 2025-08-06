using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrintCollectingInfo : MonoBehaviour
{
	public bool runFlag;

	public BlockMapDateNew mapData;

	public List<CollectingInfo> carList;

	public List<CollectingInfo> handGunList;

	public List<CollectingInfo> machineGunList;

	public GameObject pre;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			SetCarPos();
			MonoBehaviour.print("Run");
		}
	}

	public void SetCarPos()
	{
		for (int i = 0; i < carList.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(pre) as GameObject;
			gameObject.transform.parent = pre.transform.parent;
			gameObject.transform.position = carList[i].pos;
			gameObject.name = string.Empty + carList[i].index;
		}
	}

	public void Run()
	{
		for (int i = 0; i < mapData.blockLine.Length; i++)
		{
			for (int j = 0; j < mapData.blockLine[i].blockLine.Length; j++)
			{
				for (int k = 0; k < mapData.blockLine[i].blockLine[j].collectingInfoList.Length; k++)
				{
					switch (mapData.blockLine[i].blockLine[j].collectingInfoList[k].type)
					{
					case COLLECTTYPE.CAR:
						carList.Add(mapData.blockLine[i].blockLine[j].collectingInfoList[k]);
						break;
					case COLLECTTYPE.HANDGUN:
						handGunList.Add(mapData.blockLine[i].blockLine[j].collectingInfoList[k]);
						break;
					case COLLECTTYPE.MACHINEGUN:
						machineGunList.Add(mapData.blockLine[i].blockLine[j].collectingInfoList[k]);
						break;
					}
				}
			}
		}
		MonoBehaviour.print("CarListNum!!!!!!!!!!!!!!: " + carList.Count);
		MonoBehaviour.print("HandGunListNum!!!!!!!!!!!!!!: " + handGunList.Count);
		MonoBehaviour.print("MachineGunListNum!!!!!!!!!!!!!!: " + machineGunList.Count);
	}
}
