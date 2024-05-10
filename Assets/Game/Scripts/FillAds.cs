using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FillAds : MonoBehaviour
{
    public GameObject spVideoAds;
    public void OnClick()
    {
        GameController.Instance.admobAds.ShowVideoReward(delegate { ClaimAds(); }, delegate {
            GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp(
            this.transform.position,
            "No video at the moment!",
            Color.white,
            isSpawnItemPlayer: true
            );
        }, delegate { }, ActionWatchVideo.HoldAds, UseProfile.CurrentLevel.ToString());



        void ClaimAds()
        {
            spVideoAds.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).OnComplete(delegate {
                spVideoAds.transform.DOScale(new Vector3(0, 0, 0), 0.3f);
            });
            this.gameObject.tag = "Fill";

        }





    }


}
