using UnityEngine;

[ExecuteInEditMode]
public class ResetPlayerMeshPos : MonoBehaviour
{
	public Transform root;

	public Animation anima;

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
		root.gameObject.GetComponent<Animation>()["stand"].enabled = true;
		root.gameObject.GetComponent<Animation>().Play();
		root.gameObject.GetComponent<Animation>()["stand"].time = 0f;
		root.gameObject.GetComponent<Animation>().Sample();
	}
}
