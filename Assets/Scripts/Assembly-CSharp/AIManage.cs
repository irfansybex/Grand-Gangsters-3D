using UnityEngine;

public class AIManage : MonoBehaviour
{
	public AIController[] ai;

	private float tempDis;

	public float nearestDis;

	public int index;

	private void Awake()
	{
		for (int i = 0; i < ai.Length; i++)
		{
			ai[i].aiManage = this;
		}
	}

	public void OnAlarm(Vector3 pos)
	{
		for (int i = 0; i < ai.Length; i++)
		{
			tempDis = (pos - ai[i].transform.position).sqrMagnitude;
			if (tempDis > 0f && tempDis < 50f)
			{
				ai[i].OnAlarm();
			}
		}
	}

	public AIController GetNearestNPC(Vector3 pos)
	{
		nearestDis = float.PositiveInfinity;
		index = -1;
		for (int i = 0; i < ai.Length; i++)
		{
			if (ai[i].visableRander != null && ai[i].visableRander.GetComponent<Renderer>().isVisible && !ai[i].dieFlag)
			{
				tempDis = (ai[i].transform.position - pos).sqrMagnitude;
				if (tempDis < nearestDis)
				{
					index = i;
					nearestDis = tempDis;
				}
			}
		}
		if (index != -1)
		{
			return ai[index];
		}
		return null;
	}
}
