using System;
using System.Collections.Generic;
using UnityEngine;

public class CitySenceController : GameModeController
{
	public static CitySenceController instance;

	public VirtualMapController virtualMapController;

	public PlayerController playerCtl;

	public Vector3 playerPrePos;

	public float npcProduceTime;

	public float countProduceTime;

	public float npcProduceDistance;

	public float npcProduceDistanceInCar;

	public float flashNPCDifSqrDistance;

	public float flashNPCDifSqrDistanceInCar;

	public Vector3 playerPreCarPos;

	public float carProduceTime;

	public float countCarProduceTime;

	public float carProcuceDistance;

	public float carProcuceDistanceInCar;

	public float flashCarDifSqrDistance;

	public float flashCarDifSqrDistanceInCar;

	public float sideWalkMinDis;

	public float sideWalkMaxDis;

	public int policeCarPercent;

	public int policePercent;

	public UIEventListener clickAreaListener;

	public UICamera.MouseOrTouch curTouch;

	public RaycastHit hitRay;

	public LayerMask hitLayer;

	public float intervalExecuteTime;

	private float intervalCount;

	public int POLICELEVEL;

	public bool genPoliceFlag;

	public float genPoliceNum;

	public float alarmSqrDis;

	public List<GenPosInfo> carProduceList = new List<GenPosInfo>();

	public List<GenPosInfo> npcProduceList = new List<GenPosInfo>();

	public bool normalSenceDieFlag = true;

	private bool flag;

	private int ttt;

	public float alarmInterval;

	private RaycastHit hitInfo;

	public int yyy;

	private int range = 70;

	private float attackAIRandomNum;

	private NPCTYPE tempType;

	private int tempLevel;

	public bool genAttackAIFlag;

	public bool genAttackAIInLeftFlag;

	public PoliceCar pol;

	private void Awake()
	{
		UIEventListener uIEventListener = clickAreaListener;
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickArea));
		UIEventListener uIEventListener2 = clickAreaListener;
		uIEventListener2.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onDoubleClick, new UIEventListener.VoidDelegate(OnDoubleClickArea));
		instance = this;
	}

	private void Start()
	{
	}

	public void OnPlayerDie()
	{
		normalSenceDieFlag = true;
		Reset(0);
	}

	public override void Reset(int index)
	{
		if (!(GameController.instance.curMode == GameController.instance.normalMode))
		{
			return;
		}
		BlackScreen.instance.TurnOnScreen();
		Exit();
		if (normalSenceDieFlag)
		{
			normalSenceDieFlag = false;
			PlayerController.instance.transform.position = TaskLabelController.instance.lastInfo.exitPlayerPos;
			PlayerController.instance.transform.eulerAngles = TaskLabelController.instance.lastInfo.exitPlayerAngle;
			PlayerController.instance.moveCtl.playerFaceAngle = PlayerController.instance.transform.eulerAngles.y;
		}
		if (GameController.instance.preMode != GameController.instance.slotMode)
		{
			PlayerController.instance.ResetPlayer();
		}
		GameUIController.instance.InitUI();
		PoliceLevelCtl.score = 0;
		PoliceLevelCtl.level = 0;
		MinimapLightLabelController.instance.EnableLightLabel();
		for (int i = 0; i < MinimapLightLabelController.instance.lightLabel2.Length; i++)
		{
			MinimapLightLabelController.instance.lightLabel2[i].DisableStars();
		}
		if (GameController.instance.preMode != null)
		{
			if (GameController.instance.preMode.mode != GAMEMODE.SLOT)
			{
				CarManage.instance.ResetPlayerCarPos();
			}
		}
		else
		{
			CarManage.instance.ResetPlayerCarPos();
		}
		GameUIController.instance.gameMode = GAMEMODE.NORMAL;
		GameUIController.instance.curTaskInfo = null;
		virtualMapController.CheckMotor();
	}

	public void OnClickArea(GameObject obj)
	{
		curTouch = UICamera.currentTouch;
		if (!Physics.Raycast(Camera.main.ScreenPointToRay(curTouch.pos), out hitRay, 1000f, hitLayer) || hitRay.collider.gameObject.layer != LayerMask.NameToLayer("AI"))
		{
			return;
		}
		AIController component = hitRay.collider.GetComponent<AIController>();
		if (component.dieFlag)
		{
			return;
		}
		if (PlayerController.instance.curState == PLAYERSTATE.HANDGUN)
		{
			if (component != PlayerController.instance.fireTarget)
			{
				PlayerController.instance.fireTarget = component;
			}
			if (!PlayerController.instance.animaCtl.needToAimFlag)
			{
				PlayerController.instance.animaCtl.OnChangeAimState(true);
			}
		}
		else if (PlayerController.instance.curState == PLAYERSTATE.MACHINEGUN)
		{
			if (component != PlayerController.instance.fireTarget)
			{
				PlayerController.instance.fireTarget = component;
			}
			if (!PlayerController.instance.animaCtl.needToAimFlag)
			{
				PlayerController.instance.animaCtl.OnChangeAimState(true);
			}
		}
		else if ((PlayerController.instance.curState == PLAYERSTATE.NORMAL || PlayerController.instance.curState == PLAYERSTATE.FIGHT) && component != PlayerController.instance.fireTarget)
		{
			PlayerController.instance.fireTarget = component;
		}
	}

	public void OnDoubleClickArea(GameObject obj)
	{
		if (PlayerController.instance.curState == PLAYERSTATE.HANDGUN)
		{
			if (PlayerController.instance.animaCtl.needToAimFlag)
			{
				PlayerController.instance.animaCtl.OnChangeAimState(false);
				PlayerController.instance.fireTarget = null;
			}
		}
		else if (PlayerController.instance.curState == PLAYERSTATE.MACHINEGUN)
		{
			if (PlayerController.instance.animaCtl.needToAimFlag)
			{
				PlayerController.instance.animaCtl.OnChangeAimState(false);
				PlayerController.instance.fireTarget = null;
			}
		}
		else if (PlayerController.instance.curState == PLAYERSTATE.NORMAL || PlayerController.instance.curState == PLAYERSTATE.FIGHT)
		{
			PlayerController.instance.animaCtl.fightFlag = false;
			PlayerController.instance.animaCtl.comboFlag = false;
			PlayerController.instance.animaCtl.comboState = FIGHTCOMBOSTATE.NONE;
			PlayerController.instance.fireTarget = null;
		}
	}

	public void AlarmNPC()
	{
		if (GameController.instance.curMode == GameController.instance.survivalMode || !(alarmInterval >= 1f))
		{
			return;
		}
		for (int i = 0; i < NPCPoolController.instance.enableAIList.Count; i++)
		{
			if (!NPCPoolController.instance.enableAIList[i].alarmFlag && (NPCPoolController.instance.enableAIList[i].transform.position - PlayerController.instance.transform.position).sqrMagnitude < alarmSqrDis && !NPCPoolController.instance.enableAIList[i].attackAIFlag)
			{
				NPCPoolController.instance.enableAIList[i].OnAlarm();
			}
		}
		alarmInterval = 0f;
	}

	public override void MyUpdate()
	{
		intervalCount += Time.deltaTime;
		if (PlayerController.instance.fireTarget == null && PlayerController.instance.car == null && intervalCount >= intervalExecuteTime)
		{
			intervalCount = 0f;
			PlayerController.instance.preFireTarget = GetPreFireTarget();
		}
		countProduceTime += Time.deltaTime;
		countCarProduceTime += Time.deltaTime;
		alarmInterval += Time.deltaTime;
		if (genPoliceNum >= (float)POLICELEVEL)
		{
			genPoliceNum = 0f;
			genPoliceFlag = false;
		}
		if (GlobalDefine.smallPhoneFlag)
		{
			if (PlayerController.instance.curState != PLAYERSTATE.CAR)
			{
				if (countProduceTime >= npcProduceTime)
				{
					countProduceTime = 0f;
					GenerateNpc(npcProduceDistance);
					playerPrePos = playerCtl.transform.position;
				}
				else if ((playerCtl.transform.position - playerPrePos).sqrMagnitude > flashNPCDifSqrDistance)
				{
					countProduceTime = 0f;
					GenerateNpc(npcProduceDistance);
					playerPrePos = playerCtl.transform.position;
				}
				if (countCarProduceTime >= carProduceTime)
				{
					countCarProduceTime = 0f;
					GenerateCar(carProcuceDistance);
					playerPreCarPos = playerCtl.transform.position;
				}
				else if ((playerCtl.transform.position - playerPreCarPos).sqrMagnitude > flashCarDifSqrDistance)
				{
					countCarProduceTime = 0f;
					GenerateCar(carProcuceDistance);
					playerPreCarPos = playerCtl.transform.position;
				}
			}
			else
			{
				if (countProduceTime >= npcProduceTime)
				{
					countProduceTime = 0f;
					GenerateNpc(npcProduceDistanceInCar);
					playerPrePos = playerCtl.transform.position;
				}
				else if ((playerCtl.transform.position - playerPrePos).sqrMagnitude > flashNPCDifSqrDistanceInCar)
				{
					countProduceTime = 0f;
					GenerateNpc(npcProduceDistanceInCar);
					playerPrePos = playerCtl.transform.position;
				}
				if (countCarProduceTime >= carProduceTime)
				{
					countCarProduceTime = 0f;
					GenerateCar(carProcuceDistanceInCar);
					playerPreCarPos = playerCtl.transform.position;
				}
				else if ((playerCtl.transform.position - playerPreCarPos).sqrMagnitude > flashCarDifSqrDistanceInCar)
				{
					countCarProduceTime = 0f;
					GenerateCar(carProcuceDistanceInCar);
					playerPreCarPos = playerCtl.transform.position;
				}
			}
		}
		else if (PlayerController.instance.curState != PLAYERSTATE.CAR)
		{
			if (countProduceTime >= npcProduceTime / 2f)
			{
				countProduceTime = 0f;
				GenerateNpc(npcProduceDistance);
				playerPrePos = playerCtl.transform.position;
			}
			else if ((playerCtl.transform.position - playerPrePos).sqrMagnitude > flashNPCDifSqrDistance / 4f)
			{
				countProduceTime = 0f;
				GenerateNpc(npcProduceDistance);
				playerPrePos = playerCtl.transform.position;
			}
			if (countCarProduceTime >= carProduceTime / 2f)
			{
				countCarProduceTime = 0f;
				GenerateCar(carProcuceDistance);
				playerPreCarPos = playerCtl.transform.position;
			}
			else if ((playerCtl.transform.position - playerPreCarPos).sqrMagnitude > flashCarDifSqrDistance / 4f)
			{
				countCarProduceTime = 0f;
				GenerateCar(carProcuceDistance);
				playerPreCarPos = playerCtl.transform.position;
			}
		}
		else
		{
			if (countProduceTime >= npcProduceTime / 2f)
			{
				countProduceTime = 0f;
				GenerateNpc(npcProduceDistanceInCar);
				playerPrePos = playerCtl.transform.position;
			}
			else if ((playerCtl.transform.position - playerPrePos).sqrMagnitude > flashNPCDifSqrDistanceInCar / 4f)
			{
				countProduceTime = 0f;
				GenerateNpc(npcProduceDistanceInCar);
				playerPrePos = playerCtl.transform.position;
			}
			if (countCarProduceTime >= carProduceTime / 2f)
			{
				countCarProduceTime = 0f;
				GenerateCar(carProcuceDistanceInCar);
				playerPreCarPos = playerCtl.transform.position;
			}
			else if ((playerCtl.transform.position - playerPreCarPos).sqrMagnitude > flashCarDifSqrDistanceInCar / 4f)
			{
				countCarProduceTime = 0f;
				GenerateCar(carProcuceDistanceInCar);
				playerPreCarPos = playerCtl.transform.position;
			}
		}
		if (carProduceList.Count != 0 && ttt > 10)
		{
			ttt = 0;
			if (carProduceList[0].polFlag)
			{
				AICarPoolController.instance.GetNormalPoliceCar(carProduceList[0].pos, carProduceList[0].curPos, carProduceList[0].prePos, carProduceList[0].path);
			}
			else
			{
				AICarPoolController.instance.GetCar(carProduceList[0].pos, carProduceList[0].curPos, carProduceList[0].prePos, carProduceList[0].path);
			}
			carProduceList.RemoveAt(0);
		}
		ttt++;
		if (npcProduceList.Count != 0 && yyy > 10)
		{
			yyy = 0;
			if (npcProduceList[0].attackAIFlag)
			{
				NPCPoolController.instance.GetNPC(npcProduceList[0].curPos, npcProduceList[0].prePos, npcProduceList[0].pos, true, npcProduceList[0].type, npcProduceList[0].level);
			}
			else
			{
				NPCPoolController.instance.GetNPC(npcProduceList[0].curPos, npcProduceList[0].prePos, npcProduceList[0].pos);
			}
			npcProduceList.RemoveAt(0);
		}
		yyy++;
	}

	public void AlarmPolice()
	{
		for (int i = 0; i < NPCPoolController.instance.enableAIList.Count; i++)
		{
			if (!NPCPoolController.instance.enableAIList[i].enabled || !NPCPoolController.instance.enableAIList[i].policeFlag)
			{
				continue;
			}
			if (NPCPoolController.instance.enableAIList[i].handGunFlag)
			{
				if (NPCPoolController.instance.enableAIList[i].curState.stateName != AISTATE.HANDGUN)
				{
					NPCPoolController.instance.enableAIList[i].OnAlarm();
				}
			}
			else if (NPCPoolController.instance.enableAIList[i].curState.stateName != AISTATE.MACHINEGUN)
			{
				NPCPoolController.instance.enableAIList[i].OnAlarm();
			}
		}
	}

	public AIController GetPreFireTarget()
	{
		AIController result = null;
		float num = float.PositiveInfinity;
		for (int i = 0; i < NPCPoolController.instance.enableAIList.Count; i++)
		{
			if ((!NPCPoolController.instance.enableAIList[i].policeFlag || NPCPoolController.instance.enableAIList[i].curState.stateName != 0) && !NPCPoolController.instance.enableAIList[i].dieFlag && NPCPoolController.instance.enableAIList[i].visableRander.GetComponent<Renderer>().isVisible)
			{
				float num2 = Vector3.SqrMagnitude(NPCPoolController.instance.enableAIList[i].transform.position - PlayerController.instance.transform.position);
				if (num2 < num && Physics.Raycast(NPCPoolController.instance.enableAIList[i].fireTarget.transform.position, (PlayerController.instance.transform.position - NPCPoolController.instance.enableAIList[i].transform.position).normalized, out hitInfo, 200f) && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
				{
					num = num2;
					result = NPCPoolController.instance.enableAIList[i];
				}
			}
		}
		return result;
	}

	public void GenerateNpc(float genDis)
	{
		virtualMapController.GetPlayerLocation(playerCtl.transform);
		if (!GlobalDefine.smallPhoneFlag)
		{
			if (NPCPoolController.instance.enableAIList.Count >= 15)
			{
				return;
			}
		}
		else if (NPCPoolController.instance.enableAIList.Count >= 5)
		{
			return;
		}
		virtualMapController.GetNpcProducePosByRoad(genDis);
		yyy = 0;
		npcProduceList.Clear();
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 0f);
		int num = UnityEngine.Random.Range(0, virtualMapController.npcProducePos.Count);
		for (int i = 0; i < virtualMapController.npcProducePos.Count; i++)
		{
			attackAIRandomNum = UnityEngine.Random.Range(0, 100);
			if (attackAIRandomNum < (float)NPCGenerateInfoList.instance.infoList[virtualMapController.curBlock.aiRateIndex].sumAttackAIRate)
			{
				genAttackAIFlag = true;
				for (int j = 0; j < NPCGenerateInfoList.instance.infoList[virtualMapController.curBlock.aiRateIndex].rate.Length; j++)
				{
					if (attackAIRandomNum < (float)NPCGenerateInfoList.instance.infoList[virtualMapController.curBlock.aiRateIndex].rate[j])
					{
						tempType = NPCGenerateInfoList.instance.infoList[virtualMapController.curBlock.aiRateIndex].aiInfo[j].typeIndex;
						tempLevel = GlobalInf.gameLevel;
						break;
					}
				}
				if (UnityEngine.Random.Range(0, 100) > 50)
				{
					genAttackAIInLeftFlag = true;
				}
				else
				{
					genAttackAIInLeftFlag = false;
				}
			}
			else
			{
				genAttackAIFlag = false;
			}
			int index = (i + num) % virtualMapController.npcProducePos.Count;
			Vector3 normalized = (virtualMapController.npcProducePos[index].point2.position - virtualMapController.npcProducePos[index].point1.position).normalized;
			vector = virtualMapController.npcProducePos[index].point1.position + normalized * virtualMapController.npcProducePos[index].distanceFromPoint1 + virtualMapController.npcProducePos[index].point1.right * UnityEngine.Random.Range(virtualMapController.npcProducePos[index].point1.roadInfo.minSideWalkDis, virtualMapController.npcProducePos[index].point1.roadInfo.maxSideWalkDis);
			vector2 = virtualMapController.npcProducePos[index].point1.position + normalized * virtualMapController.npcProducePos[index].distanceFromPoint1 - virtualMapController.npcProducePos[index].point1.right * UnityEngine.Random.Range(virtualMapController.npcProducePos[index].point1.roadInfo.minSideWalkDis, virtualMapController.npcProducePos[index].point1.roadInfo.maxSideWalkDis);
			if (PlayerController.instance.transform.InverseTransformPoint(vector).z > 0f && UnityEngine.Random.Range(0, 100) < range && Vector3.SqrMagnitude(vector - playerCtl.transform.position) < NPCPoolController.instance.NPCRecycleDIstance)
			{
				if (!genAttackAIFlag)
				{
					if (UnityEngine.Random.Range(0, 2) > 0)
					{
						npcProduceList.Add(new GenPosInfo(vector, virtualMapController.npcProducePos[index].point1, virtualMapController.npcProducePos[index].point2, 0, false, false, NPCTYPE.GANSTARWHITE_PUNCH, 0));
					}
					else
					{
						npcProduceList.Add(new GenPosInfo(vector, virtualMapController.npcProducePos[index].point2, virtualMapController.npcProducePos[index].point1, 0, false, false, NPCTYPE.GANSTARWHITE_PUNCH, 0));
					}
				}
				else if (genAttackAIInLeftFlag)
				{
					genAttackAIFlag = false;
					if (UnityEngine.Random.Range(0, 2) > 0)
					{
						npcProduceList.Add(new GenPosInfo(vector, virtualMapController.npcProducePos[index].point1, virtualMapController.npcProducePos[index].point2, 0, true, false, tempType, tempLevel));
					}
					else
					{
						npcProduceList.Add(new GenPosInfo(vector, virtualMapController.npcProducePos[index].point2, virtualMapController.npcProducePos[index].point1, 0, true, false, tempType, tempLevel));
					}
				}
			}
			if (PlayerController.instance.transform.InverseTransformPoint(vector2).z > 0f && UnityEngine.Random.Range(0, 100) < range && Vector3.SqrMagnitude(vector2 - playerCtl.transform.position) < NPCPoolController.instance.NPCRecycleDIstance)
			{
				if (!genAttackAIFlag)
				{
					if (UnityEngine.Random.Range(0, 2) > 0)
					{
						npcProduceList.Add(new GenPosInfo(vector2, virtualMapController.npcProducePos[index].point1, virtualMapController.npcProducePos[index].point2, 0, false, false, NPCTYPE.GANSTARWHITE_PUNCH, 0));
					}
					else
					{
						npcProduceList.Add(new GenPosInfo(vector2, virtualMapController.npcProducePos[index].point2, virtualMapController.npcProducePos[index].point1, 0, false, false, NPCTYPE.GANSTARWHITE_PUNCH, 0));
					}
				}
				else if (!genAttackAIInLeftFlag)
				{
					genAttackAIFlag = false;
					if (UnityEngine.Random.Range(0, 2) > 0)
					{
						npcProduceList.Add(new GenPosInfo(vector2, virtualMapController.npcProducePos[index].point1, virtualMapController.npcProducePos[index].point2, 0, true, false, tempType, tempLevel));
					}
					else
					{
						npcProduceList.Add(new GenPosInfo(vector2, virtualMapController.npcProducePos[index].point2, virtualMapController.npcProducePos[index].point1, 0, true, false, tempType, tempLevel));
					}
				}
			}
			if (!GlobalDefine.smallPhoneFlag)
			{
				if (NPCPoolController.instance.enableAIList.Count + npcProduceList.Count >= 15)
				{
					break;
				}
			}
			else if (NPCPoolController.instance.enableAIList.Count + npcProduceList.Count >= 5)
			{
				break;
			}
		}
	}

	public bool CheckEmpty(Vector3 pos)
	{
		for (int i = 0; i < NPCPoolController.instance.enableAIList.Count; i++)
		{
			if ((NPCPoolController.instance.enableAIList[i].transform.position - pos).sqrMagnitude < 30f)
			{
				return false;
			}
		}
		return true;
	}

	public void GenerateCar(float genDis)
	{
		virtualMapController.GetPlayerLocation(playerCtl.transform);
		if (GlobalDefine.smallPhoneFlag)
		{
			if (AICarPoolController.instance.enableList.Count >= 8)
			{
				return;
			}
		}
		else if (AICarPoolController.instance.enableList.Count >= 15)
		{
			return;
		}
		virtualMapController.GetNpcProducePosByRoad(genDis);
		carProduceList.Clear();
		ttt = 0;
		Vector3 vector = new Vector3(0f, 0f, 0f);
		int count = virtualMapController.npcProducePos.Count;
		int num = UnityEngine.Random.Range(0, count);
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			int index = (i + num) % count;
			Vector3 normalized = (virtualMapController.npcProducePos[index].point2.position - virtualMapController.npcProducePos[index].point1.position).normalized;
			vector = virtualMapController.npcProducePos[index].point1.position + normalized * virtualMapController.npcProducePos[index].distanceFromPoint1;
			if (Vector3.SqrMagnitude(vector - playerCtl.transform.position) < AICarPoolController.instance.carRecycleSqrDis)
			{
				if (flag && !PoliceLevelCtl.instance.policeChaseFlag)
				{
					flag = false;
					bool flag2 = false;
					for (int j = 0; j < AICarPoolController.instance.enableList.Count; j++)
					{
						if (AICarPoolController.instance.enableList[j].policeFlag)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						if (!virtualMapController.npcProducePos[index].point1.roadInfo.arrowRoad)
						{
							carProduceList.Add(new GenPosInfo(vector, virtualMapController.npcProducePos[index].point1, virtualMapController.npcProducePos[index].point2, UnityEngine.Random.Range(0, 2), false, true, NPCTYPE.GANSTARWHITE_PUNCH, 0));
						}
						else
						{
							carProduceList.Add(new GenPosInfo(vector, virtualMapController.npcProducePos[index].point1, virtualMapController.npcProducePos[index].point2, 0, false, true, NPCTYPE.GANSTARWHITE_PUNCH, 0));
						}
					}
				}
				else if (!virtualMapController.npcProducePos[index].point1.roadInfo.arrowRoad)
				{
					carProduceList.Add(new GenPosInfo(vector, virtualMapController.npcProducePos[index].point1, virtualMapController.npcProducePos[index].point2, UnityEngine.Random.Range(0, 2), false, false, NPCTYPE.GANSTARWHITE_PUNCH, 0));
				}
				else
				{
					carProduceList.Add(new GenPosInfo(vector, virtualMapController.npcProducePos[index].point1, virtualMapController.npcProducePos[index].point2, 0, false, false, NPCTYPE.GANSTARWHITE_PUNCH, 0));
				}
				if (!virtualMapController.npcProducePos[index].point2.roadInfo.arrowRoad)
				{
					carProduceList.Add(new GenPosInfo(vector, virtualMapController.npcProducePos[index].point2, virtualMapController.npcProducePos[index].point1, UnityEngine.Random.Range(0, 2), false, false, NPCTYPE.GANSTARWHITE_PUNCH, 0));
				}
				else
				{
					carProduceList.Add(new GenPosInfo(vector, virtualMapController.npcProducePos[index].point2, virtualMapController.npcProducePos[index].point1, 0, false, false, NPCTYPE.GANSTARWHITE_PUNCH, 0));
				}
			}
			if (!GlobalDefine.smallPhoneFlag)
			{
				if (AICarPoolController.instance.enableList.Count + carProduceList.Count >= 15)
				{
					break;
				}
			}
			else if (AICarPoolController.instance.enableList.Count + carProduceList.Count >= 5)
			{
				break;
			}
		}
	}

	public void GeneratePolice(float genDis)
	{
		if (GlobalInf.firstOpenGameFlag)
		{
			return;
		}
		virtualMapController.GetPlayerLocation(playerCtl.transform);
		virtualMapController.GetNpcProducePosByRoad(genDis);
		Vector3 vector = new Vector3(0f, 0f, 0f);
		int count = virtualMapController.npcProducePos.Count;
		int num = UnityEngine.Random.Range(0, count);
		int num2 = count;
		if (AICarPoolController.instance.policeCarCount + count > PoliceLevelCtl.instance.policeCarNum[PoliceLevelCtl.level])
		{
			num2 = PoliceLevelCtl.instance.policeCarNum[PoliceLevelCtl.level] - AICarPoolController.instance.policeCarCount;
		}
		for (int i = 0; i < num2; i++)
		{
			int index = (i + num) % count;
			Vector3 normalized = (virtualMapController.npcProducePos[index].point2.position - virtualMapController.npcProducePos[index].point1.position).normalized;
			vector = virtualMapController.npcProducePos[index].point1.position + normalized * virtualMapController.npcProducePos[index].distanceFromPoint1;
			if (Vector3.SqrMagnitude(vector - playerCtl.transform.position) < AICarPoolController.instance.policeRecycleSqrDis && CheckCarEmpty(vector))
			{
				AICarPoolController.instance.GetChasingCar(vector, virtualMapController.npcProducePos[index].point1, virtualMapController.npcProducePos[index].point2);
				genPoliceNum += 1f;
			}
		}
	}

	public void GenerateBlockPoliceCar(float genDis)
	{
		virtualMapController.GetPlayerLocation(playerCtl.transform);
		virtualMapController.GetNpcProducePosByRoad(genDis);
		Vector3 vector = new Vector3(0f, 0f, 0f);
		int count = virtualMapController.npcProducePos.Count;
		int num = count;
		if (AICarPoolController.instance.policeCarCount + count > PoliceLevelCtl.instance.policeCarNum[PoliceLevelCtl.level])
		{
			num = PoliceLevelCtl.instance.policeCarNum[PoliceLevelCtl.level] - AICarPoolController.instance.policeCarCount;
		}
		for (int i = 0; i < num; i++)
		{
			if (!ToolFunction.isForward(virtualMapController.npcProducePos[i].point1.position - PlayerController.instance.transform.position, PlayerController.instance.transform.forward))
			{
				continue;
			}
			Vector3 normalized = (virtualMapController.npcProducePos[i].point2.position - virtualMapController.npcProducePos[i].point1.position).normalized;
			vector = virtualMapController.npcProducePos[i].point1.position + normalized * virtualMapController.npcProducePos[i].distanceFromPoint1;
			if (Vector3.SqrMagnitude(vector - playerCtl.transform.position) < AICarPoolController.instance.policeRecycleSqrDis && CheckCarEmpty(vector))
			{
				if (UnityEngine.Random.Range(0, 2) > 0)
				{
					AICarPoolController.instance.GetBlockPoliceCar(vector, virtualMapController.npcProducePos[i].point1.right);
				}
				else
				{
					AICarPoolController.instance.GetBlockPoliceCar(vector, virtualMapController.npcProducePos[i].point1.right * -1f);
				}
				genPoliceNum += 1f;
			}
		}
	}

	public static bool CheckPoliceAside(Vector3 pos, float sqrDis)
	{
		for (int i = 0; i < NPCPoolController.instance.enableAIList.Count; i++)
		{
			if (NPCPoolController.instance.enableAIList[i].policeFlag && Vector3.SqrMagnitude(pos - NPCPoolController.instance.enableAIList[i].transform.position) < sqrDis)
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckCarEmpty(Vector3 pos)
	{
		for (int i = 0; i < AICarPoolController.instance.enableList.Count; i++)
		{
			if ((AICarPoolController.instance.enableList[i].transform.position - pos).sqrMagnitude < 100f)
			{
				return false;
			}
		}
		return true;
	}

	public override void Exit()
	{
		base.Exit();
		PlayerController.instance.PlayerGetOffCar();
		ClearAI();
		PoliceLevelCtl.ResetPoliceLevel();
		MinimapLightLabelController.instance.DisableLightLabel();
	}

	public void ClearAI()
	{
		carProduceList.Clear();
		npcProduceList.Clear();
		while (AICarPoolController.instance.enableList.Count > 0)
		{
			AICarPoolController.instance.recylecar(AICarPoolController.instance.enableList[0]);
		}
		while (NPCPoolController.instance.enableAIList.Count > 0)
		{
			if (NPCPoolController.instance.enableAIList[0].machineGunState.machineGun != null)
			{
				NPCPoolController.instance.enableAIList[0].machineGunState.machineGun.transform.parent = GunPoolList.instance.machineGunPool.transform;
				NPCPoolController.instance.enableAIList[0].machineGunState.machineGun.transform.localPosition = Vector3.zero;
				GunPoolList.instance.machineGunPool.Recycle(NPCPoolController.instance.enableAIList[0].machineGunState.machineGun);
				NPCPoolController.instance.enableAIList[0].machineGunState.machineGun = null;
			}
			if (NPCPoolController.instance.enableAIList[0].handGunState.handGun != null)
			{
				NPCPoolController.instance.enableAIList[0].handGunState.handGun.transform.parent = GunPoolList.instance.handGunPool.transform;
				NPCPoolController.instance.enableAIList[0].handGunState.handGun.transform.localPosition = Vector3.zero;
				GunPoolList.instance.handGunPool.Recycle(NPCPoolController.instance.enableAIList[0].handGunState.handGun);
				NPCPoolController.instance.enableAIList[0].handGunState.handGun = null;
			}
			NPCPoolController.instance.RecycleAI(NPCPoolController.instance.enableAIList[0]);
		}
		FallingObjPool.instance.ClearFallingObj();
	}
}
