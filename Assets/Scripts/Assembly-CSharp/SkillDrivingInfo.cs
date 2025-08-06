using UnityEngine;

public class SkillDrivingInfo : MonoBehaviour
{
	public GameObject stateLabelListRoot;

	public SkilldrivingStateLabel[] stateLabelList;

	public SkillDrivingFinalObj finalObj;

	public Vector3 startPos;

	public Vector3 startAngle;

	private void Awake()
	{
		stateLabelListRoot.gameObject.SetActive(true);
		for (int i = 0; i < stateLabelList.Length; i++)
		{
			stateLabelList[i].gameObject.SetActive(false);
		}
		stateLabelList[0].gameObject.SetActive(true);
		finalObj.gameObject.SetActive(false);
	}
}
