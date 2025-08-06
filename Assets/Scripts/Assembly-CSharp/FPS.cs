using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
	public float updateInterval = 0.5f;

	private float accum;

	private int frames;

	private float timeleft;

	private void Start()
	{
		if (!base.GetComponent<Text>())
		{
			base.enabled = false;
		}
		else
		{
			timeleft = updateInterval;
		}
	}

	private void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if ((double)timeleft <= 0.0)
		{
			float num = accum / (float)frames;
			string text = string.Format("{0:F2}", num);
			base.GetComponent<Text>().text = text;
			if (num < 20f)
			{
				base.GetComponent<Text>().material.color = Color.red;
			}
			else if (num < 30f)
			{
				base.GetComponent<Text>().material.color = Color.yellow;
			}
			else
			{
				base.GetComponent<Text>().material.color = Color.green;
			}
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}
}
