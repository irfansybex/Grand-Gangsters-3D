using UnityEngine;

public class SlotController : MonoBehaviour
{
	public delegate void PlayDone();

	public bool runFlag;

	public SlotWheel slotWheel1;

	public SlotWheel slotWheel2;

	public SlotWheel slotWheel3;

	public SLOTTYPE[] resultType;

	private int randomNum;

	public int[] rateNum;

	public bool playingFlag;

	public Transform camPos;

	public GameObject moreGameBtn;

	public GameObject betOneBtn;

	public GameObject betFiveBtn;

	public GameObject betTenBtn;

	public GameObject cashOutBtn;

	public GameObject slotBar;

	public GameObject slotActiveObj;

	public bool winFlag;

	public int winIndex;

	public PlayDone playDone;

	public int randomIndex;

	private void Start()
	{
	}

	private void Update()
	{
		if (playingFlag && slotWheel1.stopDoneFlag && slotWheel2.stopDoneFlag && slotWheel3.stopDoneFlag)
		{
			if (playDone != null)
			{
				playDone();
			}
			playingFlag = false;
			AudioController.instance.stop(AudioType.SLOT_ROLL);
		}
	}

	public void Play()
	{
		playingFlag = true;
		ComputeRate();
		slotWheel1.targetType = resultType[0];
		slotWheel2.targetType = resultType[1];
		slotWheel3.targetType = resultType[2];
		ResetWheel(slotWheel1);
		ResetWheel(slotWheel2);
		ResetWheel(slotWheel3);
		slotWheel1.startFlag = true;
		Invoke("InvokeWheel2", 0.5f);
		Invoke("InvokeWheel3", 1f);
		AudioController.instance.play(AudioType.SLOT_STICK);
		AudioController.instance.play(AudioType.SLOT_ROLL);
	}

	public void InvokeWheel2()
	{
		slotWheel2.startFlag = true;
	}

	public void InvokeWheel3()
	{
		slotWheel3.startFlag = true;
	}

	public void ResetWheel(SlotWheel tWheel)
	{
		tWheel.startFlag = false;
		tWheel.stopFlag = false;
		tWheel.startStopFlag = false;
		tWheel.rotateSpeed = tWheel.defaultRotateSpeed;
		tWheel.stopDoneFlag = false;
		tWheel.SetTargetAngle(tWheel.targetType);
		tWheel.stopDoneFlag = false;
	}

	public void ComputeRate()
	{
		winFlag = true;
		randomNum = Random.Range(0, 100000);
		if (randomNum < rateNum[0])
		{
			winIndex = 0;
			resultType[0] = SLOTTYPE.SEVEN;
			resultType[1] = SLOTTYPE.SEVEN;
			resultType[2] = SLOTTYPE.SEVEN;
			return;
		}
		if (randomNum < rateNum[0] + rateNum[1])
		{
			winIndex = 1;
			resultType[0] = SLOTTYPE.BELL;
			resultType[1] = SLOTTYPE.BELL;
			resultType[2] = SLOTTYPE.BELL;
			return;
		}
		if (randomNum < rateNum[0] + rateNum[1] + rateNum[2])
		{
			winIndex = 2;
			resultType[0] = SLOTTYPE.BAR;
			resultType[1] = SLOTTYPE.BAR;
			resultType[2] = SLOTTYPE.BAR;
			return;
		}
		if (randomNum < rateNum[0] + rateNum[1] + rateNum[2] + rateNum[3])
		{
			winIndex = 3;
			resultType[0] = SLOTTYPE.LEMON;
			resultType[1] = SLOTTYPE.LEMON;
			resultType[2] = SLOTTYPE.LEMON;
			return;
		}
		if (randomNum < rateNum[0] + rateNum[1] + rateNum[2] + rateNum[3] + rateNum[4])
		{
			winIndex = 4;
			resultType[0] = SLOTTYPE.APPLE;
			resultType[1] = SLOTTYPE.APPLE;
			resultType[2] = SLOTTYPE.APPLE;
			return;
		}
		if (randomNum < rateNum[0] + rateNum[1] + rateNum[2] + rateNum[3] + rateNum[4] + rateNum[5])
		{
			winIndex = 5;
			resultType[0] = SLOTTYPE.CHERRY;
			resultType[1] = SLOTTYPE.CHERRY;
			resultType[2] = SLOTTYPE.CHERRY;
			return;
		}
		if (randomNum < rateNum[0] + rateNum[1] + rateNum[2] + rateNum[3] + rateNum[4] + rateNum[5] + rateNum[6])
		{
			winIndex = 6;
			randomIndex = Random.Range(0, 3);
			if (randomIndex == 0)
			{
				resultType[0] = (SLOTTYPE)Random.Range(0, 6);
				if (resultType[0] == SLOTTYPE.LEMON)
				{
					resultType[0] = SLOTTYPE.SEVEN;
				}
			}
			else
			{
				resultType[0] = SLOTTYPE.LEMON;
			}
			if (randomIndex == 1)
			{
				resultType[1] = (SLOTTYPE)Random.Range(0, 6);
				if (resultType[1] == SLOTTYPE.LEMON)
				{
					resultType[1] = SLOTTYPE.SEVEN;
				}
			}
			else
			{
				resultType[1] = SLOTTYPE.LEMON;
			}
			if (randomIndex == 2)
			{
				resultType[2] = (SLOTTYPE)Random.Range(0, 6);
				if (resultType[2] == SLOTTYPE.LEMON)
				{
					resultType[2] = SLOTTYPE.SEVEN;
				}
			}
			else
			{
				resultType[2] = SLOTTYPE.LEMON;
			}
			return;
		}
		if (randomNum < rateNum[0] + rateNum[1] + rateNum[2] + rateNum[3] + rateNum[4] + rateNum[5] + rateNum[6] + rateNum[7])
		{
			winIndex = 7;
			randomIndex = Random.Range(0, 3);
			if (randomIndex == 0)
			{
				resultType[0] = (SLOTTYPE)Random.Range(0, 6);
				if (resultType[0] == SLOTTYPE.APPLE)
				{
					resultType[0] = SLOTTYPE.SEVEN;
				}
			}
			else
			{
				resultType[0] = SLOTTYPE.APPLE;
			}
			if (randomIndex == 1)
			{
				resultType[1] = (SLOTTYPE)Random.Range(0, 6);
				if (resultType[1] == SLOTTYPE.APPLE)
				{
					resultType[1] = SLOTTYPE.SEVEN;
				}
			}
			else
			{
				resultType[1] = SLOTTYPE.APPLE;
			}
			if (randomIndex == 2)
			{
				resultType[2] = (SLOTTYPE)Random.Range(0, 6);
				if (resultType[2] == SLOTTYPE.APPLE)
				{
					resultType[2] = SLOTTYPE.SEVEN;
				}
			}
			else
			{
				resultType[2] = SLOTTYPE.APPLE;
			}
			return;
		}
		if (randomNum < rateNum[0] + rateNum[1] + rateNum[2] + rateNum[3] + rateNum[4] + rateNum[5] + rateNum[6] + rateNum[7] + rateNum[8])
		{
			winIndex = 8;
			randomIndex = Random.Range(0, 3);
			if (randomIndex == 0)
			{
				resultType[0] = (SLOTTYPE)Random.Range(0, 6);
				if (resultType[0] == SLOTTYPE.CHERRY)
				{
					resultType[0] = SLOTTYPE.SEVEN;
				}
			}
			else
			{
				resultType[0] = SLOTTYPE.CHERRY;
			}
			if (randomIndex == 1)
			{
				resultType[1] = (SLOTTYPE)Random.Range(0, 6);
				if (resultType[1] == SLOTTYPE.CHERRY)
				{
					resultType[1] = SLOTTYPE.SEVEN;
				}
			}
			else
			{
				resultType[1] = SLOTTYPE.CHERRY;
			}
			if (randomIndex == 2)
			{
				resultType[2] = (SLOTTYPE)Random.Range(0, 6);
				if (resultType[2] == SLOTTYPE.CHERRY)
				{
					resultType[2] = SLOTTYPE.SEVEN;
				}
			}
			else
			{
				resultType[2] = SLOTTYPE.CHERRY;
			}
			return;
		}
		winIndex = -1;
		winFlag = false;
		resultType[0] = (SLOTTYPE)Random.Range(0, 6);
		resultType[1] = (SLOTTYPE)Random.Range(0, 6);
		resultType[2] = (SLOTTYPE)Random.Range(0, 6);
		if (resultType[0] == resultType[1] && resultType[1] == resultType[2])
		{
			if (resultType[0] != SLOTTYPE.APPLE && resultType[0] != SLOTTYPE.LEMON && resultType[1] != SLOTTYPE.CHERRY)
			{
				resultType[Random.Range(0, 3)] = (SLOTTYPE)((int)(resultType[0] + 1) % 6);
				return;
			}
			resultType[1] = SLOTTYPE.SEVEN;
			resultType[2] = SLOTTYPE.BAR;
		}
		else if (resultType[0] == resultType[1])
		{
			if (resultType[0] == SLOTTYPE.CHERRY || resultType[0] == SLOTTYPE.LEMON || resultType[0] == SLOTTYPE.APPLE)
			{
				resultType[0] = SLOTTYPE.BAR;
			}
		}
		else if (resultType[0] == resultType[2])
		{
			if (resultType[0] == SLOTTYPE.CHERRY || resultType[0] == SLOTTYPE.LEMON || resultType[0] == SLOTTYPE.APPLE)
			{
				resultType[2] = SLOTTYPE.BELL;
			}
		}
		else if (resultType[1] == resultType[2] && (resultType[2] == SLOTTYPE.CHERRY || resultType[2] == SLOTTYPE.LEMON || resultType[2] == SLOTTYPE.APPLE))
		{
			resultType[1] = SLOTTYPE.SEVEN;
		}
	}
}
