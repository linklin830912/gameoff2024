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

    public TextObject(GameObject currentObject, string font, string alphabet) {
        this.value = currentObject;
        this.font = font;
        this.alphabet = alphabet;
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
                if (prevLineObj.nextObject != null)
                    dis1 = Math.Abs(this.value.transform.position.x - prevLineObj.nextObject.value.transform.position.x);

                if (dis1 < dis0)
                    prevLineObj = prevLineObj.nextObject;

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
        return position;
    }
    internal void setup(TextObject prevObj, TextObject prevLineObj, bool isLineStart) { 
        this.setPrevObject(prevObj);
        this.setPrevLineObject(prevLineObj, isLineStart);
    }

    internal float getDistance( TextObject textObject) {
        return Vector3.Distance(this.value.transform.position, textObject.value.transform.position);
    }
    
}
