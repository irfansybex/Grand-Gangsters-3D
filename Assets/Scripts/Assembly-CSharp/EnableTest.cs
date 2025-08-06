using UnityEngine;

public class EnableTest : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		MonoBehaviour.print("Enable!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + base.gameObject.name);
	}

	private void OnDisable()
	{
		MonoBehaviour.print("Disable!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + base.gameObject.name);
	}
}
