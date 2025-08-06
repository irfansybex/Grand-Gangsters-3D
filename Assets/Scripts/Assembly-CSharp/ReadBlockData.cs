using UnityEngine;

[ExecuteInEditMode]
public class ReadBlockData : ReadTXT
{
	public bool runflag;

	public string fileName;

	public BlockMapDateNew blockMapData;

	private void Update()
	{
		if (runflag)
		{
			runflag = false;
			Run();
			MonoBehaviour.print("run");
		}
	}

	public void Run()
	{
		InitArray(fileName);
		for (int i = 0; i < blockMapData.blockLine.Length; i++)
		{
			for (int j = 0; j < blockMapData.blockLine[i].blockLine.Length; j++)
			{
				blockMapData.blockLine[i].blockLine[j].aiRateIndex = GetInt(j + 1, 18 - i);
			}
		}
	}
}
