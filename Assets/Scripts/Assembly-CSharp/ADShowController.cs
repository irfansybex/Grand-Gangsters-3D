using UnityEngine;

public class ADShowController : MonoBehaviour
{
	public static ADShowController instance;

	public bool showFlag;

	public float waitTime;

	public float countTime;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!showFlag)
		{
			countTime += Time.deltaTime;
			if (countTime >= waitTime)
			{
				countTime = 0f;
				showFlag = true;
			}
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
