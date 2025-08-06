using System.Collections.Generic;
using UnityEngine;

public class RoadPathInfo : MonoBehaviour
{
	public VPoint[] VP;

	public DP[] DP;

	public static RoadPathInfo instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public Vector2 FindTwoWayShortCrosspoint(RoadPointNew start, RoadPointNew end)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < VP.Length; i++)
		{
			if (roadInfoListNew.instance.roadList[VP[i].roadIndex].roadPointList[VP[i].pointIndex] == start.roadInfo.roadPointList[0])
			{
				num = i;
			}
			if (roadInfoListNew.instance.roadList[VP[i].roadIndex].roadPointList[VP[i].pointIndex] == start.roadInfo.roadPointList[start.roadInfo.roadPointList.Length - 1])
			{
				num2 = i;
			}
			if (roadInfoListNew.instance.roadList[VP[i].roadIndex].roadPointList[VP[i].pointIndex] == end.roadInfo.roadPointList[0])
			{
				num3 = i;
			}
			if (roadInfoListNew.instance.roadList[VP[i].roadIndex].roadPointList[VP[i].pointIndex] == end.roadInfo.roadPointList[end.roadInfo.roadPointList.Length - 1])
			{
				num4 = i;
			}
		}
		int num5 = num;
		int num6 = num3;
		if (DP[num5].Dis[num6] > DP[num2].Dis[num3])
		{
			num5 = num2;
			num6 = num3;
		}
		if (DP[num5].Dis[num6] > DP[num].Dis[num4])
		{
			num5 = num;
			num6 = num4;
		}
		if (DP[num5].Dis[num6] > DP[num2].Dis[num4])
		{
			num5 = num2;
			num6 = num4;
		}
		return new Vector2(VP[num5].pointIndex, VP[num6].pointIndex);
	}

	public List<RoadPointNew> Getpath(RoadPointNew start, RoadPointNew end)
	{
		int num = 0;
		int num2 = 0;
		List<RoadPointNew> list = new List<RoadPointNew>();
		for (int i = 0; i < start.roadInfo.roadPointList.Length; i++)
		{
			if (start.roadInfo.roadPointList[i] == start)
			{
				num = i;
			}
		}
		for (int j = 0; j < end.roadInfo.roadPointList.Length; j++)
		{
			if (end.roadInfo.roadPointList[j] == end)
			{
				num2 = j;
			}
		}
		if (start.roadInfo == end.roadInfo)
		{
			if (num > num2)
			{
				for (int num3 = num; num3 >= num2; num3--)
				{
					list.Add(start.roadInfo.roadPointList[num3]);
				}
			}
			else
			{
				for (int k = num; k <= num2; k++)
				{
					list.Add(start.roadInfo.roadPointList[k]);
				}
			}
			return list;
		}
		return showshortestpath(start, end);
	}

	public List<RoadPointNew> showshortestpath(RoadPointNew start, RoadPointNew end)
	{
		List<RoadPointNew> list = new List<RoadPointNew>();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		for (int i = 0; i < start.roadInfo.roadPointList.Length; i++)
		{
			if (start.roadInfo.roadPointList[i] == start)
			{
				num5 = i;
			}
		}
		for (int j = 0; j < end.roadInfo.roadPointList.Length; j++)
		{
			if (end.roadInfo.roadPointList[j] == end)
			{
				num6 = j;
			}
		}
		for (int k = 0; k < VP.Length; k++)
		{
			if (roadInfoListNew.instance.roadList[VP[k].roadIndex].roadPointList[VP[k].pointIndex] == start.roadInfo.roadPointList[0])
			{
				num = k;
			}
			if (roadInfoListNew.instance.roadList[VP[k].roadIndex].roadPointList[VP[k].pointIndex] == start.roadInfo.roadPointList[start.roadInfo.roadPointList.Length - 1])
			{
				num2 = k;
			}
			if (roadInfoListNew.instance.roadList[VP[k].roadIndex].roadPointList[VP[k].pointIndex] == end.roadInfo.roadPointList[0])
			{
				num3 = k;
			}
			if (roadInfoListNew.instance.roadList[VP[k].roadIndex].roadPointList[VP[k].pointIndex] == end.roadInfo.roadPointList[end.roadInfo.roadPointList.Length - 1])
			{
				num4 = k;
			}
		}
		int num7 = num;
		int num8 = num3;
		if (DP[num7].Dis[num8] > DP[num2].Dis[num3])
		{
			num7 = num2;
			num8 = num3;
		}
		if (DP[num7].Dis[num8] > DP[num].Dis[num4])
		{
			num7 = num;
			num8 = num4;
		}
		if (DP[num7].Dis[num8] > DP[num2].Dis[num4])
		{
			num7 = num2;
			num8 = num4;
		}
		list.Add(start);
		if (VP[num7].pointIndex == 0)
		{
			for (int num9 = num5; num9 > 1; num9--)
			{
				list.Add(start.roadInfo.roadPointList[num9 - 1]);
			}
		}
		else
		{
			for (int l = num5; l < start.roadInfo.roadPointList.Length - 2; l++)
			{
				list.Add(start.roadInfo.roadPointList[l + 1]);
			}
		}
		List<RoadPointNew> list2 = new List<RoadPointNew>();
		list2.Add(roadInfoListNew.instance.roadList[VP[num7].roadIndex].roadPointList[VP[num7].pointIndex]);
		int num10;
		for (num10 = DP[num7].Pot[num8]; num10 != num8; num10 = DP[num10].Pot[num8])
		{
			list2.Add(roadInfoListNew.instance.roadList[VP[num10].roadIndex].roadPointList[VP[num10].pointIndex]);
		}
		list2.Add(roadInfoListNew.instance.roadList[VP[num10].roadIndex].roadPointList[VP[num10].pointIndex]);
		for (int m = 1; m < list2.Count; m++)
		{
			if (list2[m - 1].roadInfo == list2[m].roadInfo)
			{
				if (list2[m - 1] == list2[m - 1].roadInfo.roadPointList[0])
				{
					for (int n = 0; n < list2[m - 1].roadInfo.roadPointList.Length - 1; n++)
					{
						list.Add(list2[m - 1].roadInfo.roadPointList[n]);
					}
					continue;
				}
				for (int num11 = list2[m - 1].roadInfo.roadPointList.Length - 1; num11 > 0; num11--)
				{
					list.Add(list2[m - 1].roadInfo.roadPointList[num11]);
				}
			}
			else
			{
				list.Add(list2[m - 1]);
				if (m == list2.Count - 1)
				{
					list.Add(list2[m]);
				}
			}
		}
		if (VP[num8].pointIndex == 0)
		{
			for (int num12 = 1; num12 <= num6; num12++)
			{
				list.Add(end.roadInfo.roadPointList[num12]);
			}
		}
		else
		{
			for (int num13 = end.roadInfo.roadPointList.Length - 2; num13 >= num6; num13--)
			{
				list.Add(end.roadInfo.roadPointList[num13]);
			}
		}
		return list;
	}
}
