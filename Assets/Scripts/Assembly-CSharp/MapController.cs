using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
	public Camera mapCam;

	public LayerMask minimaplayer;

	public Transform mapLeftDownAnchor;

	public MapDrawPath mapDrawPath;

	public Vector3 screenleftdown;

	public Vector3 screenrightup;

	public RoadPointNew startp;

	public RoadPointNew endp;

	public Transform leftdown;

	public Transform rightup;

	public Vector2 old1;

	public Vector2 old2;

	public Vector3 playerpos;

	public Vector3 clickpos;

	public RaycastHit hit;

	public LocateInfoNew preendlocateInfo;

	public List<UISprite> UIlist;

	public Vector3 tempworldpos;

	public float screenRate;

	private Vector2 ldPos;

	private Vector2 ruPos;

	private void Awake()
	{
		screenRate = (float)Screen.width / (float)Screen.height;
	}

	private void Start()
	{
		base.GetComponent<Camera>().orthographicSize = 80f;
	}

	private void Update()
	{
		clickmap();
		moveminimap();
	}

	public void clickmap()
	{
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && IsRightPos(Input.GetTouch(0).position))
		{
			Ray ray = base.GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);
			if (Physics.Raycast(ray, out hit, 1000f, minimaplayer))
			{
				tempworldpos = ChangeToWorldPos(hit.point);
				Findclickwaypoint(tempworldpos);
			}
		}
	}

	public Vector3 ChangeToWorldPos(Vector3 point)
	{
		float x = (point.x - mapLeftDownAnchor.transform.position.x) * 5f + (float)VirtualminiMapController.instance.blockMap.startX - 200f;
		float y = 0f;
		float z = (point.z - mapLeftDownAnchor.transform.position.z) * 5f + (float)VirtualminiMapController.instance.blockMap.startY - 100f;
		return new Vector3(x, y, z);
	}

	public void Findclickwaypoint(Vector3 temp)
	{
		Vector3 position = PlayerController.instance.transform.position;
		VirtualminiMapController.instance.GetPlayerLocation(position);
		RoadPointNew point = VirtualminiMapController.instance.playerLocation.point1;
		RoadPointNew point2 = VirtualminiMapController.instance.playerLocation.point2;
		float distanceFromPoint = VirtualminiMapController.instance.playerLocation.distanceFromPoint1;
		startp = VirtualminiMapController.instance.playerLocation.point1;
		VirtualminiMapController.instance.GetTargetLocation(temp);
		if (VirtualminiMapController.instance.targetCrossFlag)
		{
			return;
		}
		endp = VirtualminiMapController.instance.targetLocation.point1;
		RoadPointNew point3 = VirtualminiMapController.instance.targetLocation.point1;
		RoadPointNew point4 = VirtualminiMapController.instance.targetLocation.point2;
		float distanceFromPoint2 = VirtualminiMapController.instance.targetLocation.distanceFromPoint1;
		if (startp.roadInfo == endp.roadInfo)
		{
			int a = 0;
			int b = 0;
			for (int i = 0; i < point.roadInfo.roadPointList.Length; i++)
			{
				if (point == point.roadInfo.roadPointList[i])
				{
					a = i;
				}
				if (point2 == point.roadInfo.roadPointList[i])
				{
					b = i;
				}
			}
			int c = 0;
			int d = 0;
			for (int j = 0; j < point3.roadInfo.roadPointList.Length; j++)
			{
				if (point3 == point3.roadInfo.roadPointList[j])
				{
					c = j;
				}
				if (point4 == point3.roadInfo.roadPointList[j])
				{
					d = j;
				}
			}
			if (doubleNotEqual(a, b, c, d))
			{
				Vector2 vector = FindMidNum(a, b, c, d);
				startp = point.roadInfo.roadPointList[(int)vector.x];
				endp = point3.roadInfo.roadPointList[(int)vector.y];
			}
			else
			{
				int num = findSameNum(a, b, c, d);
				startp = point.roadInfo.roadPointList[num];
				endp = point3.roadInfo.roadPointList[num];
			}
			Vector3 to = PlayerController.instance.transform.position - point.position;
			Vector3 forward = point.forward;
			float num2 = Vector3.Angle(forward, to);
			if (num2 > 90f)
			{
				playerpos = point.position - distanceFromPoint * point.forward;
			}
			else
			{
				playerpos = point.position + distanceFromPoint * point.forward;
			}
			to = temp - point3.position;
			forward = point3.forward;
			num2 = Vector3.Angle(forward, to);
			if (num2 > 90f)
			{
				clickpos = point3.position - distanceFromPoint2 * point3.forward;
			}
			else
			{
				clickpos = point3.position + distanceFromPoint2 * point3.forward;
			}
			mapDrawPath.clickpos = clickpos;
			mapDrawPath.start = startp;
			mapDrawPath.end = endp;
			mapDrawPath.playerpos = playerpos;
			return;
		}
		Vector2 vector2 = RoadPathInfo.instance.FindTwoWayShortCrosspoint(startp, endp);
		int num3 = 0;
		int num4 = 0;
		for (int k = 0; k < point.roadInfo.roadPointList.Length; k++)
		{
			if (point == point.roadInfo.roadPointList[k])
			{
				num3 = k;
			}
			if (point2 == point.roadInfo.roadPointList[k])
			{
				num4 = k;
			}
		}
		if (vector2.x == 0f)
		{
			if (num3 > num4)
			{
				startp = point2;
			}
			else
			{
				startp = point;
			}
		}
		else if (num3 > num4)
		{
			startp = point;
		}
		else
		{
			startp = point2;
		}
		Vector3 to2 = position - point.position;
		Vector3 forward2 = point.forward;
		float num5 = Vector3.Angle(forward2, to2);
		if (num5 > 90f)
		{
			playerpos = point.position - distanceFromPoint * point.forward;
		}
		else
		{
			playerpos = point.position + distanceFromPoint * point.forward;
		}
		num3 = 0;
		num4 = 0;
		for (int l = 0; l < point3.roadInfo.roadPointList.Length; l++)
		{
			if (point3 == point3.roadInfo.roadPointList[l])
			{
				num3 = l;
			}
			if (point4 == point3.roadInfo.roadPointList[l])
			{
				num4 = l;
			}
		}
		if (vector2.y == 0f)
		{
			if (num3 > num4)
			{
				endp = point4;
			}
			else
			{
				endp = point3;
			}
		}
		else if (num3 > num4)
		{
			endp = point3;
		}
		else
		{
			endp = point4;
		}
		to2 = temp - point3.position;
		forward2 = point3.forward;
		num5 = Vector3.Angle(forward2, to2);
		if (num5 > 90f)
		{
			clickpos = point3.position - distanceFromPoint2 * point3.forward;
		}
		else
		{
			clickpos = point3.position + distanceFromPoint2 * point3.forward;
		}
		mapDrawPath.clickpos = clickpos;
		mapDrawPath.start = startp;
		mapDrawPath.end = endp;
		mapDrawPath.playerpos = playerpos;
	}

	public void moveminimap()
	{
		screenleftdown = base.GetComponent<Camera>().WorldToScreenPoint(leftdown.position);
		screenrightup = base.GetComponent<Camera>().WorldToScreenPoint(rightup.position);
		ldPos = new Vector2(mapCam.orthographicSize * screenRate, mapCam.orthographicSize);
		ruPos = new Vector2(400f - mapCam.orthographicSize * screenRate, 450f - mapCam.orthographicSize);
		if (Input.touchCount == 1)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
				float num = 0.3f;
				screenleftdown = new Vector3(screenleftdown.x + deltaPosition.x * num, screenleftdown.y + deltaPosition.y * num, screenleftdown.z);
				screenrightup = new Vector3(screenrightup.x + deltaPosition.x * num, screenrightup.y + deltaPosition.y * num, screenrightup.z);
				int num2 = ((0f - deltaPosition.x < 0f && screenleftdown.x < 0f) ? 1 : 0);
				int num3 = ((0f - deltaPosition.x > 0f && screenrightup.x > (float)Screen.width) ? 1 : 0);
				int num4 = ((0f - deltaPosition.y > 0f && screenrightup.y > (float)Screen.height) ? 1 : 0);
				int num5 = ((0f - deltaPosition.y < 0f && screenleftdown.y < 0f) ? 1 : 0);
				base.transform.Translate((0f - deltaPosition.x) * num * (float)num2 - deltaPosition.x * num * (float)num3, (0f - deltaPosition.y) * num * (float)num4 - deltaPosition.y * num * (float)num5, 0f);
			}
		}
		else
		{
			if (Input.touchCount <= 1 || Input.GetTouch(0).phase != TouchPhase.Moved || Input.GetTouch(1).phase != TouchPhase.Moved)
			{
				return;
			}
			Vector2 position = Input.GetTouch(0).position;
			Vector2 position2 = Input.GetTouch(1).position;
			if (islarge(old1, old2, position, position2))
			{
				float num6 = base.GetComponent<Camera>().orthographicSize - 2f;
				if (num6 > 80f)
				{
					base.GetComponent<Camera>().orthographicSize -= 2f;
				}
				else
				{
					base.GetComponent<Camera>().orthographicSize = 80f;
				}
			}
			else
			{
				base.GetComponent<Camera>().orthographicSize += 2f;
				if (screenleftdown.x > 0f || screenleftdown.y > 0f || screenrightup.x < (float)Screen.width || screenrightup.y < (float)Screen.height)
				{
					base.GetComponent<Camera>().orthographicSize -= 2f;
				}
			}
			old1 = position;
			old2 = position2;
		}
	}

	public bool islarge(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
	{
		float num = Mathf.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
		float num2 = Mathf.Sqrt((c.x - d.x) * (c.x - d.x) + (c.y - d.y) * (c.y - d.y));
		if (num < num2)
		{
			return true;
		}
		return false;
	}

	public bool IsRightPos(Vector3 pos)
	{
		for (int i = 0; i < UIlist.Count; i++)
		{
			Vector2 vector = new Vector2(UIlist[i].transform.localPosition.x + (float)(Screen.width / 2), UIlist[i].transform.localPosition.y + (float)(Screen.height / 2));
			Vector2 vector2 = new Vector2(vector.x - (float)(UIlist[i].width / 2), vector.y - (float)(UIlist[i].width / 2));
			Vector2 vector3 = new Vector2(vector.x + (float)(UIlist[i].width / 2), vector.y + (float)(UIlist[i].width / 2));
			if (pos.x > vector2.x && pos.y > vector2.y && pos.x < vector3.x && pos.y < vector3.y)
			{
				return false;
			}
		}
		return true;
	}

	public bool doubleNotEqual(int a, int b, int c, int d)
	{
		if (a == c && b == d)
		{
			return false;
		}
		if (a == d && b == c)
		{
			return false;
		}
		return true;
	}

	public Vector2 FindMidNum(int a, int b, int c, int d)
	{
		Vector2 result = default(Vector2);
		if (a > c)
		{
			if (a > b)
			{
				result.x = b;
			}
			else
			{
				result.x = a;
			}
			if (c > d)
			{
				result.y = c;
			}
			else
			{
				result.y = d;
			}
		}
		else
		{
			if (a > b)
			{
				result.x = a;
			}
			else
			{
				result.x = b;
			}
			if (c > d)
			{
				result.y = d;
			}
			else
			{
				result.y = c;
			}
		}
		return result;
	}

	public int findSameNum(int a, int b, int c, int d)
	{
		if (a == c)
		{
			return a;
		}
		if (a == d)
		{
			return a;
		}
		return c;
	}
}
