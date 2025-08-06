using UnityEngine;

public class AICarForwardCheck : DetectRoot
{
	public AIsystem_script aiCar;

	public int count;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Triger") && other.gameObject.layer != LayerMask.NameToLayer("Floor") && other.gameObject.layer != LayerMask.NameToLayer("Ragdoll") && other.gameObject.layer != LayerMask.NameToLayer("DodgeArea"))
		{
			count++;
			aiCar.isbrake = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer("Triger") && other.gameObject.layer != LayerMask.NameToLayer("Floor") && other.gameObject.layer != LayerMask.NameToLayer("Ragdoll") && other.gameObject.layer != LayerMask.NameToLayer("DodgeArea"))
		{
			count--;
			if (count == 0)
			{
				aiCar.isbrake = false;
			}
		}
	}

	public override void RemoveDisappearObj(GameObject other)
	{
		count--;
		if (count == 0)
		{
			aiCar.isbrake = false;
		}
	}
}
