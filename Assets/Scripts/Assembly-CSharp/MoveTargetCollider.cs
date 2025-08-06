using UnityEngine;

public class MoveTargetCollider : MonoBehaviour
{
	public GameSenceTutorialController tutorialController;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			base.gameObject.SetActiveRecursively(false);
			tutorialController.OnClickContinueBtn(null);
		}
	}
}
