using System.Collections.Generic;
using UnityEngine;

public class MyAnimaQue : MonoBehaviour
{
	public List<AnimationClip> queue;

	public Animation anima;

	private void Update()
	{
		if (queue.Count > 0 && anima[queue[0].name].normalizedTime > 0.55f)
		{
			queue.RemoveAt(0);
			if (queue.Count > 0)
			{
				anima.CrossFade(queue[0].name);
			}
		}
	}

	public void AddAnima(AnimationClip clip)
	{
		if (queue.Count > 0)
		{
			queue.Add(clip);
			return;
		}
		anima.CrossFade(clip.name);
		queue.Add(clip);
	}
}
