public class OneLengthQueue<T>
{
	public T[] list;

	public int length;

	public bool Add(T t)
	{
		if (length < 2)
		{
			list[length] = t;
			length++;
			return true;
		}
		return false;
	}

	public T Get()
	{
		if (length > 0)
		{
			length--;
			T result = list[0];
			if (length > 0)
			{
				list[0] = list[1];
				list[1] = default(T);
			}
			else
			{
				list[0] = default(T);
			}
			return result;
		}
		return default(T);
	}
}
