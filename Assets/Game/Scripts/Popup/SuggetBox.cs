using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SuggetBox : BaseBox
{
    #region instance
    private static SuggetBox instance;
    public static SuggetBox Setup(GiftType giftType, bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<SuggetBox>(PathPrefabs.SUGGET_BOX));
            instance.Init();
        }

        instance.InitState(giftType);
        return instance;
    }
    #endregion

    #region  Var 
    [SerializeField] private GiftType giftType;
    [SerializeField] private  Button btnClose;
    [SerializeField] private Button btnWatch;
    [SerializeField] private Button btnCoin;
    [SerializeField] private  Image imgGift;
    [SerializeField] private  Text txtGift;
    ActionWatchVideo actionWatchVideo;



    #endregion


    private void Init()
    {
        btnClose.onClick.AddListener(delegate {  Close(); });
        btnWatch.onClick.AddListener(delegate {  OnButtonWatchClick(); });
        btnCoin.onClick.AddListener(delegate {  OnButtonCoinClick(); });
        
    }
    private void InitState(GiftType paramGiftType)
    {
           switch (paramGiftType)
            {
                case GiftType.DrillBooster:
                    giftType = paramGiftType;
                    imgGift.sprite = GameController.Instance.dataContain.giftDatabase.GetIconItem(GiftType.DrillBooster);
                    txtGift.text = "x1";
                actionWatchVideo = ActionWatchVideo.DrillBooster;
                break;
                case GiftType.DestroyScewBooster:
                    giftType = paramGiftType;
                    imgGift.sprite = GameController.Instance.dataContain.giftDatabase.GetIconItem(GiftType.DestroyScewBooster);
                    txtGift.text = "x1";
                actionWatchVideo = ActionWatchVideo.DestroyScewBooster;
                break;
            }

        imgGift.SetNativeSize();
        GameController.Instance.admobAds.HandleShowMerec();
    }


    private void OnButtonWatchClick()
    {
        GameController.Instance.admobAds.ShowVideoReward(delegate { HandleTakeGift(); } , delegate { GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp_UI
        (   
            btnWatch.transform,
            btnWatch.transform.position,
            "No video at the moment!",
            Color.white,
            isSpawnItemPlayer: true
        ); } , delegate { }, actionWatchVideo , UseProfile.CurrentLevel.ToString());

       // HandleTakeGift();


    }
    private void OnButtonCoinClick()
    {
        if(UseProfile.Coin >= 600)
        {
            UseProfile.Coin -= 600;
            HandleTakeGift();

        }
        else
        {
            GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp_UI
           (
                   btnCoin.transform,
               btnCoin.transform.position,
               "Not enought coin",
               Color.red,
               isSpawnItemPlayer: true
           );
           
    }
    }
    
    
    private  void HandleTakeGift()
    {
        List<GiftRewardShow> lstReward = new List<GiftRewardShow>();
        GiftRewardShow rw = new GiftRewardShow();
      
      
       
        switch (giftType)    
           {
               case GiftType.DrillBooster:
                GameController.Instance.dataContain.giftDatabase.Claim(GiftType.DrillBooster, 1);
                rw.type = GiftType.DrillBooster;
                break;
               case GiftType.DestroyScewBooster:
                GameController.Instance.dataContain.giftDatabase.Claim(GiftType.DestroyScewBooster, 1);
                rw.type = GiftType.DestroyScewBooster;
                break;
           }
        rw.amount = 1;
        lstReward.Add(rw);
        RewardIAPBox.Setup().Show(lstReward, actionClaim: () => { });

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameController.Instance.admobAds.HandleHideMerec();
    }
}
