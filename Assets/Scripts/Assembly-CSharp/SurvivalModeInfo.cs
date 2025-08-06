using System.Collections.Generic;
using UnityEngine;

public class SurvivalModeInfo : MonoBehaviour
{
	public Vector3 playerDefaultPos;

	public List<Transform> startPointList;

	public List<RoadPointInfo> firstPointList;

	public int levelNum;

	public NPCGroupInfo[] npcGroupList;
}
