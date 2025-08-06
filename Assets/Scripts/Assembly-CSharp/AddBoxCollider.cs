using UnityEngine;

[ExecuteInEditMode]
public class AddBoxCollider : MonoBehaviour
{
	public bool runFlag;

	public GameObject root;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Run();
		}
	}

	public void Run()
	{
		for (int i = 0; i < root.transform.childCount; i++)
		{
			for (int j = 0; j < root.transform.GetChild(i).childCount; j++)
			{
				root.transform.GetChild(i).GetChild(j).gameObject.AddComponent<BoxCollider>();
			}
		}
	}
}
