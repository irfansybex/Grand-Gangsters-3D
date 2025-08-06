using System.Collections.Generic;

public class MyStack<T>
{
	public List<T> data = new List<T>();

	public int Count;

	public void Push(T temp)
	{
		data.Add(temp);
		Count++;
	}

	public T Pop()
	{
		if (Count > 0)
		{
			T result = data[data.Count - 1];
			data.RemoveAt(data.Count - 1);
			Count--;
			return result;
		}
		return default(T);
	}

	public T GetAt(int num)
	{
		return data[num];
	}
}
