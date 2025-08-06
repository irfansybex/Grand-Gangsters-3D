using System.Collections.Generic;
using UnityEngine;

public class LightSystem : MonoBehaviour
{
	public float redtime;

	public float yellowtime;

	public float greentime;

	public float time;

	public List<LightPoint> lightpoint;

	protected int i;

	protected int j;

	protected int l;

	private void Start()
	{
	}

	private void Update()
	{
		time += Time.deltaTime;
		if (time < redtime && time > 0f)
		{
			changestoptrue(0);
			changestoptrue(2);
			changestopfalse(1);
			changestopfalse(3);
			changedriveturn(1);
			changedriveturn(3);
		}
		else if (redtime < time && time < redtime + yellowtime)
		{
			changestoptrue(0);
			changestoptrue(1);
			changestoptrue(3);
			changestoptrue(2);
		}
		else if (redtime + yellowtime < time && time < redtime + yellowtime + greentime)
		{
			changestoptrue(1);
			changestoptrue(3);
			changestopfalse(2);
			changestopfalse(0);
			changedrivestraight(0);
			changedrivestraight(2);
		}
		else if (time > redtime + yellowtime + greentime && time < redtime + yellowtime + greentime + redtime)
		{
			changestoptrue(0);
			changestoptrue(2);
			changestopfalse(1);
			changestopfalse(3);
			changedrivestraight(1);
			changedrivestraight(3);
		}
		else if (time > redtime + yellowtime + greentime + redtime && time < redtime + yellowtime + greentime + redtime + yellowtime)
		{
			changestoptrue(0);
			changestoptrue(1);
			changestoptrue(3);
			changestoptrue(2);
		}
		else if (time > redtime + yellowtime + greentime + redtime + yellowtime && time < redtime + yellowtime + greentime + redtime + yellowtime + greentime)
		{
			changestoptrue(1);
			changestoptrue(3);
			changestopfalse(2);
			changestopfalse(0);
			changedriveturn(0);
			changedriveturn(2);
		}
		else if (time > redtime + yellowtime + greentime + redtime + yellowtime + greentime)
		{
			time = 0f;
		}
	}

	public void changestoptrue(int m)
	{
		for (i = 0; i < lightpoint.Count; i++)
		{
			for (l = 0; l < lightpoint[i].dummyCrossPoint.Count; l++)
			{
				if (lightpoint[i].dummyCrossPoint[m] != null)
				{
					lightpoint[i].dummyCrossPoint[m].stop = true;
				}
			}
		}
	}

	public void changestopfalse(int n)
	{
		for (i = 0; i < lightpoint.Count; i++)
		{
			for (l = 0; l < lightpoint[i].dummyCrossPoint.Count; l++)
			{
				if (lightpoint[i].dummyCrossPoint[n] != null)
				{
					lightpoint[i].dummyCrossPoint[n].stop = false;
				}
			}
		}
	}

	public void changedrivestraight(int o)
	{
		for (i = 0; i < lightpoint.Count; i++)
		{
			for (j = 0; j < lightpoint[i].CrossPoint.Count; j++)
			{
				if (lightpoint[i].CrossPoint[o] != null)
				{
					lightpoint[i].CrossPoint[o].Drive_State = drive_state.straight;
				}
			}
		}
	}

	public void changedriveturn(int k)
	{
		for (i = 0; i < lightpoint.Count; i++)
		{
			for (j = 0; j < lightpoint[i].CrossPoint.Count; j++)
			{
				if (lightpoint[i].CrossPoint[k] != null)
				{
					lightpoint[i].CrossPoint[k].Drive_State = drive_state.turn;
				}
			}
		}
	}
}
