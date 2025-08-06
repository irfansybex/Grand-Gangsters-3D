using UnityEngine;

public class roadInfoListNew : MonoBehaviour
{
	public RoadInfoNew[] roadList;

	public static roadInfoListNew instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
}
