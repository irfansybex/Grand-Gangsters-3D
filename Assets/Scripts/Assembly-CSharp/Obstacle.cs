using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public GameObject[] obj;

	public Vector3[] pos;

	private float timeCount;

	public float flashTime;

	private int index;

	private void Start()
	{
	}

	private void Update()
	{
		timeCount += Time.deltaTime;
		if (timeCount >= flashTime)
		{
			timeCount = 0f;
			Flash();
		}
	}

	public void Flash()
	{
		for (int i = 0; i < obj.Length; i++)
		{
			obj[i].transform.localPosition = pos[(i + index) % obj.Length];
		}
		index = (index + 1) % obj.Length;
	}
}
