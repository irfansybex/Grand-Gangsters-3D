using System;
using UnityEngine;

public class PlayerAnimaShot : MonoBehaviour
{
	public AnimationClip up;

	public AnimationClip down;

	public AnimationClip left;

	public AnimationClip right;

	public AnimationClip getGun;

	public Transform upArm;

	public Transform gunDriection;

	public LayerMask layer;

	public GunCtl gun;

	public bool shotFlag;

	public Transform head;

	public Transform shoulderForward;

	public Transform shoulder;

	public Transform upShoulder;

	public Transform downShoulder;

	private RaycastHit hit;

	private Quaternion defaultRotation;

	private Quaternion defaultHeadRotation;

	public bool flag;

	public bool defaultFlag;

	public Quaternion preHandQua;

	public Quaternion preShoulderQua;

	public Vector3 hitDir;

	public float yAngle;

	public float p;

	public Quaternion angle;

	public float lerpSpeed;

	public Quaternion preQuaternion;

	private void Awake()
	{
	}

	private void GetUpShoulderRotation(AnimationClip clip, Transform data)
	{
		base.GetComponent<Animation>()[clip.name].normalizedTime = 0.5f;
		base.GetComponent<Animation>()[clip.name].enabled = true;
		base.GetComponent<Animation>()[clip.name].weight = 1f;
		base.GetComponent<Animation>().Sample();
		data.transform.localRotation = shoulder.transform.localRotation;
		base.GetComponent<Animation>()[clip.name].enabled = false;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (!flag)
			{
				base.GetComponent<Animation>()[getGun.name].enabled = true;
				base.GetComponent<Animation>()[getGun.name].normalizedTime = 0f;
				base.GetComponent<Animation>().CrossFade("getHandGun");
				defaultFlag = true;
			}
			else
			{
				base.GetComponent<Animation>().CrossFade("idle0", 0.3f, PlayMode.StopAll);
			}
			flag = !flag;
		}
		if (flag)
		{
			if (base.GetComponent<Animation>()[getGun.name].normalizedTime >= 1f)
			{
				if (defaultFlag)
				{
					defaultFlag = false;
					defaultRotation = Quaternion.Inverse(base.transform.rotation) * upArm.rotation;
					defaultHeadRotation = Quaternion.Inverse(base.transform.rotation) * head.rotation;
					preQuaternion = head.rotation;
					preHandQua = upArm.transform.localRotation;
					preShoulderQua = shoulder.transform.rotation;
				}
				Shot();
			}
		}
		else
		{
			shotFlag = false;
		}
	}

	public void Shot()
	{
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit, 1000f, layer))
		{
			gun.Shot(hit);
		}
	}

	private void LateUpdate()
	{
		if (shotFlag)
		{
			head.rotation = preQuaternion;
			upArm.transform.localRotation = preHandQua;
			shoulder.transform.rotation = preShoulderQua;
			hitDir = hit.point - gunDriection.position;
			angle = Quaternion.LookRotation(hitDir);
			upArm.rotation = LerpToTarget(upArm.rotation, angle * defaultRotation);
			yAngle = Mathf.Asin(hitDir.y / hitDir.magnitude);
			p = Mathf.Clamp01((yAngle * 180f / (float)Math.PI + 30f) / 60f);
			shoulder.transform.localRotation = LerpToTarget(shoulder.transform.localRotation, Quaternion.Lerp(downShoulder.localRotation, upShoulder.localRotation, p));
			head.rotation = LerpToTarget(head.rotation, angle * defaultHeadRotation);
			preQuaternion = head.rotation;
			preHandQua = upArm.transform.localRotation;
			preShoulderQua = shoulder.transform.rotation;
		}
	}

	public Quaternion LerpToTarget(Quaternion sourceRotation, Quaternion targetRotation)
	{
		return Quaternion.Lerp(sourceRotation, targetRotation, Time.deltaTime * lerpSpeed);
	}
}
