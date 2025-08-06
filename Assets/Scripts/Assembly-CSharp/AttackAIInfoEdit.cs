using UnityEngine;

[ExecuteInEditMode]
public class AttackAIInfoEdit : MonoBehaviour
{
	public bool runFlag;

	public BlockMapDateNew mapDate;

	private void Start()
	{
	}

	private void Update()
	{
		if (runFlag)
		{
			runFlag = false;
			Execute();
			MonoBehaviour.print("run");
		}
	}

	public void Execute()
	{
	}
}
