using UnityEngine;

public class PlayerMovementCtl : MonoBehaviour
{
	public MyNGUIJoyStick joyStick;

	public GameObject camTarget;

	public GameObject cam;

	public PlayerAnimationCtl animaCtl;

	public PlayerController playerCtl;

	public Transform speedDirection;

	public float playerFaceAngle;

	public float tempX;

	public float tempY = 1f;

	public float walkingSpeed = 2f;

	public float runningSpeed = 6f;

	public float aimingSpeed = 3f;

	public float walkingSnappyness = 50f;

	public float aimMoveSpeed = 3f;

	public Vector3 movingDirection;

	public Vector3 targetVelocity;

	public Vector3 deltaVelocity;

	public bool isGrounded = true;

	private float speedDownVale;

	private Vector3 tempDir;

	private Vector3 camTargetPosXZ;

	private Vector3 camPosXZ;

	private Rigidbody m_rigidbody;

	private Transform m_transform;

	public Transform moveTarget;

	public bool moveToTargetFlag;

	public bool disableMoveFlag;

	private void Awake()
	{
		m_rigidbody = base.GetComponent<Rigidbody>();
		m_transform = base.transform;
	}

	private void Update()
	{
		if (disableMoveFlag)
		{
			return;
		}
		if (!moveToTargetFlag)
		{
			tempX = Mathf.Lerp(tempX, joyStick.position.x, Time.deltaTime * 5f);
			tempY = Mathf.Lerp(tempY, joyStick.position.y, Time.deltaTime * 5f);
			if (animaCtl.fightFlag || playerCtl.punchedFlag || playerCtl.curState == PLAYERSTATE.DIE || playerCtl.curState == PLAYERSTATE.CAR)
			{
				tempX = 0f;
				tempY = 0f;
			}
			tempDir = camTarget.transform.right * tempX + camTarget.transform.forward * tempY;
			if (tempDir.sqrMagnitude > 1f)
			{
				tempDir.Normalize();
			}
			movingDirection = tempDir;
		}
		else
		{
			Vector3 vector = moveTarget.position - m_transform.position;
			movingDirection = new Vector3(vector.x, 0f, vector.z);
			if (movingDirection.sqrMagnitude < 0.1f)
			{
				moveToTargetFlag = false;
				playerCtl.OnGetOnCarMoveDone();
			}
			else if (movingDirection.sqrMagnitude > 1f)
			{
				movingDirection.Normalize();
			}
		}
	}

	public void MoveToTarget(Transform pos)
	{
		moveTarget = pos;
		moveToTargetFlag = true;
	}

	private void FixedUpdate()
	{
		if (Mathf.Abs(tempX) < 0.005f && Mathf.Abs(tempY) < 0.005f && base.GetComponent<Rigidbody>().velocity.y < 0f)
		{
			base.GetComponent<Rigidbody>().AddForce(base.GetComponent<Rigidbody>().mass * Physics.gravity.y * m_transform.up * -1f);
		}
		if (!animaCtl.needToAimFlag)
		{
			if (animaCtl.curState == PLAYERSTATE.FIGHT)
			{
				targetVelocity = movingDirection * movingDirection.magnitude * walkingSpeed;
			}
			else
			{
				targetVelocity = movingDirection * movingDirection.magnitude * runningSpeed;
			}
		}
		else
		{
			targetVelocity = movingDirection * movingDirection.magnitude * aimingSpeed;
		}
		deltaVelocity = targetVelocity - m_rigidbody.velocity;
		if (m_rigidbody.useGravity)
		{
			deltaVelocity.y = 0f;
		}
		m_rigidbody.AddForce(deltaVelocity * walkingSnappyness, ForceMode.Acceleration);
		m_rigidbody.angularVelocity = Vector3.zero;
		if (!animaCtl.needToAimFlag && movingDirection.sqrMagnitude > 0.01f)
		{
			speedDirection.forward = new Vector3(movingDirection.x, 0f, movingDirection.z);
			playerFaceAngle = Mathf.LerpAngle(playerFaceAngle, speedDirection.eulerAngles.y, Time.fixedDeltaTime * 5f);
			m_transform.eulerAngles = new Vector3(0f, playerFaceAngle, 0f);
		}
		if (animaCtl.needToAimFlag)
		{
			playerFaceAngle = Mathf.LerpAngle(playerFaceAngle, m_transform.rotation.eulerAngles.y + animaCtl.difAngle, Time.fixedDeltaTime * 5f);
			m_transform.eulerAngles = new Vector3(0f, playerFaceAngle, 0f);
		}
		if (animaCtl.fightFlag && animaCtl.playerCtl.fireTarget != null)
		{
			playerFaceAngle = Mathf.LerpAngle(playerFaceAngle, m_transform.rotation.eulerAngles.y + animaCtl.difAngle, Time.fixedDeltaTime * 5f);
			m_transform.eulerAngles = new Vector3(0f, playerFaceAngle, 0f);
		}
	}
}
