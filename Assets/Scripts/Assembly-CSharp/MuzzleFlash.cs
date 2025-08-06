using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
	public Vector2 angle;

	private void Update()
	{
		if (Time.timeScale != 0f)
		{
			base.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
			base.transform.localEulerAngles = new Vector3(angle.x, angle.y, Random.Range(0f, 90f));
		}
	}
}
