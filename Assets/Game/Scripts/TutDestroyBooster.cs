using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutDestroyBooster : MonoBehaviour
{
    public static TutDestroyBooster Instance;
    public GameObject panel_Tut;
    public GameObject hand_1;
    public GameObject booster_Destroy;
    public bool isTut = false;
    public GameObject hand_2;

    private void Start()
    {
        Instance = this;
        Init();


    }

    public void Init()
    {
        if (UseProfile.CurrentLevel == 7)
        {
            isTut = true;
            panel_Tut.gameObject.SetActive(true);
            hand_1.gameObject.SetActive(true);
            booster_Destroy.transform.parent = panel_Tut.gameObject.transform;
            booster_Destroy.transform.SetAsLastSibling();
        }
    }

    public void MoveTutDestroy()
    {
        if (isTut)
        {
            panel_Tut.gameObject.SetActive(false);
            hand_1.gameObject.SetActive(false);
            booster_Destroy.transform.parent = this.transform;
            hand_2.gameObject.SetActive(true);
        }
    }
    public void OffTutDestroy()
    {
        if (isTut)
        {
            isTut = false;
            hand_2.gameObject.SetActive(false);
        }
    }
}
