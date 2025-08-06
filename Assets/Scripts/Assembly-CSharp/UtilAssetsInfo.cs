using UnityEngine;

public class UtilAssetsInfo : MonoBehaviour
{
	public Transform root;

	public Texture[] textures;

	private bool hasInit;

	private float totalTextureMemory;

	public float[] texturesMemoryList;

	private string totoalMemoryStr;

	public string[] texutureInfoStrList;

	public bool isShow;

	private Vector2 scrollPosition;

	private void Start()
	{
	}

	private void Update()
	{
		if (!hasInit)
		{
			hasInit = true;
			refreshMesh();
		}
	}

	public void refreshMesh()
	{
		int count = 0;
		caculate(ref count, root);
	}

	public void caculate(ref int count, Transform parent)
	{
		parent.gameObject.SetActiveRecursively(true);
		Transform[] componentsInChildren = parent.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			if (transform.gameObject.GetComponent<MeshFilter>() != null)
			{
				MeshFilter component = transform.gameObject.GetComponent<MeshFilter>();
				count += component.mesh.vertexCount;
			}
		}
	}

	public void refresh()
	{
		textures = Object.FindObjectsOfTypeIncludingAssets(typeof(Texture)) as Texture[];
		texturesMemoryList = new float[textures.Length];
		texutureInfoStrList = new string[textures.Length];
		totalTextureMemory = 0f;
		for (int i = 0; i < textures.Length; i++)
		{
			float num = (float)getTextureMemory(textures[i]) / 1024f;
			texturesMemoryList[i] = num;
			totalTextureMemory += num;
			texutureInfoStrList[i] = textures[i].name + getSizeShow(textures[i]);
			texutureInfoStrList[i] += "\n";
			texutureInfoStrList[i] += getMemoryShow(num);
			if (textures[i] is Texture2D)
			{
				ref string reference = ref texutureInfoStrList[i];
				string text = reference;
				reference = string.Concat(text, "(", (textures[i] as Texture2D).format, ")");
			}
		}
		totoalMemoryStr = getMemoryShow(totalTextureMemory);
		sortByMemory(textures, texturesMemoryList, texutureInfoStrList);
	}

	public void OnGUI()
	{
		GUILayout.Space((float)Screen.height * 0.1f);
		if (GUILayout.Button("Textures Memory"))
		{
			isShow = !isShow;
		}
		if (isShow)
		{
			GUILayout.Label("texures asset count:" + textures.Length + " || " + totoalMemoryStr);
			if (GUILayout.Button("refresh"))
			{
				refresh();
			}
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true, GUILayout.Width(250f), GUILayout.Height((float)Screen.height * 0.7f));
			for (int i = 0; i < textures.Length; i++)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(textures[i], GUILayout.Height(40f), GUILayout.Width(40f));
				GUILayout.Label(texutureInfoStrList[i]);
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}
	}

	private string getSizeShow(Texture tex)
	{
		return " (" + tex.width + "*" + tex.height + ")";
	}

	private string getMemoryShow(float sizeKB)
	{
		if (sizeKB < 1024f)
		{
			return string.Format("{0:F2}KB", sizeKB);
		}
		return string.Format("{0:F2}M", sizeKB / 1024f);
	}

	private void sortByMemory(Texture[] texList, float[] memoryList, string[] infoStrList)
	{
		int num = memoryList.Length;
		for (int i = 0; i < num - 1; i++)
		{
			int num2 = i;
			for (int j = i + 1; j < num; j++)
			{
				if (memoryList[j] > memoryList[num2])
				{
					num2 = j;
				}
			}
			if (num2 != i)
			{
				float num3 = memoryList[i];
				memoryList[i] = memoryList[num2];
				memoryList[num2] = num3;
				Texture texture = texList[i];
				texList[i] = texList[num2];
				texList[num2] = texture;
				string text = infoStrList[i];
				infoStrList[i] = infoStrList[num2];
				infoStrList[num2] = text;
			}
		}
	}

	private int getTextureMemory(Texture texture)
	{
		int num = texture.width;
		int num2 = texture.height;
		if (texture is Texture2D)
		{
			Texture2D texture2D = texture as Texture2D;
			int bitsPerPixel = GetBitsPerPixel(texture2D.format);
			int mipmapCount = texture2D.mipmapCount;
			int i = 1;
			int num3 = 0;
			for (; i <= mipmapCount; i++)
			{
				num3 += num * num2 * bitsPerPixel / 8;
				num /= 2;
				num2 /= 2;
			}
			return num3;
		}
		if (texture is Cubemap)
		{
			Cubemap cubemap = texture as Cubemap;
			int bitsPerPixel2 = GetBitsPerPixel(cubemap.format);
			return num * num2 * 6 * bitsPerPixel2 / 8;
		}
		return 0;
	}

	private int GetBitsPerPixel(TextureFormat format)
	{
		switch (format)
		{
		case TextureFormat.Alpha8:
			return 8;
		case TextureFormat.ARGB4444:
			return 16;
		case TextureFormat.RGB24:
			return 24;
		case TextureFormat.RGBA32:
			return 32;
		case TextureFormat.ARGB32:
			return 32;
		case TextureFormat.RGB565:
			return 16;
		case TextureFormat.DXT1:
			return 4;
		case TextureFormat.DXT5:
			return 8;
		case TextureFormat.PVRTC_RGB2:
			return 2;
		case TextureFormat.PVRTC_RGBA2:
			return 2;
		case TextureFormat.PVRTC_RGB4:
			return 4;
		case TextureFormat.PVRTC_RGBA4:
			return 4;
		case TextureFormat.ETC_RGB4:
			return 4;
		case TextureFormat.ETC2_RGBA8:
			return 8;
		case TextureFormat.BGRA32:
			return 32;
		default:
			return 0;
		}
	}
}
