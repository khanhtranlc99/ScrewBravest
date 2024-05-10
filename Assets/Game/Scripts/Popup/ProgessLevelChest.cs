using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgessLevelChest : ProgessIconText
{
    public levelChest tempLsLevelChest;
  
    public override void Init(WinBox param)
    {
        base.Init(param);
        int tempLevel = 0;
        int tempSubtraction = 0;
        tempLsLevelChest = new levelChest();
        var tempLevelChestOld = new levelChest();
        var tempContro = GameController.Instance.dataContain.levelChestData.lsLevelChest;
        var tempCurrent = GameController.Instance.dataContain.levelChestData.CurrentLevelChest;
        
        if (UseProfile.LevelOfLevelChest == 0)
        {
            ShowProgess(UseProfile.CurrentLevel, tempContro[0].level);
            tempLsLevelChest = tempContro[0];
            return;
        }
        else
        {
           
            for (int i = 0; i < tempContro.Count; i++)
            {
                if(tempContro[i] == tempCurrent)
                {
                
                    tempLevelChestOld = tempContro[i - 1];
                    tempLsLevelChest = tempCurrent;
                    break;
                }
            } 
        }
        UseProfile.CurrentLevelOfLevelChest += 1;
       // Debug.LogError("ProgessLevelChestInit " + UseProfile.CurrentLevelOfLevelChest);
        tempSubtraction = (tempCurrent.level - tempLevelChestOld.level) ;
        ShowProgess(UseProfile.CurrentLevelOfLevelChest, tempSubtraction);
    }

    private void ShowNextGift()
    {
        //UseProfile.LevelOfLevelChest += 1;
        GameController.Instance.dataContain.levelChestData.PlusLevelOfLevelChest();
        UseProfile.CurrentLevelOfLevelChest = 0;
        int tempSubtraction = 0;
        var tempLevelChestOld = new levelChest();
        var tempCurrent = GameController.Instance.dataContain.levelChestData.CurrentLevelChest;
        var tempContro = GameController.Instance.dataContain.levelChestData.lsLevelChest;
        if (tempCurrent == null)
        {
           
         //   this.gameObject.SetActive(false);
           
            return;
        }
        for (int i = 0; i < tempContro.Count; i++)
        {
            if (tempContro[i] == tempCurrent)
            {
                tempLevelChestOld = tempContro[i - 1];
                tempLsLevelChest = tempCurrent;
                break;
            }
        }
        tempSubtraction = (tempCurrent.level - tempLevelChestOld.level) ;
        HandleProgess(0, 0, tempSubtraction);

    }


    //   public List<GiftOfBox> tempListGiftOfBox;
    public List<GiftRewardShow> giftRewardShows;
    public override void Complete()
    {
  
        giftRewardShows = new List<GiftRewardShow>();
        giftRewardShows.Add(new GiftRewardShow() { type = tempLsLevelChest.giftType, amount = tempLsLevelChest.amount });
     //   tempListGiftOfBox = new List<GiftOfBox>();
       //  tempListGiftOfBox.Add(new GiftOfBox() { giftType = tempLsLevelChest.giftType, amount = tempLsLevelChest.amount });

        ShowNextGift();
        RewardIAPBox.Setup().Show(giftRewardShows);
        foreach(var item in giftRewardShows)
        {
            GameController.Instance.dataContain.giftDatabase.Claim(item.type, item.amount);
        }
        Debug.LogError("Complete");
        // OpenGiftBox.Setup(GiftBoxType.LevelGift, tempListGiftOfBox, false).Show();
    }
}
