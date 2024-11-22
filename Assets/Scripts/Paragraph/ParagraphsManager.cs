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
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                GameObject childGameObject = gameObject.transform.GetChild(i).gameObject;
                ParagraphSpawner paragraphSpawner = childGameObject.GetComponent<ParagraphSpawner>();                
                paragraphSpawner.Init();
                MaskManager.AddToColorParagraphList(paragraphSpawner.getColorCode(), paragraphSpawner);
                MaskManager.SetupDepthForColorCode(paragraphSpawner.getColorCode(), paragraphSpawner);
                if(startSpawner == null)startSpawner = paragraphSpawner;
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
                return currentSpawner.getCurrentPrevSpawner() != null;
            case PlayerMovementEnum.Right:
                return currentSpawner.getCurrentNextSpawner() != null;
            case PlayerMovementEnum.Down:
                return currentSpawner.getCurrentNextSpawner() != null;
            case PlayerMovementEnum.Up:
                return currentSpawner.getCurrentPrevSpawner() != null;
            default:
                return false;
        }
    }

    private static TextObject getTextObject(PlayerMovementEnum movment) {
        int currentColorCode = currentSpawner.getColorCode();
        switch (movment) { 
            case PlayerMovementEnum.Left:                
                currentSpawner = currentSpawner.getCurrentPrevSpawner();
                bool isValidMove = MaskMovement.detectValidPlayerMovement(currentSpawner.getCurrentObject())
                    && PassageManager.checkValidPassage(currentSpawner.getCurrentObject());
                if (!isValidMove) currentSpawner = currentSpawner.getCurrentNextSpawner();
                else if(currentColorCode!=MaskManager.STATIC_COLOR_CODE 
                    && currentSpawner.getColorCode()==MaskManager.STATIC_COLOR_CODE)MaskManager.setToColor(-1);
                return currentSpawner.getCurrentObject();
            case PlayerMovementEnum.Right:
                currentSpawner = currentSpawner.getCurrentNextSpawner();
                isValidMove = MaskMovement.detectValidPlayerMovement(currentSpawner.getCurrentObject())
                    && PassageManager.checkValidPassage(currentSpawner.getCurrentObject());
                if (!isValidMove) currentSpawner = currentSpawner.getCurrentPrevSpawner();
                else if(currentColorCode!=MaskManager.STATIC_COLOR_CODE 
                    && currentSpawner.getColorCode()==MaskManager.STATIC_COLOR_CODE)MaskManager.setToColor(-1);
                return currentSpawner.getCurrentObject();
            case PlayerMovementEnum.Up:
                currentSpawner = currentSpawner.getCurrentPrevSpawner();
                isValidMove = MaskMovement.detectValidPlayerMovement(currentSpawner.getCurrentObject())
                    && PassageManager.checkValidPassage(currentSpawner.getCurrentObject());
                if (!isValidMove) {
                    currentSpawner = currentSpawner.getCurrentNextSpawner();
                    return currentTextObject;
                }
                else if(currentColorCode!=MaskManager.STATIC_COLOR_CODE 
                    && currentSpawner.getColorCode()==MaskManager.STATIC_COLOR_CODE)MaskManager.setToColor(-1);
                TextObject textObj = currentSpawner.getClosestObject(currentTextObject, false);
                if(currentTextObject.Equals(textObj)) currentSpawner = currentSpawner.getCurrentNextSpawner();
                return textObj;
            case PlayerMovementEnum.Down:
                currentSpawner = currentSpawner.getCurrentNextSpawner();
                TextObject closestTextObj = currentSpawner.getClosestObject(currentTextObject, false);
                isValidMove = MaskMovement.detectValidPlayerMovement(closestTextObj)
                    && PassageManager.checkValidPassage(closestTextObj);
                if (!isValidMove) {
                    currentSpawner = currentSpawner.getCurrentPrevSpawner();
                    return currentTextObject;
                }
                else if(currentColorCode!=MaskManager.STATIC_COLOR_CODE 
                    && currentSpawner.getColorCode()==MaskManager.STATIC_COLOR_CODE)MaskManager.setToColor(-1);
                if(currentTextObject.Equals(closestTextObj)) currentSpawner = currentSpawner.getCurrentPrevSpawner();
                return closestTextObj;            
            default:
                return null;
        }
    }
}
