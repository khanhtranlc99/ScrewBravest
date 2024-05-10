using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PackInShop : MonoBehaviour
{
    public TypePackIAP typePackIAP;
    public Button btnBuy;
    public Text tvBuy;
    public void Init()
    {
        //tvBuy.text = "" + ;
    
     
        btnBuy.onClick.AddListener(delegate { ButtonOnClick(); });
    
    }

    public void ButtonOnClick()
    {
        
    }
      
}
