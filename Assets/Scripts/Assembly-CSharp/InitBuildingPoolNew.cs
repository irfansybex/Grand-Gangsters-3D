using UnityEngine;

[ExecuteInEditMode]
public class InitBuildingPoolNew : MonoBehaviour
{
	public bool runFlag;

	public BuildingPool buildingPool;

	public BuildingPoolList buildingPoolList;

	public GameObject root;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			InitPool();
		}
	}

	public void InitPool()
	{
		int num = buildingPoolList.poolList.Count;
		for (int i = 0; i < root.transform.childCount; i++)
		{
			Transform child = root.transform.GetChild(i);
			for (int j = 0; j < child.childCount; j++)
			{
				GameObject gameObject = Object.Instantiate(child.transform.GetChild(j).gameObject) as GameObject;
				gameObject.gameObject.name = child.transform.GetChild(j).gameObject.name;
				BuildingPool component = (Object.Instantiate(buildingPool.gameObject) as GameObject).GetComponent<BuildingPool>();
				component.gameObject.name = gameObject.gameObject.name;
				component.transform.parent = buildingPoolList.transform;
				gameObject.transform.parent = component.transform;
				component.disableList.Add(gameObject);
				component.sourceObj = gameObject;
				component.index = num;
				num++;
				buildingPoolList.poolList.Add(component);
			}
		}
	}
}
