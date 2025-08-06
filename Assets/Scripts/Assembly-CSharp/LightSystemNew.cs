using UnityEngine;

public class LightSystemNew : MonoBehaviour
{
	public static LightSystemNew instance;

	public float redTime;

	public float yellowTime;

	public float greenTime;

	public float countTime;

	public bool nsPassFlag;

	public bool ewPassFlag;

	public bool straitFlag;

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
		countTime += Time.deltaTime;
		if (countTime < redTime)
		{
			nsPassFlag = false;
			ewPassFlag = true;
			straitFlag = true;
		}
		else if (countTime < redTime + yellowTime)
		{
			nsPassFlag = false;
			ewPassFlag = false;
		}
		else if (countTime < redTime + yellowTime + greenTime)
		{
			nsPassFlag = true;
			ewPassFlag = false;
		}
		else if (countTime < redTime + 2f * yellowTime + greenTime)
		{
			nsPassFlag = false;
			ewPassFlag = false;
		}
		else if (countTime < 2f * redTime + 2f * yellowTime + greenTime)
		{
			nsPassFlag = false;
			ewPassFlag = true;
			straitFlag = false;
		}
		else if (countTime < 2f * redTime + 3f * yellowTime + greenTime)
		{
			nsPassFlag = false;
			ewPassFlag = false;
		}
		else if (countTime < 2f * redTime + 3f * yellowTime + 2f * greenTime)
		{
			nsPassFlag = true;
			ewPassFlag = false;
		}
		else if (countTime < 2f * redTime + 4f * yellowTime + 2f * greenTime)
		{
			nsPassFlag = false;
			ewPassFlag = false;
		}
		else
		{
			countTime = 0f;
		}
	}
}
