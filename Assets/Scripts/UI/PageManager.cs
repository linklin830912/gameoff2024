using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject infoPanelButton;
    [SerializeField]
    private GameObject frontpage;
    [SerializeField]
    private GameObject endpage;
    internal static PageManager pageManagerInstance;
    void Awake() {
        if (pageManagerInstance == null) pageManagerInstance = this;
    }
    void Start()
    {
        infoPanelButton.SetActive(false);
        frontpage.SetActive(true);
        endpage.SetActive(false);
    }

    public void onStartButtonClicked() { 
        frontpage.SetActive(false);
    }
    public void onInfoButtonClicked() {         
        infoPanelButton.SetActive(!infoPanelButton.active);
    }
    public void onInfoPanelButtonClicked() {         
        infoPanelButton.SetActive(false);
    }
    public static void onGameComplete() {
        pageManagerInstance.endpage.SetActive(true);
    }
}
