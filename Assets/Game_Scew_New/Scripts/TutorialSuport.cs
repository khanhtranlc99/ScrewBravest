using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSuport : MonoBehaviour
{
    public static TutorialSuport Instance;
    public GameObject handSuport;
    public void Start()
    {
        Instance = this;
    }
}
