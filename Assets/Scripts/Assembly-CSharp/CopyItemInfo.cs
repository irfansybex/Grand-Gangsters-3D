using UnityEngine;

[ExecuteInEditMode]
public class CopyItemInfo : MonoBehaviour
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
				target.blockLine[i].blockLine[j].itemList = null;
				if (source.blockLine[i].blockLine[j].itemList != null)
				{
					target.blockLine[i].blockLine[j].itemList = new BuildingInfo[source.blockLine[i].blockLine[j].itemList.Length];
					for (int k = 0; k < target.blockLine[i].blockLine[j].itemList.Length; k++)
					{
						target.blockLine[i].blockLine[j].itemList[k] = source.blockLine[i].blockLine[j].itemList[k];
					}
				}
			}
		}
	}
}
