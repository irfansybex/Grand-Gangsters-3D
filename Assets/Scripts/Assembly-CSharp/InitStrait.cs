using UnityEngine;

[ExecuteInEditMode]
public class InitStrait : MonoBehaviour
{
	public bool runFlag;

	public roadInfoListNew roadList;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			MonoBehaviour.print("run");
		}
	}

	public void Init()
	{
	}
}
