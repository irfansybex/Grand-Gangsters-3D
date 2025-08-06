using UnityEngine;

public class SkilldrivingStateLabel : MonoBehaviour
{
	public bool enterFlag;

	private void Start()
	{
		enterFlag = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (enterFlag)
		{
			enterFlag = false;
			base.gameObject.SetActiveRecursively(false);
			GameController.instance.skillDrivingMode.MoveState();
			TempObjControllor.instance.GetEatLightLabel().Play();
			AudioController.instance.play(AudioType.PICK_ITEM);
		}
	}
}
