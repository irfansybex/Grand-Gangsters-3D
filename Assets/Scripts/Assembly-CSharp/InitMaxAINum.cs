using UnityEngine;

[ExecuteInEditMode]
public class InitMaxAINum : MonoBehaviour
{
	public bool runFlag;

	public BlockMapDateNew mapDate;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Run();
			MonoBehaviour.print("run");
		}
	}

	public void Run()
	{
		for (int i = 0; i < mapDate.blockLine.Length; i++)
		{
			for (int j = 0; j < mapDate.blockLine[i].blockLine.Length; j++)
			{
				mapDate.blockLine[i].blockLine[j].maxAINum = 10;
				mapDate.blockLine[i].blockLine[j].maxCarNum = 10;
			}
		}
	}
}
