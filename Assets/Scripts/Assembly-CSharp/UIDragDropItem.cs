using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
	public enum Restriction
	{
		None,
		Horizontal,
		Vertical,
		PressAndHold
	}

	public Restriction restriction;

	protected Transform mTrans;

	protected Transform mParent;

	protected Collider mCollider;

	protected UIRoot mRoot;

	protected UIGrid mGrid;

	protected UITable mTable;

	protected int mTouchID = int.MinValue;

	protected float mPressTime;

	protected UIDragScrollView mDragScrollView;

	protected virtual void Start()
	{
		mTrans = base.transform;
		mCollider = base.GetComponent<Collider>();
		mDragScrollView = GetComponent<UIDragScrollView>();
	}

	private void OnPress(bool isPressed)
	{
		if (isPressed)
		{
			mPressTime = RealTime.time;
		}
	}

	private void OnDragStart()
	{
		if (!base.enabled || mTouchID != int.MinValue)
		{
			return;
		}
		if (restriction != 0)
		{
			if (restriction == Restriction.Horizontal)
			{
				Vector2 totalDelta = UICamera.currentTouch.totalDelta;
				if (Mathf.Abs(totalDelta.x) < Mathf.Abs(totalDelta.y))
				{
					return;
				}
			}
			else if (restriction == Restriction.Vertical)
			{
				Vector2 totalDelta2 = UICamera.currentTouch.totalDelta;
				if (Mathf.Abs(totalDelta2.x) > Mathf.Abs(totalDelta2.y))
				{
					return;
				}
			}
			else if (restriction == Restriction.PressAndHold && mPressTime + 1f > RealTime.time)
			{
				return;
			}
		}
		if (mDragScrollView != null)
		{
			mDragScrollView.enabled = false;
		}
		if (mCollider != null)
		{
			mCollider.enabled = false;
		}
		mTouchID = UICamera.currentTouchID;
		mParent = mTrans.parent;
		mRoot = NGUITools.FindInParents<UIRoot>(mParent);
		mGrid = NGUITools.FindInParents<UIGrid>(mParent);
		mTable = NGUITools.FindInParents<UITable>(mParent);
		if (UIDragDropRoot.root != null)
		{
			mTrans.parent = UIDragDropRoot.root;
		}
		Vector3 localPosition = mTrans.localPosition;
		localPosition.z = 0f;
		mTrans.localPosition = localPosition;
		NGUITools.MarkParentAsChanged(base.gameObject);
		OnDragDropStart();
	}

	private void OnDrag(Vector2 delta)
	{
		if (base.enabled && mTouchID == UICamera.currentTouchID)
		{
			OnDragDropMove((Vector3)delta * mRoot.pixelSizeAdjustment);
		}
	}

	private void OnDragEnd()
	{
		if (base.enabled && mTouchID == UICamera.currentTouchID)
		{
			mTouchID = int.MinValue;
			if (mCollider != null)
			{
				mCollider.enabled = true;
			}
			OnDragDropRelease(UICamera.hoveredObject);
			mParent = mTrans.parent;
			mGrid = NGUITools.FindInParents<UIGrid>(mParent);
			mTable = NGUITools.FindInParents<UITable>(mParent);
			if (mDragScrollView != null)
			{
				mDragScrollView.enabled = true;
			}
			NGUITools.MarkParentAsChanged(base.gameObject);
			OnDragDropEnd();
		}
	}

	protected virtual void OnDragDropStart()
	{
		if (mTable != null)
		{
			mTable.repositionNow = true;
		}
		if (mGrid != null)
		{
			mGrid.repositionNow = true;
		}
	}

	protected virtual void OnDragDropMove(Vector3 delta)
	{
		mTrans.localPosition += delta;
	}

	protected virtual void OnDragDropRelease(GameObject surface)
	{
		UIDragDropContainer uIDragDropContainer = ((!surface) ? null : NGUITools.FindInParents<UIDragDropContainer>(surface));
		if (uIDragDropContainer != null)
		{
			mTrans.parent = ((!(uIDragDropContainer.reparentTarget != null)) ? uIDragDropContainer.transform : uIDragDropContainer.reparentTarget);
			Vector3 localPosition = mTrans.localPosition;
			localPosition.z = 0f;
			mTrans.localPosition = localPosition;
		}
		else
		{
			mTrans.parent = mParent;
		}
	}

	protected virtual void OnDragDropEnd()
	{
		if (mTable != null)
		{
			mTable.repositionNow = true;
		}
		if (mGrid != null)
		{
			mGrid.repositionNow = true;
		}
	}
}
