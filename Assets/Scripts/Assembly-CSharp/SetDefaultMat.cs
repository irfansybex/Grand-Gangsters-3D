using UnityEngine;

[ExecuteInEditMode]
public class SetDefaultMat : MonoBehaviour
{
	public bool runFlag;

	public BuildingPoolList temp;

	public BuildingMatPoolList target;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			SetMat();
			MonoBehaviour.print("run");
		}
	}

	public void SetMat()
	{
		for (int i = 0; i < temp.poolList.Count; i++)
		{
			if (temp.poolList[i] != null)
			{
				string text = temp.poolList[i].transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial.name.Split(' ')[0];
				MonoBehaviour.print(text);
				target.matPoolList[i].appearMat = Resources.Load("Mat/AppearBuildingMat/" + text) as Material;
				target.matPoolList[i].disappearMat = Resources.Load("Mat/DisappearBuildingMat/" + text) as Material;
				target.matPoolList[i].targetMat = Resources.Load("Mat/NormalBuildingMat/" + text) as Material;
				if (target.matPoolList[i].appearMat == null)
				{
					MonoBehaviour.print("sourceMat : " + i);
				}
				if (target.matPoolList[i].targetMat == null)
				{
					MonoBehaviour.print("targetMat : " + i);
				}
			}
		}
	}
}
