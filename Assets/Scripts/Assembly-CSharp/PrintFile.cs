using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class PrintFile : MonoBehaviour
{
	public bool runFlag;

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
		if (!File.Exists("PathInfo.txt"))
		{
			File.Create("PathInfo.txt");
		}
		File.WriteAllLines("PathInfo.txt", new string[5] { "helloWorld0", "helloWorld1\n", "helloWorld2", null, null });
	}
}
