using UnityEngine;

[ExecuteInEditMode]
public class EditMotorPos : MonoBehaviour
{
	public bool runFlag;

	public bool clearFlag;

	public BlockMapDateNew mapData;

	public Transform root;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Run();
			MonoBehaviour.print("Run");
		}
		if (clearFlag)
		{
			clearFlag = false;
			Clear();
			MonoBehaviour.print("Clear");
		}
	}

	public void Clear()
	{
		for (int i = 0; i < mapData.blockLine.Length; i++)
		{
			for (int j = 0; j < mapData.blockLine[i].blockLine.Length; j++)
			{
				mapData.blockLine[i].blockLine[j].motorPos.Clear();
				mapData.blockLine[i].blockLine[j].motorRot.Clear();
			}
		}
	}

	public void Run()
	{
		for (int i = 0; i < root.childCount; i++)
		{
			mapData.GetBlockDate(root.GetChild(i).position).motorPos.Add(root.GetChild(i).position);
			mapData.GetBlockDate(root.GetChild(i).position).motorRot.Add(root.GetChild(i).rotation);
		}
	}
}
