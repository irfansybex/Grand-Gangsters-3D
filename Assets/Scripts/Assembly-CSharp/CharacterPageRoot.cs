using UnityEngine;

public class CharacterPageRoot : MonoBehaviour
{
	public MENUUISTATE uiType;

	public GameObject pageRoot;

	public void OnEnable()
	{
		if (pageRoot != null)
		{
			pageRoot.gameObject.SetActiveRecursively(true);
		}
	}

	public void OnDisable()
	{
		if (pageRoot != null)
		{
			pageRoot.gameObject.SetActiveRecursively(false);
		}
	}
}
