using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [HideInInspector] public bool screwed = true;
    private void Awake()
    {
        DisableCollisionWithOtherBars();
    }

    void DisableCollisionWithOtherBars()
    {


        int OwnerLayer = gameObject.layer;

        for (var i = 9; i <= 15; i++)// bar layers number
        {
            if (i != OwnerLayer)
            {
                Physics2D.IgnoreLayerCollision(OwnerLayer, i);
            }
        }

    }

   


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            screwed = false;
        }
        else if (other.CompareTag("Key"))
        {
            GameManager.instance.PlayClip(GameManager.instance.KeyCollectedSound);
            print("bolt unlocked");
            other.gameObject.SetActive(false);
            Bolt[] Lockedbolts = FindObjectsOfType<Bolt>().Where(b => b.Locked == true).ToArray();
            BoardHole[] LockedHoles = FindObjectsOfType<BoardHole>().Where(b => b.Locked == true).ToArray();
            if (Lockedbolts.Length >= 1)
            {
                for (var i = 0; i < Lockedbolts.Length; i++)
                {
                    Lockedbolts[i].Unlock();
                }
            }
            if (LockedHoles.Length >= 1)
            {
                for (var i = 0; i < LockedHoles.Length; i++)
                {
                    LockedHoles[i].Unlock();
                }
            }
        }
    }
}
