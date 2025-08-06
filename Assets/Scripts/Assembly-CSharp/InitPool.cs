using UnityEngine;

[ExecuteInEditMode]
public class InitPool : MonoBehaviour
{
	public bool runFlag;

	public NPCPoolController npcPool;

	public AICarPoolController carPool;

	public RagDollPool ragdollPool;

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			InitNPC();
			InitAICar();
		}
	}

	public void InitNPC()
	{
		npcPool.disableAIList.Clear();
		for (int i = 0; i < npcPool.transform.childCount; i++)
		{
			AIController component = npcPool.transform.GetChild(i).GetComponent<AIController>();
			if (!component.policeFlag && !npcPool.disableAIList.Contains(component))
			{
				npcPool.disableAIList.Add(component);
			}
		}
	}

	public void InitAICar()
	{
	}
}
