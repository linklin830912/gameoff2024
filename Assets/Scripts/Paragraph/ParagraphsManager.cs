using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                paragraphSpawner.Init(paragraphSpawner.getColorCode());
                MaskManager.AddToColorParagraphList(paragraphSpawner.getColorCode(), paragraphSpawner);
                MaskManager.SetupDepthForColorCode(paragraphSpawner.getColorCode(), paragraphSpawner);
                if(startSpawner == null)startSpawner = paragraphSpawner;
                paragraphSpawner.setPrevSpawner(prevSpawner);
                prevSpawner = paragraphSpawner;
            }
        }
        currentSpawner = startSpawner;
        PlayerMovement.setScale(currentSpawner.gameObject);
        PlayerMovement.setPosition(currentSpawner.getCurrentObject().getCursorPosition());
    }    
    
    internal static bool moveToObject(PlayerMovementEnum movment) {
        if (checkHaveNext(movment))
        {
            currentTextObject = currentSpawner.getCurrentObject();
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        else if (checkHaveSpawner(movment))
        {
            currentTextObject = getTextObject(movment);
            PlayerMovement.setScale(currentSpawner.gameObject);
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        else { 
            Debug.Log("back to start");
            return false;
        }
    }

    private static bool checkHaveNext(PlayerMovementEnum movment) {
        switch (movment) { 
            case PlayerMovementEnum.Left:
                return currentSpawner.prevObject();
            case PlayerMovementEnum.Right:
                return currentSpawner.nextObject();
            case PlayerMovementEnum.Down:
                return currentSpawner.nextLineObject();
            default:
                return currentSpawner.prevLineObject();
        }
    }

    private static bool checkHaveSpawner(PlayerMovementEnum movment) {
        switch (movment) { 
            case PlayerMovementEnum.Left:
                return currentSpawner.prevSpawner != null;
            case PlayerMovementEnum.Right:
                return currentSpawner.nextSpawner != null;
            case PlayerMovementEnum.Down:
                return currentSpawner.nextSpawner != null;
            default:
                return currentSpawner.prevSpawner != null;
        }
    }

    private static TextObject getTextObject(PlayerMovementEnum movment) {
        switch (movment) { 
            case PlayerMovementEnum.Left:
                currentSpawner = currentSpawner.prevSpawner;
                return currentSpawner.getCurrentObject();
            case PlayerMovementEnum.Up:
                currentSpawner = currentSpawner.prevSpawner;
                return currentSpawner.getClosestObject(currentTextObject, false);
            case PlayerMovementEnum.Down:
                currentSpawner = currentSpawner.nextSpawner;
                return currentSpawner.getClosestObject(currentTextObject, true);
            default:
                currentSpawner = currentSpawner.nextSpawner;
                return currentSpawner.getCurrentObject();
        }
    }
}
