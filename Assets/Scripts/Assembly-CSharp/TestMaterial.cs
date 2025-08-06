using UnityEngine;

public class TestMaterial : MonoBehaviour
{
	public GameObject skinObj;

	public Material appearMat;

	public Material defaultMat;

	public bool flag;

	public bool t;

	private void Start()
	{
		skinObj.GetComponent<Renderer>().sharedMaterial = appearMat;
		Invoke("ttt", 2f);
	}

	private void OnEnable()
	{
	}

	private void Update()
	{
	}

	public void ttt()
	{
		skinObj.GetComponent<Renderer>().sharedMaterial = defaultMat;
	}
}
