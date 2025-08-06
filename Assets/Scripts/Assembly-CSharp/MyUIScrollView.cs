using UnityEngine;

public class MyUIScrollView : MonoBehaviour
{
	public delegate void OnInitPage(ItemPageController page, int index);

	public delegate void OnResetBtn();

	public GameObject leftItem;

	public GameObject middleItem;

	public GameObject rightItem;

	public ItemPageController leftItemPage;

	public ItemPageController middleItemPage;

	public ItemPageController rightItemPage;

	public bool freeFlag;

	public bool movedFlag;

	public bool switchFlag;

	public bool leftFlag;

	public bool switchDoneFlag;

	public int targetPos;

	public float speed;

	public int curIndex;

	public ItemPageController curItemPage;

	public int maxIndex;

	public string viewResource;

	public OnInitPage onInitPage;

	public OnResetBtn onResetBtn;

	public UIPanel panel;

	public ItemPageController tempItemPage;

	private void Start()
	{
	}

	public void Init()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load(viewResource + "0")) as GameObject;
		gameObject.transform.parent = leftItem.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (onInitPage != null)
		{
			leftItemPage = gameObject.GetComponent<ItemPageController>();
			onInitPage(leftItemPage, 0);
		}
		gameObject = Object.Instantiate(gameObject) as GameObject;
		gameObject.transform.parent = middleItem.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (onInitPage != null)
		{
			middleItemPage = gameObject.GetComponent<ItemPageController>();
			onInitPage(middleItemPage, 1);
		}
		gameObject = Object.Instantiate(gameObject) as GameObject;
		gameObject.transform.parent = rightItem.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		if (onInitPage != null)
		{
			rightItemPage = gameObject.GetComponent<ItemPageController>();
			onInitPage(rightItemPage, 2);
		}
		leftItem.transform.localPosition = new Vector3(-700f * GlobalDefine.screenWidthFit, 0f, 0f);
		rightItem.transform.localPosition = new Vector3(700f * GlobalDefine.screenWidthFit, 0f, 0f);
		curItemPage = leftItemPage;
	}

	private void OnEnable()
	{
		if (curIndex == 0)
		{
			base.transform.localPosition = new Vector3(700f * GlobalDefine.screenWidthFit, 0f, 0f);
			curItemPage = leftItemPage;
		}
		else if (curIndex == maxIndex)
		{
			base.transform.localPosition = new Vector3(-700f * GlobalDefine.screenWidthFit, 0f, 0f);
			curItemPage = rightItemPage;
		}
		else
		{
			base.transform.localPosition = Vector3.zero;
			curItemPage = middleItemPage;
		}
	}

	private void Update()
	{
		if (!switchFlag)
		{
			if (!freeFlag)
			{
				return;
			}
			if (!switchDoneFlag)
			{
				if (base.transform.localPosition.x > 100f)
				{
					if (curIndex != 0)
					{
						if (curIndex != 1)
						{
							LeftCreatePage();
						}
						curIndex--;
						targetPos = 0;
						switchDoneFlag = true;
					}
					else
					{
						switchDoneFlag = true;
						targetPos = (int)(700f * GlobalDefine.screenWidthFit);
					}
				}
				else if (base.transform.localPosition.x < -100f)
				{
					if (curIndex != maxIndex)
					{
						if (curIndex != maxIndex - 1)
						{
							RightCreatePage();
						}
						curIndex++;
						switchDoneFlag = true;
						targetPos = 0;
					}
					else
					{
						switchDoneFlag = true;
						targetPos = (int)(-700f * GlobalDefine.screenWidthFit);
					}
				}
				else
				{
					if (curIndex == 0)
					{
						switchDoneFlag = true;
						curIndex++;
					}
					if (curIndex == maxIndex)
					{
						switchDoneFlag = true;
						curIndex--;
					}
					targetPos = 0;
					switchDoneFlag = true;
				}
			}
			if ((int)base.transform.localPosition.x < targetPos)
			{
				base.transform.localPosition += new Vector3(speed, 0f, 0f) * Time.deltaTime * Mathf.Abs(base.transform.localPosition.x - (float)targetPos) / 100f;
			}
			else if ((int)base.transform.localPosition.x > targetPos)
			{
				base.transform.localPosition -= new Vector3(speed, 0f, 0f) * Time.deltaTime * Mathf.Abs(base.transform.localPosition.x - (float)targetPos) / 100f;
			}
			return;
		}
		if (!switchDoneFlag)
		{
			if (leftFlag)
			{
				if (curIndex != 0)
				{
					if (curIndex != 1 && curIndex != maxIndex)
					{
						LeftCreatePage();
					}
					curIndex--;
					if (curIndex == 0)
					{
						targetPos = (int)(700f * GlobalDefine.screenWidthFit);
						curItemPage = leftItemPage;
					}
					else
					{
						targetPos = 0;
						curItemPage = middleItemPage;
					}
					switchDoneFlag = true;
				}
				else
				{
					targetPos = (int)(700f * GlobalDefine.screenWidthFit);
					switchDoneFlag = true;
				}
				onResetBtn();
			}
			else
			{
				if (curIndex != maxIndex)
				{
					if (curIndex != maxIndex - 1 && curIndex != 0)
					{
						RightCreatePage();
					}
					curIndex++;
					if (curIndex == maxIndex)
					{
						targetPos = (int)(-700f * GlobalDefine.screenWidthFit);
						curItemPage = rightItemPage;
					}
					else
					{
						targetPos = 0;
						curItemPage = middleItemPage;
					}
					switchDoneFlag = true;
				}
				else
				{
					targetPos = (int)(-700f * GlobalDefine.screenWidthFit);
					switchDoneFlag = true;
				}
				onResetBtn();
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

	public void LeftCreatePage()
	{
		GameObject gameObject = rightItemPage.gameObject;
		tempItemPage = rightItemPage;
		gameObject.transform.parent = null;
		middleItem.transform.GetChild(0).parent = rightItem.transform;
		rightItem.transform.GetChild(0).transform.localPosition = Vector3.zero;
		rightItemPage = middleItemPage;
		leftItem.transform.GetChild(0).parent = middleItem.transform;
		middleItem.transform.GetChild(0).transform.localPosition = Vector3.zero;
		middleItemPage = leftItemPage;
		gameObject.transform.parent = leftItem.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		base.transform.localPosition = base.transform.localPosition - new Vector3(700f * GlobalDefine.screenWidthFit, 0f, 0f);
		if (onInitPage != null)
		{
			leftItemPage = tempItemPage;
			onInitPage(leftItemPage, curIndex - 2);
		}
	}

	public void RightCreatePage()
	{
		GameObject gameObject = leftItemPage.gameObject;
		tempItemPage = leftItemPage;
		gameObject.transform.parent = null;
		middleItem.transform.GetChild(0).parent = leftItem.transform;
		leftItem.transform.GetChild(0).transform.localPosition = Vector3.zero;
		leftItemPage = middleItemPage;
		rightItem.transform.GetChild(0).parent = middleItem.transform;
		middleItem.transform.GetChild(0).transform.localPosition = Vector3.zero;
		middleItemPage = rightItemPage;
		gameObject.transform.parent = rightItem.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		base.transform.localPosition = base.transform.localPosition + new Vector3(700f * GlobalDefine.screenWidthFit, 0f, 0f);
		if (onInitPage != null)
		{
			rightItemPage = tempItemPage;
			onInitPage(rightItemPage, curIndex + 2);
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
