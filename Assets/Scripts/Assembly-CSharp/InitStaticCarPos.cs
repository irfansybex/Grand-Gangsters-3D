using UnityEngine;

[ExecuteInEditMode]
public class InitStaticCarPos : MonoBehaviour
{
	public bool runFlag;

	public Transform carStaticPosRoot;

	public BlockMapDate blockDate;

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			InitStaticPos();
		}
	}

	public void InitStaticPos()
	{
		for (int i = 0; i < blockDate.blockLine.Count; i++)
		{
			for (int j = 0; j < blockDate.blockLine[i].blockLine.Count; j++)
			{
				blockDate.blockLine[i].blockLine[j].carPos.Clear();
			}
		}
		for (int k = 0; k < carStaticPosRoot.childCount; k++)
		{
			blockDate.GetBlockDate(carStaticPosRoot.GetChild(k).position).carPos.Add(carStaticPosRoot.GetChild(k));
		}
	}
}
