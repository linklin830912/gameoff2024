using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum PlayerMovementEnum { 
    Left, Right, Up, Down
}
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement playerMovementInstance;
    public InputAction playerControl;
    void Awake() {
        if (playerMovementInstance == null) playerMovementInstance = this;
    }
    void OnEnable() {
        playerControl.Enable();
    }
    void OnDisable() {
        playerControl.Disable();
    }
    
    void Update() {
        playerMovementUpdate();
    }
    internal static void setPosition(Vector3 position) { 
        playerMovementInstance.gameObject.transform.position = position;
    }
    internal static void setScale(GameObject spawner) {
        playerMovementInstance.gameObject.transform.localScale = spawner.transform.localScale;
    }
    private bool isHold = false;
    private int count = 0;
    private void playerMovementUpdate() { 
        Vector2 dir = playerControl.ReadValue<Vector2>();
        if (dir.x > 0)
        {
            if (!isHold || (isHold && count > 50 && count % 5 == 0))
                ParagraphsManager.moveToObject(PlayerMovementEnum.Right);
                isHold = true;
            count ++;
        }
        else if (dir.x < 0)
        {            
            if(!isHold || (isHold && count >50 && count%5==0))
                ParagraphsManager.moveToObject(PlayerMovementEnum.Left);
                isHold = true;
            count ++;
        }else if (dir.y < 0)
        {            
            if(!isHold || (isHold && count >50 && count%5==0 ))
                ParagraphsManager.moveToObject(PlayerMovementEnum.Down);
                isHold = true;
            count ++;
        }
        else if (dir.y > 0)
        {            
            if(!isHold || (isHold && count >50 && count%5==0))
                ParagraphsManager.moveToObject(PlayerMovementEnum.Up);
                isHold = true;
            count ++;
        }
        else { 
            isHold = false;
            count = 0;
        }     
    }

}
