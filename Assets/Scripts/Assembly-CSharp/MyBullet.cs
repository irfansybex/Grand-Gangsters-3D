using UnityEngine;

public class MyBullet : MonoBehaviour
{
	public ParticleSystem bulletHole;

	public ParticleSystem blood;

	public ParticleSystem spark;

	public float speed;

	public float arriveTime;

	public Transform startPoint;

	public GameObject bullet;

	private Ray forwardRay;

	private Ray backRay;

	private RaycastHit hit;

	private bool hitObj;

	public float maxLifeTime;

	private float tempLifeTime;

	public bool hitFlag;

	public bool recycleFlag;

	public float recycleTime;

	public Transform targetTra;

	public float bulletLength = 10f;

	private float shotTime;

	private bool showParticle;

	private bool hitPersonFlag;

	private bool hitMetalFlag;

	private float shotTimeCopy;

	public Transform m_transform;

	private void Awake()
	{
		m_transform = base.transform;
	}

	public void Init(Vector3 pos, Vector3 dir, float sp, Transform targetObj, RaycastHit hitInfo, bool pFlag, bool personFlag, bool metalFlag)
	{
		targetTra = targetObj;
		hit = hitInfo;
		bulletHole.gameObject.SetActiveRecursively(false);
		blood.gameObject.SetActiveRecursively(false);
		base.transform.position = pos;
		base.transform.forward = dir.normalized;
		hitFlag = false;
		recycleFlag = false;
		Vector3 normalized = (Camera.main.transform.position - base.transform.position).normalized;
		Vector3 vector = Vector3.Cross(base.transform.forward, normalized);
		vector = Vector3.Cross(base.transform.forward, -vector);
		base.transform.rotation = Quaternion.LookRotation(base.transform.forward, vector);
		tempLifeTime = maxLifeTime;
		if (targetTra != null)
		{
			shotTime = ((targetTra.position - pos).magnitude - bulletLength) / sp;
		}
		else
		{
			shotTime = ((hit.point - pos).magnitude - bulletLength) / sp;
		}
		shotTime -= 0.1f;
		if (shotTime <= 0f)
		{
			bullet.SetActiveRecursively(false);
		}
		shotTimeCopy = shotTime;
		speed = sp;
		showParticle = pFlag;
		hitPersonFlag = personFlag;
		hitMetalFlag = metalFlag;
	}

	private void Update()
	{
		if (!hitFlag)
		{
			if (shotTime <= 0f)
			{
				if (targetTra == null)
				{
					BulletHit(hit.point, hit.normal);
				}
				else
				{
					BulletHit(targetTra.position - m_transform.forward * 0.2f, -m_transform.forward);
				}
				return;
			}
			MoveBullet();
			tempLifeTime -= Time.deltaTime;
			if (tempLifeTime <= 0f)
			{
				hitFlag = true;
				recycleFlag = true;
				Recycle();
			}
			else
			{
				shotTime -= Time.deltaTime;
			}
		}
		else if (!recycleFlag)
		{
			tempLifeTime -= Time.deltaTime;
			if (tempLifeTime <= 0f)
			{
				Recycle();
			}
		}
	}

	public void Recycle()
	{
		recycleFlag = true;
		targetTra = null;
		base.gameObject.SetActiveRecursively(false);
		BulletPool.instance.RecycleBullet(this);
	}

	private void MoveBullet()
	{
		if (targetTra == null)
		{
			m_transform.position += speed * m_transform.forward * Time.deltaTime;
			return;
		}
		m_transform.position = Vector3.Lerp(m_transform.position, targetTra.position, (shotTimeCopy - shotTime) / shotTimeCopy);
		m_transform.LookAt(targetTra);
	}

	private void BulletHit(Vector3 pos, Vector3 dir)
	{
		if (showParticle)
		{
			if (!hitMetalFlag)
			{
				if (!GlobalDefine.smallPhoneFlag)
				{
					if (hitPersonFlag)
					{
						blood.transform.position = pos;
						blood.transform.rotation = Quaternion.FromToRotation(Vector3.forward, dir);
						blood.gameObject.SetActiveRecursively(true);
						blood.Play();
						if (AudioController.instance != null)
						{
							AudioController.instance.play(AudioType.HIT_HUMAN);
						}
					}
					else
					{
						bulletHole.transform.position = pos;
						bulletHole.transform.rotation = Quaternion.FromToRotation(Vector3.forward, dir);
						bulletHole.gameObject.SetActiveRecursively(true);
						bulletHole.Play();
						if (AudioController.instance != null)
						{
							AudioController.instance.play(AudioType.HIT_WALL);
						}
					}
				}
			}
			else if (!GlobalDefine.smallPhoneFlag)
			{
				spark.transform.position = pos;
				spark.transform.rotation = Quaternion.FromToRotation(Vector3.forward, dir);
				spark.gameObject.SetActiveRecursively(true);
				spark.Play();
				if (AudioController.instance != null)
				{
					AudioController.instance.play(AudioType.HIT_CAR);
				}
			}
		}
		bullet.SetActiveRecursively(false);
		hitFlag = true;
		tempLifeTime = recycleTime;
	}
}
