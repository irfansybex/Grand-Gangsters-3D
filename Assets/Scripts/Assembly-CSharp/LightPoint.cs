using System;
using System.Collections.Generic;

[Serializable]
public class LightPoint
{
	public List<RoadPointInfo> CrossPoint = new List<RoadPointInfo>();

	public List<RoadPointInfo> dummyCrossPoint = new List<RoadPointInfo>();
}
