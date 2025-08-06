using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
	public Transform parent;

	public AIsystem_script aiscript;

	private void Start()
	{
		parent = base.transform.parent.parent;
		aiscript = parent.transform.GetComponent<AIsystem_script>();
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo("PlayerAI") == 0)
		{
			aiscript.isbrake = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag.CompareTo("PlayerAI") == 0)
		{
			aiscript.isbrake = false;
		}
	}
}
