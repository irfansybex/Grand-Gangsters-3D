using UnityEngine;

public class FacePlayer : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		Vector3 vector = Camera.main.transform.position - base.transform.position;
		base.transform.forward = new Vector3(vector.x, 0f, vector.z);
	}
}
