using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassageManager : MonoBehaviour
{
    internal static PassageManager passageManagerInstance;
    private static string PASSAGE = "greasyshampoosunglassesintrovertneuroscientistgymSelfiepositivityBurningSagemakeupplanetselfAwarenessTheOneAndOnly";
    private static string NEXT = "next";
    private static string currentPassage = "";
    void Awake()
    {
        if (passageManagerInstance == null) passageManagerInstance = this;
    }
    internal static bool checkValidPassage(TextObject textObject) {
        if (textObject.colorCode != MaskManager.STATIC_COLOR_CODE)
        {
            string newPassage = currentPassage + textObject.alphabet;
            if (PASSAGE.Length > newPassage.Length && newPassage.Equals(PASSAGE.Substring(0, newPassage.Length)))
            {
                currentPassage = newPassage;
                WordList.checkWordComplete(currentPassage);
                return true;
            }
            else if (newPassage.Equals(PASSAGE)) {
                PageManager.onGameComplete();
                AudioManager.PlayAudioSource(AudioSourceEnum.Applause);
                return true;
            }
            else if (PASSAGE.Length >= newPassage.Length && !newPassage.Equals(PASSAGE.Substring(0, newPassage.Length))) {
                return false;
            }
            else if (PASSAGE.Length < newPassage.Length) { return false; }
        }
        return true;
    }
}
