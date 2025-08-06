using UnityEngine;

public class HealthKitParticleObj : MonoBehaviour
{
	public Transform target;

	public ParticleSystem parSys;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.position = target.position;
		base.transform.eulerAngles = Vector3.zero;
	}
}
