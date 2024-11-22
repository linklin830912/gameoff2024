using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDictionaryGenerator : MonoBehaviour
{
    private static string COMMA = "comma";
    public static string _COMMA = ",";
    private static string PERIOD = "period";
    public static string _PERIOD = ".";
    private static string SPACE = "space";
    public static string _SPACE = " ";
    private static string ENTER = "enter";
    public static string _ENTER = "\n";
    private static string QUESTION = "question";
    public static string _QUESTION = "?";
    private static string SINGLE_QUOTE = "squote";
    public static string _SINGLE_QUOTE = "\'";
    private static string DOUBLE_QUOTE = "dquote";
    public static string _DOUBLE_QUOTE = "\"";

    [SerializeField]
    private GameObject[] textDictionaryObjects;
    private static Dictionary<string, Dictionary<string, GameObject>> fontDictionary;
    private static Dictionary<string, GameObject> alphabetDictionary;
    public static TextDictionaryGenerator serifDictionaryGeneratorInstance;
    void Awake() {
        if (serifDictionaryGeneratorInstance == null){
            serifDictionaryGeneratorInstance = this;
            fontDictionary = new Dictionary<string, Dictionary<string, GameObject>>();
            for (int i=0;i<textDictionaryObjects.Length;i++) { 
                alphabetDictionary = new Dictionary<string, GameObject>();
                for (char c = 'a'; c <= 'z'; c++)
                {
                    GameObject g = textDictionaryObjects[i].transform.Find(c.ToString()).gameObject;
                    alphabetDictionary.Add(c.ToString(), g);
                    g.SetActive(false);
                }

                for (char c = 'A'; c <= 'Z'; c++)
                {
                    GameObject g = textDictionaryObjects[i].transform.Find(c.ToString()).gameObject;
                    alphabetDictionary.Add(c.ToString(), g);
                    g.SetActive(false);
                }
                GameObject gSpace = textDictionaryObjects[i].transform.Find(SPACE).gameObject;
                alphabetDictionary.Add(SPACE, gSpace );
                gSpace.SetActive(false);
                GameObject gEnter = textDictionaryObjects[i].transform.Find(ENTER).gameObject;
                alphabetDictionary.Add(ENTER, gEnter );
                gEnter.SetActive(false);
                GameObject gComma = textDictionaryObjects[i].transform.Find(COMMA).gameObject;
                alphabetDictionary.Add(COMMA, gComma);
                gComma.SetActive(false);
                GameObject gPeriod = textDictionaryObjects[i].transform.Find(PERIOD).gameObject;
                alphabetDictionary.Add(PERIOD, gPeriod);
                gPeriod.SetActive(false);

                GameObject gQuestion = textDictionaryObjects[i].transform.Find(QUESTION).gameObject;
                alphabetDictionary.Add(QUESTION, gQuestion );
                gQuestion.SetActive(false);
                GameObject gSquote = textDictionaryObjects[i].transform.Find(SINGLE_QUOTE).gameObject;
                alphabetDictionary.Add(SINGLE_QUOTE, gSquote );
                gSquote.SetActive(false);
                GameObject gDquote = textDictionaryObjects[i].transform.Find(DOUBLE_QUOTE).gameObject;
                alphabetDictionary.Add(DOUBLE_QUOTE, gDquote );
                gDquote.SetActive(false);

                fontDictionary[textDictionaryObjects[i].name] = alphabetDictionary;
            }
            
        }   
    }

    internal static GameObject getFontAlphabet(string font, string alphabet, GameObject parent) {
        if (alphabet.Equals(_SPACE)) alphabet = SPACE; 
        else if (alphabet.Equals(_COMMA)) alphabet = COMMA;  
        else if (alphabet.Equals(_PERIOD)) alphabet = PERIOD; 
        else if (alphabet.Equals(_QUESTION)) alphabet = QUESTION; 
        else if (alphabet.Equals(_SINGLE_QUOTE)) alphabet = SINGLE_QUOTE;
        else if (alphabet.Equals(_DOUBLE_QUOTE)) alphabet = DOUBLE_QUOTE;
        return Instantiate(fontDictionary[font][alphabet], parent.transform);
    }
    internal static Vector2 getFontSize(GameObject gameObject) {
        return gameObject.GetComponent<BoxCollider>().bounds.size;
    }
}
