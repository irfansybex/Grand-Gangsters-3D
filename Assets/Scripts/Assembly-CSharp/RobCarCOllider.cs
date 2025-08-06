using UnityEngine;

public class RobCarCOllider : MonoBehaviour
{
	public RobCarModeController robCarMode;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			base.gameObject.SetActiveRecursively(false);
			GameController.instance.robbingCarMode.OnEnterCar();
			GameUIController.instance.DisableLocateLabel();
		}
	}
}
