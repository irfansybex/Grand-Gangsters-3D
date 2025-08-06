using UnityEngine;

[ExecuteInEditMode]
public class ResetBuildingPos : MonoBehaviour
{
	public bool runFlag;

	public GameObject sourseRoot;

	public GameObject targetRoot;

	public GameObject fangziRoot;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			ResetPos();
			MonoBehaviour.print("run");
		}
	}

	public void ResetPos()
	{
		for (int i = 0; i < fangziRoot.transform.childCount; i++)
		{
			string text = fangziRoot.transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh.name.Split(' ')[0];
			if (sourseRoot.transform.Find(text) != null)
			{
				BuildingPool component = sourseRoot.transform.Find(text).gameObject.GetComponent<BuildingPool>();
				GameObject gameObject = Object.Instantiate(component.sourceObj) as GameObject;
				gameObject.name = text;
				gameObject.transform.position = fangziRoot.transform.GetChild(i).transform.position;
				gameObject.transform.rotation = fangziRoot.transform.GetChild(i).transform.rotation;
				gameObject.transform.parent = targetRoot.transform;
			}
		}
	}
}
