using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutHand : MonoBehaviour
{
    public Vector3 firstTranform;
   [SerializeField]  public Transform target;

    public void Start()
    {
        firstTranform = this.transform.position;
        Move();
    }

    public void Move()
    {
        this.transform.DOMove(target.transform.position, 0.5f).OnComplete(delegate {
            this.transform.DOMove(firstTranform, 0.5f).OnComplete(delegate { Move(); });
        });
    }

}
