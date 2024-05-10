using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroadProgess : MonoBehaviour
{
    public GameObject broadProgess;
    public GameObject progessLevelChest;
    public GameObject smallProgess;
    public Image imageBroad;

    public void SetSmallBroad()
    {
        imageBroad.color = new Color32(0,0,0,0);
        imageBroad.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
        smallProgess.SetActive(true);
        broadProgess.SetActive(false);
       
    }
    public void SetBigBroad()
    {
        imageBroad.color = new Color32(255, 255, 255, 255);
        imageBroad.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        smallProgess.SetActive(false);
        broadProgess.SetActive(true);
    }
}
