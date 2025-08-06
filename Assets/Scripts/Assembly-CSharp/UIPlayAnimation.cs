using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Play Animation")]
public class UIPlayAnimation : MonoBehaviour
{
	public Animation target;

	public string clipName;

	public Trigger trigger;

	public Direction playDirection = Direction.Forward;

	public bool resetOnPlay;

	public bool clearSelection;

	public EnableCondition ifDisabledOnPlay;

	public DisableCondition disableWhenFinished;

	public List<EventDelegate> onFinished = new List<EventDelegate>();

	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	[SerializeField]
	[HideInInspector]
	private string callWhenFinished;

	private bool mStarted;

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
		if (target == null)
		{
			target = GetComponentInChildren<Animation>();
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

	public void Play(bool forward)
	{
		if (!target)
		{
			return;
		}
		if (clearSelection && UICamera.selectedObject == base.gameObject)
		{
			UICamera.selectedObject = null;
		}
		int num = 0 - playDirection;
		Direction direction = ((!forward) ? ((Direction)num) : playDirection);
		ActiveAnimation activeAnimation = ActiveAnimation.Play(target, clipName, direction, ifDisabledOnPlay, disableWhenFinished);
		if (activeAnimation != null)
		{
			if (resetOnPlay)
			{
				activeAnimation.Reset();
			}
			for (int i = 0; i < onFinished.Count; i++)
			{
				EventDelegate.Add(activeAnimation.onFinished, OnFinished, true);
			}
		}
	}

	private void OnFinished()
	{
		EventDelegate.Execute(onFinished);
		if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
		{
			eventReceiver.SendMessage(callWhenFinished, SendMessageOptions.DontRequireReceiver);
		}
		eventReceiver = null;
	}
}
