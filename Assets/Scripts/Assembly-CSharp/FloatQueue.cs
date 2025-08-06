public class FloatQueue
{
	private int length = 32;

	private float[] values;

	private float[] deltaTimes;

	private int headIndex;

	private int tailIndex;

	private float defaultValue;

	private float preAll;

	private float preAllTimes;

	private float smoothTime = 1f;

	public float smoothValue;

	public float realValue;

	public FloatQueue(int _length)
	{
		length = _length;
		values = new float[length];
		deltaTimes = new float[length];
	}

	public FloatQueue(int _length, float _smoothTime)
	{
		length = _length;
		values = new float[length];
		deltaTimes = new float[length];
		smoothTime = _smoothTime;
	}

	public void setSmoothTime(float _smoothTime)
	{
		smoothTime = _smoothTime;
	}

	public float getSmoothTime()
	{
		return smoothTime;
	}

	public void reset()
	{
		headIndex = 0;
		tailIndex = 0;
		for (int i = 0; i < length; i++)
		{
			values[i] = defaultValue;
			deltaTimes[i] = 0f;
		}
		smoothValue = defaultValue;
		preAll = defaultValue;
		preAllTimes = 0f;
	}

	private int nextIndex(int index)
	{
		return (index + 1) % length;
	}

	private int lastIndex(int index)
	{
		return (index - 1 + length) % length;
	}

	public void setSmoothVaule(float v)
	{
		int bufferLength = getBufferLength();
		smoothValue = v;
		preAll = (float)bufferLength * v;
		for (int num = tailIndex; num != headIndex; num = nextIndex(num))
		{
			values[num] = v;
		}
	}

	private int getBufferLength()
	{
		int num = headIndex - tailIndex;
		if (tailIndex > headIndex)
		{
			num = headIndex + length - tailIndex;
		}
		if (num < 1)
		{
			num = 1;
		}
		return num;
	}

	public void update(float _value, float _deltaTime)
	{
		preAll += _value;
		preAllTimes += _deltaTime;
		values[headIndex] = _value;
		deltaTimes[headIndex] = _deltaTime;
		headIndex = nextIndex(headIndex);
		if (headIndex == tailIndex)
		{
			preAll -= values[tailIndex];
			preAllTimes -= deltaTimes[tailIndex];
			tailIndex = nextIndex(tailIndex);
		}
		while (preAllTimes > smoothTime)
		{
			int num = nextIndex(tailIndex);
			if (num == headIndex)
			{
				break;
			}
			preAllTimes -= deltaTimes[tailIndex];
			preAll -= values[tailIndex];
			tailIndex = num;
		}
		int bufferLength = getBufferLength();
		smoothValue = preAll / (float)bufferLength;
		realValue = _value;
	}
}
