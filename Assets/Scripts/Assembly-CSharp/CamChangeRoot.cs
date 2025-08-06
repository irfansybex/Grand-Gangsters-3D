using UnityEngine;

public class CamChangeRoot : MonoBehaviour
{
	public Transform targetRoot;

	public Transform preRoot;

	public Vector3 startPos;

	public Quaternion startRot;

	public bool startFlag;

	public float count;

	private float changeRate;

	private void Start()
	{
	}

	private void Update()
	{
		if (startFlag)
		{
			count += Time.deltaTime * changeRate;
			base.transform.localPosition = Vector3.Lerp(startPos, Vector3.zero, count);
			base.transform.localRotation = Quaternion.Lerp(startRot, Quaternion.identity, count);
			if (count > 1f)
			{
				startFlag = false;
			}
		}
	}

	public void StartChange(Transform tar, float t)
	{
		if (!(targetRoot == tar))
		{
			changeRate = 1f / t;
			preRoot = targetRoot;
			targetRoot = tar;
			base.transform.parent = targetRoot;
			startPos = base.transform.localPosition;
			startRot = base.transform.localRotation;
			count = 0f;
			startFlag = true;
		}
	}
}
