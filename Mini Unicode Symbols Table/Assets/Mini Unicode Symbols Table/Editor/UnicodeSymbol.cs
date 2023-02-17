using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnicodeSymbol
{
    public UnicodeSymbol(char _character)
    {
        character = _character;
        isSelected = false;
    }

    public char character;
    public string hexCode;
    public string htmlCode;
    public bool isSelected;
}
