using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Interaction/Scroll View")]
public class UIScrollView : MonoBehaviour
{
	public enum Movement
	{
		Horizontal,
		Vertical,
		Unrestricted,
		Custom
	}

	public enum DragEffect
	{
		None,
		Momentum,
		MomentumAndSpring
	}

	public enum ShowCondition
	{
		Always,
		OnlyIfNeeded,
		WhenDragging
	}

	public delegate void OnDragFinished();

	public Movement movement;

	public DragEffect dragEffect = DragEffect.MomentumAndSpring;

	public bool restrictWithinPanel = true;

	public bool disableDragIfFits;

	public bool smoothDragStart = true;

	public bool iOSDragEmulation = true;

	public float scrollWheelFactor = 0.25f;

	public float momentumAmount = 35f;

	public UIScrollBar horizontalScrollBar;

	public UIScrollBar verticalScrollBar;

	public ShowCondition showScrollBars = ShowCondition.OnlyIfNeeded;

	public Vector2 customMovement = new Vector2(1f, 0f);

	public Vector2 relativePositionOnReset = Vector2.zero;

	public OnDragFinished onDragFinished;

	[HideInInspector]
	[SerializeField]
	private Vector3 scale = new Vector3(1f, 0f, 0f);

	private Transform mTrans;

	private UIPanel mPanel;

	private Plane mPlane;

	private Vector3 mLastPos;

	private bool mPressed;

	private Vector3 mMomentum = Vector3.zero;

	private float mScroll;

	private Bounds mBounds;

	private bool mCalculatedBounds;

	private bool mShouldMove;

	private bool mIgnoreCallbacks;

	private int mDragID = -10;

	private Vector2 mDragStartOffset = Vector2.zero;

	private bool mDragStarted;

	public UIPanel panel
	{
		get
		{
			return mPanel;
		}
	}

	public Bounds bounds
	{
		get
		{
			if (!mCalculatedBounds)
			{
				mCalculatedBounds = true;
				mTrans = base.transform;
				mBounds = NGUIMath.CalculateRelativeWidgetBounds(mTrans, mTrans);
			}
			return mBounds;
		}
	}

	public bool canMoveHorizontally
	{
		get
		{
			return movement == Movement.Horizontal || movement == Movement.Unrestricted || (movement == Movement.Custom && customMovement.x != 0f);
		}
	}

	public bool canMoveVertically
	{
		get
		{
			return movement == Movement.Vertical || movement == Movement.Unrestricted || (movement == Movement.Custom && customMovement.y != 0f);
		}
	}

	public virtual bool shouldMoveHorizontally
	{
		get
		{
			float num = bounds.size.x;
			if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += mPanel.clipSoftness.x * 2f;
			}
			return num > mPanel.width;
		}
	}

	public virtual bool shouldMoveVertically
	{
		get
		{
			float num = bounds.size.y;
			if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += mPanel.clipSoftness.y * 2f;
			}
			return num > mPanel.height;
		}
	}

	protected virtual bool shouldMove
	{
		get
		{
			if (!disableDragIfFits)
			{
				return true;
			}
			if (mPanel == null)
			{
				mPanel = GetComponent<UIPanel>();
			}
			Vector4 finalClipRegion = mPanel.finalClipRegion;
			Bounds bounds = this.bounds;
			float num = ((finalClipRegion.z != 0f) ? (finalClipRegion.z * 0.5f) : ((float)Screen.width));
			float num2 = ((finalClipRegion.w != 0f) ? (finalClipRegion.w * 0.5f) : ((float)Screen.height));
			if (canMoveHorizontally)
			{
				if (bounds.min.x < finalClipRegion.x - num)
				{
					return true;
				}
				if (bounds.max.x > finalClipRegion.x + num)
				{
					return true;
				}
			}
			if (canMoveVertically)
			{
				if (bounds.min.y < finalClipRegion.y - num2)
				{
					return true;
				}
				if (bounds.max.y > finalClipRegion.y + num2)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Vector3 currentMomentum
	{
		get
		{
			return mMomentum;
		}
		set
		{
			mMomentum = value;
			mShouldMove = true;
		}
	}

	private void Awake()
	{
		mTrans = base.transform;
		mPanel = GetComponent<UIPanel>();
		if (mPanel.clipping == UIDrawCall.Clipping.None)
		{
			mPanel.clipping = UIDrawCall.Clipping.ConstrainButDontClip;
		}
		if (movement != Movement.Custom && scale.sqrMagnitude > 0.001f)
		{
			if (scale.x == 1f && scale.y == 0f)
			{
				movement = Movement.Horizontal;
			}
			else if (scale.x == 0f && scale.y == 1f)
			{
				movement = Movement.Vertical;
			}
			else if (scale.x == 1f && scale.y == 1f)
			{
				movement = Movement.Unrestricted;
			}
			else
			{
				movement = Movement.Custom;
				customMovement.x = scale.x;
				customMovement.y = scale.y;
			}
			scale = Vector3.zero;
		}
		if (Application.isPlaying)
		{
			UIPanel uIPanel = mPanel;
			uIPanel.onChange = (UIPanel.OnChangeDelegate)Delegate.Combine(uIPanel.onChange, new UIPanel.OnChangeDelegate(OnPanelChange));
		}
	}

	private void OnDestroy()
	{
		if (Application.isPlaying && mPanel != null)
		{
			UIPanel uIPanel = mPanel;
			uIPanel.onChange = (UIPanel.OnChangeDelegate)Delegate.Remove(uIPanel.onChange, new UIPanel.OnChangeDelegate(OnPanelChange));
		}
	}

	private void OnPanelChange()
	{
		UpdateScrollbars(true);
	}

	private void Start()
	{
		if (Application.isPlaying)
		{
			UpdateScrollbars(true);
			if (horizontalScrollBar != null)
			{
				EventDelegate.Add(horizontalScrollBar.onChange, OnHorizontalBar);
				horizontalScrollBar.alpha = ((showScrollBars != 0 && !shouldMoveHorizontally) ? 0f : 1f);
			}
			if (verticalScrollBar != null)
			{
				EventDelegate.Add(verticalScrollBar.onChange, OnVerticalBar);
				verticalScrollBar.alpha = ((showScrollBars != 0 && !shouldMoveVertically) ? 0f : 1f);
			}
		}
	}

	public bool RestrictWithinBounds(bool instant)
	{
		return RestrictWithinBounds(instant, true, true);
	}

	public bool RestrictWithinBounds(bool instant, bool horizontal, bool vertical)
	{
		Bounds bounds = this.bounds;
		Vector3 vector = mPanel.CalculateConstrainOffset(bounds.min, bounds.max);
		if (!horizontal)
		{
			vector.x = 0f;
		}
		if (!vertical)
		{
			vector.y = 0f;
		}
		if (vector.magnitude > 1f)
		{
			if (!instant && dragEffect == DragEffect.MomentumAndSpring)
			{
				Vector3 pos = mTrans.localPosition + vector;
				pos.x = Mathf.Round(pos.x);
				pos.y = Mathf.Round(pos.y);
				SpringPanel.Begin(mPanel.gameObject, pos, 13f);
			}
			else
			{
				MoveRelative(vector);
				mMomentum = Vector3.zero;
				mScroll = 0f;
			}
			return true;
		}
		return false;
	}

	public void DisableSpring()
	{
		SpringPanel component = GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	public virtual void UpdateScrollbars(bool recalculateBounds)
	{
		if (mPanel == null)
		{
			return;
		}
		if (horizontalScrollBar != null || verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				mCalculatedBounds = false;
				mShouldMove = shouldMove;
			}
			Bounds bounds = this.bounds;
			Vector2 vector = bounds.min;
			Vector2 vector2 = bounds.max;
			if (horizontalScrollBar != null && vector2.x > vector.x)
			{
				Vector4 finalClipRegion = mPanel.finalClipRegion;
				float num = finalClipRegion.z * 0.5f;
				if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num -= mPanel.clipSoftness.x;
				}
				float num2 = finalClipRegion.x - num - bounds.min.x;
				float num3 = bounds.max.x - num - finalClipRegion.x;
				float num4 = vector2.x - vector.x;
				num2 = Mathf.Clamp01(num2 / num4);
				num3 = Mathf.Clamp01(num3 / num4);
				float num5 = num2 + num3;
				mIgnoreCallbacks = true;
				horizontalScrollBar.barSize = 1f - num5;
				horizontalScrollBar.value = ((!(num5 > 0.001f)) ? 0f : (num2 / num5));
				mIgnoreCallbacks = false;
			}
			if (verticalScrollBar != null && vector2.y > vector.y)
			{
				Vector4 finalClipRegion2 = mPanel.finalClipRegion;
				float num6 = finalClipRegion2.w * 0.5f;
				if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num6 -= mPanel.clipSoftness.y;
				}
				float num7 = finalClipRegion2.y - num6 - vector.y;
				float num8 = vector2.y - num6 - finalClipRegion2.y;
				float num9 = vector2.y - vector.y;
				num7 = Mathf.Clamp01(num7 / num9);
				num8 = Mathf.Clamp01(num8 / num9);
				float num10 = num7 + num8;
				mIgnoreCallbacks = true;
				verticalScrollBar.barSize = 1f - num10;
				verticalScrollBar.value = ((!(num10 > 0.001f)) ? 0f : (1f - num7 / num10));
				mIgnoreCallbacks = false;
			}
		}
		else if (recalculateBounds)
		{
			mCalculatedBounds = false;
		}
	}

	public virtual void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		DisableSpring();
		Bounds bounds = this.bounds;
		if (bounds.min.x == bounds.max.x || bounds.min.y == bounds.max.y)
		{
			return;
		}
		Vector4 finalClipRegion = mPanel.finalClipRegion;
		finalClipRegion.x = Mathf.Round(finalClipRegion.x);
		finalClipRegion.y = Mathf.Round(finalClipRegion.y);
		finalClipRegion.z = Mathf.Round(finalClipRegion.z);
		finalClipRegion.w = Mathf.Round(finalClipRegion.w);
		float num = finalClipRegion.z * 0.5f;
		float num2 = finalClipRegion.w * 0.5f;
		float num3 = bounds.min.x + num;
		float num4 = bounds.max.x - num;
		float num5 = bounds.min.y + num2;
		float num6 = bounds.max.y - num2;
		if (mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			num3 -= mPanel.clipSoftness.x;
			num4 += mPanel.clipSoftness.x;
			num5 -= mPanel.clipSoftness.y;
			num6 += mPanel.clipSoftness.y;
		}
		float f = Mathf.Lerp(num3, num4, x);
		float f2 = Mathf.Lerp(num6, num5, y);
		f = Mathf.Round(f);
		f2 = Mathf.Round(f2);
		if (!updateScrollbars)
		{
			Vector3 localPosition = mTrans.localPosition;
			if (canMoveHorizontally)
			{
				localPosition.x += finalClipRegion.x - f;
			}
			if (canMoveVertically)
			{
				localPosition.y += finalClipRegion.y - f2;
			}
			mTrans.localPosition = localPosition;
		}
		if (canMoveHorizontally)
		{
			finalClipRegion.x = f;
		}
		if (canMoveVertically)
		{
			finalClipRegion.y = f2;
		}
		Vector4 baseClipRegion = mPanel.baseClipRegion;
		mPanel.clipOffset = new Vector2(finalClipRegion.x - baseClipRegion.x, finalClipRegion.y - baseClipRegion.y);
		if (updateScrollbars)
		{
			UpdateScrollbars(false);
		}
	}

	[ContextMenu("Reset Clipping Position")]
	public void ResetPosition()
	{
		if (NGUITools.GetActive(this))
		{
			mCalculatedBounds = false;
			SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, false);
			SetDragAmount(relativePositionOnReset.x, relativePositionOnReset.y, true);
		}
	}

	private void OnHorizontalBar()
	{
		if (!mIgnoreCallbacks)
		{
			float x = ((!(horizontalScrollBar != null)) ? 0f : horizontalScrollBar.value);
			float y = ((!(verticalScrollBar != null)) ? 0f : verticalScrollBar.value);
			SetDragAmount(x, y, false);
		}
	}

	private void OnVerticalBar()
	{
		if (!mIgnoreCallbacks)
		{
			float x = ((!(horizontalScrollBar != null)) ? 0f : horizontalScrollBar.value);
			float y = ((!(verticalScrollBar != null)) ? 0f : verticalScrollBar.value);
			SetDragAmount(x, y, false);
		}
	}

	public virtual void MoveRelative(Vector3 relative)
	{
		mTrans.localPosition += relative;
		Vector2 clipOffset = mPanel.clipOffset;
		clipOffset.x -= relative.x;
		clipOffset.y -= relative.y;
		mPanel.clipOffset = clipOffset;
		UpdateScrollbars(false);
	}

	public void MoveAbsolute(Vector3 absolute)
	{
		Vector3 vector = mTrans.InverseTransformPoint(absolute);
		Vector3 vector2 = mTrans.InverseTransformPoint(Vector3.zero);
		MoveRelative(vector - vector2);
	}

	public void Press(bool pressed)
	{
		if (smoothDragStart && pressed)
		{
			mDragStarted = false;
			mDragStartOffset = Vector2.zero;
		}
		if (!base.enabled || !NGUITools.GetActive(base.gameObject))
		{
			return;
		}
		if (!pressed && mDragID == UICamera.currentTouchID)
		{
			mDragID = -10;
		}
		mCalculatedBounds = false;
		mShouldMove = shouldMove;
		if (!mShouldMove)
		{
			return;
		}
		mPressed = pressed;
		if (pressed)
		{
			mMomentum = Vector3.zero;
			mScroll = 0f;
			DisableSpring();
			mLastPos = UICamera.lastHit.point;
			mPlane = new Plane(mTrans.rotation * Vector3.back, mLastPos);
			Vector2 clipOffset = mPanel.clipOffset;
			clipOffset.x = Mathf.Round(clipOffset.x);
			clipOffset.y = Mathf.Round(clipOffset.y);
			mPanel.clipOffset = clipOffset;
			Vector3 localPosition = mTrans.localPosition;
			localPosition.x = Mathf.Round(localPosition.x);
			localPosition.y = Mathf.Round(localPosition.y);
			mTrans.localPosition = localPosition;
		}
		else
		{
			if (restrictWithinPanel && mPanel.clipping != 0 && dragEffect == DragEffect.MomentumAndSpring)
			{
				RestrictWithinBounds(false, canMoveHorizontally, canMoveVertically);
			}
			if ((!smoothDragStart || mDragStarted) && onDragFinished != null)
			{
				onDragFinished();
			}
		}
	}

	public void Drag()
	{
		if (!base.enabled || !NGUITools.GetActive(base.gameObject) || !mShouldMove)
		{
			return;
		}
		if (mDragID == -10)
		{
			mDragID = UICamera.currentTouchID;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		if (smoothDragStart && !mDragStarted)
		{
			mDragStarted = true;
			mDragStartOffset = UICamera.currentTouch.totalDelta;
		}
		Ray ray = ((!smoothDragStart) ? UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos - mDragStartOffset));
		float enter = 0f;
		if (!mPlane.Raycast(ray, out enter))
		{
			return;
		}
		Vector3 point = ray.GetPoint(enter);
		Vector3 vector = point - mLastPos;
		mLastPos = point;
		if (vector.x != 0f || vector.y != 0f)
		{
			vector = mTrans.InverseTransformDirection(vector);
			if (movement == Movement.Horizontal)
			{
				vector.y = 0f;
				vector.z = 0f;
			}
			else if (movement == Movement.Vertical)
			{
				vector.x = 0f;
				vector.z = 0f;
			}
			else if (movement == Movement.Unrestricted)
			{
				vector.z = 0f;
			}
			else
			{
				vector.Scale(customMovement);
			}
			vector = mTrans.TransformDirection(vector);
		}
		mMomentum = Vector3.Lerp(mMomentum, mMomentum + vector * (0.01f * momentumAmount), 0.67f);
		if (!iOSDragEmulation)
		{
			MoveAbsolute(vector);
		}
		else if (mPanel.CalculateConstrainOffset(bounds.min, bounds.max).magnitude > 1f)
		{
			MoveAbsolute(vector * 0.5f);
			mMomentum *= 0.5f;
		}
		else
		{
			MoveAbsolute(vector);
		}
		if (restrictWithinPanel && mPanel.clipping != 0 && dragEffect != DragEffect.MomentumAndSpring)
		{
			RestrictWithinBounds(true, canMoveHorizontally, canMoveVertically);
		}
	}

	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && scrollWheelFactor != 0f)
		{
			DisableSpring();
			mShouldMove = shouldMove;
			if (Mathf.Sign(mScroll) != Mathf.Sign(delta))
			{
				mScroll = 0f;
			}
			mScroll += delta * scrollWheelFactor;
		}
	}

	private void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		if (showScrollBars != 0)
		{
			bool flag = false;
			bool flag2 = false;
			if (showScrollBars != ShowCondition.WhenDragging || mDragID != -10 || mMomentum.magnitude > 0.01f)
			{
				flag = shouldMoveVertically;
				flag2 = shouldMoveHorizontally;
			}
			if ((bool)verticalScrollBar)
			{
				float alpha = verticalScrollBar.alpha;
				alpha += ((!flag) ? ((0f - deltaTime) * 3f) : (deltaTime * 6f));
				alpha = Mathf.Clamp01(alpha);
				if (verticalScrollBar.alpha != alpha)
				{
					verticalScrollBar.alpha = alpha;
				}
			}
			if ((bool)horizontalScrollBar)
			{
				float alpha2 = horizontalScrollBar.alpha;
				alpha2 += ((!flag2) ? ((0f - deltaTime) * 3f) : (deltaTime * 6f));
				alpha2 = Mathf.Clamp01(alpha2);
				if (horizontalScrollBar.alpha != alpha2)
				{
					horizontalScrollBar.alpha = alpha2;
				}
			}
		}
		if (mShouldMove && !mPressed)
		{
			if (movement == Movement.Horizontal || movement == Movement.Unrestricted)
			{
				mMomentum -= mTrans.TransformDirection(new Vector3(mScroll * 0.05f, 0f, 0f));
			}
			else if (movement == Movement.Vertical)
			{
				mMomentum -= mTrans.TransformDirection(new Vector3(0f, mScroll * 0.05f, 0f));
			}
			else
			{
				mMomentum -= mTrans.TransformDirection(new Vector3(mScroll * customMovement.x * 0.05f, mScroll * customMovement.y * 0.05f, 0f));
			}
			if (mMomentum.magnitude > 0.0001f)
			{
				mScroll = NGUIMath.SpringLerp(mScroll, 0f, 20f, deltaTime);
				Vector3 absolute = NGUIMath.SpringDampen(ref mMomentum, 9f, deltaTime);
				MoveAbsolute(absolute);
				if (restrictWithinPanel && mPanel.clipping != 0)
				{
					RestrictWithinBounds(false, canMoveHorizontally, canMoveVertically);
				}
				if (mMomentum.magnitude < 0.0001f && onDragFinished != null)
				{
					onDragFinished();
				}
				return;
			}
			mScroll = 0f;
			mMomentum = Vector3.zero;
		}
		else
		{
			mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref mMomentum, 9f, deltaTime);
	}
}
