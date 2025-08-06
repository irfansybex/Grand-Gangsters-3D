using UnityEngine;

public class GameController : MonoBehaviour
{
	public GAMEMODE curGameMode;

	public int index;

	public static GameController instance;

	public GameModeController curMode;

	public GameModeController preMode;

	public GameModeController emptyMpde;

	public CitySenceController normalMode;

	public DrivingModeController driveMode;

	public DeliverModeController deliverMode;

	public SurvivalModeController survivalMode;

	public SlotModeController slotMode;

	public GunKillingModeControllor gunKillingMode;

	public CarKillingModeControllor carKillingMode;

	public SkillDrivingMode skillDrivingMode;

	public RobCarModeController robbingCarMode;

	public FightingModeController fightingMode;

	public RobMotorModeController robMotorMode;

	public OnActiveActionCtl startLabel;

	public GameObject startLabelRoot;

	public int citySenceHealthVal;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		if (!GlobalInf.firstOpenGameFlag)
		{
			preMode = normalMode;
			curMode = normalMode;
		}
		else
		{
			curMode = emptyMpde;
		}
	}

	private void Start()
	{
		curMode.Reset(0);
	}

	private void Update()
	{
		curMode.MyUpdate();
	}

	public void ChangeMode(GAMEMODE newMode, int i)
	{
		if (newMode != 0)
		{
			TaskLabelController.instance.DisableFlag(newMode);
			startLabelRoot.SetActive(false);
			if (!GlobalInf.restarFlag)
			{
				citySenceHealthVal = GlobalInf.playerHealthVal;
			}
			else
			{
				GlobalInf.restarFlag = false;
				GlobalInf.playerHealthVal = citySenceHealthVal;
			}
		}
		else
		{
			startLabelRoot.SetActive(true);
			TaskLabelController.instance.countTime = 5f;
			GlobalInf.playerHealthVal = citySenceHealthVal;
			StoreDateController.SetPlayerHealthVal();
		}
		Platform.flurryEvent_onTaskStart((int)newMode, i);
		GameUIController.instance.minimapController.DisableTargetPos();
		index = i;
		switch (newMode)
		{
		case GAMEMODE.NORMAL:
			ChangeMode(normalMode);
			AudioController.instance.play(AudioType.FIGHTING);
			break;
		case GAMEMODE.DRIVING0:
			ChangeMode(driveMode);
			AudioController.instance.play(AudioType.DRIVING);
			break;
		case GAMEMODE.DELIVER:
			ChangeMode(deliverMode);
			AudioController.instance.play(AudioType.DRIVING);
			break;
		case GAMEMODE.SURVIVAL:
			ChangeMode(survivalMode);
			AudioController.instance.play(AudioType.DRIVING);
			break;
		case GAMEMODE.SLOT:
			ChangeMode(slotMode);
			AudioController.instance.play(AudioType.FIGHTING);
			break;
		case GAMEMODE.GUNKILLING:
			ChangeMode(gunKillingMode);
			AudioController.instance.play(AudioType.DRIVING);
			break;
		case GAMEMODE.CARKILLING:
			ChangeMode(carKillingMode);
			AudioController.instance.play(AudioType.DRIVING);
			break;
		case GAMEMODE.SKILLDRIVING:
			ChangeMode(skillDrivingMode);
			AudioController.instance.play(AudioType.FIGHTING);
			break;
		case GAMEMODE.ROBCAR:
			ChangeMode(robbingCarMode);
			AudioController.instance.play(AudioType.DRIVING);
			break;
		case GAMEMODE.FIGHTING:
			ChangeMode(fightingMode);
			AudioController.instance.play(AudioType.FIGHTING);
			break;
		case GAMEMODE.ROBMOTOR:
			ChangeMode(robMotorMode);
			AudioController.instance.play(AudioType.FIGHTING);
			break;
		}
		GameUIController.instance.getOnCarBtnFlag = false;
	}

	public void ChangeMode(GameModeController newMode)
	{
		if (curMode == normalMode && (newMode == deliverMode || newMode == gunKillingMode || newMode == carKillingMode))
		{
			PlayerController.instance.PlayerGetOffCar();
		}
		if (curMode.mode != 0 && curMode.mode != GAMEMODE.SKILLDRIVING && curMode.mode != GAMEMODE.SLOT)
		{
			AudioController.instance.stop(AudioType.DRIVING);
		}
		else if (newMode.mode != GAMEMODE.SLOT && newMode.mode != GAMEMODE.SKILLDRIVING && newMode.mode != 0)
		{
			AudioController.instance.stop(AudioType.FIGHTING);
		}
		preMode = curMode;
		curMode = newMode;
		curGameMode = curMode.mode;
		preMode.Exit();
		curMode.Reset(index);
	}
}
