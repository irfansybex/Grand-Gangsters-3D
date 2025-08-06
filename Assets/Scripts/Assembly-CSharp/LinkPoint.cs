using System;

[Serializable]
public class LinkPoint
{
	public RoadInfoNew road;

	public int pointIndex;

	public LinkPoint(RoadInfoNew r, int i)
	{
		road = r;
		pointIndex = i;
	}
}
