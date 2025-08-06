using UnityEngine;

public class StreetLight : MonoBehaviour
{
	public bool recycleFlag;

	public BuildingPool objPool;

	private void OnEnable()
	{
		if (base.GetComponent<Rigidbody>() != null)
		{
			Object.Destroy(base.GetComponent<Rigidbody>());
		}
		recycleFlag = false;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.relativeVelocity.sqrMagnitude > 64f)
		{
			if (base.GetComponent<Rigidbody>() == null)
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
			base.GetComponent<Rigidbody>().AddForceAtPosition(other.relativeVelocity * 30f, other.contacts[0].point);
			if (!recycleFlag)
			{
				recycleFlag = true;
				Invoke("RecycleDelay", 4f);
			}
			GlobalInf.dailyKnockDownLight++;
		}
	}

	private void RecycleDelay()
	{
		objPool.RecycleObj(base.gameObject);
	}
}
