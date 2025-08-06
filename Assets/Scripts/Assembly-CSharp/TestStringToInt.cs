using UnityEngine;

[ExecuteInEditMode]
public class TestStringToInt : MonoBehaviour
{
	public bool runflag;

	public GameObject rootObj;

	private void Start()
	{
	}

	private void Update()
	{
		if (runflag)
		{
			runflag = false;
			for (int i = 0; i < rootObj.transform.childCount; i++)
			{
				string text = rootObj.transform.GetChild(i).name;
				MonoBehaviour.print(text);
				text = text.Remove(0, 8);
				MonoBehaviour.print(text);
				int num = int.Parse(text);
				MonoBehaviour.print(string.Empty + num);
			}
		}
	}
}
