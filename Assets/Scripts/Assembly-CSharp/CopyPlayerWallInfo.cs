using UnityEngine;

[ExecuteInEditMode]
public class CopyPlayerWallInfo : MonoBehaviour
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
				target.blockLine[i].blockLine[j].playerWall = null;
				if (source.blockLine[i].blockLine[j].playerWall != null)
				{
					target.blockLine[i].blockLine[j].playerWall = new PlayerWallInfo[source.blockLine[i].blockLine[j].playerWall.Length];
					for (int k = 0; k < target.blockLine[i].blockLine[j].playerWall.Length; k++)
					{
						target.blockLine[i].blockLine[j].playerWall[k] = source.blockLine[i].blockLine[j].playerWall[k];
					}
				}
			}
		}
	}
}
