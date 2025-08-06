using System.Collections.Generic;
using UnityEngine;

public class RagDollPool : MonoBehaviour
{
	public static RagDollPool instance;

	public List<GameObject> ganstarBlackRagdoll;

	public List<GameObject> ganstarWhiteRagdoll;

	public List<GameObject> ganstarNakedRagdoll;

	public List<GameObject> normalWhiteRagdoll;

	public List<GameObject> normalBlackRagdoll;

	public List<GameObject> normalWomenRagdoll;

	public List<GameObject> police1RagdollList;

	public List<GameObject> police2RagdollList;

	public List<GameObject> normalWhite2Ragdoll;

	public List<GameObject> normalWomen2Ragdoll;

	public GameObject temp;

	public int ganstarBlackCount = 1;

	public int ganstarWhiteCount = 1;

	public int ganstarNakedCount = 1;

	public int normalWomenCount = 1;

	public int normalWhiteCount = 1;

	public int normalBlackCount = 1;

	public int police1NumCount = 1;

	public int police2NumCount = 1;

	public int normalWhite2Count = 1;

	public int normalWomen2Count = 1;

	public GameObject ganstarBlackPreferb;

	public GameObject ganstarNakedPreferb;

	public GameObject ganstarWhitePreferb;

	public GameObject normalBlackPreferb;

	public GameObject normalWhitePreferb;

	public GameObject normalWomenPreferb;

	public GameObject police1Preferb;

	public GameObject police2Preferb;

	public GameObject normalWhite2Preferb;

	public GameObject normalWomen2Preferb;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public GameObject GetNpcRagdoll(NPCTYPE type)
	{
		switch (type)
		{
		case NPCTYPE.GANSTARBLACK_PUNCH:
		case NPCTYPE.GANSTARBLACK_HG:
		case NPCTYPE.GANSTARBLACK_MG:
			return GetRightNpcRagdoll(ganstarBlackRagdoll, ganstarBlackCount, ganstarBlackPreferb);
		case NPCTYPE.GANSTARWHITE_PUNCH:
		case NPCTYPE.GANSTARWHITE_HG:
			return GetRightNpcRagdoll(ganstarWhiteRagdoll, ganstarWhiteCount, ganstarWhitePreferb);
		case NPCTYPE.GANSTARNAKED_PUNCH:
		case NPCTYPE.GANSTARNAKED_HG:
		case NPCTYPE.GANSTARNAKED_MG:
			return GetRightNpcRagdoll(ganstarNakedRagdoll, ganstarNakedCount, ganstarNakedPreferb);
		case NPCTYPE.NORMALWHITE:
			return GetRightNpcRagdoll(normalWhiteRagdoll, normalWhiteCount, normalWhitePreferb);
		case NPCTYPE.NORMALWOMEN:
			return GetRightNpcRagdoll(normalWomenRagdoll, normalWhiteCount, normalWomenPreferb);
		case NPCTYPE.NORMALBLACK_PUNCH:
		case NPCTYPE.NORMALBLACK_HG:
			return GetRightNpcRagdoll(normalBlackRagdoll, normalBlackCount, normalBlackPreferb);
		case NPCTYPE.POLICE1_HG:
			return GetRightNpcRagdoll(police1RagdollList, police1NumCount, police1Preferb);
		case NPCTYPE.POLICE2_HG:
		case NPCTYPE.POLICE2_MG:
			return GetRightNpcRagdoll(police2RagdollList, police2NumCount, police2Preferb);
		case NPCTYPE.NORMALWHITE2:
			return GetRightNpcRagdoll(normalWhite2Ragdoll, normalWhite2Count, normalWhite2Preferb);
		case NPCTYPE.NORMALWOMEN2:
			return GetRightNpcRagdoll(normalWomen2Ragdoll, normalWomen2Count, normalWomen2Preferb);
		default:
			return GetRightNpcRagdoll(normalWhiteRagdoll, normalWhiteCount, normalWhitePreferb);
		}
	}

	public GameObject GetRightNpcRagdoll(List<GameObject> ragdollList, int countNum, GameObject preferb)
	{
		if (ragdollList.Count == 0)
		{
			if (countNum < 10)
			{
				temp = (GameObject)Object.Instantiate(preferb);
				countNum++;
				return temp;
			}
			return null;
		}
		temp = ragdollList[0];
		ragdollList.RemoveAt(0);
		return temp;
	}

	public void RecycleNpcRagdoll(GameObject obj, NPCTYPE type)
	{
		switch (type)
		{
		case NPCTYPE.GANSTARBLACK_PUNCH:
		case NPCTYPE.GANSTARBLACK_HG:
		case NPCTYPE.GANSTARBLACK_MG:
			RecycleRightNpcRagdoll(ganstarBlackRagdoll, obj);
			break;
		case NPCTYPE.GANSTARNAKED_PUNCH:
		case NPCTYPE.GANSTARNAKED_HG:
		case NPCTYPE.GANSTARNAKED_MG:
			RecycleRightNpcRagdoll(ganstarNakedRagdoll, obj);
			break;
		case NPCTYPE.GANSTARWHITE_PUNCH:
		case NPCTYPE.GANSTARWHITE_HG:
			RecycleRightNpcRagdoll(ganstarWhiteRagdoll, obj);
			break;
		case NPCTYPE.NORMALBLACK_PUNCH:
		case NPCTYPE.NORMALBLACK_HG:
			RecycleRightNpcRagdoll(normalBlackRagdoll, obj);
			break;
		case NPCTYPE.NORMALWHITE:
			RecycleRightNpcRagdoll(normalWhiteRagdoll, obj);
			break;
		case NPCTYPE.NORMALWOMEN:
			RecycleRightNpcRagdoll(normalWomenRagdoll, obj);
			break;
		case NPCTYPE.POLICE1_HG:
			RecycleRightNpcRagdoll(police1RagdollList, obj);
			break;
		case NPCTYPE.POLICE2_HG:
		case NPCTYPE.POLICE2_MG:
			RecycleRightNpcRagdoll(police2RagdollList, obj);
			break;
		case NPCTYPE.NORMALWHITE2:
			RecycleRightNpcRagdoll(normalWhite2Ragdoll, obj);
			break;
		case NPCTYPE.NORMALWOMEN2:
			RecycleRightNpcRagdoll(normalWomen2Ragdoll, obj);
			break;
		}
	}

	public void RecycleRightNpcRagdoll(List<GameObject> ragdoll, GameObject obj)
	{
		obj.transform.parent = base.transform;
		obj.SetActiveRecursively(false);
		ragdoll.Add(obj);
	}
}
