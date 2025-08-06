using UnityEngine;

[ExecuteInEditMode]
public class CopyCollider : MonoBehaviour
{
	public bool runflag;

	public GameObject source;

	public GameObject target;

	private void Start()
	{
	}

	private void Update()
	{
		if (runflag)
		{
			runflag = false;
			Run();
		}
	}

	public void Run()
	{
		for (int i = 0; i < source.transform.childCount; i++)
		{
			for (int j = 0; j < source.transform.GetChild(i).childCount; j++)
			{
				for (int k = 0; k < source.transform.GetChild(i).GetChild(j).childCount; k++)
				{
					GameObject gameObject = (GameObject)Object.Instantiate(source.transform.GetChild(i).GetChild(j).GetChild(k)
						.gameObject);
						gameObject.name = source.transform.GetChild(i).GetChild(j).GetChild(k)
							.name;
						gameObject.transform.parent = target.transform.Find(source.transform.GetChild(i).name).Find(source.transform.GetChild(i).GetChild(j).name);
						gameObject.transform.localPosition = source.transform.GetChild(i).GetChild(j).GetChild(k)
							.localPosition;
						gameObject.transform.localRotation = source.transform.GetChild(i).GetChild(j).GetChild(k)
							.localRotation;
					}
				}
			}
		}
	}
