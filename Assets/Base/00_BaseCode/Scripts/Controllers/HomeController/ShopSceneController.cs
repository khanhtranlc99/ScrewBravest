using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSceneController : SceneBase
{

    public List<PackInShop> lsPackInShops;
    public override void Init()
    {
      foreach(var item in lsPackInShops)
        {
            item.Init();
        }
    }
}
