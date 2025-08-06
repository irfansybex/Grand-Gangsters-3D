using UnityEngine;

public class MinimapLightLabelController : MonoBehaviour
{
	public static MinimapLightLabelController instance;

	public GameObject[] lightLabel;

	public MapLabelStarController[] lightLabel2;

	public GameObject[] lightLabelNew;

	public GameObject lightLabelRoot;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public void DisableLightLabel()
	{
		lightLabelRoot.SetActive(false);
	}

	public void EnableLightLabel()
	{
		lightLabelRoot.SetActive(true);
		if (!GlobalInf.newUserFlag)
		{
			if (GlobalInf.gameState < GameStateController.MAXSTATENUM)
			{
				for (int i = 0; i < GlobalInf.gameState; i++)
				{
					lightLabel[i].gameObject.SetActive(true);
				}
			}
			else
			{
				lightLabelRoot.gameObject.SetActiveRecursively(true);
			}
		}
		else if (GlobalInf.gameState < GameStateController.NEWMAXSTATENUM)
		{
			for (int j = 0; j < GlobalInf.gameState; j++)
			{
				lightLabelNew[j].gameObject.SetActive(true);
			}
		}
		else
		{
			lightLabelRoot.gameObject.SetActiveRecursively(true);
		}
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}
}
