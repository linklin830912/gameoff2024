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
    internal GameObject value;
    private string font;
    private string alphabet;

    public TextObject (GameObject currentObject, string font, string alphabet){
        this.value = currentObject;
        this.font = font;
        this.alphabet = alphabet;
    }
    internal void setPrevObject(TextObject prevObject) { 
        this.prevObject = prevObject;
        if(this.prevObject != null)
            this.prevObject.nextObject = this;
    }
    internal Vector3 getCursorPosition() {
        Vector2 size = TextDictionaryGenerator.getFontSize(this.value);
        Vector3 position = value.transform.position;
        position.x-=size.x/2;
        return position;
    }
    
}
