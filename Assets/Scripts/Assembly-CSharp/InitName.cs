using UnityEngine;

[ExecuteInEditMode]
public class InitName : MonoBehaviour
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
			for (int i = 0; i < root.transform.childCount - 1; i++)
			{
				MonoBehaviour.print(root.transform.GetChild(i).gameObject.GetComponent<MeshFilter>().sharedMesh.name);
			}
		}
	}
}
