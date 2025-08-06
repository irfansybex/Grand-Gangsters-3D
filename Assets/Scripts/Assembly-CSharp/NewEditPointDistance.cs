using UnityEngine;

[ExecuteInEditMode]
public class NewEditPointDistance : MonoBehaviour
{
	public roadInfoListNew roadList;

	public bool runFlag;

	public bool runFlag2;

	public bool runFlag3;

	public bool labeledFlag;

	public bool drawFlag;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			MonoBehaviour.print("run");
			AddDistance();
		}
		if (runFlag2)
		{
			runFlag2 = false;
			SetCrossInfo();
			MonoBehaviour.print("run");
		}
		if (runFlag3)
		{
			runFlag3 = false;
			SetThreeCross();
			MonoBehaviour.print("run");
		}
	}

	public void SetThreeCross()
	{
		for (int i = 0; i < roadList.roadList.Length; i++)
		{
			if (roadList.roadList[i].roadPointList[0].crossFlag && CheckThree(roadList.roadList[i].roadPointList[0]))
			{
				roadList.roadList[i].roadPointList[0].threeCrossFlag = true;
			}
			if (roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1].crossFlag && CheckThree(roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1]))
			{
				roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1].threeCrossFlag = true;
			}
		}
	}

	public bool CheckThree(RoadPointNew point)
	{
		for (int i = 0; i < 4; i++)
		{
			if (point.GetLinkPoint(i) == null)
			{
				return true;
			}
		}
		return false;
	}

	public void SetCrossInfo()
	{
		for (int i = 0; i < roadList.roadList.Length; i++)
		{
			if (roadList.roadList[i].roadPointList[0].crossFlag)
			{
				labeledFlag = false;
				for (int j = 1; j < roadList.roadList[i].roadPointList[0].linkPoint.Length; j++)
				{
					if (roadList.roadList[i].roadPointList[0].GetLinkPoint(j) != null && roadList.roadList[i].roadPointList[0].GetLinkPoint(j).nsDirectionFlag)
					{
						labeledFlag = true;
						break;
					}
				}
				if (!labeledFlag)
				{
					roadList.roadList[i].roadPointList[0].nsDirectionFlag = true;
					if (roadList.roadList[i].roadPointList[0].linkPoint[1].road != null)
					{
						roadList.roadList[i].roadPointList[0].GetLinkPoint(1).nsDirectionFlag = true;
					}
				}
			}
			if (!roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1].crossFlag)
			{
				continue;
			}
			labeledFlag = false;
			for (int k = 1; k < roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1].linkPoint.Length; k++)
			{
				if (roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1].GetLinkPoint(k) != null && roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1].GetLinkPoint(k).nsDirectionFlag)
				{
					labeledFlag = true;
					break;
				}
			}
			if (!labeledFlag)
			{
				if (roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1].linkPoint[2].road != null)
				{
					roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1].GetLinkPoint(2).nsDirectionFlag = true;
				}
				if (roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1].linkPoint[3].road != null)
				{
					roadList.roadList[i].roadPointList[roadList.roadList[i].roadPointList.Length - 1].GetLinkPoint(3).nsDirectionFlag = true;
				}
			}
		}
	}

	public void AddDistance()
	{
		for (int i = 0; i < roadList.roadList.Length; i++)
		{
			for (int j = 0; j < roadList.roadList[i].roadPointList.Length; j++)
			{
				roadList.roadList[i].roadPointList[j].roadDistance = new float[roadList.roadList[i].roadPointList[j].linkPoint.Length];
				for (int k = 0; k < roadList.roadList[i].roadPointList[j].linkPoint.Length; k++)
				{
					if (roadList.roadList[i].roadPointList[j].linkPoint[k].road != null)
					{
						roadList.roadList[i].roadPointList[j].roadDistance[k] = Vector3.Distance(roadList.roadList[i].roadPointList[j].position, roadList.roadList[i].roadPointList[j].linkPoint[k].road.roadPointList[roadList.roadList[i].roadPointList[j].linkPoint[k].pointIndex].position);
					}
				}
			}
		}
	}
}
