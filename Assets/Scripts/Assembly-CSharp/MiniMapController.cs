using System;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
	public Camera mapCam;

	public LayerMask minimapLayer;

	public LayerMask minimapTaskLabelLayer;

	public Transform mapLeftDownAnchor;

	public MapDrawPath mapDrawPath;

	public Vector3 leftDownBorder;

	public Vector3 rightUpBorder;

	public RoadPointNew startPoint;

	public RoadPointNew endPoint;

	public Vector2 old1;

	public Vector2 old2;

	public Vector3 playerpos;

	public Vector3 clickpos;

	public RaycastHit hit;

	public LocateInfoNew preendlocateInfo;

	public List<UISprite> UIlist;

	public Vector3 tempworldpos;

	public float screenRate;

	public GameObject mapLabelRoot;

	public Transform deliverRoot;

	public Transform drivingRoot;

	public Transform survivalRoot;

	public Transform gunKillingRoot;

	public Transform carKillingRoot;

	public bool mapUIFlag;

	public OnClickMapEnter mapBackBtn;

	public Vector2 preClickPos;

	public Vector3 preTargetClickPos;

	public GameObject[] levelMaskObj;

	public Material unlockMat;

	public GameObject[] levelLine;

	public Material levelLineMat;

	public UILabel starNumLabel;

	public GameObject starRoot;

	public UIWidget[] mapLevelInfoRoot;

	public UIEventListener[] normalUnlockLevelBtn;

	public UIEventListener unlockLevelBtn;

	public UILabel unLockLevelLabel;

	public int unlockLevelPrise;

	public bool enterMapBlackScreenFlag;

	public int unLockStape;

	public float autoMovePercent;

	public Vector3[] moveTarget;

	public float autoMoveStartTime;

	public Vector3 startPos;

	public float levelLinePercent;

	private float animaStartTime;

	public bool policeFlag;

	public float policeStartTime;

	private float startTime;

	private Vector2 clickPos;

	private Vector3 carPos;

	private float speedCount = 1f;

	private Vector2 startSpeed;

	private Vector2 touchDeltaPosition = Vector2.zero;

	private float startSpeedTime;

	private void Awake()
	{
		screenRate = (float)Screen.width / (float)Screen.height;
	}

	private void Start()
	{
		UIEventListener uIEventListener = unlockLevelBtn;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickUnLockLevelBtn));
		for (int i = 0; i < normalUnlockLevelBtn.Length; i++)
		{
			UIEventListener obj = normalUnlockLevelBtn[i];
			obj.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(obj.onClick, new UIEventListener.VoidDelegate(OnClickNormalUnLockBtn));
		}
	}

	public void OnClickNormalUnLockBtn(GameObject btn)
	{
		MonoBehaviour.print("OnClickNormalUnLockBtn");
		unLockStape = 2;
		autoMoveStartTime = Time.realtimeSinceStartup;
		Controller.instance.unLockLevelFlag = true;
	}

	public void OnClickUnLockLevelBtn(GameObject btn)
	{
		if (GlobalInf.gold >= unlockLevelPrise)
		{
			GlobalInf.gold -= unlockLevelPrise;
			StoreDateController.SetGold();
			unLockStape = 2;
			autoMoveStartTime = Time.realtimeSinceStartup;
			Controller.instance.unLockLevelFlag = true;
			Platform.flurryEvent_onEquipmentLevelPurchase(GlobalInf.gameLevel);
			if (!GlobalInf.newUserFlag)
			{
				if (GlobalInf.gameState >= GameStateController.MAXSTATENUM)
				{
					return;
				}
				GlobalInf.gameState = GameStateController.MAXSTATENUM;
				StoreDateController.SetGameState();
				if (GameController.instance.curGameMode == GAMEMODE.NORMAL)
				{
					MinimapLightLabelController.instance.EnableLightLabel();
					for (int i = 0; i < MinimapLightLabelController.instance.lightLabel2.Length; i++)
					{
						if (MinimapLightLabelController.instance.lightLabel2[i].gameObject.active)
						{
							MinimapLightLabelController.instance.lightLabel2[i].EnableStars(TaskLabelController.instance.GetStarNum(MinimapLightLabelController.instance.lightLabel2[i].mode, MinimapLightLabelController.instance.lightLabel2[i].index));
						}
					}
				}
				else
				{
					MinimapLightLabelController.instance.DisableLightLabel();
				}
			}
			else
			{
				if (GlobalInf.gameState >= GameStateController.NEWMAXSTATENUM)
				{
					return;
				}
				GlobalInf.gameState = GameStateController.NEWMAXSTATENUM;
				StoreDateController.SetGameState();
				if (GameController.instance.curGameMode == GAMEMODE.NORMAL)
				{
					MinimapLightLabelController.instance.EnableLightLabel();
					for (int j = 0; j < MinimapLightLabelController.instance.lightLabel2.Length; j++)
					{
						if (MinimapLightLabelController.instance.lightLabel2[j].gameObject.active)
						{
							MinimapLightLabelController.instance.lightLabel2[j].EnableStars(TaskLabelController.instance.GetStarNum(MinimapLightLabelController.instance.lightLabel2[j].mode, MinimapLightLabelController.instance.lightLabel2[j].index));
						}
					}
				}
				else
				{
					MinimapLightLabelController.instance.DisableLightLabel();
				}
			}
		}
		else
		{
			GameUIController.instance.topLine.OnClickGoldBtn(null);
			GlobalInf.chargeShowType = CHARGESHOWTYPE.LEVEL;
			Platform.OnChargeEvent(GlobalInf.chargeShowType, 0);
		}
	}

	private void OnEnable()
	{
		mapCam.orthographicSize = 70f;
		enterMapBlackScreenFlag = true;
		BlackScreen.instance.TurnOnScreen();
		if (GameController.instance.curGameMode == GAMEMODE.NORMAL)
		{
			MinimapLightLabelController.instance.EnableLightLabel();
			for (int i = 0; i < MinimapLightLabelController.instance.lightLabel2.Length; i++)
			{
				if (MinimapLightLabelController.instance.lightLabel2[i].gameObject.active)
				{
					MinimapLightLabelController.instance.lightLabel2[i].EnableStars(TaskLabelController.instance.GetStarNum(MinimapLightLabelController.instance.lightLabel2[i].mode, MinimapLightLabelController.instance.lightLabel2[i].index));
				}
			}
		}
		else
		{
			MinimapLightLabelController.instance.DisableLightLabel();
		}
		if (Controller.instance.unLockLevelFlag)
		{
			Controller.instance.levelObj[GlobalInf.gameLevel].gameObject.SetActiveRecursively(false);
			Controller.instance.levelObj[GlobalInf.gameLevel].gameObject.SetActive(true);
			levelMaskObj[GlobalInf.gameLevel].gameObject.SetActiveRecursively(true);
			levelLine[GlobalInf.gameLevel * 2].gameObject.SetActiveRecursively(true);
			levelLine[GlobalInf.gameLevel * 2 + 1].gameObject.SetActiveRecursively(true);
			GameUIController.instance.fingerObj.transform.position = normalUnlockLevelBtn[GlobalInf.gameLevel].transform.position;
			GameUIController.instance.miniMapFinger.gameObject.GetComponent<BoxCollider>().enabled = false;
		}
		starNumLabel.text = string.Empty + Controller.instance.sumStarNum + "/" + 30 * (GlobalInf.gameLevel + 1);
		for (int j = 0; j < 3; j++)
		{
			if (j < GlobalInf.gameLevel)
			{
				mapLevelInfoRoot[j].gameObject.SetActiveRecursively(false);
				continue;
			}
			mapLevelInfoRoot[j].gameObject.SetActiveRecursively(true);
			if (j == GlobalInf.gameLevel)
			{
				if (Controller.instance.sumStarNum >= Controller.instance.levelUnlockStarNum[GlobalInf.gameLevel + 1])
				{
					normalUnlockLevelBtn[j].gameObject.GetComponent<UISprite>().spriteName = "play";
					normalUnlockLevelBtn[j].gameObject.GetComponent<BoxCollider>().enabled = true;
				}
				else
				{
					normalUnlockLevelBtn[j].gameObject.GetComponent<UISprite>().spriteName = "huidu";
					normalUnlockLevelBtn[j].gameObject.GetComponent<BoxCollider>().enabled = false;
				}
			}
			else
			{
				normalUnlockLevelBtn[j].gameObject.GetComponent<UISprite>().spriteName = "huidu";
				normalUnlockLevelBtn[j].gameObject.GetComponent<BoxCollider>().enabled = false;
			}
		}
		if (GlobalInf.gameLevel >= 3)
		{
			unlockLevelBtn.gameObject.SetActiveRecursively(false);
		}
		else if (Controller.instance.sumStarNum >= Controller.instance.levelUnlockStarNum[GlobalInf.gameLevel + 1] || GlobalInf.gameState < 4)
		{
			unlockLevelBtn.gameObject.SetActiveRecursively(false);
		}
		else
		{
			unlockLevelPrise = (Controller.instance.levelUnlockStarNum[GlobalInf.gameLevel + 1] - Controller.instance.sumStarNum) * 2;
			unlockLevelBtn.gameObject.SetActiveRecursively(true);
			unlockLevelBtn.transform.parent = mapLevelInfoRoot[GlobalInf.gameLevel].transform;
			unlockLevelBtn.transform.localPosition = new Vector3(0f, -115f, 0f);
			unLockLevelLabel.text = string.Empty + unlockLevelPrise;
		}
		CheckCamPos();
	}

	public void ResetUnLockLevel()
	{
		if (GlobalInf.gameLevel >= 3)
		{
			unlockLevelBtn.gameObject.SetActiveRecursively(false);
			return;
		}
		if (Controller.instance.sumStarNum >= Controller.instance.levelUnlockStarNum[GlobalInf.gameLevel + 1] || GlobalInf.gameState < 4)
		{
			unlockLevelBtn.gameObject.SetActiveRecursively(false);
			return;
		}
		unlockLevelPrise = (Controller.instance.levelUnlockStarNum[GlobalInf.gameLevel + 1] - Controller.instance.sumStarNum) * 2;
		unlockLevelBtn.gameObject.SetActiveRecursively(true);
		unlockLevelBtn.transform.parent = mapLevelInfoRoot[GlobalInf.gameLevel].transform;
		unlockLevelBtn.transform.localPosition = new Vector3(0f, -115f, 0f);
		unLockLevelLabel.text = string.Empty + unlockLevelPrise;
	}

	private void OnDisable()
	{
		for (int i = 0; i < MinimapLightLabelController.instance.lightLabel2.Length; i++)
		{
			MinimapLightLabelController.instance.lightLabel2[i].DisableStars();
		}
		if (GameUIController.instance != null)
		{
			GameUIController.instance.DisableOutOffAmmo();
		}
	}

	public void CheckCamPos()
	{
		leftDownBorder = new Vector3(mapCam.orthographicSize * screenRate + 20f, -470f, mapCam.orthographicSize + 20f);
		rightUpBorder = new Vector3(380f - mapCam.orthographicSize * screenRate, -470f, 380f - mapCam.orthographicSize);
		if (mapCam.transform.localPosition.x < leftDownBorder.x)
		{
			mapCam.transform.localPosition = new Vector3(leftDownBorder.x, mapCam.transform.localPosition.y, mapCam.transform.localPosition.z);
		}
		if (mapCam.transform.localPosition.x > rightUpBorder.x)
		{
			mapCam.transform.localPosition = new Vector3(rightUpBorder.x, mapCam.transform.localPosition.y, mapCam.transform.localPosition.z);
		}
		if (mapCam.transform.localPosition.z < leftDownBorder.z)
		{
			mapCam.transform.localPosition = new Vector3(mapCam.transform.localPosition.x, mapCam.transform.localPosition.y, leftDownBorder.z);
		}
		if (mapCam.transform.localPosition.z > rightUpBorder.z)
		{
			mapCam.transform.localPosition = new Vector3(mapCam.transform.localPosition.x, mapCam.transform.localPosition.y, rightUpBorder.z);
		}
	}

	private void Update()
	{
		if (Controller.instance.unLockLevelFlag)
		{
			GameUIController.instance.fingerObj.transform.position = normalUnlockLevelBtn[GlobalInf.gameLevel].transform.position;
			if (unLockStape == 0 && !BlackScreen.instance.blackPix.gameObject.active)
			{
				autoMoveStartTime = Time.realtimeSinceStartup;
				unLockStape = 1;
			}
			if (unLockStape == 1)
			{
				if (autoMovePercent < 0.47f)
				{
					autoMovePercent = (Time.realtimeSinceStartup - autoMoveStartTime - 0.5f) / 3f;
					mapCam.transform.localPosition = Vector3.Lerp(mapCam.transform.localPosition, moveTarget[GlobalInf.gameLevel], autoMovePercent);
				}
				else
				{
					unLockStape = 7;
					unlockMat.color = levelMaskObj[GlobalInf.gameLevel].GetComponent<Renderer>().sharedMaterial.color;
					levelMaskObj[GlobalInf.gameLevel].GetComponent<Renderer>().sharedMaterial = unlockMat;
					autoMovePercent = levelMaskObj[GlobalInf.gameLevel].GetComponent<Renderer>().sharedMaterial.color.a;
					levelLinePercent = 1f;
					levelLine[GlobalInf.gameLevel * 2].GetComponent<Renderer>().sharedMaterial = levelLineMat;
					levelLine[GlobalInf.gameLevel * 2 + 1].GetComponent<Renderer>().sharedMaterial = levelLineMat;
					levelLineMat.color = new Color(1f, 1f, 1f, 1f);
				}
			}
			if (unLockStape == 2)
			{
				if (Time.realtimeSinceStartup - autoMoveStartTime < 1f)
				{
					unlockMat.color = new Color(1f, 1f, 1f, Mathf.Lerp(autoMovePercent, 0f, Time.realtimeSinceStartup - autoMoveStartTime));
					levelLineMat.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, Time.realtimeSinceStartup - autoMoveStartTime));
					mapLevelInfoRoot[GlobalInf.gameLevel].alpha = 1f - (Time.realtimeSinceStartup - autoMoveStartTime);
					if (GlobalInf.gameLevel < 2)
					{
						starNumLabel.text = string.Empty + Controller.instance.sumStarNum + "/" + (int)(Mathf.Lerp(30 * (GlobalInf.gameLevel + 1), 30 * (GlobalInf.gameLevel + 2), Time.realtimeSinceStartup - autoMoveStartTime) + 0.9f);
					}
					else if (starRoot.gameObject.active)
					{
						starRoot.gameObject.SetActiveRecursively(false);
					}
				}
				else
				{
					unLockStape = 3;
					mapLevelInfoRoot[GlobalInf.gameLevel].gameObject.SetActiveRecursively(false);
				}
			}
			if (unLockStape == 3)
			{
				Controller.instance.LevelUp();
				unLockStape = 4;
				GameUIController.instance.miniMapFinger.gameObject.GetComponent<BoxCollider>().enabled = true;
				if (GlobalInf.gameLevel >= 3)
				{
					unlockLevelBtn.gameObject.SetActiveRecursively(false);
					return;
				}
				if (Controller.instance.sumStarNum >= Controller.instance.levelUnlockStarNum[GlobalInf.gameLevel + 1] || GlobalInf.gameState < 4)
				{
					unlockLevelBtn.gameObject.SetActiveRecursively(false);
					return;
				}
				unlockLevelPrise = (Controller.instance.levelUnlockStarNum[GlobalInf.gameLevel + 1] - Controller.instance.sumStarNum) * 2;
				unlockLevelBtn.transform.parent = mapLevelInfoRoot[GlobalInf.gameLevel].transform;
				unlockLevelBtn.transform.localPosition = new Vector3(0f, -115f, 0f);
				unLockLevelLabel.text = string.Empty + unlockLevelPrise;
				NGUITools.SetActiveRecursively(unlockLevelBtn.gameObject, false);
				NGUITools.SetActiveRecursively(unlockLevelBtn.gameObject, true);
			}
		}
		else
		{
			if (!mapUIFlag)
			{
				ClickMap();
				moveminimap();
			}
			if (policeFlag && Time.realtimeSinceStartup - policeStartTime >= 2f)
			{
				policeFlag = false;
				GameUIController.instance.DisableOutOffAmmo();
			}
		}
	}

	public void OnEnableTaskUI(GameObject taskObj)
	{
		policeFlag = false;
		if (PoliceLevelCtl.level != 0)
		{
			policeFlag = true;
			policeStartTime = Time.realtimeSinceStartup;
			GameUIController.instance.EnableOutOffAmmo("You have to escape from police chase\nbefore entering a task");
			return;
		}
		int taskIndex = int.Parse(taskObj.name);
		if (taskObj.transform.parent.gameObject.name.Equals("DeliverRoot"))
		{
			GameUIController.instance.taskCheckUI.gameObject.SetActiveRecursively(true);
			GameUIController.instance.gameMode = GAMEMODE.DELIVER;
			GameUIController.instance.taskIndex = taskIndex;
			GameUIController.instance.ResetTaskCheckLabel();
			mapUIFlag = true;
		}
		else if (taskObj.transform.parent.gameObject.name.Equals("DrivingRoot"))
		{
			GameUIController.instance.taskCheckUI.gameObject.SetActiveRecursively(true);
			GameUIController.instance.gameMode = GAMEMODE.DRIVING0;
			GameUIController.instance.taskIndex = taskIndex;
			GameUIController.instance.ResetTaskCheckLabel();
			mapUIFlag = true;
		}
		else if (taskObj.transform.parent.gameObject.name.Equals("SurvivalRoot"))
		{
			GameUIController.instance.taskCheckUI.gameObject.SetActiveRecursively(true);
			GameUIController.instance.gameMode = GAMEMODE.SURVIVAL;
			GameUIController.instance.taskIndex = taskIndex;
			GameUIController.instance.ResetTaskCheckLabel();
			mapUIFlag = true;
		}
		else if (taskObj.transform.parent.gameObject.name.Equals("GunKillingRoot"))
		{
			GameUIController.instance.taskCheckUI.gameObject.SetActiveRecursively(true);
			GameUIController.instance.gameMode = GAMEMODE.GUNKILLING;
			GameUIController.instance.taskIndex = taskIndex;
			GameUIController.instance.ResetTaskCheckLabel();
			mapUIFlag = true;
		}
		else if (taskObj.transform.parent.gameObject.name.Equals("CarKillingRoot"))
		{
			GameUIController.instance.taskCheckUI.gameObject.SetActiveRecursively(true);
			GameUIController.instance.gameMode = GAMEMODE.CARKILLING;
			GameUIController.instance.taskIndex = taskIndex;
			GameUIController.instance.ResetTaskCheckLabel();
			mapUIFlag = true;
		}
		else if (taskObj.transform.parent.gameObject.name.Equals("SkillDrivingRoot"))
		{
			GameUIController.instance.taskCheckUI.gameObject.SetActiveRecursively(true);
			GameUIController.instance.gameMode = GAMEMODE.SKILLDRIVING;
			GameUIController.instance.taskIndex = taskIndex;
			GameUIController.instance.ResetTaskCheckLabel();
			mapUIFlag = true;
		}
		else if (taskObj.transform.parent.gameObject.name.Equals("RobCarRoot"))
		{
			GameUIController.instance.taskCheckUI.gameObject.SetActiveRecursively(true);
			GameUIController.instance.gameMode = GAMEMODE.ROBCAR;
			GameUIController.instance.taskIndex = taskIndex;
			GameUIController.instance.ResetTaskCheckLabel();
			mapUIFlag = true;
		}
		else if (taskObj.transform.parent.gameObject.name.Equals("FightingRoot"))
		{
			GameUIController.instance.taskCheckUI.gameObject.SetActiveRecursively(true);
			GameUIController.instance.gameMode = GAMEMODE.FIGHTING;
			GameUIController.instance.taskIndex = taskIndex;
			GameUIController.instance.ResetTaskCheckLabel();
			mapUIFlag = true;
		}
		else if (taskObj.transform.parent.gameObject.name.Equals("RobMotorRoot"))
		{
			GameUIController.instance.taskCheckUI.gameObject.SetActiveRecursively(true);
			GameUIController.instance.gameMode = GAMEMODE.ROBMOTOR;
			GameUIController.instance.taskIndex = taskIndex;
			GameUIController.instance.ResetTaskCheckLabel();
			mapUIFlag = true;
		}
		GameUIController.instance.rewardIndex = TaskLabelController.instance.GetRewardIndex(GameUIController.instance.gameMode, GameUIController.instance.taskIndex);
		GameSenceBackBtnCtl.instance.ChangeGameUIState(GAMEUISTATE.TASKCHECK);
		Platform.showFeatureView(FeatureViewPosType.MIDDLE);
	}

	public void EnablePlayerCarPos(Vector3 pos)
	{
		mapDrawPath.carFlag = true;
		carPos = AttackAILabelPool.instance.ChangToMapLocalPos(pos) + new Vector3(1000f, 480f, 0f);
		mapDrawPath.carPos = new Vector3(carPos.x, -493f, carPos.z);
	}

	public void DisablePlayerCarPos()
	{
		mapDrawPath.carFlag = false;
	}

	public void EnableTargetPos(Vector3 pos)
	{
		if (mapDrawPath.targetFlag && Vector3.Distance(pos, preTargetClickPos) < 5f)
		{
			DisableTargetPos();
			return;
		}
		mapDrawPath.targetFlag = true;
		preTargetClickPos = pos;
		mapDrawPath.targetPos = new Vector3(pos.x, -493f, pos.z);
	}

	public void DisableTargetPos()
	{
		mapDrawPath.targetFlag = false;
		mapDrawPath.ClearLine();
	}

	public void FindWay(Vector3 targetPos)
	{
		if (targetPos.x > -800f && targetPos.x < 800f && targetPos.z > -900f && targetPos.z < 900f)
		{
			Findclickwaypoint(targetPos);
			if (!VirtualminiMapController.instance.targetCrossFlag)
			{
				GlobalInf.isDrawPathInfo = true;
			}
		}
	}

	public void ClickMap()
	{
		if (GameController.instance.curGameMode != 0)
		{
			return;
		}
		if (!GlobalInf.newUserFlag)
		{
			if (GlobalInf.gameState < GameStateController.MAXSTATENUM)
			{
				return;
			}
		}
		else if (GlobalInf.gameState < GameStateController.NEWMAXSTATENUM)
		{
			return;
		}
		if (Input.touchCount != 1)
		{
			return;
		}
		if (Input.GetTouch(0).phase == TouchPhase.Began && IsRightPos(Input.GetTouch(0).position))
		{
			startTime = Time.time;
			clickPos = Input.GetTouch(0).position;
		}
		if (Input.GetTouch(0).phase != TouchPhase.Ended || !IsRightPos(Input.GetTouch(0).position) || !(Time.time - startTime < 0.8f) || !(Vector2.Distance(clickPos, Input.GetTouch(0).position) < 25f))
		{
			return;
		}
		if (GlobalInf.isDrawPathInfo)
		{
			if (Vector2.Distance(Input.GetTouch(0).position, preClickPos) < 30f)
			{
				DisableTargetPos();
				return;
			}
			Ray ray = base.GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);
			if (!Physics.Raycast(ray, out hit, 1000f))
			{
				return;
			}
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MiniMap"))
			{
				tempworldpos = ChangeToWorldPos(hit.point);
				EnableTargetPos(hit.point);
				if (tempworldpos.x > -800f && tempworldpos.x < 800f && tempworldpos.z > -900f && tempworldpos.z < 900f)
				{
					Findclickwaypoint(tempworldpos);
					if (!VirtualminiMapController.instance.targetCrossFlag)
					{
						GlobalInf.isDrawPathInfo = true;
						preClickPos = Input.GetTouch(0).position;
					}
				}
			}
			else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MapUI"))
			{
				OnEnableTaskUI(hit.collider.gameObject);
			}
			return;
		}
		Ray ray2 = base.GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);
		if (!Physics.Raycast(ray2, out hit, 1000f))
		{
			return;
		}
		if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MiniMap"))
		{
			tempworldpos = ChangeToWorldPos(hit.point);
			EnableTargetPos(hit.point);
			if (tempworldpos.x > -800f && tempworldpos.x < 800f && tempworldpos.z > -900f && tempworldpos.z < 900f)
			{
				Findclickwaypoint(tempworldpos);
				if (!VirtualminiMapController.instance.targetCrossFlag)
				{
					GlobalInf.isDrawPathInfo = true;
					preClickPos = Input.GetTouch(0).position;
				}
			}
		}
		else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MapUI"))
		{
			OnEnableTaskUI(hit.collider.gameObject);
		}
	}

	public Vector3 ChangeToWorldPos(Vector3 point)
	{
		float x = (point.x - mapLeftDownAnchor.transform.position.x) * 5f + (float)VirtualminiMapController.instance.blockMap.startX - 200f;
		float y = 0f;
		float z = (point.z - mapLeftDownAnchor.transform.position.z) * 5f + (float)VirtualminiMapController.instance.blockMap.startY - 100f;
		return new Vector3(x, y, z);
	}

	public void Findclickwaypoint(Vector3 temp)
	{
		if (!GlobalDefine.smallPhoneFlag && CitySenceController.instance.virtualMapController.playerLocation.point1.roadInfo == null)
		{
			GameController.instance.normalMode.virtualMapController.GetPlayerLocation(PlayerController.instance.transform);
		}
		if (GlobalDefine.smallPhoneFlag || !(CitySenceController.instance.virtualMapController.playerLocation.point1.roadInfo != null))
		{
			return;
		}
		Vector3 position = PlayerController.instance.transform.position;
		VirtualminiMapController.instance.GetPlayerLocation(position);
		RoadPointNew point = VirtualminiMapController.instance.playerLocation.point1;
		RoadPointNew point2 = VirtualminiMapController.instance.playerLocation.point2;
		float distanceFromPoint = VirtualminiMapController.instance.playerLocation.distanceFromPoint1;
		startPoint = VirtualminiMapController.instance.playerLocation.point1;
		VirtualminiMapController.instance.GetTargetLocation(temp);
		if (VirtualminiMapController.instance.targetCrossFlag)
		{
			return;
		}
		endPoint = VirtualminiMapController.instance.targetLocation.point1;
		RoadPointNew point3 = VirtualminiMapController.instance.targetLocation.point1;
		RoadPointNew point4 = VirtualminiMapController.instance.targetLocation.point2;
		float distanceFromPoint2 = VirtualminiMapController.instance.targetLocation.distanceFromPoint1;
		if (startPoint.roadInfo == endPoint.roadInfo)
		{
			int a = 0;
			int b = 0;
			for (int i = 0; i < point.roadInfo.roadPointList.Length; i++)
			{
				if (point == point.roadInfo.roadPointList[i])
				{
					a = i;
				}
				if (point2 == point.roadInfo.roadPointList[i])
				{
					b = i;
				}
			}
			int c = 0;
			int d = 0;
			for (int j = 0; j < point3.roadInfo.roadPointList.Length; j++)
			{
				if (point3 == point3.roadInfo.roadPointList[j])
				{
					c = j;
				}
				if (point4 == point3.roadInfo.roadPointList[j])
				{
					d = j;
				}
			}
			if (doubleNotEqual(a, b, c, d))
			{
				Vector2 vector = FindMidNum(a, b, c, d);
				startPoint = point.roadInfo.roadPointList[(int)vector.x];
				endPoint = point3.roadInfo.roadPointList[(int)vector.y];
			}
			else
			{
				int num = findSameNum(a, b, c, d);
				startPoint = point.roadInfo.roadPointList[num];
				endPoint = point3.roadInfo.roadPointList[num];
			}
			Vector3 to = PlayerController.instance.transform.position - point.position;
			Vector3 forward = point.forward;
			float num2 = Vector3.Angle(forward, to);
			if (num2 > 90f)
			{
				playerpos = point.position - distanceFromPoint * point.forward;
			}
			else
			{
				playerpos = point.position + distanceFromPoint * point.forward;
			}
			to = temp - point3.position;
			forward = point3.forward;
			num2 = Vector3.Angle(forward, to);
			if (num2 > 90f)
			{
				clickpos = point3.position - distanceFromPoint2 * point3.forward;
			}
			else
			{
				clickpos = point3.position + distanceFromPoint2 * point3.forward;
			}
			mapDrawPath.clickpos = clickpos;
			mapDrawPath.start = startPoint;
			mapDrawPath.end = endPoint;
			mapDrawPath.playerpos = playerpos;
			return;
		}
		Vector2 vector2 = RoadPathInfo.instance.FindTwoWayShortCrosspoint(startPoint, endPoint);
		int num3 = 0;
		int num4 = 0;
		for (int k = 0; k < point.roadInfo.roadPointList.Length; k++)
		{
			if (point == point.roadInfo.roadPointList[k])
			{
				num3 = k;
			}
			if (point2 == point.roadInfo.roadPointList[k])
			{
				num4 = k;
			}
		}
		if (vector2.x == 0f)
		{
			if (num3 > num4)
			{
				startPoint = point2;
			}
			else
			{
				startPoint = point;
			}
		}
		else if (num3 > num4)
		{
			startPoint = point;
		}
		else
		{
			startPoint = point2;
		}
		Vector3 to2 = position - point.position;
		Vector3 forward2 = point.forward;
		float num5 = Vector3.Angle(forward2, to2);
		if (num5 > 90f)
		{
			playerpos = point.position - distanceFromPoint * point.forward;
		}
		else
		{
			playerpos = point.position + distanceFromPoint * point.forward;
		}
		num3 = 0;
		num4 = 0;
		for (int l = 0; l < point3.roadInfo.roadPointList.Length; l++)
		{
			if (point3 == point3.roadInfo.roadPointList[l])
			{
				num3 = l;
			}
			if (point4 == point3.roadInfo.roadPointList[l])
			{
				num4 = l;
			}
		}
		if (vector2.y == 0f)
		{
			if (num3 > num4)
			{
				endPoint = point4;
			}
			else
			{
				endPoint = point3;
			}
		}
		else if (num3 > num4)
		{
			endPoint = point3;
		}
		else
		{
			endPoint = point4;
		}
		to2 = temp - point3.position;
		forward2 = point3.forward;
		num5 = Vector3.Angle(forward2, to2);
		if (num5 > 90f)
		{
			clickpos = point3.position - distanceFromPoint2 * point3.forward;
		}
		else
		{
			clickpos = point3.position + distanceFromPoint2 * point3.forward;
		}
		mapDrawPath.clickpos = clickpos;
		mapDrawPath.start = startPoint;
		mapDrawPath.end = endPoint;
		mapDrawPath.playerpos = playerpos;
	}

	public void moveminimap()
	{
		leftDownBorder = new Vector3(mapCam.orthographicSize * screenRate + 20f, -470f, mapCam.orthographicSize + 20f);
		rightUpBorder = new Vector3(380f - mapCam.orthographicSize * screenRate, -470f, 380f - mapCam.orthographicSize);
		if (speedCount < 1f)
		{
			speedCount = (Time.realtimeSinceStartup - startSpeedTime) * 2f;
			touchDeltaPosition = Vector2.Lerp(startSpeed, Vector2.zero, speedCount);
		}
		mapCam.transform.localPosition += new Vector3(touchDeltaPosition.x / GlobalDefine.screenScale.x, 0f, touchDeltaPosition.y / GlobalDefine.screenScale.y);
		if (Input.touchCount == 1)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				touchDeltaPosition = Input.GetTouch(0).deltaPosition * -0.5f * Mathf.Lerp(0.25f, 1f, (mapCam.orthographicSize - 40f) / 40f);
				if (GlobalDefine.smallPhoneFlag)
				{
					touchDeltaPosition *= 2f;
				}
			}
			if (Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				speedCount = 0f;
				startSpeedTime = Time.realtimeSinceStartup;
				startSpeed = touchDeltaPosition;
			}
		}
		else if (Input.touchCount > 1 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
		{
			Vector2 position = Input.GetTouch(0).position;
			Vector2 position2 = Input.GetTouch(1).position;
			if (islarge(old1, old2, position, position2))
			{
				mapCam.orthographicSize -= 1.5f;
				if (mapCam.GetComponent<Camera>().orthographicSize < 40f)
				{
					mapCam.orthographicSize = 40f;
				}
				ChangeSize(mapCam.orthographicSize);
			}
			else
			{
				mapCam.orthographicSize += 1.5f;
				if (mapCam.orthographicSize > 100f)
				{
					mapCam.orthographicSize = 100f;
				}
				ChangeSize(mapCam.orthographicSize);
			}
			old1 = position;
			old2 = position2;
		}
		if (mapCam.transform.localPosition.x < leftDownBorder.x)
		{
			mapCam.transform.localPosition = new Vector3(leftDownBorder.x, mapCam.transform.localPosition.y, mapCam.transform.localPosition.z);
		}
		if (mapCam.transform.localPosition.x > rightUpBorder.x)
		{
			mapCam.transform.localPosition = new Vector3(rightUpBorder.x, mapCam.transform.localPosition.y, mapCam.transform.localPosition.z);
		}
		if (mapCam.transform.localPosition.z < leftDownBorder.z)
		{
			mapCam.transform.localPosition = new Vector3(mapCam.transform.localPosition.x, mapCam.transform.localPosition.y, leftDownBorder.z);
		}
		if (mapCam.transform.localPosition.z > rightUpBorder.z)
		{
			mapCam.transform.localPosition = new Vector3(mapCam.transform.localPosition.x, mapCam.transform.localPosition.y, rightUpBorder.z);
		}
	}

	public void ChangeSize(float screenSize)
	{
	}

	public bool islarge(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
	{
		float num = (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
		float num2 = (c.x - d.x) * (c.x - d.x) + (c.y - d.y) * (c.y - d.y);
		if (num < num2 - 5f)
		{
			return true;
		}
		return false;
	}

	public bool IsRightPos(Vector2 pos)
	{
		Vector2 vector = new Vector2(pos.x / (float)Screen.width * GlobalDefine.screenRatioWidth, pos.y / (float)Screen.height * 480f);
		for (int i = 0; i < UIlist.Count; i++)
		{
			Vector2 vector2 = new Vector2(UIlist[i].transform.localPosition.x + GlobalDefine.screenRatioWidth / 2f, UIlist[i].transform.localPosition.y + 240f);
			Vector2 vector3 = new Vector2(vector2.x - (float)(UIlist[i].width / 2), vector2.y - (float)(UIlist[i].width / 2));
			Vector2 vector4 = new Vector2(vector2.x + (float)(UIlist[i].width / 2), vector2.y + (float)(UIlist[i].width / 2));
			if (vector.x > vector3.x && vector.y > vector3.y && vector.x < vector4.x && vector.y < vector4.y)
			{
				return false;
			}
		}
		return true;
	}

	public bool doubleNotEqual(int a, int b, int c, int d)
	{
		if (a == c && b == d)
		{
			return false;
		}
		if (a == d && b == c)
		{
			return false;
		}
		return true;
	}

	public Vector2 FindMidNum(int a, int b, int c, int d)
	{
		Vector2 result = default(Vector2);
		if (a > c)
		{
			if (a > b)
			{
				result.x = b;
			}
			else
			{
				result.x = a;
			}
			if (c > d)
			{
				result.y = c;
			}
			else
			{
				result.y = d;
			}
		}
		else
		{
			if (a > b)
			{
				result.x = a;
			}
			else
			{
				result.x = b;
			}
			if (c > d)
			{
				result.y = d;
			}
			else
			{
				result.y = c;
			}
		}
		return result;
	}

	public int findSameNum(int a, int b, int c, int d)
	{
		if (a == c)
		{
			return a;
		}
		if (a == d)
		{
			return a;
		}
		return c;
	}
}
