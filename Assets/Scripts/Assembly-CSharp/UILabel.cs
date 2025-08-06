using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Label")]
[ExecuteInEditMode]
public class UILabel : UIWidget
{
	public enum Effect
	{
		None,
		Shadow,
		Outline
	}

	public enum Overflow
	{
		ShrinkContent,
		ClampContent,
		ResizeFreely,
		ResizeHeight
	}

	public enum Crispness
	{
		Never,
		OnDesktop,
		Always
	}

	public Crispness keepCrispWhenShrunk = Crispness.OnDesktop;

	[SerializeField]
	[HideInInspector]
	private Font mTrueTypeFont;

	[SerializeField]
	[HideInInspector]
	private UIFont mFont;

	[Multiline(6)]
	[HideInInspector]
	[SerializeField]
	private string mText = string.Empty;

	[HideInInspector]
	[SerializeField]
	private int mFontSize = 16;

	[HideInInspector]
	[SerializeField]
	private FontStyle mFontStyle;

	[SerializeField]
	[HideInInspector]
	private bool mEncoding = true;

	[HideInInspector]
	[SerializeField]
	private int mMaxLineCount;

	[HideInInspector]
	[SerializeField]
	private Effect mEffectStyle;

	[SerializeField]
	[HideInInspector]
	private Color mEffectColor = Color.black;

	[SerializeField]
	[HideInInspector]
	private NGUIText.SymbolStyle mSymbols = NGUIText.SymbolStyle.Uncolored;

	[SerializeField]
	[HideInInspector]
	private Vector2 mEffectDistance = Vector2.one;

	[SerializeField]
	[HideInInspector]
	private Overflow mOverflow;

	[SerializeField]
	[HideInInspector]
	private Material mMaterial;

	[SerializeField]
	[HideInInspector]
	private bool mApplyGradient;

	[HideInInspector]
	[SerializeField]
	private Color mGradientTop = Color.white;

	[SerializeField]
	[HideInInspector]
	private Color mGradientBottom = new Color(0.7f, 0.7f, 0.7f);

	[HideInInspector]
	[SerializeField]
	private int mSpacingX;

	[SerializeField]
	[HideInInspector]
	private int mSpacingY;

	[SerializeField]
	[HideInInspector]
	private bool mShrinkToFit;

	[HideInInspector]
	[SerializeField]
	private int mMaxLineWidth;

	[SerializeField]
	[HideInInspector]
	private int mMaxLineHeight;

	[SerializeField]
	[HideInInspector]
	private float mLineWidth;

	[SerializeField]
	[HideInInspector]
	private bool mMultiline = true;

	private Font mActiveTTF;

	private bool mShouldBeProcessed = true;

	private string mProcessedText;

	private bool mPremultiply;

	private Vector2 mCalculatedSize = Vector2.zero;

	private float mScale = 1f;

	private int mLastWidth;

	private int mLastHeight;

	private int mPrintedSize;

	private static BetterList<Vector3> mTempVerts = new BetterList<Vector3>();

	private static BetterList<int> mTempIndices = new BetterList<int>();

	private bool hasChanged
	{
		get
		{
			return mShouldBeProcessed;
		}
		set
		{
			if (value)
			{
				mChanged = true;
				mShouldBeProcessed = true;
			}
			else
			{
				mShouldBeProcessed = false;
			}
		}
	}

	public override Material material
	{
		get
		{
			if (mMaterial != null)
			{
				return mMaterial;
			}
			if (mFont != null)
			{
				return mFont.material;
			}
			if (mTrueTypeFont != null)
			{
				return mTrueTypeFont.material;
			}
			return null;
		}
		set
		{
			if (mMaterial != value)
			{
				MarkAsChanged();
				mMaterial = value;
				MarkAsChanged();
			}
		}
	}

	[Obsolete("Use UILabel.bitmapFont instead")]
	public UIFont font
	{
		get
		{
			return bitmapFont;
		}
		set
		{
			bitmapFont = value;
		}
	}

	public UIFont bitmapFont
	{
		get
		{
			return mFont;
		}
		set
		{
			if (!(mFont != value))
			{
				return;
			}
			if (value != null && value.dynamicFont != null)
			{
				trueTypeFont = value.dynamicFont;
				return;
			}
			if (trueTypeFont != null)
			{
				trueTypeFont = null;
			}
			else
			{
				RemoveFromPanel();
			}
			mFont = value;
			MarkAsChanged();
		}
	}

	public Font trueTypeFont
	{
		get
		{
			return mTrueTypeFont;
		}
		set
		{
			if (mTrueTypeFont != value)
			{
				SetActiveRecursivelyFont(null);
				RemoveFromPanel();
				mTrueTypeFont = value;
				hasChanged = true;
				mFont = null;
				SetActiveRecursivelyFont(value);
				ProcessAndRequest();
				if (mActiveTTF != null)
				{
					base.MarkAsChanged();
				}
			}
		}
	}

	public UnityEngine.Object ambigiousFont
	{
		get
		{
			return (!(mFont != null)) ? ((UnityEngine.Object)mTrueTypeFont) : ((UnityEngine.Object)mFont);
		}
		set
		{
			UIFont uIFont = value as UIFont;
			if (uIFont != null)
			{
				bitmapFont = uIFont;
			}
			else
			{
				trueTypeFont = value as Font;
			}
		}
	}

	public string text
	{
		get
		{
			return mText;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(mText))
				{
					mText = string.Empty;
					hasChanged = true;
					ProcessAndRequest();
				}
			}
			else if (mText != value)
			{
				mText = value;
				hasChanged = true;
				ProcessAndRequest();
			}
		}
	}

	public int fontSize
	{
		get
		{
			if (mFont != null)
			{
				return mFont.defaultSize;
			}
			return mFontSize;
		}
		set
		{
			value = Mathf.Clamp(value, 0, 144);
			if (mFontSize != value)
			{
				mFontSize = value;
				hasChanged = true;
				ProcessAndRequest();
			}
		}
	}

	public FontStyle fontStyle
	{
		get
		{
			return mFontStyle;
		}
		set
		{
			if (mFontStyle != value)
			{
				mFontStyle = value;
				hasChanged = true;
				ProcessAndRequest();
			}
		}
	}

	public bool applyGradient
	{
		get
		{
			return mApplyGradient;
		}
		set
		{
			if (mApplyGradient != value)
			{
				mApplyGradient = value;
				MarkAsChanged();
			}
		}
	}

	public Color gradientTop
	{
		get
		{
			return mGradientTop;
		}
		set
		{
			if (mGradientTop != value)
			{
				mGradientTop = value;
				if (mApplyGradient)
				{
					MarkAsChanged();
				}
			}
		}
	}

	public Color gradientBottom
	{
		get
		{
			return mGradientBottom;
		}
		set
		{
			if (mGradientBottom != value)
			{
				mGradientBottom = value;
				if (mApplyGradient)
				{
					MarkAsChanged();
				}
			}
		}
	}

	public int spacingX
	{
		get
		{
			return mSpacingX;
		}
		set
		{
			if (mSpacingX != value)
			{
				mSpacingX = value;
				MarkAsChanged();
			}
		}
	}

	public int spacingY
	{
		get
		{
			return mSpacingY;
		}
		set
		{
			if (mSpacingY != value)
			{
				mSpacingY = value;
				MarkAsChanged();
			}
		}
	}

	private bool usePrintedSize
	{
		get
		{
			if (trueTypeFont != null && keepCrispWhenShrunk != 0)
			{
				return keepCrispWhenShrunk == Crispness.Always;
			}
			return false;
		}
	}

	public bool supportEncoding
	{
		get
		{
			return mEncoding;
		}
		set
		{
			if (mEncoding != value)
			{
				mEncoding = value;
				hasChanged = true;
			}
		}
	}

	public NGUIText.SymbolStyle symbolStyle
	{
		get
		{
			return mSymbols;
		}
		set
		{
			if (mSymbols != value)
			{
				mSymbols = value;
				hasChanged = true;
			}
		}
	}

	public Overflow overflowMethod
	{
		get
		{
			return mOverflow;
		}
		set
		{
			if (mOverflow != value)
			{
				mOverflow = value;
				hasChanged = true;
			}
		}
	}

	[Obsolete("Use 'width' instead")]
	public int lineWidth
	{
		get
		{
			return base.width;
		}
		set
		{
			base.width = value;
		}
	}

	[Obsolete("Use 'height' instead")]
	public int lineHeight
	{
		get
		{
			return base.height;
		}
		set
		{
			base.height = value;
		}
	}

	public bool multiLine
	{
		get
		{
			return mMaxLineCount != 1;
		}
		set
		{
			if (mMaxLineCount != 1 != value)
			{
				mMaxLineCount = ((!value) ? 1 : 0);
				hasChanged = true;
			}
		}
	}

	public override Vector3[] localCorners
	{
		get
		{
			if (hasChanged)
			{
				ProcessText();
			}
			return base.localCorners;
		}
	}

	public override Vector3[] worldCorners
	{
		get
		{
			if (hasChanged)
			{
				ProcessText();
			}
			return base.worldCorners;
		}
	}

	public override Vector4 drawingDimensions
	{
		get
		{
			if (hasChanged)
			{
				ProcessText();
			}
			return base.drawingDimensions;
		}
	}

	public int maxLineCount
	{
		get
		{
			return mMaxLineCount;
		}
		set
		{
			if (mMaxLineCount != value)
			{
				mMaxLineCount = Mathf.Max(value, 0);
				hasChanged = true;
				if (overflowMethod == Overflow.ShrinkContent)
				{
					MakePixelPerfect();
				}
			}
		}
	}

	public Effect effectStyle
	{
		get
		{
			return mEffectStyle;
		}
		set
		{
			if (mEffectStyle != value)
			{
				mEffectStyle = value;
				hasChanged = true;
			}
		}
	}

	public Color effectColor
	{
		get
		{
			return mEffectColor;
		}
		set
		{
			if (mEffectColor != value)
			{
				mEffectColor = value;
				if (mEffectStyle != 0)
				{
					hasChanged = true;
				}
			}
		}
	}

	public Vector2 effectDistance
	{
		get
		{
			return mEffectDistance;
		}
		set
		{
			if (mEffectDistance != value)
			{
				mEffectDistance = value;
				hasChanged = true;
			}
		}
	}

	[Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
	public bool shrinkToFit
	{
		get
		{
			return mOverflow == Overflow.ShrinkContent;
		}
		set
		{
			if (value)
			{
				overflowMethod = Overflow.ShrinkContent;
			}
		}
	}

	public string processedText
	{
		get
		{
			if (mLastWidth != mWidth || mLastHeight != mHeight)
			{
				mLastWidth = mWidth;
				mLastHeight = mHeight;
				mShouldBeProcessed = true;
			}
			if (hasChanged)
			{
				ProcessText();
			}
			return mProcessedText;
		}
	}

	public Vector2 printedSize
	{
		get
		{
			if (hasChanged)
			{
				ProcessText();
			}
			return mCalculatedSize;
		}
	}

	public override Vector2 localSize
	{
		get
		{
			if (hasChanged)
			{
				ProcessText();
			}
			return base.localSize;
		}
	}

	private bool isValid
	{
		get
		{
			return mFont != null || mTrueTypeFont != null;
		}
	}

	private float pixelSize
	{
		get
		{
			return (!(mFont != null)) ? 1f : mFont.pixelSize;
		}
	}

	protected override void OnInit()
	{
		base.OnInit();
		if (mTrueTypeFont == null && mFont != null && mFont.isDynamic)
		{
			mTrueTypeFont = mFont.dynamicFont;
			mFontSize = mFont.defaultSize;
			mFontStyle = mFont.dynamicFontStyle;
			mFont = null;
		}
		SetActiveRecursivelyFont(mTrueTypeFont);
	}

	protected override void OnDisable()
	{
		SetActiveRecursivelyFont(null);
		base.OnDisable();
	}

	protected void SetActiveRecursivelyFont(Font fnt)
	{
		if (mActiveTTF != fnt)
		{
			if (mActiveTTF != null)
			{
				Font obj = mActiveTTF;
				obj.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Remove(obj.textureRebuildCallback, new Font.FontTextureRebuildCallback(MarkAsChanged));
			}
			mActiveTTF = fnt;
			if (mActiveTTF != null)
			{
				Font obj2 = mActiveTTF;
				obj2.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Combine(obj2.textureRebuildCallback, new Font.FontTextureRebuildCallback(MarkAsChanged));
			}
		}
	}

	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (hasChanged)
		{
			ProcessText();
		}
		return base.GetSides(relativeTo);
	}

	protected override void UpgradeFrom265()
	{
		ProcessText(true);
		if (mShrinkToFit)
		{
			overflowMethod = Overflow.ShrinkContent;
			mMaxLineCount = 0;
		}
		if (mMaxLineWidth != 0)
		{
			base.width = mMaxLineWidth;
			overflowMethod = ((mMaxLineCount > 0) ? Overflow.ResizeHeight : Overflow.ShrinkContent);
		}
		else
		{
			overflowMethod = Overflow.ResizeFreely;
		}
		if (mMaxLineHeight != 0)
		{
			base.height = mMaxLineHeight;
		}
		if (mFont != null)
		{
			int num = Mathf.RoundToInt((float)mFont.defaultSize * mFont.pixelSize);
			if (base.height < num)
			{
				base.height = num;
			}
		}
		mMaxLineWidth = 0;
		mMaxLineHeight = 0;
		mShrinkToFit = false;
		if (GetComponent<BoxCollider>() != null)
		{
			NGUITools.AddWidgetCollider(base.gameObject, true);
		}
	}

	protected override void OnAnchor()
	{
		if (mOverflow == Overflow.ResizeFreely || mOverflow == Overflow.ResizeHeight)
		{
			mOverflow = Overflow.ShrinkContent;
		}
		base.OnAnchor();
	}

	private void ProcessAndRequest()
	{
		if (ambigiousFont != null)
		{
			ProcessText();
			if (mActiveTTF != null)
			{
				NGUIText.RequestCharactersInTexture(mActiveTTF, mText);
			}
		}
	}

	protected override void OnStart()
	{
		base.OnStart();
		if (mLineWidth > 0f)
		{
			mMaxLineWidth = Mathf.RoundToInt(mLineWidth);
			mLineWidth = 0f;
		}
		if (!mMultiline)
		{
			mMaxLineCount = 1;
			mMultiline = true;
		}
		mPremultiply = material != null && material.shader != null && material.shader.name.Contains("Premultiplied");
		ProcessAndRequest();
	}

	public override void MarkAsChanged()
	{
		hasChanged = true;
		base.MarkAsChanged();
	}

	private void ProcessText()
	{
		ProcessText(false);
	}

	private void ProcessText(bool legacyMode)
	{
		if (!isValid)
		{
			return;
		}
		mChanged = true;
		hasChanged = false;
		int num = fontSize;
		float num2 = 1f / (float)num;
		float num3 = pixelSize;
		float num4 = 1f / num3;
		float num5 = ((!legacyMode) ? ((float)base.width * num4) : ((mMaxLineWidth == 0) ? 1000000f : ((float)mMaxLineWidth * num4)));
		float num6 = ((!legacyMode) ? ((float)base.height * num4) : ((mMaxLineHeight == 0) ? 1000000f : ((float)mMaxLineHeight * num4)));
		mScale = 1f;
		mPrintedSize = Mathf.Abs((!legacyMode) ? num : Mathf.RoundToInt(base.cachedTransform.localScale.x));
		UpdateNGUIText(num, mWidth, mHeight);
		if (mPrintedSize > 0)
		{
			bool flag;
			do
			{
				mScale = (float)mPrintedSize * num2;
				flag = true;
				NGUIText.lineWidth = ((mOverflow != Overflow.ResizeFreely) ? Mathf.RoundToInt(num5 / mScale) : 1000000);
				if (mOverflow == Overflow.ResizeFreely || mOverflow == Overflow.ResizeHeight)
				{
					NGUIText.lineHeight = 1000000;
				}
				else
				{
					NGUIText.lineHeight = Mathf.RoundToInt(num6 / mScale);
				}
				NGUIText.Update(false);
				if (num5 > 0f || num6 > 0f)
				{
					flag = NGUIText.WrapText(mText, out mProcessedText);
				}
				else
				{
					mProcessedText = mText;
				}
				if (!string.IsNullOrEmpty(mProcessedText))
				{
					mCalculatedSize = NGUIText.CalculatePrintedSize(mProcessedText);
				}
				else
				{
					mCalculatedSize = Vector2.zero;
				}
				if (mOverflow == Overflow.ResizeFreely)
				{
					mWidth = Mathf.RoundToInt(mCalculatedSize.x * num3);
					mHeight = Mathf.RoundToInt(mCalculatedSize.y * num3);
					if ((mWidth & 1) == 1)
					{
						mWidth++;
					}
					if ((mHeight & 1) == 1)
					{
						mHeight++;
					}
					break;
				}
				if (mOverflow == Overflow.ResizeHeight)
				{
					mHeight = Mathf.RoundToInt(mCalculatedSize.y * num3);
					break;
				}
			}
			while (mOverflow == Overflow.ShrinkContent && !flag && --mPrintedSize > 1);
			if (legacyMode)
			{
				base.width = Mathf.RoundToInt(mCalculatedSize.x * num3);
				base.height = Mathf.RoundToInt(mCalculatedSize.y * num3);
				base.cachedTransform.localScale = Vector3.one;
			}
		}
		else
		{
			base.cachedTransform.localScale = Vector3.one;
			mProcessedText = string.Empty;
			mScale = 1f;
		}
	}

	public override void MakePixelPerfect()
	{
		if (ambigiousFont != null)
		{
			float num = ((!(bitmapFont != null)) ? 1f : bitmapFont.pixelSize);
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = Mathf.RoundToInt(localPosition.x);
			localPosition.y = Mathf.RoundToInt(localPosition.y);
			localPosition.z = Mathf.RoundToInt(localPosition.z);
			base.cachedTransform.localPosition = localPosition;
			base.cachedTransform.localScale = Vector3.one;
			if (mOverflow == Overflow.ResizeFreely)
			{
				AssumeNaturalSize();
				return;
			}
			Overflow overflow = mOverflow;
			mOverflow = Overflow.ShrinkContent;
			ProcessText(false);
			mOverflow = overflow;
			int num2 = Mathf.RoundToInt(mCalculatedSize.x * num);
			int num3 = Mathf.RoundToInt(mCalculatedSize.y * num);
			if (bitmapFont != null)
			{
				num2 = Mathf.Max(bitmapFont.defaultSize);
				num3 = Mathf.Max(bitmapFont.defaultSize);
			}
			else
			{
				num2 = Mathf.Max(base.minWidth);
				num3 = Mathf.Max(base.minHeight);
			}
			if (base.width < num2)
			{
				base.width = num2;
			}
			if (base.height < num3)
			{
				base.height = num3;
			}
		}
		else
		{
			base.MakePixelPerfect();
		}
	}

	public void AssumeNaturalSize()
	{
		if (ambigiousFont != null)
		{
			ProcessText(false);
			float num = ((!(bitmapFont != null)) ? 1f : bitmapFont.pixelSize);
			base.width = Mathf.RoundToInt(mCalculatedSize.x * num);
			base.height = Mathf.RoundToInt(mCalculatedSize.y * num);
		}
	}

	public int GetCharacterIndex(Vector3 worldPos)
	{
		Vector2 localPos = base.cachedTransform.InverseTransformPoint(worldPos);
		return GetCharacterIndex(localPos);
	}

	public int GetCharacterIndex(Vector2 localPos)
	{
		if (isValid)
		{
			string value = processedText;
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}
			float num = ((!(mFont != null)) ? 1f : mFont.pixelSize);
			float num2 = mScale * num;
			bool flag = usePrintedSize;
			if (flag)
			{
				UpdateNGUIText(mPrintedSize, mWidth, mHeight);
			}
			else
			{
				UpdateNGUIText(fontSize, Mathf.RoundToInt((float)mWidth / num2), mHeight);
			}
			NGUIText.PrintCharacterPositions(value, mTempVerts, mTempIndices);
			if (mTempVerts.size > 0)
			{
				ApplyOffset(mTempVerts, flag, num2, 0);
				int closestCharacter = NGUIText.GetClosestCharacter(mTempVerts, localPos);
				closestCharacter = mTempIndices[closestCharacter];
				mTempVerts.Clear();
				mTempIndices.Clear();
				return closestCharacter;
			}
		}
		return 0;
	}

	public int GetCharacterIndex(int currentIndex, KeyCode key)
	{
		if (isValid)
		{
			string text = processedText;
			if (string.IsNullOrEmpty(text))
			{
				return 0;
			}
			float num = ((!(mFont != null)) ? 1f : mFont.pixelSize);
			float num2 = mScale * num;
			bool flag = usePrintedSize;
			if (flag)
			{
				UpdateNGUIText(mPrintedSize, mWidth, mHeight);
			}
			else
			{
				UpdateNGUIText(fontSize, Mathf.RoundToInt((float)mWidth / num2), mHeight);
			}
			NGUIText.PrintCharacterPositions(text, mTempVerts, mTempIndices);
			if (mTempVerts.size > 0)
			{
				ApplyOffset(mTempVerts, flag, num2, 0);
				for (int i = 0; i < mTempIndices.size; i++)
				{
					if (mTempIndices[i] == currentIndex)
					{
						Vector2 pos = mTempVerts[i];
						switch (key)
						{
						case KeyCode.UpArrow:
							pos.y += fontSize + spacingY;
							break;
						case KeyCode.DownArrow:
							pos.y -= fontSize + spacingY;
							break;
						case KeyCode.Home:
							pos.x -= 1000f;
							break;
						case KeyCode.End:
							pos.x += 1000f;
							break;
						}
						int closestCharacter = NGUIText.GetClosestCharacter(mTempVerts, pos);
						closestCharacter = mTempIndices[closestCharacter];
						if (closestCharacter == currentIndex)
						{
							break;
						}
						mTempVerts.Clear();
						mTempIndices.Clear();
						return closestCharacter;
					}
				}
				mTempVerts.Clear();
				mTempIndices.Clear();
			}
			switch (key)
			{
			case KeyCode.UpArrow:
			case KeyCode.Home:
				return 0;
			case KeyCode.DownArrow:
			case KeyCode.End:
				return text.Length;
			}
		}
		return currentIndex;
	}

	public void PrintOverlay(int start, int end, UIGeometry caret, UIGeometry highlight, Color caretColor, Color highlightColor)
	{
		if (caret != null)
		{
			caret.Clear();
		}
		if (highlight != null)
		{
			highlight.Clear();
		}
		if (!isValid)
		{
			return;
		}
		string text = processedText;
		float num = ((!(mFont != null)) ? 1f : mFont.pixelSize);
		float num2 = mScale * num;
		bool flag = usePrintedSize;
		if (flag)
		{
			UpdateNGUIText(mPrintedSize, mWidth, mHeight);
		}
		else
		{
			UpdateNGUIText(fontSize, Mathf.RoundToInt((float)mWidth / num2), mHeight);
		}
		int size = caret.verts.size;
		Vector2 item = new Vector2(0.5f, 0.5f);
		float num3 = finalAlpha;
		if (highlight != null && start != end)
		{
			int size2 = highlight.verts.size;
			NGUIText.PrintCaretAndSelection(text, start, end, caret.verts, highlight.verts);
			if (highlight.verts.size > size2)
			{
				ApplyOffset(highlight.verts, flag, num2, size2);
				Color32 item2 = new Color(highlightColor.r, highlightColor.g, highlightColor.b, highlightColor.a * num3);
				for (int i = size2; i < highlight.verts.size; i++)
				{
					highlight.uvs.Add(item);
					highlight.cols.Add(item2);
				}
			}
		}
		else
		{
			NGUIText.PrintCaretAndSelection(text, start, end, caret.verts, null);
		}
		ApplyOffset(caret.verts, flag, num2, size);
		Color32 item3 = new Color(caretColor.r, caretColor.g, caretColor.b, caretColor.a * num3);
		for (int j = size; j < caret.verts.size; j++)
		{
			caret.uvs.Add(item);
			caret.cols.Add(item3);
		}
	}

	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (!isValid)
		{
			return;
		}
		int size = verts.size;
		Color color = base.color;
		color.a = finalAlpha;
		if (mFont != null && mFont.premultipliedAlpha)
		{
			color = NGUITools.ApplyPMA(color);
		}
		string text = processedText;
		float num = ((!(mFont != null)) ? 1f : mFont.pixelSize);
		float num2 = mScale * num;
		bool flag = usePrintedSize;
		int size2 = verts.size;
		if (flag)
		{
			UpdateNGUIText(mPrintedSize, mWidth, mHeight);
		}
		else
		{
			UpdateNGUIText(fontSize, Mathf.RoundToInt((float)mWidth / num2), mHeight);
		}
		NGUIText.tint = color;
		NGUIText.Print(text, verts, uvs, cols);
		Vector2 vector = ApplyOffset(verts, flag, mScale, size2);
		if (effectStyle != 0)
		{
			int size3 = verts.size;
			float num3 = num;
			vector.x = num3 * mEffectDistance.x;
			vector.y = num3 * mEffectDistance.y;
			ApplyShadow(verts, uvs, cols, size, size3, vector.x, 0f - vector.y);
			if (effectStyle == Effect.Outline)
			{
				size = size3;
				size3 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size3, 0f - vector.x, vector.y);
				size = size3;
				size3 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size3, vector.x, vector.y);
				size = size3;
				size3 = verts.size;
				ApplyShadow(verts, uvs, cols, size, size3, 0f - vector.x, 0f - vector.y);
			}
		}
	}

	protected Vector2 ApplyOffset(BetterList<Vector3> verts, bool usePS, float scale, int start)
	{
		Vector2 vector = base.pivotOffset;
		float num = Mathf.Lerp(0f, -mWidth, vector.x);
		float num2 = Mathf.Lerp(mHeight, 0f, vector.y) + Mathf.Lerp(mCalculatedSize.y * scale - (float)mHeight, 0f, vector.y);
		if (usePS || scale == 1f)
		{
			for (int i = start; i < verts.size; i++)
			{
				verts.buffer[i].x += num;
				verts.buffer[i].y += num2;
			}
		}
		else
		{
			for (int j = start; j < verts.size; j++)
			{
				verts.buffer[j].x = num + verts.buffer[j].x * scale;
				verts.buffer[j].y = num2 + verts.buffer[j].y * scale;
			}
		}
		return new Vector2(num, num2);
	}

	private void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
	{
		Color color = mEffectColor;
		color.a *= finalAlpha;
		Color32 color2 = ((!(bitmapFont != null) || !bitmapFont.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 vector = verts.buffer[i];
			vector.x += x;
			vector.y += y;
			verts.buffer[i] = vector;
			cols.buffer[i] = color2;
		}
	}

	public int CalculateOffsetToFit(string text)
	{
		UpdateNGUIText();
		NGUIText.encoding = false;
		NGUIText.symbolStyle = NGUIText.SymbolStyle.None;
		return NGUIText.CalculateOffsetToFit(text);
	}

	public void SetCurrentProgress()
	{
		if (UIProgressBar.current != null)
		{
			text = UIProgressBar.current.value.ToString("F");
		}
	}

	public void SetCurrentPercent()
	{
		if (UIProgressBar.current != null)
		{
			text = Mathf.RoundToInt(UIProgressBar.current.value * 100f) + "%";
		}
	}

	public void SetCurrentSelection()
	{
		if (UIPopupList.current != null)
		{
			text = ((!UIPopupList.current.isLocalized) ? UIPopupList.current.value : Localization.Localize(UIPopupList.current.value));
		}
	}

	public bool Wrap(string text, out string final)
	{
		return Wrap(text, out final, 1000000);
	}

	public bool Wrap(string text, out string final, int height)
	{
		UpdateNGUIText(fontSize, mWidth, height);
		return NGUIText.WrapText(text, out final);
	}

	public void UpdateNGUIText()
	{
		UpdateNGUIText(fontSize, mWidth, mHeight);
	}

	public void UpdateNGUIText(int size, int lineWidth, int lineHeight)
	{
		NGUIText.size = size;
		NGUIText.style = mFontStyle;
		NGUIText.lineWidth = lineWidth;
		NGUIText.lineHeight = lineHeight;
		NGUIText.gradient = mApplyGradient;
		NGUIText.gradientTop = mGradientTop;
		NGUIText.gradientBottom = mGradientBottom;
		NGUIText.encoding = mEncoding;
		NGUIText.premultiply = mPremultiply;
		NGUIText.symbolStyle = mSymbols;
		NGUIText.spacingX = Mathf.RoundToInt(mScale * (float)mSpacingX);
		NGUIText.spacingY = Mathf.RoundToInt(mScale * (float)mSpacingY);
		NGUIText.maxLines = mMaxLineCount;
		if (mFont != null)
		{
			NGUIText.bitmapFont = mFont;
			while (true)
			{
				UIFont replacement = NGUIText.bitmapFont.replacement;
				if (replacement == null)
				{
					break;
				}
				NGUIText.bitmapFont = replacement;
			}
			if (NGUIText.bitmapFont.isDynamic)
			{
				NGUIText.dynamicFont = NGUIText.bitmapFont.dynamicFont;
				NGUIText.bitmapFont = null;
			}
			else
			{
				NGUIText.dynamicFont = null;
			}
		}
		else
		{
			NGUIText.dynamicFont = mTrueTypeFont;
			NGUIText.bitmapFont = null;
		}
		if (NGUIText.dynamicFont != null)
		{
			UIRoot uIRoot = base.root;
			NGUIText.pixelDensity = ((!usePrintedSize || !(uIRoot != null)) ? 1f : (1f / uIRoot.pixelSizeAdjustment));
		}
		else
		{
			NGUIText.pixelDensity = 1f;
		}
		switch (base.pivot)
		{
		case Pivot.TopLeft:
		case Pivot.Left:
		case Pivot.BottomLeft:
			NGUIText.alignment = TextAlignment.Left;
			break;
		case Pivot.TopRight:
		case Pivot.Right:
		case Pivot.BottomRight:
			NGUIText.alignment = TextAlignment.Right;
			break;
		default:
			NGUIText.alignment = TextAlignment.Center;
			break;
		}
		NGUIText.Update();
	}
}
