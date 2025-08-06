using UnityEngine;

public class DeadUIControllor : MonoBehaviour
{
	public int taskLevel;

	public GameObject backGround;

	public UIEventListener okBtn;

	public UIEventListener backMenuBtn;

	public UILabel driveLabel;

	public UILabel timeLabel;

	public UILabel killLabel;

	public UILabel hartCountLabel;

	public int hartRemainTime;

	public TweenPosition disAppearHart;

	public UISprite hartSprite;

	private int min;

	private int sec;

	private string temp;

	private float preTime;

	private float countT;

	public void ResetDeadUI()
	{
		Platform.CountHarts();
		if (GlobalInf.hartCount > 0)
		{
			hartCountLabel.text = string.Empty + GlobalInf.hartCount + "/" + (GlobalInf.gameLevel + 3);
		}
		else
		{
			hartRemainTime = Platform.GetRemainHartCountTime();
			hartCountLabel.text = ChangeSecToMin(hartRemainTime);
		}
	}

	public string ChangeSecToMin(int val)
	{
		temp = string.Empty;
		min = val / 60;
		sec = val % 60;
		string text = temp;
		temp = text + "0" + min + ":";
		if (sec < 10)
		{
			temp = temp + "0" + sec;
		}
		else
		{
			temp = temp + string.Empty + sec;
		}
		return temp;
	}

	private void Awake()
	{
		preTime = Time.realtimeSinceStartup;
		countT = 0f;
		ResetDeadUI();
	}

	private void Update()
	{
		countT += Time.realtimeSinceStartup - preTime;
		preTime = Time.realtimeSinceStartup;
		if (countT >= 1f)
		{
			countT -= 1f;
			ResetDeadUI();
		}
	}
}
