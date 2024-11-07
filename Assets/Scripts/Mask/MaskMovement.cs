using UnityEngine;
using UnityEngine.InputSystem;

public class MaskMovement : MonoBehaviour
{
    private static GameObject currentMask;
    private float zDistanceFromCamera = 10f;
    private float staticPositionZ = 10f;
    private static float validRadius = 0;
    // Update is called once per frame
    void Update()
    {
        if (currentMask !=null) {
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 screenPositionWithZ = new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, zDistanceFromCamera);
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(screenPositionWithZ);
            mouseWorldPosition.z = staticPositionZ;
            currentMask.transform.position = mouseWorldPosition;
        }
    }

    internal static void setMask(GameObject mask) {
        currentMask = mask;
        if(mask != null)validRadius = mask.GetComponent<SphereCollider>().radius;
    }
    internal static bool detectValidPlayerMovement(TextObject textObject) {
        if (currentMask == null) return true;
        Vector3 pos0 = textObject.value.transform.position;
        Vector3 pos1 = currentMask.GetComponent<SphereCollider>().center;
        pos0.z = 0;
        pos1.z = 0;
        float dis = Vector3.Distance(pos0, pos1);
        Debug.Log(dis);
        return dis <= validRadius;
    }
}
