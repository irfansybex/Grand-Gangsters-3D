using UnityEngine;

[ExecuteInEditMode]
public class InitBuildingPool1 : MonoBehaviour
{
	public bool runFlag;

	public BuildingPool buildingPool;

	public BuildingMatPoolList buildingPoolList;

	public GameObject root;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			InitBuild();
		}
	}

	public void InitBuild()
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		for (int i = 0; i < root.transform.childCount; i++)
		{
			empty2 = root.transform.GetChild(i).gameObject.name;
			if (!empty2.Equals(empty))
			{
				AddAPool(root.transform.GetChild(i).gameObject, GetBuildingIndex(empty2));
			}
		}
	}

	public int GetBuildingIndex(string name)
	{
		string s = name.Remove(0, 8);
		return int.Parse(s);
	}

	public void AddAPool(GameObject obj, int index)
	{
		BuildingPool buildingPool = Object.Instantiate(this.buildingPool) as BuildingPool;
		GameObject gameObject = Object.Instantiate(obj) as GameObject;
		buildingPool.transform.parent = buildingPoolList.transform;
		buildingPool.transform.localPosition = Vector3.zero;
		buildingPool.transform.localRotation = Quaternion.identity;
		buildingPool.transform.gameObject.name = "BuildingPool" + index;
		gameObject.transform.parent = buildingPool.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.name = "Building" + index;
		buildingPool.disableList.Add(gameObject);
	}
}
