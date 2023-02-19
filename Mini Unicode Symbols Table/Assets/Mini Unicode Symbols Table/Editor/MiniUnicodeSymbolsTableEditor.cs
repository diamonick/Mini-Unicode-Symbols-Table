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
        Miscellaneous = 11,
        Favorites = 12
    }
    #endregion

    private readonly Dictionary<char, string> ASCIIPrintableNames = new Dictionary<char, string>()
    {
        #region ASCII-Printable
        {(char)32, "Space"},
        {(char)33, "Exclamation point"},
        {(char)34, "Quotation mark"},
        {(char)35, "Number sign"},
        {(char)36, "Dollar sign"},
        {(char)37, "Percent sign"},
        {(char)38, "Ampersand"},
        {(char)39, "Apostrophe"},
        {(char)40, "Left parenthesis"},
        {(char)41, "Right parenthesis"},
        {(char)42, "Asterisk"},
        {(char)43, "Plus sign"},
        {(char)44, "Comma"},
        {(char)45, "Hyphen-minus"},
        {(char)46, "Full stop"},
        {(char)47, "Slash (Solidus)"},
        {(char)48, "Digit Zero"},
        {(char)49, "Digit One"},
        {(char)50, "Digit Two"},
        {(char)51, "Digit Three"},
        {(char)52, "Digit Four"},
        {(char)53, "Digit Five"},
        {(char)54, "Digit Six"},
        {(char)55, "Digit Seven"},
        {(char)56, "Digit Eight"},
        {(char)57, "Digit Nine"},
        {(char)58, "Colon"},
        {(char)59, "Semicolon"},
        {(char)60, "Less-than sign"},
        {(char)61, "Equal sign"},
        {(char)62, "Greater-than sign"},
        {(char)63, "Question mark"},
        {(char)64, "At sign"},
        {(char)65, "Latin Capital letter A"},
        {(char)66, "Latin Capital letter B"},
        {(char)67, "Latin Capital letter C"},
        {(char)68, "Latin Capital letter D"},
        {(char)69, "Latin Capital letter E"},
        {(char)70, "Latin Capital letter F"},
        {(char)71, "Latin Capital letter G"},
        {(char)72, "Latin Capital letter H"},
        {(char)73, "Latin Capital letter I"},
        {(char)74, "Latin Capital letter J"},
        {(char)75, "Latin Capital letter K"},
        {(char)76, "Latin Capital letter L"},
        {(char)77, "Latin Capital letter M"},
        {(char)78, "Latin Capital letter N"},
        {(char)79, "Latin Capital letter O"},
        {(char)80, "Latin Capital letter P"},
        {(char)81, "Latin Capital letter Q"},
        {(char)82, "Latin Capital letter R"},
        {(char)83, "Latin Capital letter S"},
        {(char)84, "Latin Capital letter T"},
        {(char)85, "Latin Capital letter U"},
        {(char)86, "Latin Capital letter V"},
        {(char)87, "Latin Capital letter W"},
        {(char)88, "Latin Capital letter X"},
        {(char)89, "Latin Capital letter Y"},
        {(char)90, "Latin Capital letter Z"},
        {(char)91, "Left Square Bracket"},
        {(char)92, "Backslash"},
        {(char)93, "Right Square Bracket"},
        {(char)94, "Circumflex accent"},
        {(char)95, "Low line"},
        {(char)96, "Grave accent"},
        {(char)97, "Latin Small letter A"},
        {(char)98, "Latin Small letter B"},
        {(char)99, "Latin Small letter C"},
        {(char)100, "Latin Small letter D"},
        {(char)101, "Latin Small letter E"},
        {(char)102, "Latin Small letter F"},
        {(char)103, "Latin Small letter G"},
        {(char)104, "Latin Small letter H"},
        {(char)105, "Latin Small letter I"},
        {(char)106, "Latin Small letter J"},
        {(char)107, "Latin Small letter K"},
        {(char)108, "Latin Small letter L"},
        {(char)109, "Latin Small letter M"},
        {(char)110, "Latin Small letter N"},
        {(char)111, "Latin Small letter O"},
        {(char)112, "Latin Small letter P"},
        {(char)113, "Latin Small letter Q"},
        {(char)114, "Latin Small letter R"},
        {(char)115, "Latin Small letter S"},
        {(char)116, "Latin Small letter T"},
        {(char)117, "Latin Small letter U"},
        {(char)118, "Latin Small letter V"},
        {(char)119, "Latin Small letter W"},
        {(char)120, "Latin Small letter X"},
        {(char)121, "Latin Small letter Y"},
        {(char)122, "Latin Small letter Z"},
        {(char)123, "Left Curly Bracket"},
        {(char)124, "Vertical bar"},
        {(char)125, "Right Curly Bracket"},
        {(char)126, "Tilde"},
        #endregion
    };

    private static MiniUnicodeSymbolsTableEditor window;    // Editor window
    private Vector2 scrollPosition;                         // Current scroll position

    // Icon paths
    // Note: Make sure to import the package(s) under Assets to have all icons display properly in the edito window.
    private readonly string copyIconPath = "Assets/Mini Unicode Symbols Table/Textures/CopyIcon.png";

    private string copyTooltip = "Click to copy.";
    private string copySymbolTooltip = "Click this to copy the previewed Unicode symbol and paste it anywhere.";
    private string favoriteButtonTooltip = "Click this to add/remove this Unicode symbol to and from ★ Favorites.";

    private GUIStyle symbolButtonStyle;
    private GUIStyle copyButtonStyle;

    private bool infoFoldout = false;
    private bool settingsFoldout = false;

    private UnicodeCategory unicodeCategory = UnicodeCategory.ASCII;

    private List<char> unicodeSymbols;
    private List<char> favoriteUnicodeSymbols;
    private bool unicodeSymbolsInitialized = false;
    private readonly int rowCount = 10;
    private readonly int columnCount = 100;
    private readonly int maxNumOfFavoriteSymbols = 50;
    private char selectedSymbol = ' ';
    private char favoriteSymbolPending = ' ';
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
            favoriteUnicodeSymbols = new List<char>();

            for (int i = 0; i < maxNumOfFavoriteSymbols; i++)
            {
                if (!MUSTEditorPrefs.HasKey(GetFavoriteSymbolKey(i)))
                    continue;

                char ch = (char)MUSTEditorPrefs.GetInt(GetFavoriteSymbolKey(i));
                Debug.Log(ch);
                if (ch == null)
                    continue;

                favoriteUnicodeSymbols.Add(ch);
            }
        }
    }

    /// <summary>
    /// Editor GUI.
    /// </summary>
    private void OnGUI()
    {
        // Get window.
        if (window == null)
        {
            window = GetWindow<MiniUnicodeSymbolsTableEditor>("Mini Unicode Symbols Table (MUST) V1.0");
        }

        // Set minimum & maximum size of the editor window.
        window.minSize = new Vector2(312f, 464f);

        // Initialize GUI style for a character symbol button.
        symbolButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fixedWidth = 28,
            fixedHeight = 28,
        };

        GUIStyle favoriteButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 20,
            fixedWidth = 28,
            fixedHeight = 28,
        };

        GUIStyle tabButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 12,
            fixedHeight = 20,
            stretchWidth = true,
            stretchHeight = false
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

        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(false, "Table", tabButtonStyle))
        {

        }
        if (GUILayout.Toggle(false, "Settings", tabButtonStyle))
        {

        }
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        #region Preview Box
        GUI.backgroundColor = AddColor("#0062ff") * 3f;
        GUILayout.BeginVertical(GUI.skin.box);
        GUI.backgroundColor = Color.white;
        symbolPreviewStyle.normal.textColor = Color.white;
        GUILayout.Label(selectedSymbol.ToString(), symbolPreviewStyle, GUILayout.Height(96f));
        GUI.backgroundColor = AddColor("#0062ff") * 3f;
        previewHeaderStyle.normal.textColor = Color.white;
        GUILayout.Label(CharToUnicode(selectedSymbol), previewHeaderStyle);
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
            CopyToClipboard(selectedSymbol.ToString());
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
            #region Symbol
            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Symbol: ", GUILayout.ExpandWidth(false));
            GUILayout.Label($"{selectedSymbol}", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            #endregion
            #region Name
            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Name: ", GUILayout.ExpandWidth(false));
            //GUILayout.Label($"Name: {ASCIIPrintableNames[selectedSymbol]}", GUILayout.ExpandWidth(false));
            GUILayout.Label($" ", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
            #endregion
            #region Unicode Number
            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Unicode Number: ", GUILayout.ExpandWidth(false));
            GUI.backgroundColor = AddColor("#0062ff") * 2f;
            GUI.contentColor = AddColor(Color.white);
            GUIContent unicodeNumContent = new GUIContent(CharToUnicode(selectedSymbol), copyTooltip);
            if (GUILayout.Button(unicodeNumContent, GUILayout.ExpandWidth(false)))
            {
                CopyToClipboard(unicodeNumContent.text);
            }
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
            #endregion
            #region Decimal
            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Decimal: {CharToDecimal(selectedSymbol)}");
            GUILayout.EndHorizontal();
            #endregion
            #region Hexadecimal
            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Hexadecimal: {UnicodeToHexCode(selectedSymbol)}");
            GUILayout.EndHorizontal();
            #endregion
            #region Octal
            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"Octal: {CharToOctal(selectedSymbol)}");
            GUILayout.EndHorizontal();
            #endregion
            #region HTML Code
            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"HTML Code: ", GUILayout.ExpandWidth(false));
            GUI.backgroundColor = AddColor("#0062ff") * 2f;
            GUI.contentColor = AddColor(Color.white);
            GUIContent htmlCodeContent = new GUIContent(CharToHTML(selectedSymbol), copyTooltip);
            if (GUILayout.Button(htmlCodeContent, GUILayout.ExpandWidth(false)))
            {
                CopyToClipboard(htmlCodeContent.text);
            }
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
            #endregion
            #region CSS Code
            GUILayout.BeginHorizontal();
            DrawBulletPoint("#0062ff");
            GUILayout.Label($"CSS Code: ", GUILayout.ExpandWidth(false));
            GUI.backgroundColor = AddColor("#0062ff") * 2f;
            GUI.contentColor = AddColor(Color.white);
            GUIContent cssCodeContent = new GUIContent(CharToCSS(selectedSymbol), copyTooltip);
            if (GUILayout.Button(cssCodeContent, GUILayout.ExpandWidth(false)))
            {
                CopyToClipboard(cssCodeContent.text);
            }
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
            #endregion

            GUI.contentColor = Color.white;
            GUILayout.EndVertical();
        }
        #endregion
        //
        GUILayout.EndVertical();
        #endregion

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

        GUI.backgroundColor = HasFavoriteSymbol(selectedSymbol) ? AddColor("#000842") : AddColor("#0062ff") * 2f;
        GUI.contentColor = HasFavoriteSymbol(selectedSymbol) ? Color.white : Color.white;
        GUILayout.BeginArea(new Rect(position.width - 40f, 34f, 36f, 36f));
        GUIContent favoriteButtonContent = new GUIContent(HasFavoriteSymbol(selectedSymbol) ? "★" : "☆", favoriteButtonTooltip);
        if (GUILayout.Toggle(false, favoriteButtonContent, favoriteButtonStyle))
        {
            ToggleFavorite();
        }
        GUILayout.EndArea();
        GUI.contentColor = Color.white;
        GUI.backgroundColor = AddColor("#0062ff") * 2f;

        string[] categoryNames = new string[]
        {
            "All ()",
            "ASCII-Printable (95)",
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
        GUI.contentColor = AddColor(Color.white);
        unicodeCategory = (UnicodeCategory)EditorGUILayout.Popup((int)unicodeCategory, categoryNames);
        GUI.backgroundColor = Color.white;

        GUILayout.Space(8f);

        // Update scroll position in the editor window.
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, false, GUI.skin.horizontalScrollbar, GUIStyle.none);
        SetupUnicodeTable();
        
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
        if (unicodeSymbols == null)
        {
            unicodeSymbols = new List<char>();
        }
        else if (unicodeSymbols.Count > 0)
        {
            selectedSymbol = unicodeSymbols[selectedIndex];
            return;
        }

        for (int i = 0; i < numOfUnicodeSymbols; i++)
        {
            char us = startChar++;
            if (unicodeSymbols.Contains(us))
                continue;

            unicodeSymbols.Add(us);
        }

        selectedSymbol = unicodeSymbols[selectedIndex];
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

    private void SetupUnicodeTable()
    {
        unicodeSymbols.Clear();
        InitializeUnicodeSymbols();

        switch (unicodeCategory)
        {
            case UnicodeCategory.ASCII:
                List<char> asciiPrintableSymbols = new List<char>();
                foreach (char ch in ASCIIPrintableNames.Keys)
                {
                    asciiPrintableSymbols.Add(ch);
                }
                DrawUnicodeTable(6, 16, asciiPrintableSymbols);
                break;
            case UnicodeCategory.Favorites:
                DrawUnicodeTable(5, 10, favoriteUnicodeSymbols);
                break;
        }
    }

    private void DrawUnicodeTable(int rows, int columns, List<char> characters)
    {
        bool lastSymbolButton = false;

        for (int y = 0; y < rows; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < columns; x++)
            {
                int index = x + (columns * y);
                lastSymbolButton = index == characters.Count - 1;
                char us = characters[index];
                if (!unicodeSymbols.Contains(us))
                {
                    characters.Add(us);
                }
                DrawSymbolButton(us);

                if (lastSymbolButton)
                    break;
            }
            GUILayout.EndHorizontal();

            if (lastSymbolButton)
                break;
        }
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

    public void CopySymbol() => CopyToClipboard(selectedSymbol.ToString());
    public void CopyUnicode() => CopyToClipboard(CharToUnicode(selectedSymbol));
    public void CopyHexCode() => CopyToClipboard(CharToHTML(selectedSymbol));
    public void CopyCSSCode() => CopyToClipboard(CharToCSS(selectedSymbol));

    public void ShowCopyContextMenu(char us)
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

    public void ShowSymbolContextMenu(char us)
    {
        // Get current event.
        Event current = Event.current;

        if (current.button == 1)
        {
            GenericMenu menu = new GenericMenu();
            favoriteSymbolPending = us;
            AddMenuItem(menu, "Add to Favorites", AddToFavorites);
            AddMenuItem(menu, "Remove from Favorites", RemoveFromFavorites);
            menu.ShowAsContext();

            current.Use();
        }
    }

    /// <summary>
    /// Add a Unicode symbol to the Favorites list. 
    /// </summary>
    public void AddToFavorites()
    {
        AddToFavorites(favoriteSymbolPending);
    }

    /// <summary>
    /// Add a Unicode symbol to the Favorites list. 
    /// </summary>
    public void AddToFavorites(char us)
    {
        char character = us;
        if (HasFavoriteSymbol(us) || favoriteUnicodeSymbols.Count == maxNumOfFavoriteSymbols)
            return;

        MUSTEditorPrefs.SetInt(GetFavoriteSymbolKey(favoriteUnicodeSymbols.Count), (int)character);
        favoriteUnicodeSymbols.Add(character);

        // Display quick notification.
        window.ShowNotification(new GUIContent($"{character}\n\nAdded to\n★ Favorites!"));
    }

    /// <summary>
    /// Remove a Unicode symbol from the Favorites list. 
    /// </summary>
    public void RemoveFromFavorites()
    {
        RemoveFromFavorites(favoriteSymbolPending);
    }

    /// <summary>
    /// Remove a Unicode symbol from the Favorites list. 
    /// </summary>
    public void RemoveFromFavorites(char us)
    {
        char character = us;
        if (!HasFavoriteSymbol(favoriteSymbolPending) || favoriteUnicodeSymbols.Count == 0)
            return;

        MUSTEditorPrefs.DeleteKey(GetFavoriteSymbolKey(favoriteUnicodeSymbols.IndexOf(character)));
        favoriteUnicodeSymbols.Remove(character);

        ReassignFavoriteSymbolKeys();

        // Display quick notification.
        window.ShowNotification(new GUIContent($"{character}\n\nRemoved from\n★ Favorites!"));
    }

    public void ToggleFavorite()
    {
        if (HasFavoriteSymbol(selectedSymbol))
        {
            RemoveFromFavorites(selectedSymbol);
        }
        else
        {
            AddToFavorites(selectedSymbol);
        }
    }

    public string GetFavoriteSymbolKey(int value) => $"Favorite Symbol[{value}]";

    private void ReassignFavoriteSymbolKeys()
    {
        // Delete all key references of the user's favorite Unicode symbols from EditorPrefs.
        for (int i = 0; i < maxNumOfFavoriteSymbols; i++)
        {
            string key = GetFavoriteSymbolKey(i);
            if (!MUSTEditorPrefs.HasKey(key))
                continue;
            MUSTEditorPrefs.DeleteKey(key);
        }

        // Reassign and organize the user's favorite Unicode symbols from EditorPrefs.
        for (int i = 0; i < favoriteUnicodeSymbols.Count; i++)
        {
            char us = favoriteUnicodeSymbols[i];
            MUSTEditorPrefs.SetInt(GetFavoriteSymbolKey(i), (int)us);
        }
    }

    private bool HasFavoriteSymbol(char us)
    {
        return favoriteUnicodeSymbols.Contains(selectedSymbol);
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
    private void DrawSymbolButton(char us)
    {
        if (GUILayout.Toggle(false, us.ToString(), symbolButtonStyle))
        {
            selectedIndex = unicodeSymbols.IndexOf(us);
            selectedSymbol = us;
            Debug.Log($"Index: {selectedIndex}");

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
            Debug.Log(favoriteUnicodeSymbols[i]);
        }
        //Debug.Log($"Symbol: {selectedSymbol}");
    }
}
