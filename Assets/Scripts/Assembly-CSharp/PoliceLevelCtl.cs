using UnityEngine;

public class PoliceLevelCtl : MonoBehaviour
{
	public static PoliceLevelCtl instance;

	public static int score;

	public static int level;

	public static int preLevel;

	public int[] levelScorce;

	public int[] policeCarNum;

	public int[] policeNum;

	public int[] policeLevel;

	public float[] safeDis;

	public int[] reduceScorce;

	public int[] police1Rate;

	public int[] police2HGRate;

	public int attackPerson;

	public int killPerson;

	public int attackPolice;

	public int killPolice;

	public int robCar;

	public int robPoliceCar;

	public int scratchPerson;

	public int crashPerson;

	public int scratchPolice;

	public int crashPolice;

	public int scratchCar;

	public int crashCar;

	public int scratchPoliceCar;

	public int crashPoliceCar;

	public float countTime;

	public float coutPoliceTime;

	public float reduceInterval;

	public float policeAlarmDis;

	public bool policeChaseFlag;

	public float startReduseTime;

	public float countStartReduceTime;

	private void Awake()
	{
		instance = this;
		preLevel = 0;
	}

	private void Update()
	{
		if (score <= 0)
		{
			return;
		}
		if (countStartReduceTime > startReduseTime)
		{
			countTime += Time.deltaTime;
			if (countTime > reduceInterval)
			{
				countTime = 0f;
				if (!CitySenceController.CheckPoliceAside(PlayerController.instance.transform.position, safeDis[level]))
				{
					score -= reduceScorce[level];
				}
			}
		}
		else
		{
			countStartReduceTime += Time.deltaTime;
		}
		coutPoliceTime += Time.deltaTime;
		if (coutPoliceTime > 3f)
		{
			CountPoliceLevel();
			coutPoliceTime = 0f;
			CheckPoliceCar();
		}
	}

	public static void ResetPoliceLevel()
	{
		level = 0;
		score = 0;
	}

	public void CountPoliceLevel()
	{
		if (GameController.instance.curGameMode == GAMEMODE.SURVIVAL || GameController.instance.curGameMode == GAMEMODE.GUNKILLING || GameController.instance.curGameMode == GAMEMODE.CARKILLING || GameController.instance.curGameMode == GAMEMODE.DRIVING0 || GameController.instance.curGameMode == GAMEMODE.ROBCAR || GameController.instance.curGameMode == GAMEMODE.FIGHTING || GameController.instance.curGameMode == GAMEMODE.ROBMOTOR)
		{
			level = 0;
			if (preLevel != level)
			{
				preLevel = level;
				LevelChange();
			}
			return;
		}
		if (score > levelScorce[3])
		{
			level = 3;
		}
		else if (score > levelScorce[2])
		{
			level = 2;
		}
		else if (score > levelScorce[1])
		{
			level = 1;
		}
		else
		{
			level = 0;
			policeChaseFlag = false;
		}
		if (level > 0)
		{
			policeChaseFlag = true;
		}
		if (preLevel != level)
		{
			preLevel = level;
			LevelChange();
		}
	}

	public void LevelChange()
	{
		GameUIController.instance.CheckPoliceLevel();
	}

	public void CheckPoliceCar()
	{
		if (GameController.instance.curGameMode != GAMEMODE.SURVIVAL && GameController.instance.curGameMode != GAMEMODE.GUNKILLING && GameController.instance.curGameMode != GAMEMODE.CARKILLING && GameController.instance.curGameMode != GAMEMODE.DRIVING0 && GameController.instance.curGameMode != GAMEMODE.FIGHTING && GameController.instance.curGameMode != GAMEMODE.ROBMOTOR && AICarPoolController.instance.policeCarCount < instance.policeCarNum[level] && NPCPoolController.instance.policeCount < instance.policeNum[level])
		{
			if (PlayerController.instance.curState == PLAYERSTATE.CAR)
			{
				CitySenceController.instance.GenerateBlockPoliceCar(CitySenceController.instance.carProcuceDistance);
			}
			else
			{
				CitySenceController.instance.GeneratePolice(CitySenceController.instance.carProcuceDistance);
			}
		}
	}

	public static void AttackPerson()
	{
		score += instance.attackPerson;
		instance.countStartReduceTime = 0f;
	}

	public static void KillPerson()
	{
		score += instance.killPerson;
		instance.countStartReduceTime = 0f;
	}

	public static void AttackPolice()
	{
		score += instance.attackPolice;
		instance.countStartReduceTime = 0f;
	}

	public static void KillPolice()
	{
		score += instance.killPolice;
		instance.countStartReduceTime = 0f;
	}

	public static void RobCar()
	{
		score += instance.robCar;
		instance.countStartReduceTime = 0f;
	}

	public static void RobPoliceCar()
	{
		score += instance.robPoliceCar;
		instance.countStartReduceTime = 0f;
	}

	public static void ScratchPerson()
	{
		score += instance.scratchPerson;
		instance.countStartReduceTime = 0f;
	}

	public static void CrashPerson()
	{
		score += instance.crashPerson;
		instance.countStartReduceTime = 0f;
	}

	public static void ScratchPolice()
	{
		score += instance.scratchPolice;
		instance.countStartReduceTime = 0f;
	}

	public static void CrashPolice()
	{
		score += instance.crashPolice;
	}

	public static void ScratchCar()
	{
		score += instance.scratchCar;
		instance.countStartReduceTime = 0f;
	}

	public static void CrashCar()
	{
		score += instance.crashCar;
		instance.countStartReduceTime = 0f;
	}

	public static void ScratchPoliceCar()
	{
		score += instance.scratchPoliceCar;
		instance.countStartReduceTime = 0f;
	}

	public static void CrashPoliceCar()
	{
		score += instance.crashPoliceCar;
		instance.countStartReduceTime = 0f;
	}
}
