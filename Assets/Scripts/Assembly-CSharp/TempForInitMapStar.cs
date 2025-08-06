using UnityEngine;

[ExecuteInEditMode]
public class TempForInitMapStar : MonoBehaviour
{
	public bool runFlag;

	public MinimapLightLabelController minimapLightLabel;

	public GameObject root;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Run3();
			MonoBehaviour.print("run");
		}
	}

	public void Run3()
	{
		for (int i = 0; i < minimapLightLabel.lightLabel2.Length; i++)
		{
			if (minimapLightLabel.lightLabel2[i].transform.parent.gameObject.name.Equals("CarKillingRoot"))
			{
				minimapLightLabel.lightLabel2[i].mode = GAMEMODE.CARKILLING;
			}
			else if (minimapLightLabel.lightLabel2[i].transform.parent.gameObject.name.Equals("DeliverRoot"))
			{
				minimapLightLabel.lightLabel2[i].mode = GAMEMODE.DELIVER;
			}
			else if (minimapLightLabel.lightLabel2[i].transform.parent.gameObject.name.Equals("DrivingRoot"))
			{
				minimapLightLabel.lightLabel2[i].mode = GAMEMODE.DRIVING0;
			}
			else if (minimapLightLabel.lightLabel2[i].transform.parent.gameObject.name.Equals("FightingRoot"))
			{
				minimapLightLabel.lightLabel2[i].mode = GAMEMODE.FIGHTING;
			}
			else if (minimapLightLabel.lightLabel2[i].transform.parent.gameObject.name.Equals("GunKillingRoot"))
			{
				minimapLightLabel.lightLabel2[i].mode = GAMEMODE.GUNKILLING;
			}
			else if (minimapLightLabel.lightLabel2[i].transform.parent.gameObject.name.Equals("RobCarRoot"))
			{
				minimapLightLabel.lightLabel2[i].mode = GAMEMODE.ROBCAR;
			}
			else if (minimapLightLabel.lightLabel2[i].transform.parent.gameObject.name.Equals("SkillDrivingRoot"))
			{
				minimapLightLabel.lightLabel2[i].mode = GAMEMODE.SKILLDRIVING;
			}
			else if (minimapLightLabel.lightLabel2[i].transform.parent.gameObject.name.Equals("SurvivalRoot"))
			{
				minimapLightLabel.lightLabel2[i].mode = GAMEMODE.SURVIVAL;
			}
			minimapLightLabel.lightLabel2[i].index = int.Parse(minimapLightLabel.lightLabel2[i].gameObject.name);
		}
	}

	public void Run2()
	{
		for (int i = 0; i < minimapLightLabel.lightLabel.Length; i++)
		{
			minimapLightLabel.lightLabel2[i] = minimapLightLabel.lightLabel[i].GetComponent<MapLabelStarController>();
		}
	}

	public void Run()
	{
		for (int i = 0; i < root.transform.childCount; i++)
		{
			for (int j = 0; j < root.transform.GetChild(i).childCount; j++)
			{
				MapLabelStarController component = root.transform.GetChild(i).GetChild(j).gameObject.GetComponent<MapLabelStarController>();
				for (int k = 0; k < root.transform.GetChild(i).GetChild(j).childCount; k++)
				{
					GameObject gameObject = root.transform.GetChild(i).GetChild(j).GetChild(k)
						.gameObject;
					if (gameObject.name.Contains("back"))
					{
						if (gameObject.name.Contains("0"))
						{
							component.starBacks[0] = gameObject;
						}
						else if (gameObject.name.Contains("1"))
						{
							component.starBacks[1] = gameObject;
						}
						else if (gameObject.name.Contains("2"))
						{
							component.starBacks[2] = gameObject;
						}
					}
					else if (gameObject.name.Contains("0"))
					{
						component.stars[0] = gameObject;
					}
					else if (gameObject.name.Contains("1"))
					{
						component.stars[1] = gameObject;
					}
					else if (gameObject.name.Contains("2"))
					{
						component.stars[2] = gameObject;
					}
				}
			}
		}
	}
}
