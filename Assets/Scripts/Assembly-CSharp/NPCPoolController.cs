using System.Collections.Generic;
using UnityEngine;

public class NPCPoolController : MonoBehaviour
{
	public static NPCPoolController instance;

	public List<AIController> enableAIList;

	public List<AIController> disableAIList;

	public List<AIController> normalWhiteList;

	public List<AIController> normalWomenList;

	public List<AIController> normalBlackList;

	public List<AIController> ganstarWhiteList;

	public List<AIController> ganstarBlackList;

	public List<AIController> ganstarNakedList;

	public List<AIController> normalWhite2List;

	public List<AIController> normalWomen2List;

	public List<AIController> police1DisableList;

	public List<AIController> police2DisableList;

	public AIController normalWhitePreferb;

	public AIController normalWomenPreferb;

	public AIController normalBlackPreferb;

	public AIController ganstarWhitePreferb;

	public AIController ganstarBlackPreferb;

	public AIController ganstarNakedPreferb;

	public AIController police1Preferb;

	public AIController police2Preferb;

	public AIController normalWhite2Preferb;

	public AIController normalWomen2Preferb;

	private AIController tempAI;

	public float NPCRecycleDIstance;

	public int policeCount;

	public int policeNumCount = 1;

	public int[] normalAIHealthVal;

	public int[] normalAIFallingMoney;

	public float normalAIBulletRate;

	private int normalNPCRandomNum;

	private AIInfo tempInfo;

	private void Awake()
	{
		instance = this;
		instance.NPCRecycleDIstance = CitySenceController.instance.npcProduceDistance * CitySenceController.instance.npcProduceDistance * 4f;
		policeNumCount = 1;
	}

	public void GetNormalNPC(RoadPointNew curPoint, RoadPointNew prePoint, Vector3 pos, List<AIController> poolList, AIController preferb)
	{
		if (poolList.Count == 0)
		{
			if (CheckEmpty(pos))
			{
				tempAI = (AIController)Object.Instantiate(preferb);
				tempAI.transform.parent = base.transform;
				enableAIList.Add(tempAI);
				tempAI.healthCtl.maxHealthVal = normalAIHealthVal[GlobalInf.gameLevel];
				tempAI.fallingMoneyVal = normalAIFallingMoney[GlobalInf.gameLevel];
				tempAI.fallingBulletRate = normalAIBulletRate;
				tempAI.npcLevel = GlobalInf.gameLevel;
				tempAI.ResetAI(curPoint, prePoint, pos, false, false, false, false);
			}
		}
		else if (CheckEmpty(pos))
		{
			tempAI = poolList[0];
			poolList.RemoveAt(0);
			enableAIList.Add(tempAI);
			tempAI.healthCtl.maxHealthVal = normalAIHealthVal[GlobalInf.gameLevel];
			tempAI.fallingMoneyVal = normalAIFallingMoney[GlobalInf.gameLevel];
			tempAI.fallingBulletRate = normalAIBulletRate;
			tempAI.npcLevel = GlobalInf.gameLevel;
			tempAI.ResetAI(curPoint, prePoint, pos, false, false, false, false);
		}
	}

	public void GetNPC(RoadPointNew curPoint, RoadPointNew prePoint, Vector3 pos)
	{
		switch (normalNPCRandomNum)
		{
		case 0:
			GetNormalNPC(curPoint, prePoint, pos, normalWhiteList, normalWhitePreferb);
			break;
		case 1:
			GetNormalNPC(curPoint, prePoint, pos, normalWomenList, normalWomenPreferb);
			break;
		case 2:
			GetNormalNPC(curPoint, prePoint, pos, normalWhite2List, normalWhite2Preferb);
			break;
		case 3:
			GetNormalNPC(curPoint, prePoint, pos, normalWomen2List, normalWomen2Preferb);
			break;
		default:
			GetNormalNPC(curPoint, prePoint, pos, normalWhiteList, normalWhitePreferb);
			break;
		}
		normalNPCRandomNum = (normalNPCRandomNum + 1) % 4;
	}

	public void GetNPCInList(RoadPointNew curPoint, RoadPointNew prePoint, Vector3 pos, bool attackFlag, NPCTYPE type, int level, List<AIController> poolList, AIController preferb)
	{
		if (poolList.Count == 0)
		{
			if (CheckEmpty(pos))
			{
				if (type == NPCTYPE.POLICE1_HG || type == NPCTYPE.POLICE2_HG || type == NPCTYPE.POLICE2_MG)
				{
					policeNumCount++;
					policeCount++;
				}
				tempAI = (AIController)Object.Instantiate(preferb);
				tempAI.transform.parent = base.transform;
				enableAIList.Add(tempAI);
				tempAI.type = type;
				tempAI.npcLevel = level;
				tempInfo = AIInfoList.instance.GetAIInfo(type, level);
				tempAI.healthCtl.maxHealthVal = tempInfo.health;
				tempAI.handGunFlag = tempInfo.handGunFlag;
				tempAI.machineGunFlag = tempInfo.machineGunFlag;
				if (tempInfo.handGunFlag)
				{
					tempAI.gunInfo = GunInfoList.instance.GetGunInfo(1, 0);
				}
				else if (tempInfo.machineGunFlag)
				{
					tempAI.gunInfo = GunInfoList.instance.GetGunInfo(0, 0);
				}
				tempAI.gunInfo.accuracy = tempInfo.accuracy;
				tempAI.gunInfo.damage = tempInfo.attackVal;
				tempAI.gunInfo.shotInterval = tempInfo.fireInterval;
				tempAI.punchedDamageVal = tempInfo.attackVal;
				tempAI.fallingBulletRate = tempInfo.bulletRate;
				tempAI.fallingMoneyVal = tempInfo.fallingMoney;
				tempAI.ResetAI(curPoint, prePoint, pos, attackFlag, true, tempAI.handGunFlag, tempAI.machineGunFlag);
			}
		}
		else if (CheckEmpty(pos))
		{
			tempAI = poolList[0];
			poolList.RemoveAt(0);
			enableAIList.Add(tempAI);
			if (type == NPCTYPE.POLICE1_HG || type == NPCTYPE.POLICE2_HG || type == NPCTYPE.POLICE2_MG)
			{
				policeCount++;
			}
			tempAI.type = type;
			tempAI.npcLevel = level;
			tempInfo = AIInfoList.instance.GetAIInfo(type, level);
			tempAI.healthCtl.maxHealthVal = tempInfo.health;
			tempAI.handGunFlag = tempInfo.handGunFlag;
			tempAI.machineGunFlag = tempInfo.machineGunFlag;
			if (tempInfo.handGunFlag)
			{
				tempAI.gunInfo = GunInfoList.instance.GetGunInfo(1, 0);
			}
			else if (tempInfo.machineGunFlag)
			{
				tempAI.gunInfo = GunInfoList.instance.GetGunInfo(0, 0);
			}
			tempAI.gunInfo.accuracy = tempInfo.accuracy;
			tempAI.gunInfo.damage = tempInfo.attackVal;
			tempAI.gunInfo.shotInterval = tempInfo.fireInterval;
			tempAI.fallingMoneyVal = tempInfo.fallingMoney;
			tempAI.fallingBulletRate = tempInfo.bulletRate;
			tempAI.ResetAI(curPoint, prePoint, pos, attackFlag, true, tempAI.handGunFlag, tempAI.machineGunFlag);
		}
	}

	public void GetNPC(RoadPointNew curPoint, RoadPointNew prePoint, Vector3 pos, bool attackFlag, NPCTYPE type, int level)
	{
		switch (type)
		{
		case NPCTYPE.GANSTARBLACK_PUNCH:
		case NPCTYPE.GANSTARBLACK_HG:
		case NPCTYPE.GANSTARBLACK_MG:
			GetNPCInList(curPoint, prePoint, pos, attackFlag, type, level, ganstarBlackList, ganstarBlackPreferb);
			break;
		case NPCTYPE.GANSTARNAKED_PUNCH:
		case NPCTYPE.GANSTARNAKED_HG:
		case NPCTYPE.GANSTARNAKED_MG:
			GetNPCInList(curPoint, prePoint, pos, attackFlag, type, level, ganstarNakedList, ganstarNakedPreferb);
			break;
		case NPCTYPE.GANSTARWHITE_PUNCH:
		case NPCTYPE.GANSTARWHITE_HG:
			GetNPCInList(curPoint, prePoint, pos, attackFlag, type, level, ganstarWhiteList, ganstarWhitePreferb);
			break;
		case NPCTYPE.POLICE1_HG:
			GetNPCInList(curPoint, prePoint, pos, false, type, level, police1DisableList, police1Preferb);
			break;
		case NPCTYPE.POLICE2_HG:
		case NPCTYPE.POLICE2_MG:
			GetNPCInList(curPoint, prePoint, pos, false, type, level, police2DisableList, police2Preferb);
			break;
		case NPCTYPE.NORMALBLACK_PUNCH:
		case NPCTYPE.NORMALBLACK_HG:
			GetNPCInList(curPoint, prePoint, pos, false, type, level, normalBlackList, normalBlackPreferb);
			break;
		default:
			GetNPCInList(curPoint, prePoint, pos, attackFlag, type, level, ganstarBlackList, ganstarBlackPreferb);
			break;
		}
	}

	public AIController GetPolice(NPCTYPE type)
	{
		if (type == NPCTYPE.POLICE1_HG)
		{
			return GetPolice(police1DisableList, police1Preferb);
		}
		return GetPolice(police2DisableList, police2Preferb);
	}

	public AIController GetPolice(List<AIController> disableList, AIController preFerb)
	{
		if (disableList.Count == 0)
		{
			tempAI = (AIController)Object.Instantiate(preFerb);
			tempAI.transform.parent = base.transform;
			enableAIList.Add(tempAI);
			policeNumCount++;
			policeCount++;
			tempAI.dieFlag = false;
			tempAI.healthCtl.Reset();
			return tempAI;
		}
		policeCount++;
		tempAI = disableList[0];
		disableList.RemoveAt(0);
		tempAI.dieFlag = false;
		enableAIList.Add(tempAI);
		tempAI.healthCtl.Reset();
		return tempAI;
	}

	public AIController GetAttackAIInList(List<AIController> poolList, AIController preferb)
	{
		if (poolList.Count == 0)
		{
			tempAI = (AIController)Object.Instantiate(preferb);
			tempAI.transform.parent = base.transform;
			enableAIList.Add(tempAI);
			return tempAI;
		}
		tempAI = poolList[0];
		poolList.RemoveAt(0);
		enableAIList.Add(tempAI);
		return tempAI;
	}

	public AIController GetAttackAI(NPCTYPE aiType)
	{
		switch (aiType)
		{
		case NPCTYPE.GANSTARBLACK_PUNCH:
		case NPCTYPE.GANSTARBLACK_HG:
		case NPCTYPE.GANSTARBLACK_MG:
			return GetAttackAIInList(ganstarBlackList, ganstarBlackPreferb);
		case NPCTYPE.GANSTARNAKED_PUNCH:
		case NPCTYPE.GANSTARNAKED_HG:
		case NPCTYPE.GANSTARNAKED_MG:
			return GetAttackAIInList(ganstarNakedList, ganstarNakedPreferb);
		case NPCTYPE.GANSTARWHITE_PUNCH:
		case NPCTYPE.GANSTARWHITE_HG:
			return GetAttackAIInList(ganstarWhiteList, ganstarWhitePreferb);
		default:
			return GetAttackAIInList(ganstarBlackList, ganstarBlackPreferb);
		}
	}

	public void RecycleAI(AIController ai)
	{
		enableAIList.Remove(ai);
		ai.alarmFlag = false;
		ai.attackedFlag = false;
		ai.GetComponent<Rigidbody>().useGravity = true;
		ai.GetComponent<Rigidbody>().velocity = Vector3.zero;
		ai.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		ai.fightingStateCurPoint = null;
		ai.fightModeStandFlag = false;
		if (ai.ragDollObj != null)
		{
			RagDollPool.instance.RecycleNpcRagdoll(ai.ragDollObj, ai.type);
			ai.ragDollObj = null;
		}
		if (ai.attackLabelFlag)
		{
			ai.attackLabelFlag = false;
			AttackAILabelPool.instance.RemoveAttackAI(ai.gameObject);
		}
		switch (ai.type)
		{
		case NPCTYPE.GANSTARBLACK_PUNCH:
		case NPCTYPE.GANSTARBLACK_HG:
		case NPCTYPE.GANSTARBLACK_MG:
			ganstarBlackList.Add(ai);
			break;
		case NPCTYPE.GANSTARNAKED_PUNCH:
		case NPCTYPE.GANSTARNAKED_HG:
		case NPCTYPE.GANSTARNAKED_MG:
			ganstarNakedList.Add(ai);
			break;
		case NPCTYPE.GANSTARWHITE_PUNCH:
		case NPCTYPE.GANSTARWHITE_HG:
			ganstarWhiteList.Add(ai);
			break;
		case NPCTYPE.NORMALBLACK_PUNCH:
		case NPCTYPE.NORMALBLACK_HG:
			normalBlackList.Add(ai);
			break;
		case NPCTYPE.NORMALWHITE:
			normalWhiteList.Add(ai);
			break;
		case NPCTYPE.NORMALWOMEN:
			normalWomenList.Add(ai);
			break;
		case NPCTYPE.POLICE1_HG:
			police1DisableList.Add(ai);
			policeCount--;
			AICarPoolController.instance.policeCarCount--;
			if (AICarPoolController.instance.policeCarCount <= 0)
			{
				GameUIController.instance.CheckPoliceLevel();
			}
			break;
		case NPCTYPE.POLICE2_HG:
		case NPCTYPE.POLICE2_MG:
			police2DisableList.Add(ai);
			policeCount--;
			AICarPoolController.instance.policeCarCount--;
			if (AICarPoolController.instance.policeCarCount <= 0)
			{
				GameUIController.instance.CheckPoliceLevel();
			}
			break;
		case NPCTYPE.NORMALWHITE2:
			normalWhite2List.Add(ai);
			break;
		case NPCTYPE.NORMALWOMEN2:
			normalWomen2List.Add(ai);
			break;
		}
		ai.gameObject.SetActiveRecursively(false);
	}

	private void OnDestroy()
	{
		instance = null;
	}

	public bool CheckEmpty(Vector3 pos)
	{
		for (int i = 0; i < enableAIList.Count; i++)
		{
			if ((enableAIList[i].transform.position - pos).sqrMagnitude < 30f)
			{
				return false;
			}
		}
		return true;
	}
}
