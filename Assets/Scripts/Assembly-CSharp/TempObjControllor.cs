using UnityEngine;

public class TempObjControllor : MonoBehaviour
{
	public static TempObjControllor instance;

	public ParticleSystem explosion;

	public GameObject brokenCar;

	public ParticleSystem smoke;

	public ParticleSystem eatLightLabelParticle;

	public CarController curBrokenCar;

	public ParticleSystem carSpark;

	public CarController curSmokeCar;

	public HealthKitParticleObj healKitObj;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Destroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	public ParticleSystem GetExplosion()
	{
		return explosion;
	}

	public ParticleSystem GetSmoke(CarController curCar)
	{
		curSmokeCar = curCar;
		smoke.gameObject.SetActiveRecursively(true);
		return smoke;
	}

	public void RecycleSmoke()
	{
		smoke.transform.parent = base.transform;
		smoke.transform.localPosition = Vector3.zero;
		smoke.gameObject.SetActiveRecursively(false);
	}

	public GameObject GetBrokenCar(CarController c)
	{
		curBrokenCar = c;
		return brokenCar;
	}

	public ParticleSystem GetEatLightLabel()
	{
		eatLightLabelParticle.gameObject.SetActiveRecursively(true);
		return eatLightLabelParticle;
	}

	public void RecycleEatLightLabel()
	{
		eatLightLabelParticle.gameObject.SetActiveRecursively(false);
		eatLightLabelParticle.transform.parent = null;
		eatLightLabelParticle.transform.position = Vector3.zero;
	}

	public void GetHealthObj(Transform target)
	{
		healKitObj.target = target;
		healKitObj.gameObject.SetActiveRecursively(true);
		healKitObj.parSys.Play();
	}
}
