using UnityEngine;

public class WheelCheckCollider : MonoBehaviour
{
	public delegate void OnChangeCheckStyle(bool style);

	public bool isGrounded;

	public Transform floorTra;

	public Transform tempFloorTra;

	public Transform tempTra;

	public bool towFloorFlag;

	public OnChangeCheckStyle onChangeCheckStyle;

	public bool enableFlag = true;

	private float floorDis;

	private float tempFloorDis;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("changeStyle"))
		{
			enableFlag = !enableFlag;
			if (onChangeCheckStyle != null)
			{
				onChangeCheckStyle(!enableFlag);
			}
		}
		if (other.gameObject.layer == LayerMask.NameToLayer("Floor") && enableFlag)
		{
			isGrounded = true;
			if (floorTra == null)
			{
				floorTra = other.transform;
				return;
			}
			tempFloorTra = other.transform;
			towFloorFlag = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Floor") && enableFlag)
		{
			if (other.transform == floorTra)
			{
				isGrounded = false;
				floorTra = null;
			}
			if (other.transform == tempFloorTra)
			{
				tempFloorTra = null;
				towFloorFlag = false;
			}
		}
	}

	private void Update()
	{
		if (!enableFlag || !towFloorFlag)
		{
			return;
		}
		if (floorTra == null)
		{
			floorTra = tempFloorTra;
			return;
		}
		floorDis = WheelColliderSource.ComputeDistance(base.transform.parent, floorTra.transform);
		tempFloorDis = WheelColliderSource.ComputeDistance(base.transform.parent, tempFloorTra.transform);
		if (floorDis > tempFloorDis)
		{
			tempTra = floorTra;
			floorTra = tempFloorTra;
			tempFloorTra = tempTra;
		}
	}
}
