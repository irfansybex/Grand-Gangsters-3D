using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockDateNew
{
	public RoadInfoNew[] roadList;

	public BuildingInfo[] buildingList;

	public List<GameObject> enableBuildingList;

	public BuildingInfo[] itemList;

	public List<GameObject> enableItemList;

	public int aiRateIndex;

	public int maxAINum;

	public int maxCarNum;

	public List<CollectingObject> enableCollectingList;

	public CollectingInfo[] collectingInfoList;

	public PlayerWallInfo[] playerWall;

	public List<GameObject> enablePlayerWallList;

	public List<Vector3> motorPos;

	public List<Quaternion> motorRot;
}
