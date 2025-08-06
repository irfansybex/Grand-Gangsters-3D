using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Play Tween")]
[ExecuteInEditMode]
public class UIPlayTween : MonoBehaviour
{
	public GameObject tweenTarget;

	public int tweenGroup;

	public Trigger trigger;

	public Direction playDirection = Direction.Forward;

	public bool resetOnPlay;

	public bool resetIfDisabled;

	public EnableCondition ifDisabledOnPlay;

	public DisableCondition disableWhenFinished;

	public bool includeChildren;

	public List<EventDelegate> onFinished = new List<EventDelegate>();

	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	[HideInInspector]
	[SerializeField]
	private string callWhenFinished;

	private UITweener[] mTweens;

	private bool mStarted;

	private int mActive;

	private bool mActivated;

	private void Awake()
	{
		if (eventReceiver != null && EventDelegate.IsValid(onFinished))
		{
			eventReceiver = null;
			callWhenFinished = null;
		}
	}

	private void Start()
	{
		mStarted = true;
		if (tweenTarget == null)
		{
			tweenTarget = base.gameObject;
		}
	}

	private void OnEnable()
	{
		if (mStarted)
		{
			OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled && (trigger == Trigger.OnHover || (trigger == Trigger.OnHoverTrue && isOver) || (trigger == Trigger.OnHoverFalse && !isOver)))
		{
			mActivated = isOver && trigger == Trigger.OnHover;
			Play(isOver);
		}
	}

	private void OnDragOut()
	{
		if (base.enabled && mActivated)
		{
			mActivated = false;
			Play(false);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && (trigger == Trigger.OnPress || (trigger == Trigger.OnPressTrue && isPressed) || (trigger == Trigger.OnPressFalse && !isPressed)))
		{
			mActivated = isPressed && trigger == Trigger.OnPress;
			Play(isPressed);
		}
	}

	private void OnClick()
	{
		if (base.enabled && trigger == Trigger.OnClick)
		{
			Play(true);
		}
	}

	private void OnDoubleClick()
	{
		if (base.enabled && trigger == Trigger.OnDoubleClick)
		{
			Play(true);
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (trigger == Trigger.OnSelect || (trigger == Trigger.OnSelectTrue && isSelected) || (trigger == Trigger.OnSelectFalse && !isSelected)))
		{
			mActivated = isSelected && trigger == Trigger.OnSelect;
			Play(isSelected);
		}
	}

	private void OnActivate(bool isActive)
	{
		if (base.enabled && (trigger == Trigger.OnActivate || (trigger == Trigger.OnActivateTrue && isActive) || (trigger == Trigger.OnActivateFalse && !isActive)))
		{
			Play(isActive);
		}
	}

	private void Update()
	{
		if (disableWhenFinished == DisableCondition.DoNotDisable || mTweens == null)
		{
			return;
		}
		bool flag = true;
		bool flag2 = true;
		int i = 0;
		for (int num = mTweens.Length; i < num; i++)
		{
			UITweener uITweener = mTweens[i];
			if (uITweener.tweenGroup == tweenGroup)
			{
				if (uITweener.enabled)
				{
					flag = false;
					break;
				}
				if (uITweener.direction != (Direction)disableWhenFinished)
				{
					flag2 = false;
				}
			}
		}
		if (flag)
		{
			if (flag2)
			{
				NGUITools.SetActiveRecursively(tweenTarget, false);
			}
			mTweens = null;
		}
	}

	public void Play(bool forward)
	{
		mActive = 0;
		GameObject gameObject = ((!(tweenTarget == null)) ? tweenTarget : base.gameObject);
		if (!NGUITools.GetActive(gameObject))
		{
			if (ifDisabledOnPlay != EnableCondition.EnableThenPlay)
			{
				return;
			}
			NGUITools.SetActiveRecursively(gameObject, true);
		}
		mTweens = ((!includeChildren) ? gameObject.GetComponents<UITweener>() : gameObject.GetComponentsInChildren<UITweener>());
		if (mTweens.Length == 0)
		{
			if (disableWhenFinished != 0)
			{
				NGUITools.SetActiveRecursively(tweenTarget, false);
			}
			return;
		}
		bool flag = false;
		if (playDirection == Direction.Reverse)
		{
			forward = !forward;
		}
		int i = 0;
		for (int num = mTweens.Length; i < num; i++)
		{
			UITweener uITweener = mTweens[i];
			if (uITweener.tweenGroup != tweenGroup)
			{
				continue;
			}
			if (!flag && !NGUITools.GetActive(gameObject))
			{
				flag = true;
				NGUITools.SetActiveRecursively(gameObject, true);
			}
			mActive++;
			if (playDirection == Direction.Toggle)
			{
				uITweener.Toggle();
			}
			else
			{
				if (resetOnPlay || (resetIfDisabled && !uITweener.enabled))
				{
					uITweener.ResetToBeginning();
				}
				uITweener.Play(forward);
			}
			EventDelegate.Add(uITweener.onFinished, OnFinished, true);
		}
	}

	private void OnFinished()
	{
		if (--mActive == 0)
		{
			EventDelegate.Execute(onFinished);
			if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
			{
				eventReceiver.SendMessage(callWhenFinished, SendMessageOptions.DontRequireReceiver);
			}
			eventReceiver = null;
		}
	}
}
