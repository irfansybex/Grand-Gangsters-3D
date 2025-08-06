using System;

[Serializable]
public class ItemCarInfo
{
	public bool unlockFlag;

	public int unlockByLevel;

	public int carLevel;

	public string carName;

	public bool buyFlag;

	public bool equipedFlag;

	public CarInfo carInfo;

	public bool carGoldFlag;

	public int carPrise;

	public int carGoldPrise;

	public int repairePrise;

	public int carNum;

	public int[] upgradeCarNum;

	public float[] maxSpeedLevelList;

	public float[] brakeForceLevelList;

	public float[] maxSteerAngleLevelList;

	public float[] maxSpeedSteerAngleLevelList;

	public int[] maxHealthLevelList;
}
