using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MassiveUnicodeSymbolTable;

namespace MassiveUnicodeSymbolTable
{
    public class MUSTEditor : EditorWindow
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
            Arrows = 7,
            Zodiac = 8,
            Planets = 9,
            PlayingCardSuits = 10,
            Musical = 11,
            Other = 12,
            Favorites = 13
        }

        public enum Tab
        {
            Table = 0,
            Settings = 1
        }
        #endregion

        #region Unicode Tables
        private readonly Dictionary<char, string> ASCIIPrintableNames = new Dictionary<char, string>()
        {
            {(char)32, "Space"},
            {(char)33, "Exclamation Mark"},
            {(char)34, "Quotation Mark"},
            {(char)35, "Number Sign"},
            {(char)36, "Dollar Sign"},
            {(char)37, "Percent Sign"},
            {(char)38, "Ampersand"},
            {(char)39, "Apostrophe"},
            {(char)40, "Left parenthesis"},
            {(char)41, "Right parenthesis"},
            {(char)42, "Asterisk"},
            {(char)43, "Plus Sign"},
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
            {(char)60, "Less-than Sign"},
            {(char)61, "Equal Sign"},
            {(char)62, "Greater-than Sign"},
            {(char)63, "Question Mark"},
            {(char)64, "Commercial At"},
            {(char)65, "Latin Capital Letter A"},
            {(char)66, "Latin Capital Letter B"},
            {(char)67, "Latin Capital Letter C"},
            {(char)68, "Latin Capital Letter D"},
            {(char)69, "Latin Capital Letter E"},
            {(char)70, "Latin Capital Letter F"},
            {(char)71, "Latin Capital Letter G"},
            {(char)72, "Latin Capital Letter H"},
            {(char)73, "Latin Capital Letter I"},
            {(char)74, "Latin Capital Letter J"},
            {(char)75, "Latin Capital Letter K"},
            {(char)76, "Latin Capital Letter L"},
            {(char)77, "Latin Capital Letter M"},
            {(char)78, "Latin Capital Letter N"},
            {(char)79, "Latin Capital Letter O"},
            {(char)80, "Latin Capital Letter P"},
            {(char)81, "Latin Capital Letter Q"},
            {(char)82, "Latin Capital Letter R"},
            {(char)83, "Latin Capital Letter S"},
            {(char)84, "Latin Capital Letter T"},
            {(char)85, "Latin Capital Letter U"},
            {(char)86, "Latin Capital Letter V"},
            {(char)87, "Latin Capital Letter W"},
            {(char)88, "Latin Capital Letter X"},
            {(char)89, "Latin Capital Letter Y"},
            {(char)90, "Latin Capital Letter Z"},
            {(char)91, "Left Square Bracket"},
            {(char)92, "Backslash"},
            {(char)93, "Right Square Bracket"},
            {(char)94, "Circumflex accent"},
            {(char)95, "Low line"},
            {(char)96, "Grave accent"},
            {(char)97, "Latin Small Letter A"},
            {(char)98, "Latin Small Letter B"},
            {(char)99, "Latin Small Letter C"},
            {(char)100, "Latin Small Letter D"},
            {(char)101, "Latin Small Letter E"},
            {(char)102, "Latin Small Letter F"},
            {(char)103, "Latin Small Letter G"},
            {(char)104, "Latin Small Letter H"},
            {(char)105, "Latin Small Letter I"},
            {(char)106, "Latin Small Letter J"},
            {(char)107, "Latin Small Letter K"},
            {(char)108, "Latin Small Letter L"},
            {(char)109, "Latin Small Letter M"},
            {(char)110, "Latin Small Letter N"},
            {(char)111, "Latin Small Letter O"},
            {(char)112, "Latin Small Letter P"},
            {(char)113, "Latin Small Letter Q"},
            {(char)114, "Latin Small Letter R"},
            {(char)115, "Latin Small Letter S"},
            {(char)116, "Latin Small Letter T"},
            {(char)117, "Latin Small Letter U"},
            {(char)118, "Latin Small Letter V"},
            {(char)119, "Latin Small Letter W"},
            {(char)120, "Latin Small Letter X"},
            {(char)121, "Latin Small Letter Y"},
            {(char)122, "Latin Small Letter Z"},
            {(char)123, "Left Curly Bracket"},
            {(char)124, "Vertical Line"},
            {(char)125, "Right Curly Bracket"},
            {(char)126, "Tilde"}
        };

        private readonly Dictionary<char, string> CurrencyNames = new Dictionary<char, string>()
        {
            {(char)36, "Dollar Sign"},
            {(char)162, "Cent Sign"},
            {(char)163, "Pound Sign"},
            {(char)164, "Currency Sign"},
            {(char)165, "Yen Sign"},
            {(char)1423, "Armenian Dram Sign"},
            {(char)1547, "Afghani Sign"},
            {(char)2546, "Bengali Rupee Mark"},
            {(char)2547, "Bengali Rupee Sign"},
            {(char)3065, "Tamil Rupee Sign"},
            {(char)3647, "Thai Currency Symbol Baht Sign"},
            {(char)8352, "Euro-Currency Sign"},
            {(char)8353, "Colon Sign"},
            {(char)8354, "Cruzeiro Sign"},
            {(char)8355, "French Franc Sign"},
            {(char)8356, "Lira Sign"},
            {(char)8357, "Mill Sign"},
            {(char)8358, "Naira Sign"},
            {(char)8359, "Peseta Sign"},
            {(char)8360, "Rupee Sign"},
            {(char)8361, "Won Sign"},
            {(char)8362, "New Sheqek Sign"},
            {(char)8363, "Dong Sign"},
            {(char)8364, "Euro Sign"},
            {(char)8365, "Kip Sign"},
            {(char)8366, "Tugrik Sign"},
            {(char)8367, "Drachma Sign"},
            {(char)8368, "German Penny Sign"},
            {(char)8369, "Peso Sign"},
            {(char)8370, "Guarani Sign"},
            {(char)8371, "Austral Sign"},
            {(char)8372, "Hryvnia Sign"},
            {(char)8373, "Cedi Sign"},
            {(char)8374, "Livre Tournois Sign"},
            {(char)8375, "Spesmilo Sign"},
            {(char)8376, "Tenge Sign"},
            {(char)8377, "Indian Rupee Sign"},
            {(char)65020, "Rial Sign"}
        };

        private readonly Dictionary<char, string> GreekLetterNames = new Dictionary<char, string>()
        {
            {(char)913, "Greek Capital Letter Alpha"},
            {(char)914, "Greek Capital Letter Beta"},
            {(char)915, "Greek Capital Letter Gamma"},
            {(char)916, "Greek Capital Letter Delta"},
            {(char)917, "Greek Capital Letter Epsilon"},
            {(char)918, "Greek Capital Letter Zeta"},
            {(char)919, "Greek Capital Letter Eta"},
            {(char)920, "Greek Capital Letter Theta"},
            {(char)921, "Greek Capital Letter Iota"},
            {(char)922, "Greek Capital Letter Kappa"},
            {(char)923, "Greek Capital Letter Lamda"},
            {(char)924, "Greek Capital Letter Mu"},
            {(char)925, "Greek Capital Letter Nu"},
            {(char)926, "Greek Capital Letter Xi"},
            {(char)927, "Greek Capital Letter Omicron"},
            {(char)928, "Greek Capital Letter Pi"},
            {(char)929, "Greek Capital Letter Rho"},
            {(char)931, "Greek Capital Letter Sigma"},
            {(char)932, "Greek Capital Letter Tau"},
            {(char)933, "Greek Capital Letter Upsilon"},
            {(char)934, "Greek Capital Letter Phi"},
            {(char)935, "Greek Capital Letter Chi"},
            {(char)936, "Greek Capital Letter Psi"},
            {(char)937, "Greek Capital Letter Omega"},
            {(char)945, "Greek Small Letter Alpha"},
            {(char)946, "Greek Small Letter Beta"},
            {(char)947, "Greek Small Letter Gamma"},
            {(char)948, "Greek Small Letter Delta"},
            {(char)949, "Greek Small Letter Epsilon"},
            {(char)950, "Greek Small Letter Zeta"},
            {(char)951, "Greek Small Letter Eta"},
            {(char)952, "Greek Small Letter Theta"},
            {(char)953, "Greek Small Letter Iota"},
            {(char)954, "Greek Small Letter Kappa"},
            {(char)955, "Greek Small Letter Lamda"},
            {(char)956, "Greek Small Letter Mu"},
            {(char)957, "Greek Small Letter Nu"},
            {(char)958, "Greek Small Letter Xi"},
            {(char)959, "Greek Small Letter Omicron"},
            {(char)960, "Greek Small Letter Pi"},
            {(char)961, "Greek Small Letter Rho"},
            {(char)962, "Greek Small Letter Final Sigma"},
            {(char)963, "Greek Small Letter Sigma"},
            {(char)964, "Greek Small Letter Tau"},
            {(char)965, "Greek Small Letter Upsilon"},
            {(char)966, "Greek Small Letter Phi"},
            {(char)967, "Greek Small Letter Chi"},
            {(char)968, "Greek Small Letter Psi"},
            {(char)969, "Greek Small Letter Omega"}
        };

        private readonly Dictionary<char, string> RomanNumeralNames = new Dictionary<char, string>()
        {
            {(char)8544, "Roman Numeral One"},
            {(char)8545, "Roman Numeral Two"},
            {(char)8546, "Roman Numeral Three"},
            {(char)8547, "Roman Numeral Four"},
            {(char)8548, "Roman Numeral Five"},
            {(char)8549, "Roman Numeral Six"},
            {(char)8550, "Roman Numeral Seven"},
            {(char)8551, "Roman Numeral Eight"},
            {(char)8552, "Roman Numeral Nine"},
            {(char)8553, "Roman Numeral Ten"},
            {(char)8554, "Roman Numeral Eleven"},
            {(char)8555, "Roman Numeral Twelve"},
            {(char)8556, "Roman Numeral Fifty"},
            {(char)8557, "Roman Numeral One Hundred"},
            {(char)8558, "Roman Numeral Five Hundred"},
            {(char)8559, "Roman Numeral One Thousand"},
            {(char)8560, "Small Roman Numeral One"},
            {(char)8561, "Small Roman Numeral Two"},
            {(char)8562, "Small Roman Numeral Three"},
            {(char)8563, "Small Roman Numeral Four"},
            {(char)8564, "Small Roman Numeral Five"},
            {(char)8565, "Small Roman Numeral Six"},
            {(char)8566, "Small Roman Numeral Seven"},
            {(char)8567, "Small Roman Numeral Eight"},
            {(char)8568, "Small Roman Numeral Nine"},
            {(char)8569, "Small Roman Numeral Ten"},
            {(char)8570, "Small Roman Numeral Eleven"},
            {(char)8571, "Small Roman Numeral Twelve"},
            {(char)8572, "Small Roman Numeral Fifty"},
            {(char)8573, "Small Roman Numeral One Hundred"},
            {(char)8574, "Small Roman Numeral Five Hundred"},
            {(char)8575, "Small Roman Numeral One Thousand"},
            {(char)8576, "Roman Numeral One Thousand C D"},
            {(char)8577, "Roman Numeral Five Thousand"},
            {(char)8578, "Roman Numeral Ten Thousand"},
            {(char)8583, "Roman Numeral Fifty Thousand"},
            {(char)8584, "Roman Numeral One Hundred Thousand"}
        };

        private readonly Dictionary<char, string> PunctuationNames = new Dictionary<char, string>()
        {
            {(char)33, "Exclamation Mark"},
            {(char)34, "Quotation Mark"},
            {(char)35, "Number Sign"},
            {(char)37, "Percent Sign"},
            {(char)38, "Ampersand"},
            {(char)39, "Apostrophe"},
            {(char)42, "Asterisk"},
            {(char)44, "Comma"},
            {(char)46, "Full stop"},
            {(char)47, "Slash (Solidus)"},
            {(char)58, "Colon"},
            {(char)59, "Semicolon"},
            {(char)63, "Question Mark"},
            {(char)64, "Commercial At"},
            {(char)92, "Reverse Solidus"},
            {(char)161, "Inverted Exclamation Mark"},
            {(char)167, "Section Sign"},
            {(char)182, "Pilcrow Sign"},
            {(char)183, "Middle Dot"},
            {(char)191, "Inverted Question Mark"},
            {(char)8216, "Left Single Quotation Mark"},
            {(char)8217, "Right Single Quotation Mark"},
            {(char)8220, "Left Double Quotation Mark"},
            {(char)8221, "Right Double Quotation Mark"},
            {(char)8226, "Bullet"},
            {(char)8227, "Triangular Bullet"},
            {(char)8230, "Horizontal Ellipsis"},
            {(char)8231, "Hyphenation Point"},
            {(char)8240, "Per Mille Sign"},
            {(char)8251, "Reference Mark"},
            {(char)8252, "Double Exclamation Mark"},
            {(char)8263, "Double Question Mark"},
            {(char)8264, "Question Exclamation Mark"},
            {(char)8265, "Exclamation Question Mark"},
        };

        private readonly Dictionary<char, string> MathNames = new Dictionary<char, string>()
        {
            {(char)43, "Plus Sign"},
            {(char)60, "Less-than Sign"},
            {(char)61, "Equal Sign"},
            {(char)62, "Greater-than Sign"},
            {(char)124, "Vertical Line"},
            {(char)126, "Tilde"},
            {(char)172, "Not Sign"},
            {(char)177, "Plus-Minus Sign"},
            {(char)215, "Multiplication Sign"},
            {(char)247, "Division Sign"},
            {(char)1014, "Greek Reversed Lunate Epsilon Symbol"},
            {(char)1542, "Arabic-Indic Cube Root"},
            {(char)1543, "Arabic-Indic Fourth Root"},
            {(char)1544, "Arabic Ray"},
            {(char)8260, "Fraction Slash"},
            {(char)8274, "Commercial Minus Sign"},
            {(char)8314, "Superscript Plus Sign"},
            {(char)8315, "Superscript Minus Sign"},
            {(char)8316, "Superscript Equal Sign"},
            {(char)8330, "Subscript Plus Sign"},
            {(char)8331, "Subscript Minus Sign"},
            {(char)8332, "Subscript Equal Sign"},
            {(char)8704, "For All"},
            {(char)8705, "Complement"},
            {(char)8706, "Partial Differential"},
            {(char)8707, "There Exists"},
            {(char)8708, "There Does Not Exist"},
            {(char)8709, "Empty Set"},
            {(char)8710, "Increment"},
            {(char)8711, "Nabla"},
            {(char)8712, "Element Of"},
            {(char)8713, "Not An Element Of"},
            {(char)8714, "Small Element Of"},
            {(char)8715, "Contains as Member"},
            {(char)8716, "Does Not Contain as Member"},
            {(char)8717, "Small Contains as Member"},
            {(char)8719, "N-Ary Product"},
            {(char)8720, "N-Ary Coproduct"},
            {(char)8721, "N-Ary Summation"},
            {(char)8722, "Minus Sign"},
            {(char)8723, "Minus-or-Plus Sign"},
            {(char)8724, "Dot Plus"},
            {(char)8727, "Asterisk Operator"},
            {(char)8728, "Ring Operator"},
            {(char)8729, "Bullet Operator"},
            {(char)8730, "Square Root"},
            {(char)8731, "Cube Root"},
            {(char)8732, "Fourth Root"},
            {(char)8733, "Proportional To"},
            {(char)8734, "Infinity"},
            {(char)8735, "Right Angle"},
            {(char)8736, "Angle"},
            {(char)8737, "Measured Angle"},
            {(char)8738, "Spherical Angle"},
            {(char)8740, "Does Not Divide"},
            {(char)8741, "Parallel To"},
            {(char)8742, "Not Parallel To"},
            {(char)8743, "Logical And"},
            {(char)8744, "Logical Or"},
            {(char)8745, "Intersection"},
            {(char)8746, "Union"},
            {(char)8747, "Integral"},
            {(char)8758, "Ratio"},
            {(char)8759, "Proportion"},
            {(char)8773, "Approximately Equal To"},
            {(char)8776, "Almost Equal To"},
            {(char)8777, "Not Almost Equal To"},
            {(char)8778, "Almost Equal or Equal To"},
            {(char)8780, "All Equal To"},
            {(char)8781, "Equivalent To"},
            {(char)8782, "Geometrically Equivalent To"},
            {(char)8799, "Questioned Equal To"},
            {(char)8800, "Not Equal To"},
            {(char)8801, "Identical To"},
            {(char)8802, "Not Identical To"},
            {(char)8804, "Less-Than or Equal To"},
            {(char)8805, "Greater-Than or Equal To"},
            {(char)8810, "Much Less-Than"},
            {(char)8811, "Much Greater-Than"},
            {(char)8814, "Not Less-Than"},
            {(char)8815, "Not Greater-Than"},
            {(char)8834, "Subset Of"},
            {(char)8835, "Superset Of"},
            {(char)8836, "Not A Subset Of"},
            {(char)8837, "Not A Superset Of"},
            {(char)8838, "Subset Of or Equal To"},
            {(char)8839, "Superset Of or Equal To"},
            {(char)8840, "Neither A Subset Of Nor Equal To"},
            {(char)8841, "Neither A Superset Of Nor Equal To"},
            {(char)8903, "Division Times"},
        };

        private readonly Dictionary<char, string> ArrowNames = new Dictionary<char, string>()
        {
            {(char)8592, "Leftwards Arrow"},
            {(char)8593, "Upwards Arrow"},
            {(char)8594, "Righwards Arrow"},
            {(char)8595, "Downwards Arrow"},
            {(char)8596, "Left Right Arrow"},
            {(char)8597, "Up Down Arrow"},
            {(char)8598, "North West Arrow"},
            {(char)8599, "North East Arrow"},
            {(char)8600, "South East Arrow"},
            {(char)8601, "South West Arrow"},
            {(char)8634, "Anticlockwise Open Circle Arrow"},
            {(char)8635, "Clockwise Open Circle Arrow"}
        };

        private readonly Dictionary<char, string> ZodiacNames = new Dictionary<char, string>()
        {
            {(char)9800, "Aries"},
            {(char)9801, "Taurus"},
            {(char)9802, "Gemini"},
            {(char)9803, "Cancer"},
            {(char)9804, "Leo"},
            {(char)9805, "Virgo"},
            {(char)9806, "Libra"},
            {(char)9807, "Scorpius"},
            {(char)9808, "Sagittarius"},
            {(char)9809, "Capricorn"},
            {(char)9810, "Aquarius"},
            {(char)9811, "Pisces"}
        };

        private readonly Dictionary<char, string> PlanetNames = new Dictionary<char, string>()
        {
            {(char)9737, "Sun"},
            {(char)9789, "First Quarter Moon"},
            {(char)9790, "Last Quarter Moon"},
            {(char)9791, "Mercury"},
            {(char)9792, "Venus"},
            {(char)9793, "Earth"},
            {(char)9794, "Mars"},
            {(char)9795, "Jupiter"},
            {(char)9796, "Saturn"},
            {(char)9797, "Uranus"},
            {(char)9798, "Neptune"},
            {(char)9799, "Pluto"}
        };

        private readonly Dictionary<char, string> PlayingCardSuitNames = new Dictionary<char, string>()
        {
            {(char)9824, "Black Spade Suit"},
            {(char)9825, "White Heart Suit"},
            {(char)9826, "White Diamond Suit"},
            {(char)9827, "Black Club Suit"},
            {(char)9828, "White Spade Suit"},
            {(char)9829, "Black Heart Suit"},
            {(char)9830, "Black Diamond Suit"},
            {(char)9831, "White Club Suit"}
        };

        private readonly Dictionary<char, string> MusicalNames = new Dictionary<char, string>()
        {
            {(char)9833, "Quarter Note"},
            {(char)9834, "Eighth Note"},
            {(char)9835, "Beamed Eighth Notes"},
            {(char)9836, "Beamed Sixteenth Notes"},
            {(char)9837, "Music Flat Sign"},
            {(char)9838, "Music Natural Sign"},
            {(char)9839, "Music Sharp Sign"}
        };

        private readonly Dictionary<char, string> OtherNames = new Dictionary<char, string>()
        {
            {(char)169, "Copyright Sign"},
            {(char)174, "Registered Sign"},
            {(char)176, "Degree Sign"},
            {(char)8451, "Degree Celsius"},
            {(char)8457, "Degree Fahrenheit"},
            {(char)8471, "Sound Recording Copyright"},
            {(char)8480, "Service Mark"},
            {(char)8481, "Telephone Sign"},
            {(char)8482, "Trade Mark Sign"},
            {(char)8507, "Facsimile Sign"},
            {(char)8984, "Place of Interest Sign"},
            {(char)9733, "Black Star"},
            {(char)9734, "White Star"},
            {(char)9786, "White Smiling Face"},
            {(char)9787, "Black Smiling Face"},
            {(char)9888, "Warning Sign"},
            {(char)10003, "Check Mark"},
            {(char)65532, "Object Replacement Character"}
        };
        #endregion

        private Dictionary<char, string> AllUnicodeNames;

        private static MUSTEditor window;    // Editor window
        private Vector2 scrollPosition;      // Current scroll position

        // Icon paths
        // Note: Make sure to import the package(s) under Assets to have all icons display properly in the edito window.
        private readonly string copyIconPath = "Assets/Massive Unicode Symbol Table (MUST)/Textures/CopyIcon.png";

        #region Tooltips
        private string copyTooltip = "Click to copy.";
        private string copySymbolTooltip = "Click this to copy the previewed Unicode symbol and paste it anywhere.";
        private string favoriteButtonTooltip = "Click this to add/remove this Unicode symbol to and from ★ Favorites.";
        #endregion

        #region GUI Styles
        private GUIStyle symbolButtonStyle;
        private GUIStyle favoriteButtonStyle;
        private GUIStyle tabButtonStyle;
        private GUIStyle copyButtonStyle;
        private GUIStyle sideButtonStyle;
        private GUIStyle previewHeaderStyle;
        private GUIStyle symbolPreviewStyle;
        private GUIStyle settingSubButtonStyle;
        private GUIStyle colorLabelStyle;
        private GUIStyle swatchStyle;
        private GUIStyle bulletPointStyle;
        #endregion

        private bool infoFoldout = false;

        private Tab mainTab = Tab.Table;
        private UnicodeCategory unicodeCategory = UnicodeCategory.ASCII;

        private List<char> unicodeSymbols;
        private List<char> favoriteUnicodeSymbols;
        private readonly int maxNumOfFavoriteSymbols = 50;
        private char selectedSymbol = ' ';
        private char favoriteSymbolPending = ' ';
        private int selectedIndex = 0;
        private float windowWidth;

        #region Settings
        private readonly string[] ColorStyles = new string[18]
        {
            "#ff3838",      // 0
            "#ff512e",      // 1
            "#ff852e",      // 2
            "#00a81a",      // 3
            "#0062ff",      // 4
            "#b22eff",      // 5
            "#ff006a",      // 6
            "#00a9ff",      // 7
            "#75523f",      // 8
            "#b978ff",      // 9
            "#6f00ff",      // 10
            "#1d8a7d",      // 11
            "#5a8f00",      // 12
            "#b08a64",      // 13
            "#68768a",      // 14
            "#486860",      // 15
            "#847545",      // 16
            "#767676",      // 17
        };

        private string mainColorStyle;
        private readonly string PreferredColorStyleKey = "Preferred Color Style";

        #region Settings Tooltips
        private readonly string deleteAllFavoritesTooltip = "Click this button to delete all Unicode symbols from ★ Favorites.";
        private readonly string documentationTooltip = "Click this button to see the official documentation on how to use " +
                                                       "the Massive Unicode Symbol Table (MUST).";
        #endregion
        #endregion

        /// <summary>
        /// Display the Massive Unicode Symbol Table menu item. (Tools -> Massive Unicode Symbol Table)
        /// </summary>
        [MenuItem("Tools/Massive Unicode Symbol Table", false, 10)]
        public static void DisplayWindow()
        {
            window = GetWindow<MUSTEditor>("Massive Unicode Symbol Table (MUST) V1.0");
            //CreateInstance<MUSTEditor>().Show();
        }

        private void OnEnable()
        {
            if (AllUnicodeNames == null)
            {
                AllUnicodeNames = new Dictionary<char, string>();

                #region Add Categories
                AddNames(ASCIIPrintableNames);          // Add ASCII-Printable names to global list.
                AddNames(CurrencyNames);                // Add Currency names to global list.
                AddNames(GreekLetterNames);             // Add Greek Letter names to global list.
                AddNames(RomanNumeralNames);            // Add RRoman Numeral names to global list.
                AddNames(PunctuationNames);             // Add Punctuation names to global list.
                AddNames(MathNames);                    // Add Math names to global list.
                AddNames(ArrowNames);                   // Add Arrow names to global list.
                AddNames(ZodiacNames);                  // Add Zodiac names to global list.
                AddNames(PlanetNames);                  // Add Planet names to global list.
                AddNames(PlayingCardSuitNames);         // Add Playing Card Suit names to global list.
                AddNames(MusicalNames);                 // Add Musical names to global list.
                AddNames(OtherNames);                   // Add Other names to global list.
                #endregion
            }

            // Populate your favorite Unicode symbols to ★ Favorites.
            if (favoriteUnicodeSymbols == null)
            {
                favoriteUnicodeSymbols = new List<char>();

                for (int i = 0; i < maxNumOfFavoriteSymbols; i++)
                {
                    if (!MUSTEditorPrefs.HasKey(GetFavoriteSymbolKey(i)))
                        continue;

                    char ch = (char)MUSTEditorPrefs.GetInt(GetFavoriteSymbolKey(i));
                    if (ch == (char)0)
                        continue;

                    favoriteUnicodeSymbols.Add(ch);
                }
            }

            // Set the main color style of the editor window.
            int colorSytleID = MUSTEditorPrefs.HasKey(PreferredColorStyleKey) ? MUSTEditorPrefs.GetInt(PreferredColorStyleKey) : 4;
            SetColorStyle(colorSytleID);
        }

        /// <summary>
        /// Editor GUI.
        /// </summary>
        private void OnGUI()
        {
            // Get window.
            if (window == null)
            {
                window = GetWindow<MUSTEditor>("Massive Unicode Symbol Table (MUST) V1.0");
            }

            // Set minimum &maximum window size(Docked/ Windowed).
            if (!window.docked)
            {
                window.minSize = new Vector2(312f, 312f);
                window.maxSize = new Vector2(1920f, 1080f);
            }

            // Get the width of the editor window.
            windowWidth = position.width;

            // Initialize various GUI Styles.
            InitializeGUIStyles();

            // Initialize Unicode symbols chart based on the selected category.
            if (unicodeSymbols == null)
            {
                unicodeSymbols = new List<char>();
            }

            #region Tabs
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = mainTab == Tab.Table ? AddColor(mainColorStyle) * 0.75f : AddColor(mainColorStyle) * 1.75f;
            GUI.contentColor = mainTab == Tab.Table ? Color.white * 0.75f : Color.white;
            if (GUILayout.Toggle(mainTab == Tab.Table, "Table", tabButtonStyle))
            {
                mainTab = Tab.Table;
            }
            GUI.backgroundColor = mainTab == Tab.Settings ? AddColor(mainColorStyle) * 0.75f : AddColor(mainColorStyle) * 1.75f;
            GUI.contentColor = mainTab == Tab.Settings ? Color.white * 0.75f : Color.white;
            if (GUILayout.Toggle(mainTab == Tab.Settings, "Settings", tabButtonStyle))
            {
                mainTab = Tab.Settings;
            }
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
            #endregion

            if (mainTab == Tab.Table)
            {
                GUILayout.BeginVertical();
                #region Preview Box
                GUI.backgroundColor = AddColor(mainColorStyle) * 3.75f;
                GUILayout.BeginVertical(GUI.skin.box);
                GUI.backgroundColor = Color.white;
                symbolPreviewStyle.normal.textColor = Color.white;
                GUILayout.Label(selectedSymbol.ToString(), symbolPreviewStyle, GUILayout.Height(96f));
                GUI.backgroundColor = AddColor(mainColorStyle) * 3.75f;
                previewHeaderStyle.normal.textColor = Color.white;
                GUILayout.Label(CharToUnicode(selectedSymbol), previewHeaderStyle);
                GUI.backgroundColor = Color.white;
                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();
                #region Previous Symbol
                GUIContent prevSymbolContent = new GUIContent("◀", "");
                GUI.backgroundColor = AddColor(mainColorStyle) * 1.75f;
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

                string copyText = copyIcon != null ? " Copy" : "Copy";
                GUIContent copyContent = new GUIContent(copyText, copyIcon, copySymbolTooltip);
                GUI.backgroundColor = AddColor(mainColorStyle) * 1.75f;
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
                GUI.backgroundColor = AddColor(mainColorStyle) * 1.75f;
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
                    DrawBulletPoint(mainColorStyle);
                    GUILayout.Label($"Symbol: ", GUILayout.ExpandWidth(false));
                    GUILayout.Label($"{selectedSymbol}", GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();
                    #endregion
                    #region Name
                    GUILayout.BeginHorizontal();
                    DrawBulletPoint(mainColorStyle);
                    GUILayout.Label($"Name: {AllUnicodeNames[selectedSymbol]}", GUILayout.ExpandWidth(false));
                    GUILayout.Label($" ", GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();
                    #endregion
                    #region Unicode Number
                    GUILayout.BeginHorizontal();
                    DrawBulletPoint(mainColorStyle);
                    GUILayout.Label($"Unicode Number: ", GUILayout.ExpandWidth(false));
                    GUI.backgroundColor = AddColor(mainColorStyle) * 2f;
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
                    DrawBulletPoint(mainColorStyle);
                    GUILayout.Label($"Decimal: {CharToDecimal(selectedSymbol)}");
                    GUILayout.EndHorizontal();
                    #endregion
                    #region Hexadecimal
                    GUILayout.BeginHorizontal();
                    DrawBulletPoint(mainColorStyle);
                    GUILayout.Label($"Hexadecimal: {CharToHexCode(selectedSymbol)}");
                    GUILayout.EndHorizontal();
                    #endregion
                    #region Octal
                    GUILayout.BeginHorizontal();
                    DrawBulletPoint(mainColorStyle);
                    GUILayout.Label($"Octal: {CharToOctal(selectedSymbol)}");
                    GUILayout.EndHorizontal();
                    #endregion
                    #region HTML Code
                    GUILayout.BeginHorizontal();
                    DrawBulletPoint(mainColorStyle);
                    GUILayout.Label($"HTML Code: ", GUILayout.ExpandWidth(false));
                    GUI.backgroundColor = AddColor(mainColorStyle) * 2f;
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
                    DrawBulletPoint(mainColorStyle);
                    GUILayout.Label($"CSS Code: ", GUILayout.ExpandWidth(false));
                    GUI.backgroundColor = AddColor(mainColorStyle) * 2f;
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
                GUILayout.EndVertical();
                #endregion

                DrawLine(GetColorFromHexString("#555555"), 1, 4f);

                GUI.backgroundColor = AddColor(mainColorStyle) * 2f;

                GUI.backgroundColor = HasFavoriteSymbol(selectedSymbol) ? AddColor("#000000") : AddColor(mainColorStyle) * 2f;
                GUI.contentColor = HasFavoriteSymbol(selectedSymbol) ? Color.white : Color.white;
                GUILayout.BeginArea(new Rect(position.width - 40f, 34f, 36f, 36f));
                GUIContent favoriteButtonContent = new GUIContent(HasFavoriteSymbol(selectedSymbol) ? "★" : "☆", favoriteButtonTooltip);
                if (GUILayout.Toggle(false, favoriteButtonContent, favoriteButtonStyle))
                {
                    ToggleFavorite();
                }
                GUILayout.EndArea();
                GUI.contentColor = Color.white;
                GUI.backgroundColor = AddColor(mainColorStyle) * 2f;

                string[] categoryNames = new string[]
                {
                $"All ({AllUnicodeNames.Count})",
                $"ASCII-Printable ({ASCIIPrintableNames.Count})",
                $"Currency ({CurrencyNames.Count})",
                $"Greek Letters ({GreekLetterNames.Count})",
                $"Roman Numerals ({RomanNumeralNames.Count})",
                $"Punctuation ({PunctuationNames.Count})",
                $"Math ({MathNames.Count})",
                $"Arrows ({ArrowNames.Count})",
                $"Zodiac ({ZodiacNames.Count})",
                $"Planets ({PlanetNames.Count})",
                $"Playing Card Suits ({PlayingCardSuitNames.Count})",
                $"Musical ({MusicalNames.Count})",
                $"Other ({OtherNames.Count})",
                $"★ Favorites ({favoriteUnicodeSymbols.Count})"
                };
                GUI.contentColor = AddColor(Color.white);
                unicodeCategory = (UnicodeCategory)EditorGUILayout.Popup((int)unicodeCategory, categoryNames);
                GUI.backgroundColor = Color.white;

                GUILayout.Space(8f);

                // Update scroll position in the editor window.
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar);
                SetupUnicodeTable();
                GUILayout.FlexibleSpace();

                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUI.contentColor = Color.white;
                GUILayout.Label("Color Styles", colorLabelStyle, GUILayout.ExpandWidth(false));
                DrawLine(GetColorFromHexString("#555555"), 1, 4f);
                GUILayout.Label("Choose a color style.", GUILayout.ExpandWidth(false));

                DrawColorStyles();

                GUILayout.EndVertical();

                DrawLine(GetColorFromHexString("#555555"), 1, 4f);

                #region Delete All Favorites
                GUI.enabled = favoriteUnicodeSymbols.Count != 0;
                GUI.backgroundColor = GUI.enabled ? AddColor(mainColorStyle) * 1.75f : Color.white;
                GUIContent deleteFavoritesContent = new GUIContent("Delete All Favorites", deleteAllFavoritesTooltip);
                bool onDeleteClick = GUILayout.Button(deleteFavoritesContent, settingSubButtonStyle);
                if (onDeleteClick)
                {
                    DeleteAllFavorites();
                }
                GUI.enabled = true;
                GUI.backgroundColor = Color.white;
                #endregion

                GUILayout.Space(2f);

                #region Documentation
                GUI.backgroundColor = AddColor(mainColorStyle) * 1.75f;
                GUIContent documentationContent = new GUIContent("Documentation", documentationTooltip);
                bool onDocumentationClick = GUILayout.Button(documentationContent, settingSubButtonStyle);
                if (onDocumentationClick)
                {
                    OpenDocumentation();
                }
                GUI.backgroundColor = Color.white;
                #endregion
            }
        }



        #region Copy Method(s)
        /// <summary>
        /// Copies a string to the Clipboard.
        /// </summary>
        public static void CopyToClipboard(string s)
        {
            GUIUtility.systemCopyBuffer = s;

            // Display quick notification.
            window.ShowNotification(new GUIContent($"{s}\n\nCopied!"));
        }

        /// <summary>
        /// Copies the Unicode symbol to the Clipboard.
        /// </summary>
        public void CopySymbol() => CopyToClipboard(selectedSymbol.ToString());

        /// <summary>
        /// Copies the Unicode string to the Clipboard.
        /// </summary>
        public void CopyUnicode() => CopyToClipboard(CharToUnicode(selectedSymbol));

        /// <summary>
        /// Copies the HTML entity code (Hexadecimal) to the Clipboard.
        /// </summary>
        public void CopyHexCode() => CopyToClipboard(CharToHTML(selectedSymbol));

        /// <summary>
        /// Copies the CSS code to the Clipboard.
        /// </summary>
        public void CopyCSSCode() => CopyToClipboard(CharToCSS(selectedSymbol));
        #endregion

        #region Settings Method(s)
        /// <summary>
        /// Set the main color style of the editor window.
        /// </summary>
        /// <param name="colorStyleID">The ID of the color style (0-15).</param>
        private void SetColorStyle(int colorStyleID)
        {
            mainColorStyle = ColorStyles[colorStyleID];
            MUSTEditorPrefs.SetInt(PreferredColorStyleKey, colorStyleID);
        }

        /// <summary>
        /// Delete all Unicode symbols from ★ Favorites.
        /// </summary>
        private void DeleteAllFavorites()
        {
            List<char> favorites = new List<char>();
            favorites.AddRange(favoriteUnicodeSymbols);

            foreach (char favoriteSymbol in favorites)
            {
                favoriteSymbolPending = favoriteSymbol;
                RemoveFromFavorites(favoriteSymbol, false);
            }

            favorites.Clear();

            // Display quick notification.
            window.ShowNotification(new GUIContent($"All ★ Favorites\nsymbols deleted!"));
        }

        /// <summary>
        /// Open the official documentation on how to use the Massive Unicode Symbol Table (MUST).
        /// </summary>
        private void OpenDocumentation()
        {
            Application.OpenURL("https://acrobat.adobe.com/link/track?uri=urn:aaid:scds:US:d99c34d2-e21b-326e-95ee-a9ba13a53e25");
        }
        #endregion

        #region MUST Method(s)
        /// <summary>
        /// Get the next Unicode symbol.
        /// </summary>
        private void NextSymbol()
        {
            if (unicodeSymbols.Count == 0)
                return;

            selectedIndex = Mathf.Clamp(selectedIndex + 1, 0, unicodeSymbols.Count - 1);
            selectedSymbol = unicodeSymbols[selectedIndex];
        }

        /// <summary>
        /// Get the previous Unicode symbol.
        /// </summary>
        private void PreviousSymbol()
        {
            if (unicodeSymbols.Count == 0)
                return;

            selectedIndex = Mathf.Clamp(selectedIndex - 1, 0, unicodeSymbols.Count - 1);
            selectedSymbol = unicodeSymbols[selectedIndex];
        }

        /// <summary>
        /// Initialize GUI Styles.
        /// </summary>
        private void InitializeGUIStyles()
        {
            #region Symbol Button
            symbolButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedWidth = 28,
                fixedHeight = 28
            };
            #endregion
            #region Favorite Button
            favoriteButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 20,
                fixedWidth = 28,
                fixedHeight = 28
            };
            #endregion
            #region Tab Button
            tabButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12,
                fixedHeight = 20,
                stretchWidth = true,
                stretchHeight = false
            };
            #endregion
            #region Copy Button
            copyButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 24,
                stretchWidth = true,
                fixedHeight = 36
            };
            #endregion
            #region Side Button
            sideButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 16,
                stretchWidth = false,
                fixedWidth = 36,
                fixedHeight = 36,
                alignment = TextAnchor.MiddleCenter
            };
            #endregion
            #region Preview Header
            previewHeaderStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Normal,
                fontSize = 12,
                fixedHeight = 18,
                stretchWidth = true,
                stretchHeight = false,
                alignment = TextAnchor.MiddleCenter
            };
            #endregion
            #region Symbol Preview
            symbolPreviewStyle = new GUIStyle(GUI.skin.box)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 72,
                stretchWidth = true,
                stretchHeight = false,
                alignment = TextAnchor.MiddleCenter
            };
            #endregion
            #region Setting Sub Button
            settingSubButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 16,
                fixedHeight = 36,
                alignment = TextAnchor.MiddleCenter
            };
            #endregion
            #region Color Styles Label
            colorLabelStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 18,
                alignment = TextAnchor.MiddleCenter
            };
            #endregion
            #region Swatch
            swatchStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 24,
                fixedWidth = 47f,
                fixedHeight = 47f,
                stretchWidth = true,
                stretchHeight = true,
                alignment = TextAnchor.MiddleCenter
            };
            #endregion
        }

        /// <summary>
        /// Setup the Unicode table based on the selected category.
        /// </summary>
        private void SetupUnicodeTable()
        {
            unicodeSymbols.Clear();
            switch (unicodeCategory)
            {
                // All (390)
                case UnicodeCategory.All:
                    List<char> allSymbols = new List<char>();
                    foreach (char ch in AllUnicodeNames.Keys)
                    {
                        allSymbols.Add(ch);
                    }
                    DrawUnicodeTable(allSymbols);
                    break;
                // ASCII-Printable (95)
                case UnicodeCategory.ASCII:
                    List<char> asciiPrintableSymbols = new List<char>();
                    foreach (char ch in ASCIIPrintableNames.Keys)
                    {
                        asciiPrintableSymbols.Add(ch);
                    }
                    DrawUnicodeTable(asciiPrintableSymbols);
                    break;
                // Currency (38)
                case UnicodeCategory.Currency:
                    List<char> currencySymbols = new List<char>();
                    foreach (char ch in CurrencyNames.Keys)
                    {
                        currencySymbols.Add(ch);
                    }
                    DrawUnicodeTable(currencySymbols);
                    break;
                // Greek Letters (49)
                case UnicodeCategory.GreekLetters:
                    List<char> greekLetterSymbols = new List<char>();
                    foreach (char ch in GreekLetterNames.Keys)
                    {
                        greekLetterSymbols.Add(ch);
                    }
                    DrawUnicodeTable(greekLetterSymbols);
                    break;
                // Roman Numerals (37)
                case UnicodeCategory.RomanNumerals:
                    List<char> romanNumeralSymbols = new List<char>();
                    foreach (char ch in RomanNumeralNames.Keys)
                    {
                        romanNumeralSymbols.Add(ch);
                    }
                    DrawUnicodeTable(romanNumeralSymbols);
                    break;
                // Punctuation (34)
                case UnicodeCategory.Punctuation:
                    List<char> punctuationSymbols = new List<char>();
                    foreach (char ch in PunctuationNames.Keys)
                    {
                        punctuationSymbols.Add(ch);
                    }
                    DrawUnicodeTable(punctuationSymbols);
                    break;
                // Math (90)
                case UnicodeCategory.Math:
                    List<char> mathSymbols = new List<char>();
                    foreach (char ch in MathNames.Keys)
                    {
                        mathSymbols.Add(ch);
                    }
                    DrawUnicodeTable(mathSymbols);
                    break;
                // Arrows (12)
                case UnicodeCategory.Arrows:
                    List<char> arrowSymbols = new List<char>();
                    foreach (char ch in ArrowNames.Keys)
                    {
                        arrowSymbols.Add(ch);
                    }
                    DrawUnicodeTable(arrowSymbols);
                    break;
                // Zodiac (12)
                case UnicodeCategory.Zodiac:
                    List<char> zodiacSymbols = new List<char>();
                    foreach (char ch in ZodiacNames.Keys)
                    {
                        zodiacSymbols.Add(ch);
                    }
                    DrawUnicodeTable(zodiacSymbols);
                    break;
                // Planets (12)
                case UnicodeCategory.Planets:
                    List<char> planetSymbols = new List<char>();
                    foreach (char ch in PlanetNames.Keys)
                    {
                        planetSymbols.Add(ch);
                    }
                    DrawUnicodeTable(planetSymbols);
                    break;
                // Playing Card Suits (8)
                case UnicodeCategory.PlayingCardSuits:
                    List<char> playingCardSuitSymbols = new List<char>();
                    foreach (char ch in PlayingCardSuitNames.Keys)
                    {
                        playingCardSuitSymbols.Add(ch);
                    }
                    DrawUnicodeTable(playingCardSuitSymbols);
                    break;
                // Musical (7)
                case UnicodeCategory.Musical:
                    List<char> musicalSymbols = new List<char>();
                    foreach (char ch in MusicalNames.Keys)
                    {
                        musicalSymbols.Add(ch);
                    }
                    DrawUnicodeTable(musicalSymbols);
                    break;
                // Other (18)
                case UnicodeCategory.Other:
                    List<char> otherSymbols = new List<char>();
                    foreach (char ch in OtherNames.Keys)
                    {
                        otherSymbols.Add(ch);
                    }
                    DrawUnicodeTable(otherSymbols);
                    break;
                // Favorites
                case UnicodeCategory.Favorites:
                    if (favoriteUnicodeSymbols.Count == 0)
                        break;
                    DrawUnicodeTable(favoriteUnicodeSymbols);
                    break;
            }
        }

        /// <summary>
        /// Add the Unicode symbol naes from the given name list to the global list.
        /// </summary>
        /// <param name="nameList">List of Unicode symbol names.</param>
        private void AddNames(Dictionary<char, string> nameList)
        {
            foreach (var pair in nameList)
            {
                if (!AllUnicodeNames.ContainsKey(pair.Key))
                    AllUnicodeNames.Add(pair.Key, pair.Value);
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
            RemoveFromFavorites(favoriteSymbolPending, true);
        }

        /// <summary>
        /// Remove a Unicode symbol from the Favorites list. 
        /// </summary>
        public void RemoveFromFavorites(char us, bool showNotification)
        {
            char character = us;
            if (!HasFavoriteSymbol(favoriteSymbolPending) || favoriteUnicodeSymbols.Count == 0)
                return;

            MUSTEditorPrefs.DeleteKey(GetFavoriteSymbolKey(favoriteUnicodeSymbols.IndexOf(character)));
            favoriteUnicodeSymbols.Remove(character);
            ReassignFavoriteSymbolKeys();

            // Display quick notification.
            if (showNotification)
            {
                window.ShowNotification(new GUIContent($"{character}\n\nRemoved from\n★ Favorites!"));
            }
        }

        /// <summary>
        /// Toggle the Favorite button.
        /// </summary>
        public void ToggleFavorite()
        {
            // If the selected Unicode symbol is favorited, remove it from ★ Favorites.
            if (HasFavoriteSymbol(selectedSymbol))
            {
                favoriteSymbolPending = selectedSymbol;
                RemoveFromFavorites(selectedSymbol, true);
            }
            // Otherwise, add the selected Unicode symbol to ★ Favorites.
            else
            {
                AddToFavorites(selectedSymbol);
            }
        }

        /// <summary>
        /// Get the specified favorite symbol key.
        /// </summary>
        /// <param name="value">Integer representation of the Unicode character.</param>
        /// <returns>Favorite Unicode symbol key string.</returns>
        public string GetFavoriteSymbolKey(int value) => $"Favorite Symbol[{value}]";

        /// <summary>
        /// Reassign the user's favorite Unicode symbols by key string.
        /// </summary>
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

        /// <summary>
        /// Check if the specified Unicode symbol is favorited in ★ Favorites.
        /// </summary>
        /// <param name="us">Unicode character symbol.</param>
        /// <returns>TRUE if ★ Favorites contains the specified Unicode symbol. Otherwise, FALSE.</returns>
        private bool HasFavoriteSymbol(char us)
        {
            return favoriteUnicodeSymbols.Contains(us);
        }
        #endregion

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
        /// <returns>Hexadecimal string.</returns>
        public static string CharToHexCode(char ch)
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
            return "U+" + CharToHexCode(ch);
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
            return "\\" + CharToHexCode(ch);
        }
        #endregion

        #region Draw Method(s)
        /// <summary>
        /// Draw the Unicode table in the editor window.
        /// </summary>
        /// <param name="characters">List of Unicode character symbols.</param>
        private void DrawUnicodeTable(List<char> characters)
        {
            bool lastSymbolButton = false;
            int numOfColumns = (int)(windowWidth / 31f);
            int numOfRows = Mathf.CeilToInt((float)characters.Count / (float)numOfColumns);

            for (int y = 0; y < numOfRows; y++)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < numOfColumns; x++)
                {
                    int index = x + (numOfColumns * y);
                    lastSymbolButton = index == characters.Count - 1;
                    char us = characters[index];
                    if (!unicodeSymbols.Contains(us))
                    {
                        unicodeSymbols.Add(us);
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
        /// Draw the specified Unicode character symbol on a button.
        /// </summary>
        /// <param name="us">Unicode character symbol.</param>
        private void DrawSymbolButton(char us)
        {
            if (GUILayout.Toggle(false, us.ToString(), symbolButtonStyle))
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
        private void DrawBulletPoint(string bulletPointColor, int indents = 0)
        {
            // GUI Style: Bullet Point
            bulletPointStyle = new GUIStyle(GUI.skin.label)
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

        /// <summary>
        /// Draw all selectable color styles in the editor window.
        /// </summary>
        private void DrawColorStyles()
        {
            bool lastSymbolButton = false;
            int numOfColumns = (int)(windowWidth / 51.1f);
            int numOfRows = Mathf.CeilToInt((float)ColorStyles.Length / (float)numOfColumns);

            for (int x = 0; x < numOfRows; x++)
            {
                GUILayout.BeginHorizontal();
                for (int y = 0; y < numOfColumns; y++)
                {
                    int index = y + (numOfColumns * x);
                    lastSymbolButton = index == ColorStyles.Length - 1;
                    GUI.backgroundColor = AddColor(ColorStyles[index]) * 1.75f;
                    string checkmark = mainColorStyle.Equals(ColorStyles[index]) ? "✓" : string.Empty;
                    if (GUILayout.Button(checkmark, swatchStyle))
                    {
                        SetColorStyle(index);
                    }
                    GUI.backgroundColor = Color.white;

                    if (lastSymbolButton)
                        break;
                }
                GUILayout.EndHorizontal();

                if (lastSymbolButton)
                    break;
            }
        }
        #endregion

        #region Custom Context Menus

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
                AddMenuItem(menu, "Add to ★ Favorites", AddToFavorites);
                AddMenuItem(menu, "Remove from ★ Favorites", RemoveFromFavorites);
                menu.ShowAsContext();

                current.Use();
            }
        }

        /// <summary>
        /// Add new menu item to a context menu.
        /// </summary>
        /// <param name="menu">Context menu.</param>
        /// <param name="menuPath">Menu path.</param>
        /// <param name="method">The specified method to invoke when the user selects the menu item.</param>
        private void AddMenuItem(GenericMenu menu, string menuPath, GenericMenu.MenuFunction method)
        {
            menu.AddItem(new GUIContent(menuPath), false, method);
        }
        #endregion

        #region Miscellaneous
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
}
