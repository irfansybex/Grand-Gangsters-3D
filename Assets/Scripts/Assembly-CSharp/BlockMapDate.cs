using System.Collections.Generic;
using UnityEngine;

public class BlockMapDate : MonoBehaviour
{
	public List<BlockLine> blockLine;

	public int startX;

	public int startY;

	public int endX;

	public int endY;

	public BlockDate GetBlockDate(Vector3 pos)
	{
		int index = ((int)pos.x - startX) / 100;
		int index2 = ((int)pos.z - startY) / 100;
		return blockLine[index2].blockLine[index];
	}
}
