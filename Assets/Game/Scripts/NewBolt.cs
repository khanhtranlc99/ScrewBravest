using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBolt : MonoBehaviour
{
    public GameObject connectedBody;
    public List<GameObject> connectedBodylist;
    public List<GameObject> previousbodies = new List<GameObject>();

    public GameManager_Scew_Old gamemanger;
    void Start()
    {
        gamemanger=GameManager_Scew_Old.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CUBE"))
        {
            if (gamemanger.gamestate == GameManager_Scew_Old.State.Idle)
            {
                if (!connectedBodylist.Contains(other.gameObject))
                {
                    connectedBodylist.Add(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CUBE"))
        {
            if (connectedBodylist.Contains(other.gameObject))
            {
                connectedBodylist.Remove(other.gameObject);
            }
        }
    }
}
