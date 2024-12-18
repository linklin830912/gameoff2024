using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ParagraphSpawner : MonoBehaviour
{
    [SerializeField]
    private ScriptableObject spawnObject;
    private ParagraphObject paragraphGeneratorObject;
    private TextObject startObject;
    private TextObject endObject;
    private TextObject currentObject;
    private int colorCode;
    private Dictionary<int, ParagraphSpawner> nextSpawners;
    private Dictionary<int, ParagraphSpawner> prevSpawners;
    [SerializeField]
    private List<GameObject> prevSpawnerObjects;
    
    internal void Init()
    {
        paragraphGeneratorObject = (ParagraphObject)spawnObject;
        colorCode = getColorCode();
        setupPrevNextSpawner();
        Vector3 position = gameObject.transform.position;
        TextObject prevObject = null;
        TextObject startLineObject = null;
        TextObject prevLineObject = null;
        bool isLineStart = false;
        if (paragraphGeneratorObject != null) {
            for(int i = 0; i <paragraphGeneratorObject.content.Length; i++) {
                if(i==0)isLineStart = true;
                String alphabet = paragraphGeneratorObject.content.Substring(i, 1);
                float offsetX = prevObject != null ? TextDictionaryGenerator.getFontSize(prevObject.value).x:0;
                if (alphabet.Equals(TextDictionaryGenerator._ENTER)
                    || Math.Abs(gameObject.transform.position.x - offsetX - position.x) > paragraphGeneratorObject.maxWidth)
                {
                    // line break               
                    position.x = gameObject.transform.position.x;
                    if (paragraphGeneratorObject.isReversedY)
                    {
                        position.y += paragraphGeneratorObject.lineHeight;
                    }
                    else { 
                        position.y -= paragraphGeneratorObject.lineHeight;
                    }
                    
                    prevLineObject = startLineObject;
                    isLineStart = true;
                    if (alphabet.Equals(TextDictionaryGenerator._SPACE)) continue;
                }
                if (alphabet.Equals(TextDictionaryGenerator._ENTER)) continue;

                GameObject alphabetObject = TextDictionaryGenerator.getFontAlphabet(paragraphGeneratorObject.font, alphabet, gameObject);
                if (alphabetObject==null) continue;
                float fontWidth = positionAlphabet(alphabetObject, ref position);

                TextObject textObject = new TextObject(alphabetObject, paragraphGeneratorObject.font, alphabet, colorCode, fontWidth);
                textObject.setup(prevObject, prevLineObject, isLineStart);

                if (isLineStart) {startLineObject = textObject;}
                if (startObject == null) startObject = textObject;
                prevLineObject = textObject.prevLineObject;
                prevObject = textObject;
                isLineStart = false;
                endObject = textObject;
                if (i == paragraphGeneratorObject.content.Length - 1) textObject.isEndLineObject = true;
            }
            
        }
        currentObject = startObject;
    }
    private void setupPrevNextSpawner() {
        this.prevSpawners = new Dictionary<int, ParagraphSpawner>();
        this.nextSpawners = new Dictionary<int, ParagraphSpawner>();
        foreach (var prevSpawnerObj in prevSpawnerObjects) {
            ParagraphSpawner prevSpawner = prevSpawnerObj.GetComponent<ParagraphSpawner>();

            this.prevSpawners.Add(prevSpawner.colorCode, prevSpawner);
            prevSpawner.nextSpawners.Add(this.colorCode, this);
        }
    }
    private float positionAlphabet(GameObject alphabetObject, ref Vector3 position) { 
        alphabetObject.SetActive(true);
        float fontWidth =  TextDictionaryGenerator.getFontSize(alphabetObject).x/2;
        position.x += fontWidth+paragraphGeneratorObject.wordSpacing;
        alphabetObject.transform.position = position;
        position.x += fontWidth;
        return fontWidth;
    }


    internal TextObject getCurrentObject() {
        return this.currentObject;
    }
    internal TextObject getClosestObject(TextObject textObject, bool fromStart) {
        float disSt = Math.Abs(textObject.value.transform.position.y - this.startObject.value.transform.position.y);
        float disEd = Math.Abs(textObject.value.transform.position.y - this.endObject.value.transform.position.y);
        fromStart = disSt <= disEd;

        TextObject beginAtObject = fromStart ? this.startObject : this.endObject;
        TextObject closestObject = null;
        float dis = beginAtObject.getDistance(textObject);
        float currentDis = dis;
        while (beginAtObject != null) {
            currentDis = beginAtObject.getDistance(textObject);
            if (currentDis < dis && MaskMovement.detectValidPlayerMovement(beginAtObject)) {
                closestObject = beginAtObject;
                dis = currentDis;
            }
            beginAtObject = fromStart ? beginAtObject.nextObject : beginAtObject.prevObject; 
        }
        if (closestObject != null)
            if (Math.Abs(textObject.value.transform.position.x - closestObject.value.transform.position.x) < textObject.fontWidth*2)
                this.currentObject = closestObject;
            else return textObject;
        return this.currentObject;
    }
    
    internal TextObject getNextObject(PlayerMovementEnum movement) {
        switch (movement) { 
            case PlayerMovementEnum.Right:
                if(MaskMovement.detectValidPlayerMovement(this.currentObject.nextObject))
                    if((this.currentObject.colorCode ==-1 && this.currentObject.nextObject.colorCode ==-1) 
                        || PassageManager.checkValidPassage(this.currentObject.nextObject))
                        this.currentObject = this.currentObject.nextObject;
                break;
            case PlayerMovementEnum.Left:
                if(MaskMovement.detectValidPlayerMovement(this.currentObject.prevObject))
                    if((this.currentObject.colorCode ==-1 && this.currentObject.prevObject.colorCode ==-1) 
                        || PassageManager.checkValidPassage(this.currentObject.prevObject))
                        this.currentObject = this.currentObject.prevObject;
                break;
            case PlayerMovementEnum.Up:
                if(MaskMovement.detectValidPlayerMovement(this.currentObject.prevLineObject))
                    if((this.currentObject.colorCode ==-1 && this.currentObject.prevLineObject.colorCode ==-1) 
                        || PassageManager.checkValidPassage(this.currentObject.prevLineObject))
                        this.currentObject = this.currentObject.prevLineObject;
                break;
            case PlayerMovementEnum.Down:
                if(MaskMovement.detectValidPlayerMovement(this.currentObject.nextLineObject))
                    if((this.currentObject.colorCode ==-1 && this.currentObject.nextLineObject.colorCode ==-1) 
                        || PassageManager.checkValidPassage(this.currentObject.nextLineObject))
                        this.currentObject = this.currentObject.nextLineObject;
                break;
            default:
                break;
        }
        return this.currentObject;
    }

    internal bool checkNextObject(PlayerMovementEnum movement) {
        switch (movement) { 
            case PlayerMovementEnum.Right:
                return this.currentObject.nextObject != null;
            case PlayerMovementEnum.Left:
                return this.currentObject.prevObject != null;
            case PlayerMovementEnum.Up:
                return this.currentObject.prevLineObject != null;
            case PlayerMovementEnum.Down:
                return this.currentObject.nextLineObject != null;
            default:
                return false;
        }
    }
    internal bool getIsReverseY() { 
        return ((ParagraphObject)spawnObject).isReversedY;
    }
    
    internal int getColorCode() {
        return ((ParagraphObject)spawnObject).colorCode;
    }
    internal ParagraphSpawner getCurrentPrevSpawner() {
        if (prevSpawners.GetValueOrDefault(MaskManager.currentMaskIndex) != null)
            return prevSpawners.GetValueOrDefault(MaskManager.currentMaskIndex);
        return prevSpawners.GetValueOrDefault(MaskManager.STATIC_COLOR_CODE);
    }
    internal ParagraphSpawner getCurrentNextSpawner() { 
        if (nextSpawners.GetValueOrDefault(MaskManager.currentMaskIndex) != null)
            return nextSpawners.GetValueOrDefault(MaskManager.currentMaskIndex);
        return nextSpawners.GetValueOrDefault(MaskManager.STATIC_COLOR_CODE);        
    }
}
