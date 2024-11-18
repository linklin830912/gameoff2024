using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    
    internal static bool moveToObject(PlayerMovementEnum movement) {
        if (currentSpawner.checkNextObject(movement))
        {
            currentTextObject = currentSpawner.getNextObject(movement);
            currentTextObject.setToStaticColorCode();
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        else if (checkHaveSpawner(movement))
        {
            currentTextObject = getTextObject(movement);
            currentTextObject.setToStaticColorCode();
            PlayerMovement.setScale(currentSpawner.gameObject);
            PlayerMovement.setPosition(currentTextObject.getCursorPosition());
            return true;
        }
        else { 
            Debug.Log("back to start");
            return false;
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
            case PlayerMovementEnum.Up:
                return currentSpawner.prevSpawner != null;
            default:
                return false;
        }
    }

    private static TextObject getTextObject(PlayerMovementEnum movment) {
        switch (movment) { 
            case PlayerMovementEnum.Left:
                currentSpawner = currentSpawner.prevSpawner;
                bool isValidMove = MaskMovement.detectValidPlayerMovement(currentSpawner.getCurrentObject())
                    && PassageManager.checkValidPassage(currentSpawner.getCurrentObject());
                if (!isValidMove) currentSpawner = currentSpawner.nextSpawner;
                return currentSpawner.getCurrentObject();
            case PlayerMovementEnum.Right:
                currentSpawner = currentSpawner.nextSpawner;
                isValidMove = MaskMovement.detectValidPlayerMovement(currentSpawner.getCurrentObject())
                    && PassageManager.checkValidPassage(currentSpawner.getCurrentObject());
                if (!isValidMove) currentSpawner = currentSpawner.prevSpawner;
                return currentSpawner.getCurrentObject();
            case PlayerMovementEnum.Up:
                currentSpawner = currentSpawner.prevSpawner;
                isValidMove = MaskMovement.detectValidPlayerMovement(currentSpawner.getCurrentObject())
                    && PassageManager.checkValidPassage(currentSpawner.getCurrentObject());
                if (!isValidMove) {
                    currentSpawner = currentSpawner.nextSpawner;
                    return currentTextObject;
                }
                TextObject textObj = currentSpawner.getClosestObject(currentTextObject, false);
                if(currentTextObject.Equals(textObj)) currentSpawner = currentSpawner.nextSpawner;
                return textObj;
            case PlayerMovementEnum.Down:
                currentSpawner = currentSpawner.nextSpawner;
                isValidMove = MaskMovement.detectValidPlayerMovement(currentSpawner.getCurrentObject())
                    && PassageManager.checkValidPassage(currentSpawner.getCurrentObject());
                if (!isValidMove) {
                    currentSpawner = currentSpawner.prevSpawner;
                    return currentTextObject;
                }
                textObj = currentSpawner.getClosestObject(currentTextObject, false);
                if(currentTextObject.Equals(textObj)) currentSpawner = currentSpawner.prevSpawner;
                return textObj;            
            default:
                return null;
        }
    }
}
