using UnityEngine;

public class GunParticles : MonoBehaviour
{
	//public ParticleEmitter[] bulletEmitter;

	public bool curState;

	private void Start()
	{
	//	bulletEmitter = GetComponentsInChildren<ParticleEmitter>();
	}

	public void ChangeState(bool newState)
	{
		if (curState != newState)
		{
			curState = newState;
			/*for (int i = 0; i < bulletEmitter.Length; i++)
			{
				bulletEmitter[i].emit = curState;
			}*/
		}
	}
}
