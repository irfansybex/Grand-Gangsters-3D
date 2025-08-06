using UnityEngine;

public class GenPosInfo
{
	public Vector3 pos;

	public RoadPointNew curPos;

	public RoadPointNew prePos;

	public int path;

	public bool attackAIFlag;

	public bool polFlag;

	public NPCTYPE type;

	public int level;

	public GenPosInfo(Vector3 npcPosition, RoadPointNew curRoadPoint, RoadPointNew preRoadPoint, int pathInfo, bool attackFlag, bool policeFlag, NPCTYPE npcType, int npcLevel)
	{
		pos = npcPosition;
		curPos = curRoadPoint;
		prePos = preRoadPoint;
		path = pathInfo;
		attackAIFlag = attackFlag;
		polFlag = policeFlag;
		type = npcType;
		level = npcLevel;
	}
}
