using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ProgessIconText : MonoBehaviour
{
    public BroadProgess broad;
    [SerializeField] private Text tvContent;
    [SerializeField] private Image progess;
    [SerializeField] private Image icon;
    public WinBox winBox;
    private Tween tween1;
    public virtual void Init(WinBox param)
    {
        winBox = param;
    }

    public void ShowProgess(float currentValue, float maxValue)
    {
        progess.fillAmount = 0;
        float tempProgess = currentValue / maxValue;
        var temp = (float)System.Math.Round(tempProgess);
        //    tvContent.text = currentValue + "/" + maxValue;
        TweenNumber(0, (int)currentValue, (int)maxValue, 0.5f);
      
        progess.DOFillAmount(tempProgess, 1).OnComplete(delegate {
            if (currentValue == maxValue)
            {
                Complete();
            }
   
            winBox.OnOffAllButton(true);
            


        });
    }
    public void HandleProgess(float progessParam,float currentValue, float maxValue)
    {
        progess.fillAmount = progessParam;
        //  tvContent.text = currentValue + "/" + maxValue;
        TweenNumber(0, (int)currentValue, (int)maxValue, 0.5f);
    }

    public virtual void Complete()
    {
      
    }
    public virtual void NotComplete()
    {
        
      
    }
    private void TweenNumber(int StarValue, int Endvalue, int paramSum, float time)
    {
        var temp = StarValue;
        tween1.Kill();
        tween1 = DOTween.To(() => temp, x => temp = x, Endvalue, time)
            .OnUpdate(delegate { tvContent.text = temp + "/" + paramSum; }).OnComplete(delegate
            {
                temp = (int)Endvalue;
                tvContent.text = temp + "/" + paramSum;
               
            });

        tvContent.transform.localScale = new Vector3(1, 1, 1);
        tvContent.transform.DOKill();
        tvContent.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.25f).OnComplete(delegate { tvContent.transform.DOScale(new Vector3(1, 1, 1), 0.25f); });
    }

}

