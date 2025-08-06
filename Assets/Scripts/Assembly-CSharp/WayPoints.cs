using UnityEngine;

public class WayPoints : MonoBehaviour
{
	public Transform[] points;

	public int length;

	private void Awake()
	{
		length = points.Length;
	}
}
