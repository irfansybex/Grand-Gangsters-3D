using UnityEngine;

[ExecuteInEditMode]
public class AddWordFitEdit : MonoBehaviour
{
	public bool runFlag;

	public Transform root;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Run(root);
			MonoBehaviour.print("run");
		}
	}

	public void Run(Transform nRoot)
	{
		for (int i = 0; i < nRoot.childCount; i++)
		{
			if (nRoot.GetChild(i).gameObject.GetComponent<UILabel>() != null && nRoot.GetChild(i).gameObject.GetComponent<NGUIFitWord>() == null)
			{
				nRoot.GetChild(i).gameObject.AddComponent<NGUIFitWord>();
			}
			Run(nRoot.GetChild(i));
		}
	}
}
