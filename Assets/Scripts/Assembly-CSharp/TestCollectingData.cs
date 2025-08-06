using UnityEngine;

[ExecuteInEditMode]
public class TestCollectingData : MonoBehaviour
{
	public bool runFlag;

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
			MonoBehaviour.print("run");
		}
	}

	public void Run()
	{
		int num = 0;
		for (int i = 4; i < 10; i++)
		{
			for (int j = 2; j < 12; j++)
			{
				mapData.blockLine[i].blockLine[j].collectingInfoList = null;
				mapData.blockLine[i].blockLine[j].collectingInfoList = new CollectingInfo[3];
				mapData.blockLine[i].blockLine[j].collectingInfoList[0] = new CollectingInfo();
				mapData.blockLine[i].blockLine[j].collectingInfoList[0].index = num;
				mapData.blockLine[i].blockLine[j].collectingInfoList[0].type = COLLECTTYPE.CAR;
				mapData.blockLine[i].blockLine[j].collectingInfoList[0].pos = new Vector3(mapData.startX + j * 100, 0.5f, mapData.startY + i * 100);
				mapData.blockLine[i].blockLine[j].collectingInfoList[1] = new CollectingInfo();
				mapData.blockLine[i].blockLine[j].collectingInfoList[1].index = num;
				mapData.blockLine[i].blockLine[j].collectingInfoList[1].type = COLLECTTYPE.HANDGUN;
				mapData.blockLine[i].blockLine[j].collectingInfoList[1].pos = new Vector3(mapData.startX + 10 + j * 100, 0.5f, mapData.startY + 10 + i * 100);
				mapData.blockLine[i].blockLine[j].collectingInfoList[2] = new CollectingInfo();
				mapData.blockLine[i].blockLine[j].collectingInfoList[2].index = num;
				mapData.blockLine[i].blockLine[j].collectingInfoList[2].type = COLLECTTYPE.MACHINEGUN;
				mapData.blockLine[i].blockLine[j].collectingInfoList[2].pos = new Vector3(mapData.startX + 20 + j * 100, 0.5f, mapData.startY + 20 + i * 100);
				num++;
			}
		}
	}
}
