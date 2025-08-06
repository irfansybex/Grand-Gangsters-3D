using UnityEngine;

public class SlotWheel : MonoBehaviour
{
	public SLOTTYPE targetType;

	public float defaultRotateSpeed;

	public float rotateSpeed;

	public bool startFlag;

	public bool stopFlag;

	public bool startStopFlag;

	public Vector3 targetAngle;

	public bool stopDoneFlag;

	public float countRun;

	public bool enterFlag;

	public bool stopAudioFlag;

	public float count;

	public Vector3 startStopAngle;

	private void Start()
	{
	}

	public void SetTargetAngle(SLOTTYPE tType)
	{
		switch (tType)
		{
		case SLOTTYPE.APPLE:
			targetAngle = new Vector3(15f, 0f, 0f);
			break;
		case SLOTTYPE.BAR:
			targetAngle = new Vector3(15f, 180f, 180f);
			break;
		case SLOTTYPE.BELL:
			targetAngle = new Vector3(45f, 180f, 180f);
			break;
		case SLOTTYPE.CHERRY:
			targetAngle = new Vector3(75f, 180f, 180f);
			break;
		case SLOTTYPE.LEMON:
			targetAngle = new Vector3(45f, 0f, 0f);
			break;
		case SLOTTYPE.SEVEN:
			targetAngle = new Vector3(75f, 0f, 0f);
			break;
		default:
			targetAngle = new Vector3(30f, 0f, 0f);
			break;
		}
		countRun = 0f;
	}

	private void Update()
	{
		if (startFlag && !stopFlag)
		{
			RotateWheel();
			countRun += Time.deltaTime;
			if (countRun > 1f)
			{
				countRun = 0f;
				stopFlag = true;
				stopAudioFlag = true;
			}
		}
		if (stopFlag)
		{
			StopWheel();
		}
	}

	public void RotateWheel()
	{
		base.transform.Rotate(new Vector3(Time.deltaTime * rotateSpeed, 0f, 0f));
	}

	public void StopWheel()
	{
		if (Mathf.Abs(base.transform.localEulerAngles.y - targetAngle.y) < 1f && Mathf.Abs(base.transform.localEulerAngles.z - targetAngle.z) < 1f && Mathf.Abs(base.transform.localEulerAngles.x - targetAngle.x) < 5f)
		{
			startStopFlag = true;
		}
		if (startStopFlag)
		{
			if (rotateSpeed > 0f)
			{
				rotateSpeed -= Time.deltaTime * 720f;
			}
			else
			{
				rotateSpeed = 0f;
				startStopFlag = false;
				stopDoneFlag = true;
				startStopAngle = base.transform.localEulerAngles;
				count = 0f;
				if (stopAudioFlag)
				{
					stopAudioFlag = false;
					AudioController.instance.play(AudioType.SLOT_STOP);
				}
			}
		}
		if (stopDoneFlag)
		{
			count += Time.deltaTime * 2f;
			base.transform.localEulerAngles = new Vector3(Mathf.LerpAngle(startStopAngle.x, targetAngle.x, count), targetAngle.y, targetAngle.z);
		}
		base.transform.Rotate(new Vector3(Time.deltaTime * rotateSpeed, 0f, 0f));
	}
}
