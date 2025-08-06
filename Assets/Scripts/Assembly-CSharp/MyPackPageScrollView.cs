using System.Collections.Generic;
using UnityEngine;

public class MyPackPageScrollView : MonoBehaviour
{
	public List<MyUIScrollViewPageRoot> page;

	public bool freeFlag;

	public bool movedFlag;

	public bool switchFlag;

	public bool leftFlag;

	public bool switchDoneFlag;

	public int targetPos;

	public float speed;

	public int curIndex;

	public MyUIScrollViewPageRoot curItemPage;

	public string viewResource;

	public float pageDistance;

	private void Start()
	{
	}

	public void Init()
	{
		curItemPage = page[0];
		pageDistance = 700f * GlobalDefine.screenWidthFit;
		for (int i = 0; i < page.Count; i++)
		{
			page[i].gameObject.transform.localPosition = new Vector3((float)i * pageDistance, 0f, 0f);
		}
	}

	private void OnEnable()
	{
		base.transform.localPosition = new Vector3((0f - pageDistance) * (float)curIndex, 0f, 0f);
	}

	private void Update()
	{
		if (!switchFlag)
		{
			return;
		}
		if (!switchDoneFlag)
		{
			if (leftFlag)
			{
				if (curIndex != 0)
				{
					curIndex--;
				}
				targetPos = (int)((0f - pageDistance) * (float)curIndex);
				curItemPage = page[curIndex];
				switchDoneFlag = true;
			}
			else
			{
				if (curIndex < page.Count - 1)
				{
					curIndex++;
				}
				targetPos = (int)((0f - pageDistance) * (float)curIndex);
				switchDoneFlag = true;
			}
		}
		if ((int)base.transform.localPosition.x < targetPos - 1)
		{
			base.transform.localPosition += new Vector3(speed, 0f, 0f) * Time.deltaTime * Mathf.Abs(base.transform.localPosition.x - (float)targetPos) / 100f;
		}
		else if ((int)base.transform.localPosition.x > targetPos + 1)
		{
			base.transform.localPosition -= new Vector3(speed, 0f, 0f) * Time.deltaTime * Mathf.Abs(base.transform.localPosition.x - (float)targetPos) / 100f;
		}
		else
		{
			switchFlag = false;
		}
	}

	public void OnChangeLeft()
	{
		switchFlag = true;
		leftFlag = true;
		switchDoneFlag = false;
	}

	public void OnChangeRight()
	{
		switchFlag = true;
		leftFlag = false;
		switchDoneFlag = false;
	}

	public void OnDragItem(GameObject go, Vector2 delta)
	{
		base.transform.localPosition += new Vector3(delta.x, 0f, 0f);
	}

	public void OnDropItem(GameObject a, bool isPress)
	{
		if (!isPress)
		{
			freeFlag = true;
			return;
		}
		freeFlag = false;
		if (!switchFlag)
		{
			switchDoneFlag = false;
		}
	}
}
