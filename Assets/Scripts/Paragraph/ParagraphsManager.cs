using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParagraphsManager : MonoBehaviour
{
    public static ParagraphsManager paragraphsManagerInstance;
    private static ParagraphSpawner startSpawner;
    private static ParagraphSpawner currentSpawner;
    private static TextObject currentTextObject;
    void Start() {
        if (paragraphsManagerInstance == null){
            paragraphsManagerInstance = this;
            ParagraphSpawner prevSpawner = null;
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                GameObject childGameObject = gameObject.transform.GetChild(i).gameObject;
                ParagraphSpawner paragraphSpawner = childGameObject.GetComponent<ParagraphSpawner>();
                paragraphSpawner.Init();
                if(startSpawner == null)startSpawner = paragraphSpawner;
                paragraphSpawner.setPrevSpawner(prevSpawner);
                prevSpawner = paragraphSpawner;
            }
        }
        currentSpawner = startSpawner;
        PlayerMovement.setScale(currentSpawner.gameObject);
        PlayerMovement.setPosition(currentSpawner.getCurrentObject().getCursorPosition());
    }

    
    internal static bool moveToNextObject() {
        bool hasNext = currentSpawner.nextObject();
        if (hasNext)
        {
            currentTextObject = currentSpawner.getCurrentObject();
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        else if (currentSpawner.nextSpawner != null)
        {
            currentSpawner = currentSpawner.nextSpawner;
            currentTextObject = currentSpawner.getCurrentObject();
            PlayerMovement.setScale(currentSpawner.gameObject);
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        else { 
            Debug.Log("game complete");
            return false;
        }        
    }

    internal static bool moveToPrevObject() {
        bool hasPrev = currentSpawner.prevObject();
        if (hasPrev)
        {
            currentTextObject = currentSpawner.getCurrentObject();
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        else if (currentSpawner.prevSpawner != null)
        {
            currentSpawner = currentSpawner.prevSpawner;
            currentTextObject = currentSpawner.getCurrentObject();
            PlayerMovement.setScale(currentSpawner.gameObject);
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        else { 
            Debug.Log("back to start");
            return false;
        }        
    }
    internal static bool moveToNextLineObject(){
        bool hasNext = currentSpawner.nextLineObject();
        if (hasNext)
        {
            currentTextObject = currentSpawner.getCurrentObject();
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        else if (currentSpawner.nextSpawner != null) { 
            currentSpawner = currentSpawner.nextSpawner;
            currentTextObject = currentSpawner.getClosestObject(currentTextObject, true);
            PlayerMovement.setScale(currentSpawner.gameObject);
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        return false;
    }

    internal static bool moveToPrevLineObject(){
        bool hasNext = currentSpawner.prevLineObject();
        if (hasNext)
        {
            currentTextObject = currentSpawner.getCurrentObject();
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        else if (currentSpawner.prevSpawner != null) { 
            currentSpawner = currentSpawner.prevSpawner;
            currentTextObject = currentSpawner.getClosestObject(currentTextObject, false);
            PlayerMovement.setScale(currentSpawner.gameObject);
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        return false;
    }
}
