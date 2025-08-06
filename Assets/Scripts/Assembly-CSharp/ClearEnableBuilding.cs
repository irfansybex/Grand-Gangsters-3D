using UnityEngine;

[ExecuteInEditMode]
public class ClearEnableBuilding : MonoBehaviour
{
	public bool runFlag;

	public BlockMapDateNew block;

	private void Update()
	{
		if (!runFlag)
		{
			return;
		}
		runFlag = false;
		for (int i = 0; i < block.blockLine.Length; i++)
		{
			for (int j = 0; j < block.blockLine[i].blockLine.Length; j++)
			{
				block.blockLine[i].blockLine[j].enableBuildingList.Clear();
			}
		}
	}
}
