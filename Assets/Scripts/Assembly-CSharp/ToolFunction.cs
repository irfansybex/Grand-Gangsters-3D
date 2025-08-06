using UnityEngine;

public class ToolFunction : MonoBehaviour
{
	public static float SqrDistance(Vector3 s, Vector3 d)
	{
		return (s - d).sqrMagnitude;
	}

	public static bool isForward(Vector3 dirA, Vector3 dirB)
	{
		if (Vector3.Dot(dirA, dirB) > 0f)
		{
			return true;
		}
		return false;
	}

	public static Vector2 InverseXZ(Vector3 sourcePosition, Vector3 targetPosition, Vector3 sourceForward, Vector3 sourceRight)
	{
		return new Vector2(Vector3.Project(targetPosition - sourcePosition, sourceRight).magnitude, Vector3.Project(targetPosition - sourcePosition, sourceForward).magnitude);
	}

	public static float InverseZ(Vector3 sourcePosition, Vector3 targetPosition, Vector3 sourceForward)
	{
		return Vector3.Project(targetPosition - sourcePosition, sourceForward).magnitude;
	}

	public static float InverseX(Vector3 sourcePosition, Vector3 targetPosition, Vector3 sourceRight)
	{
		return Vector3.Project(targetPosition - sourcePosition, sourceRight).magnitude;
	}

	public static Vector3 NGUIFitScreenPos(Vector3 sourcePos)
	{
		return new Vector3(sourcePos.x * GlobalDefine.screenRatioWidth / 800f, sourcePos.y, 0f);
	}

	public static Vector2 ChangeToMapPos(Vector3 sourcePos)
	{
		float x = (sourcePos.x - (float)VirtualminiMapController.instance.blockMap.startX + 200f) / 5f + 1000f;
		float y = (sourcePos.z - (float)VirtualminiMapController.instance.blockMap.startY + 100f) / 5f;
		return new Vector2(x, y);
	}

	public static Vector3 ChangeSecondToTime(int second)
	{
		int num = second / 3600;
		int num2 = (second - num * 3600) / 60;
		int num3 = second % 60;
		return new Vector3(num, num2, num3);
	}
}
