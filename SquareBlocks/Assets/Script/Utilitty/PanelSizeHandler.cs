using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSizeHandler : MonoBehaviour
{
    public float canvasScalerWidth = 1080;
    public float panelOffset = 220;
    public float offset = 70;
    public float bottompanelOffset = 0;
    public RectTransform rectPanel;
    void Awake()
    {
        
        if (rectPanel == null)
            rectPanel = GetComponent<RectTransform>();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float calculatedHeight = screenHeight / screenWidth * canvasScalerWidth;
        rectPanel.sizeDelta = new Vector2(canvasScalerWidth, (calculatedHeight - panelOffset - offset - bottompanelOffset));

        // Debug.Log("prefered Height === " + calculatedHeight + " calculated height ");
        //  Debug.Log(gameObject.name+" sizeDelta " + rectPanel.sizeDelta);
    }
}

