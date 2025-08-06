using UnityEngine;

public class AISideDetect : DetectRoot
{
	public int count;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Triger"))
		{
			count++;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Triger"))
		{
			count--;
		}
	}

	public override void RemoveDisappearObj(GameObject other)
	{
		count--;
	}
}
