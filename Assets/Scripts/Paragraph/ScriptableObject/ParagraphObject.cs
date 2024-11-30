using System;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "new Paragraph", menuName = "TextTemplate/Paragraph")]
public class ParagraphObject : ScriptableObject
{
    [TextArea(3, 20)]
    public string content;
    public string font;
    public float lineHeight;
    public float maxWidth;
    public float wordSpacing;
    public int colorCode;
    public bool isReversedX;
    public bool isReversedY;    
}
