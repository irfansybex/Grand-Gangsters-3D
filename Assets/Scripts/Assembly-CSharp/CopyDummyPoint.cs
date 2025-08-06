using UnityEngine;

[ExecuteInEditMode]
public class CopyDummyPoint : MonoBehaviour
{
	public bool runFlag;

	public LightSystem ls;

	private void Update()
	{
		if (!runFlag)
		{
			return;
		}
		runFlag = false;
		MonoBehaviour.print("ffffffffff");
		for (int i = 0; i < ls.lightpoint.Count; i++)
		{
			for (int j = 0; j < ls.lightpoint[i].CrossPoint.Count; j++)
			{
				if (ls.lightpoint[i].CrossPoint[j] != null)
				{
					ls.lightpoint[i].CrossPoint[j].CrossPoint = true;
					ls.lightpoint[i].CrossPoint[j].lightpointsystem = ls.lightpoint[i];
				}
			}
			for (int k = 0; k < ls.lightpoint[i].dummyCrossPoint.Count; k++)
			{
				if (ls.lightpoint[i].dummyCrossPoint[k] != null)
				{
					ls.lightpoint[i].dummyCrossPoint[k].dummyCrossPoint = true;
					ls.lightpoint[i].dummyCrossPoint[k].lightpointsystem = ls.lightpoint[i];
				}
			}
		}
	}
}
