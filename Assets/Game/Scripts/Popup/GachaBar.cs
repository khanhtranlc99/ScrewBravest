using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GachaBar : MonoBehaviour
{
    [SerializeField] private Transform[] lsTranformGacha;
    [SerializeField] private Transform arrow;
    private bool isStop;
    private Tween tween;
    public Text tvReward;


    public int ValueReward
    {
        get
        {
            if(arrow.transform.position.x >= lsTranformGacha[0].position.x && arrow.transform.position.x < lsTranformGacha[1].position.x)
            {
              
                return 2;
            }
            if (arrow.transform.position.x >= lsTranformGacha[1].position.x && arrow.transform.position.x < lsTranformGacha[2].position.x)
            {
             
                return 4;
            }
            if (arrow.transform.position.x >= lsTranformGacha[2].position.x && arrow.transform.position.x < lsTranformGacha[3].position.x)
            {
               
                return 5;
            }
            if (arrow.transform.position.x >= lsTranformGacha[3].position.x && arrow.transform.position.x < lsTranformGacha[4].position.x)
            {
           
                return 3;
            }
            if (arrow.transform.position.x >= lsTranformGacha[4].position.x && arrow.transform.position.x < lsTranformGacha[5].position.x)
            {
          
                return 4;
            }
            if (arrow.transform.position.x >= lsTranformGacha[5].position.x && arrow.transform.position.x < lsTranformGacha[6].position.x)
            {
            
                return 3;
            }
            if (arrow.transform.position.x >= lsTranformGacha[6].position.x && arrow.transform.position.x < lsTranformGacha[7].position.x)
            {
          
                return 2;
            }

            return 2;
        }


    }

    //private void Update()
    //{
    //    aura.transform.localEulerAngles += new Vector3(0, 0, 10) * Time.deltaTime;
    //}


    public void Init()
    {

    }
    public void InitState()
    {
        StartCoroutine(StartGacha());
    }

    private IEnumerator StartGacha()
    {
        yield return new WaitForSeconds(0.5f);
        tween.Kill();
        arrow.DOKill();
        isStop = false;
        MoveArrow();
    }


    private void MoveArrow()
    {
        tween =  arrow.DOMoveX(lsTranformGacha[0].position.x, 1).OnComplete(delegate {
         arrow.DOMoveX(lsTranformGacha[lsTranformGacha.Length - 1].position.x, 1).OnComplete(delegate {
             if (!isStop)
             {
                 MoveArrow();
             }
         }).OnUpdate(delegate {
             tvReward.text = "x" + ValueReward;
         });
        }).OnUpdate(delegate {
            tvReward.text = "x" + ValueReward;
        });

  
    }

    public void HandleOnClickStop()
    {
        isStop = true;
        tween.Kill();
        arrow.DOKill();
    }
}
