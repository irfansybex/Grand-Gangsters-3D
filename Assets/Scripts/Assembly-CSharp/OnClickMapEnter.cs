using UnityEngine;

public class OnClickMapEnter : MonoBehaviour
{
	public GameObject mapController;

	public GameObject minimap;

	public GameObject mapUI;

	public GameObject mapPic;

	public Material mapMat;

	public GameObject normalUICam;

	public Material mapAlphMat;

	public bool enterFlag;

	public bool clickFlag;

	private bool exitMap;

	private float startTime;

	public bool entermissionFlag;

	public void OnClick()
	{
		if (GlobalInf.firstOpenGameFlag || GameController.instance.curGameMode != 0)
		{
			return;
		}
		if (enterFlag)
		{
			if (!clickFlag)
			{
				clickFlag = true;
				Invoke("Execute", 0.5f);
			}
		}
		else
		{
			if (Controller.instance.unLockLevelFlag)
			{
				return;
			}
			exitMap = true;
			startTime = Time.realtimeSinceStartup;
		}
		BlackScreen.instance.TurnOffScreen();
	}

	private void Update()
	{
		if (exitMap)
		{
			BlackScreen.instance.blackPix.GetComponent<Animation>()["TurnOffScreen"].time = Time.realtimeSinceStartup - startTime;
			if (BlackScreen.instance.blackPix.GetComponent<Animation>()["TurnOffScreen"].time > BlackScreen.instance.blackPix.GetComponent<Animation>()["TurnOffScreen"].length)
			{
				exitMap = false;
				Execute();
			}
		}
	}

	public void Execute()
	{
		if (enterFlag)
		{
			mapController.transform.position = minimap.transform.position;
			Time.timeScale = 0f;
			GlobalInf.mapMode = true;
			mapUI.SetActiveRecursively(true);
			mapController.SetActiveRecursively(true);
			minimap.SetActiveRecursively(false);
			mapPic.GetComponent<Renderer>().sharedMaterial = mapMat;
			normalUICam.SetActiveRecursively(false);
			normalUICam.SetActive(true);
			normalUICam.transform.GetChild(0).gameObject.SetActive(true);
			CitySenceController.instance.virtualMapController.GetPlayerLocation(PlayerController.instance.transform);
			VirtualminiMapController.instance.playerLocation.point1 = CitySenceController.instance.virtualMapController.playerLocation.point1;
			VirtualminiMapController.instance.playerLocation.point2 = CitySenceController.instance.virtualMapController.playerLocation.point2;
			VirtualminiMapController.instance.playerLocation.distanceFromPoint1 = CitySenceController.instance.virtualMapController.playerLocation.distanceFromPoint1;
			GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.MAP);
			AudioController.instance.pauseSounds();
			clickFlag = false;
			GameUIController.instance.topLine.gameObject.SetActiveRecursively(true);
			GameUIController.instance.topLine.backLine.SetActiveRecursively(false);
			return;
		}
		if (!entermissionFlag)
		{
			BlackScreen.instance.TurnOnScreen();
		}
		else
		{
			entermissionFlag = false;
		}
		GlobalInf.mapMode = false;
		mapController.SetActiveRecursively(false);
		minimap.SetActiveRecursively(true);
		mapUI.SetActiveRecursively(false);
		mapPic.GetComponent<Renderer>().sharedMaterial = mapAlphMat;
		normalUICam.SetActive(true);
		GameUIController.instance.InitUI();
		Time.timeScale = 1f;
		GameSenceBackBtnCtl.instance.PopGameUIState();
		if (GameUIController.instance.locateTargetList.Count != 0)
		{
			for (int i = 0; i < GameUIController.instance.locateTargetList.Count; i++)
			{
				GameUIController.instance.locateLabelList[i].gameObject.SetActiveRecursively(true);
			}
		}
		AudioController.instance.resumeSounds();
	}

	private void DelayPause()
	{
		Time.timeScale = 0f;
	}
}
