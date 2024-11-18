using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageManager : MonoBehaviour
{
    internal static PassageManager passageManagerInstance;
    private static string PASSAGE = "passage";
    private static string currentPassage = "";
    void Awake()
    {
        if (passageManagerInstance == null) passageManagerInstance = this;
    }
    internal static bool checkValidPassage(TextObject textObject) {
        if (textObject.colorCode != MaskManager.STATIC_COLOR_CODE)
        {
            string newPassage = currentPassage + textObject.alphabet;
            if (newPassage.Equals(PASSAGE.Substring(0, newPassage.Length)))
            {
                currentPassage = newPassage;
                return true;
            }
        }
        else { 
            if(currentPassage.Equals(PASSAGE))return true;
        }
        return false;
    }
}
