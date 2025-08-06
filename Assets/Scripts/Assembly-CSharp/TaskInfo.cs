using System;
using UnityEngine;

[Serializable]
public class TaskInfo
{
	public GAMEMODE taskMode;

	public int taskLevel;

	public bool enableFlag;

	public int taskIndex;

	public string taskName;

	public string disCribe;

	public Vector3 startPos;

	public Vector3 startAngle;

	public Vector3 slotPos;

	public Vector3 slotAngle;

	public OnActiveActionCtl startLabelObj;

	public int starNum;

	public int highestScore;

	public int[] starScore;

	public int rewardIndex;

	public bool inverseScoresFlag;

	public int stateIndex;

	public int newStateIndex;

	public Vector3 exitPlayerPos;

	public Vector3 exitPlayerAngle;

	public Vector3 exitPlayerCarPos;

	public Vector3 exitPlayerCarAngle;
}
