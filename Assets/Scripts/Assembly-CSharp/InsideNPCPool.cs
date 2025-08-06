using UnityEngine;

public class InsideNPCPool : MonoBehaviour
{
	public static InsideNPCPool instance;

	public GameObject[] list;

	public int curIndex;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public GameObject GetInsideNPC()
	{
		return list[++curIndex % list.Length];
	}
}
