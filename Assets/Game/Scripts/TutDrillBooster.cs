using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutDrillBooster : MonoBehaviour
{
    public static TutDrillBooster Instance;
    public GameObject panel_Tut;
    public GameObject hand_1;
    public GameObject booster_Drill;
    public bool isTut = false;
    public GameObject hand_2;

    private void Start()
    {
        Instance = this;
        Init();


    }

    public void Init()
    {
        if(UseProfile.CurrentLevel == 5)
        {
            isTut = true;
            panel_Tut.gameObject.SetActive(true);
            hand_1.gameObject.SetActive(true);
            booster_Drill.transform.parent = panel_Tut.gameObject.transform;
            booster_Drill.transform.SetAsLastSibling();
            GameController.Instance.AnalyticsController.StartTut_2();
        }
    }

    public void MoveTutDrill()
    {
        if(isTut)
        {
            panel_Tut.gameObject.SetActive(false);
            hand_1.gameObject.SetActive(false);
            booster_Drill.transform.parent = this.transform;
            hand_2.gameObject.SetActive(true);
        }
    }
    public void OffTutDrill()
    {
        if (isTut)
        {
            isTut = false;
            hand_2.gameObject.SetActive(false);
            GameController.Instance.AnalyticsController.EndTut_2();
        }
    }








}
