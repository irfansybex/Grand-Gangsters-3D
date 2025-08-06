using UnityEngine;

[ExecuteInEditMode]
public class CopyCollectingInfo : MonoBehaviour
{
	public bool runFlag;

	public BlockMapDateNew source;

	public BlockMapDateNew target;

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Copy();
		}
	}

	public void Copy()
	{
		for (int i = 0; i < target.blockLine.Length; i++)
		{
			for (int j = 0; j < target.blockLine[i].blockLine.Length; j++)
			{
				target.blockLine[i].blockLine[j].collectingInfoList = null;
				if (source.blockLine[i].blockLine[j].collectingInfoList != null)
				{
					target.blockLine[i].blockLine[j].collectingInfoList = new CollectingInfo[source.blockLine[i].blockLine[j].collectingInfoList.Length];
					for (int k = 0; k < target.blockLine[i].blockLine[j].collectingInfoList.Length; k++)
					{
						target.blockLine[i].blockLine[j].collectingInfoList[k] = source.blockLine[i].blockLine[j].collectingInfoList[k];
					}
				}
			}
		}
	}
}
