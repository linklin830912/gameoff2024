using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class MaskManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] masks;
    private static GameObject[] allMasks;
    internal static int STATIC_COLOR_CODE = -1;
    private static int LEVEL_STATIC = 0;
    private static int LEVEL_DISPLAY = 0;
    private static int LEVEL_HIDDEN = 10;
    internal static MaskManager maskManagerInstance;
    internal static int currentMaskIndex = -1;
    internal static Dictionary<int, List<ParagraphSpawner>> colorParagraphList;
    void Awake() {
        if (maskManagerInstance == null){
             maskManagerInstance = this;
             colorParagraphList = new Dictionary<int, List<ParagraphSpawner>>();
            allMasks = masks;
        }
    }
    void Start() { 
        currentMaskIndex = -1;
        MaskMovement.setMask(null);
        for (int i=0;i<allMasks.Length;i++) {
            allMasks[i].SetActive(i==currentMaskIndex);
        }

    }
    internal static void AddToColorParagraphList(int colorCode, ParagraphSpawner spawner) {
        if (colorParagraphList.ContainsKey(colorCode))
        {
            colorParagraphList[colorCode].Add(spawner);
        }
        else { 
            List<ParagraphSpawner> list = new List<ParagraphSpawner>();
            list.Add(spawner);
            colorParagraphList[colorCode] = list;
        }
    }
    internal static void SetupDepthForColorCode(int colorCode, ParagraphSpawner spawner) {
        Vector3 pos = spawner.gameObject.transform.position;
            pos.z = colorCode==STATIC_COLOR_CODE ? LEVEL_STATIC : LEVEL_HIDDEN;
            spawner.gameObject.transform.position = pos;
    }
    internal static void AssignAlphabetColor(int colorCode, GameObject alphabetObj) {
        if (colorCode == STATIC_COLOR_CODE){
             alphabetObj.GetComponent<TextMeshPro>().color = Color.black;
            }
    }

    public void OnMaskSelected(int selectIndex) {
        setToColor(selectIndex);
    }
    internal static void setToColor(int selectIndex) {
        if(currentMaskIndex!=-1) allMasks[currentMaskIndex].SetActive(false);        
        if(selectIndex!=-1)allMasks[selectIndex].SetActive(true);
        MaskMovement.setMask(selectIndex==-1 ? null: allMasks[selectIndex]);
        currentMaskIndex = selectIndex;

        foreach (var pair in colorParagraphList) {
            if (pair.Key == selectIndex && pair.Key != -1)
            {
                List<ParagraphSpawner> list = pair.Value;
                foreach (var paragraph in list)
                {
                    Vector3 pos = paragraph.gameObject.transform.position;
                    pos.z = LEVEL_DISPLAY;
                    paragraph.gameObject.transform.position = pos;
                }

            }
            else if (pair.Key != selectIndex && pair.Key != -1)
            {
                List<ParagraphSpawner> list = pair.Value;
                foreach (var paragraph in list)
                {
                    Vector3 pos = paragraph.gameObject.transform.position;
                    pos.z = LEVEL_HIDDEN;
                    paragraph.gameObject.transform.position = pos;
                }
            }
            else if (pair.Key == -1)
            {
                List<ParagraphSpawner> list = pair.Value;
                foreach (var paragraph in list)
                {
                    Vector3 pos = paragraph.gameObject.transform.position;
                    pos.z = LEVEL_STATIC;
                    paragraph.gameObject.transform.position = pos;
                }
            }
        }
    }
}
