using System.Collections.Generic;
using UnityEngine;

public class MyPlayerRagdollController : MonoBehaviour
{
	public enum RagdollState
	{
		animated,
		ragdolled,
		blendToAnim
	}

	public class BodyPart
	{
		public Transform transform;

		public Vector3 storedPosition;

		public Quaternion storedRotation;
	}

	public delegate void GetUpDone();

	public GetUpDone getUpDone;

	public bool ragdollStateFlag;

	public float ragdollStateTimeCount;

	public AnimationClip upForward;

	public AnimationClip upBackward;

	public AnimationClip targetAnima;

	public bool blendDoneFlag;

	public bool floorFlag;

	public float startTime;

	public RagdollState state;

	public float ragdollToMecanimBlendTime = 0.5f;

	private float mecanimToGetUpTransitionTime = 0.05f;

	private float ragdollingEndTime = -100f;

	private Vector3 ragdolledHipPosition;

	private Vector3 ragdolledHeadPosition;

	private Vector3 ragdolledFeetPosition;

	private List<BodyPart> bodyParts = new List<BodyPart>();

	public Animation anim;

	public Transform leftFootTrans;

	public Transform rightFootTrans;

	public Transform headTrans;

	public Transform hipsTrans;

	public List<Rigidbody> rigidbodyList;

	public List<Collider> colliderList;

	public bool ragdolled
	{
		get
		{
			return state != RagdollState.animated;
		}
		set
		{
			if (value)
			{
				if (state == RagdollState.animated)
				{
					setKinematic(false);
					anim.Stop();
					state = RagdollState.ragdolled;
					GlobalInf.playerRagdollFlag = true;
					ragdollStateFlag = true;
					ragdollStateTimeCount = 0f;
					floorFlag = false;
				}
			}
			else
			{
				if (state != RagdollState.ragdolled)
				{
					return;
				}
				base.GetComponent<Rigidbody>().velocity = Vector3.zero;
				base.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				setKinematic(true);
				ragdollingEndTime = Time.time;
				anim.enabled = true;
				state = RagdollState.blendToAnim;
				foreach (BodyPart bodyPart in bodyParts)
				{
					bodyPart.storedRotation = bodyPart.transform.rotation;
					bodyPart.storedPosition = bodyPart.transform.position;
				}
				ragdolledFeetPosition = 0.5f * (leftFootTrans.position + rightFootTrans.position);
				ragdolledHeadPosition = headTrans.position;
				ragdolledHipPosition = hipsTrans.position;
				if (hipsTrans.up.y > 0f)
				{
					startTime = Time.time;
					targetAnima = upForward;
					anim.Play(upForward.name);
				}
				else
				{
					startTime = Time.time;
					targetAnima = upBackward;
					anim.Play(upBackward.name);
				}
				ragdollStateFlag = false;
				blendDoneFlag = false;
			}
		}
	}

	public void setKinematic(bool newValue)
	{
		for (int i = 0; i < colliderList.Count; i++)
		{
			colliderList[i].enabled = !newValue;
		}
		for (int j = 0; j < rigidbodyList.Count; j++)
		{
			rigidbodyList[j].isKinematic = newValue;
			rigidbodyList[j].useGravity = !newValue;
		}
	}

	private void Start()
	{
		GlobalInf.playerRagdollFlag = false;
		FindRigidbodyInChild(anim.transform);
		setKinematic(true);
		FindBodyParts(anim.transform.GetChild(0));
	}

	public void FindBodyParts(Transform root)
	{
		BodyPart bodyPart = new BodyPart();
		bodyPart.transform = root;
		bodyParts.Add(bodyPart);
		for (int i = 0; i < root.childCount; i++)
		{
			FindBodyParts(root.GetChild(i));
		}
	}

	public void FindRigidbodyInChild(Transform root)
	{
		if (root.gameObject.GetComponent<Rigidbody>() != null)
		{
			rigidbodyList.Add(root.gameObject.GetComponent<Rigidbody>());
		}
		if (root.gameObject.GetComponent<Collider>() != null)
		{
			colliderList.Add(root.gameObject.GetComponent<Collider>());
		}
		for (int i = 0; i < root.childCount; i++)
		{
			FindRigidbodyInChild(root.GetChild(i));
		}
	}

	private void Update()
	{
		if (ragdollStateFlag)
		{
			ragdollStateTimeCount += Time.deltaTime;
			if (ragdollStateTimeCount > 3f && floorFlag)
			{
				ragdolled = false;
			}
			if (ragdollStateTimeCount > 7f)
			{
				ragdolled = false;
			}
		}
		if (blendDoneFlag && Time.time - startTime > anim[targetAnima.name].length - 0.1f)
		{
			blendDoneFlag = false;
			if (getUpDone != null)
			{
				getUpDone();
			}
		}
	}

	private void LateUpdate()
	{
		if (state != RagdollState.blendToAnim)
		{
			return;
		}
		if (Time.time <= ragdollingEndTime + mecanimToGetUpTransitionTime)
		{
			Vector3 vector = ragdolledHipPosition - hipsTrans.position;
			Vector3 vector2 = base.transform.position + vector;
			RaycastHit[] array = Physics.RaycastAll(new Ray(vector2, Vector3.down));
			vector2.y = 0f;
			RaycastHit[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				RaycastHit raycastHit = array2[i];
				if (!raycastHit.transform.IsChildOf(base.transform))
				{
					vector2.y = Mathf.Max(vector2.y, raycastHit.point.y);
				}
			}
			base.transform.position = vector2;
			Vector3 vector3 = ragdolledHeadPosition - ragdolledFeetPosition;
			vector3.y = 0f;
			Vector3 vector4 = 0.5f * (leftFootTrans.position + rightFootTrans.position);
			Vector3 vector5 = headTrans.position - vector4;
			vector5.y = 0f;
			base.transform.rotation *= Quaternion.FromToRotation(vector5.normalized, vector3.normalized);
		}
		float value = 1f - (Time.time - ragdollingEndTime - mecanimToGetUpTransitionTime) / ragdollToMecanimBlendTime;
		value = Mathf.Clamp01(value);
		foreach (BodyPart bodyPart in bodyParts)
		{
			if (bodyPart.transform != base.transform)
			{
				if (bodyPart.transform == hipsTrans)
				{
					bodyPart.transform.position = Vector3.Lerp(bodyPart.transform.position, bodyPart.storedPosition, value);
				}
				bodyPart.transform.rotation = Quaternion.Slerp(bodyPart.transform.rotation, bodyPart.storedRotation, value);
			}
		}
		if (value == 0f)
		{
			blendDoneFlag = true;
			state = RagdollState.animated;
			PlayerController.instance.cam.OnChangeTarget(false);
			GlobalInf.playerRagdollFlag = false;
		}
	}
}
