using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutFirstGame : MonoBehaviour
{
    public static TutFirstGame Instance;
    public GameObject hand_1;
    public GameObject hand_2;

    void Start()
    {
        Instance = this;
        CheckIsReady();
    }

    public void CheckIsReady()
    {
        if(UseProfile.CurrentLevel == 1)
        {
            hand_1.SetActive(true);
            GameController.Instance.AnalyticsController.StartTut_1();
        }
    }

    public void Step_1()
    {
        hand_1.SetActive(false);
        hand_2.SetActive(true);
    }
    public void Step_2()
    {
        hand_2.SetActive(false);
        GameController.Instance.AnalyticsController.EndTut_1();
    }

}
