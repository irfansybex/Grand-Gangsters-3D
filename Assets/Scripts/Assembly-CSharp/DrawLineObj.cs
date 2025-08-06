using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DrawLineObj : MonoBehaviour
{
	public List<CRSpline> crSpline;

	public List<MyCRSpline> myCRSpline;

	public List<RoadInfo> lineRoad;

	public Transform root;

	public bool runFlag;

	public bool useMyCRSplineFlag;

	private void Start()
	{
	}

	private void Update()
	{
		if (!runFlag)
		{
			return;
		}
		MonoBehaviour.print("run");
		runFlag = false;
		if (useMyCRSplineFlag)
		{
			myCRSpline.Clear();
			lineRoad.Clear();
			for (int i = 0; i < root.childCount; i++)
			{
				InitMySpline(root.GetChild(i));
			}
		}
		else
		{
			for (int j = 0; j < root.childCount; j++)
			{
				InitSpline(root.GetChild(j));
			}
		}
	}

	public void InitSpline(Transform childRoot)
	{
		if (childRoot.childCount < 4)
		{
			lineRoad.Add(childRoot.gameObject.GetComponent<RoadInfo>());
			return;
		}
		CRSpline cRSpline = new CRSpline();
		Vector3[] array = new Vector3[childRoot.childCount];
		for (int i = 0; i < childRoot.childCount; i++)
		{
			array[i] = childRoot.GetChild(i).transform.position;
		}
		cRSpline.pts = array;
		crSpline.Add(cRSpline);
	}

	public void InitMySpline(Transform childRoot)
	{
		if (childRoot.childCount < 4)
		{
			lineRoad.Add(childRoot.gameObject.GetComponent<RoadInfo>());
			return;
		}
		MyCRSpline myCRSpline = new MyCRSpline();
		Transform[] array = new Transform[childRoot.childCount];
		for (int i = 0; i < childRoot.childCount; i++)
		{
			array[i] = childRoot.GetChild(i).transform;
		}
		myCRSpline.pts = array;
		this.myCRSpline.Add(myCRSpline);
	}

	private void OnDrawGizmos()
	{
	}
}
