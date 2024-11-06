using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ParagraphSpawner : MonoBehaviour
{
    [SerializeField]
    private ScriptableObject spawnObject;
    private ParagraphObject paragraphGeneratorObject;
    private TextObject startObject;
    private TextObject currentObject;
    
    internal ParagraphSpawner prevSpawner;
    internal ParagraphSpawner nextSpawner;
  
    internal void Init()
    {        
        paragraphGeneratorObject = (ParagraphObject)spawnObject;
        Vector3 position = gameObject.transform.position;
        TextObject prevObject = null;
        if (paragraphGeneratorObject != null) {
            for(int i = 0; i <paragraphGeneratorObject.content.Length; i++) {
                String alphabet = paragraphGeneratorObject.content.Substring(i, 1);
                if (alphabet.Equals("\n") || Math.Abs(gameObject.transform.position.x - position.x) > paragraphGeneratorObject.maxWidth)
                {
                    // line break
                    position.x = gameObject.transform.position.x;
                    position.y -= paragraphGeneratorObject.lineHeight;
                    if (alphabet.Equals(" ")) continue;
                }
                if (alphabet.Equals("\n")) continue;
                if (alphabet.Equals(" ")) alphabet = "space";                
                GameObject alphabetObject = TextDictionaryGenerator.getFontAlphabet(paragraphGeneratorObject.font, alphabet, gameObject);
                alphabetObject.SetActive(true);
                TextObject textObject = new TextObject(alphabetObject, paragraphGeneratorObject.font, alphabet);
                if (startObject == null) startObject = textObject;
                textObject.setPrevObject(prevObject);
                float fontWidth =  TextDictionaryGenerator.getFontSize(alphabetObject).x/2;
                position.x += fontWidth+paragraphGeneratorObject.wordSpacing;
                alphabetObject.transform.position = position;
                position.x += fontWidth;
                prevObject = textObject;    
            }
        }
        currentObject = startObject;
    }

    internal void setPrevSpawner(ParagraphSpawner prevSpawner) { 
        this.prevSpawner = prevSpawner;
        if(this.prevSpawner != null)
            this.prevSpawner.nextSpawner = this;
    }
    internal TextObject getCurrentObject() {
        return this.currentObject;
    }
    internal bool nextObject() {
        if (this.currentObject.nextObject != null)
        {
            this.currentObject = this.currentObject.nextObject;
            return true;
        }
        return false;
    }

    internal bool prevObject() {
        if (this.currentObject.prevObject != null)
        {
            this.currentObject = this.currentObject.prevObject;
            return true;
        }
        return false;
    }

}
