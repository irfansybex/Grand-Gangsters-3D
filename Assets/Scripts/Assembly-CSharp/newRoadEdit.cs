using UnityEngine;

[ExecuteInEditMode]
public class newRoadEdit : MonoBehaviour
{
	public BlockMapDate sourceDate;

	public GameObject rootObj;

	public bool runFlag1;

	public bool runFlag2;

	public bool runFlag3;

	public bool drawFlag;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag1)
		{
			runFlag1 = false;
			InitMap();
		}
		if (runFlag2)
		{
			runFlag2 = false;
			FormatLinkPoint();
			MonoBehaviour.print("runFlag2");
		}
		if (runFlag3)
		{
			runFlag3 = false;
			SetBlockMap();
			MonoBehaviour.print("runFlag3");
		}
	}

	public void SetBlockMap()
	{
		BlockMapDateNew component = rootObj.GetComponent<BlockMapDateNew>();
		component.startX = sourceDate.startX;
		component.startY = sourceDate.startY;
		component.endX = sourceDate.endX;
		component.endY = sourceDate.endY;
		component.blockLine = new BlockLineNew[sourceDate.blockLine.Count];
		for (int i = 0; i < sourceDate.blockLine.Count; i++)
		{
			component.blockLine[i] = new BlockLineNew();
			component.blockLine[i].blockLine = new BlockDateNew[sourceDate.blockLine[i].blockLine.Count];
			for (int j = 0; j < sourceDate.blockLine[i].blockLine.Count; j++)
			{
				component.blockLine[i].blockLine[j] = new BlockDateNew();
				component.blockLine[i].blockLine[j].roadList = new RoadInfoNew[sourceDate.blockLine[i].blockLine[j].roadList.Count];
				for (int k = 0; k < sourceDate.blockLine[i].blockLine[j].roadList.Count; k++)
				{
					component.blockLine[i].blockLine[j].roadList[k] = component.transform.Find(sourceDate.blockLine[i].blockLine[j].roadList[k].gameObject.name).gameObject.GetComponent<RoadInfoNew>();
				}
			}
		}
	}

	public void InitMap()
	{
		AddScript();
		SetRoadInfo();
		SetRoadList();
		SetRoadPointRoadInfo();
		LinkRoad();
		MonoBehaviour.print("run");
	}

	public void AddScript()
	{
		if (rootObj.GetComponent<BlockMapDateNew>() == null)
		{
			rootObj.AddComponent<BlockMapDateNew>();
		}
		for (int i = 0; i < rootObj.transform.childCount; i++)
		{
			if (rootObj.transform.GetChild(i).gameObject.GetComponent<RoadInfoNew>() == null)
			{
				rootObj.transform.GetChild(i).gameObject.AddComponent<RoadInfoNew>();
			}
		}
	}

	public void SetRoadInfo()
	{
		MonoBehaviour.print(rootObj.transform.childCount);
		for (int i = 0; i < rootObj.transform.childCount; i++)
		{
			RoadInfo component = sourceDate.transform.Find(rootObj.transform.GetChild(i).gameObject.name).gameObject.GetComponent<RoadInfo>();
			RoadInfoNew component2 = rootObj.transform.GetChild(i).gameObject.GetComponent<RoadInfoNew>();
			if (component != null)
			{
				for (int j = 0; j < component.roadPointList.Count; j++)
				{
					SetRoadPointInfo(component2, component);
				}
			}
			else
			{
				MonoBehaviour.print("ttt");
			}
		}
	}

	public void SetRoadPointInfo(RoadInfoNew newRoad, RoadInfo oldRoad)
	{
		if (oldRoad.straitRoadFlag)
		{
			newRoad.roadPointList = new RoadPointNew[oldRoad.roadPointList.Count];
			MonoBehaviour.print(newRoad.gameObject.name);
			MonoBehaviour.print(newRoad.roadPointList.Length);
			for (int i = 0; i < oldRoad.roadPointList.Count; i++)
			{
				newRoad.roadPointList[i] = new RoadPointNew();
				CopyRoadPoint(newRoad.roadPointList[i], oldRoad.roadPointList[i]);
				newRoad.roadPointList[i].roadInfo = newRoad;
			}
		}
		else
		{
			newRoad.roadPointList = new RoadPointNew[oldRoad.roadPointList.Count - 2];
			MonoBehaviour.print(newRoad.gameObject.name);
			MonoBehaviour.print(newRoad.roadPointList.Length);
			for (int j = 0; j < newRoad.roadPointList.Length; j++)
			{
				newRoad.roadPointList[j] = new RoadPointNew();
				CopyRoadPoint(newRoad.roadPointList[j], oldRoad.roadPointList[j + 1]);
				newRoad.roadPointList[j].roadInfo = newRoad;
			}
		}
	}

	public void SetRoadPointRoadInfo()
	{
		roadInfoListNew component = rootObj.GetComponent<roadInfoListNew>();
		for (int i = 0; i < component.roadList.Length; i++)
		{
			MonoBehaviour.print(component.roadList[i].roadPointList);
			for (int j = 0; j < component.roadList[i].roadPointList.Length; j++)
			{
				component.roadList[i].roadPointList[j].roadInfo = component.roadList[i];
			}
		}
	}

	public void CopyRoadPoint(RoadPointNew newPoint, RoadPointInfo oldPoint)
	{
		newPoint.crossFlag = oldPoint.CrossPoint;
		newPoint.Drive_State = oldPoint.Drive_State;
		newPoint.dummyCrossPoint = oldPoint.dummyCrossPoint;
		newPoint.forward = oldPoint.transform.forward;
		newPoint.right = oldPoint.transform.right;
		newPoint.position = oldPoint.transform.position;
	}

	public void SetRoadList()
	{
		rootObj.AddComponent<roadInfoListNew>();
		roadInfoListNew component = rootObj.GetComponent<roadInfoListNew>();
		component.roadList = new RoadInfoNew[rootObj.transform.childCount];
		for (int i = 0; i < component.roadList.Length; i++)
		{
			component.roadList[i] = rootObj.transform.GetChild(i).gameObject.GetComponent<RoadInfoNew>();
		}
	}

	public void LinkRoad()
	{
		for (int i = 0; i < rootObj.transform.childCount; i++)
		{
			RoadInfoNew component = rootObj.transform.GetChild(i).gameObject.GetComponent<RoadInfoNew>();
			MonoBehaviour.print(component);
			for (int j = 1; j < component.roadPointList.Length - 1; j++)
			{
				component.roadPointList[j].linkPoint = new LinkPoint[2];
				MonoBehaviour.print(component.roadPointList[j].linkPoint.Length);
				component.roadPointList[j].linkPoint[0] = new LinkPoint(component, j - 1);
				component.roadPointList[j].linkPoint[1] = new LinkPoint(component, j + 1);
			}
			component.roadPointList[0].linkPoint = new LinkPoint[4];
			component.roadPointList[component.roadPointList.Length - 1].linkPoint = new LinkPoint[4];
			component.roadPointList[0].linkPoint[0] = new LinkPoint(component, 1);
			component.roadPointList[component.roadPointList.Length - 1].linkPoint[0] = new LinkPoint(component, component.roadPointList.Length - 2);
			LinkCrossPoint(component.roadPointList[0]);
			LinkCrossPoint(component.roadPointList[component.roadPointList.Length - 1]);
		}
	}

	public void LinkCrossPoint(RoadPointNew crossPoint)
	{
		roadInfoListNew component = rootObj.GetComponent<roadInfoListNew>();
		for (int i = 0; i < component.roadList.Length; i++)
		{
			if (component.roadList[i] != crossPoint.roadInfo)
			{
				if (Vector3.Distance(crossPoint.position, component.roadList[i].roadPointList[0].position) < 20f)
				{
					AddLinkPoint(crossPoint, component.roadList[i].roadPointList[0], 0);
				}
				else if (Vector3.Distance(crossPoint.position, component.roadList[i].roadPointList[component.roadList[i].roadPointList.Length - 1].position) < 20f)
				{
					AddLinkPoint(crossPoint, component.roadList[i].roadPointList[component.roadList[i].roadPointList.Length - 1], component.roadList[i].roadPointList.Length - 1);
				}
			}
		}
	}

	public void AddLinkPoint(RoadPointNew crossPoint, RoadPointNew linkPoint, int index)
	{
		for (int i = 0; i < crossPoint.linkPoint.Length; i++)
		{
			if (crossPoint.linkPoint[i] == null)
			{
				if (i == 1)
				{
					crossPoint.crossFlag = false;
				}
				if (i == 2)
				{
					crossPoint.crossFlag = true;
				}
				crossPoint.linkPoint[i] = new LinkPoint(linkPoint.roadInfo, index);
				break;
			}
		}
	}

	public void FormatLinkPoint()
	{
		roadInfoListNew component = rootObj.GetComponent<roadInfoListNew>();
		for (int i = 0; i < component.roadList.Length; i++)
		{
			if (component.roadList[i].roadPointList[0].crossFlag)
			{
				ReFormatLinkPoint(component.roadList[i].roadPointList[0]);
			}
			if (component.roadList[i].roadPointList[component.roadList[i].roadPointList.Length - 1].crossFlag)
			{
				ReFormatLinkPoint(component.roadList[i].roadPointList[component.roadList[i].roadPointList.Length - 1]);
			}
		}
	}

	public void ReFormatLinkPoint(RoadPointNew roadPoint)
	{
		LinkPoint[] array = new LinkPoint[4];
		for (int i = 0; i < roadPoint.linkPoint.Length; i++)
		{
			if (roadPoint.linkPoint[i].road != null)
			{
				array[i] = new LinkPoint(roadPoint.linkPoint[i].road, roadPoint.linkPoint[i].pointIndex);
			}
		}
		array[1] = FindForward(roadPoint);
		array[2] = FindLeft(roadPoint);
		array[3] = FindRight(roadPoint);
		roadPoint.roadDistance = new float[4];
		if (array[1] != null)
		{
			roadPoint.linkPoint[1] = new LinkPoint(array[1].road, array[1].pointIndex);
			roadPoint.roadDistance[1] = Vector3.Project(roadPoint.linkPoint[1].road.roadPointList[roadPoint.linkPoint[1].pointIndex].position - roadPoint.position, roadPoint.forward).magnitude;
		}
		else
		{
			roadPoint.linkPoint[1] = new LinkPoint(null, 0);
		}
		if (array[2] != null)
		{
			roadPoint.linkPoint[2] = new LinkPoint(array[2].road, array[2].pointIndex);
		}
		else
		{
			roadPoint.linkPoint[2] = new LinkPoint(null, 0);
		}
		if (array[3] != null)
		{
			roadPoint.linkPoint[3] = new LinkPoint(array[3].road, array[3].pointIndex);
		}
		else
		{
			roadPoint.linkPoint[3] = new LinkPoint(null, 0);
		}
	}

	public LinkPoint FindForward(RoadPointNew roadPoint)
	{
		for (int i = 1; i < roadPoint.linkPoint.Length; i++)
		{
			if (roadPoint.linkPoint[i].road != null && Vector3.Project(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, roadPoint.forward).magnitude > 15f)
			{
				return new LinkPoint(roadPoint.linkPoint[i].road, roadPoint.linkPoint[i].pointIndex);
			}
		}
		return null;
	}

	public LinkPoint FindRight(RoadPointNew roadPoint)
	{
		for (int i = 1; i < roadPoint.linkPoint.Length; i++)
		{
			if (!(roadPoint.linkPoint[i].road != null))
			{
				continue;
			}
			if (ToolFunction.isForward(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, roadPoint.forward))
			{
				if (ToolFunction.isForward(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, roadPoint.right) && Vector3.Project(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, roadPoint.right).magnitude > 5f)
				{
					return new LinkPoint(roadPoint.linkPoint[i].road, roadPoint.linkPoint[i].pointIndex);
				}
			}
			else if (ToolFunction.isForward(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, -roadPoint.right) && Vector3.Project(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, -roadPoint.right).magnitude > 5f)
			{
				return new LinkPoint(roadPoint.linkPoint[i].road, roadPoint.linkPoint[i].pointIndex);
			}
		}
		return null;
	}

	public LinkPoint FindLeft(RoadPointNew roadPoint)
	{
		for (int i = 1; i < roadPoint.linkPoint.Length; i++)
		{
			if (!(roadPoint.linkPoint[i].road != null))
			{
				continue;
			}
			if (ToolFunction.isForward(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, roadPoint.forward))
			{
				if (ToolFunction.isForward(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, -roadPoint.right) && Vector3.Project(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, -roadPoint.right).magnitude > 5f)
				{
					return new LinkPoint(roadPoint.linkPoint[i].road, roadPoint.linkPoint[i].pointIndex);
				}
			}
			else if (ToolFunction.isForward(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, roadPoint.right) && Vector3.Project(roadPoint.linkPoint[i].road.roadPointList[roadPoint.linkPoint[i].pointIndex].position - roadPoint.position, roadPoint.right).magnitude > 5f)
			{
				return new LinkPoint(roadPoint.linkPoint[i].road, roadPoint.linkPoint[i].pointIndex);
			}
		}
		return null;
	}

	private void OnDrawGizmos()
	{
		if (!drawFlag)
		{
			return;
		}
		roadInfoListNew component = rootObj.GetComponent<roadInfoListNew>();
		for (int i = 0; i < component.roadList.Length; i++)
		{
			for (int j = 0; j < component.roadList[i].roadPointList.Length; j++)
			{
				if (!component.roadList[i].roadPointList[j].crossFlag)
				{
					for (int k = 0; k < component.roadList[i].roadPointList[j].linkPoint.Length; k++)
					{
						if (component.roadList[i].roadPointList[j].linkPoint[k].road != null)
						{
							Gizmos.color = Color.white;
							Gizmos.DrawLine(component.roadList[i].roadPointList[j].position, component.roadList[i].roadPointList[j].linkPoint[k].road.roadPointList[component.roadList[i].roadPointList[j].linkPoint[k].pointIndex].position);
						}
					}
					continue;
				}
				if (component.roadList[i].roadPointList[j].linkPoint[0].road != null)
				{
					Gizmos.color = Color.white;
					Gizmos.DrawLine(component.roadList[i].roadPointList[j].position, component.roadList[i].roadPointList[j].linkPoint[0].road.roadPointList[component.roadList[i].roadPointList[j].linkPoint[0].pointIndex].position);
				}
				if (component.roadList[i].roadPointList[j].linkPoint[1].road != null)
				{
					Gizmos.color = Color.red;
					Gizmos.DrawLine(component.roadList[i].roadPointList[j].position, component.roadList[i].roadPointList[j].linkPoint[1].road.roadPointList[component.roadList[i].roadPointList[j].linkPoint[1].pointIndex].position);
				}
				if (component.roadList[i].roadPointList[j].linkPoint[2].road != null)
				{
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(component.roadList[i].roadPointList[j].position, component.roadList[i].roadPointList[j].linkPoint[2].road.roadPointList[component.roadList[i].roadPointList[j].linkPoint[2].pointIndex].position);
				}
				if (component.roadList[i].roadPointList[j].linkPoint[3].road != null)
				{
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(component.roadList[i].roadPointList[j].position, component.roadList[i].roadPointList[j].linkPoint[3].road.roadPointList[component.roadList[i].roadPointList[j].linkPoint[3].pointIndex].position);
				}
			}
		}
	}
}
