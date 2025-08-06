using UnityEngine;

public class MaterialPool : MonoBehaviour
{
	public static MaterialPool instance;

	public Material normalWhiteAppear;

	public Material normalBlackAppear;

	public Material normalWomenAppear;

	public Material ganstarWhiteAppear;

	public Material ganstarBlackAppear;

	public Material ganstarNakedAppear;

	public Material police1Appear;

	public Material police2Appear;

	public Material normalWhite2Appear;

	public Material normalWomen2Appear;

	public Material[] normalWhiteMatList;

	public Material[] normalWomenMatList;

	public Material[] normalBlackMatList;

	public Material[] ganstarWhiteMatList;

	public Material[] ganstarBlackMatList;

	public Material[] ganstarNakedMatList;

	public Material[] police1MatList;

	public Material[] police2MatList;

	public Material[] normalWhite2MatList;

	public Material[] normalWomen2MatList;

	public int curNormalWhiteMatNum;

	public int curNormalWomenMatNum;

	public int curNormalBlackMatNum;

	public int curGanstarWhiteMatNum;

	public int curGanstarBlackMatNum;

	public int curGanstarNakedMatNum;

	public int curPolice1MatNum;

	public int curPolice2MatNum;

	public int curNormalWhite2MatNum;

	public int curNormalWomen2MatNum;

	private Material tempMat;

	private void Awake()
	{
		instance = this;
		normalWhiteMatList = new Material[3];
		normalWomenMatList = new Material[3];
		normalBlackMatList = new Material[3];
		ganstarWhiteMatList = new Material[3];
		ganstarBlackMatList = new Material[3];
		ganstarNakedMatList = new Material[3];
		police1MatList = new Material[3];
		police2MatList = new Material[3];
		normalWhite2MatList = new Material[3];
		normalWomen2MatList = new Material[3];
		for (int i = 0; i < 3; i++)
		{
			normalWhiteMatList[i] = Object.Instantiate(normalWhiteAppear) as Material;
			normalWomenMatList[i] = Object.Instantiate(normalWomenAppear) as Material;
			normalBlackMatList[i] = Object.Instantiate(normalBlackAppear) as Material;
			ganstarWhiteMatList[i] = Object.Instantiate(ganstarWhiteAppear) as Material;
			ganstarBlackMatList[i] = Object.Instantiate(ganstarBlackAppear) as Material;
			ganstarNakedMatList[i] = Object.Instantiate(ganstarNakedAppear) as Material;
			police1MatList[i] = Object.Instantiate(police1Appear) as Material;
			police2MatList[i] = Object.Instantiate(police2Appear) as Material;
			normalWhite2MatList[i] = Object.Instantiate(normalWhite2Appear) as Material;
			normalWomen2MatList[i] = Object.Instantiate(normalWomen2Appear) as Material;
		}
	}

	public Material GetNpcMaterial(NPCTYPE type)
	{
		switch (type)
		{
		case NPCTYPE.GANSTARBLACK_PUNCH:
		case NPCTYPE.GANSTARBLACK_HG:
		case NPCTYPE.GANSTARBLACK_MG:
			curGanstarBlackMatNum = (curGanstarBlackMatNum + 1) % 3;
			return GetRightMaterial(ganstarBlackMatList, curGanstarBlackMatNum);
		case NPCTYPE.GANSTARNAKED_PUNCH:
		case NPCTYPE.GANSTARNAKED_HG:
		case NPCTYPE.GANSTARNAKED_MG:
			curGanstarNakedMatNum = (curGanstarNakedMatNum + 1) % 3;
			return GetRightMaterial(ganstarNakedMatList, curGanstarNakedMatNum);
		case NPCTYPE.GANSTARWHITE_PUNCH:
		case NPCTYPE.GANSTARWHITE_HG:
			curGanstarWhiteMatNum = (curGanstarWhiteMatNum + 1) % 3;
			return GetRightMaterial(ganstarWhiteMatList, curGanstarWhiteMatNum);
		case NPCTYPE.NORMALBLACK_PUNCH:
		case NPCTYPE.NORMALBLACK_HG:
			curNormalBlackMatNum = (curNormalBlackMatNum + 1) % 3;
			return GetRightMaterial(normalBlackMatList, curNormalBlackMatNum);
		case NPCTYPE.NORMALWHITE:
			curNormalWhiteMatNum = (curNormalWhiteMatNum + 1) % 3;
			return GetRightMaterial(normalWhiteMatList, curNormalWhiteMatNum);
		case NPCTYPE.NORMALWOMEN:
			curNormalWomenMatNum = (curNormalWomenMatNum + 1) % 3;
			return GetRightMaterial(normalWomenMatList, curNormalWomenMatNum);
		case NPCTYPE.NORMALWHITE2:
			curNormalWhite2MatNum = (curNormalWhite2MatNum + 1) % 3;
			return GetRightMaterial(normalWhite2MatList, curNormalWhite2MatNum);
		case NPCTYPE.NORMALWOMEN2:
			curNormalWomen2MatNum = (curNormalWomen2MatNum + 1) % 3;
			return GetRightMaterial(normalWomen2MatList, curNormalWomen2MatNum);
		case NPCTYPE.POLICE1_HG:
			curPolice1MatNum = (curPolice1MatNum + 1) % 3;
			return GetRightMaterial(police1MatList, curPolice1MatNum);
		case NPCTYPE.POLICE2_HG:
		case NPCTYPE.POLICE2_MG:
			curPolice2MatNum = (curPolice2MatNum + 1) % 3;
			return GetRightMaterial(police2MatList, curPolice2MatNum);
		default:
			curGanstarBlackMatNum = (curGanstarBlackMatNum + 1) % 3;
			return GetRightMaterial(ganstarBlackMatList, curGanstarBlackMatNum);
		}
	}

	public Material GetRightMaterial(Material[] matArray, int matCurNum)
	{
		return matArray[matCurNum];
	}
}
