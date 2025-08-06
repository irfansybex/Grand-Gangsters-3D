using System;
using UnityEngine;

public class HandGunMoveTest : MonoBehaviour
{
	public Rigidbody rigid;

	public Transform rootBone;

	public Transform upperBodyBone;

	public float maxIdleSpeed = 0.5f;

	public float minWalkSpeed = 2f;

	public AnimationClip idle;

	public AnimationClip turn;

	public AnimationClip shootAdditive;

	public MoveAnimation[] moveAnimations;

	private MoveAnimation preBestAnima;

	private Transform tr;

	private Vector3 lastPosition = Vector3.zero;

	private Vector3 velocity = Vector3.zero;

	private Vector3 localVelocity = Vector3.zero;

	private float speed;

	private float angle;

	private float lowerBodyDeltaAngle;

	public float idleWeight;

	private Vector3 lowerBodyForwardTarget = Vector3.forward;

	private Vector3 lowerBodyForward = Vector3.forward;

	private MoveAnimation bestAnimation;

	private float lastFootstepTime;

	private float lastAnimTime;

	public Animation animationComponent;

	private void Awake()
	{
	}

	private void FixedUpdate()
	{
		velocity = (tr.position - lastPosition) / Time.deltaTime;
		localVelocity = tr.InverseTransformDirection(velocity);
		localVelocity.y = 0f;
		speed = localVelocity.magnitude;
		angle = MoveAnimation.HorizontalAngle(localVelocity);
		lastPosition = tr.position;
	}

	private void Update()
	{
		idleWeight = Mathf.Lerp(idleWeight, Mathf.InverseLerp(minWalkSpeed, maxIdleSpeed, speed), Time.deltaTime * 10f);
		animationComponent[idle.name].weight = idleWeight;
		if (speed > 0f)
		{
			float num = float.PositiveInfinity;
			MoveAnimation[] array = moveAnimations;
			foreach (MoveAnimation moveAnimation in array)
			{
				float num2 = Mathf.Abs(Mathf.DeltaAngle(angle, moveAnimation.angle));
				float num3 = num2;
				if (moveAnimation == bestAnimation)
				{
					num3 *= 0.9f;
				}
				if (num3 < num)
				{
					bestAnimation = moveAnimation;
					num = num3;
				}
			}
			animationComponent.CrossFade(bestAnimation.clip.name);
		}
		else
		{
			bestAnimation = null;
		}
		if (lowerBodyForward != lowerBodyForwardTarget && idleWeight >= 0.9f)
		{
			animationComponent.CrossFade(turn.name, 0.05f);
		}
		preBestAnima = bestAnimation;
		if (bestAnimation != null && idleWeight < 0.9f)
		{
			float num4 = Mathf.Repeat(animationComponent[bestAnimation.clip.name].normalizedTime * 2f + 0.1f, 1f);
			if (num4 < lastAnimTime && Time.time > lastFootstepTime + 0.1f)
			{
				lastFootstepTime = Time.time;
			}
			lastAnimTime = num4;
		}
	}

	private void LateUpdate()
	{
		float num = Mathf.InverseLerp(minWalkSpeed, maxIdleSpeed, speed);
		if (num < 1f)
		{
			Vector3 zero = Vector3.zero;
			MoveAnimation[] array = moveAnimations;
			foreach (MoveAnimation moveAnimation in array)
			{
				if (animationComponent[moveAnimation.clip.name].weight != 0f && !(Vector3.Dot(moveAnimation.velocity, localVelocity) <= 0f))
				{
					zero += moveAnimation.velocity * animationComponent[moveAnimation.clip.name].weight;
				}
			}
			float b = Mathf.DeltaAngle(MoveAnimation.HorizontalAngle(tr.rotation * zero), MoveAnimation.HorizontalAngle(velocity));
			lowerBodyDeltaAngle = Mathf.LerpAngle(lowerBodyDeltaAngle, b, Time.deltaTime * 10f);
			lowerBodyForwardTarget = tr.forward;
			lowerBodyForward = Quaternion.Euler(0f, lowerBodyDeltaAngle, 0f) * lowerBodyForwardTarget;
		}
		else
		{
			lowerBodyForward = Vector3.RotateTowards(lowerBodyForward, lowerBodyForwardTarget, Time.deltaTime * 520f * ((float)Math.PI / 180f), 1f);
			lowerBodyDeltaAngle = Mathf.DeltaAngle(MoveAnimation.HorizontalAngle(tr.forward), MoveAnimation.HorizontalAngle(lowerBodyForward));
			if (Mathf.Abs(lowerBodyDeltaAngle) > 80f)
			{
				lowerBodyForwardTarget = tr.forward;
			}
		}
		Quaternion quaternion = Quaternion.Euler(0f, lowerBodyDeltaAngle, 0f);
		rootBone.rotation = quaternion * rootBone.rotation;
		upperBodyBone.rotation = Quaternion.Inverse(quaternion) * upperBodyBone.rotation;
	}
}
