using System;

[Serializable]
public class ItemGunInfo
{
	public bool unlockFlag;

	public int unlockByLevel;

	public string gunName;

	public bool buyFlag;

	public bool equipedFlag;

	public GunsInfo gunInfo;

	public bool gunGoldFlag;

	public int gunPrise;

	public int gunGoldPrise;

	public int gunNum;

	public int[] upgradeGunNum;

	public float[] accuracyLevelList;

	public float[] damageLevelList;

	public int[] bulletNumLevelList;

	public float[] shotIntervalLevelList;

	public float[] reloadLevelList;
}
