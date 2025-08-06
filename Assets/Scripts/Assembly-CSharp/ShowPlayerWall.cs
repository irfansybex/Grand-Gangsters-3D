using UnityEngine;

[ExecuteInEditMode]
public class ShowPlayerWall : MonoBehaviour
{
	public bool runFlag;

	public BlockMapDateNew mapData;

	public GameObject source;

	private GameObject temp;

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
	}

	public void Run()
	{
		for (int i = 0; i < mapData.blockLine.Length; i++)
		{
			for (int j = 0; j < mapData.blockLine[i].blockLine.Length; j++)
			{
				for (int k = 0; k < mapData.blockLine[i].blockLine[j].playerWall.Length; k++)
				{
					temp = (GameObject)Object.Instantiate(source);
					temp.transform.parent = base.transform;
					temp.transform.position = mapData.blockLine[i].blockLine[j].playerWall[k].position;
					temp.transform.eulerAngles = mapData.blockLine[i].blockLine[j].playerWall[k].rotation;
				}
			}
		}
	}
}
