using UnityEngine;

public class TaskBoxUIController : MonoBehaviour
{
	public UILabel taskModeLabel;

	public GameObject[] starList;

	public GameObject[] emptyStarList;

	public BoxCollider okUIBtn;

	public UIEventListener okBtn;

	public void OnEnableOKBtn()
	{
		okUIBtn.enabled = true;
	}

	public void OnDisableOKBtn()
	{
		okUIBtn.enabled = false;
	}
}
