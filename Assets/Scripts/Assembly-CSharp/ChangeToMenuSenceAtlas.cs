using UnityEngine;

[ExecuteInEditMode]
public class ChangeToMenuSenceAtlas : MonoBehaviour
{
	public GameObject objRoot;

	public bool runFlag;

	public UIAtlas atlas;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Run();
			MonoBehaviour.print("run");
		}
	}

	public void Run()
	{
		Fun(objRoot.transform);
	}

	public void Fun(Transform root)
	{
		for (int i = 0; i < root.childCount; i++)
		{
			GameObject gameObject = root.GetChild(i).gameObject;
			UISprite component = gameObject.GetComponent<UISprite>();
			if (component != null && component.atlas != atlas)
			{
				component.atlas = atlas;
			}
			Fun(gameObject.transform);
		}
	}
}
