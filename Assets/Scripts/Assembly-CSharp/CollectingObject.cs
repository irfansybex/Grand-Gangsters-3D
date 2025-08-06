using UnityEngine;

public class CollectingObject : MonoBehaviour
{
	public COLLECTTYPE type;

	public int index;

	public GameObject obj;

	public GameObject label;

	public GameObject box;

	public BlockDateNew blockData;

	public bool eatFlag;

	private void OnTriggerEnter(Collider other)
	{
		if ((other.gameObject.layer != LayerMask.NameToLayer("Player") && other.gameObject.layer != LayerMask.NameToLayer("PlayerCar")) || eatFlag)
		{
			return;
		}
		AudioController.instance.play(AudioType.PICK_ITEM);
		switch (type)
		{
		case COLLECTTYPE.CAR:
			GlobalInf.collectCarData[index] = 1;
			StoreDateController.SetCollectingListInfo(GlobalInf.collectCarData, "CollectingCarData");
			GlobalInf.collectCarNum++;
			GameUIController.instance.pickLabel.label.text = "CollectingCar:" + GlobalInf.collectCarNum + "/" + GlobalDefine.COLLECT_CAR_MAXNUM;
			GameUIController.instance.pickLabel.Reset(base.transform.position);
			break;
		case COLLECTTYPE.HANDGUN:
			GlobalInf.collectHandGunData[index] = 1;
			StoreDateController.SetCollectingListInfo(GlobalInf.collectHandGunData, "CollectingHandGunData");
			GlobalInf.collectHandGunNum++;
			GameUIController.instance.pickLabel.label.text = "CollectingHandGun:" + GlobalInf.collectHandGunNum + "/" + GlobalDefine.COLLECT_HAND_GUN_MAXNUM;
			GameUIController.instance.pickLabel.Reset(base.transform.position);
			break;
		case COLLECTTYPE.MACHINEGUN:
			GlobalInf.collectMachineGunData[index] = 1;
			StoreDateController.SetCollectingListInfo(GlobalInf.collectMachineGunData, "CollectingMachineGunData");
			GlobalInf.collectMachineGunNum++;
			GameUIController.instance.pickLabel.label.text = "CollectingMachineGun:" + GlobalInf.collectMachineGunNum + "/" + GlobalDefine.COLLECT_MACHINE_GUN_MAXNUM;
			GameUIController.instance.pickLabel.Reset(base.transform.position);
			break;
		}
		for (int num = blockData.enableCollectingList.Count - 1; num >= 0; num--)
		{
			if (blockData.enableCollectingList[num].type == type && blockData.enableCollectingList[num].index == index)
			{
				blockData.enableCollectingList.RemoveAt(num);
			}
		}
		obj.GetComponent<Animation>().Play("EatCollectingObjAnima");
		Invoke("DelayRecycle", 0.8f);
		eatFlag = true;
		if (!GlobalInf.collectingTipsFlag)
		{
			GameUIController.instance.dialogUIController.Reset();
			PlayerPrefs.SetInt("CollectingTipsFlag", 1);
			GlobalInf.collectingTipsFlag = true;
		}
	}

	private void Update()
	{
		label.transform.eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y + 180f, 0f);
	}

	public void DelayRecycle()
	{
		eatFlag = false;
		CollectingObjPool.instance.RecycleCollectingObj(base.gameObject, type);
	}
}
