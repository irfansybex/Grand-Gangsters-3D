using UnityEngine;

public class FakeLoadingBlocker : MonoBehaviour
{
	public UIEventListener btn;

	private void Start()
	{
		btn.onClick = OnClickBtn;
	}

	private void OnClickBtn(GameObject btn)
	{
		base.gameObject.SetActive(false);
	}
}
