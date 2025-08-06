using UnityEngine;

public class testLeftRight : MonoBehaviour
{
	public AnimationClip left;

	public AnimationClip right;

	private void Start()
	{
		base.GetComponent<Animation>()[left.name].layer = 1;
		base.GetComponent<Animation>()[right.name].layer = 1;
		base.GetComponent<Animation>().Play(left.name);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (base.GetComponent<Animation>().IsPlaying(right.name))
			{
				base.GetComponent<Animation>().CrossFade(left.name, 0.3f, PlayMode.StopAll);
			}
			else if (base.GetComponent<Animation>().IsPlaying(left.name))
			{
				base.GetComponent<Animation>().CrossFade(right.name, 0.3f, PlayMode.StopAll);
			}
		}
	}
}
