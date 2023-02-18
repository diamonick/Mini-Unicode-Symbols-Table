using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MiniUnicodeSymbolsTable.Editor;

public class MiniUnicodeSymbolsTableEditor : EditorWindow
{
    #region Enums
    public enum UnicodeCategory
    {
        All = 0,
        ASCII = 1,
        Currency = 2,
        GreekLetters = 3,
        RomanNumerals = 4,
        Punctuation = 5,
        Math = 6,
        GeometricShapes = 7,
        Arrows = 8,
        Zodiac = 9,
        Planets = 10,
        Favorites = 11
    }
    #endregion

    private Dictionary<char, string> unicodeNames = new Dictionary<char, string>()
    {
        {(char)32, "Space"},
        {(char)33, "Exclamation point"},
        {(char)34, ""},
        {(char)35, ""},
        {(char)36, ""},
        {(char)37, ""},
        {(char)38, ""},
        {(char)39, ""},
        {(char)40, ""},
        {(char)41, ""},
        {(char)42, ""},
        {(char)43, ""},
        {(char)44, ""},
        {(char)45, ""},
        {(char)46, ""},
        {(char)47, ""},
        {(char)48, ""},
        {(char)49, ""},
        {(char)50, ""},
        {(char)51, ""},
        {(char)52, ""},
        {(char)53, ""},
        {(char)54, ""},
        {(char)55, ""},
        {(char)56, ""},
        {(char)57, ""},
        {(char)58, ""},
        {(char)59, ""},
        {(char)60, ""},
        {(char)61, ""},
        {(char)62, ""},
        {(char)63, ""},
        {(char)64, ""},
        {(char)65, ""},
        {(char)66, ""},
        {(char)67, ""},
        {(char)68, ""},
        {(char)69, ""},
        {(char)70, ""},
        {(char)71, ""},
        {(char)72, ""},
        {(char)73, ""},
        {(char)74, ""},
        {(char)75, ""},
        {(char)76, ""},
        {(char)77, ""},
        {(char)78, ""},
        {(char)79, ""},
        {(char)80, ""},
        {(char)81, ""},
        {(char)82, ""},
        {(char)83, ""},
        {(char)84, ""},
        {(char)85, ""},
        {(char)86, ""},
        {(char)87, ""},
        {(char)88, ""},
        {(char)89, ""},
        {(char)90, ""},
        {(char)91, ""},
        {(char)92, ""},
        {(char)93, ""},
        {(char)94, ""},
        {(char)95, ""},
        {(char)96, ""},
        {(char)97, ""},
        {(char)98, ""},
        {(char)99, ""},
        {(char)100, ""},
        {(char)101, ""},
        {(char)102, ""},
        {(char)103, ""},
        {(char)104, ""},
        {(char)105, ""},
        {(char)106, ""},
        {(char)107, ""},
        {(char)108, ""},
        {(char)109, ""},
        {(char)110, ""},
        {(char)111, ""},
    };

    private static MiniUnicodeSymbolsTableEditor window;    // Editor window
    private Vector2 scrollPosition;                         // Current scroll position
    private Vector2 categoryScrollPos;                      // Current scroll position

    // Icon paths
    // Note: Make sure to import the package(s) under Assets to have all icons display properly in the edito window.
    private readonly string copyIconPath = "Assets/Mini Unicode Symbols Table/Textures/CopyIcon.png";

    private string copyTooltip = "Click to copy.";
    private string copySymbolTooltip = "Click this to copy the previewed Unicode symbol and paste it anywhere.";

    private GUIStyle symbolButtonStyle;
    private GUIStyle copyButtonStyle;

    private bool infoFoldout = false;
    private bool settingsFoldout = false;

    private UnicodeCategory unicodeCategory = UnicodeCategory.ASCII;

    private List<UnicodeSymbol> unicodeSymbols;
    private List<UnicodeSymbol> favoriteUnicodeSymbols;
    private bool unicodeSymbolsInitialized = false;
    private readonly int rowCount = 10;
    private readonly int columnCount = 100;
    private UnicodeSymbol selectedSymbol = null;
    private UnicodeSymbol favoriteSymbolPending = null;
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

    private void OnEnable()
    {
        if (favoriteUnicodeSymbols == null)
        {
            favoriteUnicodeSymbols = new List<UnicodeSymbol>();
            //if (!MUSTEditorPrefs.HasKey($"Favorite Symbol[{favoriteUnicodeSymbols.Count}]"))
            //{

            //}

            for (int i = 0; i < 50; i++)
            {
                if (!MUSTEditorPrefs.HasKey(GetFavoriteSymbolKey(i)))
                    break;

                char ch = GetFavoriteSymbolKey(i)[0];
                UnicodeSymbol us = new UnicodeSymbol(ch);
                if (us == null)
                    continue;

                favoriteUnicodeSymbols.Add(us);
                Debug.Log(MUSTEditorPrefs.GetString(GetFavoriteSymbolKey(i)));
            }
        }
    }

    /// <summary>
    /// Editor GUI.
    /// </summary>
    private void OnGUI()
    {
        window = GetWindow<MiniUnicodeSymbolsTableEditor>("Mini Unicode Symbols Table (MUST) V1.0");

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

        GUIStyle categoryButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 10,
            stretchWidth = false,
            fixedWidth = 64,
            fixedHeight = 64,
            alignment = TextAnchor.MiddleCenter
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

        GUIStyle previewHeaderStyle = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Normal,
            fontSize = 12,
            fixedHeight = 18,
            stretchWidth = true,
            stretchHeight = false,
            alignment = TextAnchor.MiddleCenter
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

        GUILayout.BeginVertical();
        #region Preview Box
        GUI.backgroundColor = AddColor("#0062ff") * 3f;
        GUILayout.BeginVertical(GUI.skin.box);
        GUI.backgroundColor = Color.white;
        symbolPreviewStyle.normal.textColor = Color.white;
        GUILayout.Label(selectedSymbol.character.ToString(), symbolPreviewStyle, GUILayout.Height(96f));
        GUI.backgroundColor = AddColor("#0062ff") * 3f;
        previewHeaderStyle.normal.textColor = Color.white;
        GUILayout.Label(CharToUnicode(selectedSymbol.character), previewHeaderStyle);
        GUI.backgroundColor = Color.white;
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
        var copyIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(copyIconPath, typeof(Texture2D));
        EditorGUIUtility.SetIconSize(new Vector2(24f, 24f));

        GUIContent copyContent = new GUIContent(" Copy", copyIcon, copySymbolTooltip);
        GUI.backgroundColor = AddColor("#0062ff") * 1.75f;
        bool onCopyClick = GUILayout.Button(copyContent, copyButtonStyle);
        GUI.backgroundColor = Color.white;
        if (onCopyClick)
        {
            ShowCopyContextMenu(selectedSymbol);
            CopyToClipboard(selectedSymbol.character.ToString());
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

        #region Symbol Information
        GUILayout.BeginVertical(EditorStyles.helpBox);
        infoFoldout = EditorGUILayout.Foldout(infoFoldout, "ⓘ Symbol Information", true);
        if (infoFoldout)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            //GUI.contentColor = isEnabled ? AddColor(Color.white) : Color.white;
            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Symbol: ", GUILayout.ExpandWidth(false));
            GUILayout.Label($"{selectedSymbol.character}", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Name: ", GUILayout.ExpandWidth(false));
            GUILayout.Label($" ", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Unicode Number: ", GUILayout.ExpandWidth(false));
            GUI.backgroundColor = AddColor("#0062ff") * 2f;
            GUI.contentColor = AddColor(Color.white);
            GUIContent unicodeNumContent = new GUIContent(CharToUnicode(selectedSymbol.character), copyTooltip);
            if (GUILayout.Button(unicodeNumContent, GUILayout.ExpandWidth(false)))
            {
                CopyToClipboard(unicodeNumContent.text);
            }
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Decimal: {CharToDecimal(selectedSymbol.character)}");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Hexadecimal: {UnicodeToHexCode(selectedSymbol.character)}");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Octal: {CharToOctal(selectedSymbol.character)}");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"HTML Code: ", GUILayout.ExpandWidth(false));
            GUI.backgroundColor = AddColor("#0062ff") * 2f;
            GUI.contentColor = AddColor(Color.white);
            GUIContent htmlCodeContent = new GUIContent(CharToHTML(selectedSymbol.character), copyTooltip);
            if (GUILayout.Button(htmlCodeContent, GUILayout.ExpandWidth(false)))
            {
                CopyToClipboard(htmlCodeContent.text);
            }
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"CSS Code: ", GUILayout.ExpandWidth(false));
            GUI.backgroundColor = AddColor("#0062ff") * 2f;
            GUI.contentColor = AddColor(Color.white);
            GUIContent cssCodeContent = new GUIContent(CharToCSS(selectedSymbol.character), copyTooltip);
            if (GUILayout.Button(cssCodeContent, GUILayout.ExpandWidth(false)))
            {
                CopyToClipboard(cssCodeContent.text);
            }
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            GUI.contentColor = Color.white;
            GUILayout.EndVertical();
        }
        #endregion

        GUILayout.EndVertical();
        #endregion
        #region Settings
        GUILayout.BeginVertical(EditorStyles.helpBox);
        settingsFoldout = EditorGUILayout.Foldout(settingsFoldout, "ⓘ Settings", true);
        if (settingsFoldout)
        {

        }
        GUILayout.EndVertical();
        #endregion

        GUILayout.FlexibleSpace();
        DrawLine(GetColorFromHexString("#555555"), 1, 4f);

        GUI.backgroundColor = AddColor("#0062ff") * 2f;

        GUIStyle categoryStyle = new GUIStyle(GUI.skin.button)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16,
            fixedHeight = 24,
            stretchWidth = true,
            stretchHeight = false,
            alignment = TextAnchor.MiddleCenter
        };

        string[] categoryNames = new string[]
        {
            "All ()",
            "ASCII (128)",
            "Currency ()",
            "Greek Letters ()",
            "Roman Numerals ()",
            "Punctuation ()",
            "Math ()",
            "Geometric Shapes ()",
            "Arrows ()",
            "Zodiac ()",
            "Planets ()",
            "Miscellaneous ()",
            $"★ Favorites ({favoriteUnicodeSymbols.Count})"
        };
        unicodeCategory = (UnicodeCategory)EditorGUILayout.Popup((int)unicodeCategory, categoryNames);
        GUI.backgroundColor = Color.white;

        GUILayout.Space(8f);

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
        GUILayout.EndVertical();
    }

    /// <summary>
    /// Initialize list of Unicode symbols.
    /// </summary>
    private void InitializeUnicodeSymbols()
    {
        // Initialize Unicode symbols list ONLY when the user first opens the editor window.
        //if (unicodeSymbolsInitialized)
        //    return;

        char startChar = ' ';
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

    /// <summary>
    /// Copies a string to the Clipboard.
    /// </summary>
    public static void CopyToClipboard(string s)
    {
        GUIUtility.systemCopyBuffer = s;

        // Display quick notification.
        window.ShowNotification(new GUIContent($"{s}\n\nCopied!"));
    }

    public void CopySymbol() => CopyToClipboard(selectedSymbol.character.ToString());
    public void CopyUnicode() => CopyToClipboard(CharToUnicode(selectedSymbol.character));
    public void CopyHexCode() => CopyToClipboard(CharToHTML(selectedSymbol.character));
    public void CopyCSSCode() => CopyToClipboard(CharToCSS(selectedSymbol.character));

    public void ShowCopyContextMenu(UnicodeSymbol us)
    {
        // Get current event.
        Event current = Event.current;

        if (current.button == 1)
        {
            GenericMenu menu = new GenericMenu();
            AddMenuItem(menu, "Copy Symbol", CopySymbol);
            AddMenuItem(menu, "Copy Unicode", CopyUnicode);
            AddMenuItem(menu, "Copy HTML Code", CopyHexCode);
            AddMenuItem(menu, "Copy CSS Code", CopyCSSCode);
            menu.ShowAsContext();

            current.Use();
        }
    }

    public void ShowSymbolContextMenu(UnicodeSymbol us)
    {
        // Get current event.
        Event current = Event.current;

        if (current.button == 1)
        {
            GenericMenu menu = new GenericMenu();
            favoriteSymbolPending = us;
            AddMenuItem(menu, "★ Add to Favorites", AddToFavorites);
            menu.ShowAsContext();

            current.Use();
        }
    }

    /// <summary>
    /// Add
    /// </summary>
    public void AddToFavorites()
    {
        if (favoriteUnicodeSymbols.Contains(favoriteSymbolPending))
            return;

        char character = favoriteSymbolPending.character;
        MUSTEditorPrefs.SetString(GetFavoriteSymbolKey(favoriteUnicodeSymbols.Count), favoriteSymbolPending.character.ToString());
        favoriteUnicodeSymbols.Add(favoriteSymbolPending);

        // Display quick notification.
        window.ShowNotification(new GUIContent($"{character}\n\nAdded to\n★ Favorites!"));
    }

    public string GetFavoriteSymbolKey(int value) => $"Favorite Symbol[{value}]";

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

    /// <summary>
    /// Convert a character from char to HTML code.
    /// </summary>
    /// <param name="ch">Character.</param>
    /// <returns>Formatted Unicode number.</returns>
    public static string CharToHTML(char ch)
    {
        return "&#" + CharToDecimal(ch) + ";";
    }

    /// <summary>
    /// Convert a character from char to CSS code.
    /// </summary>
    /// <param name="ch">Character.</param>
    /// <returns>Formatted Unicode number.</returns>
    public static string CharToCSS(char ch)
    {
        return "\\" + UnicodeToHexCode(ch);
    }
    #endregion

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

            ShowSymbolContextMenu(us);
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

    /// <summary>
    /// Draw bullet point: "•"
    /// </summary>
    /// <param name="bulletPointColor">Bullet point color string (Hexadecimal).</param>
    /// <param name="indents">Indention level. Default: 0</param>
    private static void DrawBulletPoint(string bulletPointColor, int indents = 0)
    {
        // GUI Style: Bullet Point
        GUIStyle bulletPointStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12,
            stretchWidth = true,
            fixedWidth = 12 + (24 * indents),
            contentOffset = new Vector2(24 * indents, 0f)
        };

        // Draw bullet point w/ the specified color.
        GUI.contentColor = AddColor(bulletPointColor);
        GUILayout.Label("•", bulletPointStyle);
        GUI.contentColor = Color.white;
    }
    #endregion

    #region Custom Context Menus
    /// <summary>
    /// Add new menu item to a context menu.
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="menuPath"></param>
    /// <param name="color"></param>
    private void AddMenuItem(GenericMenu menu, string menuPath, GenericMenu.MenuFunction method)
    {
        menu.AddItem(new GUIContent(menuPath), false, method);
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

    private void OnDisable()
    {
        // Save user's favorite symbols to EditorPrefs.
        for (int i = 0; i < favoriteUnicodeSymbols.Count; i++)
        {
            Debug.Log(MUSTEditorPrefs.GetString(GetFavoriteSymbolKey(i)));
        }
    }
}
