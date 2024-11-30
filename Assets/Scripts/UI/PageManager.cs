using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject infoPanelButton;
    [SerializeField]
    private GameObject frontpage;
    // Start is called before the first frame update
    void Start()
    {
        infoPanelButton.SetActive(false);
        frontpage.SetActive(true);
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
}
