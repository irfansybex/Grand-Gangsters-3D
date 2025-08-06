using UnityEngine;

[AddComponentMenu("Image Effects/Blur")]
[ExecuteInEditMode]
public class MyBlurEffect : MonoBehaviour
{
	public int iterations = 3;

	public float blurSpread = 0.6f;

	private static string blurMatString = "Shader \"BlurConeTap\" {\r\n\tProperties { _MainTex (\"\", any) = \"\" {} }\r\n\tSubShader {\r\n\t\tPass {\r\n\t\t\tZTest Always Cull Off ZWrite Off Fog { Mode Off }\r\n\t\t\tSetTexture [_MainTex] {constantColor (0,0,0,0.25) combine texture * constant alpha}\r\n\t\t\tSetTexture [_MainTex] {constantColor (0,0,0,0.25) combine texture * constant + previous}\r\n\t\t\tSetTexture [_MainTex] {constantColor (0,0,0,0.25) combine texture * constant + previous}\r\n\t\t\tSetTexture [_MainTex] {constantColor (0,0,0,0.25) combine texture * constant + previous}\r\n\t\t}\r\n\t}\r\n\tFallback off\r\n}";

	private static Material m_Material;

	protected static Material material
	{
		get
		{
			if (m_Material == null)
			{
				m_Material = new Material(blurMatString);
				m_Material.hideFlags = HideFlags.HideAndDontSave;
				m_Material.shader.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_Material;
		}
	}

	protected void OnDisable()
	{
		if ((bool)m_Material)
		{
			Object.DestroyImmediate(m_Material.shader);
			Object.DestroyImmediate(m_Material);
		}
	}

	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
		}
		else if (!material.shader.isSupported)
		{
			base.enabled = false;
		}
	}

	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * blurSpread;
		Graphics.BlitMultiTap(source, dest, material, new Vector2(0f - num, 0f - num), new Vector2(0f - num, num), new Vector2(num, num), new Vector2(num, 0f - num));
	}

	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, material, new Vector2(0f - num, 0f - num), new Vector2(0f - num, num), new Vector2(num, num), new Vector2(num, 0f - num));
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
		DownSample4x(source, temporary);
		bool flag = true;
		for (int i = 0; i < iterations; i++)
		{
			if (flag)
			{
				FourTapCone(temporary, temporary2, i);
			}
			else
			{
				FourTapCone(temporary2, temporary, i);
			}
			flag = !flag;
		}
		if (flag)
		{
			Graphics.Blit(temporary, destination);
		}
		else
		{
			Graphics.Blit(temporary2, destination);
		}
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}
}
