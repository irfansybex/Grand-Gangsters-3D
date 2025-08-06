using UnityEngine;

public class FallingObjCtl : MonoBehaviour
{
	public GameObject pic;

	public GameObject label;

	public FALLINGOBJTYPE fallingType;

	public int num;

	public bool recycleFlag;

	public float stayTime;

	public float countTime;

	public bool eattedFlag;

	public int tempBulletNum;

	private void Start()
	{
	}

	private void OnEnable()
	{
		eattedFlag = false;
		pic.transform.localScale = Vector3.one;
		label.transform.localScale = Vector3.one;
		pic.GetComponent<Renderer>().sharedMaterial.color = new Color(0.3125f, 0.3125f, 0.3125f, 1f);
		label.GetComponent<Renderer>().sharedMaterial.color = new Color(0.3125f, 0.3125f, 0.3125f, 0.52f);
		countTime = 0f;
		if (!GlobalDefine.smallPhoneFlag)
		{
			base.GetComponent<Animation>().Play("FallingObj");
		}
	}

	private void Update()
	{
		pic.transform.LookAt(Camera.main.transform);
		countTime += Time.deltaTime;
		if (countTime >= stayTime)
		{
			RecycleSelf();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (eattedFlag || (other.gameObject.layer != LayerMask.NameToLayer("Player") && other.gameObject.layer != LayerMask.NameToLayer("PlayerCar")))
		{
			return;
		}
		eattedFlag = true;
		switch (fallingType)
		{
		case FALLINGOBJTYPE.MONEY:
			GlobalInf.cash += num;
			GlobalInf.totalCashEarned += num;
			StoreDateController.SetCash();
			GameUIController.instance.pickLabel.Reset(ITEMTYPE.MONEY, num, base.transform.position);
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.PICK_CASH);
			}
			break;
		case FALLINGOBJTYPE.HANDGUNBULLET:
			GameUIController.instance.pickLabel.Reset(ITEMTYPE.HANDGUNBULLET, num, base.transform.position);
			tempBulletNum = PlayerController.instance.gun.gunInfo.maxBulletNum - PlayerController.instance.gun.gunInfo.restBulletNum - (PlayerController.instance.gun.gunInfo.curBulletNum - PlayerController.instance.gun.bulletCount);
			if (num > tempBulletNum)
			{
				num = tempBulletNum;
			}
			PlayerController.instance.gun.gunInfo.restBulletNum += num;
			StoreDateController.SetHandGunBulletNum(GlobalInf.handgunIndex, PlayerController.instance.gun.gunInfo.restBulletNum + (PlayerController.instance.gun.gunInfo.curBulletNum - PlayerController.instance.gun.bulletCount));
			if (PlayerController.instance.curState == PLAYERSTATE.HANDGUN)
			{
				GameUIController.instance.bulletNumLabel.text = string.Empty + (PlayerController.instance.gun.gunInfo.curBulletNum - PlayerController.instance.gun.bulletCount) + "/" + (PlayerController.instance.gun.gunInfo.restBulletNum + (PlayerController.instance.gun.gunInfo.curBulletNum - PlayerController.instance.gun.bulletCount));
				if (GameUIController.instance.bulletVideoBtn.gameObject.active)
				{
					GameUIController.instance.bulletVideoBtn.gameObject.SetActiveRecursively(false);
				}
			}
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.PICK_ITEM);
			}
			break;
		case FALLINGOBJTYPE.MACHINEGUNBULLET:
			GameUIController.instance.pickLabel.Reset(ITEMTYPE.MACHINEGUNBULLET, num, base.transform.position);
			tempBulletNum = PlayerController.instance.machineGun.gunInfo.maxBulletNum - PlayerController.instance.machineGun.gunInfo.restBulletNum - (PlayerController.instance.machineGun.gunInfo.curBulletNum - PlayerController.instance.machineGun.bulletCount);
			if (num > tempBulletNum)
			{
				num = tempBulletNum;
			}
			PlayerController.instance.machineGun.gunInfo.restBulletNum += num;
			StoreDateController.SetMachineGunBulletNum(GlobalInf.machineGunIndex, PlayerController.instance.machineGun.gunInfo.restBulletNum + (PlayerController.instance.machineGun.gunInfo.curBulletNum - PlayerController.instance.machineGun.bulletCount));
			if (PlayerController.instance.curState == PLAYERSTATE.MACHINEGUN)
			{
				GameUIController.instance.bulletNumLabel.text = string.Empty + (PlayerController.instance.machineGun.gunInfo.curBulletNum - PlayerController.instance.machineGun.bulletCount) + "/" + (PlayerController.instance.machineGun.gunInfo.restBulletNum + (PlayerController.instance.machineGun.gunInfo.curBulletNum - PlayerController.instance.machineGun.bulletCount));
				if (GameUIController.instance.bulletVideoBtn.gameObject.active)
				{
					GameUIController.instance.bulletVideoBtn.gameObject.SetActiveRecursively(false);
				}
			}
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.PICK_ITEM);
			}
			break;
		case FALLINGOBJTYPE.HANDGUN:
			if (PlayerController.instance.gun == null)
			{
				GameSenceTutorialController.instance.CopyHandGunInfo();
				if (recycleFlag)
				{
					recycleFlag = false;
				}
			}
			if (AudioController.instance != null)
			{
				AudioController.instance.play(AudioType.PICK_ITEM);
			}
			GameUIController.instance.pickLabel.Reset(ITEMTYPE.HANDGUN, num, base.transform.position);
			break;
		}
		if (!GlobalDefine.smallPhoneFlag)
		{
			base.GetComponent<Animation>().Play("EatFallingObj");
		}
		Invoke("RecycleSelf", 1f);
	}

	public void RecycleSelf()
	{
		if (!recycleFlag)
		{
			FallingObjPool.instance.Recycle(this);
		}
	}
}
