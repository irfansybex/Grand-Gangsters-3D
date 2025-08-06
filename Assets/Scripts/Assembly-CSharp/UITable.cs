using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : UIWidgetContainer
{
	public enum Direction
	{
		Down,
		Up
	}

	public delegate void OnReposition();

	public int columns;

	public Direction direction;

	public bool sorted;

	public bool hideInactive = true;

	public bool keepWithinPanel;

	public Vector2 padding = Vector2.zero;

	public OnReposition onReposition;

	private UIPanel mPanel;

	private UIScrollView mDrag;

	private bool mInitDone;

	private bool mReposition;

	private List<Transform> mChildren = new List<Transform>();

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

	public List<Transform> children
	{
		get
		{
			if (mChildren.Count == 0)
			{
				Transform transform = base.transform;
				mChildren.Clear();
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					if ((bool)child && (bool)child.gameObject && (!hideInactive || NGUITools.GetActive(child.gameObject)))
					{
						mChildren.Add(child);
					}
				}
				if (sorted)
				{
					mChildren.Sort(SortByName);
				}
			}
			return mChildren;
		}
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	private void RepositionVariableSize(List<Transform> children)
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = ((columns <= 0) ? 1 : (children.Count / columns + 1));
		int num4 = ((columns <= 0) ? children.Count : columns);
		Bounds[,] array = new Bounds[num3, num4];
		Bounds[] array2 = new Bounds[num4];
		Bounds[] array3 = new Bounds[num3];
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		for (int count = children.Count; i < count; i++)
		{
			Transform transform = children[i];
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform);
			Vector3 localScale = transform.localScale;
			bounds.min = Vector3.Scale(bounds.min, localScale);
			bounds.max = Vector3.Scale(bounds.max, localScale);
			array[num6, num5] = bounds;
			array2[num5].Encapsulate(bounds);
			array3[num6].Encapsulate(bounds);
			if (++num5 >= columns && columns > 0)
			{
				num5 = 0;
				num6++;
			}
		}
		num5 = 0;
		num6 = 0;
		int j = 0;
		for (int count2 = children.Count; j < count2; j++)
		{
			Transform transform2 = children[j];
			Bounds bounds2 = array[num6, num5];
			Bounds bounds3 = array2[num5];
			Bounds bounds4 = array3[num6];
			Vector3 localPosition = transform2.localPosition;
			localPosition.x = num + bounds2.extents.x - bounds2.center.x;
			localPosition.x += bounds2.min.x - bounds3.min.x + padding.x;
			if (direction == Direction.Down)
			{
				localPosition.y = 0f - num2 - bounds2.extents.y - bounds2.center.y;
				localPosition.y += (bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y) * 0.5f - padding.y;
			}
			else
			{
				localPosition.y = num2 + (bounds2.extents.y - bounds2.center.y);
				localPosition.y -= (bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y) * 0.5f - padding.y;
			}
			num += bounds3.max.x - bounds3.min.x + padding.x * 2f;
			transform2.localPosition = localPosition;
			if (++num5 >= columns && columns > 0)
			{
				num5 = 0;
				num6++;
				num = 0f;
				num2 += bounds4.size.y + padding.y * 2f;
			}
		}
	}

	[ContextMenu("Execute")]
	public void Reposition()
	{
		if (Application.isPlaying && !mInitDone)
		{
			mReposition = true;
			return;
		}
		if (!mInitDone)
		{
			Init();
		}
		mReposition = false;
		Transform target = base.transform;
		mChildren.Clear();
		List<Transform> list = children;
		if (list.Count > 0)
		{
			RepositionVariableSize(list);
		}
		if (mDrag != null)
		{
			mDrag.UpdateScrollbars(true);
			mDrag.RestrictWithinBounds(true);
		}
		else if (mPanel != null)
		{
			mPanel.ConstrainTargetToBounds(target, true);
		}
		if (onReposition != null)
		{
			onReposition();
		}
	}

	private void Start()
	{
		Init();
		Reposition();
		base.enabled = false;
	}

	private void Init()
	{
		mInitDone = true;
		if (keepWithinPanel)
		{
			mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			mDrag = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
	}

	private void LateUpdate()
	{
		if (mReposition)
		{
			Reposition();
		}
		base.enabled = false;
	}
}
