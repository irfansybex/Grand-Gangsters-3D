using UnityEngine;

public class BlockMapDateNew : MonoBehaviour
{
	public BlockLineNew[] blockLine;

	public int startX;

	public int startY;

	public int endX;

	public int endY;

	public static BlockMapDateNew instance;

	public BlockDateNew GetBlockDate(Vector3 pos)
	{
		int num = ((int)pos.x - startX) / 100;
		int num2 = ((int)pos.z - startY) / 100;
		return blockLine[num2].blockLine[num];
	}

	public Vector2 GetBlockIndex(Vector3 pos)
	{
		return new Vector2(((int)pos.x - startX) / 100, ((int)pos.z - startY) / 100);
	}

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}
