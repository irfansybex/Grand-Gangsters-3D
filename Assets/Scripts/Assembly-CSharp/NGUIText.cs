using System.Text;
using UnityEngine;

public static class NGUIText
{
	public enum SymbolStyle
	{
		None,
		Uncolored,
		Colored
	}

	public class GlyphInfo
	{
		public Vector2 v0;

		public Vector2 v1;

		public Vector2 u0;

		public Vector2 u1;

		public float advance;

		public int channel;

		public bool rotatedUVs;
	}

	public static UIFont bitmapFont;

	public static Font dynamicFont;

	public static GlyphInfo glyph = new GlyphInfo();

	public static int size = 16;

	public static float pixelDensity = 1f;

	public static FontStyle style = FontStyle.Normal;

	public static TextAlignment alignment = TextAlignment.Left;

	public static Color tint = Color.white;

	public static int lineWidth = 1000000;

	public static int lineHeight = 1000000;

	public static int maxLines = 0;

	public static bool gradient = false;

	public static Color gradientBottom = Color.white;

	public static Color gradientTop = Color.white;

	public static bool encoding = false;

	public static int spacingX = 0;

	public static int spacingY = 0;

	public static bool premultiply = false;

	public static SymbolStyle symbolStyle;

	public static int finalSize = 0;

	public static float baseline = 0f;

	public static bool useSymbols = false;

	private static Color mInvisible = new Color(0f, 0f, 0f, 0f);

	private static BetterList<Color> mColors = new BetterList<Color>();

	private static CharacterInfo mTempChar;

	private static BetterList<float> mSizes = new BetterList<float>();

	private static Color32 s_c0;

	private static Color32 s_c1;

	public static void Update()
	{
		Update(true);
	}

	public static void Update(bool request)
	{
		finalSize = Mathf.RoundToInt((float)size * pixelDensity);
		useSymbols = bitmapFont != null && bitmapFont.hasSymbols && encoding && symbolStyle != SymbolStyle.None;
		if (dynamicFont != null && request)
		{
			dynamicFont.RequestCharactersInTexture("j", finalSize, style);
			dynamicFont.GetCharacterInfo('j', out mTempChar, finalSize, style);
			baseline = mTempChar.vert.yMax + (float)finalSize;
		}
	}

	public static void Prepare(string text)
	{
		if (dynamicFont != null)
		{
			dynamicFont.RequestCharactersInTexture(text, finalSize, style);
		}
	}

	public static BMSymbol GetSymbol(string text, int index, int textLength)
	{
		return (!(bitmapFont != null)) ? null : bitmapFont.MatchSymbol(text, index, textLength);
	}

	public static float GetGlyphWidth(int ch, int prev)
	{
		if (bitmapFont != null)
		{
			BMGlyph bMGlyph = bitmapFont.bmFont.GetGlyph(ch);
			if (bMGlyph != null)
			{
				return (prev == 0) ? ((float)bMGlyph.advance) : ((float)bMGlyph.advance + (float)bMGlyph.GetKerning(prev) / pixelDensity);
			}
		}
		else if (dynamicFont != null && dynamicFont.GetCharacterInfo((char)ch, out mTempChar, finalSize, style))
		{
			return Mathf.Round(mTempChar.width / pixelDensity);
		}
		return 0f;
	}

	public static GlyphInfo GetGlyph(int ch, int prev)
	{
		if (bitmapFont != null)
		{
			BMGlyph bMGlyph = bitmapFont.bmFont.GetGlyph(ch);
			if (bMGlyph != null)
			{
				int num = ((prev != 0) ? bMGlyph.GetKerning(prev) : 0);
				glyph.v0.x = ((prev == 0) ? bMGlyph.offsetX : (bMGlyph.offsetX + num));
				glyph.v1.y = -bMGlyph.offsetY;
				glyph.v1.x = glyph.v0.x + (float)bMGlyph.width;
				glyph.v0.y = glyph.v1.y - (float)bMGlyph.height;
				glyph.u0.x = bMGlyph.x;
				glyph.u0.y = bMGlyph.y + bMGlyph.height;
				glyph.u1.x = bMGlyph.x + bMGlyph.width;
				glyph.u1.y = bMGlyph.y;
				glyph.v0 /= pixelDensity;
				glyph.v1 /= pixelDensity;
				glyph.advance = (float)(bMGlyph.advance + num) / pixelDensity;
				glyph.channel = bMGlyph.channel;
				glyph.rotatedUVs = false;
				return glyph;
			}
		}
		else if (dynamicFont != null && dynamicFont.GetCharacterInfo((char)ch, out mTempChar, finalSize, style))
		{
			glyph.v0.x = mTempChar.vert.xMin;
			glyph.v1.x = glyph.v0.x + mTempChar.vert.width;
			glyph.v0.y = mTempChar.vert.yMax - baseline;
			glyph.v1.y = glyph.v0.y - mTempChar.vert.height;
			glyph.u0.x = mTempChar.uv.xMin;
			glyph.u0.y = mTempChar.uv.yMin;
			glyph.u1.x = mTempChar.uv.xMax;
			glyph.u1.y = mTempChar.uv.yMax;
			glyph.v0.x = Mathf.Round(glyph.v0.x) / pixelDensity;
			glyph.v0.y = Mathf.Round(glyph.v0.y) / pixelDensity;
			glyph.v1.x = Mathf.Round(glyph.v1.x) / pixelDensity;
			glyph.v1.y = Mathf.Round(glyph.v1.y) / pixelDensity;
			glyph.advance = Mathf.Round(mTempChar.width) / pixelDensity;
			glyph.channel = 0;
			glyph.rotatedUVs = mTempChar.flipped;
			return glyph;
		}
		return null;
	}

	public static Color ParseColor(string text, int offset)
	{
		int num = (NGUIMath.HexToDecimal(text[offset]) << 4) | NGUIMath.HexToDecimal(text[offset + 1]);
		int num2 = (NGUIMath.HexToDecimal(text[offset + 2]) << 4) | NGUIMath.HexToDecimal(text[offset + 3]);
		int num3 = (NGUIMath.HexToDecimal(text[offset + 4]) << 4) | NGUIMath.HexToDecimal(text[offset + 5]);
		float num4 = 0.003921569f;
		return new Color(num4 * (float)num, num4 * (float)num2, num4 * (float)num3);
	}

	public static string EncodeColor(Color c)
	{
		int num = 0xFFFFFF & (NGUIMath.ColorToInt(c) >> 8);
		return NGUIMath.DecimalToHex(num);
	}

	public static int ParseSymbol(string text, int index)
	{
		int length = text.Length;
		if (index + 2 < length && text[index] == '[')
		{
			if (text[index + 1] == '-')
			{
				if (text[index + 2] == ']')
				{
					return 3;
				}
			}
			else if (index + 7 < length && text[index + 7] == ']')
			{
				Color c = ParseColor(text, index + 1);
				if (EncodeColor(c) == text.Substring(index + 1, 6).ToUpper())
				{
					return 8;
				}
			}
		}
		return 0;
	}

	public static bool ParseSymbol(string text, ref int index)
	{
		int num = ParseSymbol(text, index);
		if (num != 0)
		{
			index += num;
			return true;
		}
		return false;
	}

	public static bool ParseSymbol(string text, ref int index, BetterList<Color> colors, bool premultiply)
	{
		if (colors == null)
		{
			return ParseSymbol(text, ref index);
		}
		int length = text.Length;
		if (index + 2 < length && text[index] == '[')
		{
			if (text[index + 1] == '-')
			{
				if (text[index + 2] == ']')
				{
					if (colors != null && colors.size > 1)
					{
						colors.RemoveAt(colors.size - 1);
					}
					index += 3;
					return true;
				}
			}
			else if (index + 7 < length && text[index + 7] == ']')
			{
				if (colors != null)
				{
					Color color = ParseColor(text, index + 1);
					if (EncodeColor(color) != text.Substring(index + 1, 6).ToUpper())
					{
						return false;
					}
					color.a = colors[colors.size - 1].a;
					if (premultiply && color.a != 1f)
					{
						color = Color.Lerp(mInvisible, color, color.a);
					}
					colors.Add(color);
				}
				index += 8;
				return true;
			}
		}
		return false;
	}

	public static string StripSymbols(string text)
	{
		if (text != null)
		{
			int num = 0;
			int length = text.Length;
			while (num < length)
			{
				char c = text[num];
				if (c == '[')
				{
					int num2 = ParseSymbol(text, num);
					if (num2 != 0)
					{
						text = text.Remove(num, num2);
						length = text.Length;
						continue;
					}
				}
				num++;
			}
		}
		return text;
	}

	public static void Align(BetterList<Vector3> verts, int indexOffset, float offset)
	{
		if (alignment == TextAlignment.Left)
		{
			return;
		}
		float num = 0f;
		if (alignment == TextAlignment.Right)
		{
			num = (float)lineWidth - offset;
			if (num < 0f)
			{
				num = 0f;
			}
		}
		else
		{
			num = ((float)lineWidth - offset) * 0.5f;
			if (num < 0f)
			{
				num = 0f;
			}
			int num2 = Mathf.RoundToInt(((float)lineWidth - offset) * pixelDensity);
			int num3 = Mathf.RoundToInt(lineWidth);
			bool flag = (num2 & 1) == 1;
			bool flag2 = (num3 & 1) == 1;
			if ((flag && !flag2) || (!flag && flag2))
			{
				num += 0.5f * pixelDensity;
			}
		}
		for (int i = indexOffset; i < verts.size; i++)
		{
			verts.buffer[i] = verts.buffer[i];
			verts.buffer[i].x += num;
		}
	}

	public static int GetClosestCharacter(BetterList<Vector3> verts, Vector2 pos)
	{
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		int result = 0;
		for (int i = 0; i < verts.size; i++)
		{
			float num3 = Mathf.Abs(pos.y - verts[i].y);
			if (!(num3 > num2))
			{
				float num4 = Mathf.Abs(pos.x - verts[i].x);
				if (num3 < num2)
				{
					num2 = num3;
					num = num4;
					result = i;
				}
				else if (num4 < num)
				{
					num = num4;
					result = i;
				}
			}
		}
		return result;
	}

	public static void EndLine(ref StringBuilder s)
	{
		int num = s.Length - 1;
		if (num > 0 && s[num] == ' ')
		{
			s[num] = '\n';
		}
		else
		{
			s.Append('\n');
		}
	}

	public static Vector2 CalculatePrintedSize(string text)
	{
		Vector2 zero = Vector2.zero;
		if (!string.IsNullOrEmpty(text))
		{
			if (encoding)
			{
				text = StripSymbols(text);
			}
			Prepare(text);
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = size + spacingY;
			int length = text.Length;
			int num5 = 0;
			int prev = 0;
			for (int i = 0; i < length; i++)
			{
				num5 = text[i];
				if (num5 == 10)
				{
					if (num > num3)
					{
						num3 = num;
					}
					num = 0f;
					num2 += num4;
				}
				else
				{
					if (num5 < 32)
					{
						continue;
					}
					BMSymbol bMSymbol = ((!useSymbols) ? null : GetSymbol(text, i, length));
					if (bMSymbol == null)
					{
						num5 = text[i];
						float glyphWidth = GetGlyphWidth(num5, prev);
						if (glyphWidth != 0f)
						{
							num += (float)spacingX + glyphWidth;
						}
						prev = num5;
					}
					else
					{
						num += (float)(spacingX + bMSymbol.advance);
						i += bMSymbol.sequence.Length - 1;
						prev = 0;
					}
				}
			}
			zero.x = ((!(num > num3)) ? num3 : num);
			zero.y = num2 + (float)size;
		}
		return zero;
	}

	public static int CalculateOffsetToFit(string text)
	{
		if (string.IsNullOrEmpty(text) || lineWidth < 1)
		{
			return 0;
		}
		Prepare(text);
		int length = text.Length;
		int num = 0;
		int prev = 0;
		int i = 0;
		for (int length2 = text.Length; i < length2; i++)
		{
			BMSymbol bMSymbol = ((!useSymbols) ? null : GetSymbol(text, i, length));
			if (bMSymbol == null)
			{
				num = text[i];
				float glyphWidth = GetGlyphWidth(num, prev);
				if (glyphWidth != 0f)
				{
					mSizes.Add((float)spacingX + glyphWidth);
				}
				prev = num;
				continue;
			}
			mSizes.Add(spacingX + bMSymbol.advance);
			int j = 0;
			for (int num2 = bMSymbol.sequence.Length - 1; j < num2; j++)
			{
				mSizes.Add(0f);
			}
			i += bMSymbol.sequence.Length - 1;
			prev = 0;
		}
		float num3 = lineWidth;
		int num4 = mSizes.size;
		while (num4 > 0 && num3 > 0f)
		{
			num3 -= mSizes[--num4];
		}
		mSizes.Clear();
		if (num3 < 0f)
		{
			num4++;
		}
		return num4;
	}

	public static string GetEndOfLineThatFits(string text)
	{
		int length = text.Length;
		int num = CalculateOffsetToFit(text);
		return text.Substring(num, length - num);
	}

	public static void RequestCharactersInTexture(Font font, string text)
	{
		if (font != null)
		{
			font.RequestCharactersInTexture(text, finalSize, style);
		}
	}

	public static bool WrapText(string text, out string finalText)
	{
		if (lineWidth < 1 || lineHeight < 1)
		{
			finalText = string.Empty;
			return false;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		float num = ((maxLines <= 0) ? lineHeight : Mathf.Min(lineHeight, size * maxLines));
		float num2 = size + spacingY;
		int num3 = ((maxLines <= 0) ? 1000000 : maxLines);
		num3 = Mathf.FloorToInt((!(num2 > 0f)) ? 0f : Mathf.Min(num3, num / num2));
		if (num3 == 0)
		{
			finalText = string.Empty;
			return false;
		}
		Prepare(text);
		StringBuilder s = new StringBuilder();
		int length = text.Length;
		float num4 = lineWidth;
		int i = 0;
		int j = 0;
		int num5 = 1;
		int num6 = 0;
		bool flag = true;
		for (; j < length; j++)
		{
			char c = text[j];
			if (c == '\n')
			{
				if (num5 == num3)
				{
					break;
				}
				num4 = lineWidth;
				if (i < j)
				{
					s.Append(text.Substring(i, j - i + 1));
				}
				else
				{
					s.Append(c);
				}
				flag = true;
				num5++;
				i = j + 1;
				num6 = 0;
				continue;
			}
			if (c == ' ' && num6 != 32 && i < j)
			{
				s.Append(text.Substring(i, j - i + 1));
				flag = false;
				i = j + 1;
				num6 = c;
			}
			if (encoding && ParseSymbol(text, ref j))
			{
				j--;
				continue;
			}
			BMSymbol bMSymbol = ((!useSymbols) ? null : GetSymbol(text, j, length));
			float num7;
			if (bMSymbol == null)
			{
				float glyphWidth = GetGlyphWidth(c, num6);
				if (glyphWidth == 0f)
				{
					continue;
				}
				num7 = (float)spacingX + glyphWidth;
			}
			else
			{
				num7 = spacingX + bMSymbol.advance;
			}
			num4 -= num7;
			if (num4 < 0f)
			{
				if (!flag && num5 != num3)
				{
					for (; i < length && text[i] == ' '; i++)
					{
					}
					flag = true;
					num4 = lineWidth;
					j = i - 1;
					num6 = 0;
					if (num5++ == num3)
					{
						break;
					}
					EndLine(ref s);
					continue;
				}
				s.Append(text.Substring(i, Mathf.Max(0, j - i)));
				if (num5++ == num3)
				{
					i = j;
					break;
				}
				EndLine(ref s);
				flag = true;
				if (c == ' ')
				{
					i = j + 1;
					num4 = lineWidth;
				}
				else
				{
					i = j;
					num4 = (float)lineWidth - num7;
				}
				num6 = 0;
			}
			else
			{
				num6 = c;
			}
			if (bMSymbol != null)
			{
				j += bMSymbol.length - 1;
				num6 = 0;
			}
		}
		if (i < j)
		{
			s.Append(text.Substring(i, j - i));
		}
		finalText = s.ToString();
		return j == length || num5 <= Mathf.Min(maxLines, num3);
	}

	public static void Print(string text, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		int num = verts.size;
		float num2 = size + spacingY;
		Prepare(text);
		mColors.Add(Color.white);
		int num3 = 0;
		int prev = 0;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = size;
		Color a = tint * gradientBottom;
		Color b = tint * gradientTop;
		Color32 color = tint;
		int length = text.Length;
		Rect rect = default(Rect);
		float num8 = 0f;
		float num9 = 0f;
		if (bitmapFont != null)
		{
			rect = bitmapFont.uvRect;
			num8 = rect.width / (float)bitmapFont.texWidth;
			num9 = rect.height / (float)bitmapFont.texHeight;
		}
		for (int i = 0; i < length; i++)
		{
			num3 = text[i];
			if (num3 == 10)
			{
				if (num4 > num6)
				{
					num6 = num4;
				}
				if (alignment != 0)
				{
					Align(verts, num, num4 - (float)spacingX);
					num = verts.size;
				}
				num4 = 0f;
				num5 += num2;
				prev = 0;
				continue;
			}
			if (num3 < 32)
			{
				prev = num3;
				continue;
			}
			if (encoding && ParseSymbol(text, ref i, mColors, premultiply))
			{
				Color color2 = tint * mColors[mColors.size - 1];
				color = color2;
				if (gradient)
				{
					a = gradientBottom * color2;
					b = gradientTop * color2;
				}
				i--;
				continue;
			}
			BMSymbol bMSymbol = ((!useSymbols) ? null : GetSymbol(text, i, length));
			if (bMSymbol == null)
			{
				GlyphInfo glyphInfo = GetGlyph(num3, prev);
				if (glyphInfo == null)
				{
					continue;
				}
				prev = num3;
				if (num3 == 32)
				{
					num4 += (float)spacingX + glyphInfo.advance;
					continue;
				}
				if (uvs != null)
				{
					if (bitmapFont != null)
					{
						glyphInfo.u0.x = rect.xMin + num8 * glyphInfo.u0.x;
						glyphInfo.u1.x = rect.xMin + num8 * glyphInfo.u1.x;
						glyphInfo.u0.y = rect.yMax - num9 * glyphInfo.u0.y;
						glyphInfo.u1.y = rect.yMax - num9 * glyphInfo.u1.y;
					}
					if (glyphInfo.rotatedUVs)
					{
						uvs.Add(glyphInfo.u0);
						uvs.Add(new Vector2(glyphInfo.u1.x, glyphInfo.u0.y));
						uvs.Add(glyphInfo.u1);
						uvs.Add(new Vector2(glyphInfo.u0.x, glyphInfo.u1.y));
					}
					else
					{
						uvs.Add(glyphInfo.u0);
						uvs.Add(new Vector2(glyphInfo.u0.x, glyphInfo.u1.y));
						uvs.Add(glyphInfo.u1);
						uvs.Add(new Vector2(glyphInfo.u1.x, glyphInfo.u0.y));
					}
				}
				if (cols != null)
				{
					if (glyphInfo.channel == 0 || glyphInfo.channel == 15)
					{
						if (gradient)
						{
							float num10 = num7 + glyphInfo.v0.y;
							float num11 = num7 + glyphInfo.v1.y;
							num10 /= num7;
							num11 /= num7;
							s_c0 = Color.Lerp(a, b, num10);
							s_c1 = Color.Lerp(a, b, num11);
							cols.Add(s_c0);
							cols.Add(s_c1);
							cols.Add(s_c1);
							cols.Add(s_c0);
						}
						else
						{
							for (int j = 0; j < 4; j++)
							{
								cols.Add(color);
							}
						}
					}
					else
					{
						Color color3 = color;
						color3 *= 0.49f;
						switch (glyphInfo.channel)
						{
						case 1:
							color3.b += 0.51f;
							break;
						case 2:
							color3.g += 0.51f;
							break;
						case 4:
							color3.r += 0.51f;
							break;
						case 8:
							color3.a += 0.51f;
							break;
						}
						for (int k = 0; k < 4; k++)
						{
							cols.Add(color3);
						}
					}
				}
				glyphInfo.v0.x += num4;
				glyphInfo.v1.x += num4;
				glyphInfo.v0.y -= num5;
				glyphInfo.v1.y -= num5;
				num4 += (float)spacingX + glyphInfo.advance;
				verts.Add(glyphInfo.v0);
				verts.Add(new Vector3(glyphInfo.v0.x, glyphInfo.v1.y));
				verts.Add(glyphInfo.v1);
				verts.Add(new Vector3(glyphInfo.v1.x, glyphInfo.v0.y));
				continue;
			}
			float num12 = num4 + (float)bMSymbol.offsetX;
			float x = num12 + (float)bMSymbol.width;
			float num13 = 0f - (num5 + (float)bMSymbol.offsetY);
			float y = num13 - (float)bMSymbol.height;
			verts.Add(new Vector3(num12, y));
			verts.Add(new Vector3(num12, num13));
			verts.Add(new Vector3(x, num13));
			verts.Add(new Vector3(x, y));
			num4 += (float)(spacingX + bMSymbol.advance);
			i += bMSymbol.length - 1;
			prev = 0;
			if (uvs != null)
			{
				Rect uvRect = bMSymbol.uvRect;
				float xMin = uvRect.xMin;
				float yMin = uvRect.yMin;
				float xMax = uvRect.xMax;
				float yMax = uvRect.yMax;
				uvs.Add(new Vector2(xMin, yMin));
				uvs.Add(new Vector2(xMin, yMax));
				uvs.Add(new Vector2(xMax, yMax));
				uvs.Add(new Vector2(xMax, yMin));
			}
			if (cols == null)
			{
				continue;
			}
			if (symbolStyle == SymbolStyle.Colored)
			{
				for (int l = 0; l < 4; l++)
				{
					cols.Add(color);
				}
				continue;
			}
			Color32 item = Color.white;
			item.a = color.a;
			for (int m = 0; m < 4; m++)
			{
				cols.Add(item);
			}
		}
		if (alignment != 0 && num < verts.size)
		{
			Align(verts, num, num4 - (float)spacingX);
			num = verts.size;
		}
		mColors.Clear();
	}

	public static void PrintCharacterPositions(string text, BetterList<Vector3> verts, BetterList<int> indices)
	{
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		Prepare(text);
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = (float)size * 0.5f;
		float num5 = size + spacingY;
		int length = text.Length;
		int num6 = verts.size;
		int num7 = 0;
		int prev = 0;
		for (int i = 0; i < length; i++)
		{
			num7 = text[i];
			verts.Add(new Vector3(num, 0f - num2 - num4));
			indices.Add(i);
			if (num7 == 10)
			{
				if (num > num3)
				{
					num3 = num;
				}
				if (alignment != 0)
				{
					Align(verts, num6, num - (float)spacingX);
					num6 = verts.size;
				}
				num = 0f;
				num2 += num5;
				prev = 0;
				continue;
			}
			if (num7 < 32)
			{
				prev = 0;
				continue;
			}
			if (encoding && ParseSymbol(text, ref i))
			{
				i--;
				continue;
			}
			BMSymbol bMSymbol = ((!useSymbols) ? null : GetSymbol(text, i, length));
			if (bMSymbol == null)
			{
				float glyphWidth = GetGlyphWidth(num7, prev);
				if (glyphWidth != 0f)
				{
					num += glyphWidth + (float)spacingX;
					verts.Add(new Vector3(num, 0f - num2 - num4));
					indices.Add(i + 1);
					prev = num7;
				}
			}
			else
			{
				num += (float)(bMSymbol.advance + spacingX);
				verts.Add(new Vector3(num, 0f - num2 - num4));
				indices.Add(i + 1);
				i += bMSymbol.sequence.Length - 1;
				prev = 0;
			}
		}
		if (alignment != 0 && num6 < verts.size)
		{
			Align(verts, num6, num - (float)spacingX);
		}
	}

	public static void PrintCaretAndSelection(string text, int start, int end, BetterList<Vector3> caret, BetterList<Vector3> highlight)
	{
		if (string.IsNullOrEmpty(text))
		{
			text = " ";
		}
		Prepare(text);
		int num = end;
		if (start > end)
		{
			end = start;
			start = num;
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = size;
		float num6 = size + spacingY;
		int indexOffset = ((caret != null) ? caret.size : 0);
		int num7 = ((highlight != null) ? highlight.size : 0);
		int length = text.Length;
		int i = 0;
		int num8 = 0;
		int prev = 0;
		bool flag = false;
		bool flag2 = false;
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		for (; i < length; i++)
		{
			if (caret != null && !flag2 && num <= i)
			{
				flag2 = true;
				caret.Add(new Vector3(num2 - 1f, 0f - num3 - num5));
				caret.Add(new Vector3(num2 - 1f, 0f - num3));
				caret.Add(new Vector3(num2 + 1f, 0f - num3));
				caret.Add(new Vector3(num2 + 1f, 0f - num3 - num5));
			}
			num8 = text[i];
			if (num8 == 10)
			{
				if (num2 > num4)
				{
					num4 = num2;
				}
				if (caret != null && flag2)
				{
					if (alignment != 0)
					{
						Align(caret, indexOffset, num2 - (float)spacingX);
					}
					caret = null;
				}
				if (highlight != null)
				{
					if (flag)
					{
						flag = false;
						highlight.Add(vector2);
						highlight.Add(vector);
					}
					else if (start <= i && end > i)
					{
						highlight.Add(new Vector3(num2, 0f - num3 - num5));
						highlight.Add(new Vector3(num2, 0f - num3));
						highlight.Add(new Vector3(num2 + 2f, 0f - num3));
						highlight.Add(new Vector3(num2 + 2f, 0f - num3 - num5));
					}
					if (alignment != 0 && num7 < highlight.size)
					{
						Align(highlight, num7, num2 - (float)spacingX);
						num7 = highlight.size;
					}
				}
				num2 = 0f;
				num3 += num6;
				prev = 0;
				continue;
			}
			if (num8 < 32)
			{
				prev = 0;
				continue;
			}
			if (encoding && ParseSymbol(text, ref i, mColors, premultiply))
			{
				i--;
				continue;
			}
			BMSymbol bMSymbol = ((!useSymbols) ? null : GetSymbol(text, i, length));
			float num9 = ((bMSymbol == null) ? GetGlyphWidth(num8, prev) : ((float)bMSymbol.advance));
			if (num9 == 0f)
			{
				continue;
			}
			float x = num2;
			float x2 = num2 + num9;
			float y = 0f - num3 - num5;
			float y2 = 0f - num3;
			num2 += num9 + (float)spacingX;
			if (highlight != null)
			{
				if (start > i || end <= i)
				{
					if (flag)
					{
						flag = false;
						highlight.Add(vector2);
						highlight.Add(vector);
					}
				}
				else if (!flag)
				{
					flag = true;
					highlight.Add(new Vector3(x, y));
					highlight.Add(new Vector3(x, y2));
				}
			}
			vector = new Vector2(x2, y);
			vector2 = new Vector2(x2, y2);
			prev = num8;
		}
		if (caret != null)
		{
			if (!flag2)
			{
				caret.Add(new Vector3(num2 - 1f, 0f - num3 - num5));
				caret.Add(new Vector3(num2 - 1f, 0f - num3));
				caret.Add(new Vector3(num2 + 1f, 0f - num3));
				caret.Add(new Vector3(num2 + 1f, 0f - num3 - num5));
			}
			if (alignment != 0)
			{
				Align(caret, indexOffset, num2 - (float)spacingX);
			}
		}
		if (highlight != null)
		{
			if (flag)
			{
				highlight.Add(vector2);
				highlight.Add(vector);
			}
			else if (start < i && end == i)
			{
				highlight.Add(new Vector3(num2, 0f - num3 - num5));
				highlight.Add(new Vector3(num2, 0f - num3));
				highlight.Add(new Vector3(num2 + 2f, 0f - num3));
				highlight.Add(new Vector3(num2 + 2f, 0f - num3 - num5));
			}
			if (alignment != 0 && num7 < highlight.size)
			{
				Align(highlight, num7, num2 - (float)spacingX);
			}
		}
	}
}
