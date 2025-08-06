using UnityEngine;

[ExecuteInEditMode]
public class ChangeName : MonoBehaviour
{
	public bool runFlag;

	public GameObject root;

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
		for (int i = 0; i < root.transform.childCount; i++)
		{
			if (root.transform.GetChild(i).gameObject.name.Equals("dongqingshu"))
			{
				root.transform.GetChild(i).gameObject.name = "xiaoludeng";
			}
		}
	}
}
