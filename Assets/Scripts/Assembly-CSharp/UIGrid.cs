using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
	public enum Arrangement
	{
		Horizontal,
		Vertical
	}

	public delegate void OnReposition();

	public Arrangement arrangement;

	public int maxPerLine;

	public float cellWidth = 200f;

	public float cellHeight = 200f;

	public bool animateSmoothly;

	public bool sorted;

	public bool hideInactive = true;

	public OnReposition onReposition;

	private bool mStarted;

	private bool mReposition;

	private UIPanel mPanel;

	private UIScrollView mDrag;

	private bool mInitDone;

	public bool repositionNow
	{
		set
		{
			if (value)
			{
				mReposition = true;
				base.enabled = true;
			}
		}
	}

	private void Init()
	{
		mInitDone = true;
		mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		mDrag = NGUITools.FindInParents<UIScrollView>(base.gameObject);
	}

	private void Start()
	{
		if (!mInitDone)
		{
			Init();
		}
		mStarted = true;
		bool flag = animateSmoothly;
		animateSmoothly = false;
		Reposition();
		animateSmoothly = flag;
		base.enabled = false;
	}

	private void Update()
	{
		if (mReposition)
		{
			Reposition();
		}
		base.enabled = false;
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	[ContextMenu("Execute")]
	public void Reposition()
	{
		if (Application.isPlaying && !mStarted)
		{
			mReposition = true;
			return;
		}
		if (!mInitDone)
		{
			Init();
		}
		mReposition = false;
		Transform transform = base.transform;
		int num = 0;
		int num2 = 0;
		if (sorted)
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if ((bool)child && (!hideInactive || NGUITools.GetActive(child.gameObject)))
				{
					list.Add(child);
				}
			}
			list.Sort(SortByName);
			int j = 0;
			for (int count = list.Count; j < count; j++)
			{
				Transform transform2 = list[j];
				if (NGUITools.GetActive(transform2.gameObject) || !hideInactive)
				{
					float z = transform2.localPosition.z;
					Vector3 vector = ((arrangement != 0) ? new Vector3(cellWidth * (float)num2, (0f - cellHeight) * (float)num, z) : new Vector3(cellWidth * (float)num, (0f - cellHeight) * (float)num2, z));
					if (animateSmoothly && Application.isPlaying)
					{
						SpringPosition.Begin(transform2.gameObject, vector, 15f);
					}
					else
					{
						transform2.localPosition = vector;
					}
					if (++num >= maxPerLine && maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < transform.childCount; k++)
			{
				Transform child2 = transform.GetChild(k);
				if (NGUITools.GetActive(child2.gameObject) || !hideInactive)
				{
					float z2 = child2.localPosition.z;
					Vector3 vector2 = ((arrangement != 0) ? new Vector3(cellWidth * (float)num2, (0f - cellHeight) * (float)num, z2) : new Vector3(cellWidth * (float)num, (0f - cellHeight) * (float)num2, z2));
					if (animateSmoothly && Application.isPlaying)
					{
						SpringPosition.Begin(child2.gameObject, vector2, 15f);
					}
					else
					{
						child2.localPosition = vector2;
					}
					if (++num >= maxPerLine && maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
			}
		}
		if (mDrag != null)
		{
			mDrag.UpdateScrollbars(true);
			mDrag.RestrictWithinBounds(true);
		}
		else if (mPanel != null)
		{
			mPanel.ConstrainTargetToBounds(transform, true);
		}
		if (onReposition != null)
		{
			onReposition();
		}
	}
}
