using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Bolt : MonoBehaviour
{


    public bool Locked;
    private void Start() {
        if(Locked){
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void Select()
    {
        if(GameManager.instance.isUseDestroyScewBooster)
        {
            GameManager.instance.isUseDestroyScewBooster = false;
            GameManager.instance.gameScene.BlockBooster(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            Vector3 DesireRotation = new Vector3(35, 0, 0);
            transform.Rotate(DesireRotation);
            GameManager.instance.gameScene.BlockBooster(false);
        }

        if (UseProfile.CurrentLevel == 1)
        {
            TutFirstGame.Instance.Step_1();
        }

        if (UseProfile.CurrentLevel == 7)
        {
            TutBoosterDestroyScew.Instance.Step_2();
        }
     
    }

    public void Deselect()
    {
      
            transform.rotation = Quaternion.identity;
        
     
        if (UseProfile.CurrentLevel == 1)
        {
            TutFirstGame.Instance.Step_2();
        }
        GameManager.instance.gameScene.BlockBooster(true);
    }

    public void Unlock(){
        Locked=false;
         transform.GetChild(1).gameObject.SetActive(false);
    }

    public void vibration()
    {
       

    }
}
