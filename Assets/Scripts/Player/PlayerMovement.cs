using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        Debug.Log(spawner.transform.localScale);
        playerMovementInstance.gameObject.transform.localScale = spawner.transform.localScale;
    }
    private bool isHold = false;
    private int count = 0;
    private void playerMovementUpdate() { 
        float dir = playerControl.ReadValue<float>();
        if (dir > 0)
        {            
            if(!isHold || (isHold && count >50 && count%5==0 ))
                ParagraphsManager.nextObject();
                isHold = true;
            count ++;
        }
        else if (dir < 0)
        {            
            if(!isHold || (isHold && count >50 && count%5==0))
                ParagraphsManager.prevObject();
                isHold = true;
            count ++;
        }
        else { 
            isHold = false;
            count = 0;
        }     
    }

}
