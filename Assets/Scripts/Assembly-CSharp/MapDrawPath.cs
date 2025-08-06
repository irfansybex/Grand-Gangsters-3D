using System.Collections.Generic;
using UnityEngine;

public class MapDrawPath : MonoBehaviour
{
	public GameObject miniMapObj;

	public List<RoadPointNew> pathpoint = new List<RoadPointNew>();

	public RoadPointNew start;

	public RoadPointNew end;

	public Vector3 playerpos;

	public Vector3 clickpos;

	public Transform playerCam;

	public LineRenderer linerenderer;

	public GameObject playerMapLabel;

	public bool targetFlag;

	public Vector3 targetPos;

	public GameObject targetLabel;

	public bool carFlag;

	public Vector3 carPos;

	public GameObject playerCarPosLabel;

	public float dis;

	private float difPos;

	private float difAngle;

	public List<Vector3> tempList;

	private void Update()
	{
		float x = (PlayerController.instance.transform.position.x - (float)VirtualminiMapController.instance.blockMap.startX + 200f) / 5f;
		float z = (PlayerController.instance.transform.position.z - (float)VirtualminiMapController.instance.blockMap.startY + 100f) / 5f;
		miniMapObj.transform.localPosition = new Vector3(x, -370f, z);
		miniMapObj.transform.localEulerAngles = new Vector3(90f, Mathf.LerpAngle(miniMapObj.transform.localEulerAngles.y, playerCam.eulerAngles.y, Time.deltaTime * 5f), 0f);
		playerMapLabel.transform.localPosition = new Vector3(x, -393f, z);
		difAngle = 0f;
		if (PlayerController.instance.car != null && PlayerController.instance.car.carType == CARTYPE.MOTOR)
		{
			difAngle = -90f;
		}
		playerMapLabel.transform.localEulerAngles = new Vector3(-90f, Mathf.LerpAngle(playerMapLabel.transform.localEulerAngles.y, PlayerController.instance.transform.eulerAngles.y + 90f + difAngle, Time.deltaTime * 5f), 0f);
		if (!carFlag)
		{
			if (playerCarPosLabel.active)
			{
				playerCarPosLabel.SetActiveRecursively(false);
			}
		}
		else
		{
			if (!playerCarPosLabel.active)
			{
				playerCarPosLabel.SetActiveRecursively(true);
			}
			if (!GlobalInf.mapMode)
			{
				difPos = Vector3.Distance(carPos, playerMapLabel.transform.position);
				if (difPos >= 25f)
				{
					playerCarPosLabel.transform.position = playerMapLabel.transform.position + (carPos - playerMapLabel.transform.position).normalized * 25f + Vector3.up * dis;
				}
				else if (difPos < 10f)
				{
					playerCarPosLabel.transform.position = carPos;
				}
				else
				{
					playerCarPosLabel.transform.position = carPos + Vector3.up * dis;
				}
				playerCarPosLabel.transform.localEulerAngles = new Vector3(-90f, miniMapObj.transform.localEulerAngles.y + 180f, 0f);
			}
			else
			{
				playerCarPosLabel.transform.position = carPos;
				playerCarPosLabel.transform.localEulerAngles = new Vector3(-90f, 180f, 0f);
			}
		}
		if (targetFlag)
		{
			if (!targetLabel.gameObject.active)
			{
				targetLabel.gameObject.SetActiveRecursively(true);
			}
			if (!GlobalInf.mapMode)
			{
				if (Vector3.Distance(targetPos, playerMapLabel.transform.position) >= 25f)
				{
					targetLabel.transform.position = playerMapLabel.transform.position + (targetPos - playerMapLabel.transform.position).normalized * 25f + Vector3.up * dis;
				}
				else
				{
					targetLabel.transform.position = targetPos + Vector3.up * dis;
				}
				targetLabel.transform.localEulerAngles = new Vector3(-90f, miniMapObj.transform.localEulerAngles.y + 180f, 0f);
			}
			else
			{
				targetLabel.transform.position = targetPos;
				targetLabel.transform.localEulerAngles = new Vector3(-90f, 180f, 0f);
			}
		}
		else if (targetLabel.gameObject.active)
		{
			targetLabel.gameObject.SetActiveRecursively(false);
		}
		if (GlobalInf.isDrawPathInfo && end.roadInfo != null)
		{
			updatePathpos();
			DrawPathLine(start, end, playerpos, clickpos);
		}
	}

	public void ClearLine()
	{
		GlobalInf.isDrawPathInfo = false;
		pathpoint.Clear();
		linerenderer.SetVertexCount(0);
	}

	public void DrawPath()
	{
		for (int i = 1; i < pathpoint.Count; i++)
		{
			float x = (pathpoint[i - 1].position.x - (float)VirtualminiMapController.instance.blockMap.startX + 200f) / 5f + base.transform.position.x;
			float z = (pathpoint[i - 1].position.z - (float)VirtualminiMapController.instance.blockMap.startY + 100f) / 5f + base.transform.position.z;
			float x2 = (pathpoint[i].position.x - (float)VirtualminiMapController.instance.blockMap.startX + 200f) / 5f + base.transform.position.x;
			float z2 = (pathpoint[i].position.z - (float)VirtualminiMapController.instance.blockMap.startY + 100f) / 5f + base.transform.position.z;
			Vector3 vector = new Vector3(x, -400f + base.transform.position.y, z);
			Vector3 vector2 = new Vector3(x2, -400f + base.transform.position.y, z2);
			Debug.DrawLine(vector, vector2, Color.blue);
		}
	}

	public void DrawPathLine(RoadPointNew start, RoadPointNew end, Vector3 playerpos, Vector3 clickpos)
	{
		if (start != end)
		{
			pathpoint = RoadPathInfo.instance.Getpath(start, end);
		}
		float x = (playerpos.x - (float)VirtualminiMapController.instance.blockMap.startX + 200f) / 5f + base.transform.position.x;
		float z = (playerpos.z - (float)VirtualminiMapController.instance.blockMap.startY + 100f) / 5f + base.transform.position.z;
		Vector3 vector = new Vector3(x, -400f + base.transform.position.y + 3f, z);
		tempList.Clear();
		tempList.Add(vector);
		if (start != end)
		{
			for (int i = 1; i < pathpoint.Count + 1; i++)
			{
				x = (pathpoint[i - 1].position.x - (float)VirtualminiMapController.instance.blockMap.startX + 200f) / 5f + base.transform.position.x;
				z = (pathpoint[i - 1].position.z - (float)VirtualminiMapController.instance.blockMap.startY + 100f) / 5f + base.transform.position.z;
				vector = new Vector3(x, -400f + base.transform.position.y + 3f, z);
				if (!(Vector3.SqrMagnitude(vector - tempList[tempList.Count - 1]) < 1f))
				{
					tempList.Add(vector);
				}
			}
		}
		x = (clickpos.x - (float)VirtualminiMapController.instance.blockMap.startX + 200f) / 5f + base.transform.position.x;
		z = (clickpos.z - (float)VirtualminiMapController.instance.blockMap.startY + 100f) / 5f + base.transform.position.z;
		vector = new Vector3(x, -400f + base.transform.position.y + 3f, z);
		tempList.Add(vector);
		linerenderer.SetVertexCount(tempList.Count);
		for (int j = 0; j < tempList.Count; j++)
		{
			linerenderer.SetPosition(j, tempList[j]);
		}
	}

	public void updatePathpos()
	{
		VirtualminiMapController.instance.GetPlayerLocation(PlayerController.instance.transform.position);
		RoadPointNew point = VirtualminiMapController.instance.playerLocation.point1;
		RoadPointNew point2 = VirtualminiMapController.instance.playerLocation.point2;
		float distanceFromPoint = VirtualminiMapController.instance.playerLocation.distanceFromPoint1;
		RoadPointNew point3 = VirtualminiMapController.instance.playerLocation.point1;
		RoadPointNew point4 = VirtualminiMapController.instance.targetLocation.point1;
		RoadPointNew point5 = VirtualminiMapController.instance.targetLocation.point1;
		RoadPointNew point6 = VirtualminiMapController.instance.targetLocation.point2;
		Vector3 vector2;
		if (point3.roadInfo == point4.roadInfo)
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
			for (int j = 0; j < point5.roadInfo.roadPointList.Length; j++)
			{
				if (point5 == point5.roadInfo.roadPointList[j])
				{
					c = j;
				}
				if (point6 == point5.roadInfo.roadPointList[j])
				{
					d = j;
				}
			}
			if (doubleNotEqual(a, b, c, d))
			{
				Vector2 vector = FindMidNum(a, b, c, d);
				point3 = point.roadInfo.roadPointList[(int)vector.x];
				point4 = point5.roadInfo.roadPointList[(int)vector.y];
			}
			else
			{
				int num = findSameNum(a, b, c, d);
				point3 = point.roadInfo.roadPointList[num];
				point4 = point5.roadInfo.roadPointList[num];
			}
			Vector3 to = PlayerController.instance.transform.position - point.position;
			Vector3 forward = point.forward;
			float num2 = Vector3.Angle(forward, to);
			vector2 = ((!(num2 > 90f)) ? (point.position + distanceFromPoint * point.forward) : (point.position - distanceFromPoint * point.forward));
			start = point3;
			end = point4;
			playerpos = vector2;
			return;
		}
		Vector2 vector3 = RoadPathInfo.instance.FindTwoWayShortCrosspoint(point3, point4);
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
		point3 = ((vector3.x == 0f) ? ((num3 <= num4) ? point : point2) : ((num3 <= num4) ? point2 : point));
		Vector3 to2 = PlayerController.instance.transform.position - point.position;
		Vector3 forward2 = point.forward;
		float num5 = Vector3.Angle(forward2, to2);
		vector2 = ((!(num5 > 90f)) ? (point.position + distanceFromPoint * point.forward) : (point.position - distanceFromPoint * point.forward));
		num3 = 0;
		num4 = 0;
		for (int l = 0; l < point5.roadInfo.roadPointList.Length; l++)
		{
			if (point5 == point5.roadInfo.roadPointList[l])
			{
				num3 = l;
			}
			if (point6 == point5.roadInfo.roadPointList[l])
			{
				num4 = l;
			}
		}
		point4 = ((vector3.y == 0f) ? ((num3 <= num4) ? point5 : point6) : ((num3 <= num4) ? point6 : point5));
		start = point3;
		end = point4;
		playerpos = vector2;
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
