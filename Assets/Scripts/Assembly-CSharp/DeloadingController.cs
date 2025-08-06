using UnityEngine;

public class DeloadingController : MonoBehaviour
{
	//public GUIText progressTxt;

	public int showProgress;

	public UISprite valLine;

	public bool ff;

	private void Awake()
	{
		showProgress = GlobalInf.loadingProgress;
	}

	private void Update()
	{
		showProgress += 2;
		valLine.fillAmount = (float)showProgress / 100f;
		if (showProgress >= 100)
		{
			base.gameObject.SetActiveRecursively(false);
			Object.Destroy(base.gameObject);
			Resources.UnloadUnusedAssets();
		}
	}
}
