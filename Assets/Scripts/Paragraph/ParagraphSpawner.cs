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
    private TextObject endObject;
    private TextObject currentObject;
    
    internal ParagraphSpawner prevSpawner;
    internal ParagraphSpawner nextSpawner;
  
    internal void Init()
    {        
        paragraphGeneratorObject = (ParagraphObject)spawnObject;
        Vector3 position = gameObject.transform.position;
        TextObject prevObject = null;
        TextObject startLineObject = null;
        TextObject prevLineObject = null;
        bool isLineStart = false;
        if (paragraphGeneratorObject != null) {
            for(int i = 0; i <paragraphGeneratorObject.content.Length; i++) {
                if(i==0)isLineStart = true;
                String alphabet = paragraphGeneratorObject.content.Substring(i, 1);
                if (alphabet.Equals(TextDictionaryGenerator._ENTER) || Math.Abs(gameObject.transform.position.x - position.x) > paragraphGeneratorObject.maxWidth)
                {
                    // line break                    
                    position.x = gameObject.transform.position.x;
                    position.y -= paragraphGeneratorObject.lineHeight;
                    prevLineObject = startLineObject;
                    isLineStart = true;
                    if (alphabet.Equals(TextDictionaryGenerator._SPACE)) continue;
                }
                if (alphabet.Equals(TextDictionaryGenerator._ENTER)) continue;

                GameObject alphabetObject = TextDictionaryGenerator.getFontAlphabet(paragraphGeneratorObject.font, alphabet, gameObject);
                positionAlphabet(alphabetObject, ref position);

                TextObject textObject = new TextObject(alphabetObject, paragraphGeneratorObject.font, alphabet);
                textObject.setup(prevObject, prevLineObject, isLineStart);

                if (isLineStart) startLineObject = textObject;
                if (startObject == null) startObject = textObject;
                prevLineObject = textObject.prevLineObject;
                prevObject = textObject;
                isLineStart = false;
                endObject = textObject;
            }
        }
        currentObject = startObject;
    }
    private void positionAlphabet(GameObject alphabetObject, ref Vector3 position) { 
        alphabetObject.SetActive(true);
        float fontWidth =  TextDictionaryGenerator.getFontSize(alphabetObject).x/2;
        position.x += fontWidth+paragraphGeneratorObject.wordSpacing;
        alphabetObject.transform.position = position;
        position.x += fontWidth;
    }

    internal void setPrevSpawner(ParagraphSpawner prevSpawner) { 
        this.prevSpawner = prevSpawner;
        if(this.prevSpawner != null)
            this.prevSpawner.nextSpawner = this;
    }
    internal TextObject getCurrentObject() {
        return this.currentObject;
    }
    internal TextObject getClosestObject(TextObject textObject, bool fromStart) {
        TextObject beginAtObject = fromStart ? this.startObject : this.endObject;
        TextObject closestObject = beginAtObject;
        float dis = beginAtObject.getDistance(textObject);
        float currentDis = dis;
        while (beginAtObject != null) {
            currentDis = beginAtObject.getDistance(textObject);
            if (currentDis < dis) {
                closestObject = beginAtObject;
                dis = currentDis;
            }
            beginAtObject = fromStart ? beginAtObject.nextObject : beginAtObject.prevObject; 
        }
        this.currentObject = closestObject;
        return closestObject;
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
    
    internal bool nextLineObject() {
        if (this.currentObject.nextLineObject != null)
        {
            this.currentObject = this.currentObject.nextLineObject;
            
            return true;
        }
        return false;
    }

    internal bool prevLineObject() {
        if (this.currentObject.prevLineObject != null)
        {
            this.currentObject = this.currentObject.prevLineObject;
            return true;
        }
        return false;
    }
}
