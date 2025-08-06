using UnityEngine;

public class PlayerCamTargetFollow : MonoBehaviour
{
	public Transform player;

	public Transform camPos;

	private Transform m_transform;

	private void Awake()
	{
		m_transform = base.transform;
	}

	private void Update()
	{
		m_transform.position = player.transform.position + Vector3.up * 1.5f;
	}
}
