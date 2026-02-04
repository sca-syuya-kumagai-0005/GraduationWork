using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum SecureClass
{
    Secra = 0,
    Vigil,
    Hazra,
    Catra,
    Nulla,
    MAX
}
public enum LiskClass
{
    Lumenis = 0,
    Velgra,
    Dravex,
    Zerath,
    Oblivara,
    MAX
}

[Serializable]
public struct ConditionalText
{
    public bool isHoused;
    [TextArea] public string Text;
    [TextArea] public string housedText;
}

[Serializable]
public struct ExplanatoryText
{
    public bool isHoused;
    public string headingText;
    [TextArea] public string Text;
}

[Serializable]
public struct SinnerMemoryData
{
    public string name;
    public Sprite sinnerImage;
    public SecureClass secureClass;
    public LiskClass liskClass;
    public string[] emotionValue;
    public ConditionalText[] conditionalTexts;
    public ExplanatoryText[] explanatoryTexts;
}
