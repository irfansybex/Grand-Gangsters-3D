using System;

[Serializable]
public class GunsInfo
{
	public GUNNAME gunName;

	public float bulletSpeed = 130f;

	public float reloadTime;

	public float shotInterval;

	public float accuracy;

	public float damage;

	public int curBulletNum;

	public int bulletNum;

	public int level;

	public int maxBulletNum;

	public int restBulletNum;

	public int bulletPrise;
}
