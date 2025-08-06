using UnityEngine;

public class NGUIFitScreenEdit : EditorBase
{
	public UIWidget root;

	private UISprite tempSp;

	private UIStretch tempSt;

	private UIAnchor tempAn;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			NGUIFitScreen();
		}
	}

	public void NGUIFitScreen()
	{
		for (int i = 0; i < root.transform.childCount; i++)
		{
			Execute(root.transform.GetChild(i).gameObject);
		}
	}

	public void Execute(GameObject obj)
	{
		if (obj.GetComponent<BoxCollider>() != null && obj.GetComponent<NGUIColliderFitScreen>() == null && obj.GetComponent<BoxCollider>().bounds.size.x != obj.GetComponent<BoxCollider>().bounds.size.x)
		{
			obj.AddComponent<NGUIColliderFitScreen>();
		}
		tempSp = obj.GetComponent<UISprite>();
		if (tempSp != null)
		{
			if (tempSp.width != tempSp.height && obj.GetComponent<UIStretch>() == null)
			{
				tempSt = obj.AddComponent<UIStretch>();
				tempSt.container = root.gameObject;
				tempSt.uiCamera = null;
				tempSt.relativeSize = new Vector2((float)tempSp.width / 800f, (float)tempSp.height / 480f);
				tempSt.style = UIStretch.Style.Both;
			}
			if (obj.GetComponent<UIAnchor>() == null && obj.GetComponent<MoveUI>() == null)
			{
				tempAn = obj.AddComponent<UIAnchor>();
				tempAn.relativeOffset = new Vector2(obj.transform.localPosition.x / 800f, obj.transform.localPosition.y / 480f);
				tempAn.container = root.gameObject;
				tempAn.side = UIAnchor.Side.Center;
			}
		}
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			Execute(obj.transform.GetChild(i).gameObject);
		}
	}
}
