using UnityEngine;

public class NoTimeScaleParticle : MonoBehaviour
{
	private double lastTime;

	private ParticleSystem particle;

	private void Awake()
	{
		particle = GetComponent<ParticleSystem>();
	}

	private void Start()
	{
		lastTime = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		float t = Time.realtimeSinceStartup - (float)lastTime;
		particle.Simulate(t, false);
		lastTime = Time.realtimeSinceStartup;
	}
}
