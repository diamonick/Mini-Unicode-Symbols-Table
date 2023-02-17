using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MiniUnicodeSymbolsTableEditor : EditorWindow
{
    private static MiniUnicodeSymbolsTableEditor window;    // Editor window
    private Vector2 scrollPosition;                         // Current scroll position

    private string copyTooltip = "Click this to copy the previewed Unicode symbol and paste it anywhere.";

    //private char startChar;
    private GUIStyle symbolButtonStyle;
    private GUIStyle copyButtonStyle;

    private List<UnicodeSymbol> unicodeSymbols;
    private bool unicodeSymbolsInitialized = false;
    private readonly int rowCount = 10;
    private readonly int columnCount = 10;
    private char currentSymbolPreview = ' ';

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
            stretchHeight = false
        };

        InitializeUnicodeSymbols();

        GUIContent copyContent = new GUIContent("Copy", copyTooltip);
        EditorGUILayout.SelectableLabel(currentSymbolPreview.ToString(), symbolPreviewStyle, GUILayout.Height(96f));
        GUI.backgroundColor = AddColor("#297bff") * 2;
        GUILayout.Box("Preview", previewHeaderStyle);
        GUI.backgroundColor = Color.white;

        DrawLine(GetColorFromHexString("#555555"), 1, 4f);

        GUILayout.Button(copyContent, copyButtonStyle);

        DrawLine(GetColorFromHexString("#555555"), 1, 4f);

        // Update scroll position in the editor window.
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, false, GUI.skin.horizontalScrollbar, GUIStyle.none);
        for (int y = 0; y < rowCount; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < columnCount; x++)
            {
                int index = x + (rowCount * y);
                char character = unicodeSymbols[index].character;
                DrawSymbolButton(character);
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
        //Debug.Log($"Unicode Count: {unicodeSymbols.Count}");

        unicodeSymbolsInitialized = true;
    }

    /// <summary>
    /// Draw the specified Unicode character symbol on a button.
    /// </summary>
    /// <param name="symbolCharacter">Unicode character symbol.</param>
    private void DrawSymbolButton(char symbolCharacter)
    {
        if (GUILayout.Toggle(false, symbolCharacter.ToString(), symbolButtonStyle))
        {
            currentSymbolPreview = symbolCharacter;
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
