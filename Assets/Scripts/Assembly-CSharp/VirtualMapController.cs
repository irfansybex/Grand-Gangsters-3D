using System.Collections.Generic;
using UnityEngine;

public class VirtualMapController : MonoBehaviour
{
	public BlockMapDateNew blockMap;

	public roadInfoListNew roadList;

	public PlayerController player;

	public BlockDateNew curBlock;

	public BlockDateNew preBlock;

	public RoadInfoNew curWay;

	public List<BlockDateNew> curBuildingBlockList;

	public List<BlockDateNew> preBuildingBlockList;

	public List<BlockDateNew> appearBlock;

	public List<BlockDateNew> disappearBlock;

	public BuildingMatPoolList buildingMatPoolList;

	public List<BlockDateNew> curItemBlockList;

	public List<BlockDateNew> preItemBlockList;

	public List<BlockDateNew> appearItemBlock;

	public List<BlockDateNew> disappearItemBlock;

	public Material[] appearMatList;

	public Material[] disappearMatList;

	public BuildingPoolList buildingPoolList;

	public BuildingPool playerWallPool;

	public GameObject testTarget;

	public RoadPointNew point1;

	public RoadPointNew point2;

	public LocateInfoNew playerLocation;

	public RoadPointNew prePoint1;

	public List<LocateInfoNew> npcProducePos = new List<LocateInfoNew>();

	public bool crossFlag;

	public bool ffFlag;

	public int npcProduceDis;

	public bool changeBlockFlag;

	public float appearCount;

	public bool disappearFlag;

	public float disappearCount;

	public float countTime = 2f;

	public LocateInfoNew[] locateNewList;

	public int locateNewListIndex;

	private bool containFlag;

	private void Update()
	{
		if (changeBlockFlag && appearCount < 0.95f)
		{
			appearCount += Time.deltaTime;
			for (int i = 0; i < appearMatList.Length; i++)
			{
				appearMatList[i].color = new Color(1f, 1f, 1f, appearCount);
			}
			if (appearCount > 0.95f)
			{
				changeBlockFlag = false;
				for (int j = 0; j < appearBlock.Count; j++)
				{
					for (int k = 0; k < appearBlock[j].enableBuildingList.Count; k++)
					{
						appearBlock[j].enableBuildingList[k].GetComponent<Renderer>().sharedMaterial = buildingMatPoolList.matPoolList[appearBlock[j].buildingList[k].index].targetMat;
					}
				}
				appearBlock.Clear();
				if (!GlobalDefine.smallPhoneFlag)
				{
					for (int l = 0; l < appearItemBlock.Count; l++)
					{
						for (int m = 0; m < appearItemBlock[l].enableItemList.Count; m++)
						{
							appearItemBlock[l].enableItemList[m].GetComponent<Renderer>().sharedMaterial = buildingMatPoolList.matPoolList[appearItemBlock[l].itemList[m].index].targetMat;
						}
					}
					appearItemBlock.Clear();
				}
			}
		}
		if (disappearFlag && disappearCount > 0.05f)
		{
			disappearCount -= Time.deltaTime;
			for (int n = 0; n < disappearMatList.Length; n++)
			{
				disappearMatList[n].color = new Color(1f, 1f, 1f, disappearCount);
			}
			if (disappearCount < 0.05f)
			{
				disappearFlag = false;
				for (int num = 0; num < disappearBlock.Count; num++)
				{
					for (int num2 = 0; num2 < disappearBlock[num].enableBuildingList.Count; num2++)
					{
						buildingPoolList.poolList[disappearBlock[num].buildingList[num2].index].RecycleObj(disappearBlock[num].enableBuildingList[num2]);
					}
					disappearBlock[num].enableBuildingList.Clear();
				}
				disappearBlock.Clear();
				if (!GlobalDefine.smallPhoneFlag)
				{
					for (int num3 = 0; num3 < disappearItemBlock.Count; num3++)
					{
						for (int num4 = 0; num4 < disappearItemBlock[num3].enableItemList.Count; num4++)
						{
							buildingPoolList.poolList[disappearItemBlock[num3].itemList[num4].index].RecycleObj(disappearItemBlock[num3].enableItemList[num4]);
						}
						disappearItemBlock[num3].enableItemList.Clear();
					}
					disappearItemBlock.Clear();
				}
			}
		}
		countTime += Time.deltaTime;
		if (countTime > 2f)
		{
			GetPlayerLocation(PlayerController.instance.transform);
		}
	}

	public void CheckInCurBlock(BlockDateNew curBlock, Transform pos)
	{
		float num = float.PositiveInfinity;
		for (int i = 0; i < curBlock.roadList.Length; i++)
		{
			for (int j = 0; j < curBlock.roadList[i].roadPointList.Length - 1; j++)
			{
				if (ToolFunction.isForward(curBlock.roadList[i].roadPointList[j].position - pos.position, curBlock.roadList[i].roadPointList[j].forward) != ToolFunction.isForward(curBlock.roadList[i].roadPointList[j + 1].position - pos.position, curBlock.roadList[i].roadPointList[j + 1].forward))
				{
					Vector2 vector = ToolFunction.InverseXZ(curBlock.roadList[i].roadPointList[j].position, pos.position, curBlock.roadList[i].roadPointList[j].forward, curBlock.roadList[i].roadPointList[j].right);
					if (Mathf.Abs(vector.x) < num && Mathf.Abs(vector.x) < 50f)
					{
						crossFlag = false;
						point1 = curBlock.roadList[i].roadPointList[j];
						point2 = curBlock.roadList[i].roadPointList[j + 1];
						prePoint1 = playerLocation.point1;
						playerLocation.point1 = point1;
						playerLocation.point2 = point2;
						playerLocation.distanceFromPoint1 = Mathf.Abs(vector.y);
						curWay = point1.roadInfo;
						num = Mathf.Abs(vector.x);
					}
				}
			}
		}
	}

	public void ChangeBlock(Transform pos)
	{
		changeBlockFlag = true;
		appearCount = 0f;
		disappearFlag = true;
		disappearCount = 1f;
		GetCurBlockList(pos);
		for (int i = 0; i < appearMatList.Length; i++)
		{
			appearMatList[i].color = new Color(1f, 1f, 1f, 0f);
			disappearMatList[i].color = new Color(1f, 1f, 1f, 1f);
		}
		if (disappearBlock != null)
		{
			for (int j = 0; j < disappearBlock.Count; j++)
			{
				if (disappearBlock[j].buildingList != null)
				{
					for (int k = 0; k < disappearBlock[j].enableBuildingList.Count; k++)
					{
						buildingPoolList.poolList[disappearBlock[j].buildingList[k].index].RecycleObj(disappearBlock[j].enableBuildingList[k]);
					}
					disappearBlock[j].enableBuildingList.Clear();
				}
				if (disappearBlock[j].enableCollectingList != null)
				{
					for (int l = 0; l < disappearBlock[j].enableCollectingList.Count; l++)
					{
						CollectingObjPool.instance.RecycleCollectingObj(disappearBlock[j].enableCollectingList[l].gameObject, disappearBlock[j].enableCollectingList[l].type);
					}
					disappearBlock[j].enableCollectingList.Clear();
				}
				if (disappearBlock[j].enablePlayerWallList != null)
				{
					for (int m = 0; m < disappearBlock[j].enablePlayerWallList.Count; m++)
					{
						playerWallPool.RecycleObj(disappearBlock[j].enablePlayerWallList[m]);
					}
					disappearBlock[j].enablePlayerWallList.Clear();
				}
			}
		}
		disappearBlock.Clear();
		for (int n = 0; n < preBuildingBlockList.Count; n++)
		{
			if (curBuildingBlockList.Contains(preBuildingBlockList[n]))
			{
				continue;
			}
			disappearBlock.Add(preBuildingBlockList[n]);
			if (preBuildingBlockList[n].buildingList != null)
			{
				for (int num = 0; num < preBuildingBlockList[n].enableBuildingList.Count; num++)
				{
					preBuildingBlockList[n].enableBuildingList[num].GetComponent<Renderer>().sharedMaterial = buildingMatPoolList.matPoolList[preBuildingBlockList[n].buildingList[num].index].disappearMat;
				}
			}
			if (preBuildingBlockList[n].enableCollectingList != null)
			{
				for (int num2 = 0; num2 < preBuildingBlockList[n].enableCollectingList.Count; num2++)
				{
					CollectingObjPool.instance.RecycleCollectingObj(preBuildingBlockList[n].enableCollectingList[num2].gameObject, preBuildingBlockList[n].enableCollectingList[num2].type);
				}
				preBuildingBlockList[n].enableCollectingList.Clear();
			}
			if (preBuildingBlockList[n].enablePlayerWallList != null)
			{
				for (int num3 = 0; num3 < preBuildingBlockList[n].enablePlayerWallList.Count; num3++)
				{
					playerWallPool.RecycleObj(preBuildingBlockList[n].enablePlayerWallList[num3]);
				}
				preBuildingBlockList[n].enablePlayerWallList.Clear();
			}
		}
		if (appearBlock.Count > 0)
		{
			for (int num4 = 0; num4 < appearBlock.Count; num4++)
			{
				for (int num5 = 0; num5 < appearBlock[num4].enableBuildingList.Count; num5++)
				{
					appearBlock[num4].enableBuildingList[num5].GetComponent<Renderer>().sharedMaterial = buildingMatPoolList.matPoolList[appearBlock[num4].buildingList[num5].index].targetMat;
				}
			}
		}
		appearBlock.Clear();
		for (int num6 = 0; num6 < curBuildingBlockList.Count; num6++)
		{
			if (preBuildingBlockList.Contains(curBuildingBlockList[num6]))
			{
				continue;
			}
			if (curBuildingBlockList[num6].buildingList != null)
			{
				appearBlock.Add(curBuildingBlockList[num6]);
				for (int num7 = 0; num7 < curBuildingBlockList[num6].buildingList.Length; num7++)
				{
					GameObject obj = buildingPoolList.poolList[curBuildingBlockList[num6].buildingList[num7].index].GetObj();
					obj.transform.position = curBuildingBlockList[num6].buildingList[num7].position;
					obj.transform.eulerAngles = curBuildingBlockList[num6].buildingList[num7].rotation;
					obj.GetComponent<Renderer>().sharedMaterial = buildingMatPoolList.matPoolList[curBuildingBlockList[num6].buildingList[num7].index].appearMat;
					curBuildingBlockList[num6].enableBuildingList.Add(obj);
				}
			}
			if (curBuildingBlockList[num6].collectingInfoList != null)
			{
				for (int num8 = 0; num8 < curBuildingBlockList[num6].collectingInfoList.Length; num8++)
				{
					switch (curBuildingBlockList[num6].collectingInfoList[num8].type)
					{
					case COLLECTTYPE.CAR:
						if (GlobalInf.collectCarData[curBuildingBlockList[num6].collectingInfoList[num8].index] == 0)
						{
							curBuildingBlockList[num6].enableCollectingList.Add(SetCollectingObj(curBuildingBlockList[num6].collectingInfoList[num8], curBuildingBlockList[num6]));
						}
						break;
					case COLLECTTYPE.HANDGUN:
						if (GlobalInf.collectHandGunData[curBuildingBlockList[num6].collectingInfoList[num8].index] == 0)
						{
							curBuildingBlockList[num6].enableCollectingList.Add(SetCollectingObj(curBuildingBlockList[num6].collectingInfoList[num8], curBuildingBlockList[num6]));
						}
						break;
					case COLLECTTYPE.MACHINEGUN:
						if (GlobalInf.collectMachineGunData[curBuildingBlockList[num6].collectingInfoList[num8].index] == 0)
						{
							curBuildingBlockList[num6].enableCollectingList.Add(SetCollectingObj(curBuildingBlockList[num6].collectingInfoList[num8], curBuildingBlockList[num6]));
						}
						break;
					}
				}
			}
			if (curBuildingBlockList[num6].enablePlayerWallList != null)
			{
				for (int num9 = 0; num9 < curBuildingBlockList[num6].playerWall.Length; num9++)
				{
					GameObject obj = playerWallPool.GetObj();
					obj.transform.position = curBuildingBlockList[num6].playerWall[num9].position;
					obj.transform.eulerAngles = curBuildingBlockList[num6].playerWall[num9].rotation;
					curBuildingBlockList[num6].enablePlayerWallList.Add(obj);
				}
			}
		}
		preBuildingBlockList.Clear();
		for (int num10 = 0; num10 < curBuildingBlockList.Count; num10++)
		{
			preBuildingBlockList.Add(curBuildingBlockList[num10]);
		}
		if (GameSenceTutorialController.instance == null)
		{
			for (int num11 = 0; num11 < curBuildingBlockList.Count; num11++)
			{
				for (int num12 = 0; num12 < curBuildingBlockList[num11].motorPos.Count; num12++)
				{
					AICarPoolController.instance.GetMotor(curBuildingBlockList[num11].motorPos[num12], curBuildingBlockList[num11].motorRot[num12]);
				}
			}
		}
		if (!GlobalDefine.smallPhoneFlag)
		{
			ChangeItemBlock();
		}
	}

	public void CheckMotor()
	{
		if (!(GameSenceTutorialController.instance == null))
		{
			return;
		}
		for (int i = 0; i < curBuildingBlockList.Count; i++)
		{
			for (int j = 0; j < curBuildingBlockList[i].motorPos.Count; j++)
			{
				AICarPoolController.instance.GetMotor(curBuildingBlockList[i].motorPos[j], curBuildingBlockList[i].motorRot[j]);
			}
		}
	}

	public void ChangeItemBlock()
	{
		if (disappearItemBlock != null)
		{
			for (int i = 0; i < disappearItemBlock.Count; i++)
			{
				if (disappearItemBlock[i].itemList != null)
				{
					for (int j = 0; j < disappearItemBlock[i].enableItemList.Count; j++)
					{
						buildingPoolList.poolList[disappearItemBlock[i].itemList[j].index].RecycleObj(disappearItemBlock[i].enableItemList[j]);
					}
					disappearItemBlock[i].enableItemList.Clear();
				}
			}
		}
		disappearItemBlock.Clear();
		for (int k = 0; k < preItemBlockList.Count; k++)
		{
			if (curItemBlockList.Contains(preItemBlockList[k]))
			{
				continue;
			}
			disappearItemBlock.Add(preItemBlockList[k]);
			if (preItemBlockList[k].itemList != null)
			{
				for (int l = 0; l < preItemBlockList[k].enableItemList.Count; l++)
				{
					preItemBlockList[k].enableItemList[l].GetComponent<Renderer>().sharedMaterial = buildingMatPoolList.matPoolList[preItemBlockList[k].itemList[l].index].disappearMat;
				}
			}
		}
		if (appearItemBlock.Count > 0)
		{
			for (int m = 0; m < appearItemBlock.Count; m++)
			{
				for (int n = 0; n < appearItemBlock[m].enableItemList.Count; n++)
				{
					appearItemBlock[m].enableItemList[n].GetComponent<Renderer>().sharedMaterial = buildingMatPoolList.matPoolList[appearItemBlock[m].itemList[n].index].targetMat;
				}
			}
		}
		appearItemBlock.Clear();
		for (int num = 0; num < curItemBlockList.Count; num++)
		{
			if (!preItemBlockList.Contains(curItemBlockList[num]) && curItemBlockList[num].itemList != null)
			{
				appearItemBlock.Add(curItemBlockList[num]);
				for (int num2 = 0; num2 < curItemBlockList[num].itemList.Length; num2++)
				{
					GameObject obj = buildingPoolList.poolList[curItemBlockList[num].itemList[num2].index].GetObj();
					obj.transform.position = curItemBlockList[num].itemList[num2].position;
					obj.transform.eulerAngles = curItemBlockList[num].itemList[num2].rotation;
					obj.GetComponent<Renderer>().sharedMaterial = buildingMatPoolList.matPoolList[curItemBlockList[num].itemList[num2].index].appearMat;
					curItemBlockList[num].enableItemList.Add(obj);
				}
			}
		}
		preItemBlockList.Clear();
		for (int num3 = 0; num3 < curItemBlockList.Count; num3++)
		{
			preItemBlockList.Add(curItemBlockList[num3]);
		}
	}

	public CollectingObject SetCollectingObj(CollectingInfo info, BlockDateNew block)
	{
		CollectingObject component = CollectingObjPool.instance.GetCollectingObj(info.type).GetComponent<CollectingObject>();
		component.transform.position = info.pos;
		component.index = info.index;
		component.gameObject.SetActiveRecursively(true);
		Color color = component.label.GetComponent<Renderer>().sharedMaterial.color;
		component.label.GetComponent<Renderer>().sharedMaterial.color = new Color(color.r, color.g, color.b, 1f);
		component.label.transform.localScale = Vector3.one;
		color = component.box.GetComponent<Renderer>().sharedMaterial.color;
		component.box.GetComponent<Renderer>().sharedMaterial.color = new Color(color.r, color.g, color.b, 1f);
		component.box.transform.localScale = Vector3.one;
		component.blockData = block;
		return component;
	}

	public void GetCurBlockList(Transform pos)
	{
		curBuildingBlockList.Clear();
		Vector2 blockIndex = blockMap.GetBlockIndex(pos.position);
		int num = (int)blockIndex.x;
		int num2 = (int)blockIndex.y;
		if (!GlobalDefine.smallPhoneFlag)
		{
			AddInCurBlock(num - 2, num2 - 2);
			AddInCurBlock(num - 2, num2 - 1);
			AddInCurBlock(num - 2, num2);
			AddInCurBlock(num - 2, num2 + 1);
			AddInCurBlock(num - 2, num2 + 2);
			AddInCurBlock(num + 2, num2 - 2);
			AddInCurBlock(num + 2, num2 - 1);
			AddInCurBlock(num + 2, num2);
			AddInCurBlock(num + 2, num2 + 1);
			AddInCurBlock(num + 2, num2 + 2);
			AddInCurBlock(num - 1, num2 - 2);
			AddInCurBlock(num - 1, num2 + 2);
			AddInCurBlock(num, num2 - 2);
			AddInCurBlock(num, num2 + 2);
			AddInCurBlock(num + 1, num2 - 2);
			AddInCurBlock(num + 1, num2 + 2);
			curItemBlockList.Clear();
			AddInCurItemBlock(num - 1, num2 - 1);
			AddInCurItemBlock(num - 1, num2);
			AddInCurItemBlock(num - 1, num2 + 1);
			AddInCurItemBlock(num, num2 - 1);
			AddInCurItemBlock(num, num2);
			AddInCurItemBlock(num, num2 + 1);
			AddInCurItemBlock(num + 1, num2 - 1);
			AddInCurItemBlock(num + 1, num2);
			AddInCurItemBlock(num + 1, num2 + 1);
		}
		AddInCurBlock(num - 1, num2 - 1);
		AddInCurBlock(num - 1, num2);
		AddInCurBlock(num - 1, num2 + 1);
		AddInCurBlock(num, num2 - 1);
		AddInCurBlock(num, num2);
		AddInCurBlock(num, num2 + 1);
		AddInCurBlock(num + 1, num2 - 1);
		AddInCurBlock(num + 1, num2);
		AddInCurBlock(num + 1, num2 + 1);
	}

	public void AddInCurBlock(int x, int y)
	{
		if (x >= 0 && x < blockMap.blockLine[0].blockLine.Length && y >= 0 && y < blockMap.blockLine.Length)
		{
			curBuildingBlockList.Add(blockMap.blockLine[y].blockLine[x]);
		}
	}

	public void AddInCurItemBlock(int x, int y)
	{
		if (x >= 0 && x < blockMap.blockLine[0].blockLine.Length && y >= 0 && y < blockMap.blockLine.Length)
		{
			curItemBlockList.Add(blockMap.blockLine[y].blockLine[x]);
		}
	}

	public void GetPlayerLocation(Transform pos)
	{
		countTime = 0f;
		curBlock = blockMap.GetBlockDate(pos.position);
		if (curBlock != preBlock)
		{
			ChangeBlock(pos);
			preBlock = curBlock;
		}
		crossFlag = true;
		curWay = null;
		point1 = null;
		point2 = null;
		CheckInCurBlock(curBlock, pos);
		if (crossFlag)
		{
			if ((pos.position.x % 100f + 100f) % 100f < 20f && pos.position.x > -700f)
			{
				curBlock = blockMap.GetBlockDate(pos.position - new Vector3(23f, 0f, 0f));
				CheckInCurBlock(curBlock, pos);
			}
			else if ((pos.position.x % 100f + 100f) % 100f > 80f && pos.position.x < 700f)
			{
				curBlock = blockMap.GetBlockDate(pos.position + new Vector3(23f, 0f, 0f));
				CheckInCurBlock(curBlock, pos);
			}
		}
		if (crossFlag)
		{
			if ((pos.position.z % 100f + 100f) % 100f < 20f && pos.position.z > -800f)
			{
				curBlock = blockMap.GetBlockDate(pos.position - new Vector3(0f, 0f, 23f));
				CheckInCurBlock(curBlock, pos);
			}
			else if ((pos.position.z % 100f + 100f) % 100f > 80f && pos.position.z < 800f)
			{
				curBlock = blockMap.GetBlockDate(pos.position + new Vector3(0f, 0f, 23f));
				CheckInCurBlock(curBlock, pos);
			}
		}
		if (crossFlag && playerLocation.point1 != null && playerLocation.point1.roadInfo != null)
		{
			if (playerLocation.point1 == playerLocation.point1.roadInfo.roadPointList[0])
			{
				playerLocation.distanceFromPoint1 = 0f;
			}
			else if (playerLocation.point1 == playerLocation.point1.roadInfo.roadPointList[playerLocation.point1.roadInfo.roadPointList.Length - 2])
			{
				playerLocation.point1 = playerLocation.point1.roadInfo.roadPointList[playerLocation.point1.roadInfo.roadPointList.Length - 1];
				playerLocation.point2 = playerLocation.point1.roadInfo.roadPointList[playerLocation.point1.roadInfo.roadPointList.Length - 2];
				playerLocation.distanceFromPoint1 = 0f;
			}
		}
	}

	public void GetNpcProducePos(Transform pos, float dis)
	{
	}

	public void GetNpcProducePosByRoad(float dis)
	{
		npcProducePos.Clear();
		locateNewListIndex = 0;
		if (Mathf.Abs(playerLocation.distanceFromPoint1) > dis)
		{
			locateNewList[locateNewListIndex].point1 = playerLocation.point1;
			locateNewList[locateNewListIndex].point2 = playerLocation.point2;
			locateNewList[locateNewListIndex].distanceFromPoint1 = playerLocation.distanceFromPoint1 - dis;
			if (locateNewList[locateNewListIndex].distanceFromPoint1 < 0f)
			{
				locateNewList[locateNewListIndex].distanceFromPoint1 = 0f;
			}
			npcProducePos.Add(locateNewList[locateNewListIndex]);
			locateNewListIndex++;
		}
		else
		{
			for (int i = 0; i < playerLocation.point1.linkPoint.Length; i++)
			{
				if (playerLocation.point1.GetLinkPoint(i) != null && playerLocation.point1.GetLinkPoint(i) != playerLocation.point2)
				{
					if (crossFlag || playerLocation.point1.linkPoint[i].road != playerLocation.point1.roadInfo)
					{
						GetPorduceLocate(playerLocation.point1, playerLocation.point2, dis - playerLocation.distanceFromPoint1);
					}
					else
					{
						GetPorduceLocate(playerLocation.point1, playerLocation.point2, dis - playerLocation.distanceFromPoint1);
					}
				}
			}
		}
		if (locateNewListIndex >= locateNewList.Length)
		{
			return;
		}
		if (Mathf.Abs(playerLocation.point1.GetRoadPointDistance(playerLocation.point2) - playerLocation.distanceFromPoint1) > dis)
		{
			locateNewList[locateNewListIndex].point1 = playerLocation.point2;
			locateNewList[locateNewListIndex].point2 = playerLocation.point1;
			locateNewList[locateNewListIndex].distanceFromPoint1 = playerLocation.point1.GetRoadPointDistance(playerLocation.point2) - playerLocation.distanceFromPoint1 - dis;
			if (locateNewList[locateNewListIndex].distanceFromPoint1 < 0f)
			{
				locateNewList[locateNewListIndex].distanceFromPoint1 = 0f;
			}
			npcProducePos.Add(locateNewList[locateNewListIndex]);
			locateNewListIndex++;
			return;
		}
		for (int j = 0; j < playerLocation.point2.linkPoint.Length; j++)
		{
			if (playerLocation.point2.GetLinkPoint(j) != null && playerLocation.point2.GetLinkPoint(j) != playerLocation.point1)
			{
				if (crossFlag || playerLocation.point2.linkPoint[j].road != playerLocation.point2.roadInfo)
				{
					GetPorduceLocate(playerLocation.point2, playerLocation.point1, dis - (playerLocation.point1.GetRoadPointDistance(playerLocation.point2) - playerLocation.distanceFromPoint1));
				}
				else
				{
					GetPorduceLocate(playerLocation.point2, playerLocation.point1, dis - (playerLocation.point1.GetRoadPointDistance(playerLocation.point2) - playerLocation.distanceFromPoint1));
				}
			}
		}
	}

	public void GetPorduceLocate(RoadPointNew curPoint, RoadPointNew prePoint, float remainDis)
	{
		if (locateNewListIndex >= locateNewList.Length)
		{
			return;
		}
		for (int i = 0; i < curPoint.linkPoint.Length; i++)
		{
			if (curPoint.GetLinkPoint(i) == null || curPoint.GetLinkPoint(i) == prePoint || (prePoint.linkPoint.Length > 2 && curPoint.GetLinkPoint(i).linkPoint.Length > 2))
			{
				continue;
			}
			if (curPoint.GetRoadPointDistance(curPoint.GetLinkPoint(i)) > remainDis)
			{
				if (curPoint.roadInfo == curPoint.GetLinkPoint(i).roadInfo)
				{
					locateNewList[locateNewListIndex].point1 = curPoint;
					locateNewList[locateNewListIndex].point2 = curPoint.GetLinkPoint(i);
					locateNewList[locateNewListIndex].distanceFromPoint1 = remainDis;
					if (locateNewList[locateNewListIndex].point2.GetPointIndex() != 0 && locateNewList[locateNewListIndex].point2.GetPointIndex() != locateNewList[locateNewListIndex].point2.roadInfo.roadPointList.Length - 1 && locateNewList[locateNewListIndex].point1.GetPointIndex() != 0 && locateNewList[locateNewListIndex].point1.GetPointIndex() != locateNewList[locateNewListIndex].point1.roadInfo.roadPointList.Length - 1)
					{
						if (locateNewList[locateNewListIndex].distanceFromPoint1 < 0f)
						{
							locateNewList[locateNewListIndex].distanceFromPoint1 = 0f;
						}
						npcProducePos.Add(locateNewList[locateNewListIndex]);
						locateNewListIndex++;
						if (locateNewListIndex >= locateNewList.Length)
						{
							break;
						}
					}
				}
				else
				{
					GetPorduceLocate(curPoint.GetLinkPoint(i), curPoint, remainDis);
				}
			}
			else if (curPoint.roadInfo == curPoint.GetLinkPoint(i).roadInfo)
			{
				GetPorduceLocate(curPoint.GetLinkPoint(i), curPoint, remainDis - curPoint.GetRoadPointDistance(curPoint.GetLinkPoint(i)));
			}
			else
			{
				GetPorduceLocate(curPoint.GetLinkPoint(i), curPoint, remainDis);
			}
		}
	}
}
