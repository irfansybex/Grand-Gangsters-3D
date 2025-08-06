using UnityEngine;

public class SlotUIController : MonoBehaviour
{
	public GameObject priseLine;

	public UISprite betLine;

	public float[] betLinePos;

	public float[] priseLinePos;

	public UIEventListener betBtn1;

	public UIEventListener betBtn2;

	public UIEventListener betBtn3;

	public UISprite[] enableBetBtn;

	public UIEventListener playBtn;

	public UIEventListener backBtn;

	public void SetWinPriseLine(int lineIndex)
	{
		priseLine.gameObject.SetActiveRecursively(true);
		priseLine.gameObject.transform.localPosition = new Vector3(betLine.transform.localPosition.x, priseLinePos[lineIndex], 0f);
	}

	public void SetBetLine(int lineIndex)
	{
		betLine.gameObject.transform.localPosition = new Vector3(betLinePos[lineIndex] * GlobalDefine.screenWidthFit, 47.83f, 0f);
		for (int i = 0; i < enableBetBtn.Length; i++)
		{
			enableBetBtn[i].gameObject.SetActiveRecursively(false);
		}
		enableBetBtn[lineIndex].gameObject.SetActiveRecursively(true);
	}
}
