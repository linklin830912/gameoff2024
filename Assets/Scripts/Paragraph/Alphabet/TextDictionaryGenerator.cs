using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDictionaryGenerator : MonoBehaviour
{
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
                GameObject gSpace = textDictionaryObjects[i].transform.Find("space").gameObject;
                alphabetDictionary.Add("space", gSpace );
                gSpace.SetActive(false);
                GameObject gEnter = textDictionaryObjects[i].transform.Find("enter").gameObject;
                alphabetDictionary.Add("enter", gEnter );
                gEnter.SetActive(false);
                fontDictionary[textDictionaryObjects[i].name] = alphabetDictionary;
            }
            
        }   
    }

    internal static GameObject getFontAlphabet(string font, string alphabet, GameObject parent) {
        return Instantiate(fontDictionary[font][alphabet], parent.transform);
    }
    internal static Vector2 getFontSize(GameObject gameObject) {
        return gameObject.GetComponent<BoxCollider>().bounds.size;
    }
}
