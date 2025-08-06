using System.Collections.Generic;
using UnityEngine;

public class FallingObjPool : MonoBehaviour
{
	public static FallingObjPool instance;

	public List<FallingObjCtl> enableFallingObj;

	public List<FallingObjCtl> moneyList;

	public List<FallingObjCtl> bulletList;

	public List<FallingObjCtl> handGunList;

	public FallingObjCtl temp;

	public GameObject moneyPreferb;

	public GameObject bulletPreferb;

	public GameObject handGunPreferb;

	public FallingRate[] fallingRateList;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	public void GetFallingObj(Vector3 pos, int fallingIndex)
	{
		float num = Random.Range(0, 100);
		if (!(num < (float)fallingRateList[fallingIndex].rateList[fallingRateList[fallingIndex].rateList.Length - 1]))
		{
			return;
		}
		for (int i = 0; i < fallingRateList[fallingIndex].rateList.Length; i++)
		{
			if (num < (float)fallingRateList[fallingIndex].rateList[i])
			{
				FallingObjCtl fallingObjCtl = ((i != 2) ? GetFallingObjInType((FALLINGOBJTYPE)i) : ((GlobalInf.machineGunIndex != -1) ? GetFallingObjInType(FALLINGOBJTYPE.MACHINEGUNBULLET) : GetFallingObjInType(FALLINGOBJTYPE.MONEY)));
				fallingObjCtl.transform.position = pos;
				if (i < 5)
				{
					fallingObjCtl.num = Random.Range(fallingRateList[fallingIndex].minNum[i], fallingRateList[fallingIndex].maxNum[i]);
				}
				else
				{
					fallingObjCtl.num = 1;
				}
				break;
			}
		}
	}

	public void GetFallingObj(Vector3 pos, int val, float bulletRate)
	{
		float num = Random.Range(0f, 1f);
		FallingObjCtl fallingObjInType;
		if (num < bulletRate)
		{
			if (Random.Range(0, 10) < 5)
			{
				if (GlobalInf.machineGunIndex == -1)
				{
					fallingObjInType = GetFallingObjInType(FALLINGOBJTYPE.HANDGUNBULLET);
					fallingObjInType.num = 2;
				}
				else
				{
					fallingObjInType = GetFallingObjInType(FALLINGOBJTYPE.MACHINEGUNBULLET);
					fallingObjInType.num = 5;
				}
			}
			else
			{
				fallingObjInType = GetFallingObjInType(FALLINGOBJTYPE.HANDGUNBULLET);
				fallingObjInType.num = 2;
			}
		}
		else
		{
			fallingObjInType = GetFallingObjInType(FALLINGOBJTYPE.MONEY);
			fallingObjInType.num = val;
		}
		if (GameController.instance.curGameMode == GAMEMODE.SURVIVAL)
		{
			float num2 = 0f;
			float num3 = 0f;
			num2 = ((pos.x - GameController.instance.survivalMode.info.playerDefaultPos.x > 13f) ? (GameController.instance.survivalMode.info.playerDefaultPos.x + 13f) : ((!(pos.x - GameController.instance.survivalMode.info.playerDefaultPos.x < -13f)) ? pos.x : (GameController.instance.survivalMode.info.playerDefaultPos.x - 13f)));
			num3 = ((pos.z - GameController.instance.survivalMode.info.playerDefaultPos.z > 13f) ? (GameController.instance.survivalMode.info.playerDefaultPos.z + 13f) : ((!(pos.z - GameController.instance.survivalMode.info.playerDefaultPos.z < -13f)) ? pos.z : (GameController.instance.survivalMode.info.playerDefaultPos.z - 13f)));
			fallingObjInType.transform.position = new Vector3(num2, 0f, num3);
		}
		else
		{
			fallingObjInType.transform.position = pos;
		}
	}

	public FallingObjCtl GetFallingObjInType(FALLINGOBJTYPE objType)
	{
		switch (objType)
		{
		case FALLINGOBJTYPE.MONEY:
			return GetMoneyObj();
		case FALLINGOBJTYPE.HANDGUNBULLET:
			return GetHandGunBullet();
		case FALLINGOBJTYPE.MACHINEGUNBULLET:
			return GetMachineGunBullet();
		case FALLINGOBJTYPE.HANDGUN:
			return GetHandGunObj();
		default:
			return GetMoneyObj();
		}
	}

	public FallingObjCtl GetMoneyObj()
	{
		if (moneyList.Count == 0)
		{
			temp = ((GameObject)Object.Instantiate(moneyPreferb)).GetComponent<FallingObjCtl>();
			temp.gameObject.SetActiveRecursively(true);
			enableFallingObj.Add(temp);
			return temp;
		}
		temp = moneyList[0];
		moneyList.RemoveAt(0);
		temp.gameObject.SetActiveRecursively(true);
		enableFallingObj.Add(temp);
		return temp;
	}

	public void RecycleMoney(FallingObjCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		moneyList.Add(obj);
	}

	public FallingObjCtl GetHandGunObj()
	{
		if (handGunList.Count == 0)
		{
			temp = ((GameObject)Object.Instantiate(handGunPreferb)).GetComponent<FallingObjCtl>();
			temp.gameObject.SetActiveRecursively(true);
			enableFallingObj.Add(temp);
			return temp;
		}
		temp = handGunList[0];
		handGunList.RemoveAt(0);
		temp.gameObject.SetActiveRecursively(true);
		enableFallingObj.Add(temp);
		return temp;
	}

	public void RecycleHandGun(FallingObjCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		handGunList.Add(obj);
	}

	public FallingObjCtl GetHandGunBullet()
	{
		if (bulletList.Count == 0)
		{
			temp = ((GameObject)Object.Instantiate(bulletPreferb)).GetComponent<FallingObjCtl>();
			temp.fallingType = FALLINGOBJTYPE.HANDGUNBULLET;
			temp.gameObject.SetActiveRecursively(true);
			enableFallingObj.Add(temp);
			return temp;
		}
		temp = bulletList[0];
		bulletList.RemoveAt(0);
		temp.fallingType = FALLINGOBJTYPE.HANDGUNBULLET;
		temp.gameObject.SetActiveRecursively(true);
		enableFallingObj.Add(temp);
		return temp;
	}

	public FallingObjCtl GetMachineGunBullet()
	{
		if (bulletList.Count == 0)
		{
			temp = ((GameObject)Object.Instantiate(bulletPreferb)).GetComponent<FallingObjCtl>();
			temp.fallingType = FALLINGOBJTYPE.MACHINEGUNBULLET;
			temp.gameObject.SetActiveRecursively(true);
			enableFallingObj.Add(temp);
			return temp;
		}
		temp = bulletList[0];
		bulletList.RemoveAt(0);
		temp.fallingType = FALLINGOBJTYPE.MACHINEGUNBULLET;
		temp.gameObject.SetActiveRecursively(true);
		enableFallingObj.Add(temp);
		return temp;
	}

	public void RecycleBullet(FallingObjCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		bulletList.Add(obj);
	}

	public void Recycle(FallingObjCtl obj)
	{
		obj.transform.parent = base.transform;
		obj.gameObject.SetActiveRecursively(false);
		obj.recycleFlag = false;
		enableFallingObj.Remove(obj);
		switch (obj.fallingType)
		{
		case FALLINGOBJTYPE.MONEY:
			moneyList.Add(obj);
			break;
		case FALLINGOBJTYPE.HANDGUNBULLET:
		case FALLINGOBJTYPE.MACHINEGUNBULLET:
			bulletList.Add(obj);
			break;
		case FALLINGOBJTYPE.HANDGUN:
			handGunList.Add(obj);
			break;
		}
	}

	public void ClearFallingObj()
	{
		for (int num = enableFallingObj.Count - 1; num >= 0; num--)
		{
			Recycle(enableFallingObj[num]);
		}
	}
}
