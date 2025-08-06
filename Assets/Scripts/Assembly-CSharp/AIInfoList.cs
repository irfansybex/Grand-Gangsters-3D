using UnityEngine;

public class AIInfoList : MonoBehaviour
{
	public static AIInfoList instance;

	public AIInfoLevelList[] aiData;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public AIInfo GetAIInfo(NPCTYPE typeIndex, int level)
	{
		return aiData[(int)typeIndex].infoList[level];
	}
}
