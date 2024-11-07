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
    private string font;
    private string alphabet;
     private int colorCode;

    public TextObject(GameObject currentObject, string font, string alphabet, int colorCode) {
        this.value = currentObject;
        this.font = font;
        this.alphabet = alphabet;
        this.colorCode = colorCode;
        MaskManager.AssignAlphabetColor(colorCode, currentObject);
    }
    internal void setPrevObject(TextObject prevObject) {
        this.prevObject = prevObject;
        if (this.prevObject != null)
            this.prevObject.nextObject = this;
    }
    internal void setPrevLineObject(TextObject prevLineObj, bool isLineStart) {

        if (prevLineObj != null) {
            if (!isLineStart) {
                //compare with the left neighbor
                double dis0 = Math.Abs(this.value.transform.position.x - prevLineObj.value.transform.position.x);
                double dis1 = 1000;
                double dis2 = 1000;
                double dis3 = 1000;
                double dis4 = 1000;
                if (prevLineObj.nextObject != null){
                    dis1 = Math.Abs(this.value.transform.position.x - prevLineObj.nextObject.value.transform.position.x);
                    if (prevLineObj.nextObject.nextObject != null){
                        dis3 = Math.Abs(this.value.transform.position.x - prevLineObj.nextObject.nextObject.value.transform.position.x);
                        if (prevLineObj.nextObject.nextObject.nextObject != null){
                            dis4 = Math.Abs(this.value.transform.position.x - prevLineObj.nextObject.nextObject.nextObject.value.transform.position.x);
                        }
                    }
                }
                if (prevLineObj.prevObject != null)
                    dis2 = Math.Abs(this.value.transform.position.x - prevLineObj.prevObject.value.transform.position.x);

                double dis = Math.Min(Math.Min(Math.Min(Math.Min(dis0, dis1), dis2), dis3), dis4);
                if (dis==dis1)
                    prevLineObj = prevLineObj.nextObject;
                else if (dis==dis2)
                    prevLineObj = prevLineObj.prevObject;
                else if (dis==dis3)
                    prevLineObj = prevLineObj.nextObject.nextObject;
                else if (dis==dis4)
                    prevLineObj = prevLineObj.nextObject.nextObject.nextObject;
            }

            this.prevLineObject = prevLineObj;
            if (this.prevLineObject != null)
                this.prevLineObject.nextLineObject = this;
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
        this.setPrevObject(prevObj);
        this.setPrevLineObject(prevLineObj, isLineStart);
    }

    internal float getDistance( TextObject textObject) {
        Vector3 pos0 = this.value.transform.position;
        pos0.z = 0;
        Vector3 pos1 = textObject.value.transform.position;
        pos1.z = 0;
        return Vector3.Distance(pos0, pos1);
    }
    
}
