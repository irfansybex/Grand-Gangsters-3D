using UnityEngine;

public class NPCGenerateInfoList : MonoBehaviour
{
	public static NPCGenerateInfoList instance;

	public NPCGenerateInfo[] infoList;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Destroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}
}
