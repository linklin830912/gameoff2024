using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordList : MonoBehaviour
{
    [SerializeField]
    private GameObject answersObj;
    private static string[] words = { "greasy", "shampoo", "sunglasses", "introvert", "neuroscientist", "gymSelfie", "positivity", 
            "BurningSage", "makeup", "planet", "selfAwareness", "TheOneAndOnly"};
    internal static WordList WordListInstance;
    internal static List<GameObject> answers;
    void Awake() {
        if (WordListInstance == null) WordListInstance = this;   
    }
    void Start(){
        answers = new List<GameObject>();
        for (int i = 0; i < answersObj.transform.childCount; i++) {
            answers.Add(answersObj.transform.GetChild(i).gameObject);
            answersObj.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private static int animateComeToScreenHeight = 0;
    void Update() { 
        if(animateComeToScreenHeight>0){
            int moveY = animateComeToScreenHeight;
            animateComeToScreenHeight = 0;
            string currentWord = words[moveY];
            AudioManager.PlayAudioSource(AudioSourceEnum.Checked);
           StartCoroutine((AnimateWordList(moveY, currentWord)));
        }
    }
    private static int substringCount = 0;
    private static string currentWord = "";
    internal static void checkWordComplete(string str) {        
        for (int i=0;i< words.Length;i++) {
            string word = words[i];
            if (str.Length < substringCount + word.Length) continue;
            if (word.Equals(str.Substring(substringCount, word.Length))) { 
                substringCount += word.Length;
                animateComeToScreenHeight = i+1;
                currentWord = word;     
                break;
            }
        }
    }
    IEnumerator AnimateWordList(int moveY,string currentWord)
    {
        Vector3 pos0 = transform.position;
        Vector3 pos1 = pos0;
        pos1.y += moveY*68;
        yield return StartCoroutine(MoveToPosition(transform, pos1, 1f, false));
        yield return StartCoroutine(AnimateTypeString(moveY - 1));     
        yield return StartCoroutine(MoveToPosition(transform, pos0, 1f, true));
        
    }
    IEnumerator MoveToPosition(Transform obj, Vector3 destination, float duration, bool isReset)
    {
        
        Vector3 initialPosition = obj.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.position = Vector3.Lerp(initialPosition, destination, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        obj.position = destination;
    }
    IEnumerator AnimateTypeString(int index) {
        TextMeshProUGUI textMeshPro = answers[index].GetComponent<TextMeshProUGUI>();
        Debug.Log(answers[index]);
        string text = textMeshPro.text;
        textMeshPro.text = "";
        answers[index].SetActive(true);
        for (int i=0; i<text.Length;i++) {
            textMeshPro.text = text.Substring(0, i+1);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
