using System.Collections.Generic;
using UnityEngine;

public class VirtualminiMapController : MonoBehaviour
{
	public static VirtualminiMapController instance;

	public BlockMapDateNew blockMap;

	public roadInfoListNew roadList;

	public BlockDateNew curBlock;

	public BlockDateNew preBlock;

	public RoadInfoNew curWay;

	public RoadInfoNew preWay;

	public RoadPointNew point1;

	public RoadPointNew point2;

	public LocateInfoNew playerLocation;

	public LocateInfoNew targetLocation;

	public RoadPointNew prePoint1;

	public List<LocateInfoNew> npcProducePos;

	public bool crossFlag;

	public bool targetCrossFlag;

	public int npcProduceDis;

	public bool changeBlockFlag;

	public float appearCount;

	public List<RoadPointNew> tempPath;

	public float countTime;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
	}

	public void CheckInCurBlock(BlockDateNew curBlock, Vector3 pos)
	{
		float num = float.PositiveInfinity;
		for (int i = 0; i < curBlock.roadList.Length; i++)
		{
			for (int j = 0; j < curBlock.roadList[i].roadPointList.Length - 1; j++)
			{
				if (ToolFunction.isForward(curBlock.roadList[i].roadPointList[j].position - pos, curBlock.roadList[i].roadPointList[j].forward) != ToolFunction.isForward(curBlock.roadList[i].roadPointList[j + 1].position - pos, curBlock.roadList[i].roadPointList[j + 1].forward))
				{
					Vector2 vector = ToolFunction.InverseXZ(curBlock.roadList[i].roadPointList[j].position, pos, curBlock.roadList[i].roadPointList[j].forward, curBlock.roadList[i].roadPointList[j].right);
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
		if (preWay == null)
		{
			preWay = curWay;
		}
		if (!(preWay != curWay))
		{
			return;
		}
		for (int k = 0; k < preWay.roadPointList.Length - 1; k++)
		{
			if (ToolFunction.isForward(preWay.roadPointList[k].position - pos, preWay.roadPointList[k].forward) != ToolFunction.isForward(preWay.roadPointList[k + 1].position - pos, preWay.roadPointList[k + 1].forward))
			{
				Vector2 vector = ToolFunction.InverseXZ(preWay.roadPointList[k].position, pos, preWay.roadPointList[k].forward, preWay.roadPointList[k].right);
				if (Mathf.Abs(vector.x) < num && Mathf.Abs(vector.x) < 30f)
				{
					crossFlag = false;
					point1 = preWay.roadPointList[k];
					point2 = preWay.roadPointList[k + 1];
					prePoint1 = playerLocation.point1;
					playerLocation.point1 = point1;
					playerLocation.point2 = point2;
					playerLocation.distanceFromPoint1 = Mathf.Abs(vector.y);
					curWay = point1.roadInfo;
					num = Mathf.Abs(vector.x);
				}
			}
		}
		preWay = curWay;
	}

	public void GetPlayerLocation(Vector3 pos)
	{
		countTime = 0f;
		curBlock = blockMap.GetBlockDate(pos);
		crossFlag = true;
		curWay = null;
		point1 = null;
		point2 = null;
		CheckInCurBlock(curBlock, pos);
		if (crossFlag)
		{
			if ((pos.x % 100f + 100f) % 100f < 20f && pos.x > -700f)
			{
				curBlock = blockMap.GetBlockDate(pos - new Vector3(23f, 0f, 0f));
				CheckInCurBlock(curBlock, pos);
			}
			else if ((pos.x % 100f + 100f) % 100f > 80f && pos.x < 700f)
			{
				curBlock = blockMap.GetBlockDate(pos + new Vector3(23f, 0f, 0f));
				CheckInCurBlock(curBlock, pos);
			}
		}
		if (crossFlag)
		{
			if ((pos.z % 100f + 100f) % 100f < 20f && pos.z > -800f)
			{
				curBlock = blockMap.GetBlockDate(pos - new Vector3(0f, 0f, 23f));
				CheckInCurBlock(curBlock, pos);
			}
			else if ((pos.z % 100f + 100f) % 100f > 80f && pos.z < 800f)
			{
				curBlock = blockMap.GetBlockDate(pos + new Vector3(0f, 0f, 23f));
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

	public void GetTargetLocation(Vector3 pos)
	{
		countTime = 0f;
		curBlock = blockMap.GetBlockDate(pos);
		targetCrossFlag = true;
		curWay = null;
		point1 = null;
		point2 = null;
		CheckTargetInCurBlock(curBlock, pos);
		if (targetCrossFlag)
		{
			if (pos.x % 100f < 20f && pos.x > -700f)
			{
				curBlock = blockMap.GetBlockDate(pos - new Vector3(23f, 0f, 0f));
				CheckTargetInCurBlock(curBlock, pos);
			}
			else if (pos.x % 100f > 80f && pos.x < 700f)
			{
				curBlock = blockMap.GetBlockDate(pos + new Vector3(23f, 0f, 0f));
				CheckTargetInCurBlock(curBlock, pos);
			}
		}
		if (targetCrossFlag)
		{
			if ((pos.z % 100f + 100f) % 100f < 20f && pos.z > -800f)
			{
				curBlock = blockMap.GetBlockDate(pos - new Vector3(0f, 0f, 23f));
				CheckTargetInCurBlock(curBlock, pos);
			}
			else if ((pos.z % 100f + 100f) % 100f > 80f && pos.z < 800f)
			{
				curBlock = blockMap.GetBlockDate(pos + new Vector3(0f, 0f, 23f));
				CheckTargetInCurBlock(curBlock, pos);
			}
		}
		if (targetCrossFlag && targetLocation.point1 != null && targetLocation.point1.roadInfo != null)
		{
			if (targetLocation.point1 == targetLocation.point1.roadInfo.roadPointList[0])
			{
				targetLocation.distanceFromPoint1 = 0f;
			}
			else if (targetLocation.point1 == targetLocation.point1.roadInfo.roadPointList[targetLocation.point1.roadInfo.roadPointList.Length - 2])
			{
				targetLocation.point1 = targetLocation.point1.roadInfo.roadPointList[targetLocation.point1.roadInfo.roadPointList.Length - 1];
				targetLocation.point2 = targetLocation.point1.roadInfo.roadPointList[targetLocation.point1.roadInfo.roadPointList.Length - 2];
				targetLocation.distanceFromPoint1 = 0f;
			}
		}
	}

	public void CheckTargetInCurBlock(BlockDateNew curBlock, Vector3 pos)
	{
		float num = float.PositiveInfinity;
		for (int i = 0; i < curBlock.roadList.Length; i++)
		{
			for (int j = 0; j < curBlock.roadList[i].roadPointList.Length - 1; j++)
			{
				if (ToolFunction.isForward(curBlock.roadList[i].roadPointList[j].position - pos, curBlock.roadList[i].roadPointList[j].forward) != ToolFunction.isForward(curBlock.roadList[i].roadPointList[j + 1].position - pos, curBlock.roadList[i].roadPointList[j + 1].forward))
				{
					Vector2 vector = ToolFunction.InverseXZ(curBlock.roadList[i].roadPointList[j].position, pos, curBlock.roadList[i].roadPointList[j].forward, curBlock.roadList[i].roadPointList[j].right);
					if (Mathf.Abs(vector.x) < num && Mathf.Abs(vector.x) < 50f)
					{
						targetCrossFlag = false;
						point1 = curBlock.roadList[i].roadPointList[j];
						point2 = curBlock.roadList[i].roadPointList[j + 1];
						prePoint1 = targetLocation.point1;
						targetLocation.point1 = point1;
						targetLocation.point2 = point2;
						targetLocation.distanceFromPoint1 = Mathf.Abs(vector.y);
						curWay = point1.roadInfo;
						num = Mathf.Abs(vector.x);
					}
				}
			}
		}
	}
}
