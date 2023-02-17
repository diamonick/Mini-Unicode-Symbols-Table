using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MiniUnicodeSymbolsTableEditor : EditorWindow
{
    private static MiniUnicodeSymbolsTableEditor window;    // Editor window
    private Vector2 scrollPosition;                         // Current scroll position

    private string copyTooltip = "Click this to copy the previewed Unicode symbol and paste it anywhere.";

    private GUIStyle symbolButtonStyle;
    private GUIStyle copyButtonStyle;

    private bool infoFoldout = false;

    private List<UnicodeSymbol> unicodeSymbols;
    private bool unicodeSymbolsInitialized = false;
    private readonly int rowCount = 5;
    private readonly int columnCount = 100;
    private UnicodeSymbol selectedSymbol = null;
    private int selectedIndex = 0;

    /// <summary>
    /// Display the Mini Unicode Symbols Table menu item. (Tools -> Mini Unicode Symbols Table)
    /// Keyboard Shortcut: ctrl-alt-D (Windows), cmd-alt-D (macOS).
    /// </summary>
    [MenuItem("Tools/Mini Unicode Symbols Table", false, 10)]
    public static void DisplayWindow()
    {
        window = GetWindow<MiniUnicodeSymbolsTableEditor>("Mini Unicode Symbols Table (MUST) V1.0");
    }

    /// <summary>
    /// Editor GUI.
    /// </summary>
    private void OnGUI()
    {

        // Initialize GUI style for a character symbol button.
        symbolButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fixedWidth = 28,
            fixedHeight = 28,
        };

        copyButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 24,
            stretchWidth = true,
            fixedHeight = 36
        };

        GUIStyle sideButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16,
            stretchWidth = false,
            fixedWidth = 36,
            fixedHeight = 36,
            alignment = TextAnchor.MiddleCenter
        };

        GUIStyle previewHeaderStyle = new GUIStyle(GUI.skin.box)
        {
            fontStyle = FontStyle.Normal,
            fontSize = 12,
            fixedHeight = 18,
            stretchWidth = true,
            stretchHeight = false
        };

        GUIStyle symbolPreviewStyle = new GUIStyle(GUI.skin.box)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 72,
            stretchWidth = true,
            stretchHeight = false,
            alignment = TextAnchor.MiddleCenter
        };

        InitializeUnicodeSymbols();

        #region Preview Box
        GUI.backgroundColor = AddColor("#0062ff") * 2.5f;
        GUILayout.BeginVertical(GUI.skin.box);
        #region Header
        GUILayout.Box("Preview", previewHeaderStyle);
        GUI.backgroundColor = Color.white;
        #endregion

        GUILayout.Label(selectedSymbol.character.ToString(), symbolPreviewStyle, GUILayout.Height(96f));
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        #region Previous Symbol
        GUIContent prevSymbolContent = new GUIContent("◀", "");
        GUI.backgroundColor = AddColor("#0062ff") * 1.75f;
        bool onPrevSymbolClick = GUILayout.Button(prevSymbolContent, sideButtonStyle);
        GUI.backgroundColor = Color.white;
        if (onPrevSymbolClick)
        {
            PreviousSymbol();
        }
        #endregion

        #region Copy
        GUIContent copyContent = new GUIContent("Copy", copyTooltip);
        GUI.backgroundColor = AddColor("#0062ff") * 1.75f;
        bool onCopyClick = GUILayout.Button(copyContent, copyButtonStyle);
        GUI.backgroundColor = Color.white;
        if (onCopyClick)
        {
            CopySymbolToClipboard(selectedSymbol.character);
        }
        #endregion

        #region Next Symbol
        GUIContent nextSymbolContent = new GUIContent("▶", "");
        GUI.backgroundColor = AddColor("#0062ff") * 1.75f;
        bool onNextSymbolClick = GUILayout.Button(nextSymbolContent, sideButtonStyle);
        GUI.backgroundColor = Color.white;
        if (onNextSymbolClick)
        {
            NextSymbol();
        }
        #endregion
        GUILayout.EndHorizontal();

        DrawLine(GetColorFromHexString("#555555"), 1, 4f);

        #region Information
        infoFoldout = EditorGUILayout.Foldout(infoFoldout, "Info", true);
        if (infoFoldout)
        {
            EditorGUILayout.HelpBox($"Symbol: {selectedSymbol.character}\n" +
                                    $"Name: \n" +
                                    $"Unicode Number: {CharToUnicode(selectedSymbol.character)}\n" +
                                    $"Decimal: {CharToDecimal(selectedSymbol.character)}\n" +
                                    $"Hexadecimal: {UnicodeToHexCode(selectedSymbol.character)}\n" +
                                    $"Octal: {CharToOctal(selectedSymbol.character)}\n" +
                                    $"HTML Code: \n" +
                                    $"CSS Code: ", MessageType.Info);
        }
        #endregion
        #endregion



        DrawLine(GetColorFromHexString("#555555"), 1, 4f);

        // Update scroll position in the editor window.
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, false, GUI.skin.horizontalScrollbar, GUIStyle.none);
        for (int y = 0; y < rowCount; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < columnCount; x++)
            {
                int index = x + (columnCount * y);
                UnicodeSymbol us = unicodeSymbols[index];
                DrawSymbolButton(us);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// Initialize list of Unicode symbols.
    /// </summary>
    private void InitializeUnicodeSymbols()
    {
        // Initialize Unicode symbols list ONLY when the user first opens the editor window.
        //if (unicodeSymbolsInitialized)
        //    return;

        char startChar = '¡';
        int numOfUnicodeSymbols = rowCount * columnCount;

        // Initialize list.
        unicodeSymbols = new List<UnicodeSymbol>();

        for (int i = 0; i < numOfUnicodeSymbols; i++)
        {
            UnicodeSymbol us = new UnicodeSymbol(startChar++);
            if (unicodeSymbols.Contains(us))
                continue;

            unicodeSymbols.Add(us);
        }

        selectedSymbol = unicodeSymbols[selectedIndex];
        //Debug.Log($"Unicode Count: {unicodeSymbols.Count}");

        unicodeSymbolsInitialized = true;
    }

    /// <summary>
    /// Get the next Unicode symbol.
    /// </summary>
    private void NextSymbol()
    {
        selectedIndex = Mathf.Clamp(selectedIndex + 1, 0, unicodeSymbols.Count - 1);
        selectedSymbol = unicodeSymbols[selectedIndex];
    }

    /// <summary>
    /// Get the previous Unicode symbol.
    /// </summary>
    private void PreviousSymbol()
    {
        selectedIndex = Mathf.Clamp(selectedIndex - 1, 0, unicodeSymbols.Count - 1);
        selectedSymbol = unicodeSymbols[selectedIndex];
    }

    #region Conversion(s)
    /// <summary>
    /// Convert a character from char to octal number.
    /// </summary>
    /// <param name="ch">Character.</param>
    /// <returns>Formatted octal number.</returns>
    public static string CharToOctal(char ch)
    {
        int decimalValue = (int)ch;
        return Convert.ToString(decimalValue, 8);
    }

    /// <summary>
    /// Convert a character from char to decimal number.
    /// </summary>
    /// <param name="ch">Character.</param>
    /// <returns>Formatted decimal number.</returns>
    public static string CharToDecimal(char ch)
    {
        int decimalValue = (int)ch;
        return decimalValue.ToString();
    }

    /// <summary>
    /// Convert a character from Unicode number to hex code (Hexadecimal).
    /// </summary>
    /// <param name="ch">Character.</param>
    /// <returns>The hexadecimal string.</returns>
    public static string UnicodeToHexCode(char ch)
    {
        return string.Format("{0:X4}", (int)ch);
    }

    /// <summary>
    /// Convert a character from char to Unicode number.
    /// </summary>
    /// <param name="ch">Character.</param>
    /// <returns>Formatted Unicode number.</returns>
    public static string CharToUnicode(char ch)
    {
        return "U+" + UnicodeToHexCode(ch);
    }
    #endregion

    /// <summary>
    /// Copies the Unicode character symbol to the Clipboard.
    /// </summary>
    public static void CopySymbolToClipboard(char ch)
    {
        GUIUtility.systemCopyBuffer = ch.ToString();
        Debug.Log($"Copied: {ch}");
    }

    #region Draw Method(s)
    /// <summary>
    /// Draw the specified Unicode character symbol on a button.
    /// </summary>
    /// <param name="symbolCharacter">Unicode character symbol.</param>
    private void DrawSymbolButton(UnicodeSymbol us)
    {
        if (GUILayout.Toggle(false, us.character.ToString(), symbolButtonStyle))
        {
            selectedIndex = unicodeSymbols.IndexOf(us);
            selectedSymbol = us;
        }
    }

    /// <summary>
    /// Draw a line in the editor window.
    /// </summary>
    /// <param name="lineColor">Line color.</param>
    /// <param name="height">Line height.</param>
    /// <param name="spacing">Spacing.</param>
    protected static void DrawLine(Color lineColor, int height, float spacing)
    {
        GUIStyle horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset(4, 4, height, height);
        horizontalLine.fixedHeight = height;

        GUILayout.Space(spacing);

        var c = GUI.color;
        GUI.color = lineColor;
        GUILayout.Box(GUIContent.none, horizontalLine);
        GUI.color = c;

        GUILayout.Space(spacing);
    }
    #endregion

    #region Miscellaneous
    /// <summary>
    /// Toggle wide mode for the Editor GUI.
    /// </summary>
    /// <param name="labelWidth">Minimum width (in pixels) for all labels.</param>
    private static void ToggleWideMode(float labelWidth)
    {
        EditorGUIUtility.wideMode = !EditorGUIUtility.wideMode;
        EditorGUIUtility.fieldWidth = 72;
        EditorGUIUtility.labelWidth = labelWidth;
    }

    /// <summary>
    /// Get color from hex string.
    /// </summary>
    /// <param name="hexColor">Hex color string.</param>
    /// <returns>New color.</returns>
    private static Color GetColorFromHexString(string hexColor)
    {
        Color color = Color.white;
        ColorUtility.TryParseHtmlString(hexColor, out color);
        return color;
    }

    /// <summary>
    /// Add color to existing color.
    /// </summary>
    /// <param name="color">Added color.</param>
    /// <returns>New color.</returns>
    private static Color AddColor(Color color)
    {
        color += color;
        return color;
    }

    /// <summary>
    /// Add color to existing color.
    /// </summary>
    /// <param name="hexColor">Hex color string.</param>
    /// <returns>New color.</returns>
    private static Color AddColor(string hexColor)
    {
        Color color = Color.white;
        ColorUtility.TryParseHtmlString(hexColor, out color);
        color += color;

        return color;
    }
    #endregion
}
