using UnityEngine;

public class MinimapTaskLabelController : MonoBehaviour
{
	public GameObject labelRoot;

	public GameObject mapCircle;

	private void LateUpdate()
	{
		for (int i = 0; i < labelRoot.transform.childCount; i++)
		{
			for (int j = 0; j < labelRoot.transform.GetChild(i).childCount; j++)
			{
				labelRoot.transform.GetChild(i).GetChild(j).localEulerAngles = new Vector3(270f, 180f + base.transform.localEulerAngles.y, 0f);
			}
		}
		mapCircle.transform.eulerAngles = new Vector3(0f, -180f, 0f);
	}

	private void OnDisable()
	{
		if (!(labelRoot != null))
		{
			return;
		}
		for (int i = 0; i < labelRoot.transform.childCount; i++)
		{
			for (int j = 0; j < labelRoot.transform.GetChild(i).childCount; j++)
			{
				labelRoot.transform.GetChild(i).GetChild(j).localEulerAngles = new Vector3(270f, 180f, 0f);
			}
		}
	}
}
