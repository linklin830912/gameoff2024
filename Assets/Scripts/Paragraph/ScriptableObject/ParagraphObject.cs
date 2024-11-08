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
    public int maxWidth;
    public float wordSpacing;

    // Example of a custom method
    public void UseItem()
    {
        Debug.Log("Using ");
        // Add logic here for what happens when the item is used
    }
    
}
