using UnityEngine;

[ExecuteInEditMode]
public class InitBulletPool : MonoBehaviour
{
	public BulletPool pool;

	public bool flag;

	private void Start()
	{
	}

	private void Update()
	{
		if (flag)
		{
			flag = false;
			Run();
			MonoBehaviour.print("run");
		}
	}

	public void Run()
	{
		pool.bulletPool.Clear();
		for (int i = 0; i < pool.transform.childCount; i++)
		{
			pool.bulletPool.Add(pool.transform.GetChild(i).gameObject.GetComponent<MyBullet>());
		}
	}
}
