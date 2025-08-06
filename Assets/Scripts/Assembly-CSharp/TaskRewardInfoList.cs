using UnityEngine;

public class TaskRewardInfoList : MonoBehaviour
{
	public static TaskRewardInfoList instance;

	public TaskRewardInfo[] taskRewardList;

	public string[] handGunName;

	public string[] carName;

	public string[] machineGunName;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}
}
