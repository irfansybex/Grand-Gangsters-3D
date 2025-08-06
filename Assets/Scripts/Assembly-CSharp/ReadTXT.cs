using UnityEngine;

public class ReadTXT : MonoBehaviour
{
	public string[][] array;

	public void InitArray(string fileName)
	{
		TextAsset textAsset = Resources.Load("TXTDATA/" + fileName) as TextAsset;
		if (textAsset == null)
		{
			MonoBehaviour.print("No File : " + fileName);
		}
		string[] array = textAsset.text.Split("\n"[0]);
		this.array = new string[array.Length][];
		for (int i = 0; i < array.Length; i++)
		{
			this.array[i] = array[i].Split(',');
		}
	}

	public int GetInt(int i, int j)
	{
		return int.Parse(array[i][j]);
	}

	public float GetFloat(int i, int j)
	{
		return float.Parse(array[i][j]);
	}

	public string GetString(int i, int j)
	{
		return array[i][j];
	}

	public void PrintArray(int i, int j)
	{
		MonoBehaviour.print(i + "," + j + " : " + array[i][j]);
	}
}
