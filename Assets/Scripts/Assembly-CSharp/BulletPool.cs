using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
	public static BulletPool instance;

	public List<MyBullet> bulletPool;

	public List<MyBullet> shotedBullet;

	private MyBullet tempBullet;

	private void Awake()
	{
		instance = this;
	}

	public MyBullet GetBullet()
	{
		if (bulletPool.Count <= 0)
		{
			tempBullet = shotedBullet[0];
			tempBullet.Recycle();
		}
		if (bulletPool.Count > 0)
		{
			tempBullet = bulletPool[0];
			bulletPool.RemoveAt(0);
			shotedBullet.Add(tempBullet);
			return tempBullet;
		}
		return tempBullet;
	}

	public void RecycleBullet(MyBullet b)
	{
		bulletPool.Add(b);
		shotedBullet.RemoveAt(shotedBullet.IndexOf(b));
	}

	private void OnDestroy()
	{
		instance = null;
	}
}
