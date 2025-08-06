using UnityEngine;

[ExecuteInEditMode]
public class ReadAchievementData : ReadTXT
{
	public bool runflag;

	public string fileName;

	public AchievementPageController achievementPage;

	private void Update()
	{
		if (runflag)
		{
			runflag = false;
			Run();
			MonoBehaviour.print("run");
		}
	}

	public void Run()
	{
		InitArray(fileName);
		for (int i = 0; i < achievementPage.items.Length; i++)
		{
			achievementPage.items[i].level[0] = GetInt(i + 1, 2);
			achievementPage.items[i].level[1] = GetInt(i + 1, 3);
			achievementPage.items[i].level[2] = GetInt(i + 1, 4);
			achievementPage.items[i].cashMoney = GetInt(i + 1, 5);
			achievementPage.items[i].goldMoney = GetInt(i + 1, 6);
		}
	}
}
