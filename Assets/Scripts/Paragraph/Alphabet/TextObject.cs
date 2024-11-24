using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextObject
{
    internal TextObject prevObject;
    internal TextObject nextObject;
    internal TextObject prevLineObject;
    internal TextObject nextLineObject;
    internal GameObject value;
    internal bool isStartLineObject = false;
    internal bool isEndLineObject = false;
    internal float fontWidth;
    private string font;
    internal string alphabet;
    internal int colorCode;

    public TextObject(GameObject currentObject, string font, string alphabet, int colorCode, float fontWidth) {
        this.value = currentObject;
        this.font = font;
        this.alphabet = alphabet;
        this.colorCode = colorCode;
        this.fontWidth = fontWidth;
        MaskManager.AssignAlphabetColor(colorCode, currentObject);
    }
    internal void setPrevObject(TextObject prevObject, bool isLineStart) {
        this.prevObject = prevObject;
        if (this.prevObject != null){
            this.prevObject.isEndLineObject = isLineStart; // if it is in the start of line, then its prev should be in the end
            this.prevObject.nextObject = this;
        }
    }
    internal void setPrevLineObject(TextObject prevLineObj, bool isLineStart) {

        if (prevLineObj != null) {
            if (!isLineStart)
            {
                if (!prevLineObj.isEndLineObject && !prevLineObj.isStartLineObject){ 
                    double minDis = Math.Abs(this.value.transform.position.x - prevLineObj.value.transform.position.x);
                    TextObject currentPrevObj = prevLineObj;
                    TextObject targetPrevObj = prevLineObj;
                    while (true) { 
                        double dis = Math.Abs(this.value.transform.position.x - currentPrevObj.value.transform.position.x);
                        if (dis < minDis) {
                            targetPrevObj = currentPrevObj;
                            minDis = dis;
                        }
                        if (currentPrevObj.nextObject != null &&  
                            Math.Abs(this.value.transform.position.y - currentPrevObj.value.transform.position.y)>0.01)
                            currentPrevObj = currentPrevObj.nextObject;
                        else break;
                        
                    }
                    prevLineObj = targetPrevObj;
                }

            }
            
            this.prevLineObject = prevLineObj;
            if (this.prevLineObject != null){
                this.prevLineObject.nextLineObject = this;
                TextObject prevLinePrevObj = this.prevLineObject.prevObject;
                TextObject beforeThis = this;
                List<TextObject> unassignNextLineObjects = new List<TextObject>();
                while (prevLinePrevObj != null) {
                    if (prevLinePrevObj.nextLineObject == null) {
                        unassignNextLineObjects.Add(prevLinePrevObj);
                        prevLinePrevObj = prevLinePrevObj.prevObject;
                    }else{
                        beforeThis = prevLinePrevObj.nextLineObject;
                        break; 
                    }                   
                }
                foreach (var unObj in unassignNextLineObjects) {
                    double dis0 = Math.Abs(unObj.value.transform.position.x - beforeThis.value.transform.position.x);
                    double dis1 = Math.Abs(unObj.value.transform.position.x - this.value.transform.position.x);
                    unObj.nextLineObject = dis0 < dis1 ? beforeThis : this;
                }
            }
        }
    }
    internal Vector3 getCursorPosition() {
        Vector2 size = TextDictionaryGenerator.getFontSize(this.value);
        Vector3 position = value.transform.position;
        position.x -= size.x / 2;
        position.z -= 1;
        return position;
    }
    internal void setup(TextObject prevObj, TextObject prevLineObj, bool isLineStart) {
        this.setPrevObject(prevObj, isLineStart);
        this.setPrevLineObject(prevLineObj, isLineStart);
    }

    internal float getDistance( TextObject textObject) {
        Vector3 pos0 = this.value.transform.position;
        pos0.z = 0;
        Vector3 pos1 = textObject.value.transform.position;
        pos1.z = 0;
        return Vector3.Distance(pos0, pos1);
    }
    internal void setToStaticColorCode() {
        this.colorCode = -1;
        MaskManager.AssignAlphabetColor(this.colorCode, this.value);// set to differnt color
    }
    
}
