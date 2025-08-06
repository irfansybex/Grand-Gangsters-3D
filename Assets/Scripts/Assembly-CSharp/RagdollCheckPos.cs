using UnityEngine;

public class RagdollCheckPos : MonoBehaviour
{
	public MyPlayerRagdollController myPlayerRagdollController;

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Floor") || other.gameObject.layer == LayerMask.NameToLayer("Car"))
		{
			myPlayerRagdollController.floorFlag = true;
		}
	}
}
