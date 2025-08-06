using UnityEngine;

public class MapLabelStarController : MonoBehaviour
{
	public GameObject[] stars;

	public GameObject[] starBacks;

	public GAMEMODE mode;

	public int index;

	private void Star()
	{
		DisableStars();
	}

	public void DisableStars()
	{
		for (int i = 0; i < stars.Length; i++)
		{
			stars[i].gameObject.SetActiveRecursively(false);
			starBacks[i].gameObject.SetActiveRecursively(false);
		}
	}

	public void EnableStars(int num)
	{
		for (int i = 0; i < stars.Length; i++)
		{
			if (i < num)
			{
				stars[i].gameObject.SetActiveRecursively(true);
				starBacks[i].gameObject.SetActiveRecursively(false);
			}
			else
			{
				stars[i].gameObject.SetActiveRecursively(false);
				starBacks[i].gameObject.SetActiveRecursively(true);
			}
		}
	}
}
